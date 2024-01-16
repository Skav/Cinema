document.getElementById('loginForm').addEventListener('submit', function (event) {
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
            const token = data.accessToken;
            const refreshToken = data.refreshToken
            const expiresIn = data.expiresIn; // The server should send the expiry time in seconds
            var seconds = parseInt(expiresIn, 10);
            const expirationDate = new Date(new Date().getTime() + seconds * 1000);
            localStorage.setItem('token', token);
            localStorage.setItem('refreshToken', refreshToken);
            localStorage.setItem('tokenExpiration', expirationDate.toISOString()); // Store as ISO string
            localStorage.setItem('isLoggedIn', true);

            alert('Success!');
            // Redirect to index page or show success message
            window.location.href = '/index.html'; // Redirect to the index page
        })
        .catch((error) => {
            console.error('Error:', error);
        });
});
