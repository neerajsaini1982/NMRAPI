namespace NMRAPIs.Core.Common
{
    public interface IDeletable
    {
        /// <summary>
        /// Indicates whether the entity is deleted or not.
        /// </summary>
        bool IsDeleted { get; }
    }
}
