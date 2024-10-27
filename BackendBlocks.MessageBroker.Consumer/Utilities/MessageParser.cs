using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using BackendBlocks.MessageBroker.Consumer.Extensions;
using BackendBlocks.Messages.PublishSubscribeMessages;
using BackendBlocks.Messages.PublishSubscribeMessages.Contracts;
using Microsoft.Extensions.Logging;

namespace BackendBlocks.MessageBroker.Consumer.Utilities;

public class MessageParser : IMessageParser
{
    private readonly ILogger<MessageParser> _logger;

    public MessageParser(ILogger<MessageParser> logger)
    {
        _logger = logger;
    }
    
    public DeserializedQueueMessage<TMessage> Parse<TMessage>(QueueMessage queueMessage)
        where TMessage : IEventMessage
    {
        try
        {
            if (queueMessage.MessageBody == null)
            {
                _logger.LogError($"Azure Queue Message {queueMessage.MessageId} of type {queueMessage.MessageType} does not contain any payload");
                return null;
            }

            var messageBody = ParseMessageBody<TMessage>(
                queueMessage.MessageBody,
                queueMessage.GetEventMessageType());

            var deserializedQueueMessage = new DeserializedQueueMessage<TMessage>
            {
                Message = messageBody,
                EventType = queueMessage.GetMessageType(),
                MessageId = queueMessage.MessageId,
            };
            return deserializedQueueMessage;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to de-serialize Azure Queue message with id {queueMessage.MessageId}", ex);
            return null;
        }
    }

    private static TMessage ParseMessageBody<TMessage>(string message, Type messageType) where TMessage : IEventMessage =>
        (TMessage)JsonSerializer.Deserialize(message, messageType, Options);

    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,  
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
        }
    };
}
