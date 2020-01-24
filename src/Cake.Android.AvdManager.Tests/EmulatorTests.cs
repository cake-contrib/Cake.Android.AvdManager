using Cake.AndroidAvdManager.Fakes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Cake.AndroidEmulator.Tests
{
    public class EmulatorTests : TestFixtureBase
    {
        static string ANDROID_SDK_ROOT
            => Environment.GetEnvironmentVariable("ANDROID_HOME")
                ?? File.ReadAllText(System.IO.Path.Combine(ContentPath, "android_home.txt"))?.Trim();

        static AndroidEmulatorToolSettings Settings
            => new AndroidEmulatorToolSettings { SdkRoot = ANDROID_SDK_ROOT };

        [Fact]
        public void List_Avds()
        {
            var avds = Cake.AndroidEmulatorListAvds(Settings);

            Assert.NotEmpty(avds);
        }
    }
}
