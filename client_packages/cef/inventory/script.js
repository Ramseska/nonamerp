let itemList = [];

let mousePos = { X: 0, Y: 0 };

let isMenuActive = -1, isInfoActive = false;

let itemsData = null;

const inventoryInfo =
{
    weigth: 0,
    items: 0
}

const playerInfo =
{
    name: "No Name",
    cash: 0,
    bank: 0,
    health: 100,
    hunger: 100,
    thirst: 100
}

class Item 
{
    constructor(id, img, amount, group, name, description, weigth, stack)
    {
        this.id = id
        this.img = img
        this.amount = amount
        this.group = group
        this.name = name
        this.description = description
        this.weigth = weigth
        this.maxstack = stack
    }

    create()
    {
        itemList.push(this)
        this.index = itemList.indexOf(this)
        
        this.element = 
        $(`<div class="item-box">
            <img class="item-picture" src="./img/items/${this.img}">
            <div class="item-amount-box">
                <p class="item-amount-value">${this.amount}</p>
            </div>
        </div>`).appendTo($("#items-box"))
        
        updateIndex()
        updateInventoryInfo()

        return this;
    }
    delete()
    {
        $(this.element).remove()
        itemList.splice(itemList[this.index], 1)

        updateIndex()
        updateInventoryInfo()
    }
    update()
    {
        this.element = 
        $(`<div class="item-box">
            <img class="item-picture" src="/img/items/${this.img}">
            <div class="item-amount-box">
                <p class="item-amount-value">${this.amount}</p>
            </div>
        </div>`)
        
        updateInventoryInfo()
    }
    toString()
    {
        return "Name: " + this.name + "\nDescription: " + this.description + "\nAmount: " + this.amount + "\nGroup: " + this.group + "\nIMG: " + this.img + "\nIndex: " + this.index +"\n\n";
    }
}

function updateIndex()
{
    itemList.forEach((e, i) => $(e.element).data('index', e.index = i));
}

function initInventory(name, cash, bank, health, hungry, thirst, items, data)
{
    itemsData = data;

    if(items.length != 0) addItems(items);
        
    updatePlayerInfo(name, cash, bank, thirst, hungry, health);
}

function addItems(item)
{
    for(let i = 0; i < item.length; i++)
    {
        new Item(item[i].itemID, itemsData[item[i].itemType].ItemImg, item[i].itemAmount, itemsData[item[i].itemType].ItemGroup, itemsData[item[i].itemType].ItemName, itemsData[item[i].itemType].ItemDescription, itemsData[item[i].itemType].ItemWeight, itemsData[item[i].itemType].ItemStack).create();
    }

    updateInventoryInfo()
}

function updateItem(item)
{
    itemList.find(x => x.id == item.itemID).amount = item.itemAmount;
}

function deleteItem(index)
{
    itemList[index].delete()
}

$(document).ready(() => {
    $(document).on('click', function(e){
        if(!$(e.target).hasClass("item-box") && !$(e.target).parent().is("#inventory-menu") && isMenuActive != -1)
        {
            onClickButtonClose()
            return;
        }
        else if($(e.target).hasClass("item-box"))
        {
            if(isMenuActive == -1)
            {
                $("#menu-container").css({left: mousePos.X, top: mousePos.Y}).fadeIn(100);
                $("#inventory-menu").fadeIn(90);
                isMenuActive = $(e.target).data('index');
            }
            else if(isMenuActive != -1)
            {
                $("#menu-container").css({left: mousePos.X, top: mousePos.Y});
                if(isInfoActive) 
                {
                    $("#menu-description").fadeOut(0)
                    isInfoActive = false
                }
                
                isMenuActive = $(e.target).data('index');
            }
        }
    });

    $(this).mousemove(function(e)   {
        mousePos.X = e.pageX;
        mousePos.Y = e.pageY;
    });
});

function onClickButtonInfo()
{
    if(!isInfoActive) 
    {
        $("#menu-description-content").html(`<h3>${itemList[isMenuActive].name}</h3><br>${itemList[isMenuActive].description}`);

        $("#menu-description").fadeIn(90)
    }
    else $("#menu-description").fadeOut(90)

    isInfoActive = !isInfoActive
}

function onClickButtonUse()
{
    mp.trigger("InventoryUseItem", itemList[isMenuActive].id)
}

function onClickButtonDrop() 
{
    throw new Error("Еще нет реализации")
}

function onClickButtonClose()
{
    $("#inventory-menu").fadeOut(90);
    $("#menu-container").fadeOut(100);
    if(isInfoActive) 
    {
        $("#menu-description").fadeOut(0)
        isInfoActive = false
    }
    isMenuActive = -1;
}

function updatePlayerInfo(name, cash, money, thirst, hunger, health)
{
    setHealth(playerInfo.health = health)
    setHunger(playerInfo.hunger = hunger)
    setThirst(playerInfo.thirst = thirst)
    setMoney(playerInfo.money = money)
    setCash(playerInfo.cash = cash)
    setName(playerInfo.name = name)
}

function setHealth(health)
{
    $('#health-line').animate({
        width: calculateLine(health) 
    }, 1000);
}

function setHunger(hunger)
{
    $('#hunger-line').animate({
        width: calculateLine(hunger) 
    }, 1000);
}

function setThirst(thirst)
{
    $('#thirst-line').animate({
        width: calculateLine(thirst) 
    }, 1000);
}

function setMoney(money)
{
    $('#player-money').html(money + "$")
}

function setCash(cash)
{
    $('#player-cash').html(cash + "$")
}

function setName(name)
{
    $('#player-name').html(name)
}

function updateInventoryInfo()
{
    let accWeigth = 0, accItems = 0;

    if(itemList.length)
    {
        itemList.forEach(e => {
            accItems += e.amount
            accWeigth += e.amount * e.weigth
        });
    }

    $("#items-info").html(`Всего предметов: ${accItems} ед.`);
    $("#items-weigth").html(`Вес: ${(accWeigth/1000).toFixed(2)}/100 кг`)
}

function calculateLine(value)
{
    return (value * 195) / 100;
}