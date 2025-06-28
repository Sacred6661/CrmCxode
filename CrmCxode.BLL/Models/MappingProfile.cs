using AutoMapper;
using CrmCxode.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmCxode.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CrmTicket, CxoneTicket>()
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
