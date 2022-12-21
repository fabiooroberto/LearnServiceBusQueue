using CatalogoWiz.Web.Api.ViewModel;

namespace CatalogoWiz.Web.Api.Services.Interface
{
    public interface IBusService
    {
        Task SenderAsync<T>(string queueName, T request);
    }
}
