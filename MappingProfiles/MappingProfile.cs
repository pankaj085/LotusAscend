using AutoMapper;
using LotusAscend.Contracts;
using LotusAscend.Models;



namespace LotusAscend.MappingProfiles;

/// <summary>
/// Configures the object-to-object mappings for the application using AutoMapper.
/// This centralizes the logic for transforming database entities into API response DTOs.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Member, MemberResponse>();
    }
}