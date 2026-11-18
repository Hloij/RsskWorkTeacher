using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nagruzka.models
{
    internal class InList
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string ChasinWeek1 { get; set; }
        public string Weeks1 { get; set; }
        public string Snyatie1 { get; set; }
        public string Konsult1 { get; set; }
        public string ChasinWeek2 { get; set; }
        public string Weeks2 { get; set; }
        public string Snyatie2 { get; set; }
        public string Konsult2 { get; set; }
        public string Pract { get; set; }
        public string Labs { get; set; }
        public string Kursach { get; set; }
        public string Zachet { get; set; }
        public string Ekzamen { get; set; }
        public string Itogo { get; set; }
        public string Itogo1 { get; set; }
        public string Itogo2 { get; set; }
        public string Snyatie { get; set; }
        public string Primichanie { get; set; }
        public InList()
        {
            Name = "0";
            Group = "0";
            ChasinWeek1 = "0";
            Weeks1 = "0";
            Snyatie1 = "0";
            Konsult1 = "0";
            ChasinWeek2 = "0";
            Weeks2 = "0";
            Snyatie2 = "0";
            Konsult2 = "0";
            Pract = "0";
            Labs = "0";
            Kursach = "0";
            Zachet = "0";
            Ekzamen = "0";
            Itogo = "0";
            Itogo1 = "0";
            Itogo2 = "0";
            Snyatie = "0";
            Primichanie = "0";
        }
    }
}
