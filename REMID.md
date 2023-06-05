# 在线聊天室

ASP.NET Core SignalR 是一个开放源代码库，可用于简化向应用添加实时 Web 功能。 实时 Web 功能使服务器端代码能够将内容推送到客户端。

适合 SignalR 的候选项：

需要从服务器进行高频率更新的应用。 示例包括游戏、社交网络、投票、拍卖、地图和 GPS 应用。

仪表板和监视应用。 示例包括公司仪表板、即时销售更新或旅行警报。

协作应用。 协作应用的示例包括白板应用和团队会议软件。

需要通知的应用。 社交网络、电子邮件、聊天、游戏、旅行警报和很多其他应用都需使用通知。

## **创建 SignalR 中心**

```c#
 /// <summary>
    /// 创建SignalR中心
    /// </summary>
    public class ChatHub : Hub
    {
        private ISqlSugarClient _db;
        public ChatHub(ISqlSugarClient db)
        {
            _db = db;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="message">密码</param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            DateTime time = DateTime.Now;
            //记录消息
            UserMessage userMessage = new UserMessage()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = user,
                Content = message,
                CreateTime = time
            };
            _db.Insertable(userMessage).ExecuteCommand();
            await Clients.All.SendAsync("ReceiveMessage", user, message, time.ToString());
        }
    }
```

## **配置 SignalR**

必须将 SignalR 服务器配置为将 SignalR 请求传递给 SignalR。 

将以下突出显示的代码添加到 Program.cs 文件。

```c#
builder.Services.AddSignalR();

app.MapHub<ChatHub>("/chatHub");
```

## **添加 SignalR 客户端代码**

```js
js中连接服务


"use strict";
//连接服务
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// 禁止发送按钮，直到建立连接
$("#sendButton").hide();

// 建立连接
connection.start().then(function () {
    // 连接成功则显示发送按钮
    $("#sendButton").show();
    console.log("连接成功！")
}).catch(function (err) {
    // 连接失败则直接返回错误消息
    return console.error(err.toString());
});

// 发送消息
$("#sendButton").click(function () {
    var user = $("#userInput").val();
    var message = $("#messageInput").text();
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    })
});

// 接收消息
connection.on("ReceiveMessage", function (user, message, time) {
    $("#content").append(`<p>${user} ${time}</p><p>${message}</p><br>`);
    $("#content").animate({ scrollTop: 100000 });
});

//查看聊天记录
var pageIndex = 1;
$("#findMessage").click(function () {
    $("#historyMessage").fadeIn();
    $.post("/Home/GetMessages", { pageIndex: pageIndex, pageSize: 10 }, function (data) {
        $.each(data, function (i, e) {
            $("#historyMessage").append(`<p>${e.userName} ${e.createTime}</p><p>${e.content}</p><br>`);
        });
    })
});
```



