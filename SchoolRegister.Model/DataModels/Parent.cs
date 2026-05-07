namespace SchoolRegister.Model.DataModels;

public class Parent : User
{
    public List<Student> Students { get; set; } = new();
}