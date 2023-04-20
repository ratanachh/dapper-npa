using DapperNpa.Tests.Repository;
using Xunit;

namespace DapperNpa.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var user = new UserRepositoryImpl();
    }
}