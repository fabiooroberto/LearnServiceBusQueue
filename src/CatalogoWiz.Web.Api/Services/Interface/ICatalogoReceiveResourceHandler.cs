namespace CatalogoWiz.Web.Api.Services.Interface
{
    public interface ICatalogoReceiveResourceHandler
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();
    }
}
