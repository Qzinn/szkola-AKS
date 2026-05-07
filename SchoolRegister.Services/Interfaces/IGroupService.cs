using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq.Expressions;

namespace SchoolRegister.Services.Interfaces;

public interface IGroupService
{
    GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm);

    GroupVm GetGroup(Expression<Func<Group, bool>> filterExpression);

    IEnumerable<GroupVm> GetGroups(
        Expression<Func<Group, bool>> filterExpression = null!);

    StudentVm AttachStudentToGroup(
        AttachDetachStudentToGroupVm attachDetachStudentToGroupVm);

    StudentVm DetachStudentFromGroup(
        AttachDetachStudentToGroupVm attachDetachStudentToGroupVm);

    GroupVm AttachSubjectToGroup(
        AttachDetachSubjectGroupVm attachDetachSubjectGroupVm);

    GroupVm DetachSubjectFromGroup(
        AttachDetachSubjectGroupVm attachDetachSubjectGroupVm);

    SubjectVm AttachTeacherToSubject(
        AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm);

    SubjectVm DetachTeacherFromSubject(
        AttachDetachSubjectToTeacherVm attachDetachSubjectToTeacherVm);
}