using System;

namespace Omego.Extensions.DbContextExtensions
{
    public static class DbContext
    {
        public static int AddRangeAndSave(this Microsoft.EntityFrameworkCore.DbContext context,
            params object[] entities)
        {
            (context ?? throw new ArgumentNullException(nameof(context))).AddRange(entities);

            return context.SaveChanges();
        }
    }
}