using AutoMapper;

using MauiUltimateTemplate.Application.DTOs;
using MauiUltimateTemplate.Domain.Entities;

namespace MauiUltimateTemplate.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Создаем карту: из Entity в DTO и обратно
            CreateMap<Note, NoteDto>().ReverseMap();

            // Если имена полей вдруг разные, можно уточнить:
            // .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Content));
        }
    }
}