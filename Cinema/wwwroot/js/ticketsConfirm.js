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
            let movie = document.createElement('td'); 
            let date = document.createElement('td');

            console.log(data);

            id.textContent = data.id;
            movie.textContent = data.movieShowId;
            date.textContent = "v:";

            tr.appendChild(id);
            tr.appendChild(movie);
            tr.appendChild(date);
            tableBody.appendChild(tr);
            
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
}

document.getElementById('btnAccept').addEventListener('click', function () {
    let reservationList = localStorage.getItem('reservation_ids').split(',');

    reservationList.forEach(ticket => {
        confirm(ticket);
    })

    localStorage.removeItem('reservation_ids');

    alert("Confirmed reservation!");
    window.location.href = '/index.html'
})
document.getElementById('btnCancel').addEventListener('click', function () {
    let reservationList = localStorage.getItem('reservation_ids').split(',');

    reservationList.forEach(ticket => {
        cancel(ticket);
    })

    localStorage.removeItem('reservation_ids');
    alert("Canceled reservation!");
    window.location.href = '/index.html'
})


function confirm(id) {
    fetch(`/api/reservations/${id}/confirmReservation`, {
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

function cancel(id) {
    fetch(`/api/reservations/${id}/cancelReservation`, {
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