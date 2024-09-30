﻿using System.Net;
using iCloud.Dav.Core.Logger;

namespace iCloud.Dav.Core;

/// <summary>
/// A thread-safe back-off handler which handles an abnormal HTTP response or an exception with
/// <see cref="IBackOff" />.
/// </summary>
public class BackOffHandler : IHttpUnsuccessfulResponseHandler, IHttpExceptionHandler
{
    private static readonly ILogger _logger = ApplicationContext.Logger.ForType<BackOffHandler>();

    /// <summary>Gets the back-off policy used by this back-off handler.</summary>
    public IBackOff BackOff { get; private set; }

    /// <summary>
    /// Gets the maximum time span to wait. If the back-off instance returns a greater time span, the handle method
    /// returns <c>false</c>. Default value is 16 seconds per a retry request.
    /// </summary>
    public TimeSpan MaxTimeSpan { get; private set; }

    /// <summary>
    /// Gets a delegate function which indicates whether this back-off handler should handle an abnormal HTTP
    /// response. The default is <see cref="Initializer.DefaultHandleUnsuccessfulResponseFunc" />.
    /// </summary>
    public Func<HttpResponseMessage, bool> HandleUnsuccessfulResponseFunc { get; private set; }

    /// <summary>
    /// Gets a delegate function which indicates whether this back-off handler should handle an exception. The
    /// default is <see cref="Initializer.DefaultHandleExceptionFunc" />.
    /// </summary>
    public Func<Exception, bool> HandleExceptionFunc { get; private set; }

    /// <summary>Constructs a new back-off handler with the given back-off.</summary>
    /// <param name="backOff">The back-off policy.</param>
    public BackOffHandler(IBackOff backOff) : this(new Initializer(backOff))
    {
    }

    /// <summary>Constructs a new back-off handler with the given initializer.</summary>
    public BackOffHandler(Initializer initializer)
    {
        BackOff = initializer.BackOff;
        MaxTimeSpan = initializer.MaxTimeSpan;
        HandleExceptionFunc = initializer.HandleExceptionFunc;
        HandleUnsuccessfulResponseFunc = initializer.HandleUnsuccessfulResponseFunc;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
    {
        return HandleUnsuccessfulResponseFunc != null && HandleUnsuccessfulResponseFunc(args.Response)
            ? await HandleAsync(args.SupportsRetry, args.CurrentFailedTry, args.CancellationToken).ConfigureAwait(false)
            : false;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> HandleExceptionAsync(HandleExceptionArgs args)
    {
        return HandleExceptionFunc != null && HandleExceptionFunc(args.Exception)
            ? await HandleAsync(args.SupportsRetry, args.CurrentFailedTry, args.CancellationToken).ConfigureAwait(false)
            : false;
    }

    /// <summary>
    /// Handles back-off. In case the request doesn't support retry or the back-off time span is greater than the
    /// maximum time span allowed for a request, the handler returns <c>false</c>. Otherwise the current thread
    /// will block for x milliseconds (x is defined by the <see cref="BackOff" /> instance), and this handler
    /// returns <c>true</c>.
    /// </summary>
    private async Task<bool> HandleAsync(bool supportsRetry, int currentFailedTry, CancellationToken cancellationToken)
    {
        if (!supportsRetry || BackOff.MaxNumOfRetries < currentFailedTry)
            return false;
        var ts = BackOff.GetNextBackOff(currentFailedTry);
        if (ts > MaxTimeSpan || ts < TimeSpan.Zero)
            return false;
        await Wait(ts, cancellationToken).ConfigureAwait(false);
        _logger.Debug("Back-Off handled the error. Waited {0}ms before next retry...", ts.TotalMilliseconds);
        return true;
    }

    /// <summary>Waits the given time span. Overriding this method is recommended for mocking purposes.</summary>
    /// <param name="ts">TimeSpan to wait (and block the current thread).</param>
    /// <param name="cancellationToken">The cancellation token in case the user wants to cancel the operation in
    /// the middle.</param>
    protected virtual async Task Wait(TimeSpan ts, CancellationToken cancellationToken) => await Task.Delay(ts, cancellationToken);

    /// <summary>An initializer class to initialize a back-off handler.</summary>
    public class Initializer
    {
        /// <summary>Default function which handles server errors (503).</summary>
        public static readonly Func<HttpResponseMessage, bool> DefaultHandleUnsuccessfulResponseFunc = r => r.StatusCode == HttpStatusCode.ServiceUnavailable;
        /// <summary>
        /// Default function which handles exception which aren't
        /// <see cref="TaskCanceledException" /> or
        /// <see cref="OperationCanceledException" />. Those exceptions represent a task or an operation
        /// which was canceled and shouldn't be retried.
        /// </summary>
        public static readonly Func<Exception, bool> DefaultHandleExceptionFunc = ex =>
        {
            return ex is not TaskCanceledException ? ex is not OperationCanceledException : false;
        };

        /// <summary>Gets the back-off policy used by this back-off handler.</summary>
        public IBackOff BackOff { get; private set; }

        /// <summary>
        /// Gets or sets the maximum time span to wait. If the back-off instance returns a greater time span than
        /// this value, this handler returns <c>false</c> to both <c>HandleExceptionAsync</c> and
        /// <c>HandleResponseAsync</c>. Default value is 16 seconds per a retry request.
        /// </summary>
        public TimeSpan MaxTimeSpan { get; set; }

        /// <summary>
        /// Gets or sets a delegate function which indicates whether this back-off handler should handle an
        /// abnormal HTTP response. The default is <see cref="DefaultHandleUnsuccessfulResponseFunc" />.
        /// </summary>
        public Func<HttpResponseMessage, bool> HandleUnsuccessfulResponseFunc { get; set; }

        /// <summary>
        /// Gets or sets a delegate function which indicates whether this back-off handler should handle an
        /// exception. The default is <see cref="DefaultHandleExceptionFunc" />.
        /// </summary>
        public Func<Exception, bool> HandleExceptionFunc { get; set; }

        /// <summary>
        /// Constructs a new initializer by the given back-off.
        /// </summary>
        public Initializer(IBackOff backOff)
        {
            BackOff = backOff;
            HandleExceptionFunc = DefaultHandleExceptionFunc;
            HandleUnsuccessfulResponseFunc = DefaultHandleUnsuccessfulResponseFunc;
            MaxTimeSpan = TimeSpan.FromSeconds(16.0);
        }
    }
}