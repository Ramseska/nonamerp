var appleCollectorApp = null; 

mp.events.add({
    'createAppleCollectorApp' : () => {
        if(appleCollectorApp != null) return;

        appleCollectorApp = mp.browsers.new('package://cef/appleminigame/index.html')

        mp.gui.chat.activate(false);
        mp.gui.cursor.visible = true;
    },

    'endAppleCollectApp' : () => {
        mp.events.callRemote('sEndCollectApples');
        mp.events.call('destroyAppleCollectorApp');
    },

    'destroyAppleCollectorApp' : () => {
        if(appleCollectorApp == null || appleCollectorApp == undefined) return;
        
        appleCollectorApp.destroy();

        mp.gui.chat.activate(true);
        mp.gui.cursor.visible = false;

        appleCollectorApp = null;
    },

    'onTakedApple' : () => {
        mp.events.call('playSound', "vetki");   
    }
});