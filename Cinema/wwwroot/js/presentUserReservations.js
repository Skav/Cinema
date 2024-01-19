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
                        fetch(`/api/movies/${movieShow.movieId}`)
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

    fetch(`/api/reviews/user`, {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const reviewContainer = document.getElementById('reviewsContainer');
            data.forEach(review => {
                let reviewBox = document.createElement('div');
                reviewBox.id = `review-${review.id}`;
                reviewBox.classList.add('reviewItem')


                let title = document.createElement('p');
                title.innerHTML = `Movie title: ${review.title}`;

                let rating = document.createElement('p');
                rating.textContent = `${review.rating}/10`;

                let content = document.createElement('p');
                content.textContent = review.content;

                let removeButton = document.createElement('input');
                removeButton.id = 'reviewRemoveButton';
                removeButton.value = 'Remove';
                removeButton.className = 'btn-remove';
                removeButton.addEventListener('click', removeReview);

                reviewBox.appendChild(title);
                reviewBox.appendChild(rating);
                reviewBox.appendChild(content);
                reviewBox.appendChild(removeButton);

                reviewContainer.appendChild(reviewBox);
            });
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });

    
});

function removeReview(event) {
    

    fetch(`/api/reviews/${event.target.parentNode.id.split('-')[1]}/delete`, {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        },
    })
        .then(response => {
            if (response.status == 200) {
                alert("Removed!")
                window.location.reload();
            }
            else {
                alert("Something goes wrong");
                console.log(response);
            }
        })
    
}