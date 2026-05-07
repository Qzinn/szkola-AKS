using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Linq.Expressions;

namespace SchoolRegister.Services.ConcreteServices;

public class GroupService : BaseService, IGroupService
{
    public GroupService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger<BaseService> logger)
        : base(dbContext, mapper, logger)
    {
    }

    public GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm)
    {
        try
        {
            var groupEntity = Mapper.Map<Group>(addOrUpdateGroupVm);

            if (!addOrUpdateGroupVm.Id.HasValue || addOrUpdateGroupVm.Id == 0)
                DbContext.Groups.Add(groupEntity);
            else
                DbContext.Groups.Update(groupEntity);

            DbContext.SaveChanges();

            return Mapper.Map<GroupVm>(groupEntity);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public GroupVm GetGroup(Expression<Func<Group, bool>> filterExpression)
    {
        try
        {
            var groupEntity = DbContext.Groups
                .FirstOrDefault(filterExpression);

            return Mapper.Map<GroupVm>(groupEntity);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public IEnumerable<GroupVm> GetGroups(
        Expression<Func<Group, bool>> filterExpression = null!)
    {
        try
        {
            var groups = DbContext.Groups.AsQueryable();

            if (filterExpression != null)
                groups = groups.Where(filterExpression);

            return Mapper.Map<IEnumerable<GroupVm>>(groups);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public StudentVm AttachStudentToGroup(
        AttachDetachStudentToGroupVm attachDetachStudentToGroupVm)
    {
        try
        {
            var student = DbContext.Users
                .OfType<Student>()
                .FirstOrDefault(x =>
                    x.Id == attachDetachStudentToGroupVm.StudentId);

            student!.GroupId = attachDetachStudentToGroupVm.GroupId;

            DbContext.SaveChanges();

            return Mapper.Map<StudentVm>(student);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public StudentVm DetachStudentFromGroup(
        AttachDetachStudentToGroupVm attachDetachStudentToGroupVm)
    {
        try
        {
            var student = DbContext.Users
                .OfType<Student>()
                .FirstOrDefault(x =>
                    x.Id == attachDetachStudentToGroupVm.StudentId);

            student!.GroupId = null;

            DbContext.SaveChanges();

            return Mapper.Map<StudentVm>(student);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public GroupVm AttachSubjectToGroup(
        AttachDetachSubjectGroupVm attachDetachSubjectGroupVm)
    {
        try
        {
            var subjectGroup = new SubjectGroup()
            {
                GroupId = attachDetachSubjectGroupVm.GroupId,
                SubjectId = attachDetachSubjectGroupVm.SubjectId
            };

            DbContext.SubjectGroups.Add(subjectGroup);

            DbContext.SaveChanges();

            var group = DbContext.Groups
                .FirstOrDefault(x =>
                    x.Id == attachDetachSubjectGroupVm.GroupId);

            return Mapper.Map<GroupVm>(group);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public GroupVm DetachSubjectFromGroup(
        AttachDetachSubjectGroupVm attachDetachSubjectGroupVm)
    {
        try
        {
            var subjectGroup = DbContext.SubjectGroups
                .FirstOrDefault(x =>
                    x.GroupId == attachDetachSubjectGroupVm.GroupId
                    && x.SubjectId == attachDetachSubjectGroupVm.SubjectId);

            DbContext.SubjectGroups.Remove(subjectGroup!);

            DbContext.SaveChanges();

            var group = DbContext.Groups
                .FirstOrDefault(x =>
                    x.Id == attachDetachSubjectGroupVm.GroupId);

            return Mapper.Map<GroupVm>(group);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public SubjectVm AttachTeacherToSubject(
        AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm)
    {
        try
        {
            var subject = DbContext.Subjects
                .FirstOrDefault(x =>
                    x.Id == attachDetachSubjectToTeacherVm.SubjectId);

            subject!.TeacherId =
                attachDetachSubjectToTeacherVm.TeacherId;

            DbContext.SaveChanges();

            return Mapper.Map<SubjectVm>(subject);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public SubjectVm DetachTeacherFromSubject(
        AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm)
    {
        try
        {
            var subject = DbContext.Subjects
                .FirstOrDefault(x =>
                    x.Id == attachDetachSubjectToTeacherVm.SubjectId);

            subject!.TeacherId = null;

            DbContext.SaveChanges();

            return Mapper.Map<SubjectVm>(subject);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message);
            throw;
        }
    }
}