using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels;

public class Student : User
{
    public virtual Group Group { get; set; } = null!;

    public int? GroupId { get; set; }

    public virtual List<Grade> Grades { get; set; } = new();

    public virtual Parent Parent { get; set; } = null!;

    public int ParentId { get; set; }

    [NotMapped]
    public double AverageGrade
    {
        get
        {
            if (Grades.Count == 0)
                return 0;

            return Grades.Average(g => (double)g.GradeValue);
        }
    }

    [NotMapped]
    public Dictionary<string, double> AverageGradePerSubject
    {
        get
        {
            return Grades
                .GroupBy(g => g.Subject.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Average(x => (double)x.GradeValue)
                );
        }
    }

    [NotMapped]
    public Dictionary<string, List<GradeScale>> GradesPerSubject
    {
        get
        {
            return Grades
                .GroupBy(g => g.Subject.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.GradeValue).ToList()
                );
        }
    }
}