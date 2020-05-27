var applejobcheckpoint = null;

mp.events.add({
    'cAppleJobCreateCheckpoint' : (p) => {
        applejobcheckpoint = mp.game.graphics.createCheckpoint(40, p.x, p.y, p.z,p.x, p.y, p.z, 2.0, 255, 255, 255, 255, 0);

        mp.gui.chat.push(`CP Created: ${applejobcheckpoint}`)
        /*
        mp.events.callRemote('sCreateApplePointKostil', coords);
        mp.gui.chat.push(`cCreateNewPoint called sCreateApplePointKostil with ${coords} points`)
        */


    },
    'cAppleJobDestroyCheckpoint' : () => {
        applejobcheckpoint.destroy();
    }
});

mp.events.add("playerEnterCheckpoint", (checkpoint) => {
    mp.gui.chat.push(`Enter on checkpoint: ${checkpoint}`);

    if(applejobcheckpoint == checkpoint)
    {
        mp.gui.chat.push(`this is true cp`);

        // mp.events.callRemote('sStartCollectApples');
    }
});