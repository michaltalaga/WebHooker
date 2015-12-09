namespace WebHookerClient
{
    public interface IClient
    {
        void Registered(string url);
        void Forward(Request request);
    }
}
