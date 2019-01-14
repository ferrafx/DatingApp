using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Model;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T:class;
         Task<bool> SaveAll(); //if changes return true if not or problem return false
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);
    }
}