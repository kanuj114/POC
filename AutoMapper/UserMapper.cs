using System.Text;
using AutoMapper;
using TokenAuthentication.DTO;
using TokenAuthentication.Models;
namespace TokenAuthentication.AutoMapper
{
    public class UserMapper: Profile
    {
        public UserMapper()
        {
            //CreateMap<TblUser,RegisterDTO>().ReverseMap();
            CreateMap<LoginDTO, TblUser>();
            CreateMap<RegisterDTO, TblUser>()
           .ForMember(dest => dest.Password, opt => opt.MapFrom(src => Encoding.Unicode.GetBytes(src.Password)));
        }
    }
}