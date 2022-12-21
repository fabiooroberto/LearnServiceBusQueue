namespace CatalogoWiz.Web.Api.Services.Interface
{
    public interface IReceiveResource
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();
    }
}
