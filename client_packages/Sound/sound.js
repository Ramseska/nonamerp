var playSoundBrowser = null;

mp.events.add('playSound', (soundname) => {
    if(playSoundBrowser == null)
        playSoundBrowser = mp.browsers.new("package://sound/index.html");
    playSoundBrowser.execute(`playSound("${soundname}")`);
});