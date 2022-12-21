using CatalogoWiz.Web.Api.Services.Interface;
using CatalogoWiz.Web.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoWiz.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;
        private readonly IResourceService _resourceService;

        public ResourceController(
            ILogger<ResourceController> logger,
            IResourceService resourceService)
        {
            _logger = logger;
            _resourceService = resourceService;
        }

        [HttpPost()]
        public async Task<ActionResult<ResourceRequestViewModel>> Post(ResourceRequestViewModel request)
        {
            var response = await _resourceService.Create(request);
            return Ok(response);
        }
    }
}