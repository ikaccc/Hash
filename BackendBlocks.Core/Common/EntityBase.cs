using System;

namespace BackendBlocks.Core.Common;

public class EntityBase
{
    public int Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
