using System;
using MediatR;

namespace BackendBlocks.Core.Features.Hashs.Commands.CreateHash;

public sealed record CreateHashCommand(/*byte[] ShaHash, DateTime Date*/) : IRequest<int>;
