//import $ from 'jquery';

let parentSwitcher = 1;
let hairActived = false;
let isInputValid = [false, false, false];

const parentsList =
[
    ["Adrian", 1, 13],["Alex", 1, 6],["Andrew", 1, 4],["Angel", 1, 11],["Anthony", 1, 20],["Benjamin", 1, 0],
    ["Claude", 1, 44],["Daniel", 1, 1],["Diego", 1, 12],["Ethan", 1, 9],["Evan", 1, 8],["Gabriel", 1, 14],["Isaac", 1, 7],["John", 1, 42],
    ["Joshua", 1, 2],["Juan", 1, 5],["Kevin", 1, 17],["Louis", 1, 18],["Michael", 1, 15],["Niko", 1, 43],["Noah", 1, 3],["Samuel", 1, 19],
    ["Santiago", 1, 16],["Vincent", 1, 10],["Amelia", 0, 25],["Ashley", 0, 34],["Audrey", 0, 22],["Ava", 0, 28],["Briana", 0, 36],["Camila", 0, 29],
    ["Charlotte", 0, 40],["Elizabeth", 0, 39],["Emma", 0, 41],["Evelyn", 0, 32],["Giselle", 0, 24],["Grace", 0, 35],["Hannah", 0, 21],["Isabella", 0, 26],
    ["Jasmine", 0, 23],["Misty", 0, 45],["Natalie", 0, 37],["Nicole", 0, 33],["Olivia", 0, 38],["Sophia", 0, 31],["Violet", 0, 30],["Zoe", 0, 27]
]
const hairList = 
[
    [0, 0], [0, 2], [0, 3], [0, 4], [0, 8], [0, 10], [0, 12], [0, 15],
    [1, 0], [1, 1], [1, 4], [1, 5], [1, 12], [1, 17], [1, 35], [1, 37]
]

const eyeColors = ["Зеленый", "Изумрудный", "Ярко-синий", "Голубой", "Карий", "Темно-карий", "Ореховый", "Темно-серый", "Светло-серый"];

const clothList = 
[
    // fem
    [
        // top
        [
            [11, 3], [11, 14], [11, 27]
        ],
        // legs
        [
            [4, 4], [4, 8], [4, 16]
        ],
        // shoes
        [
            [6, 3], [6, 13], [6, 15]
        ]
    ],
    // male
    [
        // top
        [
            [11, 9], [11, 16], [11, 38]
        ],
        // legs 
        [
            [4, 3], [4, 8], [4, 17]
        ],
        // shoes    
        [
            [6, 1], [6, 5], [6, 10]
        ]
    ]
]

var localData = 
{
    base: 
    {
        name: undefined,
        subname: undefined,
        old: undefined,
        sex: true
    },
    parents:
    {
        mother: 25,
        father: 13,
        motherSkin: 0,
        fatherSkin: 0,
        parentsMix: 0.0,
        skinMix: 0.0
    },
    features:
    {
        noseWidth: 0.0,
        noseHeigth: 0.0,
        noseLength: 0.0,
        noseBridge: 0.0,
        noseTip: 0.0,
        noseBridgeShift: 0.0,
        browHeigth: 0.0,
        browWidth: 0.0,
        cheekboneHeigth: 0.0,
        cheekboneWidth: 0.0,
        cheekWidth: 0.0,
        eyes: 0.0,
        lips: 0.0,
        jawWidth: 0.0,
        jawHeigth: 0.0,
        chinLength: 0.0,
        chinPosition: 0.0,
        chinWidth: 0.0,
        chinShape: 0.0,
        neckWidth: 0.0
    },
    hair: [0, 0, 0],
    headOverlay:
    {
        blemishes: [0, 255, 1.0],
        facialHair: [1, 255, 1.0],
        eyebrows: [2, 255, 1.0],
        ageing: [3, 255, 1.0],
        makeup: [4, 255, 1.0],
        blush: [5, 255, 1.0],
        complexion: [6, 255, 1.0],
        sunDamage: [7, 255, 1.0],
        lipStick: [8, 255, 1.0],
        moles: [9, 255, 1.0],
        chestHair: [10, 255, 1.0],
        bodyBlemishes: [11, 255, 1.0],
        addBodyBlemishes: 1
    }
};

