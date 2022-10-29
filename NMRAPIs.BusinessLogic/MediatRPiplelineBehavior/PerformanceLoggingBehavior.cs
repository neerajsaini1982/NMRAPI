namespace NMRAPIs.BusinessLogic.MediatRPiplelineBehavior
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using NMRAPIs.BusinessLogic.Helpers;

    /// <summary>
    /// Class to MediatR Performance Pipeline. This will track the time it takes to execute a MediatR Command/Query and log into a Log file.
    /// A warning would be logged if the execution time is > 500ms. Otherwise an informational log would be stored.
    /// Will track all the MediatR Query/Command that inherits from <see cref="Request"/> class.
    /// </summary>
    /// <typeparam name="TRequest">TRequest.</typeparam>
    /// <typeparam name="TResponse">TResponse.</typeparam>
    public class PerformanceLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : Request
    {
        private readonly ILogger<PerformanceLoggingBehavior<TRequest, TResponse>> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceLoggingBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The Logger Instance.</param>
        public PerformanceLoggingBehavior(ILogger<PerformanceLoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var response = await next();
            stopwatch.Stop();

            var name = typeof(TRequest).Name;

            if (stopwatch.ElapsedMilliseconds > 500)
            {
                this.logger.LogWarning("Task Performance ({Email}): {Name} ({ElapsedMilliseconds} milliseconds)", "Demo"/*request.User.GetLoggedInUserEmail()*/, name, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                this.logger.LogInformation("Task Performance ({Email}): {Name} ({ElapsedMilliseconds} milliseconds)", "Demo"/*request.User.GetLoggedInUserEmail()*/, name, stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
