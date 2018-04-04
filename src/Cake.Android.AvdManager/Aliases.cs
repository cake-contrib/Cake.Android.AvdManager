using System;
using System.Linq;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.AndroidAvdManager
{
    /// <summary>
    /// Android AVD Manager related aliases.
    /// </summary>
    [CakeAliasCategory ("Android AVD Manager")]
    public static class AndroidAvdManagerAliases
    {
        /// <summary>
        /// Creates a new AVD emulator device.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="name">Name.</param>
        /// <param name="targetSdkId">Target SDK identifier.  Eg: `system-images;android-26;google_apis;x86`.  Use <c>AndroidAvdListTargets</c> for a list of valid targets.</param>
        /// <param name="device">Name of Device template to use. Eg: `Nexus 5X`.  Use <c>AndroidAvdListDevices</c> for a list of valid devices.</param>
        /// <param name="sdCardPathOrSize">SD Card local storage path, or the SD Card size. Eg: `1024M`.</param>
        /// <param name="force">If set to <c>true</c> force creation.</param>
        /// <param name="avdPath">Optional path to create the AVD in.</param>
        /// <param name="settings">Settings.</param>
        [CakeMethodAlias]
        public static void AndroidAvdCreate (this ICakeContext context, string name, string targetSdkId, string device, string sdCardPathOrSize = null, bool force = false, string avdPath = null, AndroidAvdManagerToolSettings settings = null)
        {
            context.GetTool ().AvdCreate (settings, name, targetSdkId, device, sdCardPathOrSize, force, avdPath);
        }

        /// <summary>
        /// Deletes an existing AVD emulator device.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="name">Name of AVD to delete.  Use <c>AndroidAvdListAvds</c> for a list of AVD's.</param>
        /// <param name="settings">Settings.</param>
        [CakeMethodAlias]
        public static void AndroidAvdDelete (this ICakeContext context, string name, AndroidAvdManagerToolSettings settings = null)
        {
            context.GetTool ().AvdDelete (settings, name);
        }

        /// <summary>
        /// Moves an existing AVD emulator device
        /// </summary>
        /// <param name="context">Context.</param>
        /// <param name="name">Name of AVD to delete.  Use <c>AndroidAvdListAvds</c> for a list of AVD's.</param>
        /// <param name="path">Path to move the AVD to.</param>
        /// <param name="newName">New name of the AVD.</param>
        /// <param name="settings">Settings.</param>
        [CakeMethodAlias]
        public static void AndroidAvdMove (this ICakeContext context, string name, string path = null, string newName = null, AndroidAvdManagerToolSettings settings = null)
        {
            context.GetTool ().AvdMove (settings, name, path, newName);
        }

        /// <summary>
        /// Lists valid AVD targets
        /// </summary>
        /// <returns>List of AVD targets.</returns>
        /// <param name="context">Context.</param>
        /// <param name="settings">Settings.</param>
        [CakeMethodAlias]
        public static IEnumerable<AvdTarget> AndroidAvdListTargets (this ICakeContext context, AndroidAvdManagerToolSettings settings = null)
        {
            return context.GetTool ().AvdListTargets (settings);
        }

        /// <summary>
        /// Lists AVD emulators
        /// </summary>
        /// <returns>List of AVD's.</returns>
        /// <param name="context">Context.</param>
        /// <param name="settings">Settings.</param>
        [CakeMethodAlias]
        public static IEnumerable<Avd> AndroidAvdListAvds (this ICakeContext context, AndroidAvdManagerToolSettings settings = null)
        {
            return context.GetTool ().AvdListAvds (settings);
        }

        /// <summary>
        /// Lists valid AVD devices 
        /// </summary>
        /// <returns>List of AVD devices.</returns>
        /// <param name="context">Context.</param>
        /// <param name="settings">Settings.</param>
        [CakeMethodAlias]
        public static IEnumerable<AvdDevice> AndroidAvdListDevices (this ICakeContext context, AndroidAvdManagerToolSettings settings = null)
        {
            return context.GetTool ().AvdListDevices (settings);
        }

        static AndroidAvdManagerTool GetTool (this ICakeContext context)
            => new AndroidAvdManagerTool (context, context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
    }
}
