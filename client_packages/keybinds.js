let keys = require('./keycodes');




mp.keys.bind(keys.key.o, false, () => { // O 
    if(mp.players.local.vehicle == null) return;
    mp.events.callRemote("turnVehicleEngine");
});

mp.keys.bind(keys.key.j, false, () => { // j 
    if(weaponPickerBrowser == null) mp.events.call('CreateWeaponPicker');
    else mp.events.call('DestroyWeaponPicker');
});

mp.keys.bind(keys.key.f5, false, () => { // f5
    mp.gui.cursor.visible = !mp.gui.cursor.visible;
});

mp.keys.bind(keys.key.f6, false, () => { // f6
    let coords = `${mp.players.local.position.x.toFixed(4)}, ${mp.players.local.position.y.toFixed(4)}, ${mp.players.local.position.z.toFixed(4)}`;
    mp.events.callRemote('ESavePosition', `${coords}`);
    mp.gui.chat.push(`Saved position: ${coords}`);
});