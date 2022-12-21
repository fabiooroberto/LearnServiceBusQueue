namespace CatalogoWiz.Web.Api.ViewModel
{
    public class ResourceRequestViewModel
    {
        public Guid IdProject { get; set; }
        public string Name { get; set; }
        public bool CreateDataBase { get; set; }
        public bool CreateApi { get; set; }
    }
}
