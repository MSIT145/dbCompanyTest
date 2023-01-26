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
                userList.Remove(id);

            Update();
            if (id.waiter != null)
                await Clients.Client(id.waiter).SendAsync("UpdSystem", "系統", id.userName + " 已離線");

            await base.OnDisconnectedAsync(ex);
        }

        public async Task SendMessage(string ClientID, string message/*, string sendToID*/)
        {//客戶傳訊息
            if (message != "")
            {
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
                        await Clients.Client(Context.ConnectionId).SendAsync("UpdSystem", "系統", "客服全在忙線中請稍後");
                    else
                        await Clients.Client(user.waiter).SendAsync("UpdSystem", userName, message);
                    Update();
                }
                else
                {
                    user clients = userList.FirstOrDefault(x => x.connectionId == ClientID);
                    clients.userWords.Add("S" + message);
                    await Clients.Client(ClientID).SendAsync("UpdSystem", user.userName, message);
                    await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", message);
                }
            }
        }

        public async Task getName(string userName)
        {//設定使用者名稱
            userList.FirstOrDefault(c => c.connectionId == Context.ConnectionId).userName = userName;
            Update();
        }

        public async Task bindWaiterUser(string userId)
        {
            user olduser = userList.FirstOrDefault(x => x.waiter == Context.ConnectionId);
            if (olduser != null)
                olduser.waiter = null;
            user user = userList.FirstOrDefault(x => x.connectionId == userId);
            if (user.waiter == null)
            {
                user.waiter = Context.ConnectionId;
                string userMsgJSON = JsonConvert.SerializeObject(user.userWords);
                user.newWords = 0;
                await Clients.Client(Context.ConnectionId).SendAsync("newClientMsg", userMsgJSON);
                Update();
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("UpdSystem","系統" ,"此客戶已有人在回應");
            }
        }

        public async void Update()
        {
            List<user> client = userList.Where(x => x.userName != "客服人員").ToList();
            string jsonString = JsonConvert.SerializeObject(client);
            List<string> waiter = userList.Where(x => x.userName == "客服人員").Select(x => x.connectionId).ToList();
            foreach (string item in waiter)
                await Clients.Client(item).SendAsync("userList", jsonString);
        }

        public async Task getStaffNum(string StaffNum)
        {
            userList.FirstOrDefault(c => c.connectionId == Context.ConnectionId).userName = StaffNum;
        }

    }


}