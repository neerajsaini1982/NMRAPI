using System;

namespace NMRAPIs.Core.Common
{
    public interface ITrackable
    {
        /// <summary>
        /// CreatedAt TimeStamp of the Entity.
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// ProfileId of the User who created the Entity.
        /// </summary>
        int CreatedBy { get; set; }

        /// <summary>
        /// LastUpdated TimeStamp of the Entity.
        /// </summary>
        DateTime LastUpdatedAt { get; set; }

        /// <summary>
        /// ProfileId of the user who last updated the Entity.
        /// </summary>
        int LastUpdatedBy { get; set; }
    }
}
