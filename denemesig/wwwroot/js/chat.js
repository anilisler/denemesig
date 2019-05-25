"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

//Start connecting to hub
connection.start().then(function () {
    console.log("connected to hub successfully!");
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//calling client method "ReceiveMessage" from hub
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    //add new message to list
    document.getElementById("messagesList").appendChild(li);
});
connection.on("UpdateCount", updateOnlineUserCount);
connection.on("UpdateUserList", updateOnlineUserList);
connection.onclose(onConnectionError);

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = "";
    //calling hub method "SendMessage" from client
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function updateOnlineUserCount(userCount) {
    console.log('User count: ' + userCount);
    if (!userCount) return;
    var userCountSpan = document.getElementById('onlineUsersCount');
    userCountSpan.innerText = "Online user count: " + userCount;
};

function updateOnlineUserList(userList) {
    console.log('Online user list:' + userList);
    if (!userList) return;
    document.getElementById("onlineUsersList").innerHTML = "";
    userList.forEach(element => {
        var li = document.createElement("li");
        li.textContent = element;
        document.getElementById("onlineUsersList").appendChild(li);
    });
};

function onConnectionError(error) {
    if (error && error.message) {
        console.error(error.message);
    }
}