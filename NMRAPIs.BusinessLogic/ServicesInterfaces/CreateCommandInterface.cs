// <copyright file="CreateCommandInterface.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.BusinessLogic.ServicesInterfaces
{
    using System.Collections.Generic;
    using MediatR;
    using NMRAPIs.Core.Pagination;

    /// <summary>
    /// MediatR Command Base Interface for Creating and Updating an Entity.
    /// Returns a Int as the newly created entity Id.
    /// </summary>
    public interface ICreateUpdateCommand : IRequest<int>
    {
    }

    /// <summary>
    /// MediatR Command Base Interface for <see cref="ICreateUpdateCommand"/> Handler.
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    public interface ICreateUpdateCommandHandler<TRequest> : IRequestHandler<TRequest, int>
        where TRequest : ICreateUpdateCommand
    {
    }

    /// <summary>
    /// MediatR Command Base Interface for Deleting an Entity.
    /// Returns a flag to indicate whether the Entity was deleted or not.
    /// </summary>
    public interface IDeleteCommand : IRequest<bool>
    {
    }

    /// <summary>
    /// MediatR Command Base Interface for <see cref="IDeleteCommand"/> Handler.
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    public interface IDeleteCommandHandler<TRequest> : IRequestHandler<TRequest, bool>
        where TRequest : IDeleteCommand
    {
    }

    /// <summary>
    /// MediatR Query Base Interface for getting Paged Results of an Entity List.
    /// </summary>
    /// <typeparam name="T">The View Model which would be returned as a PagedEntity.</typeparam>
    public interface IGetAllPagedEntityCommand<T> : IRequest<PagedEntity<T>>
    {
    }

    /// <summary>
    /// MediatR Base Handler for <see cref="IGetAllPagedEntityCommand"/> class.
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    /// <typeparam name="TResponse">The View Model which would be returned as a PagedEntity.</typeparam>
    public interface IGetAllPagedEntityCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, PagedEntity<TResponse>>
        where TRequest : IGetAllPagedEntityCommand<TResponse>
    {
    }

    /// <summary>
    /// MediatR Query Base Interface for fetching an Entity.
    /// </summary>
    /// <typeparam name="T">The Generic View Model of the Entity to return.</typeparam>
    public interface IGetEntityCommand<T> : IRequest<T>
    {
    }

    /// <summary>
    /// MediatR Base handler Interface for <see cref="IGetEntityCommand"/> command.
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    /// <typeparam name="TResponse">The Generic View Model of the Entity to return.</typeparam>
    public interface IGetEntityCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IGetEntityCommand<TResponse>
    {
    }

    /// <summary>
    /// MediatR Base Query Interface for getting all the entity.
    /// </summary>
    /// <typeparam name="T">View Model of the Entity.</typeparam>
    public interface IGetAllEntityCommand<T> : IRequest<List<T>>
    {
    }

    /// <summary>
    /// MediatR Base Handler for the Query <see cref="IGetAllEntityCommand"/> Interface .
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    /// <typeparam name="TResponse">View Model of the Entity.</typeparam>
    public interface IGetAllEntityCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, List<TResponse>>
        where TRequest : IGetAllEntityCommand<TResponse>
    {
    }
}
