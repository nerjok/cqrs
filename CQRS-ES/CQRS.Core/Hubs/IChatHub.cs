namespace CQRS.Core.Hubs
{
    public interface IChatHub
    {
        public Task SendMessage(string message);
    }
}