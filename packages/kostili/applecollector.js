mp.events.add('sCreateApplePointKostil', (player, position) => {
    player.data.applecollcp = mp.checkpoints.new(0, new mp.Vector3(position.x, position.y, position.z-1), 1, {visible: false});
    player.data.applecollcp.showFor(player);

    player.setVariable("APPLECOL_CURRENTCP", player.data.applecollcp);
    
    player.outputChatBox("On js cp: " + player.getVariable("APPLECOL_CURRENTCP"))
});