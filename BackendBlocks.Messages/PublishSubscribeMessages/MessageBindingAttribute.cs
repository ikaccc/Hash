using System;

namespace BackendBlocks.Messages.PublishSubscribeMessages;

    [AttributeUsage(AttributeTargets.Class)]
    public class MessageBindingAttribute : Attribute
    {
        public EventMessageType MessageType { get; set; }

        public MessageBindingAttribute(EventMessageType messageType)
        {
            MessageType = messageType;
        }
    }
