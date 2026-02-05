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

    // Mobile number validation
    $("#PhoneNumber").on("input", function () {
        let value = $(this).val().replace(/[^0-9]/g, '');
        $(this).val(value);
        if (value.length !== 10) {
            $("#mobileError").text("Mobile number must be 10 digits");
        } else {
            $("#mobileError").text("");
        }
    });

    // Form submit validation
    $("#signupForm").submit(function (e) {
        let firstName = $("input[name='FirstName']").val().trim();
        let lastName = $("input[name='LastName']").val().trim();
        let gender = $("select[name='Gender']").val();
        let age = $("input[name='Age']").val();
        let email = $("input[name='Email']").val().trim();
        let address = $("textarea[name='Address']").val().trim();
        let valid = true;
        let errorMsg = "";
        let emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (firstName === "") { valid = false; errorMsg += "First name required<br>"; }
        if (lastName === "") { valid = false; errorMsg += "Last name required<br>"; }
        if (gender === "") { valid = false; errorMsg += "Gender required<br>"; }
        if (age === "" || age <= 0) { valid = false; errorMsg += "Valid age required<br>"; }
        if (address === "") { valid = false; errorMsg += "Address required<br>"; }
        if (email === "" || !emailPattern.test(email)) { valid = false; errorMsg += "Valid email required<br>"; }
        if ($("#mobileError").text() !== "") { valid = false; errorMsg += "Mobile number invalid<br>"; }
        if ($("#passwordRule").hasClass("text-danger")) { valid = false; errorMsg += "Password invalid<br>"; }

        if (!valid) {
            e.preventDefault();
            toastr.error(errorMsg, "Validation Error", { timeOut: 5000, extendedTimeOut: 2000 });
        }
    });
});
