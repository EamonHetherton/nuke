﻿// Copyright Matthias Koch 2017.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Core;
using Nuke.Core.BuildServers;
using Nuke.Core.Execution;
using Nuke.Core.OutputSinks;

[assembly: IconClass(typeof(Logger), "quill4")]

namespace Nuke.Core
{
    [PublicAPI]
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public static class Logger
    {
        /// <summary>
        /// Provides a logging block for better readability. The actual output is dependent on the executing environment.
        /// <ul>
        ///   <li><b>Console:</b> calls <see cref="Info(string)"/> with figlet font <em>cybermedium</em></li>
        ///   <li><b>TeamCity:</b> calls <see cref="TeamCity.OpenBlock"/> and <see cref="TeamCity.CloseBlock"/></li>
        ///   <li><b>Bitrise:</b> calls <see cref="Info(string)"/> with figlet font <em>ansi-shadow</em></li>
        /// </ul>
        /// </summary>
        /// <returns>
        /// Returns an <see cref="IDisposable"/> which will automatically end the block.
        /// </returns>
        [StringFormatMethod("format")]
        public static IDisposable Block (string text)
        {
            return OutputSink.WriteBlock(text);
        }

        #region Trace

        /// <summary>
        /// Logs a message as trace if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Trace"/>.
        /// </summary>
        [StringFormatMethod("format")]
        public static void Trace (string format, params object[] args)
        {
            Trace(string.Format(format, args));
        }

        /// <summary>
        /// Logs a message as trace if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Trace"/>.
        /// </summary>
        public static void Trace (object value)
        {
            Trace(value.ToString());
        }

        /// <summary>
        /// Logs a message as trace if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Trace"/>.
        /// </summary>
        public static void Trace (string text)
        {
            OutputSink.Trace(text);
        }

        /// <summary>
        /// Logs a message as trace if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Trace"/>.
        /// </summary>
        public static T Trace<T> (this T obj, Func<T, string> text)
        {
            Trace(text(obj));
            return obj;
        }

        #endregion
        

        #region Info

        /// <summary>
        /// Logs a message as information if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Information"/>.
        /// </summary>
        [StringFormatMethod("format")]
        public static void Info (string format, params object[] args)
        {
            Info(string.Format(format, args));
        }

        /// <summary>
        /// Logs a message as information if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Information"/>.
        /// </summary>
        public static void Info (object value)
        {
            Info(value.ToString());
        }

        /// <summary>
        /// Logs a message as information if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Information"/>.
        /// </summary>
        public static void Info (string text)
        {
            OutputSink.Info(text);
        }

        /// <summary>
        /// Logs a message as information if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Information"/>.
        /// </summary>
        public static T Info<T> (this T obj, Func<T, string> text)
        {
            Info(text(obj));
            return obj;
        }

        #endregion

        #region Warn

        /// <summary>
        /// Logs a message as warning if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Warning"/>.
        /// </summary>
        [StringFormatMethod("format")]
        public static void Warn (string format, params object[] args)
        {
            Warn(string.Format(format, args));
        }

        /// <summary>
        /// Logs a message as warning if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Warning"/>.
        /// </summary>
        public static void Warn (object value)
        {
            Warn(value.ToString());
        }

        /// <summary>
        /// Logs a message as warning if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Warning"/>.
        /// </summary>
        public static void Warn (string text)
        {
            OutputSink.Warn(text);
        }
        
        /// <summary>
        /// Logs a message as warning if <see cref="Build.LogLevel"/> is greater or equal to <see cref="LogLevel.Warning"/>.
        /// </summary>
        public static T Warn<T> (this T obj, Func<T, string> text)
        {
            Warn(text(obj));
            return obj;
        }

        #endregion

        #region Fail

        /// <summary>
        /// Logs a message as failure. Halts execution.
        /// </summary>
        [StringFormatMethod("format")]
        [ContractAnnotation("=> halt")]
        public static void Fail (string format, params object[] args)
        {
            Fail(string.Format(format, args));
        }

        /// <summary>
        /// Logs a message as failure. Halts execution.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void Fail (object value)
        {
            Fail(value.ToString());
        }

        /// <summary>
        /// Logs a message as failure. Halts execution.
        /// </summary>
        [ContractAnnotation("=> halt")]
        public static void Fail (string text)
        {
            ControlFlow.Fail(text);
        }

        #endregion
    }
}
