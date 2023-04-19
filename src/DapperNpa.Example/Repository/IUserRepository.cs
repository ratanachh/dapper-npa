using DapperNpa.Attributes;
using DapperNpa.Example.Model;

namespace DapperNpa.Example.Repository
{
    [Repository]
    public interface IUserRepository
    {
        [Query(sql: "SELECT * FROM users WHERE id = @id")]
        User GetById(Guid id);
    }
}