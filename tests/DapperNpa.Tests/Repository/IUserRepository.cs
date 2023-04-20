using System;
using DapperNpa.Attributes;
using DapperNpa.Tests.Model;

namespace DapperNpa.Tests.Repository
{
    [Repository]
    public interface IUserRepository
    {
        [Query(sql: "SELECT * FROM users WHERE id = @id")]
        User GetById(Guid id);
    }
}