using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public static class Service
    {
        public static async Task<List<DataItem>> GetSections(string url)
        {
            // Download the actual data and count it.
            using (var client = new HttpClient())
            {
                var data = await client.GetAsync(url).ConfigureAwait(false);
                var string_data = await data.Content.ReadAsStringAsync().ConfigureAwait(false);
                var doc = new HtmlDocument();
                doc.LoadHtml(string_data);
                var list = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'list-4')]//li/a[position()=2]").ToList();
                var result_list = new List<DataItem>();
                 result_list.Add(new DataItem("novinky".ToUpper(), "http://185.33.145.103:8000/api/news/", "JSON"));
                result_list.Add(new DataItem("udalosti".ToUpper(), "http://185.33.145.103:8000/api/events/", "JSON"));
                foreach (var item in list)
                {
                    result_list.Add(new DataItem(item.InnerText, "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/" + item.Attributes["href"].Value, "HTML"));
                }
                result_list.Add(new DataItem("Pre záujemcov o informatiku".ToUpper(), "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/?q=node/97/", "HTML"));               
                return result_list;
            }
        }

        public static async Task<string> GetSection(string url)
        {
            // Download the actual data and count it.
            using (var client = new HttpClient())
            {
                var data = await client.GetAsync(url).ConfigureAwait(false);
                var string_data = await data.Content.ReadAsStringAsync().ConfigureAwait(false);
                var doc = new HtmlDocument();
                doc.LoadHtml(string_data);

                List<string> xpaths = new List<string>();
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//body").First().ChildNodes)
                {
                    if (node.Id == "content" || node.XPath.Contains("#"))
                    {
                        continue;
                    }
                    xpaths.Add(node.XPath);
                }

                foreach (string xpath in xpaths)
                {
                    doc.DocumentNode.SelectSingleNode(xpath).Remove();
                }
                foreach(var img in doc.DocumentNode.SelectNodes("//img"))
                {
                    if (!img.Attributes["src"].Value.Contains("http"))
                    {
                        img.SetAttributeValue("src", "http://www.automobilova-mechatronika.fei.stuba.sk" + img.Attributes["src"].Value);
                    }
                }
                /*
                foreach (var Iframe in doc.DocumentNode.SelectNodes("//iframe"))
                {
                    var NewIframe = HtmlNode.CreateNode("<x-ms-webview/>");
                    foreach (HtmlAttribute attr in Iframe.Attributes)
                        NewIframe.Attributes.Add(attr);

                    doc.DocumentNode.SelectNodes("//body").First().AppendChild(NewIframe);
                }
                */
                return doc.DocumentNode.OuterHtml;
            }
        }

        public static async Task<string> GetSectionJson(string url)
        {
            // Download the actual data and count it.
            using (var client = new HttpClient())
            {
                var data = await client.GetAsync(url).ConfigureAwait(false);
                var string_data = await data.Content.ReadAsStringAsync().ConfigureAwait(false);

                JArray valueArray = JArray.Parse(string_data);

                StringBuilder result_html = new StringBuilder();

                result_html.Append(@"<!DOCTYPE html>
                    <html>
                    <head>
                    </head>
                    <body>");

                foreach (JObject value in valueArray)
                {
                    result_html.Append("<h1 style='text-align: center;'>" + value["title"].ToString() + "</h1>");
                    result_html.Append("<h5 style='text-align: center;'>" + value["date"].ToString() + "</h5>");
                    result_html.Append(value["description"].ToString());
                    result_html.Append("<hr>");
                }

                result_html.Append("</body></html>");
                var doc = new HtmlDocument();
                doc.LoadHtml(result_html.ToString());


                return doc.DocumentNode.OuterHtml;
            }
        }
    }
}
