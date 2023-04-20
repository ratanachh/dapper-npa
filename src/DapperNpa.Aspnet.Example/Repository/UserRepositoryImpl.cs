﻿using DapperNpa.Aspnet.Example.Model;

namespace DapperNpa.Aspnet.Example.Repository
{
    public class UserRepositoryImpl : IUserRepository
    {
        public User GetById(Guid id)
        {
            return new User { Id = id };
        }
    }
}
