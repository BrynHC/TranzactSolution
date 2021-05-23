using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tranzact.Entities;

namespace Tranzact.Service
{
    public class GoogleSearch : ISearch
    {
        private readonly GoogleEntity oGoogleEntity = new GoogleEntity();

        public GoogleSearch(string searchWord)
        {
            oGoogleEntity.GoogleUrl = ConfigurationManager.AppSettings["GoogleSearchUrl"];
            oGoogleEntity.GoogleCx = ConfigurationManager.AppSettings["GoogleCx"];
            oGoogleEntity.GoogleKey = ConfigurationManager.AppSettings["GoogleKey"];
            oGoogleEntity.SeachWord = searchWord;
        }

        #region Methods

        public Result GetSearchList()
        {
            Result googleResult = new Result();
            googleResult.ProgrammingLanguage = oGoogleEntity.SeachWord;
            googleResult.BrowserName = "Google";

            try
            {
                string resquestUriString = $"{oGoogleEntity.GoogleUrl}{oGoogleEntity.GoogleKey}&cx={oGoogleEntity.GoogleCx}&q={oGoogleEntity.SeachWord}";

                var request = WebRequest.Create(resquestUriString);
                HttpWebResponse oHttpWebResponse = (HttpWebResponse)request.GetResponse();
                Stream oStream = oHttpWebResponse.GetResponseStream();
                StreamReader oStreamReader = new StreamReader(oStream);
                string readerString = oStreamReader.ReadToEnd();
                dynamic jsonData = JsonConvert.DeserializeObject(readerString);

                foreach (var item in jsonData.queries)
                {
                    foreach (var value in item.Value)
                        googleResult.SearchQuantity = Get<long>(value.totalResults.Value);
                    break;
                }
            }
            catch (Exception ex)
            {
                googleResult.ErrorMessage = ex.Message;
            }

            return googleResult;
        }

        private T Get<T>(dynamic value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        #endregion
    }
}
