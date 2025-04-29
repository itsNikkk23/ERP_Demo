function validateForm() {
    const firstName = document.getElementById('firstName').value.trim();
    const lastName = document.getElementById('lastName').value.trim();
    const email = document.getElementById('email').value.trim();
    const phone = document.getElementById('phone').value.trim();
    const password = document.getElementById('password').value.trim();
    const confirmPassword = document.getElementById("confirmPassword").value.trim();

    // Regular expressions for email and phone validation
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const phonePattern = /^[0-9]{10}$/; // Validates a 10-digit phone number

    // Check if all fields are filled
    if (!firstName || !lastName || !email || !phone || !password) {
        alert("All fields must be filled out.");
        return false;
    }

    // Check if email format is valid
    if (!emailPattern.test(email)) {
        alert("Please enter a valid email address.");
        return false;
    }

    // Check if phone number is valid (10 digits)
    if (!phonePattern.test(phone)) {
        alert("Please enter a valid 10-digit phone number.");
        return false;
    }

    // Check if password length is at least 6 characters
    if (password.length < 6) {
        alert("Password must be at least 6 characters long.");
        return false;
    }
    if (password !== confirmPassword) {
        alert("Password and Confirm Password do not match.");
        return false;
    }
    return true;
}
function validateLoginForm() {
    const email = document.getElementById('emailL').value.trim();
    const password = document.getElementById('pass').value.trim();

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email || !password) {
        alert("Email and password must be filled out.");
        return false;
    }
    if (!emailPattern.test(email)) {
        alert("Please enter a valid email address.");
        return false;
    }
    if (password.length < 6) {
        alert("Password must be at least 6 characters long.");
        return false;
    }
}