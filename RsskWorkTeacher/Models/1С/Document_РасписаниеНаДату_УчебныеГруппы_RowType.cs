using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Document_РасписаниеНаДату_УчебныеГруппы_RowType
    {
        public Guid Ref_Key { get; set; }
        public long LineNumber { get; set; }
        public Guid? УчебнаяГруппа_Key { get; set; }
        public bool? Дистанционно { get; set; }
    }
}
