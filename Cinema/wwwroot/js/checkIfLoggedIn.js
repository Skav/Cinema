if (localStorage.getItem("isLoggedIn") != 'true') {
    alert("You need to be logged in to acces this site!")
    window.location.href = '/index.html';
}