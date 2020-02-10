var debugBrowser = null;
var interDebugBrowser = null;

mp.events.add('CreateDebugBrowser', () => {
    const player = mp.players.local;
    debugBrowser = mp.browsers.new("package://debugUI/index.html");

    interDebugBrowser = setInterval(() => {
        debugBrowser.execute(`document.getElementById("inside-content").innerHTML = "\
        <font color='ffff00'>Position:<br><font color='ffffff'>\
        X: ${player.position.x.toFixed(4)}<br>\
        Y: ${player.position.y.toFixed(4)}<br>\
        Z: ${player.position.z.toFixed(4)}<br>\
        R: ${player.getRotation(0).z.toFixed(4)}<br><br>\
        <font color='ffff00'>Dimension: <font color='ffffff'>${player.dimension}<br>\
        <font color='ffff00'>Vehicle ID: <font color='ffffff'>${player.vehicle == null ? (-1) : (player.vehicle.id)}<br>\
        <font color='ffff00'>Health: <font color='ffffff'>${player.getHealth()}<br>\
        <font color='ffff00'>Armour: <font color='ffffff'>${player.getArmour()}<br>\
        <font color='ffff00'>Speed: <font color='ffffff'>${player.getSpeed().toFixed(4)}<br><br>\
        <font color='ffff00'>Camera position: <font color='ffffff'>X: ${getCameraPosition().x.toFixed(4)}<br>\
        <font color='ffff00'>Camera position: <font color='ffffff'>Y: ${getCameraPosition().y.toFixed(4)}<br>\
        <font color='ffff00'>Camera position: <font color='ffffff'>Z: ${getCameraPosition().z.toFixed(4)}<br><br>\
        <font color='ffff00'>Camera direction: <font color='ffffff'>X: ${getCameraDirection().x.toFixed(4)}<br>\
        <font color='ffff00'>Camera direction: <font color='ffffff'>Y: ${getCameraDirection().y.toFixed(4)}<br>\
        <font color='ffff00'>Camera direction: <font color='ffffff'>Z: ${getCameraDirection().z.toFixed(4)}<br>"`);
    },  1);
});

mp.events.add('DestroyDebugBrowser', () => {
    debugBrowser.destroy();
    debugBrowser = null;
    clearInterval(interDebugBrowser);
});

function getCameraPosition() {
    return mp.cameras.new("gameplay").getCoord();
}
function getCameraDirection() {
    return mp.cameras.new("gameplay").getDirection();
}