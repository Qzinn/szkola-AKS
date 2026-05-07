using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM;

public class GradeVm
{
    public int Id { get; set; }

    public GradeScale GradeValue { get; set; }

    public DateTime DateOfIssue { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }
}