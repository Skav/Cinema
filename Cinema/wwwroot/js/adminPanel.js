document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/movies', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    })
        .then(response => response.json())
        .then(movies => {
            const movieListTableBody = document.querySelector('#movieListTable tbody');
            movies.forEach(movie => {
                const row = movieListTableBody.insertRow();
                row.innerHTML = `
                <td>${movie.id}</td>
                <td>${movie.title}</td>
                <td>${movie.duration}</td>
                <td>${movie.genre}</td>
                `;

                let delButtonContainer = document.createElement('td');
                delButtonContainer.value = movie.id;
                let delButton = document.createElement('input');
                delButton.value = "DELETE";
                delButton.className = "btn-remove";
                delButton.type = "button";
                delButton.addEventListener("click", deleteMovie);

                delButtonContainer.appendChild(delButton);
                row.appendChild(delButtonContainer);
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });

    fetch('/api/rooms', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    })
        .then(response => response.json())
        .then(rooms => {
            const roomListTableBody = document.querySelector('#roomListTable tbody');
            rooms.forEach(room => {
                const row = roomListTableBody.insertRow();
                row.innerHTML = `
                <td>${room.id}</td>
                <td>${room.roomNo}</td>
                <td>${room.rows}</td>
                <td>${room.seatsInRow}</td>
            `;

                let delButtonContainer = document.createElement('td');
                delButtonContainer.value = room.id;
                let delButton = document.createElement('input');
                delButton.value = "DELETE";
                delButton.className = "btn-remove";
                delButton.type = "button";
                delButton.addEventListener("click", deleteRoom);

                delButtonContainer.appendChild(delButton);
                row.appendChild(delButtonContainer);
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });

    fetch('/api/movieShows/all', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    })
        .then(response => response.json())
        .then(movieShows => {
            const roomListTableBody = document.querySelector('#movieShowsListTable tbody');
            movieShows.forEach(movieShow => {
                const row = roomListTableBody.insertRow();
                row.innerHTML = `
                <td>${movieShow.id}</td>
                <td>${movieShow.roomId}</td>
                <td>${movieShow.movieId}</td>
                <td>${movieShow.date}</td>
                <td>${movieShow.hour}</td>
            `;

                let delButtonContainer = document.createElement('td');
                delButtonContainer.value = movieShow.id;
                let delButton = document.createElement('input');
                delButton.value = "DELETE";
                delButton.className = "btn-remove";
                delButton.type = "button";
                delButton.addEventListener("click", deleteMovieShow);

                delButtonContainer.appendChild(delButton);
                row.appendChild(delButtonContainer);
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });

    fetch('/api/user/all', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    })
        .then(response => response.json())
        .then(users => {
            const usersListTableBody = document.querySelector('#usersListTable tbody');
            fetch('/api/user', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            })
                .then(response => {
                    if (response.status != 200) {
                        console.log("An error occured")
                    }
                    return response.json();
                })
                .then(userInfo => {
                    users.forEach(user => {
                        const row = usersListTableBody.insertRow();
                        row.innerHTML = `
                        <td>${user.id}</td>
                        <td>${user.username}</td>
                        <td>${user.email}</td>
                        <td>${user.role}</td>
                        `;

                        let delButtonContainer = document.createElement('td');
                        if (userInfo.username != user.username && user.role != "Admin") {                       
                            delButtonContainer.value = user.id;
                            let delButton = document.createElement('input');
                            delButton.value = "DELETE";
                            delButton.className = "btn-remove";
                            delButton.type = "button";
                            delButton.addEventListener("click", deleteUser);

                            delButtonContainer.appendChild(delButton);
                        }
                        row.appendChild(delButtonContainer);
                    });
                
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

function deleteUser(context) {

    fetch()
    fetch(`/api/user/${context.target.parentNode.value}/delete`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    })
        .then(response => {
            if (response.status == 200) {
                alert("Success!")
                window.location.reload();
            }
            else if (response.status == 409) {
                return response.json()
            }
            else {
                alert("Something goes wrong  - check if room isn't added to any movieShow")
            }
        })
        .then(data => {
            if (data.error != null) {
                console.log(`Error: ${data.error}`)
            }
        })
}

function deleteMovieShow(context) {
    fetch(`/api/movieShows/${context.target.parentNode.value}/delete`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    })
        .then(response => {
            if (response.status == 200) {
                alert("Success!")
                window.location.reload();
            }
            else if (response.status == 409) {
                return response.json()
            }
            else {
                alert("Something goes wrong")
            }
        })
        .then(data => {
            if (data.error != null) {
                console.log(`Error: ${data.error}`)
            }
        })
}

function deleteMovie(context) {
    fetch(`/api/movies/${context.target.parentNode.value}/delete`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    })
        .then(response => {
            if (response.status == 200) {
                alert("Success!")
                window.location.reload();
            }
            else if (response.status == 409) {
                return response.json()
            }
            else {
                alert("Something goes wrong - check if movie isn't added to any movieShow")
            }
        })
        .then(data => {
            if (data.error != null) {
                console.log(`Error: ${data.error}`)
            }
        })
}

function deleteRoom(context) {
    fetch(`/api/rooms/${context.target.parentNode.value}/delete`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
    })
        .then(response => {
            if (response.status == 200) {
                alert("Success!")
                window.location.reload();
            }
            else if (response.status == 409) {
                return response.json()
            }
            else {
                alert("Something goes wrong  - check if room isn't added to any movieShow")
            }
        })
        .then(data => {
            if (data.error != null) {
                console.log(`Error: ${data.error}`)
            }
        })
}
document.getElementById('manageMoviesBtn').addEventListener('click', function () {
    let div = document.getElementById('addMovieSection');

    if (div.classList.length == 2)
        div.classList.remove('hidden');
    else
        div.classList.add('hidden');
        
});

document.getElementById('manageShowsBtn').addEventListener('click', function () {
    let div = document.getElementById('addShowSection');

    if (div.classList.length == 2)
        div.classList.remove('hidden');
    else
        div.classList.add('hidden');
});

document.getElementById('manageUserBtn').addEventListener('click', function () {
    let div = document.getElementById('addUserSection');

    if (div.classList.length == 2)
        div.classList.remove('hidden');
    else
        div.classList.add('hidden');
});

document.getElementById('addRoomBtn').addEventListener('click', function () {
    let div = document.getElementById('addRoomSection');

    if (div.classList.length == 2)
        div.classList.remove('hidden');
    else
        div.classList.add('hidden');
});

document.getElementById('addMovieBtn').addEventListener('click', function () {
    const title = document.getElementById('title').value;
    const duration = parseInt(document.getElementById('duration').value);
    const genre = document.getElementById('genre').value;
    const description = document.getElementById('description').value;
    const token = localStorage.getItem('token');

    if (title == "" || title == null) {
        alert("Title cannot be empty!");
        return false;
    }

    if (duration == "" || duration == null) {
        alert("Duration cannot be empty!");
        return false;
    }

    if (genre == "" || genre == null) {
        alert("Genre cannot be empty!");
        return false;
    }

    if (description == "" || description == null) {
        alert("Description cannot be empty!");
        return false;
    }

    // Prepare the JSON payload with "available" set to true
    const movieData = {
        title: title,
        duration: duration,
        genre: genre,
        description: description,
        available: true // Set "available" to true as per the requirement
    };

    fetch('/api/movies/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(movieData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                alert(`An error occured: ${data.error}`);
                return false
            }
            alert('Success');
            window.location.reload();
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

document.getElementById('addMovieShowBtn').addEventListener('click', function () {
    const roomID = document.getElementById('roomID').value;
    const movieID = document.getElementById('movieID').value;
    const date = document.getElementById('date').value;
    const hour = document.getElementById('hour').value;
    const token = localStorage.getItem('token');

    if (roomID == "" || roomID == null) {
        alert("roomID cannot be empty!");
        return false;
    }

    if (movieID == "" || movieID == null) {
        alert("movieID cannot be empty!");
        return false;
    }

    if (date == "" || date == null) {
        alert("Date cannot be empty!");
        return false;
    }

    if (hour == "" || hour == null) {
        alert("Hour cannot be empty!");
        return false;
    }

    const movieShowData = {
        roomId: parseInt(roomID),
        movieId: parseInt(movieID),
        date: date,
        hour: hour,
    };

    fetch('/api/movieShows', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(movieShowData)
    })
        .then(response => response.json())
        .then(data => {
        if (data.error) {
            alert(`An error occured: ${data.error}`);
            return false
        }
            alert('Success');
            window.location.reload();
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

document.getElementById('addRoomForm').addEventListener('submit', function (event) {
    event.preventDefault();
    const roomNo = document.getElementById('roomNo').value;
    const rows = document.getElementById('rows').value;
    const seatsInRow = document.getElementById('seatsInRow').value;
    const token = localStorage.getItem('token');

    if (roomNo == "" || roomNo == null) {
        alert("roomNo cannot be empty!");
        return false;
    }

    if (rows == "" || rows == null) {
        alert("Rows cannot be empty!");
        return false;
    }

    if (seatsInRow == "" || seatsInRow == null) {
        alert("SeatsInRow cannot be empty!");
        return false;
    }

    const roomData = {
        roomNo: parseInt(roomNo),
        rows: parseInt(rows),
        seatsInRow: parseInt(seatsInRow)
    };

    fetch('/api/rooms/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(roomData)
    })
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                alert(`An error occured: ${data.error}`);
                return false
            }
            alert('Success');
            window.location.reload();
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

document.getElementById('addUserBtn').addEventListener('click', function () {
    const username = document.getElementById('username').value;
    const email = document.getElementById('email').value;
    console.log(email);
    const password = document.getElementById('password').value;
    const role = document.getElementById('role').value;
    const token = localStorage.getItem('token');

    if (username == "" || username == null) {
        alert("Username cannot be empty!");
        return false;
    }

    if (email == "" || email == null) {
        alert("Email cannot be empty!");
        return false;
    }

    if (password == "" || password == null) {
        alert("Password cannot be empty!");
        return false;
    }

    if (role == "" || role == null) {
        alert("Role cannot be empty!");
        return false;
    }

    // Prepare the JSON payload with "available" set to true
    const userData = {
        username: username,
        email: email,
        password: password,
        role: role
    };

    fetch('/api/user/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(userData)
    })
        .then(response => {
            if (response.status == 201) {
                alert("Success!")
                window.location.reload();
            }
            else if (response.status == 400) {
                alert("Validation error!")
            }
            return response.json()
        })
        .then(data => {
            if (data.error) {
                alert(`An error occured: ${data.error}`);
                return false
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
});