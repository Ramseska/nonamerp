mp.events.add('Send_ToChat', (player, message) =>{
    mp.gui.chat.push(`${player.name}(${player.id}): ${message}`);
    console.log('[client]: called with params: ' + player.name + " send message " + message);
});
