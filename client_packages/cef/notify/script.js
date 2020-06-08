/**
 * Пушит уведомление на экран.
 * 
 * Типы уведомлений:
 * 
 * 0 - Обычное уведомление.
 * 
 * 1 - Сообщение от игрока
 * 
 * 2 - Информация о чем-либо
 */
function PushNotification(notifyType = 0, content = "Null", senderName = "Undefined")
{
    /*
        0 - Уведомление
        1 - Сообщение
        2 - Информация
    */
    let nb = document.createElement('div'), 
        sh = document.createElement('div'), 
        ni = document.createElement('img'), 
        tb = document.createElement('div'), 
        ht = document.createElement('h4'),
        hb = document.createElement('h5');

    let imagePath = headText = "";

    switch(notifyType)
    {
        case 0: 
        {
            imagePath = "./notify-icon.png";
            headText = "Уведомление";
            break;
        }
        case 1: 
        {
            imagePath = "./message-icon.png";
            headText = senderName;
            break;
        }
        case 2: 
        {
            imagePath = "./info-icon.png";
            headText = "Информация";
            break;
        }   
        default:
        {
            imagePath = "./notify-icon.png";
            headText = "Уведомление";
            break;
        }
    }

    $(nb).addClass('notify-box').hide();
    $('#container').append(nb);
    $(sh).addClass('shadow').appendTo(nb);
    $(ni).attr('src', imagePath).addClass('notify-icon').appendTo(nb);
    $(tb).addClass('text-box').appendTo(nb);
    $(ht).attr('id', 'head_text').text(headText).appendTo(tb);
    $(hb).attr('id', 'body_text').text(content).appendTo(tb);

    $('div.notify-box').last().fadeIn(200);

    $(sh).addClass('puka');

    setTimeout(() => {
        $(nb).fadeOut(300);
        setTimeout(() => { nb.remove(); }, 400);
    }, 10000);

    if($('div.notify-box').length > 5)
        $('div.notify-box').first().remove();
}