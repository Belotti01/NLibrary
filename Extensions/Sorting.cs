using System;
using System.Collections.Generic;
using System.Linq;

namespace NL.Extensions {
	public static class Sorting {

		#region LinqParallelSort

		public static IEnumerable<T> ParallelSort<T, TKey>(this IEnumerable<T> collection, Func<T, TKey> keySelector) {
            return collection
                .AsParallel()
                .OrderBy(keySelector)
                .AsEnumerable();
        }

        public static IEnumerable<T> ParallelSort<T>(this IEnumerable<T> collection) {
            return collection
                .AsParallel()
                .OrderBy(x => x)
                .AsEnumerable();
        }

		#endregion


		#region BubbleSort

		public static IEnumerable<T> BubbleSort<T>(this IEnumerable<T> collection) where T : IComparable<T>
            => collection.BubbleSort(Comparer<T>.Default);

        public static IEnumerable<T> BubbleSort<T>(this IEnumerable<T> collection, IComparer<T> comparer) {
            T[] items = collection.ToArray();

            bool itemMoved;
            do {
                itemMoved = false;
                for(int i = 0; i < items.Count() - 1; i++) {
                    if(comparer.Compare(items[i], items[i + 1]) > 0) {
                        var lowerValue = items[i + 1];
                        items[i + 1] = items[i];
                        items[i] = lowerValue;
                        itemMoved = true;
                    }
                }
            } while(itemMoved);

            return items;
        }

        #endregion

        #region SelectionSort

        public static void SelectionSort<T>(this IEnumerable<T> collection) where T : IComparable<T>
            => collection.SelectionSort(Comparer<T>.Default);

        public static IEnumerable<T> SelectionSort<T>(this IEnumerable<T> collection, IComparer<T> comparer) {
            T[] items = collection.ToArray();
            
            for(int i = 0; i < items.Length; i++) {
                int min = i;
                for(int j = i + 1; j < items.Length; j++) {
                    if(comparer.Compare(items[min], items[j]) > 0) {
                        min = j;
                    }
                }

                if(min != i) {
                    T item = items[min];
                    items[min] = items[i];
                    items[i] = item;
                }
            }

            return items;
        }

		#endregion

		#region InsertionSort

		public static IEnumerable<T> InsertionSort<T>(this IEnumerable<T> collection) where T : IComparable<T>
            => collection.InsertionSort(Comparer<T>.Default);

        public static IEnumerable<T> InsertionSort<T>(this IEnumerable<T> collection, IComparer<T> comparer) {
            T[] items = collection.ToArray();

            for(int i = 0; i < items.Count(); i++) {
                var item = items[i];
                var index = i;

                while(index > 0 && comparer.Compare(items[index - 1], item) > 0) {
                    items[index] = items[index - 1];
                    index--;
                }

                items[index] = item;
            }

            return items;
        }

		#endregion
	}
}
