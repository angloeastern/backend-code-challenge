using AEBackend;

namespace AEBackend.Repositories;

public interface IUserRepository
{
  Task CreateUser(User user);
  Task<List<User>> GetAllUsers();
}