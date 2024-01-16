document.addEventListener('DOMContentLoaded', function () {
    let movieId = new URLSearchParams(document.location.search).get('movieShowId');
    fetch(`/api/movieShows/${movieId}/getSeats`, {
        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log(data)
            console.log(data.reservedSeats)
            const tableHead = document.getElementById('roomSeats').querySelector('thead');
            const tableBody = document.getElementById('roomSeats').querySelector('tbody');
            const movieTitle = document.getElementById('movieTitle');
            const movieTime = document.getElementById('movieTime');

            let movieDate = new Date(data.movieDate);
            var year = movieDate.getFullYear();
            var month = (movieDate.getMonth() + 1).toString().padStart(2, '0'); 
            var day = movieDate.getDate().toString().padStart(2, '0');

            movieTitle.textContent = data.title;
            movieTime.textContent = `${day}-${month}-${year} ${data.movieHour}`;


            const rows = data.rows;
            const seatInRow = data.seatInRow;
            const reservedSeats = data.reservedSeats;

            let screenTr = document.createElement('tr');
            let screen = document.createElement('td');
            screen.colSpan = seatInRow;
            screen.className = "screen";
            screen.textContent = "SCREEN";
            screenTr.appendChild(screen);
            tableHead.appendChild(screenTr);

            let gapTr = document.createElement('tr');
            let gap = document.createElement('td');
            gap.colSpan = seatInRow;
            gap.className = "screenSeatGap";
            gapTr.appendChild(gap);
            tableHead.appendChild(gapTr);

            for (let i = 1; i <= rows; i++) {
                let row = document.createElement('tr');

                for (let j = 1; j <= seatInRow; j++) {
                    let seatItem = document.createElement('td');
                    if (i in reservedSeats && reservedSeats[i].includes(j))
                        seatItem.classList.add('reserved');
                    else
                        seatItem.classList.add('free');  

                    seatItem.innerHTML = `Row: ${i} <br> seat: ${j}`;
                    seatItem.id = `${i}-${j}`;

                    row.appendChild(seatItem);
                }
                tableBody.appendChild(row);
            }
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
});

document.getElementById('roomSeats').addEventListener("click", function (event) {
    let item = document.getElementById(event.target.id);
    let counter = document.getElementById('noOfSeats');
    if (item.className == 'reserved' || item.className == 'screen' || item.className == 'screenSeatGap')
        return false;

    if (item.className == 'free')
        item.className = 'selected';
    else if (item.className == 'selected')
        item.className = 'free';

    let reservedSeats = document.getElementsByClassName('selected').length;
    counter.textContent = `Selected seats: ${reservedSeats}`;
})

document.getElementById('btnSubmit').addEventListener('click', function (event) {
    let selectedSeats = document.getElementsByClassName('selected');
    let seatsToReserve = []

    for (let i = 0; i < selectedSeats.length; i++) {
        seatsToReserve.push(selectedSeats[i].id);
    }

    let movieShowId = new URLSearchParams(document.location.search).get('movieShowId');
    fetch(`/api/movieShows/${movieShowId}/getSeats`, {
        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            let reservedSeats = data.reservedSeats;
            const rows = data.rows;
            const seatInRow = data.seatInRow;
            let reservedSeatsToString = []

            for (let i = 1; i <= rows; i++) 
                for (let j = 1; j <= seatInRow; j++) 
                    if (i in reservedSeats && reservedSeats[i].includes(j))
                        reservedSeatsToString.push(`${i}-${j}`);


            let conflictsSeats = reservedSeatsToString.filter(v => seatsToReserve.includes(v));
            if (conflictsSeats > 0) {
                alert(`Seats: ${conflictsSeats} are alerady taken!`);
                return false
            }

            let reservationIds = [];
            localStorage.removeItem('reservation_ids');
            for (let seatItem of seatsToReserve) {
                var [row, seat] = seatItem.split('-');

                reserveSeat(row, seat, movieShowId);
            }
            alert("Reserved!");
            window.location.href = '/ticketConfirm.html'
        })
        .catch(error => {
            console.error('Error:', error);
            // Optionally, update the UI to show an error message
        });
})

function reserveSeat(row, column, movieShowId) {
    fetch('/api/reservations', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            "movieShowId": movieShowId,
            "seatRow": row,
            "seatColumn": column
        }),
    })
        .then(response => {
            if (response.status == 401) {
                alert("You're sesion has expired!");
                return false;
            }
            if (!response.ok) {
                console.log(response);
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            let ids = []
            let localId = localStorage.getItem('reservation_ids');
            if (localId != null)
                ids.push(localId);
            ids.push(data.id);
            localStorage.setItem('reservation_ids', ids);
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

    
   