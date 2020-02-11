//mp.game.ui.displayRadar(false); // disable radar 
var pukikaki = null;

mp.events.add('unevhnd', (args) => { mp.events.callRemote('SetPlayerCustomize', args); });
mp.events.add({
    'playerEnterVehicle': (vehicle, seat) => {
        if (mp.players.local.getSeatIsTryingToEnter() !== -1 || vehicle.getIsEngineRunning()) return;
        vehicle.setEngineOn(false, true, true);
    }
});

require("./cef/weaponpicker");
require("./cef/authorization");
require("./cef/speedo");
require("./cef/color_picker");
require("./keybinds.js");
require("./sound");
//require("./debugUI");
require("./cef/other/house");
require("./cef/reg_customizer");
require("./camfly.js");
require("./cef/notify/notify.js");