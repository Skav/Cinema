document.getElementById('registerForm').addEventListener('submit', function (event) {
    event.preventDefault();
    const email = document.getElementById('emailRegister').value;
    const password = document.getElementById('passwordRegister').value;
    const username = document.getElementById('usernameRegister').value;

    fetch('/api/user/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email: email, username: username, password: password }),
    })
        .then(response => {
            if (response.status == 201) {
                alert('Account has been created successfully.');
                window.location.href = '/login.html';
            }
            else if (response.status == 409) {
                return response.json();
            }
            else {
                alert("Something goes wrong");
            }
        })
        .then(data => {
            alert(`Error: ${data.error}`);
        })
        .catch((error) => {
            console.error('Error:', error);
            // Display the error message in a message box
            alert(error.message);
        });
});