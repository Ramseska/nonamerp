var HouseInfoBarBrowser = null;
var HouseBuyStatus = -1;

const ClassName = ["Коробка от холодильника","Времянка","Обычный домик","Средний домик","Дорого богато","Для детей депутатов"];

mp.events.add('CreateHouseInfoBar', (id, clas, price, owner) => {
    mp.gui.chat.push(`${id} | ${clas} | ${price} | ${owner}`);    

    HouseInfoBarBrowser = mp.browsers.new("package://cef/other/house/index.html");

    if(owner == "None") {
        HouseInfoBarBrowser.execute('document.getElementById("button-left").value = "Купить"');
        HouseBuyStatus = 0;
    }
    else HouseBuyStatus = 1;

    HouseInfoBarBrowser.execute(`document.getElementById("content-info").innerHTML = 'Владелец: ${owner}<br>Класс: ${ClassName[clas]}<br>Цена: ${price}<br>'`);
    HouseInfoBarBrowser.execute(`document.getElementById("head").innerHTML = 'Номер дома: ${id}'`);
    mp.gui.cursor.visible = true;
    mp.gui.chat.activate(false);
    mp.game.graphics.transitionToBlurred(500);
    mp.players.local.freezePosition(true);
});

mp.events.add('DestroyHouseInfoBar', () => {
    mp.gui.cursor.visible = false;
    mp.gui.chat.activate(true);
    mp.game.graphics.transitionFromBlurred(500);
    mp.players.local.freezePosition(false);

    HouseInfoBarBrowser.destroy();
    HouseInfoBarBrowser = null;
});