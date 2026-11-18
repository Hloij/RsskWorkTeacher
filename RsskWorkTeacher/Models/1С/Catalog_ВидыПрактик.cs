using System;
namespace ConsoleApp2
{
    public class Catalog_ВидыПрактик
    {
        public Guid Ref_Key { get; set; }
        public bool? Predefined { get; set; }
        public string PredefinedDataName { get; set; }
        public string DataVersion { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid? Parent_Key { get; set; }
        public bool? IsFolder { get; set; }
        public bool? DeletionMark { get; set; }
        public string ПолноеНаименование { get; set; }
        public string ТипПрактики { get; set; }
        public Guid? ОсновнойПреподаватель_Key { get; set; }
        public Guid? ОсновнаяАудитория_Key { get; set; }
        public string СокращенноеНаименование { get; set; }

        //public virtual CatalogВидыПрактик Parent { get; set; }
        //public virtual Преподаватель ОсновнойПреподаватель { get; set; }
        //public virtual Аудитория ОсновнаяАудитория { get; set; }
    }
}
