using NL.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NL.Diagnostics {

    /// <summary>
    ///		Can be used to precisely time single or multiple iterations
    ///		of a <see cref="Delegate"/> and get the first, average and last
    ///		execution <see cref="TimeSpan"/>.
    /// </summary>
    public class ActionTimer {

        /// <summary>
        ///		The duration of the first iteration of the <see cref="_action"/>.
        /// </summary>
        public TimeSpan FirstElapsed { get; private set; }
        /// <summary>
        ///		The duration of the last iteration of the <see cref="_action"/>.
        /// </summary>
        public TimeSpan LastElapsed { get; private set; }

        /// <summary>
        ///		The average duration of all executed iterations of the 
        ///		<see cref="_action"/>.
        /// </summary>
        public TimeSpan Average
            => new(_elapsed.Sum(ts => ts.Ticks) / _elapsed.Count);
        /// <summary>
        ///		Is <see langword="false"/> only if the <see cref="_action"/> has been
        ///		last set to <see langword="null"/>, which causes <see cref="NullReferenceException"/>
        ///		when attempting to <see cref="Run()"/> this object.
        /// </summary>
        public bool IsActionSet
            => _action is not null;

        protected object[] _args;
        protected Delegate _action;
        protected readonly List<TimeSpan> _elapsed = new();



        /// <summary>
        ///		Create an instance of <see cref="ActionTimer"/>, set the <see cref="_action"/>
        ///		to be timed, and run it once.
        /// </summary>
        /// <param name="args">
        ///		The parameters to be passed to the <paramref name="action"/> when invoked.
        /// </param>
        public ActionTimer(Delegate action, params object[] args) {
            this._action = action;
            this._args = args;
            Reset();
        }



        /// <summary>
        ///		Update the <see cref="Delegate"/>, reset the current data and try to 
        ///		run the <paramref name="action"/> once to update the execution times.
        /// </summary>
        /// <param name="action">
        ///		The <see cref="Delegate"/> to time the execution of.
        /// </param>
        /// <param name="args">
        ///		The parameters to pass to the <paramref name="action"/> when running.
        /// </param>
        public void SetAction(Delegate action, params object[] args) {
            this._action = action;
            this._args = args;
            Reset();
        }

        /// <summary>
        ///		Run and get the exeuction time of the <see cref="_action"/> once.
        /// </summary>
        /// <returns>
        ///		The execution time of the <see cref="_action"/> in this iteration.
        /// </returns>
        public TimeSpan Run() {
            if (!IsActionSet)
                throw new NullDelegateException(nameof(_action));

            LastElapsed = Time();
            _elapsed.Add(LastElapsed);
            return LastElapsed;
        }

        /// <inheritdoc cref="Run()"/>
        /// <param name="parameters">
        ///		The parameters to be passed to the <see cref="Delegate"/> when
        ///		running it.
        /// </param>
        public TimeSpan Run(params object[] parameters) {
            if (!IsActionSet)
                throw new NullDelegateException(nameof(_action));

            LastElapsed = Time(parameters);
            _elapsed.Add(LastElapsed);
            return LastElapsed;
        }

        /// <summary>
        ///		Run the <see cref="_action"/> and get its execution time
        ///		<paramref name="iterations"/> times.
        /// </summary>
        /// <param name="iterations">
        ///		The amount of times the <see cref="Delegate"/> should be executed and timed.
        ///	</param>
        /// <returns>
        ///		The average execution time of the iterations run in this call.
        /// </returns>
        public TimeSpan Run(int iterations) {
            long averageTicks;
            TimeSpan[] times = new TimeSpan[iterations];

            if (!IsActionSet)
                throw new NullDelegateException(nameof(_action));

            for (int i = 0; i < iterations; i++) {
                times[i] = Time();
            }

            LastElapsed = times[^1];
            averageTicks = times.Sum(ts => ts.Ticks) / iterations;
            _elapsed.AddRange(times);
            return new TimeSpan(averageTicks);
        }

        /// <inheritdoc cref="Run(int)"/>
        /// <param name="parameters">
        ///		The parameters to be passed to the <see cref="Delegate"/> when
        ///		running it.
        /// </param>
        public TimeSpan Run(int iterations, params object[] parameters) {
            long averageTicks;
            TimeSpan[] times = new TimeSpan[iterations];

            if (!IsActionSet)
                throw new NullDelegateException(nameof(_action));

            for (int i = 0; i < iterations; i++) {
                times[i] = Time(parameters);
            }

            LastElapsed = times[^1];
            averageTicks = times.Sum(ts => ts.Ticks) / iterations;
            _elapsed.AddRange(times);
            return new TimeSpan(averageTicks);
        }

        /// <summary>
        ///		Clear all stored execution times but keep the <see cref="_action"/> intact,
        ///		and run the <see cref="_action"/> once if possible, otherwise sets all times
        ///		to <see langword="default"/>(<see cref="TimeSpan"/>).
        /// </summary>
        public void Reset() {
            _elapsed.Clear();
            try {
                FirstElapsed = Run();
            } catch {
                FirstElapsed = default;
                LastElapsed = default;
            }
        }

        private TimeSpan Time() {
            Stopwatch stopwatch = new();

            if (_args is null || _args.Length == 0) {
                stopwatch.Start();
                _ = _action.DynamicInvoke();
                stopwatch.Stop();
            } else {
                stopwatch.Start();
                _ = _action.DynamicInvoke(_args);
                stopwatch.Stop();
            }

            return stopwatch.Elapsed;
        }

        private TimeSpan Time(params object[] parameters) {
            Stopwatch stopwatch = new();

            stopwatch.Start();
            _ = _action.DynamicInvoke(parameters);
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }
    }

}
