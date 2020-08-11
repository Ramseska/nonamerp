// gui's
require("./customize/customize.js");
require("./cef/auth/auth.js");
require("./cef/customize/index.js");
require("./cef/inventory/inventory.js");
require("./cef/hud/hud.js");
require("./cef/notify/notify.js");
require("./cef/color_picker");
require("./cef/weaponpicker/index.js");
require("./cef/other/house");
// jobs
require("./cef/workdialog/workdialog.js");
require("./jobs/applecollector.js");
require("./cef/appleminigame/appleminigame.js");
// interiors
require("./IPLInteriors.js");
// other
require("./sound/sound.js");
require("./voice/voice.js");
require("./camfly.js");

// keybinds
require("./keybinds.js");



// temp shit >>
//mp.game.ui.displayRadar(false); // disable radar 

///let blackout = false;
// temp shit <<

/*
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
*/