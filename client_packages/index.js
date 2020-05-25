//mp.game.ui.displayRadar(false); // disable radar 
var pukikaki = null;

mp.events.add('setPlayerCustomize', (args) => { mp.events.callRemote('sSetPlayerCustomize', args); });
mp.events.add('setPlayerClothes', (args) => { mp.events.callRemote('sSetPlayerClothes', args); });

mp.events.add({
    'playerEnterVehicle': (vehicle, seat) => {
        if (mp.players.local.getSeatIsTryingToEnter() !== -1 || vehicle.getIsEngineRunning()) return;
        vehicle.setEngineOn(false, true, true);
    }
});

mp.events.add("playerCommand", (command) => {
	const args = command.split(/[ ]+/);
	const commandName = args[0];

	args.shift();
		
	if (commandName === "pup") {
        mp.gui.chat.push(`You enter command a "${commandName}" [${args[0]}, ${args[1]}]`);
        mp.players.local.setComponentVariation(Number(args[0]), Number(args[1]), 0, 2);
    }
});

mp.gui.chat.isChatActive = false;

require("./cef/weaponpicker");
require("./cef/authorization");
//require("./cef/speedo");
require("./cef/color_picker");
require("./keybinds.js");
require("./sound/sound.js");
require("./debugUI/debugUI.js");
require("./cef/other/house");
//require("./cef/reg_customizer");
require("./camfly.js");
require("./cef/notify/notify.js");
require("./ints.js")
require("./voice/voice.js");
require("./cef/customize/index.js");
require("./cef/workdialog/workdialog.js");
require("./cef/hud/hud.js");
require("./cef/appleminigame/appleminigame.js");
require("./jobs/applecollector.js");