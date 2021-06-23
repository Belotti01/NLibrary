using System;

namespace NL.Exceptions {
    public class FileAccessTimeoutException : Exception {
        public string Filepath { get; private set; }
        public int Timeout { get; private set; }

        public FileAccessTimeoutException()
            : base($"The file could not be accessed in the allotted time.") { }

        public FileAccessTimeoutException(string filepath)
            : base($"The text file '{filepath}' could not be accessed in the allotted time.") {
            Filepath = filepath;
        }

        public FileAccessTimeoutException(string filepath, int timeout)
            : base($"The text file '{filepath}' could not be accessed in the allotted time ({timeout}ms).") {
            Filepath = filepath;
            Timeout = timeout;
        }
    }
}
