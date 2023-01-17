using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {

        public Models.DataModel Model { get; set; }
        public ApplicationDbContext _ApplicationDbContextContext { get; set; }

        private readonly IConfiguration _config;

        public IndexModel(IConfiguration config, ApplicationDbContext ApplicationDbContextContext)
        {
           _config = config;
           _ApplicationDbContextContext = ApplicationDbContextContext;
        }

        public void OnPost(Models.DataModel Model) 
        
        {
            var path = _config.GetValue<string>("apiuri");
            var username = _config.GetValue<string>("AuthenticationStrings:username");
            var apikey = _config.GetValue<string>("AuthenticationStrings:APIKey");
            WebRequest requestObj = WebRequest.Create(path);
            requestObj.Method = "POST";
            requestObj.ContentType = "application/json";
            requestObj.Credentials = new NetworkCredential(username, apikey);

            var modelData = JsonConvert.SerializeObject(Model);

            using (var streamWriter = new StreamWriter(requestObj.GetRequestStream()))
            {
                streamWriter.Write(modelData);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)requestObj.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var results = JsonConvert.DeserializeObject<DbModel> (result);
                    results.ID = results.ticketID;
                    results.status = "created";
                    _ApplicationDbContextContext.dbModels.Add(results);
                    _ApplicationDbContextContext.SaveChanges();
                }
            }

            
            
        }
    }
}