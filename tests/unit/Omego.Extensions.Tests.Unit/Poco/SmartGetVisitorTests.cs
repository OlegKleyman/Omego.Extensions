namespace Omego.Extensions.Tests.Unit.Poco
{
    using System;
    using System.Collections;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Omego.Extensions.Poco;

    using Xunit;

    [CLSCompliant(false)]
    public class SmartGetVisitorTests
    {
        public class SmartGetVisitorTestsTheories
        {
            public static IEnumerable OnNullShouldSetNullQualifiedNameToNullForPropertiesTheory = new object[]
                                                                                                     {
                                                                                                         new object[]
                                                                                                             {
                                                                                                                 new Test
                                                                                                                     {
                                                                                                                         Test1
                                                                                                                             =
                                                                                                                             new Test1
                                                                                                                                 {
                                                                                                                                     Test2
                                                                                                                                         =
                                                                                                                                         new Test2
                                                                                                                                         (
                                                                                                                                         )
                                                                                                                                 }
                                                                                                                     },
                                                                                                                 (Expression<Func<Test, object>>)(test => test.Test1.Test2.TestString),
                                                                                                                 "Test1.Test2.TestString"
                                                                                                             },
                                                                                                         new object[]
                                                                                                             {
                                                                                                                 new Test
                                                                                                                     {
                                                                                                                         Test1
                                                                                                                             =
                                                                                                                             new Test1()
                                                                                                                     },
                                                                                                                 (Expression<Func<Test, object>>)(test => test.Test1.Test2.TestString),
                                                                                                                 "Test1.Test2"
                                                                                                             }
                                                                                                     };
        }

        [Theory]
        [MemberData("OnNullShouldSetNullQualifiedNameToNullForPropertiesTheory", MemberType = typeof(SmartGetVisitorTestsTheories))]
        public void OnNullShouldSetNullQualifiedNameToNullForProperties(Test target, Expression expression, string expected)
        {
            var visitor = GetVisitor(target);
            
            visitor.OnNull(expression, qualifiedName => qualifiedName.ShouldBeEquivalentTo(expected));
        }

        private SmartGetVisitor GetVisitor(object target)
        {
            return new SmartGetVisitor(target);
        }

        public class Test
        {
            internal Test1 Test1 { get; set; }
        }

        public class Test1
        {
            internal Test2 Test2 { get; set; }
        }

        public class Test2
        {
            internal string TestString { get; set; }
        }
    }
}
