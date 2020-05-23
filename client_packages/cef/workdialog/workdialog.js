var workDialogBrowser = null;

mp.events.add({
    'createWorkDialog': (workName, workDesk) =>
    {
        workDialogBrowser = mp.browsers.new('package://cef/workdialog/index.html');
        mp.gui.cursor.visible = true;
        mp.gui.chat.activate(false);

        workDialogBrowser.execute(`createWorkBrowser('${workName}', '${workDesk}')`); 
    },
    'destroyWorkDialog': () => {
        mp.gui.cursor.visible = false;
        mp.gui.chat.activate(true);
        workDialogBrowser.destroy();
        workDialogBrowser = null;
    },
    'onAcceptWork': () => {
        mp.events.callRemote('sAcceptJob');
        mp.events.call('destroyWorkDialog');
    }
});