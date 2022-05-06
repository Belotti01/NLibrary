namespace NL.Exceptions {
	public class DuplicateItemException : Exception {

		public DuplicateItemException(string parameterName, string duplicateValue)
			: base($"The parameter \"{parameterName}\" is invalid. Its value \"{duplicateValue}\" is already present in the target collection.") { }

		public DuplicateItemException(string message)
			: base(message) { }
	}
}
