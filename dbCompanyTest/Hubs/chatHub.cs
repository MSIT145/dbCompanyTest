using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace dbCompanyTest.Hubs
{
    public class chatHub:Hub
    {
        // 用戶連線 ID 列表
        public static List<user> userList = new List<user>();

        public override async Task OnConnectedAsync()
        {

            if (userList.Where(p => p.connectionId == Context.ConnectionId).FirstOrDefault() == null)
            {
                user newuser = new user();
                newuser.connectionId = Context.ConnectionId;
                userList.Add(newuser);
            }
            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(userList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新個人 ID
            //await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId);

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "新連線 ID: " + Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            user id = userList.Where(p => p.connectionId == Context.ConnectionId).FirstOrDefault();
            if (id != null)
            {
                userList.Remove(id);
            }
            // 更新連線 ID 列表
            string jsonString = JsonConvert.SerializeObject(userList);
            await Clients.All.SendAsync("UpdList", jsonString);

            // 更新聊天內容
            await Clients.All.SendAsync("UpdContent", "已離線 ID: " + Context.ConnectionId);

            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendMessage(string selfID, string message/*, string sendToID*/)
        {
            if (/*string.IsNullOrEmpty(sendToID)*/true)
            {
                await Clients.All.SendAsync("UpdContent", selfID + " 說: " + message);
            }
            else
            {
                // 接收人
                //await Clients.Client(sendToID).SendAsync("UpdContent", selfID + " 私訊向你說: " + message);

                // 發送人
                //await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + sendToID + " 私訊說: " + message);
            }
        }
    }
}
