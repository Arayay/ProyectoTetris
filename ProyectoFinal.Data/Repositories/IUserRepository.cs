using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Data.Repositories
{
    public interface IUserRepository
    {
        Users GetById(int userId);
        Users GetByUsername(string username);
        IEnumerable<Users> GetAll();
        void Add(Users user);
        void Update(Users user);
        void Save();
    }
}
