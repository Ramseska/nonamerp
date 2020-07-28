var inventoryBrowser = null;

mp.events.add({
    "InventoryLoad": (name, cash, bank, health, hungry, thirst, items) => {
        if(inventoryBrowser == null) return;

        inventoryBrowser = mp.browsers.new("package://cef/inventory/index.hmtl");

        inventoryBrowser.active = false;
    },
    "InventoryOpen": () => {
        mp.gui.chat.activate(false);
        /* inventoryBrowser.visible =  */ inventoryBrowser.active = mp.gui.cursor.visible = true;
    },
    "InventoryClose": () => {
        mp.gui.chat.activate(true);
        /* inventoryBrowser.visible = */ inventoryBrowser.active = mp.gui.cursor.visible = false;
    },
    "InventoryAddItem": () => {

    },
    "InventoryRemoveItem": () => {

    },
    "InventoryDropItem": () => {

    },
    "InventoryUseItem": () => {

    }
})