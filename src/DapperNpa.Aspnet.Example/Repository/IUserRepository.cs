using DapperNpa.Aspnet.Example.Model;
using DapperNpa.Attributes;

namespace DapperNpa.Aspnet.Example.Repository
{
    [Repository]
    public interface IUserRepository
    {
        [Query(sql: "SELECT * FROM users WHERE id = @id")]
        public User GetById(Guid id);

        [Query(sql: "INSERT INTO users (id, name) VALUES(@id, @name);")]
        public void Insert(User user);
    }
}