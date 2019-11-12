var CEF_Speedo = null;
var interSpeedo = null;

mp.events.add("createSpeedo", () => {

    let vehicle = mp.players.local.vehicle;

    CEF_Speedo = mp.browsers.new("package://cef/speedo/index.html");

    interSpeedo = setInterval(() => {
        CEF_Speedo.execute(`document.getElementById("speed").innerHTML = "Speed: ${(vehicle.getSpeed() * 3.6).toFixed()} km\\h"`);
        CEF_Speedo.execute(`document.getElementById("rpm").innerHTML = "RPM: ${(vehicle.rpm).toFixed(1)}"`);
        CEF_Speedo.execute(`document.getElementById("gear").innerHTML = "Gear: ${vehicle.gear}"`);
    }, 50);
});

mp.events.add("destroySpeedo", () => {
    clearInterval(interSpeedo);
    CEF_Speedo.destroy();
    CEF_Speedo = interSpeedo = null;
});