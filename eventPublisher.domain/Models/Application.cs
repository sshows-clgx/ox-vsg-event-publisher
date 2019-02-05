namespace eventPublisher.domain.models
{
    public class Application
    {
        private long _id;
        private string _name;

        public Application(long id, string name)
        {
            _id = id;
            _name = name;
        }
    }
}