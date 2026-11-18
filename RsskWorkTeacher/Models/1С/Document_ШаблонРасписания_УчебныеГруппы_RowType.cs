using System;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace ConsoleApp2
{
    /// <summary>
    /// Шаблон учебные группы строки
    /// </summary>
    public class Document_ШаблонРасписания_УчебныеГруппы_RowType
    {
        public Guid Ref_Key { get; set; }
        public Int64 LineNumber { get; set; }
        public Guid? УчебнаяГруппа_Key { get; set; }
    }




}
