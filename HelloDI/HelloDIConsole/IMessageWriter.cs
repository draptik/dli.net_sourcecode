using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.HelloDI.CommandLine
{
    public interface IMessageWriter
    {
        void Write(string message);
    }
}
