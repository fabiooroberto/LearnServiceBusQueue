namespace LearnServiceBusQueue.Model
{
    public class PocSampleQueue
    {
        public PocSampleQueue(
            string email,
            string name,
            Guid idRepository,
            Guid idResource,
            Guid idProject)
        {
            Email = email;
            Name = name;
            IdRepository = idRepository;
            IdResource = idResource;
            IdProject = idProject;
        }

        public string Email { get; init; }
        public string Name { get; init; }
        public Guid IdRepository { get; init; }
        public Guid IdResource { get; init; }
        public Guid IdProject { get; init; }

        public static PocSampleQueue Mock()
        {
            return new PocSampleQueue(
                email: "teste@teste.com",
                name: $"Lorem Teste {new Random().Next(0, 100)}",
                idRepository: Guid.NewGuid(),
                idResource: Guid.NewGuid(),
                idProject: Guid.NewGuid());
        }
    }
}