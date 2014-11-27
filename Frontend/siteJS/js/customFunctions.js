$(document).ready(function (e) {
try {
    var cookie = readCookie("UserProfile");
    if (cookie != null)
        window.location.replace("http://localhost/Licenta/sitejs/homepage/");
} catch (Exception) { }

    $('#login').modal('show');
    $('#loginButton').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            url: apiBaseURL+'/auth',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                localStorage["logare"] = "progress";
                window.location.replace(data.split('====')[2]);
            }
        })
    })
});