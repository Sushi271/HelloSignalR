var team = document.getElementById("team");
var receiver = document.getElementById("receiver");
var outbound = document.getElementById("outbound");
var incoming = document.getElementById("incoming");
var scoreUp = document.getElementById("scoreUp");
var myScore = document.getElementById("myScore");
var abort = document.getElementById("abort");
var log = document.getElementById("log");

var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/hello").build();

connection.on("updateText", (txt) => {
    incoming.value = txt;
});
connection.on("updateScore", (score) => {
    myScore.value = score;
});
connection.on("clientConnected", (client) => {
    log.value += "[" + client + "] has joined the room!\n";
});
connection.on("clientDisconnected", (client) => {
    log.value += "[" + client + "] has left the room!\n";
});

team.addEventListener("change", (ev) => {
    var target = ev.target;
    connection.invoke("SetTeam", target.value);
});
outbound.addEventListener("change", (ev) => {
    var target = ev.target;
    connection.invoke("SyncTextBox", target.value, receiver.value);
});

scoreUp.addEventListener("click", (ev) => {
    connection.invoke("ScoreUp");
})

abort.addEventListener("click", (ev) => {
    connection.invoke("Abort");
})

connection.start();