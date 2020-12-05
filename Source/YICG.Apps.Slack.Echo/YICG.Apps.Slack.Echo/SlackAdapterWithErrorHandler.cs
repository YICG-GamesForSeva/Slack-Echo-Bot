// <copyright file="SlackAdapterWithErrorHandler.cs" company="Young Indian Culture Group Inc">
// Copyright (c) Young Indian Culture Group Inc. All rights reserved.
// </copyright>

namespace YICG.Apps.Slack.Echo
{
    using Microsoft.Bot.Builder.Adapters.Slack;
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

        }
    }
}