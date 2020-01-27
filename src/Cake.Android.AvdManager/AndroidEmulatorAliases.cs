using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cake.AndroidEmulator
{
    /// <summary>
    /// Android AVD Manager related aliases.
    /// </summary>
    [CakeAliasCategory("Android Emulator")]
    public static class AndroidEmulatorAliases
    {
        /// <summary>
        /// Starts an emulator.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="avdName">Android AVD Name.</param>
        /// <param name="settings">Settings.</param>
        [CakeMethodAlias]
        public static AndroidEmulatorProcess AndroidEmulatorStart(this ICakeContext context, string avdName, AndroidEmulatorToolSettings settings = null)
            => context.GetTool().Run(context, avdName, settings);

        /// <summary>
        /// Returns a list of available Emulator AVD names.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="settings">Settings.</param>
        /// <returns></returns>
        public static List<string> AndroidEmulatorListAvds(this ICakeContext context, AndroidEmulatorToolSettings settings)
            => context.GetTool().ListAvds(context, settings)?.ToList() ?? new List<string>();

        static AndroidEmulatorTool GetTool(this ICakeContext context)
            => new AndroidEmulatorTool(context, context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
    }
}
