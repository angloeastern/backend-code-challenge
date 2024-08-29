using AEBackend;
using AEBackend.DomainModels;

namespace AEBackend.Repositories;

public interface IUserRepository
{
  Task<User> CreateUser(User user, string role);
  Task<List<User>> GetAllUsers();
  Task<User?> GetUserById(string id);
  Task<List<Ship>> GetUserShips(string userId);
  Task<User> UpdateUserShips(User existingUser, string[] shipdIds);
}