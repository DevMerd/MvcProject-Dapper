function GirisYap() {
    var veri = {
        Username: $('#username').val(),
        Password: $('#password').val(),
        ReturnUrl: $('#returnUrl').val()
    };

    $.ajax({
        type: "POST",
        url: "/Account/Login",
        data: veri,
        dataType: "json",
        success: function (response) {
            if (response.isError) {
                $('#ErrorResponse').text(response.errorMessage);
                $('#ErrorResponse').css('display', '');
            } else {
                if (response.result != null) {
                    window.location.href = response.result;
                } else {
                    window.location.href = "/";
                }
            }
        }
    });
}
$('#GirisYapBTN').click(function (e) {
    GirisYap();
});