var creatorCamera;

mp.keys.bind(0x4F, false, () => { // O 
    /*
    if(mp.players.local.vehicle == null) return;
    mp.events.callRemote("turnVehicleEngine");
    */
    if(CustomizeBrowser == null) {
        mp.events.call('CreateCustomizeBrowser');

        mp.events.callRemote('tempEvent');

        /* More right 
        creatorCamera = mp.cameras.new("creatorCamera", new mp.Vector3(500.3524,5605.3784,798.6163), new mp.Vector3(0,0,0), 40);
        creatorCamera.pointAtCoord(501.6,5603.7,798.5);
        */
        creatorCamera = mp.cameras.new("creatorCamera", new mp.Vector3(502.0, 5606.9819, 798.7), new mp.Vector3(0,0,0), 40);
        //creatorCamera.pointAtCoord(501.6,5603.7,798.6);

        creatorCamera.pointAtPedBone(mp.players.local.handle, 12844,0,0,0,true);

        creatorCamera.setActive(true);
        mp.game.cam.renderScriptCams(true, false, 0, true, false);
    } 
    else
    {
        mp.events.call('DestroyCustomizeBrowser');
        creatorCamera.setActive(false);
        mp.game.cam.renderScriptCams(false, false, 0, true, false);
        creatorCamera = undefined;
    } 
});

mp.keys.bind(0x4A, false, () => { // j 
    if (loginBrowser != null) return;
    if(weaponPickerBrowser == null) mp.events.call('CreateWeaponPicker');
    else mp.events.call('DestroyWeaponPicker');
});

mp.keys.bind(0x74, false, () => { // f5
    mp.gui.cursor.visible = !mp.gui.cursor.visible;
});