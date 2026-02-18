using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace APX.Extra.Misc
{
    public class WeakDelegate<TDelegate> : AWeakDelegate where TDelegate : class
    {
        private readonly System.WeakReference _object;
        private readonly MethodInfo _method;

        static WeakDelegate()
        {
            if (!typeof(TDelegate).IsSubclassOf(typeof(System.Delegate)))
                throw new System.InvalidOperationException(typeof(TDelegate).Name + " is not of type Delegate");
        }

        public WeakDelegate(TDelegate action)
        {
            var dl = action as System.Delegate;

            if (dl.Method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length > 0)
                throw new System.InvalidOperationException("Delegate must not have its own closure, and must be a class-method");

            _object = new System.WeakReference(dl.Target, false);
            _method = dl.Method;
        }

        public static TDelegate Wrap(TDelegate action, System.Action ifDisposed) { return AWeakDelegate.Wrap(action, ifDisposed); }

        public TDelegate Invoke
        {
            get
            {
                if (!IsAlive)
                    throw new System.ObjectDisposedException("Weak Target");
                return System.Delegate.CreateDelegate(typeof(TDelegate), _object.Target, _method) as TDelegate;
            }
        }

        public bool IsAlive => _object.IsAlive;

        public bool IsSameAs(TDelegate other)
        {
            if (!IsAlive) return false;

            var otherDelegate = other as System.Delegate;
            return ReferenceEquals(_object.Target, otherDelegate.Target) && _method == otherDelegate.Method;
        }

        public bool IsSameAs(System.Delegate other) { return other is TDelegate targetDelegate && IsSameAs(targetDelegate); }
    }

    public abstract class AWeakDelegate
    {
        public static TDelegate Wrap<TDelegate>(TDelegate method, System.Action ifDisposed)
            where TDelegate : class
        {
            //Verify delegate is a delegate
            if (!typeof(TDelegate).IsSubclassOf(typeof(System.Delegate)))
                throw new System.InvalidOperationException(typeof(TDelegate).Name + " is not of type Delegate");

            //Verify delegate is not weak itself (eg, must be class method)
            var realDelegate = method as System.Delegate;
            if (realDelegate.Method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Length > 0)
                throw new System.InvalidOperationException("Delegate must not have its own closure, and must be a class-method");

            //Create wrap to delegate type that takes 'this' as a parameter
            var parameters = realDelegate.Method.GetParameters();
            ParameterExpression[] args = new ParameterExpression[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                args[i] = Expression.Parameter(parameters[i].ParameterType, parameters[i].Name);
            }

            //Create a weak reference and resolving target
            var expWeakRef = Expression.Constant(new System.WeakReference(realDelegate.Target, false));
            var expWeakTarget = Expression.Convert(Expression.Property(expWeakRef, "Target"), realDelegate.Method.DeclaringType);

            //Create a forked expression to evaluate the weak method
            var expInvokeFork =
                Expression.Condition(
                    Expression.NotEqual(expWeakTarget, Expression.Constant(null)),
                    Expression.Call(expWeakTarget, realDelegate.Method, args),
                    Expression.Block(
                        Expression.Invoke(Expression.Constant(ifDisposed)),
                        Expression.Default(realDelegate.Method.ReturnType)
                    )
                );

            return Expression.Lambda(typeof(TDelegate), expInvokeFork, args).Compile() as TDelegate;
        }
    }
}
