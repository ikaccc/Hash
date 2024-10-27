using System;
using AutoMapper;
using BackendBlocks.Core.Entities;
using BackendBlocks.Core.Features.Hashs.Queries.GetHashList;
using BackendBlocks.Messages.HashMessages;

namespace BackendBlocks.Core.Mappings;

public class HashProfile : Profile
{
    public HashProfile()
    {
        CreateMap<CreateHashMessage, Hash>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore());
            
        CreateMap<IGrouping<DateTime, Hash>, HashViewModel>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Key.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count()));
    }
}
