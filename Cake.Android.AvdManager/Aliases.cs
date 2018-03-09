using System;
using System.Linq;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.AndroidAvdManager
{
    /// <summary>
    /// Android SDK related aliases.
    /// </summary>
    [CakeAliasCategory ("Android AVD Manager")]
    public static class AndroidAvdManagerAliases
    {
        [CakeMethodAlias]
        public static void AndroidAvdCreate (this ICakeContext context, string name, string sdkId, string device, string sdCardPathOrSize = null, bool force = false, string avdPath = null, AndroidAvdManagerToolSettings settings = null)
        {
            context.GetTool ().AvdCreate (settings, name, sdkId, device, sdCardPathOrSize, force, avdPath);
        }

        [CakeMethodAlias]
        public static void AndroidAvdDelete (this ICakeContext context, string name, AndroidAvdManagerToolSettings settings = null)
        {
            context.GetTool ().AvdDelete (settings, name);
        }

        [CakeMethodAlias]
        public static void AndroidAvdMove (this ICakeContext context, string name, string path = null, string newName = null, AndroidAvdManagerToolSettings settings = null)
        {
            context.GetTool ().AvdMove (settings, name, path, newName);
        }

        [CakeMethodAlias]
        public static IEnumerable<AvdTarget> AndroidAvdListTargets (this ICakeContext context, AndroidAvdManagerToolSettings settings = null)
        {
            return context.GetTool ().AvdListTargets (settings);
        }

        [CakeMethodAlias]
        public static IEnumerable<Avd> AndroidAvdListAvds (this ICakeContext context, AndroidAvdManagerToolSettings settings = null)
        {
            return context.GetTool ().AvdListAvds (settings);
        }

        [CakeMethodAlias]
        public static IEnumerable<AvdDevice> AndroidAvdListDevices (this ICakeContext context, AndroidAvdManagerToolSettings settings = null)
        {
            return context.GetTool ().AvdListDevices (settings);
        }

        static AndroidAvdManagerTool GetTool (this ICakeContext context)
            => new AndroidAvdManagerTool (context, context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
    }
}
