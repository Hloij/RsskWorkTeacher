using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nagruzka.models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ДвижениеКонтингента
    {
        public DateTime Period { get; set; }
        public string LineNumber { get; set; }
        public bool Active { get; set; }
        public string Студент_Key { get; set; }
        public string Статус { get; set; }
        public string Группа_Key { get; set; }
        public string ВидФинансирования { get; set; }
        public string ВидБюджета { get; set; }
        public string Аналитика { get; set; }
        public string Аналитика_Type { get; set; }
    }

    public class Root
    {
        [JsonProperty("odata.metadata")]
        public string odatametadata { get; set; }
        public List<Value> value { get; set; }
    }

    public class Value
    {
        public string Recorder { get; set; }
        public string Recorder_Type { get; set; }
        public List<ДвижениеКонтингента> RecordSet { get; set; }
    }



}
