using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace dbCompanyTest.Hubs
{
    public class chatHub : Hub
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

            string jsonString = JsonConvert.SerializeObject(userList);
            //await Clients.All.SendAsync("UpdList", jsonString);


            await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId);


            await Clients.Client(Context.ConnectionId).SendAsync("UpdSystem", "系統", "連線成功");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            user id = userList.Where(p => p.connectionId == Context.ConnectionId).FirstOrDefault();
            if (id != null)
            {
                userList.Remove(id);
            }

            string jsonString = JsonConvert.SerializeObject(userList);
            await Clients.All.SendAsync("UpdList", jsonString);


            await Clients.All.SendAsync("UpdSystem", "系統", id.userName + " 已離線");

            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendMessage(string selfID, string message/*, string sendToID*/)
        {//客戶傳訊息
            if (/*string.IsNullOrEmpty(sendToID)*/true)
            {
                user user = userList.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
                if(user.userWords ==null)
                    user.userWords = new List<string>();
                user.userWords.Add(message);
                string userName = user.userName;
                await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", message);
                if (user.waiter == null)
                    await Clients.Client(Context.ConnectionId).SendAsync("UpdSystem", userName , "客服全在忙線中請稍後");
                else
                    await Clients.Client(user.waiter).SendAsync("UpdSystem", userName, message);
            }
            else
            {
                // 接收人
                //await Clients.Client(sendToID).SendAsync("UpdContent", selfID + " 私訊向你說: " + message);

                // 發送人
                //await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + sendToID + " 私訊說: " + message);
            }
        }

        public async Task getName(string selfID, string userName)
        {//設定使用者名稱
            userList.FirstOrDefault(c => c.connectionId == Context.ConnectionId).userName = userName;
            await Clients.Others.SendAsync("UpdSystem", "系統", "歡迎 " + userName);
        }
    }
}
