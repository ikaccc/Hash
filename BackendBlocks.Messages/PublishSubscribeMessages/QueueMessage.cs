namespace BackendBlocks.Messages.PublishSubscribeMessages;

public sealed record QueueMessage(string MessageBody, string MessageId, string MessageType, ulong DeliveryTag);
