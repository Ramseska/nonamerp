var creatorCamera;
var CustomizeBrowser = null;

const cust =
{
    sex: true,
    mother: 0,
    father: 0,
    skin_1: 0,
    skin_2: 0,
    parentsMix: 0,
    skinMix: 0,
    eyeColor: 0,
    hair: 0,
    hairColor: 0,
    hilightColor: 0,

    noseWidth: 0.0,
    noseHeight: 0.0,
    noseLength: 0.0,
    noseBridge: 0.0,
    noseTip: 0.0,
    noseBridgeShift: 0.0,
    browHeight: 0.0,
    browWidth: 0.0,
    cheekboneHeight: 0.0,
    cheekboneWidth: 0.0,
    cheeksWidth: 0.0,
    eyesWidth: 0.0,
    lips: 0.0,
    jawWidth: 0.0,
    jawHeight: 0.0,
    chitHeight: 0.0,
    chinPosition: 0.0,
    chinWidth: 0.0,
    chinShape: 0.0,
    neckWidth: 0.0
};

function UpdateRanges()
{
    if($('#range-sex').val() == 1) {
        $('#range-output-sex').html("Мужчина");
        cust.sex = true;
    }
    else if($('#range-sex').val() == 0) {
        $('#range-output-sex').html("Женщина");
        cust.sex = false;
    }

    $('#range-output-mother').html($('#range-mother').val());
    $('#range-output-father').html($('#range-father').val());
    $('#range-output-skin-1').html($('#range-skin-one').val());
    $('#range-output-skin-2').html($('#range-skin-two').val());
    $('#range-output-mix-parents').html($('#range-parents-mix').val());
    $('#range-output-mix-skins').html($('#range-skin-mix').val());
    $('#range-output-eyes').html($('#range-eye').val());
    $('#range-output-hair').html($('#range-hair').val());
    $('#range-output-hair-color').html($('#range-hair-color').val());
    $('#range-output-hair-hueir').html($('#range-hair-hueir').val());

    $('#output-nose-width').html($('#nose-width').val());
    $('#output-nose-height').html($('#nose-height').val());
    $('#output-nose-length').html($('#nose-length').val());
    $('#output-nose-bridge').html($('#nose-bridge').val());
    $('#output-nose-tip').html($('#nose-tip').val());
    $('#output-nose-bridge-shift').html($('#nose-bridge-shift').val());
    $('#output-brow-height').html($('#brow-height').val());
    $('#output-brow-width').html($('#brow-width').val());
    $('#output-cheekbone-height').html($('#cheekbone-height').val());
    $('#output-cheekbone-width').html($('#cheekbone-width').val());
    $('#output-cheeks-width').html($('#cheeks-width').val());
    $('#output-eyes-width').html($('#eyes-width').val());
    $('#output-lips').html($('#lips').val());
    $('#output-jaw-width').html($('#jaw-width').val());
    $('#output-jaw-height').html($('#jaw-height').val());
    $('#output-chin-height').html($('#chin-height').val());
    $('#output-chin-position').html($('#chin-position').val());
    $('#output-chin-width').html($('#chin-width').val());
    $('#output-chin-shape').html($('#chin-shape').val());
    $('#output-neck-width').html($('#neck-width').val());

    cust.mother = $('#range-mother').val();
    cust.father = $('#range-father').val();
    cust.skin_1 = $('#range-skin-one').val();
    cust.skin_2 = $('#range-skin-two').val();
    cust.parentsMix = $('#range-parents-mix').val();
    cust.skinMix = $('#range-skin-mix').val();
    cust.eyeColor = $('#range-eye').val();
    cust.hair = $('#range-hair').val();
    cust.hairColor = $('#range-hair-color').val();
    cust.hilightColor = $('#range-hair-hueir').val();

    cust.noseWidth = $('#nose-width').val();
    cust.noseHeight = $('#nose-height').val();
    cust.noseLength = $('#nose-length').val();
    cust.noseBridge = $('#nose-bridge').val();
    cust.noseTip = $('#nose-tip').val();
    cust.noseBridgeShift = $('#nose-bridge-shift').val();
    cust.browHeight = $('#brow-height').val();
    cust.browWidth = $('#brow-width').val();
    cust.cheekboneHeight = $('#cheekbone-height').val();
    cust.cheekboneWidth = $('#cheekbone-width').val();
    cust.cheeksWidth = $('#cheeks-width').val();
    cust.eyesWidth = $('#eyes-width').val();
    cust.lips = $('#lips').val();
    cust.jawWidth = $('#jaw-width').val();
    cust.jawHeight = $('#jaw-height').val();
    cust.chitHeight = $('#chin-height').val();
    cust.chinPosition = $('#chin-position').val();
    cust.chinWidth = $('#chin-width').val();
    cust.chinShape = $('#chin-shape').val();
    cust.neckWidth = $('#neck-width').val();

    mp.trigger('EventCustomizeRemote', JSON.stringify(cust));
}

