//const uri = 'http://localhost:5000';

//document.addEventListener('DOMContentLoaded', function () {
/*
    fetch(uri + "/movie/infolist")

        .then(response => response.json())
        .then(data => {

            //alert("movie data: " + JSON.stringify(data));
            //alert(data.data[0].title);

            var select = document.getElementById('movielist');

            for (var i = 0; i < data.data.length; i++) {
                var opt = document.createElement('option');
                opt.value = i;
                opt.innerHTML = data.data[i].title;
                select.appendChild(opt);
            }

        })
        .catch(error => console.error('Unable to get movies.', error));
*/

/*
    document.getElementById("contactID").onclick = (event) => {

        alert("Call backend!");


        fetch(uri + "/user/485b76f2-c7cb-46ff-83e3-34ac33c90c76")
        //fetch(uri + "")

            .then(response => response.json())
            .then(data => {

                //alert("user data: " + JSON.stringify(data));
                document.getElementById("contactID").textContent = JSON.stringify(data);

            })
            .catch(error => console.error('Unable to get user data.', error));


        //var selectList = document.getElementById("movielist");
        //var movieTitle = selectList.options[selectList.selectedIndex].text;
        //document.getElementById("videoPlayer").src = uri +"/movie/" + movieTitle;
    }*/

//});