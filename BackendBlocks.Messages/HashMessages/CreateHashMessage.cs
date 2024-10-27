using System;
using BackendBlocks.Messages.HashMessages.Contracts;
using BackendBlocks.Messages.PublishSubscribeMessages;

namespace BackendBlocks.Messages.HashMessages;

[MessageBinding(EventMessageType.CreateHashMessage)]
public class CreateHashMessage : IHashMessage<CreateHashMessage>, IHashMessageBase
{
    public byte[] ShaHash { get; set; }
    public DateTime Date { get; set; }
    public DateTime UtcReceived { get; set; }
    public DateTime UtcCreated { get; set; }
    public string MessageId { get; set; }

    public string SessionId => MessageId;
}
