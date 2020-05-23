var debugUIBrowser = null;
var debugUIInterval = null;

mp.events.add({
    'createDebugUI': () => {
        debugUIBrowser = mp.browsers.new('package://debugUI/index.html');

        mp.events.call('updateDebugCoords');

        debugUIInterval = setInterval(() => {
            updateDebugCoords();
        }, 100);
    }
});

function updateDebugCoords()
{
    const player = mp.players.local;
    debugUIBrowser.execute(`document.getElementById("coords").innerHTML = "X: ${player.position.x.toFixed(4)} Y: ${player.position.y.toFixed(4)} Z: ${player.position.z.toFixed(4)} | R: ${player.getRotation(0).z.toFixed(4)}"`);
}