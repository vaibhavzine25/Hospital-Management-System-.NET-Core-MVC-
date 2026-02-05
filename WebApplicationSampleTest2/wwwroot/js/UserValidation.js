$(document).ready(function () {

    // Password validation
    $("#Password").on("keyup", function () {
        let password = $(this).val();
        let capitalFirst = /^[A-Z]/.test(password);
        let minLength = password.length >= 8;
        let hasNumber = /\d/.test(password);
        let hasSymbol = /[@$!%*?&#]/.test(password);

        if (capitalFirst && minLength && hasNumber && hasSymbol) {
            $("#passwordRule").text("Strong password ✔").removeClass("text-danger").addClass("text-success");
        } else {
            $("#passwordRule").text("Start with capital, min 8 chars, 1 number & 1 symbol (@$!%*?&#)").removeClass("text-success").addClass("text-danger");
        }
    });


    // 📱 Phone input: allow only numbers
    $("#PhoneNo").on("input", function () {
        let value = $(this).val().replace(/[^0-9]/g, '');
        $(this).val(value);
    });

    // 📧 Email regex
    function isValidEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    // 🚀 Form submit validation
    $("#userForm").on("submit", function (e) {
        let isValid = true;
        let isUpdate = $("#Id").val() !== "" && $("#Id").val() !== "0";

        // Clear previous errors
        $("small.text-danger").text("");

        // First Name
        if ($("#FirstName").val().trim() === "") {
            $("#errFirstName").text("First Name is required");
            isValid = false;
        }

        // Last Name
        if ($("#LastName").val().trim() === "") {
            $("#errLastName").text("Last Name is required");
            isValid = false;
        }

        // Login Name
        if ($("#LoginName").val().trim() === "") {
            $("#errLoginName").text("Login Name is required");
            isValid = false;
        }

        // Role
        if ($("#Role").val().trim() === "") {
            $("#errRole").text("Role is required");
            isValid = false;
        }

        // Gender
        if ($("#Gender").val() === "") {
            $("#errGender").text("Gender is required");
            isValid = false;
        }

        // Main Hospital
        if ($("#MainHospital").val() === "") {
            $("#errMainHospital").text("Please select Main Hospital");
            isValid = false;
        }

        // Sub Hospital (only if visible)
        if ($("#subDiv").is(":visible") && $("#SubHospital").val() === "") {
            $("#errSubHospital").text("Please select Sub Hospital");
            isValid = false;
        }

        // Phone
        let phone = $("#PhoneNo").val().trim();
        if (phone.length !== 10) {
            $("#errPhoneNo").text("Phone number must be 10 digits");
            isValid = false;
        }

        // Email
        let email = $("#EmailId").val().trim();
        if (!isValidEmail(email)) {
            $("#errEmail").text("Valid email required");
            isValid = false;
        }

        // Password only on Create
        if (!isUpdate) {
            let pwd = $("#Password").val().trim();
            if (pwd === "") {
                $("#passwordRule").text("Password is required").removeClass("text-success").addClass("text-danger");
                isValid = false;
            }
            if ($("#passwordRule").hasClass("text-danger")) {
                isValid = false;
            }
        }

        if (!isValid) e.preventDefault();
    });

    // 🔹 Main hospital change -> load sub hospitals
    $('#MainHospital').on('change', function () {
        var mainHospitalId = $(this).val();
        $('#SubHospital').empty().append('<option value="">-- Select Sub Hospital --</option>');
        if (!mainHospitalId) {
            $('#subDiv').hide();
            return;
        }

        $.ajax({
            url: '/User/GetSubHospitals',
            type: 'GET',
            data: { mainHospitalId: mainHospitalId },
            success: function (data) {
                if (data && data.length > 0) {
                    $('#subDiv').show();
                    $.each(data, function (i, item) {
                        $('#SubHospital').append(`<option value="${item.id}">${item.name}</option>`);
                    });
                } else {
                    $('#subDiv').hide();
                }
            },
            error: function () {
                $('#subDiv').hide();
            }
        });
    });

});
