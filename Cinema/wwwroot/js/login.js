﻿document.getElementById('loginForm').addEventListener('submit', function (event) {
    event.preventDefault();
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    fetch('/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email: email, password: password }),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Assuming 'data.token' is the JWT token and 'data.expiresIn' is the token's expiry time in seconds
            const token = data.accessToken;
            const expiresIn = data.expiresIn; // The server should send the expiry time in seconds
            const expirationDate = new Date(new Date().getTime() + expiresIn * 1000); // Convert to milliseconds
            localStorage.setItem('token', token);
            localStorage.setItem('tokenExpiration', expirationDate.toISOString()); // Store as ISO string

            console.log('Success:', data);
            // Redirect to index page or show success message
            window.location.href = '/index.html'; // Redirect to the index page
        })
        .catch((error) => {
            console.error('Error:', error);
        });
});
