using API.Dto;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Game, Game>();

            CreateMap<Game, GameDto>();

            CreateMap<GameParticipant, Profiles.Profile>()
                .ForMember(d => d.Nickname, o => o.MapFrom(s => s.AppUser.Nickname))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.AppUser.Id));
        }
    }
}
