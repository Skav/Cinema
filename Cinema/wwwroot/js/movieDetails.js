document.addEventListener('DOMContentLoaded', function () {
    let movieId = new URLSearchParams(document.location.search).get('movieId');
    fetch(`/api/movies/${movieId}`, {
        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            let title = document.createElement('h1');
            title.textContent = data.title;
            let description = document.createElement('p');
            description.textContent = data.description;

            document.getElementById('movieTitle').appendChild(title);
            document.getElementById('movieDescription').appendChild(description);
            document.getElementById('movieGenre').textContent = `Genre: ${data.genre}`;
            document.getElementById('movieDuration').textContent = `Duration: ${data.duration} min`;
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });

    fetch(`/api/movieShows/movie/${movieId}`, {
        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const tableBody = document.getElementById('movieShowsTable').querySelector('tbody');
            data.forEach(movieShow => {
                const row = tableBody.insertRow();
                let date = new Date(Date.parse(movieShow.date));
                let reserveButton = document.createElement('a');
                reserveButton.classList.add('reserveButton');
                reserveButton.href = `/reserve.html?movieShowId=${movieShow.id}`;
                reserveButton.textContent = "Reserve";

                row.insertCell().textContent = date.toDateString();
                row.insertCell().textContent = movieShow.hour;
                row.insertCell().appendChild(reserveButton);
            });
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });

    fetch(`/api/reviews/movie/${movieId}`, {
        method: "GET"
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
                reviewBox.classList.add('reviewItem')

                let user = document.createElement('p');
                user.textContent = review.userName;

                let rating = document.createElement('p');
                rating.textContent = `${review.rating}/10`;

                let title = document.createElement('p');
                title.textContent = review.content;

                reviewBox.appendChild(user);
                reviewBox.appendChild(rating);
                reviewBox.appendChild(title);

                reviewContainer.appendChild(reviewBox);
            });
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
});