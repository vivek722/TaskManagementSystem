using AutoMapper;
using TaskManagementSystem.DtoModels;
using TaskManagementSystem.Model;

namespace TaskManagementSystem;

public class AutoMapperProfile :Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TaskDto, TaskManage>()
            .AfterMap((src, dest) =>
            {
                if (dest.SubTasks != null)
                {
                    foreach (var sub in dest.SubTasks)
                    {
                        sub.TaskManageid = dest.Id;
                    }
                }
            });
        CreateMap<SubTaskManegdto, SubTaskManeg>();

        CreateMap<EmployeeDto, EmployeeModel>()
            .ForMember(dest => dest.password, opt => opt.Ignore());


        CreateMap<Projectdto, projectModel>();
    }
}
