let picker_Browser = null;

mp.keys.bind(0x4B, false, () => { // K
    if (picker_Browser != null) return;
    if(mp.players.local.vehicle != null) {
        if (mp.gui.cursor.visible) return;
        if (picker_Browser != null) {
            picker_Browser.destroy();
            picker_Browser = null;
            mp.gui.cursor.show(false, false);
        }
        else {
            picker_Browser = mp.browsers.new("package://cef/color_picker/color_picker.html");
            mp.gui.cursor.show(true, true);
        }
    }
});

mp.events.add("pickerCalled", (r, g, b) => {
    mp.events.callRemote("pickerChange", r, g, b);
});
mp.events.add("pickerClose", () => {
    picker_Browser.destroy();
    picker_Browser = null;
    mp.gui.cursor.show(false, false);
});