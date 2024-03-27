using Polly;
using Polly.Retry;
using ResilienceNbg.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResilienceNbg.Services;

public class TypicalConnection : ITypicalConnection
{

    public async Task DoSomethingAsync()
    {

        var retryPolicy = Policy
           .Handle<BusinessTransientException>()
           .WaitAndRetryAsync(
            retryCount: 4, // Number of retry attempts (adjust as needed)
            sleepDurationProvider: attempt =>
           TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            // Exponential backoff formula
            onRetry: (exception, retryCount, context) =>
            {
                // You can add custom logic here, like logging or reporting retries.
                Console.WriteLine(DateTime.Now.ToString());
            }
           );


        // Define a circuit breaker policy
        var circuitBreakerPolicy = Policy
            // Specify the type of exception to handle
            .Handle<BusinessTransientException>()
            // Configure the circuit breaker with thresholds and duration
            .CircuitBreakerAsync(
                // Number of exceptions before breaking the circuit
                exceptionsAllowedBeforeBreaking: 4,
                // Duration of the open state (circuit remains open before attempting to close)
                durationOfBreak: TimeSpan.FromSeconds(3),
                // Action to perform when the circuit transitions to an open state
                onBreak: (exception, timespan) =>
                {
                    // Log or handle the circuit being broken
                    Console.WriteLine($"Circuit broken for {timespan.TotalSeconds} seconds due to {exception.Message}");
                },
                // Action to perform when the circuit transitions to a half-open state
                onReset: () =>
                {
                    // Log or handle the circuit being reset
                    Console.WriteLine("Circuit reset, half-open state");
                },
                // Action to perform when the circuit transitions to a closed state
                onHalfOpen: () =>
                {
                    // Log or handle the circuit being half-opened
                    Console.WriteLine("Circuit half-opened");
                });


        // Your code block that might encounter transient failures goes here.
        // For example, calling an external API or accessing a remote service.
        // If the operation throws 'YourTransientException', the retry policy will handle it.
        // If the operation succeeds at any retry attempt, the policy will stop retrying.

        await retryPolicy.ExecuteAsync(WeakService.GetConnectionInstance);

     
        // await circuitBreakerPolicy.ExecuteAsync(WeakService.GetConnectionInstance);
    }



}