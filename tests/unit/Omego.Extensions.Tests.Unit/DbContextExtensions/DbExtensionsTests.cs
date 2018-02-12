using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Omego.Extensions.DbContextExtensions;
using Omego.Extensions.Tests.Unit.DbContextExtensions.Support;
using Xunit;
using Context = Microsoft.EntityFrameworkCore.DbContext;

namespace Omego.Extensions.Tests.Unit.DbContextExtensions
{
    public class DbExtensionsTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddRangeAndSaveShouldReturnTheNumberOfObjectsWrittenToDatabase(int numberOfObjects)
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new MockContext(options))
            {
                var objects = new List<object>();

                for (var i = 0; i < numberOfObjects; i++) objects.Add(new MockEntity());

                context.AddRangeAndSave(objects.ToArray()).ShouldBeEquivalentTo(numberOfObjects);
            }
        }

        [Fact]
        public void AddRangeAndSaveShouldSaveEntityToDataStore()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new MockContext(options))
            {
                context.AddRangeAndSave(new MockEntity {Test = "testing123"});
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