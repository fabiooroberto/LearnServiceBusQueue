namespace DevOps.Web.Api.ViewModel
{
    public class ResourceViewModel
    {
        public Guid IdProject { get; set; }
        public Guid IdResource { get; set; }
        public string Name { get; set; }
        public bool CreateDataBase { get; set; }
        public bool CreateApi { get; set; }
    }

    public class HandManViewModel
    {
        public Guid IdProject { get; set; }
        public Guid IdResource { get; set; }
        public Guid IdRepository { get; set; }
        public string Name { get; set; }
        public string NameRepository { get; set; }
    }
}
