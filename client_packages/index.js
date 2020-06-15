require("./cef/weaponpicker/index.js");
require("./cef/auth/auth.js");
//require("./cef/speedo");
require("./cef/color_picker");
require("./sound/sound.js");
// require("./debugUI/debugUI.js");
require("./cef/other/house");
//require("./cef/reg_customizer");
require("./camfly.js");
require("./cef/notify/notify.js");
require("./IPLInteriors.js");
require("./voice/voice.js");
require("./cef/customize/index.js");
require("./cef/workdialog/workdialog.js");
require("./cef/hud/hud.js");
require("./cef/appleminigame/appleminigame.js");
require("./jobs/applecollector.js");
require("./keybinds.js");

//mp.game.ui.displayRadar(false); // disable radar 

let blackout = false;

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
		
	if (commandName === "cl") {
        if(args[3] == 0) args[3] = 2;
        mp.gui.chat.push(`You enter command a "${commandName}" [${args[0]}, ${args[1]}, ${args[2]}, ${args[3]}]`);
        mp.players.local.setComponentVariation(Number(args[0]), Number(args[1]), Number(args[2]), Number(args[3]));
    }

    if(commandName === "slt") {
        mp.game.time.setClockTime(Number(args[0]), Number(args[1]), Number(0));
    }
    if(commandName === "bo")
    {
        blackout = !blackout;
        for (let i = 0; i <= 16; i++) mp.game.graphics.setLightsState(i, blackout);
    }
});