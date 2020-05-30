require("./weap.js");
require("./vehicleengine.js");
require("./playercustomize.js");
require("./savepos.js");
require("./applecollector.js")

mp.events.addCommand("anim", (player, _, dict, name, time = 5, flag = 0) => {
    player.outputChatBox(`${player} ${dict} ${name} ${flag}`)
    player.playAnimation(dict, name, time, flag);
});