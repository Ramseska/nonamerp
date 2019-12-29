//mp.game.ui.displayRadar(false); // disable radar 

mp.events.add("awfovgbawawgt", (p, w, a) => { mp.events.callRemote("giveWeaponCrutch", p,w,a); });
mp.events.add({
    'playerEnterVehicle': (vehicle, seat) => {
        if (mp.players.local.getSeatIsTryingToEnter() !== -1 || vehicle.getIsEngineRunning()) {
            return;
        }
        vehicle.setEngineOn(false, true, true);
    }
});

require("./cef/weaponpicker");
require("./cef/authorization");
require("./cef/speedo");
require("./cef/color_picker");
require("./keybinds.js");
require("./Sound");
require("./debugUI");
require("./cef/other/house");
require("./cef/reg_customizer");
require("./camfly.js");