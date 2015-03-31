namespace Neovolve.LinkChecker
{
    /// <summary>
    /// The <see cref="ResourceMetadataStatus"/>
    /// enum is used to define the status of a <see cref="ResourceMetadata"/> object.
    /// </summary>
    public enum ResourceMetadataStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Pending,

        /// <summary>
        /// 
        /// </summary>
        Processing,

        /// <summary>
        /// 
        /// </summary>
        HeadersChecked,
        /// <summary>
        /// 
        /// </summary>
        Downloading,
        /// <summary>
        /// 
        /// </summary>
        ResourcesChecked
    }
}