using System;
using System.Collections.Generic;
using System.Text;

namespace NL.Exceptions {
    public class InvalidCollectionException : Exception {
        public InvalidCollectionException(string collectionName)
            : base($"No collection was found with the name \"{collectionName}\".") { }
    }
}
