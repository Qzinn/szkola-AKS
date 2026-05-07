namespace SchoolRegister.Model.DataModels;

public class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual List<SubjectGroup> SubjectGroups { get; set; } = new();

    public virtual Teacher Teacher { get; set; } = null!;

    public int? TeacherId { get; set; }

    public virtual List<Grade> Grades { get; set; } = new();
}