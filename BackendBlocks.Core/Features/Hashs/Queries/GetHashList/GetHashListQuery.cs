using System;
using MediatR;

namespace BackendBlocks.Core.Features.Hashs.Queries.GetHashList;

public sealed record GetHashListQuery() : IRequest<List<HashViewModel>>;
