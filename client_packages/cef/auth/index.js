const authData = 
{
    switcher: false, // false - auth | true = reg
    login: "",
    password: "",
    email: "",
    floodKD: null,
    passwordHiden: true
}

$(document).ready(() => {
    $("#box-switch").click(() => OnSwitch());
    $("#button-cancel").click(() => {
        mp.trigger("cancelAuth");
    });

    $("#button-accept").click(() => {
        if(authData.floodKD) { outLog.send("Не флудите!"); return; }
        authData.floodKD = setTimeout(() => authData.floodKD = null, 3000);

        // code there
        if(!authData.switcher)
        {
            if(checkLoginOnValid($("#login").val()))
            {
                outLog.send(checkLoginOnValid($("#login").val()));
                return;
            }
            else if(checkPasswordOnValid($("#password").val()))
            {
                outLog.send(checkPasswordOnValid($("#password").val()));
                return;
            }
            
            mp.trigger('authSendToServer', +authData.switcher, authData.login, authData.password, authData.email);
            hideOutLog();
        }
        else 
        {
            if(checkLoginOnValid($("#login").val()))
            {
                outLog.send(checkLoginOnValid($("#login").val()));
                return;
            }
            else if(checkPasswordOnValid($("#password").val()))
            {
                outLog.send(checkPasswordOnValid($("#password").val()));
                return;
            }
            else if(checkMailOnValid($("#email").val()))
            {
                outLog.send(checkMailOnValid($("#email").val()));
                return;
            }

            mp.trigger('authSendToServer', +authData.switcher, authData.login, authData.password, authData.email);
            hideOutLog();
        }
        outLog.send('Wait...');
    });
    
    $("#password-icon").click(function () {
        if(authData.passwordHiden)
        {
            $(this).attr('src', './img/ne-hiden.png');
            $("#password").attr('type', 'text');
        }
        else
        {
            $(this).attr('src', './img/hiden.png');
            $("#password").attr('type', 'password');   
        }
        
        authData.passwordHiden = !authData.passwordHiden;
    });

    $(".input-box").focusin(function(el) {
        $(el.target).parent().children().addClass('input-focused');
    });
    $(".input-box").focusout(function(el) {
        $(el.target).parent().children().removeClass('input-focused');
    });

    $('#login').on('input', function() {
        authData.login = $("#login").val();

        if(checkLoginOnValid($(this).val())) $(this).parents().children().addClass('input-error');
        else $(this).parents().children().removeClass('input-error');
    });
    $('#password').on('input', function() {
        authData.password = $("#password").val();

        if(checkPasswordOnValid($(this).val())) $(this).parents().children().addClass('input-error');
        else $(this).parents().children().removeClass('input-error');
    });
    $('#email').on('input', function() {
        authData.email = $("#email").val();

        if(checkMailOnValid($(this).val())) $(this).parents().children().addClass('input-error');
        else $(this).parents().children().removeClass('input-error');
    });
});

function OnSwitch()
{
    if(!authData.switcher)
    {
        $("#title").html("Регистрация");
        $("#box-switch").html("Авторизироваться");
        $("#button-accept").val("Продолжить");
        $("#email-container").fadeIn(200);
    }
    else
    {
        $("#title").html("Авторизация");
        $("#box-switch").html("Создать аккаунт");
        $("#button-accept").val("Войти");
        $("#email-container").fadeOut(100);
    }

    authData.switcher = !authData.switcher;
    clearInputs();
    hideOutLog();
}       

function clearInputs()
{
    $("#login").val(authData.login = "");
    $("#password").val(authData.password = "");
    $("#email").val(authData.email = "");
}

const outLog =
{
    text: "",
    send: (log) => {
        if(log == null || log == undefined) return;
        if(outLog.text.length < log.length)
        {
            $("#box-outlog").fadeIn(100);
            $("#box-outlog").html(outLog.text = outLog.text + log[outLog.text.length++]);
            setTimeout(() => outLog.send(log), 10);
        }
        else outLog.text = "";
    }
}

function hideOutLog() 
{ 
    $("#box-outlog").html("").fadeOut(10); 
}

function checkLoginOnValid(login)
{
    if(login.length < 2 || login.length > 24)
    {
        return 'Ошибка: Логин не может быть короче 2-х и длиннее 24-х символов!';
    }
    if(login.match(/[^A-Za-z0-9_]/)) return 'Ошибка: Логин содержит запрещенные символы!';
    return false;
}

function checkPasswordOnValid(password)
{
    if(password.length < 6)
    {
        return 'Ошибка: Пароль не может быть короче 6-ти символов!';
    }
    return false;
}

function checkMailOnValid(email)
{
    if(email.match(/^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/) == null) return 'Ошибка: Некорректный email!';
    return false;
}