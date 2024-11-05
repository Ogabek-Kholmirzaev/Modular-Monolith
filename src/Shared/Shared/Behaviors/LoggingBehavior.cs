using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name,
            typeof(TResponse).Name,
            request);

        var timer = new Stopwatch();

        timer.Start();

        var response = await next();

        timer.Stop();

        var timerTaken = timer.Elapsed;

        if (timerTaken.Seconds > 3)
        {
            logger.LogWarning(
                "[PERFORMANCE] The request {Request} took {TimeTaken} seconds.",
                typeof(TRequest).Name,
                timerTaken.Seconds);
        }

        logger.LogInformation(
            "[END] Handled {TRequest} with {TResponse}",
            typeof(TRequest).Name,
            typeof(TResponse).Name);

        return response;
    }
}
