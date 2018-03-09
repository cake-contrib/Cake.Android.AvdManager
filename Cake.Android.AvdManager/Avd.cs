namespace Cake.AndroidAvdManager
{
    /// <summary>
    /// AVD Information
    /// </summary>
    public class Avd
    {
        /// <summary>
        /// Gets or sets the AVD name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        public override string ToString ()
        {
            return Name;
        }
    }
}
