// <copyright file="EchoBot.cs" company="Young Indian Culture Group Inc">
// Copyright (c) Young Indian Culture Group Inc. All rights reserved.
// </copyright>

namespace YICG.Apps.Slack.Echo.Bots
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;

    /// <summary>
    /// This is the echo bot.
    /// </summary>
    public class EchoBot : ActivityHandler
    {
        /// <summary>
        /// Anytime there is a message coming into the bot, this method will execute.
        /// </summary>
        /// <param name="turnContext">The current turn/execution flow.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A unit of execution.</returns>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Anytime that a new member gets added to the chat with this bot.
        /// </summary>
        /// <param name="membersAdded">The list of the <see cref="ChannelAccount"/> which are added.</param>
        /// <param name="turnContext">The current turn/execution flow.</param>
        /// <param name="cancellationToken">The cancellation token to signal completion of a turn.</param>
        /// <returns>A unit of execution.</returns>
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            if (membersAdded is null)
            {
                throw new ArgumentNullException(nameof(membersAdded));
            }

            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}