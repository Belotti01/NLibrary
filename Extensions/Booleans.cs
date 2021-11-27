using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL.Extensions {
    public static class Booleans {

        public static string ToString(this bool value, string ifTrue, string ifFalse)
            => value ? ifTrue : ifFalse;

    }
}
