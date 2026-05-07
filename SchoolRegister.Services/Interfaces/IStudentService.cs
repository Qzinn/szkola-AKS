using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Interfaces;

public interface IStudentService
{
    StudentVm GetStudent(Expression<Func<Student, bool>> filterExpression);

    IEnumerable<StudentVm> GetStudents(
        Expression<Func<Student, bool>> filterExpression = null!);
}