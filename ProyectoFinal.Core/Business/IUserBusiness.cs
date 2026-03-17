using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinal.Data;

namespace ProyectoFinal.Core.Business
{
    public interface IUserBusiness
    {
        Users ValidateUser(string username, string password);
        bool RegisterUser(string username, string password, string email);
        Users GetUserById(int userId);
    }
}
