$(function () {
    var errorText = "An error has occurred. An administrator has been notified. Please try again later";
    $("button[name='SaveSelectedUsers']").prop('disabled', true);

    $('select').on('change', function () {
        var url = "/Admin/UsersToCategory/GetUsersForCategory?categoryId=" + this.value;

        if (this.value != 0) {
            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    $("#UsersCheckList").html(data);
                    $("button[name='SaveSelectedUsers']").prop('disabled', false);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    PresentClosableBootstrapAlert("#alert_placeholder", "danger", "An error occurred!", errorText);
                    console.error("An error has occurred: " + thrownError + "Status: " + xhr.status + "\r\n" + xhr.responseText);
                }
            });
        } else {
            $("button[name='SaveSelectedUsers']").prop('disabled', true);
            $("input[type=checkbox]").prop("checked", false);
            $("input[type=checkbox]").prop("disabled", true);
        }
    });

    $('#SaveSelectedUsers').click(function () {
        var url = "/Admin/UsersToCategory/SaveSelectedUsers";
        var categoryId = $("#CategoryId").val();
        var antiForgeryToken = $("input[name='__RequestVerificationToken']").val();
        var usersSelected = [];

        // DISABLE the button to prevent multiple clicks
        $(this).prop('disabled', true);
        DisableControls(true);

        // Show Bootstrap progress bar - FIXED
        $('#saveProgressBar').fadeIn(); // Use fadeIn() instead of show()

        $('input[type=checkbox]:checked').each(function () {
            var userModel = {
                Id: $(this).attr("value")
            };
            usersSelected.push(userModel);
        });

        var usersSelectedForCategory = {
            __RequestVerificationToken: antiForgeryToken,
            CategoryId: categoryId,
            UserSelected: usersSelected
        };

        $.ajax({
            type: "POST",
            url: url,
            data: usersSelectedForCategory,
            success: function (data) {
                $("#UsersCheckList").html(data);

                // Hide progress bar on success after a small delay
                setTimeout(function () {
                    $('#saveProgressBar').fadeOut(400, function () {
                        DisableControls(false);

                        // Show success message if you have one
                        // Example: Show a Bootstrap toast or alert
                        if ($('.alert-success').length) {
                            $('.alert-success').fadeIn().delay(2000).fadeOut();
                        }
                    });
                }, 500); // Small delay to ensure user sees the progress

            },
            error: function (xhr, ajaxOptions, thrownError) {
                // Hide progress bar on error
                $('#saveProgressBar').fadeOut(400, function () {
                    PresentClosableBootstrapAlert("#alert_placeholder", "danger", "An error occurred!", errorText);
                    console.error("An error has occurred: " + thrownError + "Status: " + xhr.status + "\r\n" + xhr.responseText);
                    DisableControls(false);
                });
            }
        });
    });

    function DisableControls(disable) {
        $('input[type=checkbox]').prop("disabled", disable);
        $("#SaveSelectedUsers").prop('disabled', disable);
        $('select').prop('disabled', disable);
    }
});