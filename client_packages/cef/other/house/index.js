var HouseInfoBarBrowser = null;
var HouseBuyStatus = -1;


mp.events.add('CreateHouseInfoBar', (id, cls, price, owner) => {
    mp.gui.chat.push(`${mp.gui.cursor.visible}`);

    HouseInfoBarBrowser = mp.browsers.new("package://cef/other/house/index.html");

    if(owner == "None") {
        HouseInfoBarBrowser.execute('document.getElementById("button-left").value = "Купить"');
        HouseBuyStatus = 0;
    }
    else HouseBuyStatus = 1;

    HouseInfoBarBrowser.execute(`document.getElementById("text-box").innerHTML = 'Номер дома: ${id}<br><br>Класс: ${cls}<br><br>Цена: ${price}$<br><br>Владелец: ${owner}<br><br>'`);

    mp.gui.cursor.visible = true;
    mp.gui.chat.activate(false);
    mp.game.graphics.transitionToBlurred(500);
    mp.players.local.freezePosition(true);

    mp.gui.chat.push(`${mp.gui.cursor.visible}`);
});

mp.events.add('DestroyHouseInfoBar', () => {
    mp.gui.chat.push(`${mp.gui.cursor.visible}`);

    mp.gui.cursor.visible = false;
    mp.gui.chat.activate(true);
    mp.game.graphics.transitionFromBlurred(500);
    mp.players.local.freezePosition(false);

    HouseInfoBarBrowser.destroy();
    HouseInfoBarBrowser = null;
    
    mp.gui.chat.push(`${mp.gui.cursor.visible}`);
});