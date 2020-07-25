// gui's
require("./cef/weaponpicker/index.js");
require("./cef/auth/auth.js");
require("./cef/color_picker");
require("./cef/other/house");
require("./cef/notify/notify.js");
require("./cef/customize/index.js");
require("./cef/workdialog/workdialog.js");
require("./cef/appleminigame/appleminigame.js");
require("./cef/hud/hud.js");
// jobs
require("./jobs/applecollector.js");
// interiors
require("./IPLInteriors.js");
// other
require("./sound/sound.js");
require("./voice/voice.js");
require("./camfly.js");
require("./customize/customize.js");

// keybinds
require("./keybinds.js"); // forever last



// temp shit >>
//mp.game.ui.displayRadar(false); // disable radar 

let blackout = false;
// temp shit <<


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
    if(commandName === "tt")
    {
        mp.events.call("createTestEvent");
    }
});