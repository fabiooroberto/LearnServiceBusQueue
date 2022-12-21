namespace CatalogoWiz.Web.Api.ViewModel
{
    public class ResourceResponseViewModel
    {
        public Guid IdResource { get; set; }
        public Guid IdProject { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool CreateDataBase { get; set; }
        public bool CreateApi { get; set; }
    }
}
