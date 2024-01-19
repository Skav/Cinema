document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/movies', {
        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const tableBody = document.getElementById('moviesTable').querySelector('tbody');
            data.forEach(movie => {
                const row = tableBody.insertRow();
                let link = document.createElement('a');
                link.href = `/movie.html?movieId=${movie.id}`;
                link.textContent = movie.title;
                link.className = "movieLink"
                row.insertCell().appendChild(link);
                row.insertCell().textContent = movie.duration;
                row.insertCell().textContent = movie.genre;
            });
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
});