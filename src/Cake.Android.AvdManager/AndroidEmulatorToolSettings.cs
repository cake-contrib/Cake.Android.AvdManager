using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.AndroidEmulator
{
	public class AndroidEmulatorToolSettings : ToolSettings
	{
		/// <summary>
		/// Gets or sets the Android SDK root path.
		/// </summary>
		/// <value>The sdk root.</value>
		public DirectoryPath SdkRoot { get; set; }


		public bool NoSnapshotLoad { get; set; }

		public bool NoSnapshotSave { get; set; }

		public bool NoSnapshot { get; set; }

		const string cameraArgumentMessage = "Possible values are `none`, `emulated`, or `webcamX` where `X` is a number.";

		string cameraBack = null;
		public string CameraBack {
			get => cameraBack;
			set
			{
				var v = value.ToLowerInvariant();

				if (v != "emulated" && v != "none" && !v.StartsWith("webcam"))
					throw new ArgumentOutOfRangeException(nameof(CameraBack), cameraArgumentMessage);

				if (v.StartsWith("webcam"))
				{
					var n = v.Substring(6);

					if (!int.TryParse(n, out _))
						throw new ArgumentOutOfRangeException(nameof(CameraBack), cameraArgumentMessage);
				}

				cameraBack = v;
			}
		}

		string cameraFront = null;
		public string CameraFront
		{
			get => cameraFront;
			set
			{
				var v = value.ToLowerInvariant();

				if (v != "emulated" && v != "none" && !v.StartsWith("webcam"))
					throw new ArgumentOutOfRangeException(nameof(CameraBack), cameraArgumentMessage);

				if (v.StartsWith("webcam"))
				{
					var n = v.Substring(6);

					if (!int.TryParse(n, out _))
						throw new ArgumentOutOfRangeException(nameof(CameraBack), cameraArgumentMessage);
				}

				cameraFront = v;
			}
		}

		public int? MemoryMegabytes { get; set; }

		public FilePath SdCard { get; set; }

		public bool WipeData { get; set; }

		public string[] Debug { get; set; }

		public string[] Logcat { get; set; }

		public bool ShowKernel { get; set; }

		public bool Verbose { get; set; }

		public string[] DnsServers { get; set; }

		public string HttpProxy { get; set; }

		public string NetDelay { get; set; }

		public bool NetFast { get; set; }

		public string NetSpeed { get; set; }

		public uint? Port { get; set; }

		public (uint console, uint adb)? Ports { get; set; }

		public FilePath TcpDump { get; set; }

		public AndroidEmulatorAccelerationMode? Acceleration { get; set; }

		public AndroidEmulatorEngine? Engine { get; set; }

		public string Gpu { get; set; }

		public bool NoAccel { get; set; }

		public bool NoJni { get; set; }

		public AndroidEmulatorSeLinux? SeLinux { get; set; }

		public string Timezone { get; set; }

		public bool NoBootAnim { get; set; }

		public AndroidEmulatorScreenMode? Screen { get; set; }

		public string[] ExtraArgs { get; set; }
	}

	public enum AndroidEmulatorScreenMode
	{
		Touch,
		MultiTouch,
		NoTouch
	}

	public enum AndroidEmulatorSeLinux
	{
		Disabled,
		Permissive
	}

	public enum AndroidEmulatorAccelerationMode
	{
		Auto,
		Off,
		On
	}

	public enum AndroidEmulatorEngine
	{
		Auto,
		Classic,
		Qemu2
	}
}
