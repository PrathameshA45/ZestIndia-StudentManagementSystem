using AutoMapper;
using Structure.Data.DTOs;
using Structure.Data.Entities;
using StudentEntity = Structure.Data.Entities.Student;

namespace Structure.Infrastructure.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudentEntity, StudentDto>()
            .ReverseMap();
    }
}