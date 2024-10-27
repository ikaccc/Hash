using System;
using System.Text.Json.Serialization;
using BackendBlocks.Messages.PublishSubscribeMessages.Contracts;

namespace BackendBlocks.Messages.HashMessages.Contracts;

/// <summary>
/// Usage of CRTP
/// https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern
/// JsonDerivedType attribute is used because of Polymorphic serialization
/// </summary>
[JsonDerivedType(typeof(CreateHashMessage), typeDiscriminator: nameof(CreateHashMessage))]
public interface IHashMessageBase : IHashMessage<IHashMessageBase>, IEventMessage
{ }

public interface IHashMessage<T> where T : IHashMessage<T>
{
    DateTime UtcReceived { get; set; }
    DateTime UtcCreated { get; set; }
    string MessageId { get; set; }
}