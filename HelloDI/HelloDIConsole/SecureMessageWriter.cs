using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Ploeh.Samples.HelloDI.CommandLine
{
    public class SecureMessageWriter : IMessageWriter
    {
        private readonly IMessageWriter writer;

        public SecureMessageWriter(IMessageWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            this.writer = writer;
        }

        public void Write(string message)
        {
            if (Thread.CurrentPrincipal.Identity
                .IsAuthenticated)
            {
                this.writer.Write(message);
            }
        }
    }
}
