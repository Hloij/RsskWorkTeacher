using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace ConsoleApp2
{
    /// <summary>
    /// Документ Шаблон расписания
    /// Общая информация об элементе расписания
    /// </summary>
    public class Document_ШаблонРасписания
    {
        public Guid Ref_Key { get; set; }
        public string DataVersion { get; set; }
        public string Number { get; set; }
        public DateTime? Date { get; set; }
        public bool? DeletionMark { get; set; }
        public bool? Posted { get; set; }
        public Guid? Организация_Key { get; set; }
        public Guid? ПериодОбучения_Key { get; set; }
        public bool? ПарыНаОтдельнуюЗакладку { get; set; }
        public Guid? Отделение_Key { get; set; }
        public Int64? ТекущийНомерПары { get; set; }
        public Guid? Ответственный_Key { get; set; }
        public Guid? Автор_Key { get; set; }
        public string Комментарий { get; set; }
        public string ФормаОбучения { get; set; }
        public virtual ICollection<Document_ШаблонРасписания_Расписание_RowType> Расписание { get; set; }
        public virtual ICollection<Document_ШаблонРасписания_УчебныеГруппы_RowType> УчебныеГруппы { get; set; }

        //public string Организация { get; set; }
        //public string ПериодОбучения { get; set; }
        //public string Отделение { get; set; }
        //public string Ответственный { get; set; }
        //public string Автор { get; set; }


        //<NavigationProperty Name = "Организация" Relationship="StandardODATA.Document_ШаблонРасписания_Организация" FromRole="Begin" ToRole="End"/>
        //<NavigationProperty Name = "ПериодОбучения" Relationship="StandardODATA.Document_ШаблонРасписания_ПериодОбучения" FromRole="Begin" ToRole="End"/>
        //<NavigationProperty Name = "Отделение" Relationship="StandardODATA.Document_ШаблонРасписания_Отделение" FromRole="Begin" ToRole="End"/>
        //<NavigationProperty Name = "Ответственный" Relationship="StandardODATA.Document_ШаблонРасписания_Ответственный" FromRole="Begin" ToRole="End"/>
        //<NavigationProperty Name = "Автор" Relationship="StandardODATA.Document_ШаблонРасписания_Автор" FromRole="Begin" ToRole="End"/>








    }




}
