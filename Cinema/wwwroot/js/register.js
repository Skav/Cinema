document.getElementById('registerForm').addEventListener('submit', function (event) {
    event.preventDefault();
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    fetch('/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email: email, password: password }),
    })
        .then(response => {
            if (response.ok) {
                // If the response is ok, display a success message and redirect
                alert('Account has been created successfully.');
                window.location.href = '/login.html';
            } else {
                // If the response is not ok, parse the JSON to get the error message
                return response.json().then(errorData => {
                    // Extract the message after "0:"
                    const errorMessage = errorData.errors[Object.keys(errorData.errors)[0]][0];
                    // Throw an error with the message from the server response
                    throw new Error(errorMessage || 'An error occurred');
                });
            }
        })
        .catch((error) => {
            console.error('Error:', error);
            // Display the error message in a message box
            alert(error.message);
        });
});