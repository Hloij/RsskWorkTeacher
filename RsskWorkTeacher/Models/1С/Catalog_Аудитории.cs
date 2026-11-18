using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Catalog_Аудитории
    {
        public Guid Ref_Key { get; set; }
        public bool? Predefined { get; set; }
        public string PredefinedDataName { get; set; }
        public string DataVersion { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid? Owner_Key { get; set; }
        public Guid? Parent_Key { get; set; }
        public bool? IsFolder { get; set; }
        public bool? DeletionMark { get; set; }
        public long? Вместимость { get; set; }
        public Guid? ВидАудитории_Key { get; set; }


        //public ICollection<CatalogАудиторииИзучаемыеДисциплиныRowType> ИзучаемыеДисциплины { get; set; }
        //public ICollection<CatalogАудиторииДополнительныеРеквизитыRowType> ДополнительныеРеквизиты { get; set; }

        //public virtual OwnerType Owner { get; set; }
        //public virtual ParentType Parent { get; set; }
        //public virtual ВидАудиторииType ВидАудитории { get; set; }
    }
}
