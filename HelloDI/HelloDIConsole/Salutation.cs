using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.HelloDI.CommandLine
{
    public class Salutation
    {
        private readonly IMessageWriter writer;

        public Salutation(IMessageWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            this.writer = writer;
        }

        public void Exclaim()
        {
            this.writer.Write("Hello DI!");
        }
    }
}
