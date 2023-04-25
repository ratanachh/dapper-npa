using DapperNpa.Aspnet.Example.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using DapperNpa.Aspnet.Example.Model;
using Dapper;


namespace DapperNpa.Aspnet.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IDbConnection _connection;

        public HomeController(IUserRepository userRepository, IDbConnection connection)
        {
            _userRepository = userRepository;
            _connection = connection;
        }
        public User Index()
        {
            return _connection.Query<User>("SELECT 1 FROM user;").FirstOrDefault();
        }
    }
}
