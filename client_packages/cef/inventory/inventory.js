var inventoryBrowser = null;


mp.events.add({
    "InventoryLoad": (name, cash, bank, health, hungry, thirst, items, data) => {
        if(inventoryBrowser != null || inventoryBrowser != undefined) return;

        inventoryBrowser = mp.browsers.new("package://cef/inventory/index.html");

        inventoryBrowser.active = false;

        inventoryBrowser.execute(`initInventory("${name}", ${cash}, ${bank}, ${health}, ${hungry}, ${thirst}, ${items}, ${data})`);

        // mp.events.callRemote("debugLogFromClientSide", dbg)
    },
    "ZALIUPA": (text) => {
        mp.events.callRemote("debugLogFromClientSide", text)
    },
    "InventoryOpen": () => {
        mp.gui.chat.activate(false);
        inventoryBrowser.active = mp.gui.cursor.visible = true;
    },
    "InventoryClose": () => {
        mp.gui.chat.activate(true);
        inventoryBrowser.active = mp.gui.cursor.visible = false;
    },
    "InventoryAddItem": (item) => {

    },
    "InventoryRemoveItem": (id) => {
        mp.events.callRemote("sRemoveItem", id);
    },
    "InventoryDropItem": (id) => {

    },
    "InventoryUpdateItem": (item) => {
        inventoryBrowser.execute(`updateItem(${item})`)
    },
    "InventoryUseItem": (id) => {
        mp.events.callRemote("sUseItem", id);
    },
    "InventoryDestroy": () => {
        if(inventoryBrowser == null || inventoryBrowser == undefined) return;

        inventoryBrowser.active = false;
        inventoryBrowser.destroy();

        inventoryBrowser = null;
    },
    "InventoryUpdateBar": (name, cash, money, health, hunger, thirst) => {
        if(inventoryBrowser == null || inventoryBrowser == undefined) return;

        inventoryBrowser.execute(`updatePlayerInfo(${name}, ${cash}, ${money}, ${thirst}, ${hunger}, ${health})`)
    }
});

function InventoryBuildObject(item)
{

}