$(document).ready(function () {

    console.log("Hospital validation loaded ✅");

    // 📱 Phone number – only digits + 10 length
    $("#PhoneNumber").on("input", function () {
        let value = $(this).val() ? $(this).val().replace(/[^0-9]/g, '') : '';
        $(this).val(value);

        if (value.length !== 10) {
            $("#errPhoneNumber").text("Phone number must be 10 digits");
        } else {
            $("#errPhoneNumber").text("");
        }
    });

    // 📧 Email validation
    function isValidEmail(email) {
        let pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return pattern.test(email);
    }

    // 🚀 FORM SUBMIT – HARD STOP + VALIDATE
    $("#hospitalForm").on("submit", function (e) {

        e.preventDefault(); // ⛔ STOP submit first

        $(".text-danger").text("");
        let isValid = true;

        let isUpdate = $("#Id").val() && $("#Id").val() !== "0";

        // 🏥 Name
        let nameVal = $("#Name").val() || '';
        if (nameVal.trim() === "") {
            $("#errName").text("Hospital Name is required");
            isValid = false;
        }

        // 📱 Phone
        let phone = $("#PhoneNumber").val() || '';
        if (phone.trim().length !== 10) {
            $("#errPhoneNumber").text("Valid 10 digit phone number required");
            isValid = false;
        }

        // 📧 Email
        let email = $("#EmailId").val() || '';
        if (email.trim() === "" || !isValidEmail(email)) {
            $("#errEmail").text("Valid email required");
            isValid = false;
        }

        // 🧾 Registration
        let reg = $("#RegistrationNumber").val() || '';
        if (reg.trim() === "") {
            $("#errRegNumber").text("Registration Number is required");
            isValid = false;
        }

        // 📝 Description
        let desc = $("#Description").val() || '';
        if (desc.trim() === "") {
            $("#errDescription").text("Description is required");
            isValid = false;
        }

        // 🔗 Meta
        let meta = $("#MetaLink").val() || '';
        if (meta.trim() === "") {
            $("#errMetaLink").text("Meta Link is required");
            isValid = false;
        }

        // 📸 Instagram
        let insta = $("#InstaLink").val() || '';
        if (insta.trim() === "") {
            $("#errInstaLink").text("Instagram link is required");
            isValid = false;
        }




        // 🏥 Parent Hospital (Sub hospital case)
        let hospitalType = $("#HospitalType").val() || '';
        let parentId = $("#ParentHospitalId").val() || '';
        if (hospitalType === "true" && parentId === "") {
            alert("Please select Parent Hospital");
            isValid = false;
        }

        // ✅ SUBMIT ONLY WHEN VALID
        if (isValid) {
            this.submit();
        }
    });

    
});
