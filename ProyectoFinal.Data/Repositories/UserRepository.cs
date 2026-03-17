using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ProyectoFinal.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProyectoFinalDbEntities _context;

        public UserRepository()
        {
            _context = new ProyectoFinalDbEntities();
        }

        public Users GetById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public Users GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<Users> GetAll()
        {
            return _context.Users.ToList();
        }

        public void Add(Users user)
        {
            _context.Users.Add(user);
        }

        public void Update(Users user)
        {
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

