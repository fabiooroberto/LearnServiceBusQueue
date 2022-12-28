namespace DevOps.Web.Api.Services.Interface
{
    public interface IDevOpsReceiveResourceHandler
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();
    }
}
