var weaponPickerBrowser = null;

var weapon_list = 
[
    "weapon_bat",
    "weapon_knife",
    "weapon_battleaxe",
    "weapon_bottle",
    "weapon_crowbar",
    "weapon_dagger",
    "weapon_flashlight",
    "weapon_golfclub",
    "weapon_hammer",
    "weapon_hatchet",
    "weapon_knuckle",
    "weapon_machete",
    "weapon_nightstick",
    "weapon_poolcue",
    "weapon_stone_hatchet",
    "weapon_switchblade",
    "weapon_wrench", 
    "weapon_pistol", // pistols
    "weapon_snspistol",
    "weapon_snspistol_mk2",
    "weapon_pistol50",
    "weapon_combatpistol",
    "weapon_heavypistol",
    "weapon_pistol_mk2",
    "weapon_stungun",
    "weapon_appistol",
    "weapon_doubleaction",
    "weapon_revolver",
    "weapon_revolver_mk2",
    "weapon_vintagepistol",
    "weapon_marksmanpistol",
    "weapon_flaregun",
    "weapon_raypistol",
    "WEAPON_MAKAROV",
    "weapon_machinepistol", // smg
    "weapon_minismg",
    "weapon_microsmg",
    "weapon_smg",
    "weapon_smg_mk2",
    "weapon_assaultsmg",
    "weapon_combatpdw",
    "weapon_raycarbine",
    "weapon_assaultrifle", // assault rifles
    "weapon_assaultrifle_mk2",
    "weapon_bullpuprifle",
    "weapon_bullpuprifle_mk2",
    "weapon_carbinerifle",
    "weapon_carbinerifle_mk2",
    "weapon_specialcarbine",
    "weapon_specialcarbine_mk2",
    "weapon_advancedrifle",
    "weapon_compactrifle",
    "weapon_dbshotgun", // shotguns
    "weapon_sawnoffshotgun", 
    "weapon_pumpshotgun",
    "weapon_pumpshotgun_mk2",
    "weapon_heavyshotgun",
    "weapon_musket",
    "weapon_bullpupshotgun",
    "weapon_assaultshotgun",
    "weapon_autoshotgun",
    "weapon_mg", // machineguns
    "weapon_combatmg",
    "weapon_combatmg_mk2",
    "weapon_gusenberg",
    "weapon_sniperrifle", // sniper rifles
    "weapon_marksmanrifle",
    "weapon_marksmanrifle_mk2",
    "weapon_heavysniper_mk2",
    "weapon_heavyspiner",
    "weapon_compactlauncher", // heavy weapons
    "weapon_firework",
    "weapon_rpg",
    "weapon_hominglauncher",
    "weapon_grenadelauncher",
    "weapon_grenadelauncher_smoke",
    "weapon_minigun",
    "weapon_rayminigun",
    "weapon_railgun",
    "weapon_ball", // metalki
    "weapon_snowball",
    "weapon_flare",
    "weapon_smokegrenade",
    "weapon_bzgas",
    "weapon_molotov",
    "weapon_grenade",
    "weapon_pipebomb",
    "weapon_proxmine",
    "weapon_stickybomb",
    "weapon_parachute", // other 
    "weapon_fireextinguisher",
    "weapon_petrolcan"
];


function initThisFuckingShit()
{
    // var container = document.getElementById('container');

    for(let i = 0; i < weapon_list.length; i++)
    {
        /*
        let newItem = document.createElement('div');
        newItem.className = "item";
        newItem.onclick = function() { mp.trigger('CalledWeaponPicker', weapon_list[i]); };
        container.appendChild(newItem);
        */

        let newItem = document.createElement('div');
        $(newItem).addClass('item').click(function() { mp.trigger('CalledWeaponPicker', weapon_list[i]); }).appendTo($("#container"));

        let itemImg = document.createElement('img');
        itemImg.src = "img/" + weapon_list[i] + ".webp";
        switch(weapon_list[i])
        {
            case "weapon_smokegrenade": case "weapon_bzgas": case "weapon_molotov": case "weapon_parachute": case "weapon_fireextinguisher": itemImg.height = 115;
            case "weapon_petrolcan": itemImg.height = 90;
        }
        newItem.appendChild(itemImg);
    }
}

mp.events.add("CreateWeaponPicker", () => {
    weaponPickerBrowser = mp.browsers.new("package://cef/weaponpicker/index.html");

    mp.gui.chat.activate(false);
    weaponPickerBrowser.visible = true;
    mp.gui.cursor.visible = true;
});

mp.events.add("DestroyWeaponPicker", () => {
    weaponPickerBrowser.destroy();
    mp.gui.cursor.visible = false;
    mp.gui.chat.activate(true);

    weaponPickerBrowser = null;

    mp.game.graphics.transitionFromBlurred(500);
});

mp.events.add("CalledWeaponPicker", (weapon_name) => {
    mp.events.callRemote("giveWeaponCrutch", mp.players.local, weapon_name, 999999);
    mp.gui.chat.push(`Выдано оружие !{f1f8aa}${weapon_name.split('_').join(' ').split('weapon ').join('')}`);
});