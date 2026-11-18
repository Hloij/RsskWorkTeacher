using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// Список шаблонов расписания за все семестры  и года
    /// </summary>
    public class ListDocument_ШаблонРасписания
    {
        public string OdataMetadata { get; set; }
        public List<Document_ШаблонРасписания> value { get; set; }
    }
}
