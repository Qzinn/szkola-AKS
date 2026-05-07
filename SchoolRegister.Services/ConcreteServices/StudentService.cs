using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Linq.Expressions;

namespace SchoolRegister.Services.ConcreteServices;

public class StudentService : BaseService, IStudentService
{
    public StudentService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger<BaseService> logger)
        : base(dbContext, mapper, logger)
    {
    }

    public StudentVm GetStudent(Expression<Func<Student, bool>> filterExpression)
    {
        try
        {
            if (filterExpression == null)
                throw new ArgumentNullException(nameof(filterExpression));

            var studentEntity = DbContext.Users
                .OfType<Student>()
                .FirstOrDefault(filterExpression);

            var studentVm = Mapper.Map<StudentVm>(studentEntity);

            return studentVm;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public IEnumerable<StudentVm> GetStudents(
        Expression<Func<Student, bool>> filterExpression = null!)
    {
        try
        {
            var studentEntities = DbContext.Users
                .OfType<Student>()
                .AsQueryable();

            if (filterExpression != null)
                studentEntities = studentEntities.Where(filterExpression);

            var studentVms =
                Mapper.Map<IEnumerable<StudentVm>>(studentEntities);

            return studentVms;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }
}