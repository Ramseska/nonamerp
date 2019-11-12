var appStatus = null;

const wallpaperList = 
[
    "bg_img_1", "bg_img_2", "bg_img_3", "bg_img_4", "bg_img_5", "bg_img_6", "bg_img_7", "bg_img_8", "bg_img_9", "bg_img_10", "bg_img_11", "bg_img_12"
];

const settingsList = 
[
    { // wallpaper settings
        picture: {src: "./source/setting_icons/setting_icon_wallpaper.png",class: "settings_item_picture"},
        text: {value: "Wallpapers and themes",class: "settings_item_text"},
        subtext: {value: "Wallpapers, themes, icons",class: "settings_item_text_2"}
    },
    { // sound settings 
        picture: {src: "./source/setting_icons/setting_icon_sound.png",class: "settings_item_picture"},
        text: {value: "Sounds",class: "settings_item_text"},
        subtext: {value: "Sounds, Do not disturb",class: "settings_item_text_2"}
    }
];

setInterval(() => timeUpdate(), 1000);

function initPhone()
{
    $('#main_color_box').hide();

    timeUpdate();
}

function timeUpdate()
{
    let date = new Date();
    let m = date.getMinutes();
    if(m < 10)  { m = "0" + date.getMinutes(); }
    document.getElementById("phone_header_time").innerText = `${date.getHours()}:${m}`;
}

function returnFromWPSelector()
{
    let main_box = document.getElementById('main_box');
    main_box.style.borderTop = "";
    while(main_box.firstChild) main_box.removeChild(main_box.firstChild);
    main_box.style.height = 363;
    main_box.style.top = 38;

    let box = document.getElementById('back_button');
    box.style.borderTop = "";
    while(box.firstChild) box.removeChild(box.firstChild);
    box.remove();

    callApp("aps_settings");
}

function selectSettingItem(arg)
{
    switch(arg)
    {
        case 0: // wallpapers
        {
            let box = document.getElementById('main_box');
            box.style.borderTop = "";
            while(box.firstChild) box.removeChild(box.firstChild);

            box.setAttribute('class', 'flex-mb-wplist');

            let backBox = document.createElement('div');
            backBox.id = "back_button";
            backBox.onclick = function() { returnFromWPSelector(); };

            let backBoxArrowPict = document.createElement('img');
            backBoxArrowPict.src = "./source/back_arrow.png";
            backBoxArrowPict.style.width = 10;
            backBoxArrowPict.style.height = 14;
            backBoxArrowPict.style.marginLeft = 15;
            backBoxArrowPict.style.marginTop = 7;
            backBox.appendChild(backBoxArrowPict);

            let backBoxButtonText = document.createElement('div');
            backBoxButtonText.innerHTML = `MY WALLPAPERS`;
            backBoxButtonText.style.float = "right";
            backBoxButtonText.style.fontSize = 13;
            backBoxButtonText.style.fontWeight = "bold";
            backBoxButtonText.style.marginRight = 60;
            backBoxButtonText.style.marginTop = 6;
            backBox.appendChild(backBoxButtonText);

            let backBoxLine = document.createElement('div');
            backBoxLine.id = "back_button_line";
            backBoxLine.style.position = "absolute";
            backBoxLine.style.width = 207;
            backBoxLine.style.height = 1;
            backBoxLine.style.top = 29;
            backBoxLine.style.backgroundColor = "rgb(234,234,234)";
            backBox.appendChild(backBoxLine);

            document.getElementById('container').insertBefore(backBox, document.getElementById('middle_box'));

            document.getElementById('main_box').style.height = 330;
            document.getElementById('main_box').style.top = 70;

            for(let c = 0; c < wallpaperList.length; c++)
            {
                let newItem = document.createElement('img');
                newItem.className = "wallpaper_item";
                newItem.src = "./source/bg/" + wallpaperList[c] + ".jpg";
                newItem.onclick = function() 
                {
                    document.getElementById('phone_bg').src = "./source/bg/" + wallpaperList[c] + ".jpg";
                    
                };
                box.appendChild(newItem);
            }
            
            appStatus = "aps_settings_wallpapers";
            break;
        }
    }
}

function callApp(id)
{
    switch(id)
    {
        case "aps_settings": 
        {
            let box = document.getElementById('main_box');
            $('#footer_box').hide();
            $('#middle_box').hide();

            document.getElementById('header_color_box').style.backgroundColor = 
            document.getElementById('middle_color_box').style.backgroundColor = "#fafafa";
            document.getElementById('bottom_color_box').style.backgroundColor = "#eaeaea";
            box.style.borderTop = "1px solid rgb(234,234,234)";

            changeHeadButtonColors(1);

            box.setAttribute('class', 'flex-mb-stlist');

            $('#main_color_box').show();

            for(let i = 0; i < settingsList.length; i++)
            {
                let newItem = document.createElement('div');
                newItem.className = "settings_item";
                newItem.onclick = function() { selectSettingItem(i); };

                let pict = document.createElement('img');
                pict.src = settingsList[i].picture.src;
                pict.className = settingsList[i].picture.class;
                newItem.appendChild(pict);

                let text = document.createElement('div');
                text.innerHTML = settingsList[i].text.value;
                text.className = settingsList[i].text.class;
                newItem.appendChild(text);

                let _text = document.createElement('div');
                _text.innerHTML = settingsList[i].subtext.value;
                _text.className = settingsList[i].subtext.class;
                newItem.appendChild(_text);

                let line = document.createElement('div');
                line.className = "settings_item_bottomline";
                newItem.appendChild(line);

                box.appendChild(newItem);
            }

            appStatus = id;
            break;
        }
    }
}

function changeHeadButtonColors(arg)
{
    if(arg == 1)
    {
        document.getElementById('buttons_cascade').setAttribute('class', 'filtered-bottom');
        document.getElementById('phone_header').setAttribute('class', 'filtered-header');
    }
    else
    {
        document.getElementById('buttons_cascade').removeAttribute('class');
        document.getElementById('phone_header').removeAttribute('class');
    }
    
}

function buttonBack()
{
    switch(appStatus)
    {
        case null: break;
        case "aps_settings": 
        {
            changeHeadButtonColors(0);
            let box = document.getElementById('main_box');
            box.style.borderTop = "";
            while(box.firstChild) box.removeChild(box.firstChild);

            $('#footer_box').show();
            $('#middle_box').show();
            $('#main_color_box').hide();

            appStatus = null;
            break;
        }
        case "aps_settings_wallpapers":
        {
            returnFromWPSelector();
            break;
        }
        default: break;
    }
}

function buttonHome()
{
    switch(appStatus)
    {
        case null: break;
        case "aps_settings":
        {
            let box = document.getElementById('main_box');
            box.style.borderTop = "";
            while(box.firstChild) box.removeChild(box.firstChild);
            break;
        }
        case "aps_settings_wallpapers":
        {
            let box = document.getElementById('back_button');
            box.style.borderTop = "";
            while(box.firstChild) box.removeChild(box.firstChild);
            let main_box = document.getElementById('main_box');
            main_box.style.borderTop = "";
            while(main_box.firstChild) main_box.removeChild(main_box.firstChild);
            main_box.style.height = 363;
            main_box.style.top = 38;
            box.remove();
            break;
        }
        default: break;
    }

    changeHeadButtonColors(0);

    $('#footer_box').show();
    $('#middle_box').show();
    $('#main_color_box').hide();

    appStatus = null;
}