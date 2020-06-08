mp.events.add("playerChat", (player, message) =>{
    player.call('Send_ToChat', [player, message]);
    console.log('[server]: called with params: ' + player + " send message " + message);
});