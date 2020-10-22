const typeMap = [
	{
		imagePath: './notify-icon.png',
		headText: 'Уведомление',
	},
	{
		imagePath: './message-icon.png',
		headText: 'unknown',
	},
	{
		imagePath: './info-icon.png',
		headText: 'Информация',
	},
];
/**
 * Push-уведомления для игрока
 * @param notifyType Тип уведомления
 * @param senderName Имя отправителя сообщения (для notifyType = 1)
 *
 * 0 - Уведомление (Пр: "Вы успешно авторизировались")
 *
 * 1 - Сообщение от игрока с указанием senderName (Пр: "Ку, позвони если свободен")
 *
 * 2 - Какая-либо игровая информация (Пр: "Вы сняли 300$ с банковского счета")
 */
function PushNotification(notifyType = 0, content = 'null', senderName = 'unknown') {
	const nb = document.createElement('div'),
		sh = document.createElement('div'),
		tb = document.createElement('div'),
		ni = document.createElement('img'),
		ht = document.createElement('h4'),
		hb = document.createElement('h5');

	$(nb).addClass('notify-box').hide();
	$('#container').append(nb);
	$(nb).append(
		$(sh).addClass('shadow'),
		$(ni).attr('src', typeMap[notifyType].imagePath).addClass('notify-icon'),
		$(tb).addClass('text-box'),
	);
	$(tb).append(
		$(ht).attr('id', 'head_text').text(notifyType === 1 ? senderName : typeMap[notifyType].headText),
		$(hb).attr('id', 'body_text').text(content),
	);

	$('div.notify-box').last().fadeIn(200);

	$(sh).addClass('puka');

	setTimeout(() => {
		$(nb).fadeOut(300);
		setTimeout(() => nb.remove(), 400);
	}, 10000);

	if ($('div.notify-box').length > 5) $('div.notify-box').first().remove();
}

function dd()
{
    while($('div.notify-box').length < 5)
	PushNotification(0, 'хуй')
}