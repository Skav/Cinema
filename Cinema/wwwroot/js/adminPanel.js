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
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

function showMovieShows(movieId) {
    // Implement the logic to show movie shows for the given movieId
    console.log('Show shows for movie ID:', movieId);
    // You might want to navigate to another page or open a modal with the movie shows
}

document.getElementById('manageMoviesBtn').addEventListener('click', function () {
    let div = document.getElementById('addMovieSection');

    if (div.classList.length == 2)
        div.classList.remove('hidden');
    else
        div.classList.add('hidden');
        
});

document.addEventListener('DOMContentLoaded', function () {
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
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });
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

document.getElementById('manageShowsBtn').addEventListener('click', function () {
    let div = document.getElementById('addShowSection');

    if (div.classList.length == 2)
        div.classList.remove('hidden');
    else
        div.classList.add('hidden');
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

document.getElementById('addRoomBtn').addEventListener('click', function () {
    let div = document.getElementById('addRoomSection');

    if (div.classList.length == 2)
        div.classList.remove('hidden');
    else
        div.classList.add('hidden');
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