"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

//Start connecting to hub
connection.start().then(function () {
    console.log("connected to hub successfully!");
    document.getElementById("sendButton").disabled = false;
    updateOnlineUserCount(connection);
    updateOnlineUserList(connection);
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

function updateOnlineUserCount(connection) {
    var messageCallback = function (message) {
        console.log('message' + message);
        if (!message) return;
        var userCountSpan = document.getElementById('onlineUsersCount');
        userCountSpan.innerText = "Online users: " + message;
    };
    //calling client method "updateCount" from hub
    connection.on("updateCount", messageCallback);
    connection.onclose(onConnectionError);
}

function updateOnlineUserList(connection) {
    var funcCallback = function (userList) {
        console.log('userList' + userList);
        if (!userList) return;
        document.getElementById("onlineUsersList").innerHTML = "";
        userList.forEach(element => {
            var li = document.createElement("li");
            li.textContent = element;
            document.getElementById("onlineUsersList").appendChild(li);
        });
    };
    //calling client method "updateUserList" from hub
    connection.on("updateUserList", funcCallback);
    connection.onclose(onConnectionError);
}

function onConnectionError(error) {
    if (error && error.message) {
        console.error(error.message);
    }
}