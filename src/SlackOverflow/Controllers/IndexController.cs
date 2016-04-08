using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json;
using SlackOverflow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlackOverflow.Controllers
{
    [Route("")]
    public class IndexController : Controller
    {
        private readonly IOptions<GoogleCustomSearchConfiguration> googleCustomSearchConfiguration;
        private readonly IOptions<StackOverflowConfiguration> stackOverflowConfiguration;

        public IndexController(IOptions<GoogleCustomSearchConfiguration> googleCustomSearchConfiguration, IOptions<StackOverflowConfiguration> stackOverflowConfiguration)
        {
            this.googleCustomSearchConfiguration = googleCustomSearchConfiguration;
            this.stackOverflowConfiguration = stackOverflowConfiguration;
        }

        HttpClientHandler handler = new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        [HttpPost("")]
        public async Task<SlackResponse> Post(string text)
        {
            var urlEncoder = new UrlEncoder();
            using (var googleSearchClient = new HttpClient(handler))
            {
                var googleRawResponse = await googleSearchClient.GetAsync(
                    $"{googleCustomSearchConfiguration.Value.APIURL}?q={text}&num=4&cx={urlEncoder.UrlEncode(googleCustomSearchConfiguration.Value.SearchEngineID)}&key={urlEncoder.UrlEncode(googleCustomSearchConfiguration.Value.APIKey)}");

                var googleResponse = JsonConvert.DeserializeObject<GoogleCustomSearchResponse>(await googleRawResponse.Content.ReadAsStringAsync());

                if (googleResponse.items != null)
                {
                    List<ulong> questionIDs = new List<ulong>();
                    foreach (var item in googleResponse.items)
                    {
                        var questionID = getQuestionIDFromURL(item.link);
                        if (questionID != null)
                            questionIDs.Add(questionID.Value);
                    }


                    using (var stackOverflowClient = new HttpClient(handler))
                    {
                        var stackOverflowRawResponse = await stackOverflowClient.GetAsync($"{stackOverflowConfiguration.Value.APIURL}/questions/{string.Join(";", questionIDs)}?site=stackoverflow");
                        var stackOverflowResponse = JsonConvert.DeserializeObject<StackOverflowResponse>(await stackOverflowRawResponse.Content.ReadAsStringAsync());

                        var slackResponse = new SlackResponse
                        {
                            response_type = "in_channel",
                            text = $"Thanks for using *Stack Overflow* instead of asking <@U07083MGT> :clap:\nHere are the first *4* results of *{googleResponse.searchInformation.formattedTotalResults}* for: *{text}*",
                            attachments = googleResponse.items.Select(i =>
                            {
                                var stackQuestion = stackOverflowResponse.items.SingleOrDefault(q => q.question_id == getQuestionIDFromURL(i.link));

                                List<Field> fields = new List<Field>();
                                if (stackQuestion != null)
                                {
                                    fields.Add(new Field
                                    {
                                        title = "Votes",
                                        value = stackQuestion.score.ToString(),
                                        _short = true
                                    });
                                    fields.Add(new Field
                                    {
                                        title = "Answers",
                                        value = stackQuestion.answer_count.ToString(),
                                        _short = true
                                    });
                                }
                                return new Attachment()
                                {
                                    //author_name = stackQuestion.owner.display_name,
                                    //author_link = stackQuestion.owner.link,
                                    title = i.title,
                                    text = i.snippet,
                                    title_link = i.link,
                                    color = stackQuestion == null ? "#c2c2c2" : (stackQuestion.is_answered ? "#49a300" : "#a30000"),
                                    fields = fields.ToArray(),
                                    thumb_url = stackQuestion?.owner.profile_image
                                };
                            }).ToList()
                        };

                        slackResponse.attachments.Add(new Attachment()
                        {
                            title = "See the rest from StackOverflow.com",
                            text = "Open this link to see the rest of the results for StackOverflow",
                            title_link = $"https://www.google.com/search?q={urlEncoder.UrlEncode("site:stackoverflow.com " + text)}",
                            color = "#0266C8"
                        });
                        slackResponse.attachments.Add(new Attachment()
                        {
                            title = "See the rest from all websites",
                            text = "Open this link to see the rest of the results for all websites",
                            title_link = $"https://www.google.com/search?q={urlEncoder.UrlEncode(text)}",
                            color = "#0266C8"
                        });
                        return slackResponse;
                    }
                }
                else
                {
                    return new SlackResponse
                    {
                        response_type = "in_channel",
                        text = $"No StackOverflow results for: *{text}* :cry:"
                    };
                }
            }
        }

        private static ulong? getQuestionIDFromURL(string url)
        {
            var match = Regex.Match(url, @"stackoverflow.com\/questions\/(\d+)", RegexOptions.IgnoreCase);
            if (match.Groups.Count == 2)
                return Convert.ToUInt64(match.Groups[1].Value);
            else
                return null;
        }
    }
}
