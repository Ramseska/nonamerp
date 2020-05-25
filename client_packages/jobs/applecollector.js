mp.events.add({
    'cCreateNewPoint' : (coords) => {
        mp.events.callRemote('sCreateApplePointKostil', coords);
        mp.gui.chat.push(`cCreateNewPoint called sCreateApplePointKostil with ${coords} points`)
    },
});