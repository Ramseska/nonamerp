let itemsData = {"ITEM_APPLE":{"ItemImg":"apple.svg","ItemStack":1,"ItemGroup":"Food","ItemName":"Яблоко","ItemDescription":"Спелое зеленое яблоко. Немного утоляет голод.","ItemWeight":170},"ITEM_AID_KIT":{"ItemImg":"first-aid-kit.svg","ItemStack":3,"ItemGroup":"Medical","ItemName":"Аптечка","ItemDescription":"Содержит в себе все необходимые инструменты и препараты для оказания первой помощи.","ItemWeight":2100},"ITEM_WATER_BOTTLE":{"ItemImg":"water-bottle.svg","ItemStack":3,"ItemGroup":"Food","ItemName":"Бутылка воды","ItemDescription":"Литровая бутылка дистиллированной воды. Утоляет жажду.","ItemWeight":1010}};
let itemsList = [], mousePos = { X: 0, Y: 0 }, menu = -1, descWindow = false;

const playerInfo = { name: "No Name", cash: 0, bank: 0, health: 100, hunger: 100, thirst: 100 }



class Item
{
    constructor(id, img, amount, group, name, description, weigth, stack)
    {
        this.id = id
        this.img = img
        this._amount = amount
        this.group = group
        this.name = name
        this.description = description
        this.weigth = weigth
        this.maxstack = stack
        this.element = undefined
    }

    get amount() { return this._amount; }
    set amount(val) 
    { 
        $(this.element).find('.item-amount-value').html(this._amount = val);
        
        updateInventoryInfo();
    }

    create()
    {
        itemsList.push(this)

        this.element = 
        $(`<div class="item-box">
            <img class="item-picture" src="./img/items/${this.img}">
            <div class="item-amount-box">
                <p class="item-amount-value">${this._amount}</p>
            </div>
        </div>`).appendTo($('#items-box')).data('item-id', this.id);

        updateInventoryInfo();
    }

    delete()
    {
        $(this.element).remove();

        itemsList.splice(itemsList.indexOf(this), 1);

        updateInventoryInfo();
    }

    toString() 
    {
        return `\nItemID: ${this.id}\nItemName: ${this.name}\nItemAmount: ${this._amount}\nItemGroup: ${this.group}\nItemWeigth: ${this.weigth}\nItemStack: ${this.maxstack}\nItemImg: ${this.img}\n`;
    }
}

/*
    Inventory
     - Init inventory +
     - Update Inventory Info (weigth / count items) +
     - Update Player Bar +
    Items
     - Add +
     - Delete\drop
     - Update +
     - Use +
     - Merge 
     - Swap
*/

function initInventory(name, cash, bank, health, hungry, thirst, items, data)
{
    itemsData = data;

    updatePlayerBar(name, cash, bank, thirst, hungry, health);
    
    if(items != null && items != undefined) addItem(items);
}

function addItem(items)
{
    if(!Array.isArray(items))
    {
        new Item(items.itemID, itemsData[items.itemType].ItemImg, items.itemAmount, itemsData[items.itemType].ItemGroup, itemsData[items.itemType].ItemName, itemsData[items.itemType].ItemDescription, 
            itemsData[items.itemType].ItemWeight, itemsData[items.itemType].ItemStack).create();
    }
    else 
    {
        items.forEach(items => {
            new Item(items.itemID, itemsData[items.itemType].ItemImg, items.itemAmount, itemsData[items.itemType].ItemGroup, itemsData[items.itemType].ItemName, itemsData[items.itemType].ItemDescription, 
                itemsData[items.itemType].ItemWeight, itemsData[items.itemType].ItemStack).create();
        });
    }
}

function deleteItemById(id)
{
    let item = findItemById(id);

    if(item === undefined) return;

    item.delete();
}

function useItem()
{
    console.log('Use: ' + menu)
    // mp.trigger('InventoryUseItem', menu);
    tempHook(menu)

    hideMenu();
}

