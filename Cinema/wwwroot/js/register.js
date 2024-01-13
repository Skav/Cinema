document.addEventListener('DOMContentLoaded', function () {
    var registerForm = document.getElementById('registerForm');
    registerForm.addEventListener('submit', function (event) {
        event.preventDefault();
        var email = document.getElementById('email').value;
        var password = document.getElementById('password').value;

        fetch('/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email: email, password: password }),
        })
            .then(function (response) {
                if (!response.ok) {
                    throw new Error('Network response was not ok: ' + response.statusText);
                }
                return response.json();
            })
            .then(function (data) {
                console.log('Success:', data);
                // Handle success, such as redirecting to the login page or displaying a success message
            })
            .catch(function (error) {
                console.error('Error:', error);
                // Handle errors, such as displaying an error message to the user
            });
    });
});
