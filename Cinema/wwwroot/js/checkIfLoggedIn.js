let url = window.location.href.split('/');
if (url.includes('register.html') || url.includes('login.html')) {
    if(localStorage.getItem("isLoggedIn") == "true")
        window.history.back();
}
else if (localStorage.getItem("isLoggedIn") != 'true') {
    alert("You need to be logged in to acces this site!")
    window.location.href = '/index.html';
}
