mp.keys.bind(0x4F, false, () => { // O 
    if(mp.players.local.vehicle == null) return;
    mp.events.callRemote("turnVehicleEngine");
});

mp.keys.bind(0x4A, true, () => { // j 
    if (loginBrowser != null) return;
    if(weaponPickerBrowser == null) mp.events.call('CreateWeaponPicker');
    else mp.events.call('DestroyWeaponPicker');
});