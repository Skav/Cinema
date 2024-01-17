document.addEventListener('DOMContentLoaded', function () {
    let reservationList = localStorage.getItem('reservation_ids').split(',');

    reservationList.forEach(ticket => {
        getTicket(ticket);
    })
    
})

function getTicket(id) {
    fetch(`/api/reservations/${id}`, {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const tableBody = document.getElementById('ticketsTable').querySelector('tbody');
            let tr = document.createElement('tr');
            let id = document.createElement('td');
            let movieTitle = document.createElement('td'); 
            let roomNo = document.createElement('td');
            let date = document.createElement('td');
            let seatNo = document.createElement('td');

            seatNo.innerHTML = `Row: ${data.seatRow} <br> Column: ${data.seatColumn}`;
            id.textContent = data.id;

            fetch(`/api/movieShows/${data.movieShowId}`).then(response => response.json())
                .then(movieShow => {
                    let movieDate = new Date(movieShow.date);
                    let year = movieDate.getFullYear();
                    let month = (movieDate.getMonth() + 1).toString().padStart(2, '0');
                    let day = movieDate.getDate().toString().padStart(2, '0');

                    date.textContent = `${day}-${month}-${year} ${movieShow.hour}`;

                    fetch(`/api/movies/${movieShow.movieId}`).then(response => response.json())
                        .then(movie => {
                            movieTitle.textContent = movie.title;
                        });

                    fetch(`/api/rooms/${movieShow.roomId}`).then(response => response.json())
                        .then(room => {
                            roomNo.textContent = room.roomNo;
                        })

                        
                })

            tr.appendChild(id);
            tr.appendChild(movieTitle);
            tr.appendChild(roomNo);
            tr.appendChild(seatNo);
            tr.appendChild(date);      
            tableBody.appendChild(tr);
            
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
}

document.getElementById('btnAccept').addEventListener('click', async function () {
    let reservationList = localStorage.getItem('reservation_ids').split(',');

    for (const ticket of reservationList) {
        await confirm(ticket);
    }

    localStorage.removeItem('reservation_ids');

    alert("Confirmed reservation!");
    window.location.href = '/index.html'
})
document.getElementById('btnCancel').addEventListener('click', async function () {
    let reservationList = localStorage.getItem('reservation_ids').split(',');

    for (const ticket of reservationList) {  
        await cancel(ticket);
    }

    localStorage.removeItem('reservation_ids');
    alert("Canceled reservation!");
    window.location.href = '/index.html'
})


async function confirm(id) {
    return fetch(`/api/reservations/${id}/confirmReservation`, {
        method: "PUT",
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
}

async function cancel(id) {
    return fetch(`/api/reservations/${id}/cancelReservation`, {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
}
