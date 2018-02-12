using System;

namespace Omego.Extensions.DbContextExtensions
{
    /// <summary>
    ///     Contains extension methods for <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    /// </summary>
    public static class DbContext
    {
        /// <summary>
        /// Adds entities to context and saves.
        /// </summary>
        /// <param name="context">The <see cref="Microsoft.EntityFrameworkCore.DbContext"/> to add
        /// entities to and use to save to the database.</param>
        /// <param name="entities">The entities to add and save.</param>
        /// <returns>The number of objects written to the underlying database.s</returns>
        public static int AddRangeAndSave(this Microsoft.EntityFrameworkCore.DbContext context,
            params object[] entities)
        {
            (context ?? throw new ArgumentNullException(nameof(context))).AddRange(entities);

            return context.SaveChanges();
        }
    }
}