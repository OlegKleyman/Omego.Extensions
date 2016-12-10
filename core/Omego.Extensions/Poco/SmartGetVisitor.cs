namespace Omego.Extensions.Poco
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public class SmartGetVisitor : ExpressionVisitor
    {
        private object target;

        private readonly Queue<string> nameQueue;

        public SmartGetVisitor(object target)
        {
            this.target = target;
            nameQueue = new Queue<string>();
        }
        
        protected override Expression VisitMember(MemberExpression node)
        {
            base.VisitMember(node);

            if (target == null)
            {
                return node;
            }

            var member = (PropertyInfo)node.Member;

            target = member.GetValue(target, null);
            nameQueue.Enqueue(node.Member.Name);

            return node;
        }

        public void OnNull(Expression expression, Action<string> onNullCallBack)
        {
            Visit(expression);

            if (nameQueue.Count > 0)
            {
                onNullCallBack(string.Join(".", nameQueue));
            }
        }
    }
}