using System;
using System.Threading.Tasks;

namespace NL.Extensions {

    public static class Tasks {

        /// <summary>
        ///     Resolve a <see cref="Task"/> synchronously.
        /// </summary>
        public static void Resolve(this Task task)
            => task.GetAwaiter().GetResult();

        /// <inheritdoc cref="Resolve(Task)"/>
        public static T Resolve<T>(this Task<T> task)
            => task.GetAwaiter().GetResult();

        /// <summary>
        ///     Ignore any <see cref="Exception"/> thrown by this <see cref="Task"/>.
        /// </summary>
        public static async Task IgnoreExceptions(this Task task) {
            try {
                task.Start();
                await task;
            } catch { }
        }

        /// <summary>
        ///     Ignore any <see cref="Exception"/> thrown by this <see cref="Task"/>.
        /// </summary>
        /// <returns>
        ///     The result of the <see cref="Task"/> if no <see cref="Exception"/> is thrown,
        ///     <see langword="default"/> otherwise.
        /// </returns>
        public static async Task<T> IgnoreExceptions<T>(this Task<T> task) {
            try {
                task.Start();
                return await task;
            } catch {
                return default;
            }
        }
    }

}
