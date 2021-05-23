using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tranzact.Entities;

namespace Tranzact.Service
{
    public class MsnSearch : ISearch
    {
        private readonly MsnEntity oMsnEntity = new MsnEntity();

        public MsnSearch(string searchWord)
        {
            oMsnEntity.MsngUrl = ConfigurationManager.AppSettings["MsnSearchUrl"];
            oMsnEntity.MsnCustomConfigId = ConfigurationManager.AppSettings["MsnCustomConfigId"];
            oMsnEntity.MsnKey = ConfigurationManager.AppSettings["MsnKey"];
            oMsnEntity.SearchWord = searchWord;
        }

        public Result GetSearchList()
        {
            Result msnResult = new Result();
            msnResult.ProgrammingLanguage = oMsnEntity.SearchWord;
            msnResult.BrowserName = "MSN Search";

            try
            {
                string requestUriString = $"{oMsnEntity.MsngUrl}{oMsnEntity.SearchWord}&customconfig={oMsnEntity.MsnCustomConfigId}";

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", oMsnEntity.MsnKey);

                var httpResponseMessage = client.GetAsync(requestUriString).Result;
                var responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                dynamic response = JsonConvert.DeserializeObject(responseContent);

                msnResult.SearchQuantity = response.webPages.totalEstimatedMatches.Value;
            }
            catch (Exception ex)
            {
                msnResult.ErrorMessage = ex.Message;
            }

            return msnResult;
        }
    }
}
