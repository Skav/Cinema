if (localStorage.getItem("IsLoggedIn") != true)
    window.history.back()


fetch('/api/user', {
    method: 'GET',
    headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
        'Content-Type': 'application/json',
    }
})
    .then(response => {
        if (response.status != 200) {
            window.history.back();
        }
        return response.json();
    })
    .then(user => {
        if (user.role != "Admin" || user.role != "Staff")
            window.history.back();
        })



