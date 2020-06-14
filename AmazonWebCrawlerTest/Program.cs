using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Xml;
using HtmlAgilityPack;
using System.Net;

namespace AmazonWebCrawlerTest
{
    class MainClass
    {
        public static void loadingAnimation()
        {
            bool On = true;
            string one = "|";
            string two = "/";
            string three = "-";
            string four = @"\";

            while (On)
            {
                Console.Clear();
                Console.WriteLine(one);
                Thread.Sleep(70);
                Console.Clear();
                Console.WriteLine(two);
                Thread.Sleep(70);
                Console.Clear();
                Console.WriteLine(three);
                Thread.Sleep(70);
                Console.Clear();
                Console.WriteLine(four);
                Thread.Sleep(70);
            }
        }
        public static void Main(string[] args)
        {
            bool On = true;

            while(On) {
                Console.WriteLine("Amazon Price Crawler. I'll find you the best deal!");
                Console.WriteLine("Made by: Alex Wzorek. \n");
                Console.WriteLine("Enter the item you're looking for: ");
                string item = returnConvertedString(Console.ReadLine());
                Console.WriteLine("Enter the main key word for the item: ");
                string keyword = Console.ReadLine().ToLower();


                try {

                    ThreadStart loading = new ThreadStart(loadingAnimation);
                    Thread childThread = new Thread(loading);
                    childThread.Start();
 
                    HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.amazon.co.uk/s?k=" + item + "&ref=nb_sb_noss_2");
                    //Console.WriteLine("https://www.amazon.co.uk/s?k=" + item + "&ref=nb_sb_noss_2");

                    bool internetCheck = CheckForInternetConnection();
                    Console.WriteLine(Convert.ToString(internetCheck));

                    String[,] getPages = getMorePages(web, keyword, item);

                    childThread.Abort();
                    Console.Clear();

                    for (int i = 0; i < getPages.GetLength(0); i++) {
                        string tempString = "";
                        for (int j = 0; j < 2; j++) {
                            if (j == 1) {
                                tempString = tempString + getPages[i, j];
                            } else {
                                
                                tempString = tempString + getPages[i, j];
                            }
                        }
                        Console.WriteLine(tempString);
                    }
                        
                    /*


                    var ListingTitles = doc.DocumentNode
                                           .SelectNodes("//span[@class='a-size-base-plus a-color-base a-text-normal']");
                    var ListingPrices = doc.DocumentNode
                                           .SelectNodes("//span[@class='a-offscreen']");
                    if(ListingTitles != null) {
                        ListingTitles.ToList();
                    } else {
                        Console.WriteLine("Please try and be more specific!");
                        break;
                    }
                    if(ListingPrices != null) {
                        ListingPrices.ToList();
                    } else {
                        break;
                    }*/

                } catch(Exception e) {Console.WriteLine("Error: " + e);}

            }
        }

        public static string returnConvertedString(string item) {
            string returnString = "";
            foreach (char letter in item)
            {
                if (Convert.ToString(letter) == " ")
                {
                    returnString = returnString + "+";
                }
                else
                {
                    returnString = returnString + Convert.ToString(letter);
                }
            }
            return (returnString);
        }
        public static String[,] getMorePages(HtmlAgilityPack.HtmlWeb web, string keyword, string item) {

            List<String> titles = new List<string>();
            List<String> prices = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.amazon.co.uk/s?k=" + item + "&page=" + Convert.ToString(i));

                var ListingTitles = doc.DocumentNode
                                       .SelectNodes("//span[@class='a-size-base-plus a-color-base a-text-normal']");
                var ListingPrices = doc.DocumentNode
                                       .SelectNodes("//span[@class='a-offscreen']");
                if(ListingTitles != null) {
                    ListingTitles.ToList();
                } else { continue; }
                if (ListingPrices != null){
                    ListingPrices.ToList();
                }else { continue; }


                int count = 0;
                string tempTitleHold = "";
                foreach(var items in ListingTitles) {
                    tempTitleHold = items.InnerText;
                    if (tempTitleHold.ToLower().Contains(keyword) == true) {
                        titles.Add(tempTitleHold);
                        prices.Add(ListingPrices[count].InnerText);
                    } else {
                        continue;
                    }
                    count = count + 1;
                }
            }

            Console.WriteLine(Convert.ToString(titles.Count()));
            Console.WriteLine(Convert.ToString(prices.Count()));

            String[,] finalPriceAndTitle = new string[titles.Count(), prices.Count()];

            for (int i = 0; i < titles.Count(); i++) {
                for (int j = 0; j < 2; j++) {
                    if (j == 1) {
                        finalPriceAndTitle[i, j] = titles[i];
                    } else {
                        finalPriceAndTitle[i, j] = prices[i] + ", ";
                    } 
                 } 
            }

            return finalPriceAndTitle;
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
