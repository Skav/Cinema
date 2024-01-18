// Function to add a movie
document.getElementById('addMovieBtn').addEventListener('click', () => {
    const title = document.getElementById('title').value;
    const duration = parseInt(document.getElementById('duration').value);
    const genre = document.getElementById('genre').value;
    const description = document.getElementById('description').value;

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
        available: true, // Set "available" to true as per the updated requirement
    };

    // Retrieve the token from local storage
    const token = localStorage.getItem('token');

    // Send the POST request to the API endpoint with the Authorization header
    fetch('/api/movies/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            // Include the token in the Authorization header
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(movieData),
    })
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                alert(`An error occured: ${data.error}`);
                return false
            }
            console.log('Success:', data);
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

// Function to add a movie show
document.getElementById('addMovieShowBtn').addEventListener('click', () => {
    const roomID = document.getElementById('roomID').value;
    const movieID = document.getElementById('movieID').value;
    const date = document.getElementById('date').value;
    const hour = document.getElementById('hour').value;

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
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify(movieShowData),
    })
        .then(response => response.json())
        .then(data => {
        if (data.error) {
            alert(`An error occured: ${data.error}`);
            return false
        }
        console.log('Success:', data);
        })
        .catch(error => {
            console.error('Error:', error);
        });
});

// Function to add a room
document.getElementById('addRoomBtn').addEventListener('click', () => {
    const roomNo = document.getElementById('roomNo').value;
    const rows = document.getElementById('rows').value;
    const seatsInRow = document.getElementById('seatsInRow').value;

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
        seatsInRow: parseInt(seatsInRow),
    };

    fetch('/api/rooms/create', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify(roomData),
    })
        .then(response => response.json())
        .then(data => {
            if (data.error) {
                alert(`An error occured: ${data.error}`);
                return false
            }
            console.log('Success:', data);
        })
        .catch(error => {
            console.error('Error:', error);
        });
        });