function tempHook(id)
{
    let amount = findItemById(id).amount - 1;

    updateItem(id, amount)
}

function updateItem(id, amount)
{
    let item = findItemById(id);

    if((item.amount = amount) <= 0) item.delete();

    showAllItems()
}

function showAllItems()
{
    itemsList.forEach(e => {
        console.log(e.toString());
    });
}

function showMenu(id)
{
    menu = id;

    $("#menu-container").css({left: mousePos.X, top: mousePos.Y}).fadeIn(100);
    $("#inventory-menu").fadeIn(90);
}

function hideMenu()
{
    menu = -1;

    $("#menu-container").fadeOut(100);
    $("#inventory-menu").fadeOut(90);

    hideDescription()
}

function showDescription()
{
    let item = itemsList.find(x => x.id == menu);

    $("#menu-description-content").html(`<h3>${item.name}</h3><br>${item.description}`);

    $("#menu-description").fadeIn(100);

    descWindow = true;
}

function hideDescription()
{
    $("#menu-description").fadeOut(100);

    descWindow = false;
}

function buttonDropItem()
{
    
}

function updatePlayerBar(name, cash, money, thirst, hunger, health)
{
    setHealth(health);
    setHunger(hunger);
    setThirst(thirst);
    setMoney(money);
    setCash(cash);
    setName(name);
}

function updateInventoryInfo()
{
    let accWeigth = 0, accItems = 0;

    if(itemsList.length)
    {
        itemsList.forEach(e => {
            accItems += e.amount
            accWeigth += e.amount * e.weigth
        });
    }

    $("#items-info").html(`Всего предметов: ${accItems} ед.`);
    $("#items-weigth").html(`Вес: ${(accWeigth/1000).toFixed(2)}/100 кг`)
}

$(document).ready(() => 
{
    $(document).on('click', (e) => {

        if(menu == -1 && $(e.target).data('item-id') != undefined)
        {
            showMenu($(e.target).data('item-id'));
        }
        else if(menu != -1 && $(e.target).data('item-id') != undefined)
        {
            if($(e.target).data('item-id') == menu)
            {
                hideDescription();

                setTimeout($("#menu-container").css({left: mousePos.X, top: mousePos.Y}), 100);
            }
            else hideMenu();
        }
        else if(menu != -1 && $(e.target).data('item-id') == undefined && $(e.target).attr('class') != 'inventory-menu-button' && $(e.target).attr('id') != 'inventory-menu')
        {
            hideMenu();
        }

        //console.log(`Mouse Coords:\n\tX: ${mousePos.X}\n\tY: ${mousePos.Y}\n\nElement: ${$(e.target).data('item-id')}`)
    });

    $(this).mousemove(function(e) {
        mousePos.X = e.pageX;
        mousePos.Y = e.pageY;
    });
});

function setHealth(health)
{
    $('#health-line').animate({
        width: calculateLine(playerInfo.health = health) 
    }, 1000);
}

function setHunger(hunger)
{
    $('#hunger-line').animate({
        width: calculateLine(playerInfo.hunger = hunger) 
    }, 1000);
}

function setThirst(thirst)
{
    $('#thirst-line').animate({
        width: calculateLine(playerInfo.thirst = thirst) 
    }, 1000);
}

function setMoney(money)
{
    smoothChangeValue(playerInfo.money, playerInfo.money = money, $('#player-money'));
}

function setCash(cash)
{		
    smoothChangeValue(playerInfo.cash, playerInfo.cash = cash, $('#player-cash'));
}

function setName(name)
{
    $('#player-name').html(playerInfo.name = name)
}

function calculateLine(value)
{
    return (value * 195) / 100;
}

function findItemById(id)
{
    return itemsList.find(x => x.id == id);
}

function smoothChangeValue(old, cur, el)
{
    $({numberValue: old}).animate({numberValue: cur}, {
        duration: 2000,
        easing: "easeOutSine",
        step: function(val) {
            $(el).html(`${Math.ceil(val)}$`);
        }
    });
}