using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System.Threading.Tasks;

namespace Cake.AndroidEmulator
{
    internal partial class AndroidEmulatorTool : Tool<AndroidEmulatorToolSettings>
    {
        public AndroidEmulatorTool(ICakeContext cakeContext, IFileSystem fileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IToolLocator toolLocator)
            : base(fileSystem, cakeEnvironment, processRunner, toolLocator)
        {
            context = cakeContext;
            environment = cakeEnvironment;
        }

        ICakeContext context;
        ICakeEnvironment environment;

        protected override string GetToolName()
        {
            return "emulator";
        }

        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new List<string> {
                "emulator",
                "emulator.exe"
            };
        }

        protected override IEnumerable<FilePath> GetAlternativeToolPaths(AndroidEmulatorToolSettings settings)
        {
            var results = new List<FilePath>();

            var ext = environment.Platform.Family == PlatformFamily.Windows ? ".exe" : "";
            var androidHome = settings.SdkRoot?.MakeAbsolute(environment)?.FullPath;

            if (string.IsNullOrEmpty(androidHome) || !System.IO.Directory.Exists(androidHome))
                androidHome = environment.GetEnvironmentVariable("ANDROID_HOME");

            if (!string.IsNullOrEmpty(androidHome) && System.IO.Directory.Exists(androidHome))
            {
                var exe = new DirectoryPath(androidHome).Combine("emulator").CombineWithFilePath("emulator" + ext);
                results.Add(exe);
            }

            return results;
        }

        internal IEnumerable<string> ListAvds(ICakeContext context, AndroidEmulatorToolSettings settings)
        {
            if (settings == null)
                settings = new AndroidEmulatorToolSettings();

            var builder = new ProcessArgumentBuilder();

            builder.Append("-list-avds");

            var lines = run(context, settings, builder);

            foreach (var l in lines)
            {
                if (!string.IsNullOrWhiteSpace(l))
                    yield return l;
            }
        }

        internal AndroidEmulatorProcess Run(ICakeContext context, string avdName, AndroidEmulatorToolSettings settings, params string[] args)
        {
            if (settings == null)
                settings = new AndroidEmulatorToolSettings();

            var builder = new ProcessArgumentBuilder();

            builder.Append($"-avd {avdName}");

            if (settings.NoSnapshotLoad)
                builder.Append("-no-snapshot-load");
            if (settings.NoSnapshotSave)
                builder.Append("-no-snapshot-save");
            if (settings.NoSnapshot)
                builder.Append("-no-snapshot");

            if (!string.IsNullOrEmpty(settings.CameraBack))
                builder.Append($"-camera-back {settings.CameraBack}");
            if (!string.IsNullOrEmpty(settings.CameraFront))
                builder.Append($"-camera-front {settings.CameraFront}");

            if (settings.MemoryMegabytes.HasValue)
                builder.Append($"-memory {settings.MemoryMegabytes}");

            if (settings.SdCard != null)
            {
                builder.Append("-sdcard");
                builder.AppendQuoted(settings.SdCard.MakeAbsolute(context.Environment).FullPath);
            }

            if (settings.WipeData)
                builder.Append("-wipe-data");

            if (settings.Debug != null && settings.Debug.Length > 0)
                builder.Append("-debug " + string.Join(",", settings.Debug));

            if (settings.Logcat != null && settings.Logcat.Length > 0)
                builder.Append("-logcat " + string.Join(",", settings.Logcat));

            if (settings.ShowKernel)
                builder.Append("-show-kernel");

            if (settings.Verbose)
                builder.Append("-verbose");

            if (settings.DnsServers != null && settings.DnsServers.Length > 0)
                builder.Append("-dns-server " + string.Join(",", settings.DnsServers));

            if (!string.IsNullOrEmpty(settings.HttpProxy))
                builder.Append($"-http-proxy {settings.HttpProxy}");

            if (!string.IsNullOrEmpty(settings.NetDelay))
                builder.Append($"-netdelay {settings.NetDelay}");

            if (settings.NetFast)
                builder.Append("-netfast");

            if (!string.IsNullOrEmpty(settings.NetSpeed))
                builder.Append($"-netspeed {settings.NetSpeed}");

            if (settings.Ports.HasValue)
                builder.Append($"-ports {settings.Ports.Value.console},{settings.Ports.Value.adb}");
            else if (settings.Port.HasValue)
                builder.Append($"-port {settings.Port.Value}");

            if (settings.TcpDump != null)
            {
                builder.Append("-tcpdump");
                builder.AppendQuoted(settings.TcpDump.MakeAbsolute(context.Environment).FullPath);
            }

            if (settings.Acceleration.HasValue)
                builder.Append($"-accel {settings.Acceleration.Value.ToString().ToLowerInvariant()}");

            if (settings.NoAccel)
                builder.Append("-no-accel");

            if (settings.Engine.HasValue)
                builder.Append($"-engine {settings.Engine.Value.ToString().ToLowerInvariant()}");

            if (settings.NoJni)
                builder.Append("-no-jni");

            if (settings.SeLinux.HasValue)
                builder.Append($"-selinux {settings.SeLinux.Value.ToString().ToLowerInvariant()}");

            if (!string.IsNullOrEmpty(settings.Timezone))
                builder.Append($"-timezone {settings.Timezone}");

            if (settings.NoBootAnim)
                builder.Append("-no-boot-anim");

            if (settings.Screen.HasValue)
                builder.Append($"-screen {settings.Screen.Value.ToString().ToLowerInvariant()}");

            if (settings.ExtraArgs != null && settings.ExtraArgs.Length > 0)
            {
                foreach (var arg in settings.ExtraArgs)
                    builder.Append(arg);
            }

            return new AndroidEmulatorProcess(start(context, settings, builder));
        }

        IEnumerable<string> run(ICakeContext context, AndroidEmulatorToolSettings settings, ProcessArgumentBuilder builder, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                foreach (var arg in args)
                    builder.Append(arg);
            }

            var p = start(context, settings, builder);
            p.WaitForExit();

            var stdout = p.GetStandardOutput();

            return stdout;
        }

        IProcess start(ICakeContext context, AndroidEmulatorToolSettings settings, ProcessArgumentBuilder builder, params string[] args)
        {
            if (args != null && args.Length > 0)
            {
                foreach (var arg in args)
                    builder.Append(arg);
            }

            var p = RunProcess(settings, builder, new ProcessSettings
            {
                RedirectStandardOutput = true
            });

            return p;
        }
    }

    public class AndroidEmulatorProcess
    {
        public AndroidEmulatorProcess(IProcess p)
        {
            process = p;
        }

        IProcess process;

        public int WaitForExit()
        {
            process.WaitForExit();
            return process.GetExitCode();
        }

        public void Kill()
            => process.Kill();

        public IEnumerable<string> GetStandardOutput()
            => process.GetStandardOutput();
    }
}
