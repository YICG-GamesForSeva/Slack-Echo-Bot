// <copyright file="SlackAdapterWithErrorHandler.cs" company="Young Indian Culture Group Inc">
// Copyright (c) Young Indian Culture Group Inc. All rights reserved.
// </copyright>

namespace YICG.Apps.Slack.Echo
{
    using Microsoft.Bot.Builder.Adapters.Slack;
    using Microsoft.Bot.Builder.TraceExtensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This is the Slack Adapter with error handling implementation.
    /// </summary>
    public class SlackAdapterWithErrorHandler : SlackAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SlackAdapterWithErrorHandler"/> class.
        /// </summary>
        /// <param name="configuration">Application key value settings.</param>
        /// <param name="adapterOptions">Slack adapter options.</param>
        /// <param name="logger">Logging mechanism.</param>
        public SlackAdapterWithErrorHandler(IConfiguration configuration, SlackAdapterOptions adapterOptions, ILogger<SlackAdapter> logger)
            : base(configuration, adapterOptions, logger)
        {
            this.OnTurnError = async (turnContext, exception) =>
            {
                // Log any leaked exception from the application.
                logger.LogError(exception, $"[OnTurnError] unhandled error : {exception.Message}");

                // Send a message to the user
                await turnContext.SendActivityAsync("The bot encountered an error or bug.").ConfigureAwait(false);
                await turnContext.SendActivityAsync("To continue to run this bot, please fix the bot source code.").ConfigureAwait(false);

                // Send a trace activity, which will be displayed in the Bot Framework Emulator
                await turnContext.TraceActivityAsync("OnTurnError Trace", exception.Message, "https://www.botframework.com/schemas/error", "TurnError").ConfigureAwait(false);
            };
        }
    }
}