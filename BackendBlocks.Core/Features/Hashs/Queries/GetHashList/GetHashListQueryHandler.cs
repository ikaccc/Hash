using System;
using AutoMapper;
using BackendBlocks.Core.Contracts.Persistence;
using BackendBlocks.Core.Entities;
using MediatR;

namespace BackendBlocks.Core.Features.Hashs.Queries.GetHashList;

public class GetHashListQueryHandler(IAsyncRepository<Hash> _repository, IMapper _mapper) : IRequestHandler<GetHashListQuery, List<HashViewModel>>
{
    public async Task<List<HashViewModel>> Handle(GetHashListQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync();

        return _mapper.Map<List<HashViewModel>>(result.GroupBy(x => x.Date).ToList());
    }
}
