using ItHappened.Domain;

namespace ItHappened.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Register(string login, string password)
        {
            throw new System.NotImplementedException();
        }

        public User TryFindByLogin(string login)
        {
            var user = _userRepository.TryFindByLogin(login);
            return user;
        }
    }
}