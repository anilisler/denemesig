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

    var datetime = "("+new Date().getHours().toString().padStart(2, '0') + ":" + new Date().getMinutes().toString().padStart(2, '0') + ") ";
    var encodedMsg = datetime + user + " -> " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    //add new message to list
    document.getElementById("messagesList").appendChild(li);
    $('#messagesList').animate({scrollTop: $('#messagesList').prop("scrollHeight")}, 500);
});
connection.on("UpdateCount", updateOnlineUserCount);
connection.on("UpdateUserList", updateOnlineUserList);

connection.onclose(onConnectionError);

//send message button click event handler
document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
  
    //calling hub method "SendMessage" from client if user and message aren't empty
    if (user != "" && message != "") {
        document.getElementById("userInput").readOnly = true;
    document.getElementById("messageInput").value = "";
         connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    }
    event.preventDefault();
});

//update user count span according to online user count in the system
function updateOnlineUserCount(userCount) {
    console.log('User count: ' + userCount);
    if (!userCount) return;
    var userCountSpan = document.getElementById('onlineUsersCount');
    userCountSpan.innerText = "Online user count: " + userCount;
};

//update user list according to online user connection ids and names in the system
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