mp.keys.bind(0x4F, false, () => { // O 
    if(mp.players.local.vehicle == null) return;
    mp.events.callRemote("turnVehicleEngine");
});

mp.keys.bind(0x4A, false, () => { // j 
    if(weaponPickerBrowser == null) mp.events.call('CreateWeaponPicker');
    else mp.events.call('DestroyWeaponPicker');
});

mp.keys.bind(0x74, false, () => { // f5
    mp.gui.cursor.visible = !mp.gui.cursor.visible;
});

mp.keys.bind(0x75, false, () => { // f6
    let coords = `${mp.players.local.position.x.toFixed(4)}, ${mp.players.local.position.y.toFixed(4)}, ${mp.players.local.position.z.toFixed(4)}`;
    mp.events.callRemote('ESavePosition', `${coords}`);
    mp.gui.chat.push(`Saved position: ${coords}`);
});

mp.keys.bind(0x49, false, () => { // i
    if(mp.gui.cursor.visible == true && inventoryBrowser.active == false) return;

    if(inventoryBrowser.active) mp.events.call("InventoryClose");
    else mp.events.call("InventoryOpen");
});