using DapperNpa.Aspnet.Example.Model;
using DapperNpa.Attributes;

namespace DapperNpa.Aspnet.Example.Repository
{
    [Repository]
    public interface IUserRepository
    {
        [Query(sql: "SELECT * FROM users WHERE id = @id and name = @name")]
        public User GetById(Guid id, string name);

        [Query(sql: "INSERT INTO users (id, name) VALUES(@id, @name);")]
        public void Insert(User user);

        [Query(sql: "DELETE FROM users WHERE id = @id")]
        public bool Delete(Guid id);

        [Query(sql: "UPDAT users set name = @name WHERE id = @id")]
        public bool Update(Guid id, string name);

    }
}