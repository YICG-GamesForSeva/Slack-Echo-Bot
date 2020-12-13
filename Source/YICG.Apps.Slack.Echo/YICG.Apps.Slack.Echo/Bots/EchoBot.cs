// <copyright file="EchoBot.cs" company="Young Indian Culture Group Inc">
// Copyright (c) Young Indian Culture Group Inc. All rights reserved.
// </copyright>

namespace YICG.Apps.Slack.Echo.Bots
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Adapters.Slack.Model;
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;
    using SlackAPI;
    using Attachment = Microsoft.Bot.Schema.Attachment;

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
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken).ConfigureAwait(false);
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

        /// <summary>
        /// OnEventActivityAsync method returns an async Task.
        /// </summary>
        /// <param name="turnContext">The current turn/execution flow; turnContext of ITurnContext{T}, where T is an IActivity.</param>
        /// <param name="cancellationToken">The cancellation token propagates notifications that the operation(s) should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/>; a unit of execution representing the result of the asynchronous operation.</returns>
        protected override async Task OnEventActivityAsync(ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (turnContext.Activity.Name == "Command")
            {
                if (turnContext.Activity.Value.ToString() == "/block")
                {
                    var interactiveMessage = MessageFactory.Attachment(CreateInteractiveMessage(Directory.GetCurrentDirectory() + @"\Resources\InteractiveMessage.json"));
                    await turnContext.SendActivityAsync(interactiveMessage, cancellationToken).ConfigureAwait(false);
                }
            }

            if (turnContext.Activity.Value is EventType slackEvent)
            {
                if (slackEvent.Type == "message")
                {
                    if (slackEvent.AdditionalProperties.ContainsKey("subtype") &&
                        slackEvent.AdditionalProperties["subtype"].ToString() == "file_share")
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("Echo: I received a file attachment"), cancellationToken).ConfigureAwait(false);
                    }

                    if (slackEvent.AdditionalProperties.ContainsKey("subtype") &&
                        slackEvent.AdditionalProperties["subtype"].ToString() == "link_shared")
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("Echo: I received a link share"), cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }

        private static Attachment CreateInteractiveMessage(string filePath)
        {
            var interactiveMsgJson = System.IO.File.ReadAllText(filePath);
            var adaptiveCardAttachment = JsonConvert.DeserializeObject<Block[]>(interactiveMsgJson);
            var blockList = adaptiveCardAttachment.ToList();
            var attachment = new Attachment
            {
                Content = blockList,
                ContentType = "application/json",
                Name = "blocks",
            };

            return attachment;
        }
    }
}