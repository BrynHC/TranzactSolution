using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranzact.Entities;
using Tranzact.Service;

namespace Sol_TranzactSearchFight
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Result> googleResultList = new List<Result>();
            Result googleResult = new Result();

            List<Result> bingResultList = new List<Result>();
            Result bingResult = new Result();

            TypeText:
            Console.WriteLine("Type programming languages and press enter:");

            ///GET DATA FROM INPUT
            string data = Console.ReadLine();

            if (!ValidateInputText(data))
                goto TypeText;

            //if (data.Contains("\""))
            //{

            //}

            ///SEPARATING DATA FROM INPUT BY SPACE
            var searchList = data.Contains("\"") ? data.Split('\"').ToList() : data.Split(' ').ToList();
            searchList.Remove(string.Empty);

            ISearch googleSearch;
            ISearch bingSearch;
            string resultSearch = string.Empty;

            foreach (var item in searchList)
            {
                List<ISearch> iSearchList = new List<ISearch>();

                googleSearch = new GoogleSearch(item);
                bingSearch = new MsnSearch(item);

                iSearchList.Add(googleSearch);
                iSearchList.Add(bingSearch);

                resultSearch += SearchService.Search(iSearchList);
            }

            Console.WriteLine($"\n{resultSearch}\n");

            string totalWinner = string.Empty;
            string winnerDetail = GetWinnerDetail(resultSearch, ref totalWinner);
            
            Console.WriteLine(winnerDetail);
            
            Console.WriteLine($"Total winner: {totalWinner}");

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private static bool ValidateInputText(string inputText)
        {
            ///VALIDATE INPUTTEXT NOT TO BE EMPTY
            if (inputText.Trim() == string.Empty)
            {
                Console.WriteLine("\nNot white spaces or empty values allowed.\n");
                return false;
            }

            ///VALIDATE INPUTTEXT NOT TO HAVE SINGLE QUOTE
            if (inputText.Contains("'"))
            {
                Console.WriteLine("\nNot single quote allowed.\n");
                return false;
            }

            return true;
        }

        private static string GetWinnerDetail(string resultSearch, ref string totalWinner)
        {
            string winner = string.Empty;
            long quantitySearch = default(long);

            ///GET PROGRAM LANGUAGES INPUT
            var searchInputList = resultSearch.Replace("\t", string.Empty).Replace(" ", "").Split('\n').ToList();

            ///REMOVE BLANK
            searchInputList.Remove(string.Empty);
            
            ///GET BROWSERS
            var browserList = searchInputList.Select(m => m.Split('>').LastOrDefault().Split(':').FirstOrDefault()).Distinct().ToList();

            ///GET DISTINCT PROGRAM LANGUAGES
            var programmingLanguageList = searchInputList.Select(m => m.Substring(default(int), m.IndexOf("-"))).Distinct().ToList();

            long quantityValidation = default(long);

            foreach (var browser in browserList)
            {
                ///GET QUANTITY OF EVERY INPUT SEARCHING
                quantitySearch = Math.Max(long.MinValue, Convert.ToInt64(searchInputList.Where(m => m.Contains(browser)).ToList().Select(m => m.Substring(m.IndexOf(':'), m.Length - m.IndexOf(':')).Replace(":", "")).FirstOrDefault()));
                ///CONCAT WINNER OF EVERY BROWSER
                winner += $"{browser} winner: {searchInputList.Where(m => m.Contains(browser)).FirstOrDefault(m => m.Contains(quantitySearch.ToString())).Split('-').First()} - {quantitySearch}\n";
                
                ///VALIDATION TO GET PROGRAMMING LANGUAGE WINNER
                if (quantityValidation != default(long) && quantityValidation > quantitySearch)
                    totalWinner = searchInputList.FirstOrDefault(m => m.Contains(quantityValidation.ToString())).Split('-').FirstOrDefault();

                quantityValidation = quantitySearch;
            }

            return winner;
        }
    }
}
