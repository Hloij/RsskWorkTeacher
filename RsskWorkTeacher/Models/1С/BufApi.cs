using ConsoleApp2;
using RsskWorkTeacher.Models._1С;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RsskWorkTeacher.Models
{
    public class BufApi
    {
        public static  ListCatalog_УчебныеГруппы ListУчебныеГруппы;    
        public static string pattern = @"^[а-яА-Я]{1,4}-.*";
        public static  List<Catalog_УчебныеГруппы> УчебныеГруппы;       
        public static  ListCatalog_Сотрудники ListСотрудники;
        public static  List<Catalog_Сотрудники> Сотрудники;
        public static  Catalog_ЦикловыеМетодическиеКомиссии Комиссии;
        public static async Task <bool> Conect()
        {
            try
            {
                ListУчебныеГруппы = await APIMetods1.Get<ListCatalog_УчебныеГруппы>("Catalog_УчебныеГруппы?$format=json");
                УчебныеГруппы = ListУчебныеГруппы.value.Where(item => Regex.IsMatch(item.Description, pattern)).ToList();
                ListСотрудники = await APIMetods1.Get<ListCatalog_Сотрудники>("Catalog_Сотрудники?$format=json");
                Комиссии = await APIMetods1.Get<Catalog_ЦикловыеМетодическиеКомиссии>("Catalog_ЦикловыеМетодическиеКомиссии?$format=json");
                Сотрудники = ListСотрудники.value.ToList();
                УчебныеГруппы = УчебныеГруппы.OrderBy(p => int.Parse(Regex.Match(p.Description, @"\d+").Value)).ToList();
                Сотрудники = Сотрудники.OrderBy(p => p.Description).ToList();
                return true;
            }
            catch
            {
                return false;
            }
        } 
    }
}
