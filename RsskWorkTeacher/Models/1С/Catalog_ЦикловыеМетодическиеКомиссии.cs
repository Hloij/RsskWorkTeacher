using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RsskWorkTeacher.Models._1С
{
    public class Catalog_ЦикловыеМетодическиеКомиссии
    {

        [JsonProperty("odata.metadata")]
        public string odatametadata { get; set; }
        public List<Value> value { get; set; }
    }

    public class Value
    {
        public string Ref_Key { get; set; }
        public string DataVersion { get; set; }
        public bool DeletionMark { get; set; }
        public string Owner_Key { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Работает { get; set; }
        public List<ДисциплиныКафедры> ДисциплиныКафедры { get; set; }
        public List<object> ДополнительныеРеквизиты { get; set; }
        public List<ПрактикиКафедры> ПрактикиКафедры { get; set; }
        public bool Predefined { get; set; }
        public string PredefinedDataName { get; set; }

        [JsonProperty("Owner@navigationLinkUrl")]
        public string OwnernavigationLinkUrl { get; set; }
    }

    public class ДисциплиныКафедры
    {
        public string Ref_Key { get; set; }
        public string LineNumber { get; set; }
        public string _Key { get; set; }
    }

    public class ПрактикиКафедры
    {
        public string Ref_Key { get; set; }
        public string LineNumber { get; set; }
        public string _Key { get; set; }
    }
}