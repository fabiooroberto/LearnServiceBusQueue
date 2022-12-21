namespace CatalogoWiz.Web.Api.ViewModel
{
    public class ResourceDevopsViewModel
    {
        public Guid IdProject { get; set; }
        public Guid IdResource { get; set; }
        public Guid IdRepository { get; set; }
        public string Name { get; set; }
        public string NameRepository { get; set; }
        public string UrlRepository { get; set; }
    }
}
