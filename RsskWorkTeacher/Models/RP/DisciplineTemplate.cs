using System.Collections.Generic;

namespace RsskWorkTeacher.Models.RP
{
    public class DisciplineTemplate
    {
        public string DisciplineKod { get; set; }
        public string Purpose { get; set; }
        public string MaterialTech { get; set; } // ДОБАВЛЕНО
        public List<Competency> Competencies { get; set; } = new();
        public List<string> LiteratureMain { get; set; } = new();
        public List<string> LiteratureAdditional { get; set; } = new();
        public List<EvaluationCriteria> Evaluations { get; set; } = new();
        public List<ThemePlan> Themes { get; set; } = new(); // ДОБАВЛЕНО
    }
}