mp.events.add('Camera:zoom', (vector) => {
    switch(vector)
    {
        case "forward:l":
        {
            if(creatorCamera.getFov() == 20) return;
            creatorCamera.setFov(creatorCamera.getFov() - 1);
            break;
        }
        case "back:l": 
        {
            if(creatorCamera.getFov() == 50) return;
            creatorCamera.setFov(creatorCamera.getFov() + 1);
            break;
        }
        case "forward:r":
        {
            if(creatorCamera.getFov() == 20) return;
            else if(creatorCamera.getFov() <= 30) creatorCamera.setFov(20);
            else creatorCamera.setFov(creatorCamera.getFov() - 10);
            break;
        }
        case "back:r": 
        {
            if(creatorCamera.getFov() == 50) return;
            else if(creatorCamera.getFov() >= 40) creatorCamera.setFov(50);
            else creatorCamera.setFov(creatorCamera.getFov() + 10);
            break;
        }
    }
});
mp.events.add('Camera:move', (vector) => {
    switch(vector)
    {
        case 0:
        {
            // creatorCamera.setCoord(500.3524,5605.3784,798.6163);
            creatorCamera.setCoord(403.9008, -997.3071, -98.5);
            break;
        }
        case 1: 
        {
            // creatorCamera.setCoord(502.0, 5606.9819, 798.7);
            creatorCamera.setCoord(402.8664, -997.5515, -98.5);
            break;
        }
    }
    creatorCamera.pointAtPedBone(mp.players.local.handle, 12844,0,0,0,true);
});

function ChangeViewAngle(arg)
{
    if(arg == 0)
    {
        $('#view-angle').hide();
        $('#view-front').show();
    }
    else if(arg == 1)
    {
        $('#view-angle').show();
        $('#view-front').hide();
    }
    mp.trigger('Camera:move', arg);
}

mp.events.add('FinishCustomize', () => {
    mp.events.callRemote('EndPlayerCustomize', pukikaki);
    mp.events.call('DestroyCustomizeBrowser');
    mp.game.ui.displayRadar(true);
});

mp.events.add('CreateCustomizeBrowser', () => {
    CustomizeBrowser = mp.browsers.new("package://cef/reg_customizer/index.html");
    mp.gui.cursor.visible = true;
    mp.gui.chat.activate(false);
    mp.game.ui.displayRadar(false);
});
mp.events.add('DestroyCustomizeBrowser', () => {
    CustomizeBrowser.destroy();
    mp.gui.chat.activate(true);
    CustomizeBrowser = null;
    mp.gui.cursor.visible = false;

    creatorCamera.setActive(false);
    mp.game.cam.renderScriptCams(false, false, 0, true, false);
    creatorCamera = undefined;
});

mp.events.add('EventCustomizeRemote', (arg) => {
    pukikaki = arg;
    mp.events.callRemote('SetPlayerCustomize', arg);
});

mp.events.add('StartPlayerCustomize', () => {
    mp.events.call('CreateCustomizeBrowser');

    mp.events.callRemote('tempEvent'); // delete soon 
    // creatorCamera = mp.cameras.new("creatorCamera", new mp.Vector3(502.0, 5606.9819, 798.7), new mp.Vector3(0,0,0), 40);
    creatorCamera = mp.cameras.new("creatorCamera", new mp.Vector3(402.8664, -997.5515, -98.5), new mp.Vector3(0,0,20), 50);
    creatorCamera.setCoord(402.8664, -997.5515, -98.5);
    creatorCamera.pointAtPedBone(mp.players.local.handle, 12844,0,0,0,true);
    setTimeout(() => {
        creatorCamera.pointAtPedBone(mp.players.local.handle, 12844,0,0,0,true);
    }, 2000);

    creatorCamera.setActive(true);
    mp.game.cam.renderScriptCams(true, false, 0, true, false);
});