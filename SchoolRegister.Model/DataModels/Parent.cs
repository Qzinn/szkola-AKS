namespace SchoolRegister.Model.DataModels;

public class Parent : User
{
    public virtual List<Student> Students { get; set; } = new();
}