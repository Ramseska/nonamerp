var creatorCamera;
var customizeBrowser = null;

const lp = mp.players.local;
var currentSex = true;
var tout = null;

mp.events.add("enableCustomize", () => {
    customizeBrowser = mp.browsers.new("package://cef/customize/index.html");
    mp.gui.cursor.visible = true;
    mp.gui.chat.activate(false);
    mp.game.ui.displayRadar(false);

    mp.gui.chat.push(`!{ffff00}[INFO]!{ffffff} Используйте зажатую парвую кнопку мыши, что бы вращать камеру.`)

    creatorCamera = mp.cameras.new("creatorCamera", new mp.Vector3(402.8664, -997.5515, -98.5), new mp.Vector3(0,0,20), 60);
    creatorCamera.setCoord(402.8664, -997.5515, -98.5);
    
    setTimeout(() => {
        creatorCamera.pointAtPedBone(mp.players.local.handle, 12844,0,0,0,true);
    }, 2000);

    creatorCamera.setActive(true);
    mp.game.cam.renderScriptCams(true, false, 0, true, false);

    let currentCamPos = creatorCamera.getCoord();
    const sensivityAxis = 0.2;

    tout = setInterval(() => {
        if(mp.keys.isDown(2) === true)
        {
            if(!currentCamPos.x + mp.game.controls.getDisabledControlNormal(0, 220) * sensivityAxis > 403.7 || !currentCamPos.x + mp.game.controls.getDisabledControlNormal(0, 220) * sensivityAxis < 401.7)
            {
                currentCamPos.x = currentCamPos.x + mp.game.controls.getDisabledControlNormal(0, 220) * sensivityAxis;
            }
            if(!currentCamPos.z + -mp.game.controls.getDisabledControlNormal(0, 221) * sensivityAxis > -97.5 || !currentCamPos.z + -mp.game.controls.getDisabledControlNormal(0, 221) * sensivityAxis < 99.2)
            {
                currentCamPos.z = currentCamPos.z + -mp.game.controls.getDisabledControlNormal(0, 221) * sensivityAxis;
            }

            if(currentCamPos.x + mp.game.controls.getDisabledControlNormal(0, 220) * sensivityAxis > 403.7)
            {
                currentCamPos.x = 403.7;
            }
            else if(currentCamPos.x + mp.game.controls.getDisabledControlNormal(0, 220) * sensivityAxis < 401.7)
            {
                currentCamPos.x = 401.7;
            }
            else 
            {
                currentCamPos.x = currentCamPos.x + mp.game.controls.getDisabledControlNormal(0, 220) * sensivityAxis;
            }

            if(currentCamPos.z + -mp.game.controls.getDisabledControlNormal(0, 221) * sensivityAxis > -97.5)
            {
                currentCamPos.z = -97.5;
            }
            else if(currentCamPos.z + -mp.game.controls.getDisabledControlNormal(0, 221) * sensivityAxis < -99.2)
            {
                currentCamPos.z = -99.2;
            }
            else 
            {
                currentCamPos.z = currentCamPos.z + -mp.game.controls.getDisabledControlNormal(0, 221) * sensivityAxis;    
            }

            creatorCamera.setCoord(currentCamPos.x, currentCamPos.y, currentCamPos.z);
        }
    }, 1);
});

mp.events.add("disableCustomize", () => {
    customizeBrowser.destroy();
    mp.gui.chat.activate(true);
    customizeBrowser = null;
    mp.gui.cursor.visible = false;
    mp.game.ui.displayRadar(true);

    clearInterval(tout);
    
    creatorCamera.setActive(false);
    mp.game.cam.renderScriptCams(false, false, 0, true, false);
    creatorCamera = undefined;
});

mp.events.add('customizeCameraZoom', (kuda) => {
    switch(kuda)
    {
        case 0:
            if(creatorCamera.getFov() >= 94)
                creatorCamera.setFov(94);
            else
                creatorCamera.setFov((creatorCamera.getFov() + 0.5));
            break;
        case 1:
            if(creatorCamera.getFov() <= 40)
                creatorCamera.setFov(40);
            else
                creatorCamera.setFov((creatorCamera.getFov() - 0.5));
            break;
    }
});

