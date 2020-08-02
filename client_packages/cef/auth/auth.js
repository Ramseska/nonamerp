var authBrowser = null;

mp.events.add({
    'createAuthBrowser' : () => {
        if(authBrowser != null || authBrowser != undefined) return;

        authBrowser = mp.browsers.new("package://cef/auth/index.html");

        mp.gui.chat.activate(false);
        mp.gui.cursor.visible = true;
    },
    'destroyAuthBrowser' : () => {
        if(authBrowser == null || authBrowser == undefined) return;

        authBrowser.destroy();

        mp.gui.chat.activate(true);
        mp.gui.cursor.visible = false;

        authBrowser = null;
    },
    'cancelAuth' : () => {
        mp.events.call('destroyAuthBrowser');

        mp.events.callRemote("Event_CancelAuth");
    },
    'authSendToServer' : (type, login, password, email = "none") => {
        mp.events.callRemote("Event_CheckDataOnValid", type, login, password, email);
    },
    'authSendError' : (error) => {
        authBrowser.execute(`outLog.send("${error}")`);
    }
});