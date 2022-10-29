// <copyright file="IBaseService.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.ServicesInterfaces
{
    /// <summary>
    /// Only a Marker Interface to be used in DI.
    /// If the service is inherited from this class it would be easy to use ID as through reflection we can fetch the service using this base interface.
    /// </summary>
    public interface IBaseService
    {
    }
}
