var loginController = function () {
//    Phương thức tỉnh khi chạy lần đầu tiên
    this.initialize = function() {
        registerEvents();
    }
    var registerEvents = function () {
        $('#frmLogin').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                userName: "required",
                password: "required"
            },
            message: {
                userName: "you have to enter",
                password: "you have to enter"
            }
        });
        $('#btnLogin').on('click', function (e) {
            if ($('#frmLogin').valid()) {
                e.preventDefault();
                var user = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(user, password);
           }

        });
    }
    var login = function (user, pass) {
        $.ajax({
            type: 'POST',
            data: {
                Username: user,
                Password: pass
            },
            dateType: 'json',
            url: '/admin/login/authen',
            success: function (res) {
                console.log(res);
                if (res.Success) {
                    
                   window.location.href = "/Admin/Home/Index";
                } else {
                    tedu.notify('Login failed', 'error');
                }
            }
        });
    }
}