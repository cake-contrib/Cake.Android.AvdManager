using Cake.Core;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cake.AndroidAvdManager.Fakes
{
    public abstract class TestFixtureBase : IDisposable
    {
        FakeCakeContext context;

        protected ICakeContext Cake
            => context.CakeContext;

        public TestFixtureBase ()
        {
            context = new FakeCakeContext ();

            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName (typeof (TestFixtureBase).Assembly.Location);
        }

        protected static string ContentPath
            => System.IO.Path.GetDirectoryName (typeof (TestFixtureBase).Assembly.Location);

        public void Dispose ()
        {
            context.DumpLogs ();
        }
    }
}
