using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Linq.Expressions;

namespace SchoolRegister.Services.ConcreteServices;

public class TeacherService : BaseService, ITeacherService
{
    public TeacherService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger<BaseService> logger)
        : base(dbContext, mapper, logger)
    {
    }

    public TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterExpression)
    {
        try
        {
            var teacherEntity = DbContext.Users
                .OfType<Teacher>()
                .FirstOrDefault(filterExpression);

            return Mapper.Map<TeacherVm>(teacherEntity);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public IEnumerable<TeacherVm> GetTeachers(
        Expression<Func<Teacher, bool>> filterExpression = null!)
    {
        try
        {
            var teacherEntities = DbContext.Users
                .OfType<Teacher>()
                .AsQueryable();

            if (filterExpression != null)
                teacherEntities = teacherEntities.Where(filterExpression);

            return Mapper.Map<IEnumerable<TeacherVm>>(teacherEntities);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public IEnumerable<GroupVm> GetTeachersGroups(TeachersGroupsVm teachersGroupsVm)
    {
        try
        {
            var groups = DbContext.Subjects
                .Include(x => x.SubjectGroups)
                .ThenInclude(x => x.Group)
                .Where(x => x.TeacherId == teachersGroupsVm.TeacherId)
                .SelectMany(x => x.SubjectGroups.Select(y => y.Group))
                .Distinct()
                .ToList();

            return Mapper.Map<IEnumerable<GroupVm>>(groups);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }
}