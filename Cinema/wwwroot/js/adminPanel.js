// Function to add a movie
function addMovie() {
    const title = document.getElementById('title').value;
    const duration = parseInt(document.getElementById('duration').value);
    const genre = document.getElementById('genre').value;
    const description = document.getElementById('description').value;

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
        .then(response => {
            if (response.status != 201) {
                alert('Something goes wrong!')
                console.log(response.status, response);
            }
            return response.json()
        })
        .then(data => {
            console.log('Success:', data);
            // Optionally, provide feedback to the user or clear the form
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

// Function to add a movie show
function addMovieShow() {
    const roomID = document.getElementById('roomID').value;
    const movieID = document.getElementById('movieID').value;
    const date = document.getElementById('date').value;
    const hour = document.getElementById('hour').value;

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
        .then(response => {
            if (response.status != 201) {
                alert('Something goes wrong!')
                console.log(response.status, response);
            }
            return response.json();
        })
        .then(data => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

// Function to add a room
function addRoom() {
    const roomNo = document.getElementById('roomNo').value;
    const rows = document.getElementById('rows').value;
    const seatsInRow = document.getElementById('seatsInRow').value;

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
        .then(response => {
            if (response.status != 201) {
                alert('Something goes wrong!')
                console.log(response.status, response);
            }
            return response.json();
        .then(data => {
            console.log('Success:', data);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}