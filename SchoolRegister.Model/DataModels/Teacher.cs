namespace SchoolRegister.Model.DataModels;

public class Teacher : User
{
    public List<Subject> Subjects { get; set; } = new();
}