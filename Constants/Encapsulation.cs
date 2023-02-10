namespace UITestingFramework
{
    /// <summary>
    /// Access layer definitions for reflection object types.
    /// </summary>
    public enum Encapsulation
    {
        /// <summary>
        /// Instance Definition.
        /// </summary>
        Instance = 4,
        /// <summary>
        /// Static Definition.
        /// </summary>
        Static = 8,
        /// <summary>
        /// Public Definition.
        /// </summary>
        Public = 16,
        /// <summary>
        /// Private Definition.
        /// </summary>
        Private = 32,
        /// <summary>
        /// StaticInstance Definition.
        /// </summary>
        StaticInstance = Static | Instance,
        /// <summary>
        /// PublicInstance Definition.
        /// </summary>
        PublicInstance = Public | Instance,
        /// <summary>
        /// PrivateInstance Definition.
        /// </summary>
        PrivateInstance = Private | Instance,
        /// <summary>
        /// All Definition.
        /// </summary>
        All = Instance | Static | Public | Private
    }
}