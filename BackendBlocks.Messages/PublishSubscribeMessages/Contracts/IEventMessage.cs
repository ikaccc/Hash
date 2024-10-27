using System;

namespace BackendBlocks.Messages.PublishSubscribeMessages.Contracts;

public interface IEventMessage
{
    //for scaling purposes, we need to make sure that the messages are processed in the order they are received (if required)
    string SessionId { get; }
}
