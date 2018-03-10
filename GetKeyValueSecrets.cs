
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

using System.Net;
using System.Configuration;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Text;
using System.Threading;
using System.Net.Http;
using System.Configuration;

namespace Company.Function
{
    public static class GetKeyVaultSecrets
    {[FunctionName("GetKeyVaultSecrets")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            // public static async Task<HttpResponseMessage> Run(InputMessage req, TraceWriter log, ExecutionContext context)
            // {6
    log.Info("C# HTTP trigger function processed a request.");

    // SecretRequest secretRequest = req.Content.ReadAsAsync<SecretRequest>().Result;
    
    
    // if(string.IsNullOrEmpty(secretRequest.Secret))
    //     return (ActionResult)new OkObjectResult("Fucked up0");
   
    // log.Info($"GetKeyVaultSecret request received for secret {secretRequest.Secret}");        

    var serviceTokenProvider = new AzureServiceTokenProvider();
    
    var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(serviceTokenProvider.KeyVaultTokenCallback));            

    var secretUri = "https://getfitsavemoneykeyvault.vault.azure.net/Secrets/SuperSecret";
    
    log.Info($"Key Vault URI {secretUri} generated");
    SecretBundle secretValue; 
    try
    {
      secretValue = keyVaultClient.GetSecretAsync(secretUri).Result;
    }
    catch(KeyVaultErrorException kex)
    {
        return (ActionResult)new OkObjectResult("Fucked up0");
    }
    log.Info("Secret Value retrieved from KeyVault.");

    var secretResponse = new SecretResponse {Secret ="Hardkodet", Value = secretValue.Value};

return (ActionResult)new OkObjectResult(new StringContent(JsonConvert.SerializeObject(secretResponse), Encoding.UTF8, "application/json"));
   
}
// public static string SecretUri(string secret)
// {
//    return $"https://getfitsavemoneykeyvault.vault.azure.net//Secrets/{secret}";
// }
}

public class SecretRequest
{
    public string Secret {get;set;}
}

public class SecretResponse
{
    public string Secret {get; set;}
    public string Value {get; set;}
}

}