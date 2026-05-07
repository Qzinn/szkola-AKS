namespace SchoolRegister.ViewModels.VM;

public class GradesReportVm
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = null!;

    public List<GradeVm> Grades { get; set; } = new();
}