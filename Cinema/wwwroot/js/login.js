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
            // Assuming 'data.token' is the property where the token is sent in the response
            localStorage.setItem('token', data.token); // Store the token in local storage
            alert('Success!');
            // Redirect to index page or show success message
            window.location.href = '/index.html'; // Redirect to the index page
        })
        .catch((error) => {
            console.error('Error:', error);
        });
});
