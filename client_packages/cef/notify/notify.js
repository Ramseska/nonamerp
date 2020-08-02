var notifyBrowser = null;

mp.events.add('pushNotify', (type, content, sendername = null) => {
    if(notifyBrowser === null || notifyBrowser === undefined) notifyBrowser = mp.browsers.new('package://cef/notify/index.html');
    notifyBrowser.execute(`PushNotification(${type}, "${content}"), "${sendername}"`);         
});