using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Linq.Expressions;

namespace Ploeh.Samples.Menu.Mef
{
    public class MefAdapter<T> where T : new()
    {
        private readonly T export;

        public MefAdapter()
        {
            this.export = new T();
        }

        [Export]
        public T Export
        {
            get { return this.export; }
        }
    }

    public class MefAdapter<T, TCtorArg1, TCtorArg2>
    {
        private readonly static Func<TCtorArg1, TCtorArg2, T> createExport = CreateFactory();
        private readonly T export;

        [ImportingConstructor]
        public MefAdapter(TCtorArg1 arg1, TCtorArg2 arg2)
        {
            if (arg1 == null)
            {
                throw new ArgumentNullException("arg1");
            }
            if (arg2 == null)
            {
                throw new ArgumentNullException("arg2");
            }

            this.export = createExport(arg1, arg2);
        }

        [Export]
        public T Export
        {
            get { return this.export; }
        }

        private static Func<TCtorArg1, TCtorArg2, T> CreateFactory()
        {
            var arg1Exp = Expression.Parameter(typeof(TCtorArg1), "arg1");
            var arg2Exp = Expression.Parameter(typeof(TCtorArg2), "arg2");

            var ctorInfo = typeof(T).GetConstructor(new[] { typeof(TCtorArg1), typeof(TCtorArg2) });
            var ctorExp = Expression.New(ctorInfo, arg1Exp, arg2Exp);

            var factory = Expression.Lambda<Func<TCtorArg1, TCtorArg2, T>>(ctorExp, arg1Exp, arg2Exp).Compile();
            return factory;
        }
    }
}
