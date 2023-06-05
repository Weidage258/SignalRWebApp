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