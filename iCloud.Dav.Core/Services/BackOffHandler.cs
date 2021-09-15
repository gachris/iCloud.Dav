using iCloud.Dav.Core.Args;
using iCloud.Dav.Core.Logger;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Dav.Core.Services
{
    /// <summary>
    /// A thread-safe back-off handler which handles an abnormal HTTP response or an exception with
    /// <see cref="T:ICloud.Api.Util.IBackOff" />.
    /// </summary>
    public class BackOffHandler : IHttpUnsuccessfulResponseHandler, IHttpExceptionHandler
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<BackOffHandler>();

        /// <summary>Gets the back-off policy used by this back-off handler.</summary>
        public IBackOff BackOff { get; private set; }

        /// <summary>
        /// Gets the maximum time span to wait. If the back-off instance returns a greater time span, the handle method
        /// returns <c>false</c>. Default value is 16 seconds per a retry request.
        /// </summary>
        public TimeSpan MaxTimeSpan { get; private set; }

        /// <summary>
        /// Gets a delegate function which indicates whether this back-off handler should handle an abnormal HTTP
        /// response. The default is <see cref="F:ICloud.Api.Http.BackOffHandler.Initializer.DefaultHandleUnsuccessfulResponseFunc" />.
        /// </summary>
        public Func<HttpResponseMessage, bool> HandleUnsuccessfulResponseFunc { get; private set; }

        /// <summary>
        /// Gets a delegate function which indicates whether this back-off handler should handle an exception. The
        /// default is <see cref="F:ICloud.Api.Http.BackOffHandler.Initializer.DefaultHandleExceptionFunc" />.
        /// </summary>
        public Func<Exception, bool> HandleExceptionFunc { get; private set; }

        /// <summary>Constructs a new back-off handler with the given back-off.</summary>
        /// <param name="backOff">The back-off policy.</param>
        public BackOffHandler(IBackOff backOff) : this(new BackOffHandler.Initializer(backOff))
        {
        }

        /// <summary>Constructs a new back-off handler with the given initializer.</summary>
        public BackOffHandler(BackOffHandler.Initializer initializer)
        {
            this.BackOff = initializer.BackOff;
            this.MaxTimeSpan = initializer.MaxTimeSpan;
            this.HandleExceptionFunc = initializer.HandleExceptionFunc;
            this.HandleUnsuccessfulResponseFunc = initializer.HandleUnsuccessfulResponseFunc;
        }

        public virtual async Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
        {
            if (this.HandleUnsuccessfulResponseFunc != null && this.HandleUnsuccessfulResponseFunc(args.Response))
                return await this.HandleAsync(args.SupportsRetry, args.CurrentFailedTry, args.CancellationToken).ConfigureAwait(false);
            return false;
        }

        public virtual async Task<bool> HandleExceptionAsync(HandleExceptionArgs args)
        {
            if (this.HandleExceptionFunc != null && this.HandleExceptionFunc(args.Exception))
                return await this.HandleAsync(args.SupportsRetry, args.CurrentFailedTry, args.CancellationToken).ConfigureAwait(false);
            return false;
        }

        /// <summary>
        /// Handles back-off. In case the request doesn't support retry or the back-off time span is greater than the
        /// maximum time span allowed for a request, the handler returns <c>false</c>. Otherwise the current thread
        /// will block for x milliseconds (x is defined by the <see cref="P:ICloud.Api.Http.BackOffHandler.BackOff" /> instance), and this handler
        /// returns <c>true</c>.
        /// </summary>
        private async Task<bool> HandleAsync(bool supportsRetry, int currentFailedTry, CancellationToken cancellationToken)
        {
            if (!supportsRetry || this.BackOff.MaxNumOfRetries < currentFailedTry)
                return false;
            TimeSpan ts = this.BackOff.GetNextBackOff(currentFailedTry);
            if (ts > this.MaxTimeSpan || ts < TimeSpan.Zero)
                return false;
            await this.Wait(ts, cancellationToken).ConfigureAwait(false);
            BackOffHandler.Logger.Debug("Back-Off handled the error. Waited {0}ms before next retry...", (object)ts.TotalMilliseconds);
            return true;
        }

        /// <summary>Waits the given time span. Overriding this method is recommended for mocking purposes.</summary>
        /// <param name="ts">TimeSpan to wait (and block the current thread).</param>
        /// <param name="cancellationToken">The cancellation token in case the user wants to cancel the operation in
        /// the middle.</param>
        protected virtual async Task Wait(TimeSpan ts, CancellationToken cancellationToken)
        {
#if net40
            await TaskEx.Delay(ts, cancellationToken).ConfigureAwait(false);
#endif
#if others_frameworks
            await Task.Delay(ts, cancellationToken);
#endif
        }

        /// <summary>An initializer class to initialize a back-off handler.</summary>
        public class Initializer
        {
            /// <summary>Default function which handles server errors (503).</summary>
            public static readonly Func<HttpResponseMessage, bool> DefaultHandleUnsuccessfulResponseFunc = r => r.StatusCode == HttpStatusCode.ServiceUnavailable;
            /// <summary>
            /// Default function which handles exception which aren't
            /// <see cref="T:System.Threading.Tasks.TaskCanceledException" /> or
            /// <see cref="T:System.OperationCanceledException" />. Those exceptions represent a task or an operation
            /// which was canceled and shouldn't be retried.
            /// </summary>
            public static readonly Func<Exception, bool> DefaultHandleExceptionFunc = ex =>
            {
                if (!(ex is TaskCanceledException))
                    return !(ex is OperationCanceledException);
                return false;
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
            /// abnormal HTTP response. The default is <see cref="F:ICloud.Api.Http.BackOffHandler.Initializer.DefaultHandleUnsuccessfulResponseFunc" />.
            /// </summary>
            public Func<HttpResponseMessage, bool> HandleUnsuccessfulResponseFunc { get; set; }

            /// <summary>
            /// Gets or sets a delegate function which indicates whether this back-off handler should handle an
            /// exception. The default is <see cref="F:ICloud.Api.Http.BackOffHandler.Initializer.DefaultHandleExceptionFunc" />.
            /// </summary>
            public Func<Exception, bool> HandleExceptionFunc { get; set; }

            /// <summary>Constructs a new initializer by the given back-off.</summary>
            public Initializer(IBackOff backOff)
            {
                this.BackOff = backOff;
                this.HandleExceptionFunc = BackOffHandler.Initializer.DefaultHandleExceptionFunc;
                this.HandleUnsuccessfulResponseFunc = BackOffHandler.Initializer.DefaultHandleUnsuccessfulResponseFunc;
                this.MaxTimeSpan = TimeSpan.FromSeconds(16.0);
            }
        }
    }
}
