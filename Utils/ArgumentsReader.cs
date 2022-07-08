using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace NLCommon.Utils {
	public class ArgumentsReader {
		public string? this[params string[] argumentAliases] {
			get {
				foreach(var alias in argumentAliases) {
					if(Arguments.ContainsKey(alias))
						return Arguments[alias];
				}

				return null;
			}
		}

		protected Dictionary<string, string> Arguments;

		public string Prefix { get; protected init; }
		public string Divisor { get; protected init; }
		public string MainArgument;

		public ArgumentsReader(string prefix, string divisor, string args) {
			Prefix = prefix;
			Divisor = divisor;
			Read(args);
		}

		public ArgumentsReader(string prefix, string divisor, IEnumerable<string> args)
			: this(prefix, divisor, string.Join(' ', args)) { }

		[MemberNotNull(nameof(Arguments), nameof(MainArgument))]
		protected void Read(string argsString) {
			Arguments ??= new(StringComparer.OrdinalIgnoreCase);
			Arguments.Clear();
			argsString = argsString.Trim();
			bool hasMainArgument = !argsString.StartsWith(Prefix);
			string[] args = argsString.Split(Prefix);
			string[] argParts;

			if(hasMainArgument) {
				MainArgument = args[0];
				args = args.Length > 1
					? args[1..]
					: Array.Empty<string>();
			} else {
				MainArgument = string.Empty;
			}

			foreach(string arg in args) {
				argParts = arg.Split(Divisor, 2);
				Arguments.Add(argParts[0], argParts.Length == 2 ? argParts[1] : string.Empty);
			}
		}

		public bool HasArguments(string argument)
			=> Arguments.ContainsKey(argument);
	}
}
