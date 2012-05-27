using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

// Ripreso da Db4objects.Db4o.Linq.Expressions
namespace Farmacie.Common.Utility.Expressions
{
    internal class ExpressionComparison : ExpressionVisitor
    {
        private bool _areEqual = true;

        private Queue<Expression> _candidates;
        private Expression _candidate;

        public bool AreEqual
        {
            get { return _areEqual; }
        }

        public ExpressionComparison(Expression a, Expression b)
        {
            _candidates = new Queue<Expression>(new ExpressionEnumeration(b));

            Visit(a);

            if (_candidates.Count > 0) Stop();
        }

        private Expression PeekCandidate()
        {
            if (_candidates.Count == 0) return null;
            return _candidates.Peek();
        }

        private Expression PopCandidate()
        {
            return _candidates.Dequeue();
        }

        private bool CheckAreOfSameType(Expression candidate, Expression expression)
        {
            if (!CheckEqualAndStop(expression.NodeType, candidate.NodeType)) return false;
            if (!CheckEqualAndStop(expression.Type, candidate.Type)) return false;

            return true;
        }

        private void Stop()
        {
            _areEqual = false;
        }

        private T CandidateFor<T>(T original) where T : Expression
        {
            return (T)_candidate;
        }

        protected override void Visit(Expression expression)
        {
            if (expression == null) return;
            if (!AreEqual) return;

            _candidate = PeekCandidate();
            if (!CheckNotNull(_candidate)) return;
            if (!CheckAreOfSameType(_candidate, expression)) return;

            PopCandidate();

            base.Visit(expression);
        }

        protected override void VisitConstant(ConstantExpression constant)
        {
            var candidate = CandidateFor(constant);
            if (!CheckEqualAndStop(constant.Value, candidate.Value)) return;
        }

        protected override void VisitMemberAccess(MemberExpression member)
        {
            var candidate = CandidateFor(member);
            //if (!CheckEqual(member.Member, candidate.Member))
            //{
                // la funzione precedente mi torna false nel caso in cui 
                // mi sono arrivati due oggetti (anche uguali) ma con nomi diversi
                // in questo caso provo a prendermi il valore di entrambi
                // e li confronto
                object memberValue = GetMemberExpressionValue(member);
                object candidateValue = GetMemberExpressionValue(candidate);

                if (!memberValue.Equals(candidateValue))
                    return;
            //}

            // su due istanze dei servizi mi va in Stop (modificato checkequals per fare confronto nome)
            base.VisitMemberAccess(member);
        }

        protected override void VisitMethodCall(MethodCallExpression methodCall)
        {
            var candidate = CandidateFor(methodCall);
            if (!CheckEqualAndStop(methodCall.Method, candidate.Method)) return;

            base.VisitMethodCall(methodCall);
        }

        protected override void VisitParameter(ParameterExpression parameter)
        {
            var candidate = CandidateFor(parameter);
            if (!CheckEqualAndStop(parameter.Name, candidate.Name)) return;
        }

        protected override void VisitTypeIs(TypeBinaryExpression type)
        {
            var candidate = CandidateFor(type);
            if (!CheckEqualAndStop(type.TypeOperand, candidate.TypeOperand)) return;

            base.VisitTypeIs(type);
        }

        protected override void VisitBinary(BinaryExpression binary)
        {
            var candidate = CandidateFor(binary);
            if (!CheckEqualAndStop(binary.Method, candidate.Method)) return;
            if (!CheckEqualAndStop(binary.IsLifted, candidate.IsLifted)) return;
            if (!CheckEqualAndStop(binary.IsLiftedToNull, candidate.IsLiftedToNull)) return;

            base.VisitBinary(binary);
        }

        protected override void VisitUnary(UnaryExpression unary)
        {
            var candidate = CandidateFor(unary);
            if (!CheckEqualAndStop(unary.Method, candidate.Method)) return;
            if (!CheckEqualAndStop(unary.IsLifted, candidate.IsLifted)) return;
            if (!CheckEqualAndStop(unary.IsLiftedToNull, candidate.IsLiftedToNull)) return;

            base.VisitUnary(unary);
        }

        protected override void VisitNew(NewExpression nex)
        {
            var candidate = CandidateFor(nex);
            if (!CheckEqualAndStop(nex.Constructor, candidate.Constructor)) return;
            CompareList(nex.Members, candidate.Members);

            base.VisitNew(nex);
        }

        private void CompareList<T>(ReadOnlyCollection<T> collection, ReadOnlyCollection<T> candidates)
        {
            CompareList(collection, candidates, (item, candidate) => EqualityComparer<T>.Default.Equals(item, candidate));
        }

        private void CompareList<T>(ReadOnlyCollection<T> collection, ReadOnlyCollection<T> candidates, Func<T, T, bool> comparer)
        {
            if (!CheckAreOfSameSize(collection, candidates)) return;

            for (int i = 0; i < collection.Count; i++)
            {
                if (!comparer(collection[i], candidates[i]))
                {
                    Stop();
                    return;
                }
            }
        }

        private bool CheckAreOfSameSize<T>(ReadOnlyCollection<T> collection, ReadOnlyCollection<T> candidate)
        {
            return CheckEqualAndStop(collection.Count, candidate.Count);
        }

        private bool CheckNotNull<T>(T t) where T : class
        {
            if (t == null)
            {
                Stop();
                return false;
            }

            return true;
        }


        private bool CheckEqual<T>(T t, T candidate)
        {
            if (!EqualityComparer<T>.Default.Equals(t, candidate))
            {
                /*var fieldT = t as System.Reflection.FieldInfo;
                var fieldCandidate = candidate as System.Reflection.FieldInfo;

                if (fieldT != null && fieldCandidate != null)
                {
                    // Confronto tipi
                    return fieldT.FieldType == fieldT.FieldType;
                }*/
                return false;
            }

            return true;
        }

        private bool CheckEqualAndStop<T>(T t, T candidate)
        {
            if (!EqualityComparer<T>.Default.Equals(t, candidate))
            {
                if (t.ToString() != candidate.ToString())
                {
                    Stop();
                    return false;
                }
            }

            return true;
        }

        private object GetMemberExpressionValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));

            var getterLambda = Expression.Lambda<Func<object>>(objectMember);

            var getter = getterLambda.Compile();

            return getter();
        }

    }
}
