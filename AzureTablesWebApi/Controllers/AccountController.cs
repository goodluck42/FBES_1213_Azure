using Azure;
using Azure.Data.Tables;

using AzureTablesWebApi.Entities;
using AzureTablesWebApi.Models;

using Microsoft.AspNetCore.Mvc;

using System;

namespace AzureTablesWebApi.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    private const string c_TableName = "Accounts";
    private readonly TableServiceClient _tableServiceClient;
    private readonly TableClient _table;

    public AccountController(TableServiceClient tableServiceClient)
    {
        _tableServiceClient = tableServiceClient;
        _tableServiceClient.CreateTableIfNotExists(c_TableName);
        _table = _tableServiceClient.GetTableClient(c_TableName);
    }

    [HttpPost]
    public async Task Add(Account account)
    {
        var entity = new AccountEntity
        {
            Country = account.Country,
            Login = account.Login,
            Password = account.Password,
            Phone = account.Phone
        };

        // Thru transaction
        var transactionAction = new TableTransactionAction(TableTransactionActionType.Add, entity);

        _ = await _table.SubmitTransactionAsync(new[] { transactionAction });

        // Simple add
        // _ = await _table.AddEntityAsync(entity);
    }

    [HttpGet("all")]
    public async Task<List<Account>> GetAll()
    {
        var response = _table.QueryAsync<AccountEntity>();
        var result = new List<Account>();

        await foreach (var accountEntity in response)
        {
            result.Add(new()
            {
                Country = accountEntity.Country,
                Login = accountEntity.Login,
                Password = accountEntity.Password,
                Phone = accountEntity.Phone
            });
        }

        return result;
    }

    [HttpGet("{guid}")]
    public async Task<Account?> Get(string guid)
    {
        var response = _table.QueryAsync<AccountEntity>(a => a.RowKey == guid);

        await foreach (var item in response)
        {
            return new Account
            {
                Country = item.Country,
                Login = item.Login,
                Password = item.Password,
                Phone = item.Phone
            };
        }

        return null;
    }

    [HttpDelete]
    public async Task Delete(string guid)
    {
        var response = _table.QueryAsync<AccountEntity>(a => a.RowKey == guid);

        AccountEntity? accountEntity = null;

        await foreach (var item in response)
        {
            accountEntity = item;
            break;
        }

        if (accountEntity == null)
        {
            return;
        }

        // Thru transaction
        // var transactionAction = new TableTransactionAction(TableTransactionActionType.Delete, accountEntity);

        //await _table.SubmitTransactionAsync(new[] { transactionAction });

        // Simple delete
        await _table.DeleteEntityAsync(accountEntity.PartitionKey, guid);
    }

    [HttpPut]
    public async Task Update(Account account)
    {
        var response = _table.QueryAsync<AccountEntity>(a => a.RowKey == account.RowKey);
        AccountEntity? entity = null;

        await foreach (var item in response)
        {
            entity = item;
            break;
        }

        if (entity == null)
        {
            return;
        }

        entity.Login = account.Login ?? entity.Login;
        entity.Password = account.Password ?? entity.Password;
        entity.Phone = account.Phone ?? entity.Phone;

        var transactionAction = new TableTransactionAction(TableTransactionActionType.UpdateReplace, entity);

        await _table.SubmitTransactionAsync(new[] { transactionAction });

        //await _table.UpdateEntityAsync(entity, entity.ETag, TableUpdateMode.Replace);
    }

    [HttpPut("upsert")]
    public async Task Upsert(Account account)
    {
        NullableResponse<AccountEntity>? response = null;
        if (account.RowKey != null && account.PartitionKey != null)
        {
            response = await _table.GetEntityIfExistsAsync<AccountEntity>(account.PartitionKey, account.RowKey);
        }

        AccountEntity entity = response != null && response.HasValue ? response.Value : new();

        entity.Country = account.Country ?? entity.Country;
        entity.Login = account.Login ?? entity.Login;
        entity.Password = account.Password ?? entity.Password;
        entity.Phone = account.Phone ?? entity.Phone;

        await _table.UpsertEntityAsync(entity, TableUpdateMode.Replace);
    }
}