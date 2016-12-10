namespace Omego.Extensions.Tests.Unit.Poco
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using FluentAssertions;

    using NSubstitute;

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
                                                                                                             },
                                                                                                         new object[]
                                                                                                             {
                                                                                                                 new Test
                                                                                                                     {
                                                                                                                         Test1
                                                                                                                             =
                                                                                                                             new Test1()
                                                                                                                     },
                                                                                                                 (Expression<Func<Test, object>>)(test => test.Test1.Test2Field),
                                                                                                                 "Test1.Test2Field"
                                                                                                             }
                                                                                                     };

            public static IEnumerable OnNullShouldSetCurrentObjectTheory = new object[]
                                                                               {
                                                                                   new object[]
                                                                                       {
                                                                                           new Test
                                                                                               {
                                                                                                   Test1 =
                                                                                                       new Test1
                                                                                                           {
                                                                                                               Test2 =
                                                                                                                   new Test2
                                                                                                                       {
                                                                                                                           TestString
                                                                                                                               =
                                                                                                                               "testing2"
                                                                                                                       }
                                                                                                           }
                                                                                               },
                                                                                           (
                                                                                           Expression
                                                                                           <Func<Test, object>>)
                                                                                           (test =>
                                                                                               test.Test1.Test2
                                                                                                   .TestString),
                                                                                           "testing2"
                                                                                       }
                                                                               };
        }

        [Theory]
        [MemberData("OnNullShouldSetNullQualifiedNameToNullForPropertiesTheory", MemberType = typeof(SmartGetVisitorTestsTheories))]
        public void OnNullShouldSetNullQualifiedNameToNullForProperties(Test target, Expression expression, string expected)
        {
            var visitor = GetVisitor(target);

            var handler = Substitute.For<Action<string>>();

            visitor.OnNull(expression, handler);

            handler.Received(1)(Arg.Is(expected));
        }

        [Theory]
        [MemberData("OnNullShouldSetCurrentObjectTheory", MemberType = typeof(SmartGetVisitorTestsTheories))]
        public void OnNullShouldSetCurrentObject(Test target, Expression expression, string expected)
        {
            var visitor = GetVisitor(target);

            visitor.OnNull(expression, null);

            visitor.Current.ShouldBeEquivalentTo(expected);
        }

        private SmartGetVisitor GetVisitor(object target) => new SmartGetVisitor(target);

        public class Test
        {
            internal Test1 Test1 { get; set; }
        }

        public class Test1
        {
            public Test2 Test2Field;

            internal Test2 Test2 { get; set; }

            public Test2 GetTest2() => new Test2();
        }

        public class Test2
        {
            internal string TestString { get; set; }
        }
    }
}
