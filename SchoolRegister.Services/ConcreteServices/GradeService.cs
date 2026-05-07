using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices;

public class GradeService : BaseService, IGradeService
{
    private readonly UserManager<User> _userManager;

    public GradeService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger<BaseService> logger,
        UserManager<User> userManager)
        : base(dbContext, mapper, logger)
    {
        _userManager = userManager;
    }

    public GradeVm AddGradeToStudent(
        AddGradeToStudentVm addGradeToStudentVm)
    {
        try
        {
            var teacher = DbContext.Users
                .OfType<Teacher>()
                .FirstOrDefault(x =>
                    x.Id == addGradeToStudentVm.TeacherId);

            if (teacher == null)
                throw new Exception("Teacher not found");

            var student = DbContext.Users
                .OfType<Student>()
                .FirstOrDefault(x =>
                    x.Id == addGradeToStudentVm.StudentId);

            if (student == null)
                throw new Exception("Student not found");

            var subject = DbContext.Subjects
                .FirstOrDefault(x =>
                    x.Id == addGradeToStudentVm.SubjectId);

            if (subject == null)
                throw new Exception("Subject not found");

            var grade = new Grade()
            {
                StudentId = student.Id,
                SubjectId = subject.Id,
                GradeValue = addGradeToStudentVm.GradeValue,
                DateOfIssue = DateTime.Now
            };

            DbContext.Grades.Add(grade);

            DbContext.SaveChanges();

            return Mapper.Map<GradeVm>(grade);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public GradesReportVm GetGradesReportForStudent(
        GetGradesReportVm getGradesReportVm)
    {
        try
        {
            var student = DbContext.Users
                .OfType<Student>()
                .FirstOrDefault(x =>
                    x.Id == getGradesReportVm.StudentId);

            if (student == null)
                throw new Exception("Student not found");

            var grades = DbContext.Grades
                .Where(x =>
                    x.StudentId == student.Id)
                .ToList();

            var gradesVm = Mapper.Map<List<GradeVm>>(grades);

            return new GradesReportVm()
            {
                StudentId = student.Id,
                StudentName = $"{student.FirstName} {student.LastName}",
                Grades = gradesVm
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }
}