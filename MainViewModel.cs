using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class MainViewModel
    {
        const string main_url = "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/?q=aplika%C4%8Dn%C3%A9-oblasti-mechatroniky";
        public NotifyTaskCompletion<List<DataItem>> Sections { get; private set; }

        public MainViewModel()
        {
            Sections = new NotifyTaskCompletion<List<DataItem>>(Service.GetSections(main_url));
        }

        public void DownloadContent(int index)
        {
            Sections.Result[index].DownloadContent();
        }
    }
}
