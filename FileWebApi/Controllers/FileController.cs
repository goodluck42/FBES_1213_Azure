using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.AspNetCore.Mvc;

using System.ComponentModel;

namespace FileWebApi.Controllers;

[ApiController]
[Route("api/v1/file")]
public class FileController : ControllerBase
{
    private const string c_ContainerName = "images";

    private readonly BlobServiceClient _serviceClient;

    public FileController(BlobServiceClient serviceClient)
    {
        _serviceClient = serviceClient;
    }

    [HttpGet("{blobName}")]
    public async Task<IActionResult> DownloadFile(string blobName) // fileName
    {
        var container = _serviceClient.GetBlobContainerClient(c_ContainerName);
        var blobClient = container.GetBlobClient(blobName);
        var memoryStream = new MemoryStream();
        var response = await blobClient.DownloadToAsync(memoryStream);

        memoryStream.Position = 0;

        return File(memoryStream, response.Headers.ContentType!, blobName);
    }

    [HttpPost]
    public async Task UploadFile(string blobName, string localPath)
    {
        var container = _serviceClient.GetBlobContainerClient(c_ContainerName);
        using var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read);

        await container.UploadBlobAsync(blobName, stream);
    }

    [HttpPut]
    public async Task UpdateFile(string blobName, string localPath)
    {
        var container = _serviceClient.GetBlobContainerClient(c_ContainerName);
        var blobClient = container.GetBlobClient(blobName);

        await blobClient.DeleteAsync();
        using var stream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
        await container.UploadBlobAsync(blobName, stream);
    }

    [HttpDelete]
    public async Task DeleteFile(string blobName)
    {
        var container = _serviceClient.GetBlobContainerClient(c_ContainerName);
        var blobClient = container.GetBlobClient(blobName);

        await blobClient.DeleteAsync();
    }
}