using Azure.Messaging.ServiceBus;
using CatalogoWiz.Web.Api.Services.Interface;
using CatalogoWiz.Web.Api.ViewModel;
using System.Text.Json;

namespace CatalogoWiz.Web.Api.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IBusService _busService;
        public ResourceService(IBusService busService)
        {
            _busService = busService;
        }
        public async Task<ResourceResponseViewModel> Create(ResourceRequestViewModel request)
        {
            var body = new ResourceResponseViewModel
            {
                IdProject = request.IdProject,
                Name = request.Name,
                CreateApi = request.CreateApi,
                CreateDataBase = request.CreateDataBase,
                IdResource = Guid.NewGuid()
            };
            await _busService.SenderAsync("devops-create-resource", body);
            return body;
        }
    }
}
