using System;
using Cake.Core.IO;
using Cake.AndroidAvdManager.Fakes;
using Cake.AndroidAvdManager;
using Cake.AndroidSdkManager;
using Cake.Core;
using Cake;
using System.Linq;
using Xunit;
using System.IO;

namespace Cake.AndroidAvdManager.Tests
{
    public class Tests : TestFixtureBase
    {
        static string ANDROID_SDK_ROOT
            => Environment.GetEnvironmentVariable ("ANDROID_HOME")
                ?? File.ReadAllText (System.IO.Path.Combine (ContentPath, "android_home.txt"))?.Trim ();

        static AndroidAvdManagerToolSettings Settings
            => new AndroidAvdManagerToolSettings { SdkRoot = ANDROID_SDK_ROOT };

        [Fact]
        public void List_Avds ()
        {
            var avds = Cake.AndroidAvdListAvds (Settings);

            Assert.NotEmpty (avds);
        }

        [Fact]
        public void List_Devices ()
        {
            var avds = Cake.AndroidAvdListDevices (Settings);

            Assert.NotEmpty (avds);
        }

        [Fact]
        public void List_Targets ()
        {
            var avds = Cake.AndroidAvdListTargets (Settings);

            Assert.NotEmpty (avds);
        }

        [Fact]
        public void Create_Delete_AVD ()
        {
            Cake.AndroidAvdCreate ("AVDTEST", "system-images;android-26;google_apis;x86", "Nexus 5X", force: true, settings: Settings);

            Assert.Contains(Cake.AndroidAvdListAvds (Settings), a => a.Name == "AVDTEST");

            Cake.AndroidAvdDelete ("AVDTEST", Settings);

            Assert.DoesNotContain (Cake.AndroidAvdListAvds (Settings), a => a.Name == "AVDTEST");
        }
    }
}
