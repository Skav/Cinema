localStorage.setItem('isLoggedIn', false);
localStorage.removeItem('token');
localStorage.removeItem('refreshToken');
localStorage.removeItem('tokenExpiration');

window.location.href = '/index.html';