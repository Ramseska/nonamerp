var isTap = false;
var loginBrowser = null;

function OnEnterAuth(login, password) {
    if (isTap) { mp.gui.chat.push("Подождите.."); return; }
    else { isTap = true; }

    if (login == "" || password == "") 
        loginBrowser.execute(`document.getElementById("log_output").innerHTML = "[Ошибка]: Введите логин и пароль!"`);

    else if(CheckNickNameOnValid(login) != "") 
        loginBrowser.execute(`document.getElementById("log_output").innerHTML = "${CheckNickNameOnValid(login)}"`);

    else if (CheckPasswordOnValid(password) != "") 
        loginBrowser.execute(`document.getElementById("log_output").innerHTML = "${CheckPasswordOnValid(password)}"`);

    else if(CheckPasswordOnValid(password) == "" && CheckNickNameOnValid(login) == "") {
        loginBrowser.execute(`document.getElementById("log_output").innerHTML = "..."`);
        mp.events.callRemote("Event_GetAccountFromBD", 0, login, password);
        return;
    }
    isTap = false;
}


function OnEnterReg(login, password, email) {
    if (isTap) { mp.gui.chat.push("Подождите.."); return; }
    else { isTap = true; }

    if (login == "" || password == "" || email == "") 
        loginBrowser.execute(`document.getElementById("log_output_reg").innerHTML = "[Ошибка]: Заполните все поля!"`);

    else if (CheckNickNameOnValid(login) != "") 
        loginBrowser.execute(`document.getElementById("log_output_reg").innerHTML = "${CheckNickNameOnValid(login)}"`);

    else if (CheckPasswordOnValid(password) != "") 
        loginBrowser.execute(`document.getElementById("log_output_reg").innerHTML = "${CheckPasswordOnValid(password)}"`);

    else if (CheckEmailOnValid(email) != "") 
        loginBrowser.execute(`document.getElementById("log_output_reg").innerHTML = "${CheckEmailOnValid(email)}"`);

    else if (CheckPasswordOnValid(password) == "" && CheckNickNameOnValid(login) == "" && CheckEmailOnValid(email) == "") {
        loginBrowser.execute(`document.getElementById("log_output_reg").innerHTML = "..."`);
        mp.events.callRemote("Event_GetAccountFromBD", 1, login, password, email);
        return;
    }
    isTap = false;
}


function SwitchOnAuth() {
    if (isTap) { mp.gui.chat.push("Подождите.."); return; }

    document.getElementById("reg_box").hidden = true;
    document.getElementById("log_box").hidden = false;
}
function SwitchOnReg() {
    if (isTap) { mp.gui.chat.push("Подождите.."); return; }

    document.getElementById("reg_box").hidden = false;
    document.getElementById("log_box").hidden = true;
}

function CheckNickNameOnValid(name) {
    if (name.length < 3 || name.length > 24) 
        return "[Ошибка]: Никнейм не может содержать меньше 3 и более 24 символов!";

    else if (name.match(/[^A-Za-z_]/gi)) 
        return "[Ошибка]: Никнейм содержит запрещенные символы!";

    else return "";
}
function CheckPasswordOnValid(pass) {
    if (pass.length < 6 || pass.length > 32)
        return "[Ошибка]: Пароль должен содержать от 6 до 32 символов!";
    else if (pass.match(/['`;"]/gi))
            return "[Ошибка]: Пароль содержит запрещенные символы!";
    else return "";
}
function CheckEmailOnValid(email) {
    if (email.length < 8)
        return "[Ошибка]: Слишком короткий E-Mail!";
    else if (!email.match(/[@\.]/gi))
        return "[Ошибка]: Неверный E-Mail адрес!";
    else return "";
    
}
function Load() {
    document.getElementById("reg_box").hidden = true;
    document.getElementById("log_box").hidden = false;
}

mp.events.add("SendBadAnswer", (answer) => {
    loginBrowser.execute(`document.getElementById("log_output").innerHTML = "${answer}"`);
    isTap = false;
});

mp.events.add("SendBadAnswerReg", (answer) => {
    loginBrowser.execute(`document.getElementById("log_output_reg").innerHTML = "${answer}"`);
    isTap = false;
});

mp.events.add('OnEnterAuth', OnEnterAuth);
mp.events.add('OnEnterReg', OnEnterReg);

mp.events.add("CreateAuthWindow", () => {
    if (loginBrowser != null) return;
    loginBrowser = mp.browsers.new("package://cef/authorization/index.html");
    mp.gui.cursor.visible = true;
    mp.gui.chat.activate(false);

    mp.game.graphics.transitionToBlurred(2000);
    mp.players.local.freezePosition(true);
});

mp.events.add("DestroyAuthBrowser", () => {
    mp.gui.chat.activate(true);

    loginBrowser.destroy();
    mp.gui.cursor.visible = false;
    isTap = false;
    loginBrowser = null;
    
    mp.game.graphics.transitionFromBlurred(500);
    mp.players.local.freezePosition(false);
});

mp.events.add("AuthWasCnacelled", () => {
    mp.gui.chat.activate(true);

    loginBrowser.destroy();
    mp.gui.cursor.visible = false;
    isTap = false;
    mp.events.callRemote("Event_CancelAuth");
});