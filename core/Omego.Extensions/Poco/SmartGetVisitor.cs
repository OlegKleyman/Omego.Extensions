namespace Omego.Extensions.Poco
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    ///     Represents a smart get listener.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    public class SmartGetVisitor : ExpressionVisitor
    {
        private static readonly NotSupportedException NotSupportedNodeException =
            new NotSupportedException("Only non-default properties and fields fields are supported.");

        private readonly Queue<string> nameQueue;

        public SmartGetVisitor(object current)
        {
            Current = current;
            nameQueue = new Queue<string>();
        }

        public object Current { get; set; }

        /// <inheritdoc />
        protected override Expression VisitBinary(BinaryExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitBlock(BlockExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitConditional(ConditionalExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitConstant(ConstantExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitDefault(DefaultExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitExtension(Expression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitGoto(GotoExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitInvocation(InvocationExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitLabel(LabelExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitLoop(LoopExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            base.VisitMember(node);

            if (Current == null) return node;

            var propertyInfo = node.Member as PropertyInfo;
            var fieldInfo = node.Member as FieldInfo;

            if ((propertyInfo != null) || (fieldInfo != null))
            {
                if (propertyInfo != null)
                {
                    var member = propertyInfo;
                    Current = member.GetValue(Current);
                }
                else
                {
                    var member = (FieldInfo)node.Member;
                    Current = member.GetValue(Current);
                }

                nameQueue.Enqueue(node.Member.Name);
            }

            return node;
        }

        /// <inheritdoc />
        protected override Expression VisitIndex(IndexExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitNew(NewExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitTry(TryExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitUnary(UnaryExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override Expression VisitListInit(ListInitExpression node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override ElementInit VisitElementInit(ElementInit node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            throw NotSupportedNodeException;
        }

        /// <inheritdoc />
        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            throw NotSupportedNodeException;
        }

        public void OnNull(Expression expression, Action<string> onNullCallBack)
        {
            Visit(expression);

            if ((nameQueue.Count > 0) && (Current == null))
            {
                if (onNullCallBack == null) throw new ArgumentNullException(nameof(onNullCallBack));
                onNullCallBack(string.Join(".", nameQueue));
            }
        }
    }
}