var playSoundBrowser = null;

mp.events.add('playSound', (soundname) => {
    playSoundBrowser = mp.browsers.new("package://cusu/index.html");
    playSoundBrowser.execute(`playSound("${soundname}")`);
    playSoundBrowser.destroy();
    playSoundBrowser = null;
})