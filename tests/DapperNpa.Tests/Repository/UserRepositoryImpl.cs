using System;
using DapperNpa.Tests.Model;

namespace DapperNpa.Tests.Repository
{
    public class UserRepositoryImpl : IUserRepository
    {
        public User GetById(Guid id)
        {
            return new User { Id = id };
        }
    }
}