const hairColors =
[
    "#191815", "#312a24", "#453228", "#512917", "#783719", "#893b19", "#a45830", "#a2653f", "#ae744d", "#a87650", "#b6885a", 
    "#d0ab74", "#cda974", "#dcb369", "#dfb671", "#e6c184", "#bf8d5c", "#a85d3b", "#9f3f28", "#820b06", "#910f09", "#ae160e", 
    "#c83216", "#e75320", "#c8582e", "#da5320", "#917866", "#b39984", "#d4bea9", "#f0ddcc", "#714c5e", "#8e5772", "#ac5067", 
    "#fd4fd9", "#fc4694", "#f7a0af", "#029d8f", "#017d88", "#025082", "#66ab60", "#319563", "#217660", "#bfc52c", "#a6c013", 
    "#5ba41d", "#f1cb5d", "#f9ce06", "#f6a206", "#fa880b", "#f67c20", "#fe7c13", "#f75a1f", "#f5350a", "#d10406", "#9b0306", 
    "#25140e", "#3b1e16", "#582e1b", "#603727", "#492416", "#301c12", "#020205", "#a9865c", "#c2935d"
];

$(document).ready(() => {
    for(let q = 0; q < 64; q++)
    {
        $('.colors_line_box').append($('<div class="colors_element"></div>').css('background-color', hairColors[q]));
    }

    $("#circle_container").click(function(event) {
        if(event.target.parentElement.id === "circle_container")
        {
            $("#left_inside_container").children(".left_box_content").hide();
            $("#circle_container").children(".circle_actived").removeClass("circle_actived").addClass("circle_hover");
            $(event.target).addClass("circle_actived").removeClass("circle_hover");
            // $(event.target).index()
            $("#left_inside_container").children(".left_box_content").eq($(event.target).index()).show();

            $("#right_inside_container").children().fadeOut(100);
            $("#right_container").fadeOut(100);
            
            // right container 
            switch($(event.target).index())
            {
                default: 
                {
                    if(hairActived) unloadHairList()
                    break;
                }
                case 1: 
                {
                    unloadParentsList()
                    $("#right_container").fadeIn(100);
                    $("#parents_container").fadeIn(100);
                    loadParentsList(parentSwitcher);
                    break;
                }
            }
        }
    });
    $(".form-field").focusin((el) => {
        $(el.target).css('border', '1px solid #bdf5bc').css('border-left', '0').css('border-right', '0');
        $(el.target.parentElement.firstElementChild).css('background-color', "#bdf5bc");
        $(el.target.parentElement.lastElementChild).css('border', '1px solid #bdf5bc').css('border-left', '0');
    }).focusout((el) => {
        $(el.target).css('border', '1px solid #b0afb7').css('border-left', '0').css('border-right', '0');
        $(el.target.parentElement.firstElementChild).css('background-color', "#b0afb7");
        $(el.target.parentElement.lastElementChild).css('border', '1px solid #b0afb7').css('border-left', '0');
    });
    $("#input_sex").change(() => {
        localData.base.sex = !$("#input_sex").prop('checked');

        mp.trigger('customizeChangeSex', JSON.stringify(localData));
        mp.trigger('customizeSetDefaultClothes', JSON.stringify(localData.base.sex));
    });

    $('#end_button').click(() => { mp.trigger('customizeEnd', JSON.stringify(localData), isInputValid); });
    
    $(document).on('input change', '#parent_mix', function()  {
        $('#output_parent_mix').html(localData.parents.parentsMix = $(this).val());
    });
    $(document).on('input change', '#parent_skin', function()  {
        $('#output_parent_skin').html(localData.parents.skinMix = $(this).val());
    });

    $(document).on('input change', '#features_nose_width', function()  {
        $('#output_features_nose_width').html(localData.features.noseWidth = $(this).val());
    });
    $(document).on('input change', '#features_nose_heigth', function()  {
        $('#output_features_nose_heigth').html(localData.features.noseHeigth = $(this).val());
    });
    $(document).on('input change', '#features_nose_length', function()  {
        $('#output_features_nose_length').html(localData.features.noseLength = $(this).val());
    });
    $(document).on('input change', '#features_nose_bridge', function()  {
        $('#output_features_nose_bridge').html(localData.features.noseBridge = $(this).val());
    });
    $(document).on('input change', '#features_nose_tip', function()  {
        $('#output_features_nose_tip').html(localData.features.noseTip = $(this).val());
    });
    $(document).on('input change', '#features_nose_bridgeshift', function()  {
        $('#output_features_nose_bridgeshift').html(localData.features.noseBridgeShift = $(this).val());
    });
    $(document).on('input change', '#features_brow_heigth', function()  {
        $('#output_features_brow_heigth').html(localData.features.browHeigth = $(this).val());
    });
    $(document).on('input change', '#features_brow_width', function()  {
        $('#output_features_brow_width').html(localData.features.browWidth = $(this).val());
    });
    $(document).on('input change', '#features_cheekbone_heigth', function()  {
        $('#output_features_cheekbone_heigth').html(localData.features.cheekboneHeigth = $(this).val());
    });
    $(document).on('input change', '#features_cheekbone_width', function()  {
        $('#output_features_cheekbone_width').html(localData.features.cheekboneWidth = $(this).val());
    });
    $(document).on('input change', '#features_cheek_width', function()  {
        $('#output_features_cheek_width').html(localData.features.cheekWidth = $(this).val());
    });
    $(document).on('input change', '#features_eyes', function()  {
        $('#output_features_eyes').html(localData.features.eyes = $(this).val());
    });
    $(document).on('input change', '#features_lips', function()  {
        $('#output_features_lips').html(localData.features.lips = $(this).val());
    });
    $(document).on('input change', '#features_jaw_width', function()  {
        $('#output_features_jaw_width').html(localData.features.jawWidth = $(this).val());
    });
    $(document).on('input change', '#features_jaw_heigth', function()  {
        $('#output_features_jaw_heigth').html(localData.features.jawHeigth = $(this).val());
    });
    $(document).on('input change', '#features_chin_length', function()  {
        $('#output_features_chin_length').html(localData.features.chinLength = $(this).val());
    });
    $(document).on('input change', '#features_chin_position', function()  {
        $('#output_features_chin_position').html(localData.features.chinPosition = $(this).val());
    });
    $(document).on('input change', '#features_chin_width', function()  {
        $('#output_features_chin_width').html(localData.features.chinWidth = $(this).val());
    });
    $(document).on('input change', '#features_chin_shape', function()  {
        $('#output_features_chin_shape').html(localData.features.chinShape = $(this).val());
    });
    $(document).on('input change', '#features_neck_width', function()  {
        $('#output_features_neck_width').html(localData.features.neckWidth = $(this).val());
    });

    $(document).on('input change', '#headoverlay_blemishes', function()  {
        $('#output_headoverlay_blemishes').html((localData.headOverlay.blemishes[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.blemishes[1])
    });
    $(document).on('input change', '#headoverlay_blemishes_opacity', function()  {
        $('#output_headoverlay_blemishes_opacity').html(localData.headOverlay.blemishes[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_facialhair', function()  {
        $('#output_headoverlay_facialhair').html((localData.headOverlay.facialHair[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.facialHair[1])
    });
    $(document).on('input change', '#headoverlay_facialhair_opacity', function()  {
        $('#output_headoverlay_facialhair_opacity').html(localData.headOverlay.facialHair[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_eyebrows', function()  {
        $('#output_headoverlay_eyebrows').html((localData.headOverlay.eyebrows[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.eyebrows[1])
    });
    $(document).on('input change', '#headoverlay_eyebrows_opacity', function()  {
        $('#output_headoverlay_eyebrows_opacity').html(localData.headOverlay.eyebrows[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_ageing', function()  {
        $('#output_headoverlay_ageing').html((localData.headOverlay.ageing[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.ageing[1])
    });
    $(document).on('input change', '#headoverlay_ageing_opacity', function()  {
        $('#output_headoverlay_ageing_opacity').html(localData.headOverlay.ageing[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_complexion', function()  {
        $('#output_headoverlay_complexion').html((localData.headOverlay.complexion[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.complexion[1])
    });
    $(document).on('input change', '#headoverlay_complexion_opacity', function()  {
        $('#output_headoverlay_complexion_opacity').html(localData.headOverlay.complexion[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_sundamage', function()  {
        $('#output_headoverlay_sundamage').html((localData.headOverlay.sunDamage[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.sunDamage[1])
    });
    $(document).on('input change', '#headoverlay_sundamage_opacity', function()  {
        $('#output_headoverlay_sundamage_opacity').html(localData.headOverlay.sunDamage[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_moles', function()  {
        $('#output_headoverlay_moles').html((localData.headOverlay.moles[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.moles[1])
    });
    $(document).on('input change', '#headoverlay_moles_opacity', function()  {
        $('#output_headoverlay_moles_opacity').html(localData.headOverlay.moles[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_chesthair', function()  {
        $('#output_headoverlay_chesthair').html((localData.headOverlay.chestHair[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.chestHair[1])
    });
    $(document).on('input change', '#headoverlay_chesthair_opacity', function()  {
        $('#output_headoverlay_chesthair_opacity').html(localData.headOverlay.chestHair[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_bodybelmishes', function()  {
        $('#output_headoverlay_bodybelmishes').html((localData.headOverlay.bodyBlemishes[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.bodyBlemishes[1])
    });
    $(document).on('input change', '#headoverlay_bodybelmishes_opacity', function()  {
        $('#output_headoverlay_bodybelmishes_opacity').html(localData.headOverlay.bodyBlemishes[2] = $(this).val());
    });


    $(document).on('input change', '#headoverlay_blush', function()  {
        $('#output_headoverlay_blush').html((localData.headOverlay.blush[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.blush[1])
    });
    $(document).on('input change', '#headoverlay_blush_opacity', function()  {
        $('#output_headoverlay_blush_opacity').html(localData.headOverlay.blush[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_lipstick', function()  {
        $('#output_headoverlay_lipstick').html((localData.headOverlay.lipStick[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.lipStick[1])
    });
    $(document).on('input change', '#headoverlay_lipstick_opacity', function()  {
        $('#output_headoverlay_lipstick_opacity').html(localData.headOverlay.lipStick[2] = $(this).val());
    });

    $(document).on('input change', '#headoverlay_makeup', function()  {
        $('#output_headoverlay_makeup').html((localData.headOverlay.makeup[1] = $(this).val() == -1 ? 255 : $(this).val()) == 255 ? "No" : localData.headOverlay.makeup[1])
    });
    $(document).on('input change', '#headoverlay_makeup_opacity', function()  {
        $('#output_headoverlay_makeup_opacity').html(localData.headOverlay.makeup[2] = $(this).val());
    });
    
    $(document).on('input change', '#input_name', function()  {
        if(checkNameOnValid($(this).val()) != null)
        {
            $(this.parentElement).children('.input-error').children('.span-lable-error').fadeIn(100).attr('title', 'Ошибка!\nИмя может содержать только латинские символы!');
            localData.base.name = "";
            isInputValid[0] = false;
        }
        else if(checkNameOnValid($(this).val()) == null)
        {
            if($(this).val().length < 2)
            {
                $(this.parentElement).children('.input-error').children('.span-lable-error').fadeIn(100).attr('title', 'Имя не может содержать менее 2-х символов!');
                localData.base.name = "";
                isInputValid[0] = false;
            }
            else {
                $(this.parentElement).children('.input-error').children('.span-lable-error').fadeOut(100).attr('title', '');
                localData.base.name = $(this).val();
                isInputValid[0] = true;
            }   
        }
    });
    $(document).on('input change', '#input_subname', function() {
        if(checkNameOnValid($(this).val()) != null)
        {
            $(this.parentElement).children('.input-error').children('.span-lable-error').fadeIn(100).attr('title', 'Ошибка!\nИмя может содержать только латинские символы!');
            localData.base.subname = "";
            isInputValid[1] = false;
        }
        else if(checkNameOnValid($(this).val()) == null)
        {
            if($(this).val().length < 2)
            {
                $(this.parentElement).children('.input-error').children('.span-lable-error').fadeIn(100).attr('title', 'Фамилия не может содержать менее 2-х символов!');
                localData.base.subname = "";
                isInputValid[1] = false;
            }
            else {
                $(this.parentElement).children('.input-error').children('.span-lable-error').fadeOut(100).attr('title', '');
                localData.base.subname = $(this).val();
                isInputValid[1] = true;
            }   
        }
    })
    $(document).on('input change', '#input_old', function() {
        if(checkOldOnValid($(this).val()) != null)
        {
            $(this.parentElement).children('.input-error').children('.span-lable-error').fadeIn(100).attr('title', 'Ошибка!\nВозраст может содержать только цифры!');
            localData.base.old = 0;
            isInputValid[2] = false;
        }
        else if(checkOldOnValid($(this).val()) == null)
        {
            if($(this).val().length == 0)
            {
                $(this.parentElement).children('.input-error').children('.span-lable-error').fadeIn(100).attr('title', 'Ошибка!\nПоле не может быть пустым!');
                localData.base.old = 0;
                isInputValid[2] = false;
            }
            else if($(this).val() < 16 || $(this).val() > 80)
            {
                $(this.parentElement).children('.input-error').children('.span-lable-error').fadeIn(100).attr('title', 'Ошибка!\nВозраст персонажа не должен быть меньше 16 или более 80 лет!');
                localData.base.old = 0;
                isInputValid[2] = false;
            }
            else 
            {
                $(this.parentElement).children('.input-error').children('.span-lable-error').fadeOut(100).attr('title', '');
                localData.base.old = $(this).val();
                isInputValid[2] = true;
            }
        }
    })

    $('.colors_element').click(function(e) {

        $(e.target.parentElement).children(this).css('border', 'solid 1px #242933');
        
        $(e.target).css('border', 'solid 1px #00ff00');

        switch(e.target.parentElement.id)
        {
            case "color_hair_1":
            {
                localData.hair[1]= $(e.target).index();
                break;
            }
            case "color_hair_2":
            {
                localData.hair[2] = $(e.target).index();
                break;
            }
        }

        mp.trigger('customizeChangeHairColor', localData.hair[1], localData.hair[2]);
    });

    $(".parents_switch_button").click(() => switchParentsList())

    $("#hair_button").click(function () {
        hairActived === false ? loadHairList() : unloadHairList()
    });
    $('.colors_buttons').on('click', function (e) {
        if(e.target.id == 'right_color_button')
            $(e.target.parentElement).children('.colors_line_box').animate({ scrollLeft: '+=100' }, 200);
        else $(e.target.parentElement).children('.colors_line_box').animate({ scrollLeft: '-=100' }, 200);
    });

    $('.cloth_buttons').on('click', function(e) {
        var i = $(e.target.parentElement).children('.cloth_output').data('sld');

        if(e.target.id == 'right_cloth_button')
        {
            if(i >= 2) return;
            $(e.target.parentElement).children('.cloth_output').data('sld', i + 1);
        }
        else
        {
            if(i <= 0) return;
            $(e.target.parentElement).children('.cloth_output').data('sld', i - 1);
        }
        $(e.target.parentElement).children('.cloth_output').html($(e.target.parentElement).children('.cloth_output').data('sld')+1)

        setCloth(
            clothList[Number(localData.base.sex)][$(e.target.parentElement).children('.cloth_output').data('kid')][$(e.target.parentElement).children('.cloth_output').data('sld')][0], 
            clothList[Number(localData.base.sex)][$(e.target.parentElement).children('.cloth_output').data('kid')][$(e.target.parentElement).children('.cloth_output').data('sld')][1]
        );
    });

    $(document).on('input change', function() {
        mp.trigger('customizeChangeBlendData', localData.parents.mother, localData.parents.father, 0, localData.parents.mother, localData.parents.father, 0, localData.parents.parentsMix, localData.parents.skinMix, 0)
        mp.trigger('customizeSetFeatures', JSON.stringify(localData.features));
        mp.trigger('customizeSetHeadOverlay', JSON.stringify(localData.headOverlay));
    });

    mp.trigger('customizeSetDefaultClothes', JSON.stringify(localData.base.sex));
});

function setCloth(type, cloth)
{
    let toros = -1;
    type == 11 ? cloth == 9 ? toros = 0 : cloth == 16 ? toros = 0 : cloth == 38 ? toros = 8 : cloth == 3 ? toros = 3 : cloth == 14 ? toros = 14 : cloth == 27 ? toros = 0 : toros = 0 : toros = -1;

    mp.trigger('customizeSetCloth', type, cloth, toros);
}

function loadHairList()
{
    $("#right_container").fadeIn(100);
    $("#hair_container").fadeIn(100);
    $("#hair_button").val("<");

    hairActived = true;

    for(let i = 0; i < hairList.length; i++)
    {
        if(hairList[i][0] === +localData.base.sex)
        {
            let img = `url(img/hairs/hair_`+ +localData.base.sex + `_${hairList[i][1]}.png)`;
            
            $("#hair_list").append($('<div class="hair_item_container"></div>').data("hairID", hairList[i][1]).css('background-color', localData.hair[0] === hairList[i][1] ? `#4c3232` : '#393f4a').click(function() {
                $("#hair_list").children(this).css('background-color', '#393f4a');
                $(this).css('background-color', '#4c3232');

                mp.trigger('customizeSetHair', $(this).data("hairID"));
            }).append('<div class="hair_item"></div>').css('background-image', img));
        }
    }
}

function unloadHairList()
{
    $("#hair_container").fadeOut(100);
    $("#right_container").fadeOut(100);
    $("#hair_button").val(">");

    hairActived = false;

    setTimeout(() => { $("#hair_list").empty() }, 110);
}

function loadParentsList(parentStatus)
{
    for(let i = 0; i < parentsList.length; i++)
    {
        if(parentsList[i][1]===parentStatus)
        {
            let image = "url(img/parents/p_icon_"+parentsList[i][0].toLowerCase()+".png)";

            $("#parents_list").append($('<div class="parent_item_container"></div>').data("itemID", parentsList[i][2]).data("sex", parentsList[i][1]).css('background-color', parentStatus === 0 ? (localData.parents.mother === parentsList[i][2] ? `#4c3232` : '#393f4a') : (localData.parents.father === parentsList[i][2] ? `#4c3232` : '#393f4a')).click(function () {
                $(this).data("sex") === 0 ? localData.parents.mother = localData.parents.motherSkin = $(this).data("itemID") : localData.parents.father = localData.parents.fatherSkin = $(this).data("itemID");
                $("#parents_list").children(this).css('background-color', '#393f4a');
                $(this).css('background-color', '#4c3232');

                mp.trigger('customizeChangeBlendData', localData.parents.mother, localData.parents.father, 0, localData.parents.mother, localData.parents.father, 0, localData.parents.parentsMix, localData.parents.skinMix, 0)
                    
            }).append($('<div class="parent_item_img"></div>').css('background-image', image)).append($(`<div class='parent_item_name'>${parentsList[i][0]}</div>`)))
         }
    }
}
function unloadParentsList()
{
    $("#parents_list").empty()
}
function switchParentsList()
{
    unloadParentsList()
    parentSwitcher = parentSwitcher === 0 ? 1 : 0;
    if(parentSwitcher === 1) $("#parents_switcher").html('Отец')
    else $("#parents_switcher").html('Мать')
    loadParentsList(parentSwitcher)
}

function checkNameOnValid(value)
{
    return value.match(/[^a-zA-Z]/);
}

function checkOldOnValid(value)
{
    return value.match(/[^0-9]/)
}