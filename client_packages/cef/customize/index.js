var creatorCamera;
var customizeBrowser = null;

const lp = mp.players.local;
var currentSex = true;

mp.events.add("enableCustomize", () => {
    customizeBrowser = mp.browsers.new("package://cef/customize/index.html");
    mp.gui.cursor.show(true, true);
});

mp.events.add("disableCustomize", () => {
    if(customizeBrowser != null)
    {
        customizeBrowser.destroy();
        customizeBrowser = null;
        mp.gui.cursor.show(false, false);
    }
});

mp.events.add("customizeChangeBlendData", (shapeFirstID, shapeSecondID, shapeThirdID, skinFirstID, skinSecondID, skinThirdID, shapeMix, skinMix, thirdMix) => {
    lp.setHeadBlendData(parseInt(shapeFirstID), parseInt(shapeSecondID), parseInt(shapeThirdID), parseInt(skinFirstID), parseInt(skinSecondID), parseInt(skinThirdID), parseFloat(shapeMix), parseFloat(skinMix), thirdMix, false);
});

mp.events.add("customizeChangeSex", (data) => {
    mp.events.callRemote("sCustomizeSetMainData", data);
});

mp.events.add("customizeSetHair", (hair) => {
    mp.events.callRemote("sCustomizeSetHair", hair)
});

mp.events.add("customizeSetFeatures", (data) => {

    const c = JSON.parse(data);
    lp.setFaceFeature(0, parseFloat(c.noseWidth))
    lp.setFaceFeature(1, parseFloat(c.noseHeigth))
    lp.setFaceFeature(2, parseFloat(c.noseLength))
    lp.setFaceFeature(3, parseFloat(c.noseBridge))
    lp.setFaceFeature(4, parseFloat(c.noseTip))
    lp.setFaceFeature(5, parseFloat(c.noseBridgeShift))
    lp.setFaceFeature(6, parseFloat(c.browHeigth))
    lp.setFaceFeature(7, parseFloat(c.browWidth))
    lp.setFaceFeature(8, parseFloat(c.cheekboneHeigth))
    lp.setFaceFeature(9, parseFloat(c.cheekboneWidth))
    lp.setFaceFeature(10, parseFloat(c.cheekWidth))
    lp.setFaceFeature(11, parseFloat(c.eyes))
    lp.setFaceFeature(12, parseFloat(c.lips))
    lp.setFaceFeature(13, parseFloat(c.jawWidth))
    lp.setFaceFeature(14, parseFloat(c.jawHeigth))
    lp.setFaceFeature(15, parseFloat(c.chinLength))
    lp.setFaceFeature(16, parseFloat(c.chinPosition))
    lp.setFaceFeature(17, parseFloat(c.chinWidth))
    lp.setFaceFeature(18, parseFloat(c.chinShape))
    lp.setFaceFeature(19, parseFloat(c.neckWidth))
})

mp.events.add('customizeChangeHairColor', (color1, color2) => {
    mp.events.callRemote('sCustomizeChangeHairColor', color1, color2)
})

mp.events.add('customizeSetHeadOverlay', (data) => {
    
    const c = JSON.parse(data);

    lp.setHeadOverlay(0, parseInt(c.blemishes[1]), parseFloat(c.blemishes[2]), 0, 0);
    lp.setHeadOverlay(1, parseInt(c.facialHair[1]), parseFloat(c.facialHair[2]), 0, 0);
    lp.setHeadOverlay(2, parseInt(c.eyebrows[1]), parseFloat(c.eyebrows[2]), 0, 0);
    lp.setHeadOverlay(3, parseInt(c.ageing[1]), parseFloat(c.ageing[2]), 0, 0);
    lp.setHeadOverlay(4, parseInt(c.makeup[1]), parseFloat(c.makeup[2]), 0, 0);
    lp.setHeadOverlay(5, parseInt(c.blush[1]), parseFloat(c.blush[2]), 0, 0);
    lp.setHeadOverlay(6, parseInt(c.complexion[1]), parseFloat(c.complexion[2]), 0, 0);
    lp.setHeadOverlay(7, parseInt(c.sunDamage[1]), parseFloat(c.sunDamage[2]), 0, 0);
    lp.setHeadOverlay(8, parseInt(c.lipStick[1]), parseFloat(c.lipStick[2]), 0, 0);
    lp.setHeadOverlay(9, parseInt(c.moles[1]), parseFloat(c.moles[2]), 0, 0);
    lp.setHeadOverlay(10, parseInt(c.chestHair[1]), parseFloat(c.chestHair[2]), 0, 0);
    lp.setHeadOverlay(11, parseInt(c.bodyBlemishes[1]), parseFloat(c.bodyBlemishes[2]), 0, 0);
    lp.setHeadOverlay(12, parseInt(c.addBodyBlemishes[1]), parseFloat(c.addBodyBlemishes[2]), 0, 0);
});

mp.events.add('customizeSetCloth', (type, cloth, toros) => {

    if(parseInt(toros) != -1)
        lp.setComponentVariation(3, parseInt(toros), 0, 0);

    lp.setComponentVariation(parseInt(type), parseInt(cloth), 0, 0);
    lp.setComponentVariation(8, 15, 0, 0);
});

mp.events.add('customizeSetDefaultClothes', (sex) => {
    currentSex = JSON.parse(sex);

    setTimeout(() => {
        if(currentSex == true) lp.setComponentVariation(4, 21, 0, 0);
        else lp.setComponentVariation(4, 15, 0, 0);

        lp.setComponentVariation(6, 5, 0, 0);
        lp.setComponentVariation(3, 15, 0, 0);
        lp.setComponentVariation(11, 15, 0, 0);
        lp.setComponentVariation(8, 15, 0, 0);
    }, 500);  
})
//'customizeEnd', JSON.stringify(localData), isInputValid
mp.events.add('customizeEnd', (data, inputs) => {
    for(let i = 0; i < inputs.length; i++)
    {
        if(inputs[i] == false)
        {
            mp.gui.chat.push('[Ошибка]: Введена неверная персональная информация!');
            return;
        }
    }

    mp.events.callRemote('sCustomizeEnd', data);
});