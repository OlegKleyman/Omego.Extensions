namespace Omego.Extensions.Poco
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Represents a smart get listener.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    public class SmartGetVisitor : ExpressionVisitor
    {
        public object Current { get; set; }

        private readonly Queue<string> nameQueue;

        private static readonly NotSupportedException NotSupportedNodeException =
            new NotSupportedException("Only non-default properties and fields fields are supported.");

        public SmartGetVisitor(object current)
        {
            Current = current;
            nameQueue = new Queue<string>();
        }
        
        protected override Expression VisitBinary(BinaryExpression node)
        {
            base.VisitBinary(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            base.VisitBlock(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            base.VisitConditional(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            base.VisitConstant(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            base.VisitDebugInfo(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            base.VisitDynamic(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            base.VisitDefault(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitExtension(Expression node)
        {
            base.VisitExtension(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            base.VisitGoto(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            base.VisitInvocation(node);
            throw NotSupportedNodeException;
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            base.VisitLabelTarget(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            base.VisitLabel(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            base.VisitLoop(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            base.VisitMember(node);

            if (Current == null)
            {
                return node;
            }

            var propertyInfo = node.Member as PropertyInfo;
            var fieldInfo = node.Member as FieldInfo;

            if (propertyInfo != null || fieldInfo != null)
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

        protected override Expression VisitIndex(IndexExpression node)
        {
            base.VisitIndex(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            base.VisitMethodCall(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            base.VisitNewArray(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            base.VisitNew(node);
            throw NotSupportedNodeException;
        }
        
        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            base.VisitRuntimeVariables(node);
            throw NotSupportedNodeException;
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            base.VisitSwitchCase(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            base.VisitSwitch(node);
            throw NotSupportedNodeException;
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            base.VisitCatchBlock(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitTry(TryExpression node)
        {
            base.VisitTry(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            base.VisitTypeBinary(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            base.VisitUnary(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            base.VisitMemberInit(node);
            throw NotSupportedNodeException;
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            base.VisitListInit(node);
            throw NotSupportedNodeException;
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            base.VisitElementInit(node);
            throw NotSupportedNodeException;
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            base.VisitMemberBinding(node);
            throw NotSupportedNodeException;
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            base.VisitMemberAssignment(node);
            throw NotSupportedNodeException;
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            base.VisitMemberMemberBinding(node);
            throw NotSupportedNodeException;
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            base.VisitMemberListBinding(node);
            throw NotSupportedNodeException;
        }

        public void OnNull(Expression expression, Action<string> onNullCallBack)
        {
            Visit(expression);

            if (nameQueue.Count > 0 && Current == null)
            {
                onNullCallBack(string.Join(".", nameQueue));
            }
        }
    }
}