mp.events.add('customizeCameraDirOn', (bone) => {
    switch(bone)
    {
        case 0: 
            creatorCamera.pointAtPedBone(mp.players.local.handle, 12844,0,0,0,true);
            break;
        case 1:
            creatorCamera.pointAtPedBone(mp.players.local.handle, 0,0,0,0,true);
            break; 
    }
})

mp.events.add({
    'localSetCustomize': (data) => {
        let c = JSON.parse(data);

        lp.setHeadBlendData(parseInt(c.mother), parseInt(c.father), parseInt(0), parseInt(c.motherSkin), parseInt(c.fatherSkin), parseInt(0), parseFloat(c.parentsMix), parseFloat(c.skinMix), parseInt(0), false);
        
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

        lp.setHeadOverlay(0, parseInt(c.headOverlay.blemishes[1]), parseFloat(c.headOverlay.blemishes[2]), 0, 0);
        lp.setHeadOverlay(1, parseInt(c.headOverlay.facialHair[1]), parseFloat(c.headOverlay.facialHair[2]), 0, 0);
        lp.setHeadOverlay(2, parseInt(c.headOverlay.eyebrows[1]), parseFloat(c.headOverlay.eyebrows[2]), 0, 0);
        lp.setHeadOverlay(3, parseInt(c.headOverlay.ageing[1]), parseFloat(c.headOverlay.ageing[2]), 0, 0);
        lp.setHeadOverlay(4, parseInt(c.headOverlay.makeup[1]), parseFloat(c.headOverlay.makeup[2]), 0, 0);
        lp.setHeadOverlay(5, parseInt(c.headOverlay.blush[1]), parseFloat(c.headOverlay.blush[2]), 0, 0);
        lp.setHeadOverlay(6, parseInt(c.headOverlay.complexion[1]), parseFloat(c.headOverlay.complexion[2]), 0, 0);
        lp.setHeadOverlay(7, parseInt(c.headOverlay.sunDamage[1]), parseFloat(c.headOverlay.sunDamage[2]), 0, 0);
        lp.setHeadOverlay(8, parseInt(c.headOverlay.lipStick[1]), parseFloat(c.headOverlay.lipStick[2]), 0, 0);
        lp.setHeadOverlay(9, parseInt(c.headOverlay.moles[1]), parseFloat(c.headOverlay.moles[2]), 0, 0);
        lp.setHeadOverlay(10, parseInt(c.headOverlay.chestHair[1]), parseFloat(c.headOverlay.chestHair[2]), 0, 0);
        lp.setHeadOverlay(11, parseInt(c.headOverlay.bodyBlemishes[1]), parseFloat(c.headOverlay.bodyBlemishes[2]), 0, 0);
        lp.setHeadOverlay(12, 0, 0, 0, 0);

        mp.events.callRemote("sCustomizeSetHair", c.hair[0])
        mp.events.callRemote('sCustomizeChangeHairColor', c.hair[1], c.hair[2])
    },
    'localCustomizeSetDefaultClothes': (sex) => {
        currentSex = JSON.parse(sex);
    
        setTimeout(() => {
            if(currentSex == true) lp.setComponentVariation(4, 21, 0, 0);
            else lp.setComponentVariation(4, 15, 0, 0);
    
            lp.setComponentVariation(6, 5, 0, 0);
            lp.setComponentVariation(3, 15, 0, 0);
            lp.setComponentVariation(11, 15, 0, 0);
            lp.setComponentVariation(8, 15, 0, 0);
        }, 500);  
    },
    'localCustomizeSetCloth': (type, cloth, toros) => {

        if(parseInt(toros) != -1)
            lp.setComponentVariation(3, parseInt(toros), 0, 0);
    
        lp.setComponentVariation(parseInt(type), parseInt(cloth), 0, 0);
        lp.setComponentVariation(8, 15, 0, 0);
    },

    'localCustomizeEnd': (base, customize, clothes, status) => {
        if(status == false)
        {
            mp.gui.chat.push('[Ошибка]: Введена неверная персональная информация!');
            return;
        }
        mp.events.callRemote('EndPlayerCustomize', base, customize, clothes);
    },

    'localCustomizeChangeSex': (data) => {
        mp.events.callRemote('sCustomizeSetMainData', data);
        setTimeout(() => {
            creatorCamera.pointAtPedBone(mp.players.local.handle, 12844,0,0,0,true);
        }, 500);
        setTimeout(() => {
            creatorCamera.pointAtPedBone(mp.players.local.handle, 12844,0,0,0,true);
        }, 300);
    }
});