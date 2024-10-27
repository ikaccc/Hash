using AutoMapper;
using BackendBlocks.Core.Contracts.Persistence;
using BackendBlocks.MessageBroker.Consumer;
using BackendBlocks.MessageBroker.Consumer.Contracts;
using BackendBlocks.MessageBroker.Consumer.Utilities;
using BackendBlocks.Messages.HashMessages;
using BackendBlocks.Messages.HashMessages.Contracts;
using BackendBlocks.Messages.PublishSubscribeMessages;

namespace BackednBlocks.Hash.Processor.HostedService;

public class HashWorker : MessageConsumerProcessorBase
{
    private readonly ILogger<HashWorker> _logger;
    private readonly IMessageParser _messageParser;
    private readonly IAsyncRepository<BackendBlocks.Core.Entities.Hash> _hashRepository;
    private readonly IMapper _mapper;

    private readonly Dictionary<
    EventMessageType,
    Func<DeserializedQueueMessage<IHashMessageBase>, Task>> _messageTypeHandlers;

    public HashWorker(ILogger<HashWorker> logger, 
            IMessageParser messageParser, 
            IMapper mapper,
            IAsyncRepository<BackendBlocks.Core.Entities.Hash> hashRepository,
            IMessageConsumer messageConsumer) : base(logger, messageConsumer)
    {
        _logger = logger;
        _messageParser = messageParser;
        _hashRepository = hashRepository;
        _mapper = mapper;
        
        _messageTypeHandlers = new Dictionary<EventMessageType, Func<DeserializedQueueMessage<IHashMessageBase>, Task>>
        {
            { EventMessageType.CreateHashMessage, HandleCreateHashMessageAsync }
        };
    }

    protected override async Task<ulong> HandleMessagesAsync(QueueMessage qMessages)
    {
        try
        {
            var message = ParseReceivedMessages(qMessages);

            if (_messageTypeHandlers.TryGetValue(message.EventType, out var typedEventMessageHandler))
            {
                await typedEventMessageHandler.Invoke(message).ConfigureAwait(false);
            }
            else
            {
                _logger.LogError($"No handler implemented for message type {message.EventType}");
            }

            return qMessages.DeliveryTag;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            throw;
        }
    }

    private DeserializedQueueMessage<IHashMessageBase> ParseReceivedMessages(QueueMessage qMessages)
    {
        try
        {
            return _messageParser.Parse<IHashMessageBase>(qMessages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing message");
            throw;
        }
    }

    private async Task HandleCreateHashMessageAsync(DeserializedQueueMessage<IHashMessageBase> message)
    {      
        try 
        {
            var hashMessage = (CreateHashMessage)message.Message;

            var entity = _mapper.Map<BackendBlocks.Core.Entities.Hash>(hashMessage);

            await _hashRepository.CreateAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating hash");
        }   
    }
}