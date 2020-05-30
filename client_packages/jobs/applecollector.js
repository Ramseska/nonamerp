var applejobcheckpoint = null;

mp.events.add({
    'cAppleJobCreateCheckpoint' : (p) => {
        mp.events.callRemote('sCreateApplePointKostil', p);
    },
    'cAppleJobDestroyCheckpoint' : () => {
        mp.events.callRemote('sDestroyApplePointKostil');
    }
});