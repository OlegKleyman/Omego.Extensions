using Microsoft.EntityFrameworkCore;

namespace Omego.Extensions.Tests.Unit.DbContextExtensions.Support
{
    public class MockContext : DbContext
    {
        public MockContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<MockEntity> MockEntities { get; set; }
    }
}