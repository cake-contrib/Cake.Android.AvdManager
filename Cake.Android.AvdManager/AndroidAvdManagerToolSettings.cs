using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AndroidAvdManager
{
	/// <summary>
	/// Android SDK Manager tool settings.
	/// </summary>
	public class AndroidAvdManagerToolSettings : ToolSettings
	{
		/// <summary>
		/// Gets or sets the Android SDK root path.
		/// </summary>
		/// <value>The sdk root.</value>
		public DirectoryPath SdkRoot { get; set; }
	}
}