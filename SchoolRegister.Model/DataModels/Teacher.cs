namespace SchoolRegister.Model.DataModels;

public class Teacher : User
{
    public virtual List<Subject> Subjects { get; set; } = new();
}