var hudBrowser = null;

mp.events.add('createHud', (satiety, thirst, money, bank) => {
    if(hudBrowser != null) return;
    hudBrowser = mp.browsers.new('package://cef/hud/index.html');

    let waitInter = setInterval(() => {
        if(hudBrowser != null)
        {
            mp.events.call('updateHud', satiety, thirst, money, bank);
            clearInterval(waitInter);
        }
    }, 1000);
});

mp.events.add('updateHud', (satiety, thirst, money, bank) => {
    if(hudBrowser == null) return;
    hudBrowser.execute(`onUpdateHud(${satiety.toFixed(2)}, ${thirst.toFixed(2)}, ${money.toFixed(2)}, ${bank.toFixed(2)})`);
});

mp.events.add('changeMicStatus', (status) => {
    if(status == true)
    {
        hudBrowser.execute("enableMic()");
    }
    else 
    {
        hudBrowser.execute("disableMic()");
    }
});