mp.events.add('sCustomizeSetMainData', (player, data) => {
    
    const c = JSON.parse(data);

    player.setCustomization(
        c.base.sex, 
        parseInt(c.parents.mother), 
        parseInt(c.parents.father), 
        0, 
        parseInt(c.parents.mother), 
        parseInt(c.parents.father), 
        0, 
        parseFloat(c.parents.parentsMix), 
        parseFloat(c.parents.skinMix), 
        0, 
        parseInt(0), 
        parseInt(c.hair[1]), 
        parseInt(c.hair[2]), 
        [
            parseFloat(c.features.noseWidth),
            parseFloat(c.features.noseHeigth),
            parseFloat(c.features.noseLength),
            parseFloat(c.features.noseBridge),
            parseFloat(c.features.noseTip),
            parseFloat(c.features.noseBridgeShift),
            parseFloat(c.features.browHeight),
            parseFloat(c.features.browWidth),
            parseFloat(c.features.cheekboneHeight),
            parseFloat(c.features.cheekboneWidth),
            parseFloat(c.features.cheekWidth),
            parseFloat(c.features.eyes),
            parseFloat(c.features.lips),
            parseFloat(c.features.jawWidth),
            parseFloat(c.features.jawHeight),
            parseFloat(c.features.chinLength),
            parseFloat(c.features.chinPosition),
            parseFloat(c.features.chinWidth),
            parseFloat(c.features.chinShape),
            parseFloat(c.features.neckWidth)
        ]
    );
    // player.setHairColor(5, 0);
});

mp.events.add('sCustomizeSetHair', (player, hair) => {
    player.setClothes(2, parseInt(hair), 0, 2);
});

mp.events.add('sCustomizeChangeHairColor', (player, c1, c2) => {
    player.setHairColor(parseInt(c1), parseInt(c2));
});

mp.events.add('sCustomizeEnd', (player, data) => {

});

mp.events.add('applyCustomize', (player, data) => {

});