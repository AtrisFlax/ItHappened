namespace ItHappened.Domain
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        User TryFindByLogin(string login);
    }
}