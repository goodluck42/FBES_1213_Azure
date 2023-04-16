using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace CalcFunctionApp
{
    public static class CalcFunction
    {
        private const string c_QueryParamName = "expr";

        [FunctionName("CalcFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            if (req.Query.TryGetValue(c_QueryParamName, out var result))
            {
                var expr = result.FirstOrDefault();
                string[]? split = null;
                double finalResult = 0;

                if (expr.Contains('+'))
                {
                    split = expr.Split('+');

                    finalResult = double.Parse(split[0]) + double.Parse(split[1]);
                }

                if (expr.Contains("-"))
                {
                    split = expr.Split("-");

                    finalResult = double.Parse(split[0]) - double.Parse(split[1]);
                }

                if (expr.StartsWith("/"))
                {
                    split = expr.Split("/");

                    finalResult = double.Parse(split[0]) / double.Parse(split[1]);
                }

                if (expr.StartsWith("*"))
                {
                    split = expr.Split("*");
                    finalResult = double.Parse(split[0]) * double.Parse(split[1]);
                }

                return new JsonResult(new
                {
                    result = finalResult
                });
            }

            return new BadRequestResult();
        }
    }
}
