namespace SchoolRegister.Model.DataModels;

public class Group
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual List<Student> Students { get; set; } = new();

    public virtual List<SubjectGroup> SubjectGroups { get; set; } = new();
}