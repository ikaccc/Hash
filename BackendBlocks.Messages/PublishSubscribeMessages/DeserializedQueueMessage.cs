using System;
using BackendBlocks.Messages.PublishSubscribeMessages.Contracts;

namespace BackendBlocks.Messages.PublishSubscribeMessages;

public class DeserializedQueueMessage<TMessage> where TMessage : IEventMessage
{
    public TMessage Message { get; set; }
    public EventMessageType EventType { get; set; }
    public string MessageId { get; set; }
}
