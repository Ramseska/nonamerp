mp.events.add('sCustomizeSetMainData', (player, data) => {
    
    const c = JSON.parse(data);

    player.setCustomization(
        c.sex, 

        parseInt(c.mother), 
        parseInt(c.father), 
        0, 
        
        parseInt(c.motherSkin), 
        parseInt(c.fatherSkin), 
        0, 
        
        parseFloat(c.parentsMix), 
        parseFloat(c.skinMix), 
        0,

        parseInt(0), 
        parseInt(c.hair[1]), 
        parseInt(c.hair[2]), 
        [
            parseFloat(c.noseWidth),
            parseFloat(c.noseHeigth),
            parseFloat(c.noseLength),
            parseFloat(c.noseBridge),
            parseFloat(c.noseTip),
            parseFloat(c.noseBridgeShift),
            parseFloat(c.browHeight),
            parseFloat(c.browWidth),
            parseFloat(c.cheekboneHeight),
            parseFloat(c.cheekboneWidth),
            parseFloat(c.cheekWidth),
            parseFloat(c.eyes),
            parseFloat(c.lips),
            parseFloat(c.jawWidth),
            parseFloat(c.jawHeight),
            parseFloat(c.chinLength),
            parseFloat(c.chinPosition),
            parseFloat(c.chinWidth),
            parseFloat(c.chinShape),
            parseFloat(c.neckWidth)
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


mp.events.add('sSetPlayerCustomize', (player, cust) => {
    const c = JSON.parse(cust);

    player.setCustomization(
        c.sex, 

        parseInt(c.mother), 
        parseInt(c.father), 
        0, 
        
        parseInt(c.motherSkin), 
        parseInt(c.fatherSkin), 
        0, 
        
        parseFloat(c.parentsMix), 
        parseFloat(c.skinMix), 
        0,

        parseInt(0), 
        parseInt(c.hair[1]), 
        parseInt(c.hair[2]), 
        [
            parseFloat(c.noseWidth),
            parseFloat(c.noseHeigth),
            parseFloat(c.noseLength),
            parseFloat(c.noseBridge),
            parseFloat(c.noseTip),
            parseFloat(c.noseBridgeShift),
            parseFloat(c.browHeight),
            parseFloat(c.browWidth),
            parseFloat(c.cheekboneHeight),
            parseFloat(c.cheekboneWidth),
            parseFloat(c.cheekWidth),
            parseFloat(c.eyes),
            parseFloat(c.lips),
            parseFloat(c.jawWidth),
            parseFloat(c.jawHeight),
            parseFloat(c.chinLength),
            parseFloat(c.chinPosition),
            parseFloat(c.chinWidth),
            parseFloat(c.chinShape),
            parseFloat(c.neckWidth)
        ]
    );

    player.setHeadOverlay(0, [parseInt(c.headOverlay.blemishes[1]), parseFloat(c.headOverlay.blemishes[2]), 0, 0]);
    player.setHeadOverlay(1, [parseInt(c.headOverlay.facialHair[1]), parseFloat(c.headOverlay.facialHair[2]), 0, 0]);
    player.setHeadOverlay(2, [parseInt(c.headOverlay.eyebrows[1]), parseFloat(c.headOverlay.eyebrows[2]), 0, 0]);
    player.setHeadOverlay(3, [parseInt(c.headOverlay.ageing[1]), parseFloat(c.headOverlay.ageing[2]), 0, 0]);
    player.setHeadOverlay(4, [parseInt(c.headOverlay.makeup[1]), parseFloat(c.headOverlay.makeup[2]), 0, 0]);
    player.setHeadOverlay(5, [parseInt(c.headOverlay.blush[1]), parseFloat(c.headOverlay.blush[2]), 0, 0]);
    player.setHeadOverlay(6, [parseInt(c.headOverlay.complexion[1]), parseFloat(c.headOverlay.complexion[2]), 0, 0]);
    player.setHeadOverlay(7, [parseInt(c.headOverlay.sunDamage[1]), parseFloat(c.headOverlay.sunDamage[2]), 0, 0]);
    player.setHeadOverlay(8, [parseInt(c.headOverlay.lipStick[1]), parseFloat(c.headOverlay.lipStick[2]), 0, 0]);
    player.setHeadOverlay(9, [parseInt(c.headOverlay.moles[1]), parseFloat(c.headOverlay.moles[2]), 0, 0]);
    player.setHeadOverlay(10, [parseInt(c.headOverlay.chestHair[1]), parseFloat(c.headOverlay.chestHair[2]), 0, 0]);
    player.setHeadOverlay(11, [parseInt(c.headOverlay.bodyBlemishes[1]), parseFloat(c.headOverlay.bodyBlemishes[2]), 0, 0]);
    player.setHeadOverlay(12, [0, 0, 0, 0]);

    setTimeout(() => {
        player.setClothes(2, parseInt(c.hair[0]), 0, 2);
        player.setHairColor(parseInt(c.hair[1]), parseInt(c.hair[2]));
    }, 1000)
});

mp.events.add('sSetPlayerClothes', (player, cloth) => {
    const c = JSON.parse(cloth);
    
    player.setClothes(1, c[1].drawable, c[1].texture, c[1].palette);
    player.setClothes(2, c[2].drawable, c[2].texture, c[2].palette);
    player.setClothes(3, c[3].drawable, c[3].texture, c[3].palette);
    player.setClothes(4, c[4].drawable, c[4].texture, c[4].palette);
    player.setClothes(5, c[5].drawable, c[5].texture, c[5].palette);
    player.setClothes(6, c[6].drawable, c[6].texture, c[6].palette);
    player.setClothes(7, c[7].drawable, c[7].texture, c[7].palette);
    player.setClothes(8, c[8].drawable, c[8].texture, c[8].palette);
    player.setClothes(9, c[9].drawable, c[9].texture, c[9].palette);
    player.setClothes(10, c[10].drawable, c[10].texture, c[10].palette);
    player.setClothes(11, c[11].drawable, c[11].texture, c[11].palette);
});