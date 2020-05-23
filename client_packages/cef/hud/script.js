const hud = 
{
    satiety: 100,
    thirst: 100,
    money: 0,
    bank: 0
}

function onUpdateHud(satiety = hud.satiety, thirst = hud.thirst, money = hud.money, bank = hud.bank)
{
    hud.satiety = satiety;
    hud.thirst = thirst;
    hud.money = money;
    hud.bank = bank;

    changeSatiety(satiety);
    changeThirst(thirst);
    changeMoney(money);
    changeBank(bank);
}

function changeSatiety(satiety)
{
    $('#satiety-line').animate({
        top: `${(100-satiety)}%`,
        heigth: calculateLine(satiety)
    }, 1000);
}

function changeThirst(thirst)
{
    $('#thirst-line').animate({
        top: `${(100-thirst)}%`,
        heigth: calculateLine(thirst) 
    }, 1000);
}

function changeMoney(money)
{
    $('#cash-text').html(`${money}$`);
}

function changeBank(bank)
{
    $('#bank-text').html(`${bank}$`);
}

function calculateLine(value)
{
    return (value * 60) / 100;
}

function enableMic()
{
    $('#voice-border-circle').animate({
        backgroundColor: '#6bdc81'
    }, 100);
}

function disableMic()
{
    $('#voice-border-circle').animate({
        backgroundColor: '#fd8888'
    }, 100);
}