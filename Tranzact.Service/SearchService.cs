using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranzact.Entities;

namespace Tranzact.Service
{
    public class SearchService
    {
        public static void Search(string searchWord, ref Result googleResult, ref Result msnResult)
        {
            ISearch iSearch = new GoogleSearch(searchWord);
            googleResult = iSearch.GetSearchList();

            iSearch = new MsnSearch(searchWord);
            msnResult = iSearch.GetSearchList();
        }

        public static string Search(List<ISearch> searchList)
        {
            List<string> resultStringList = new List<string>();
            string message = string.Empty;

            List<Result> resultSearchList = new List<Result>();
            Result resultSearch;

            searchList.ForEach(m => 
            {
                resultSearch = m.GetSearchList();
                resultSearchList.Add(resultSearch);
            });

            var programLanguageList = searchList.Select(m => m).ToList();

            var list = resultSearchList.GroupBy(m => new { m.BrowserName, m.ProgrammingLanguage })
                                        .Select(m => new Result()
                                        {
                                            BrowserName = m.First().BrowserName,
                                            ErrorMessage = m.First().ErrorMessage,
                                            ProgrammingLanguage = m.First().ProgrammingLanguage,
                                            SearchQuantity = m.First().SearchQuantity
                                        }).ToList();

            list.ForEach(m =>
            {
                message += $"{m.ProgrammingLanguage.Trim()}->\t{m.BrowserName}: {m.SearchQuantity}\n";
            });
            
            return message;
        }
    }
}
