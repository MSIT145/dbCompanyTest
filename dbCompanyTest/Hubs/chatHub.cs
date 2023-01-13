using dbCompanyTest.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

            //await Clients.All.SendAsync("UpdList", jsonString);
            //await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId);

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

            Update();

            await Clients.All.SendAsync("UpdSystem", "系統", id.userName + " 已離線");

            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendMessage(string selfID, string message/*, string sendToID*/)
        {//客戶傳訊息
            user user = userList.FirstOrDefault(x => x.connectionId == Context.ConnectionId);
            if (user.userName != "客服人員")
            {
                if (user.userWords == null)
                    user.userWords = new List<string>();
                if (user.waiter == null)
                    user.newWords++;
                else
                    user.newWords = 0;

                user.userWords.Add(message);
                string userName = user.userName;
                await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", message);
                if (user.waiter == null)
                    await Clients.Client(Context.ConnectionId).SendAsync("UpdSystem", userName, "客服全在忙線中請稍後");
                else
                    await Clients.Client(user.waiter).SendAsync("UpdSystem", userName, message);
                Update();
            }
            else
            {
                // 接收人
                //await Clients.Client(sendToID).SendAsync("UpdContent", selfID + " 私訊向你說: " + message);

                // 發送人
                //await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "你向 " + sendToID + " 私訊說: " + message);
            }
        }

        public async Task getName(string userName)
        {//設定使用者名稱
            userList.FirstOrDefault(c => c.connectionId == Context.ConnectionId).userName = userName;
            //if (userName != "客服人員")
            //    await Clients.Others.SendAsync("UpdSystem", "系統", "歡迎 " + userName);
            Update();
        }

        public async Task bindWaiterUser(string userId)
        {
            user olduser = userList.FirstOrDefault(x => x.waiter == Context.ConnectionId);
            if (olduser != null)
                olduser.waiter = null;
            user user = userList.FirstOrDefault(x => x.connectionId == userId);
            user.waiter = Context.ConnectionId;
        }

        public async void Update()
        {
            List<user> client = userList.Where(x => x.userName != "客服人員").ToList();
            string jsonString = JsonConvert.SerializeObject(client);
            List<string> waiter = userList.Where(x => x.userName == "客服人員").Select(x => x.connectionId).ToList();
            foreach (string item in waiter)
                await Clients.Client(item).SendAsync("userList", jsonString);
        }
    }
}