mp.events.add("giveWeaponCrutch", (player, _, weapon, ammo) => {
    let hash = mp.joaat(weapon);
    player.giveWeapon(hash, ammo);
});