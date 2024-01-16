document.addEventListener('DOMContentLoaded', function () {
    // Fetch loyalty points and display them
    fetch('/api/LoyalityPointy')
        .then(response => response.json())
        .then(data => {
            const pointsContainer = document.getElementById('loyaltyPoints');
            // Assuming the API returns an object with an amountOfPoints property
            pointsContainer.textContent = `You have ${data.amountOfPoints} points.`;
        });

    // Fetch reservations and display them in the table
    fetch('/api/reservations')
        .then(response => response.json())
        .then(reservations => {
            const tableBody = document.getElementById('reservationsTable').querySelector('tbody');
            reservations.forEach(reservation => {
                Promise.all([
                    fetch(`/api/movieShows/${reservation.movieShowId}`).then(response => response.json()),
                    fetch(`/api/movies/${reservation.movieId}`).then(response => response.json())
                ]).then(([movieShow, movie]) => {
                    const row = tableBody.insertRow();
                    row.innerHTML = `
                        <td>${movie.title}</td>
                        <td>${new Date(movieShow.date).toLocaleDateString()}</td>
                        <td>${movieShow.hour}</td>
                        <td>${reservation.seatRow}</td>
                        <td>${reservation.seatColumn}</td>
                    `;
                });
            });
        });
});