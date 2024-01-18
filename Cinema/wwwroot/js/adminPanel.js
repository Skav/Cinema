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
    document.getElementById('addMovieSection').style.display = 'block';
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
        .then(response => {
            if (response.status !== 201) {
                throw new Error('Failed to create movie');
            }
            return response.json();
        })
        .then(data => {
            console.log('Movie added:', data);
            // Optionally, reset the form and hide the add movie section
            document.getElementById('addMovieForm').reset();
            document.getElementById('addMovieSection').style.display = 'none';
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

document.getElementById('manageShowsBtn').addEventListener('click', function () {
    document.getElementById('addShowSection').style.display = 'block';
});

document.getElementById('addShowBtn').addEventListener('click', function () {
    const roomID = document.getElementById('roomID').value;
    const movieID = document.getElementById('movieID').value;
    const date = document.getElementById('date').value;
    const hour = document.getElementById('hour').value;
    const token = localStorage.getItem('token');

    // Generate the current timestamp in ISO 8601 format
    const now = new Date().toISOString();

    const showData = {
        roomId: parseInt(roomID),
        movieId: parseInt(movieID),
        date: date,
        hour: hour,
        dateUpdate: now // Set to current timestamp in ISO 8601 format
    };

    fetch('/api/movieShows', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(showData)
    })
        .then(response => {
            if (response.status !== 201) {
                throw new Error('Failed to create movie show');
            }
            return response.json();
        })
        .then(data => {
            console.log('Movie show added:', data);
            // Optionally, reset the form and hide the add show section
            document.getElementById('addShowForm').reset();
            document.getElementById('addShowSection').style.display = 'none';
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

document.getElementById('addRoomBtn').addEventListener('click', function () {
    document.getElementById('addRoomSection').style.display = 'block';
});

document.getElementById('addRoomForm').addEventListener('submit', function (event) {
    event.preventDefault();
    const roomNo = document.getElementById('roomNo').value;
    const rows = document.getElementById('rows').value;
    const seatsInRow = document.getElementById('seatsInRow').value;
    const token = localStorage.getItem('token');

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
        .then(response => {
            if (response.status !== 201) {
                throw new Error('Failed to create room');
            }
            return response.json();
        })
        .then(data => {
            console.log('Room added:', data);
            // Optionally, reset the form and hide the add room section
            document.getElementById('addRoomForm').reset();
            document.getElementById('addRoomSection').style.display = 'none';
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

document.addEventListener('DOMContentLoaded', function () {
    const token = localStorage.getItem('token');
    console.log('Token:', token); // For debugging: log the token to the console

    fetch('/api/movieShows/all', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(movieShows => {
            console.log('Movie Shows:', movieShows); // For debugging: log the movie shows to the console
            const movieShowsListTableBody = document.querySelector('#movieShowsListTable tbody');
            movieShows.forEach(show => {
                const row = movieShowsListTableBody.insertRow();
                row.innerHTML = `
                <td>${show.id}</td>
                <td>${show.roomId}</td>
                <td>${show.movieId}</td>
                <td>${show.date}</td>
                <td>${show.hour}</td>
            `;
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });
});