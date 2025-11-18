using Microsoft.EntityFrameworkCore;
using rise_gs;
using System;

namespace RiseTest.Helpers
{
    public static class TestDbContextFactory
    {
        public static RiseContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<RiseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new RiseContext(options);
            return context;
        }
    }
}
