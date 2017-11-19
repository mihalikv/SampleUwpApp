using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class DataItem
    {
        public string Header { get; set; }
        public string ContentUrl { get; set; }
        public string Format { get; set; }
        public NotifyTaskCompletion<string> Content { get; private set; }

        public DataItem(string Header, string ContentUrl, string Format)
        {
            this.Header = Header;
            this.ContentUrl = ContentUrl;
            this.Format = Format;
        }

        public void DownloadContent()
        {
            if(this.Format == "JSON")
            {
                Content = new NotifyTaskCompletion<string>(Service.GetSectionJson(ContentUrl));
            }
            else
            {
                Content = new NotifyTaskCompletion<string>(Service.GetSection(ContentUrl));
            }
        }
    }
}
