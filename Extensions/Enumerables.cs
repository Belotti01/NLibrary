namespace NLCommon.Extensions {

	public static class Enumerables {

		/// <summary>
		///     Remove all <see langword="default"/> and <see langword="null"/> values
		///     from the <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <returns>
		///     A copy of the <see cref="IEnumerable{T}"/>, without <see langword="default"/> 
		///     and <see langword="null"/> values
		/// </returns>
		public static IEnumerable<T> RemoveDefaults<T>(this IEnumerable<T> collection) {
			return collection
				.Where(item => !item.IsNullOrDefault());
		}

		/// <summary>
		///     Remove all instances of <see langword="null"/> and <see cref="string.Empty"/>
		///     from the <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <returns>
		///     A copy of the <see cref="IEnumerable{T}"/>, without <see langword="null"/> 
		///     and <see cref="string.Empty"/> values
		/// </returns>
		public static IEnumerable<string> RemoveDefaults(this IEnumerable<string> collection) {
			return collection
				.Where(item => !item.IsNullOrDefault() && item != string.Empty);
		}

		/// <summary>
		///     Remove all instances of <see langword="null"/>, <see cref="string.Empty"/>,
		///     and whitespace-only <see langword="string"/>s from the <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <returns>
		///     A copy of the <see cref="IEnumerable{T}"/>, without <see langword="null"/>, 
		///     <see cref="string.Empty"/> and whitespace-only <see langword="string"/> values
		/// </returns>
		public static IEnumerable<string> RemoveDefaultsAndWhitespace(this IEnumerable<string> collection) {
			return collection
				.Where(item => !(item.IsNullOrDefault() || item.IsEmpty()));
		}

		/// <summary>
		///     Execute the <see cref="string.Trim(char[])"/> method on all <see langword="string"/>s
		///     in the <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <param name="trimChars">
		///     The <see langword="char"/>s to trim from the <see langword="string"/>s.
		/// </param>
		/// <returns>
		///     A copy of the <see cref="IEnumerable{T}"/> in which the <see cref="string.Trim(char[])"/>
		///     method is executed for every present <see langword="string"/>.
		/// </returns>
		public static IEnumerable<string> TrimAll(this IEnumerable<string> collection, params char[] trimChars) {
			return collection.Select(s => s is null ? s : s.Trim(trimChars));
		}

		public static int FirstIndexWhere<T>(this IEnumerable<T> collection, Func<T, bool> predicate) {
			int index = -1;
			foreach(T item in collection) {
				index++;
				if(predicate.Invoke(item)) {
					return index;
				}
			}
			return -1;
		}

		public static IEnumerable<int> IndexesWhere<T>(this IEnumerable<T> collection, Func<T, bool> predicate) {
			int index = -1;

			foreach(T item in collection) {
				index++;
				if(predicate.Invoke(item)) {
					yield return index;
				}
			}
		}

		public static IEnumerable<T> Swap<T>(this IEnumerable<T> collection, int index1, int index2) {
			T[] items = collection.ToArray();
			T item = items[index1];
			items[index1] = items[index2];
			items[index2] = item;
			return items;
		}

		public static Dictionary<T, int> CountDinstinct<T>(this IEnumerable<T> collection, Func<T, bool> predicate = default) {
			Dictionary<T, int> counts = new();
			int count;

			foreach(T item in collection.Distinct()) {
				count = collection.Count(predicate ?? new(x => x.Equals(item)));
				counts.Add(item, count);
			}

			return counts;
		}
	}

}
