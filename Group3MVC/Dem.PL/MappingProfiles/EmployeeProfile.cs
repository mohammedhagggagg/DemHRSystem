using AutoMapper;
using Dem.DAL.Models;
using Dem.PL.ViewModels;

namespace Dem.PL.MappingProfiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            //CreateMap<EmployeeViewModel, Employee>().ForMember(D => D.Name,O => O.MapFrom(S => S.Name));
        }
    }
}
