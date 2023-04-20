using DapperNpa.Aspnet.Example.Model;
using DapperNpa.Attributes;

namespace DapperNpa.Aspnet.Example.Repository
{
    [Repository]
    public interface IUserRepository
    {
        [Query(sql: "SELECT * FROM users WHERE id = @id")]
        User GetById(Guid id);
    }
}