using CatalogoWiz.Web.Api.ViewModel;

namespace CatalogoWiz.Web.Api.Services.Interface
{
    public interface IResourceService
    {
        Task<ResourceResponseViewModel> Create(ResourceRequestViewModel request);
    }
}
