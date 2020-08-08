var inventoryBrowser = null;


mp.events.add({
    "InventoryLoad": (name, cash, bank, health, hungry, thirst, items, data) => {
        if(inventoryBrowser != null || inventoryBrowser != undefined) return;

        inventoryBrowser = mp.browsers.new("package://cef/inventory/index.html");

        inventoryBrowser.active = false;

        inventoryBrowser.execute(`initInventory("${name}", ${cash}, ${bank}, ${health}, ${hungry}, ${thirst}, ${items}, ${data})`);
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
        inventoryBrowser.execute(`addItem(${item})`);
    },
    "InventoryRemoveItem": (id) => {
        mp.events.callRemote("sRemoveItem", id);
    },
    "InventoryDropItem": (id) => {

    },
    "InventoryUpdateItem": (id, amount) => {
        mp.gui.chat.push("In InventoryUpdateItem > id: " + id + " amount: " + amount)
        inventoryBrowser.execute(`updateItem(${id}, ${amount})`)
    },
    "InventoryUseItem": (id) => {
        mp.events.callRemote("sUseItem", id);
        mp.events.call("debugLogFromClientSide", `Used item id in InventoryUseItem (CS): ${id}`)
    },
    "InventoryDestroy": () => {
        if(inventoryBrowser == null || inventoryBrowser == undefined) return;

        inventoryBrowser.active = false;
        inventoryBrowser.destroy();

        inventoryBrowser = null;
    },
    "InventoryUpdateBar": (name, cash, money, health, hunger, thirst) => {
        if(inventoryBrowser == null || inventoryBrowser == undefined) return;

        inventoryBrowser.execute(`updatePlayerBar("${name}", ${cash}, ${money}, ${thirst}, ${hunger}, ${health})`)
    },
    "logConsole": (text) => { // dbg
        mp.events.callRemote("debugLogFromClientSide", text)
    }
});