using AEBackend;
using AEBackend.DomainModels;

namespace AEBackend.Repositories;

public interface IUserRepository
{
  Task<User> CreateUser(User user, string role);
  Task<List<User>> GetAllUsers();
  Task<User?> GetUserById(string id);
  Task<User> UpdateUserShips(User existingUser, string[] shipdIds);
}