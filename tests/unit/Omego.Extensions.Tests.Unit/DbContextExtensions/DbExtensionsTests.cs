using System;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Context = Microsoft.EntityFrameworkCore.DbContext;
using NSubstitute;
using Omego.Extensions.DbContextExtensions;
using Omego.Extensions.Tests.Unit.DbContextExtensions.Support;
using Xunit;

namespace Omego.Extensions.Tests.Unit.DbContextExtensions
{
    public class DbExtensionsTests
    {
        [Fact]
        public void AddRangeAndSaveShouldSaveEntityToDataStore()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new MockContext(options))
            {
                context.AddRangeAndSave(new MockEntity{Test = "testing123"});
            }

            using (var context = new MockContext(options))
            {
                context.MockEntities.Should().Contain(entity => entity.Test == "testing123");
            }
        }

        [Fact]
        public void AddRangeAndSaveShouldThrowArgumentNullExceptionWhenContextIsNull()
        {
            Action addRangeAndSave = () => ((Context) null).AddRangeAndSave(null);

            addRangeAndSave.ShouldThrow<ArgumentNullException>().Which.ParamName.ShouldBeEquivalentTo("context");
        }
    }
}
