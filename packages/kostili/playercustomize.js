mp.events.add('SetPlayerCustomize', (player, cust) => {
    let c = JSON.parse(cust);

    player.customizeparams = cust;

    player.setCustomization(
        c.sex, 
        parseInt(c.mother), 
        parseInt(c.father), 
        0, 
        parseInt(c.skin_1), 
        parseInt(c.skin_2), 
        0, 
        parseFloat(c.parentsMix), 
        parseFloat(c.skinMix), 
        0, 
        parseInt(c.eyeColor), 
        parseInt(c.hairColor), 
        parseInt(c.hilightColor), 
        [
            parseFloat(c.noseWidth),
            parseFloat(c.noseHeight),
            parseFloat(c.noseLength),
            parseFloat(c.noseBridge),
            parseFloat(c.noseTip),
            parseFloat(c.noseBridgeShift),
            parseFloat(c.browHeight),
            parseFloat(c.browWidth),
            parseFloat(c.cheekboneHeight),
            parseFloat(c.cheekboneWidth),
            parseFloat(c.cheeksWidth),
            parseFloat(c.eyesWidth),
            parseFloat(c.lips),
            parseFloat(c.jawWidth),
            parseFloat(c.jawHeight),
            parseFloat(c.chitHeight),
            parseFloat(c.chinPosition),
            parseFloat(c.chinWidth),
            parseFloat(c.chinShape),
            parseFloat(c.neckWidth)
        ]);
    player.setClothes(2, parseInt(c.hair), 0, 2);
});

mp.events.add('FinishedCustomize', (player) => {
    console.log(`Customize settings for player ${player.name}: \n${player.customizeparams}`);
})

