using System;
using System.Reflection;
using BackendBlocks.Messages.PublishSubscribeMessages;
using BackendBlocks.Messages.PublishSubscribeMessages.Contracts;

namespace BackendBlocks.MessageBroker.Consumer.Extensions;

public static class MessageExtensions
{
    private static readonly Dictionary<EventMessageType, Type> Types = new();

    static MessageExtensions()
    {
        var types = typeof(IEventMessage).Assembly.GetExportedTypes().Where(t => !t.IsInterface && !t.IsAbstract)
            .Where(t => typeof(IEventMessage).IsAssignableFrom(t));

        foreach (var type in types)
        {
            var attr = type.GetCustomAttribute<MessageBindingAttribute>();
            if (attr != null)
            {
                Types.Add(attr.MessageType, type);
            }
        }
    }

    public static Type GetEventMessageType(this QueueMessage message)
    {
        var eventType = GetMessageType(message);

        if (Types.TryGetValue(eventType, out var type))
        {
            return type;
        }

        throw new ArgumentOutOfRangeException($"Message type {eventType} not known");
    }

    public static EventMessageType GetMessageType(this QueueMessage message)
    {
        if (string.IsNullOrEmpty(message.MessageType))
        {
            throw new InvalidOperationException($"Azure Queue message {message.MessageId} does not have an message type");
        }

        var eventTypeValue = message.MessageType;

        if (Enum.TryParse(eventTypeValue, out EventMessageType eventType))
        {
            return eventType;
        }

        throw new ArgumentOutOfRangeException($"Message type {eventTypeValue} not known");
    }
}

