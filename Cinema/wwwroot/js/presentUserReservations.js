document.addEventListener('DOMContentLoaded', function () {
    // Fetch loyalty points and display them
    fetch(`/api/loyalityPoints`, {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())
        .then(data => {
            const pointsContainer = document.getElementById('loyaltyPoints');

            let points = (data.amountOfPoints == null) ? 0 : data.amountOfPoints;

            // Assuming the API returns an object with an amountOfPoints property
            pointsContainer.textContent = `You have ${points} points.`;
        });

    // Fetch reservations and display them in the table
    fetch('/api/reservations', {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        },
    })
        .then(response => response.json())
        .then(reservations => {
            const tableBody = document.getElementById('reservationsTable').querySelector('tbody');
            reservations.forEach(reservation => {
                fetch(`/api/movieShows/${reservation.movieShowId}`)
                    .then(response => response.json())
                    .then(movieShow => {
                        fetch(`/api/movies/${movieShow.id}`)
                            .then(response => response.json())
                            .then(movie => {
                                fetch(`/api/rooms/${movieShow.roomId}`).then(response => response.json())
                                    .then(room => {
                                        const row = tableBody.insertRow();
                                        row.innerHTML = `
                                        <td>${movie.title}</td>
                                        <td>${new Date(movieShow.date).toLocaleDateString()}</td>
                                        <td>${movieShow.hour}</td>
                                        <td>Row: ${reservation.seatRow} <br> Column: ${reservation.seatColumn}</td>
                                        <td>${room.roomNo}</td>
                                        <td>${reservation.status}</td>`;
                                    })
                            })
                    });
            })

            
        })
});