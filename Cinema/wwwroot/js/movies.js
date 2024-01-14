document.addEventListener('DOMContentLoaded', function () {
    fetch('/api/movies')
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
                row.insertCell().textContent = movie.title;
                row.insertCell().textContent = movie.duration;
                row.insertCell().textContent = movie.genre;
            });
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
});