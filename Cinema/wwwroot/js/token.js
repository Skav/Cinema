let isLoggedIn = localStorage.getItem("isLoggedIn");

if (isLoggedIn == 'true') {
    let expireDate = localStorage.getItem("tokenExpiration");
    let currDate = new Date().toISOString();
    if (expireDate <= currDate) {
        let refreshToken = localStorage.getItem("refreshToken");
        fetch('/refresh', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ refreshToken: refreshToken }),
        })
            .then(response => {
                if (response.status == 401) {
                    localStorage.setItem('isLoggedIn', false);
                    localStorage.removeItem('token');
                    localStorage.removeItem('refreshToken');
                    localStorage.removeItem('tokenExpiration');
                    fillTopBar();
                    throw new Error('RefreshToken expired or is invalid - logout user');
                }
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                const token = data.accessToken;
                const expiresIn = data.expiresIn; // The server should send the expiry time in seconds
                var seconds = parseInt(expiresIn, 10);
                const expirationDate = new Date(new Date().getTime() + seconds * 1000);
                localStorage.setItem('token', token);
                localStorage.setItem('refreshToken', refreshToken);
                localStorage.setItem('tokenExpiration', expirationDate.toISOString()); // Store as ISO string
                fillTopBar();
                alert('Token refreshed!');
                window.location.reload;
            })
            .catch((error) => {
                console.error('Error:', error);
            });
    }
    else {
        fillTopBar();
    }
}
else {
    localStorage.setItem('isLoggedIn', false);
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('tokenExpiration');
    fillTopBar();
}

function fillTopBar() {
    const topBar = document.getElementById('topBar');
    let homeButton = document.createElement('a');
    homeButton.href = '/index.html';
    homeButton.textContent = "Home";

    topBar.appendChild(homeButton);
    let loggedIn = localStorage.getItem("isLoggedIn");

    if (loggedIn == 'true') {
        let profileButton = document.createElement('a');
        profileButton.href = '/profile.html';
        profileButton.textContent = 'Profile';

        let logOutButton = document.createElement('a');
        logOutButton.href = '/logout.html';
        logOutButton.textContent = "Logout";

        topBar.appendChild(profileButton);
        topBar.appendChild(logOutButton);
    }
    else {
        let loginButton = document.createElement('a');
        loginButton.href = '/login.html';
        loginButton.textContent = "Login";
        let registerButton = document.createElement('a');
        registerButton.href = '/register.html';
        registerButton.textContent = "Register";

        topBar.appendChild(loginButton);
        topBar.appendChild(registerButton);
    }
}
