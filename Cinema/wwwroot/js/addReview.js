document.addEventListener('DOMContentLoaded', function () {
    let addReviewBox = document.getElementById('addReview');
    if (localStorage.getItem('isLoggedIn') == "false") {
        let needLogin = document.createElement('p');
        needLogin.textContent = "You must be logged in to add review";
        addReviewBox.appendChild(needLogin);
    }

    let movieId = new URLSearchParams(document.location.search).get('movieId');
     fetch(`/api/reviews/movie/${movieId}/user`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json',
        }
    })
        .then(response => response.json())
        .then(data => {

                if (Object.keys(data).length > 0) {
                    let alreadyExists = document.createElement('p');
                    alreadyExists.textContent = "You already added review for this film";
                    addReviewBox.appendChild(alreadyExists);
                    return false;
                }
            

            
            let rating = document.createElement('input');
            rating.type = 'number';
            rating.max = 10;
            rating.min = 0;
            rating.placeholder = 'Add rating (0-10)';
            rating.id = 'movieRating';
            

            let context = document.createElement('textarea');
            context.type = "text";
            context.id = 'movieContext';
            context.maxLength = "255";
            context.placeholder = 'Add review';

            let submitButton = document.createElement('input');
            submitButton.type = 'button';
            submitButton.className = 'btn-register';
            submitButton.value = 'Add';
            submitButton.id = 'submitButton';
            submitButton.addEventListener('click', addReview)
            
            addReviewBox.appendChild(rating);
            addReviewBox.appendChild(context);
            addReviewBox.appendChild(submitButton);
    });

                    
});

function addReview() {
    let rating = document.getElementById('movieRating');
    let movieContext = document.getElementById('movieContext');
    let addReviewBox = document.getElementById('addReview'); 
    let movieId = new URLSearchParams(document.location.search).get('movieId');
    
    Array.from(addReviewBox.getElementsByClassName('error')).forEach(item => {
        addReviewBox.removeChild(item);
    });

    

    console.log(rating.value);

    if (rating.value > 10 || rating.value < 0 || rating.value == null || rating.value == "") {
        let error = document.createElement('p');
        error.className = 'error';
        error.textContent = "Wrong rating value";
        addReviewBox.appendChild(error);
        return false;
    }

    if (movieContext.length > 255 || movieContext.value == "" || movieContext == null) {
        let error = document.createElement('p');
        error.className = 'error';
        error.textContent = "Wrong context value";
        addReviewBox.appendChild(error);
        return false;
    }

    fetch('/api/reviews', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            "movieId": movieId,
            "rating": rating.value,
            "content": movieContext.value,
        }),
    })
        .then(response => {
            if (response.status == 201) {
                let success = document.createElement('p');
                success.className = 'success';
                success.textContent = "Added!"

                addReviewBox.removeChild(movieContext);
                addReviewBox.removeChild(rating);
                addReviewBox.removeChild(document.getElementById('submitButton'));
                addReviewBox.appendChild(success);

                console.log("Added!");
            }
            else {
                let error = document.createElement('p');
                error.className = 'error';
                error.textContent = `An error occured ${response.statusText}`;
                addReviewBox.appendChild(error);
            }
        })
}