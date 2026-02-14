$(function () {
    console.log("Registration script loaded");

    // Bootstrap 5 uses 'hidden.bs.modal' (same as Bootstrap 4, so it's fine)
    $("#UserRegistrationModal").on('hidden.bs.modal', function (e) {
        $("#UserRegistrationModal input[name='CategoryId']").val('0');
    });

    // Update trigger links to use data-bs-toggle
    $('.RegisterLink').click(function (e) {
        e.preventDefault();
        var categoryId = $(this).attr('data-categoryId');
        $("#UserRegistrationModal input[name='CategoryId']").val(categoryId);

        // Bootstrap 5 modal show
        var modal = new bootstrap.Modal(document.getElementById('UserRegistrationModal'));
        modal.show();
    });

    // Terms agreement checkbox
    $("#UserRegistrationModal input[name='AcceptUserAgreement']").click(onAcceptUserAgreementClick);

    function onAcceptUserAgreementClick() {
        var isChecked = $(this).is(":checked");
        $("#UserRegistrationModal button[name='register']").prop("disabled", !isChecked);
    }

    // Email validation on blur
    $("#UserRegistrationModal input[name='Email']").blur(function () {
        var email = $(this).val();
        if (!email) return;

        var url = "UserAuth/UserNameExists?userName=" + encodeURIComponent(email);

        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                if (data == true) {
                    PresentClosableBootstrapAlert("#alert_placeholder_register", "warning", "Invalid Email", "This email address has already been registered");
                } else {
                    CloseAlert("#alert_placeholder_register");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;
                PresentClosableBootstrapAlert("#alert_placeholder_register", "danger", "Error!", errorText);
                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }
        });
    });

    // Register button click handler
    var registerUserButton = $("#UserRegistrationModal button[name='register']").click(onUserRegisterClick);

    function onUserRegisterClick() {
        var url = "UserAuth/RegisterUser";
        var antiForgeryToken = $("#UserRegistrationModal input[name='__RequestVerificationToken']").val();

        // Collect form data
        var formData = {
            __RequestVerificationToken: antiForgeryToken,
            Email: $("#UserRegistrationModal input[name='Email']").val(),
            Password: $("#UserRegistrationModal input[name='Password']").val(),
            ConfirmPassword: $("#UserRegistrationModal input[name='ConfirmPassword']").val(),
            FirstName: $("#UserRegistrationModal input[name='FirstName']").val(),
            LastName: $("#UserRegistrationModal input[name='LastName']").val(),
            Address1: $("#UserRegistrationModal input[name='Address1']").val(),
            Address2: $("#UserRegistrationModal input[name='Address2']").val(),
            PostCode: $("#UserRegistrationModal input[name='PostCode']").val(),
            PhoneNumber: $("#UserRegistrationModal input[name='PhoneNumber']").val(),
            AcceptUserAgreement: $("#UserRegistrationModal input[name='AcceptUserAgreement']").is(":checked"),
            CategoryId: $("#UserRegistrationModal input[name='CategoryId']").val()
        };

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            success: function (data) {
                var parsed = $.parseHTML(data);
                var hasErrors = $(parsed).find("input[name='RegistrationInValid']").val() == 'true';

                if (hasErrors) {
                    // Update modal with error response
                    $("#UserRegistrationModal").html($(parsed).find('#UserRegistrationModal').html() || data);

                    // Rebind events
                    $("#UserRegistrationModal button[name='register']").off('click').click(onUserRegisterClick);
                    $("#UserRegistrationModal input[name='AcceptUserAgreement']").off('click').click(onAcceptUserAgreementClick);

                    // Reinitialize validation
                    var form = $("#UserRegistrationForm");
                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                } else {
                    // Success - close modal and redirect
                    var modal = bootstrap.Modal.getInstance(document.getElementById('UserRegistrationModal'));
                    modal.hide();
                    location.href = '/Home/Index';
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;
                PresentClosableBootstrapAlert("#alert_placeholder_register", "danger", "Error!", errorText);
                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }
        });
    }
});