using System;
using BackendBlocks.Core.Common;

namespace BackendBlocks.Core.Entities;

public class Hash : EntityBase
{
    public byte[] ShaHash { get; set; }
    public DateTime Date { get; set; }
}   
