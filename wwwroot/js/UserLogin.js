$(document).ready(function () {

    // Event delegation: works even after replacing modal HTML
    $("#UserLoginModal").on("click", "button[name='login']", function () {

        var url = "/UserAuth/Login";
        var antiForgeryToken = $("#UserLoginModal input[name='__RequestVerificationToken']").val();
        var email = $("#UserLoginModal input[name='Email']").val();
        var password = $("#UserLoginModal input[name='Password']").val();
        var rememberMe = $("#UserLoginModal input[name='RememberMe']").prop('checked');

        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            RememberMe: rememberMe
        };

        $.ajax({
            type: "POST",
            url: url,
            data: userInput,
            success: function (data) {
                var parsed = $.parseHTML(data);
                var hasErrors = $(parsed).find("input[name='LoginInValid']").val() === "True";

                if (hasErrors) {
                    $("#UserLoginModal .modal-content").html(data);
                } else {
                    location.href = '/'; // redirect to home on success
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.error("Error: " + thrownError);
            }
        });

    });

});
