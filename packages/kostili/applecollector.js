mp.events.add('sCreateApplePointKostil', (player, position) => {
    player.data.applecollcp = mp.checkpoints.new(45, new mp.Vector3(position.x, position.y, position.z-1), 1.5, 
    {
        direction: new mp.Vector3(0, 0, 0),
        color: [125, 221, 236, 255],
        visible: false,
        dimension: 0
    });
    player.setVariable("APPLECOL_CURRENT_CP", player.data.applecollcp);

    mp.players.forEach(p => player.data.applecollcp.hideFor(p));

    player.data.applecollcp.showFor(player);
});