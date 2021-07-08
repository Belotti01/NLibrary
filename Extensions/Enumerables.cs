using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NL.Extensions {
    public static class Enumerables {

        public static IEnumerable<T> RemoveDefaults<T>(this IEnumerable<T> collection) {
            return collection
                .Where(item => !item.IsDefault());
        }

    }
}
