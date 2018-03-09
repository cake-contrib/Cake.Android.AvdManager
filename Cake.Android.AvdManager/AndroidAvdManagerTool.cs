using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System.Threading.Tasks;

namespace Cake.AndroidAvdManager
{
    internal partial class AndroidAvdManagerTool : Tool<AndroidAvdManagerToolSettings>
    {
        public AndroidAvdManagerTool (ICakeContext cakeContext, IFileSystem fileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IToolLocator toolLocator)
            : base (fileSystem, cakeEnvironment, processRunner, toolLocator)
        {
            context = cakeContext;
            environment = cakeEnvironment;
        }

        ICakeContext context;
        ICakeEnvironment environment;

        protected override string GetToolName ()
        {
            return "avdmanager";
        }

        protected override IEnumerable<string> GetToolExecutableNames ()
        {
            return new List<string> {
                "avdmanager",
                "avdmanager.bat"
            };
        }

        protected override IEnumerable<FilePath> GetAlternativeToolPaths (AndroidAvdManagerToolSettings settings)
        {
            var results = new List<FilePath> ();

            var ext = environment.Platform.Family == PlatformFamily.Windows ? ".bat" : "";
            var androidHome = settings.SdkRoot.MakeAbsolute (environment).FullPath;

            if (!System.IO.Directory.Exists (androidHome))
                androidHome = environment.GetEnvironmentVariable ("ANDROID_HOME");

            if (!string.IsNullOrEmpty (androidHome) && System.IO.Directory.Exists (androidHome)) {
                var exe = new DirectoryPath (androidHome).Combine ("tools").Combine ("bin").CombineWithFilePath ("avdmanager" + ext);
                results.Add (exe);
            }

            return results;
        }

        public void AvdCreate (AndroidAvdManagerToolSettings settings, string name, string sdkId, string device, string sdCardPathOrSize = null, bool force = false, string avdPath = null)
        {
            var args = new List<string> {
                "create", "avd", "-n", name, "-k", $"\"{sdkId}\""
            };

            if (!string.IsNullOrEmpty(device)) {
                args.Add ("--device");
                args.Add ($"\"{device}\"");
            }

            if (!string.IsNullOrEmpty (sdCardPathOrSize)) {
                args.Add ("-c");
                args.Add ($"\"{sdCardPathOrSize}\"");
            }

            if (force)
                args.Add ("--force");
            
            if (!string.IsNullOrEmpty (avdPath)) {
                args.Add ("-p");
                args.Add ($"\"{avdPath}\"");
            }

            run (settings, args.ToArray ());
        }

        public void AvdDelete (AndroidAvdManagerToolSettings settings, string name)
        {
            run (settings, "delete", "avd", "-n", name);
        }

        public void AvdMove (AndroidAvdManagerToolSettings settings, string name, string path = null, string newName = null)
        {
            var args = new List<string> {
                "move", "avd", "-n", name
            };

            if (!string.IsNullOrEmpty(path)) {
                args.Add ("-p");
                args.Add (path);
            }

            if (!string.IsNullOrEmpty(newName)) {
                args.Add ("-r");
                args.Add (newName);
            }

            run (settings, args.ToArray());
        }

        public IEnumerable<AvdTarget> AvdListTargets (AndroidAvdManagerToolSettings settings)
        {
            foreach (var line in run (settings, "list", "target", "-c"))
                yield return new AvdTarget { Id = line.Trim () };
        }

        public IEnumerable<Avd> AvdListAvds (AndroidAvdManagerToolSettings settings)
        {
            foreach (var line in run (settings, "list", "avd", "-c"))
                yield return new Avd { Name = line.Trim () };
        }

        public IEnumerable<AvdDevice> AvdListDevices (AndroidAvdManagerToolSettings settings)
        {
            foreach (var line in run (settings, "list", "device", "-c"))
                yield return new AvdDevice { Name = line.Trim () };
        }

        IEnumerable<string> run (AndroidAvdManagerToolSettings settings, params string[] args)
        {
            if (settings == null)
                settings = new AndroidAvdManagerToolSettings ();

            var builder = new ProcessArgumentBuilder ();

            foreach (var arg in args)
                builder.Append (arg);

            var p = RunProcess (settings, builder, new ProcessSettings {
                RedirectStandardOutput = true
            });

            p.WaitForExit ();

            var stdout = p.GetStandardOutput ();

            return stdout;
        }
    }
}
