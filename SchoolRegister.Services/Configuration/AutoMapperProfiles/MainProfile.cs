using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Configuration.AutoMapperProfiles;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<Subject, SubjectVm>()
            .ForMember(dest => dest.TeacherName,
                x => x.MapFrom(src =>
                    src.Teacher == null
                        ? null
                        : $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
            .ForMember(dest => dest.Groups,
                x => x.MapFrom(src =>
                    src.SubjectGroups.Select(y => y.Group)));

        CreateMap<AddOrUpdateSubjectVm, Subject>();

        CreateMap<AddOrUpdateGroupVm, Group>();

        CreateMap<Group, GroupVm>()
            .ForMember(dest => dest.Students,
                x => x.MapFrom(src => src.Students))
            .ForMember(dest => dest.Subjects,
                x => x.MapFrom(src =>
                    src.SubjectGroups.Select(s => s.Subject)));

        CreateMap<SubjectVm, AddOrUpdateSubjectVm>();

        CreateMap<Student, StudentVm>()
            .ForMember(dest => dest.GroupName,
                x => x.MapFrom(src =>
                    src.Group == null
                        ? null
                        : src.Group.Name))
            .ForMember(dest => dest.ParentName,
                x => x.MapFrom(src =>
                    src.Parent == null
                        ? null
                        : $"{src.Parent.FirstName} {src.Parent.LastName}"));

        CreateMap<Teacher, TeacherVm>();

        CreateMap<Grade, GradeVm>();
    }
}