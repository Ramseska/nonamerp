const applePosOffSet =
[
    [63, 98],   [87, 131],  [107, 85],  [146, 132], [152, 77], [188, 89], [58, 284], [194, 250], [230, 112], [243, 161],
    [263, 212], [200, 200], [143, 212], [118, 181], [78, 207], [22, 190], [235, 60], [108, 274], [163, 316], [215, 297]
];

let countApples = 0;
let idk = false;

$(document).ready(() => {
    loadApp();
});

function loadApp()
{
    if(idk == true) return;
    idk = true;

    let positions = getRandom(20, 5);
    console.log(positions)

    for(let i = 0; i < 5; i++)
    {
        createApple(applePosOffSet[positions[i]][0], applePosOffSet[positions[i]][1]);
    }

    $('.circle').droppable({
        drop: function(event, ui) 
        {
            $(event.toElement).removeClass('apple-item').addClass('apple-img').draggable("disable").appendTo($(this)).css({left: 0, top: 0, position: 'relative'});
            $(this).droppable("disable").animate({borderColor: '#6dda6d'}, 200);

            //mp.trigger('onTakedApple');

            countApples++;

            if(countApples == 5)
            {
                endApp();   
            }
        }
    });
}

function createApple(posY, posX)
{
    let apple = $("<img src='./img/apple.png' class='apple-item'>").css({
        left: posX,
        top: posY,
        position: "absolute"
    }).draggable({
        start: function()
        {
            $(this).appendTo($('.container'))
        }
    });
    $('#apples-box').append(apple);
}

function endApp()
{
    $('#tree').fadeOut(200);
    

    setTimeout(() => {
        $('#end-picture').fadeIn(100);

        setTimeout(() => {
            $('.container').addClass('poshel-nahui');

            setTimeout(() => {
                mp.trigger('endAppleCollectApp');
            }, 2000);
        }, 500);
    }, 210);

    idk = false;
}

function getRandom(range, size)
{
    let m = {};
    let a = [];
    for (let i = 0; i < size; ++i) {
        let r = Math.floor(Math.random() * (range - i));
        a.push(((r in m) ? m[r] : r));
        let l = range - i - 1;
        m[r] = (l in m) ? m[l] : l;
    }

    return a;
}