﻿


using dbCompanyTest.Hubs;
using dbCompanyTest.Models.LineMess.Dtos.Messages;
using dbCompanyTest.Models.LineMess.Dtos.Messages.Request;
using dbCompanyTest.Models.LineMess.Dtos.Webhook;
using dbCompanyTest.Models.LineMess.Enum;
using dbCompanyTest.Models.LineMess.Providers;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace dbCompanyTest.Models.LineMess.Domain
{
    public class LineBotService
    {
        private readonly string channelAccessToken = "CiB9XbeXDnIXgfN8u7zbtIFGkaxP+VXghErm0tE/bntZJ6M9VZrIKvxUoLT2/38sLsDIXthopd+NwlcX/DT+LJKuOMUp9TJ/VlqVlcrsWjp1cjwFDzaL/2KcN3b+vNRgnP83LrM+iA6QYkFt/VqKiAdB04t89/1O/w1cDnyilFU=";
        private readonly string channelSecret = "dd0a693282da9bd4a90aa2c837787648";

        private readonly string replyMessageUri = "https://api.line.me/v2/bot/message/reply";
        private readonly string broadcastMessageUri = "https://api.line.me/v2/bot/message/broadcast";
        private readonly string pushtMessageUri = "https://api.line.me/v2/bot/message/push";

        private static HttpClient client = new HttpClient(); // 負責處理HttpRequest
        private readonly JsonProvider _jsonProvider = new JsonProvider();


        public LineBotService()
        {
        }

        // <summary>
        // 接收到廣播請求時，在將請求傳至 Line 前多一層處理，依據收到的 messageType 將 messages 轉換成正確的型別，這樣 Json 轉換時才能正確轉換
        // </summary>
        // <param name="messageType"></param>
        // <param name="requestBody"></param>
        public void BroadcastMessageHandler(string messageType, object requestBody)
        {
            string strBody = requestBody.ToString();
            switch (messageType)
            {
                case MessageTypeEnum.Text:
                    var messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<TextMessageDto>>(strBody); //Providers/JsonProvider
                    BroadcastMessage(messageRequest);
                    break;
            }

        }
        // <summary>
        // 將廣播訊息請求送到 Line
        // </summary>
        // <typeparam name="T"></typeparam>
        // <param name="request"></param>
        //todo X-Line-Retry-Key非必要,有時間再弄
        public async void BroadcastMessage<T>(BroadcastMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //-H 'Content-Type: application/json'
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken); //帶入 channel access token  //-H 'Authorization: Bearer {channel access token}'

            var json = _jsonProvider.Serialize(request);//Providers/JsonProvider

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(broadcastMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }













        public void PushMessageHandler(string messageType, string requestBody)//私訊
        {
           // string strBody = requestBody.ToString();

            switch (messageType)
            {
                case MessageTypeEnum.Text:

                    var messageRequest = _jsonProvider.Deserialize<BroadcastMessageRequestDto<TextMessageDto>>(requestBody); //Providers/JsonProvider
                    PushMessage(messageRequest);
                    break;
            }

        }
        public async void PushMessage<T>(BroadcastMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //-H 'Content-Type: application/json'
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken); //帶入 channel access token  //-H 'Authorization: Bearer {channel access token}'

            var json = _jsonProvider.Serialize(request);//Providers/JsonProvider

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(pushtMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }





















        // <summary>
        // 接收到回覆請求時，在將請求傳至 Line 前多一層處理(目前為預留)
        // </summary>
        // <param name="messageType"></param>
        // <param name="requestBody"></param>
        public void ReplyMessageHandler<T>(string messageType, ReplyMessageRequestDto<T> requestBody)//ReplyMessageRequestDto=>LineMess/Dtos/Messages/Request/ReplyMessageRequestDto
        {
            ReplyMessage(requestBody);
        }
        // <summary>
        // 將回覆訊息請求送到 Line
        // </summary>
        // <typeparam name="T"></typeparam>
        // <param name="request"></param>  
        public async void ReplyMessage<T>(ReplyMessageRequestDto<T> request)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", channelAccessToken);
            var json = _jsonProvider.Serialize(request);

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(replyMessageUri),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(requestMessage);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }




















        public void ReceiveWebhook(WebhookRequestBodyDto requestBody)
        {
            foreach (var eventObject in requestBody.Events)
            {
                switch (eventObject.Type)
                {
                    case WebhookEventTypeEnum.Message:

                        //Console.WriteLine("==================");
                        //Console.WriteLine("收到使用者傳送訊息");
                        //Console.WriteLine($"使用者{eventObject.Source.UserId}");
                        //Console.WriteLine(eventObject.Message.Text);
                        var UserName1 = GetUserProfile(eventObject.Source.UserId);
                        var UserName = UserName1.Result.displayName;
                        string LineID_For_ChatHub = $"LINE{eventObject.Source.UserId}";
                        user user;
                        if (chatHub.userList.Exists(X => X.LineID == LineID_For_ChatHub))
                        {
                           user = chatHub.userList.FirstOrDefault(x=> x.LineID== LineID_For_ChatHub);
                            user.userWords.Add(eventObject.Message.Text);
                            if (user.waiter == null)
                                user.newWords++;
                        }
                        else
                        {
                            user = new user();
                            user.userName = UserName;
                            user.connectionId = "wt1oTUm9GsiL3dS2vVhyTw";
                            user.LineID = LineID_For_ChatHub;
                            if (user.userWords == null)
                                user.userWords = new List<string>();
                            user.userWords.Add(eventObject.Message.Text);
                            if (user.waiter == null)
                                user.newWords++;
                            chatHub.userList.Add(user);
                        }
                        
                        //chatHub CH = new chatHub();
                        //CH.
                        //CH.LineSendMessage(user, eventObject.Message.Text);
                        //CH.Update();


                        //var replyMessage = new ReplyMessageRequestDto<TextMessageDto>()
                        //{
                        //    ReplyToken = eventObject.ReplyToken,
                        //    Messages = new List<TextMessageDto>
                        //     {
                        //        new TextMessageDto(){Text = $"\"{eventObject.Message.Text}\"??? 這是自動回復"},
                        //        //new TextMessageDto(){Text = $"You know what, I don't give a shit about what you wanna tell me"}
                        //     }
                        //};
                        //ReplyMessageHandler("text", replyMessage);


                        break;

                    case WebhookEventTypeEnum.Unsend:
                        Console.WriteLine("==================");
                        Console.WriteLine($"使用者{eventObject.Source.UserId}在聊天室收回訊息");
                        break;

                    case WebhookEventTypeEnum.Follow:
                        Console.WriteLine("==================");
                        Console.WriteLine($"使用者{eventObject.Source.UserId}將我們新增為好友");
                        break;

                    case WebhookEventTypeEnum.Unfollow:
                        Console.WriteLine("==================");
                        Console.WriteLine($"使用者{eventObject.Source.UserId}封鎖我們");
                        break;

                    case WebhookEventTypeEnum.Join:
                        Console.WriteLine("==================");
                        Console.WriteLine("我們被邀請進入聊天室");
                        break;

                    case WebhookEventTypeEnum.Leave:
                        Console.WriteLine("==================");
                        Console.WriteLine("我們被聊天室踢出");
                        break;

                    case WebhookEventTypeEnum.MemberJoined:
                        string joinedMemberIds = "";
                        foreach (var member in eventObject.Joined.Members)
                        {
                            joinedMemberIds += $"{member.UserId} ";
                        }
                        Console.WriteLine("==================");
                        Console.WriteLine($"使用者{joinedMemberIds}加入群組");
                        break;

                    case WebhookEventTypeEnum.MemberLeft:
                        string leftMemberIds = "";
                        foreach (var member in eventObject.Left.Members)
                        {
                            leftMemberIds += $"{member.UserId} ";
                        }
                        Console.WriteLine("==================");
                        Console.WriteLine($"使用者{leftMemberIds}離開群組");
                        break;

                    case WebhookEventTypeEnum.Postback:
                        Console.WriteLine("==================");
                        Console.WriteLine($"使用者{eventObject.Source.UserId}觸發postback事件");
                        break;

                    case WebhookEventTypeEnum.VideoPlayComplete:
                        Console.WriteLine("==================");
                        Console.WriteLine($"使用者{eventObject.Source.UserId}看過影片");
                        break;
                }
            }
        }

        public class UserProfile
        {
            public string displayName { get; set; }
            public string userId { get; set; }
            public string pictureUrl { get; set; }
            public string statusMessage { get; set; }
        }




        private async Task<UserProfile> GetUserProfile(string userId)
        {
            //GET https://api.line.me/v2/bot/profile/{userId}
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.line.me/v2/bot/profile/{userId}"),
                Headers = {
                    { "Authorization", $"Bearer {channelAccessToken}" },
                }
            };
            var result = await client.SendAsync(httpRequestMessage);

            var content = await result.Content.ReadAsStringAsync();

            var profile = JsonConvert.DeserializeObject<UserProfile>(content);

            return profile;
        }
    }
}
