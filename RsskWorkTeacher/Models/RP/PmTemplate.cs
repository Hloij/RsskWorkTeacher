using System.Collections.Generic;

namespace RsskWorkTeacher.Models.RP
{
    public class PmTemplate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; } = "";
        public string MaterialTech { get; set; } = "";
        public List<Competency> Competencies { get; set; } = new List<Competency>();
        public List<ThemePlan> Themes { get; set; } = new List<ThemePlan>();
        public List<string> LiteratureMain { get; set; } = new List<string>();
        public List<EvaluationCriteria> Evaluations { get; set; } = new List<EvaluationCriteria>();
    }
}