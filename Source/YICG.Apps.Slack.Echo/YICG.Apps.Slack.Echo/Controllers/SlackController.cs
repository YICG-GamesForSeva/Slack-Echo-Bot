// <copyright file="SlackController.cs" company="Young Indian Culture Group Inc">
// Copyright (c) Young Indian Culture Group Inc. All rights reserved.
// </copyright>

namespace YICG.Apps.Slack.Echo.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Adapters.Slack;

    /// <summary>
    /// This is the slack controller, and all messages will come here.
    /// </summary>
    [Route("api/slack")]
    [ApiController]
    public class SlackController : ControllerBase
    {
        private readonly SlackAdapter adapter;
        private readonly IBot bot;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlackController"/> class.
        /// </summary>
        /// <param name="adapter">The Slack adapter middleware.</param>
        /// <param name="bot">The bot that can process the incoming messages/activities.</param>
        public SlackController(SlackAdapter adapter, IBot bot)
        {
            this.adapter = adapter;
            this.bot = bot;
        }

        /// <summary>
        /// This method will process the incoming requests that are decorated as
        /// HTTP POST to the adapter, which will then invoke the bot.
        /// </summary>
        /// <returns>A unit of execution.</returns>
        [HttpPost]
        [HttpGet]
        public async Task PostAsync()
        {
            var cnclToken = CancellationToken.None;
            await this.adapter.ProcessAsync(this.Request, this.Response, this.bot, cnclToken).ConfigureAwait(false);
        }
    }
}