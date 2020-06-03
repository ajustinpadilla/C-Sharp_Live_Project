



// Script for shrinking logo
window.onscroll = function () { shrinkFunction() };

//Simple welcome message that prints to the console on App Start
console.log("Welcome to the theatre!");

//Script for Landing Page
if (document.getElementById("main-carousel") != null) {
    var slideIndex = 1;

    showSlides(slideIndex);

    function changeSlide(n) {
        showSlides(slideIndex += n);
    }

    function currentSlide(n) {
        showSlides(slideIndex = n);
    }

    function showSlides(n) {
        var i;
        var slides = document.getElementsByClassName("slides");
        var dots = document.getElementsByClassName("dot");
        if (n > slides.length) { slideIndex = 1 }
        if (n < 1) { slideIndex = slides.length }
        for (i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }
        for (i = 0; i < dots.length; i++) {
            dots[i].className = dots[i].className.replace(" active", "");
        }
        slides[slideIndex - 1].style.display = "block";
        dots[slideIndex - 1].className += " active";
    }
};

//Script for ~/Photo/Index modal

function ShowModel(id) {


    var modal = document.getElementById("photo-modal-" + id);

    var img = document.getElementById("photo-index-img-" + id);
    var modalImg = document.getElementById("photo-modal--content-" + id);

    
    modal.style.display = "block";
    modalImg.src = img.src;
   
    var span = document.getElementById("photo-modal--close-" + id);
    span.onclick = function () {
        modal.style.display = "none";
    }
}



//End script for ~/Photo/Index modal


////Script for sticky navbar
//window.onscroll = function () { stickyNav() };
//var menu = document.getElementById("menu");
//var sticky = menu.offsetTop;

//function stickyNav() {
//    if (window.pageYOffset >= sticky) {
//        menu.classList.add("sticky")
//    } else {
//        menu.classList.remove("sticky");
//    }
//}


function shrinkFunction() {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.getElementById("logo").style.height = "40px";
        document.getElementById("menu").style.padding = " 1px 20px";
    } else {
        document.getElementById("logo").style.height = "90px";
        document.getElementById("menu").style.padding = "20px";
    }
}

// Begin script for Bulk Add

if (document.getElementById("generate-showtimes-section") != null) {
    $("#showtimes-container").hide();

    $("#generate__production-field").change(function () {
        var productionId = $("#generate__production-field").val();
        $.ajax({
            method: 'GET',
            url: '/CalendarEvents/GetDates',
            data: { "productionId": productionId },
            dataType: 'json',
            success: function (data) {
                if (data != "[]") {
                    let production = jQuery.parseJSON(data);
                    let openingDay = production[0].OpeningDay.substr(0, 10); // Removes the time from the date
                    let closingDay = production[0].ClosingDay.substr(0, 10);
                    $("#generate__start-date-field").val(openingDay); //code for adding a date to the start date field
                    $("#generate__end-date-field").val(closingDay);
                    $("#matinee-time").html(moment(production[0].ShowtimeMat).format('h:mm a'));
                    $("#evening-time").html(moment(production[0].ShowtimeEve).format('h:mm a'));
                }
            },
            //beforeSend: function () {
            //    $("#progress").show();
            //},
            //complete: function () {
            //    $("#progress").hide();
            //},
            error: function () {
                alert("Error while retrieving data!");
            }
        });
    });


    // 

    $("#generate-button").click(function () {

        var modal = $('#bulk-add-modal'),
            btnYes = document.getElementById('bulk-add-modal_yes'),
            btnNo = document.getElementById('bulk-add-modal_no')

        modal.show();


        let production = $("#generate__production-field").children("option").filter(":selected").text(),
            startDate = moment($("#generate__start-date-field").val()),
            endDate = moment($("#generate__end-date-field").val()),
            eventDate = startDate,
            dateRange = endDate.diff(startDate, 'days'),
            interval = $("#interval").children("option").filter(":selected").val(),
            eventList = [],
            startTimes = [];
                if ($('#matinee').is(':checked')) {
                    startTimes.push($('#matinee-time').text());
                }
                if ($('#evening').is(':checked')) {
                    startTimes.push($('#evening-time').text());
                }
                if ($('#custom-time').val() != "") {
                    startTimes.push($('#custom-time').val());
                }

        let productionDays = [];
            if ($('#sunday').is(':checked')) {
                productionDays.push(0);
            }
            if ($('#monday').is(':checked')) {
                productionDays.push(1);
            }
            if ($('#tuesday').is(':checked')) {
                productionDays.push(2);
            }
            if ($('#wednesday').is(':checked')) {
                productionDays.push(3);
            }
            if ($('#thursday').is(':checked')) {
                productionDays.push(4);
            }
            if ($('#friday').is(':checked')) {
                productionDays.push(5);
            }
            if ($('#saturday').is(':checked')) {
                productionDays.push(6);
            }

        class CalendarEvent {
            constructor(production, date, dayOfWeek, startTime) {
                this.production = production;
                this.date = date;
                this.dayOfWeek = dayOfWeek;
                this.startTime = startTime;
                this.dateString = date;
            }
        }

        for (i = 0; i < productionDays.length; i++) {
            if (productionDays[i] < startDate.day()) {
                productionDays[i] += 7;
            }
            console.log(productionDays[i])
            startDate.day(productionDays[i]);
            eventDate = startDate;
            for (j = productionDays[i]; j <= dateRange + 7; j += 7 * interval) {
                if (eventDate.isBetween(startDate, endDate, undefined, '[]')) { //check for the eventDate to be within start and end date. The '[]' argument sets it to be inclusive of the start and end date.
                    for (k = 0; k < startTimes.length; k++) {
                        console.log(startTimes[k])
                        console.log(eventDate.format('ll'));
                        const event = new CalendarEvent(production, moment(eventDate), eventDate.format('dddd'), startTimes[k], eventDate.format('ll'));
                        eventList.push(event);
                        
                    }
                }
                eventDate.add((7 * interval).toString(), 'days').format('ll');
            }
            startDate = moment($("#generate__start-date-field").val());
            eventDate = startDate;
        }
        eventList.sort((a, b) => a.date - b.date);
        console.log(eventList);

        // end block


        // This block handles rendering the showtimes table
        $("#showtimes-container").show();
        var columns = 3,
            count = 0,
            table = document.createElement("table"),
            row = table.insertRow();

        for (i = 0; i < eventList.length; i++) {
            var cell = row.insertCell();
            cell.innerHTML = eventList[i].date.format('ll');
            count++;
            cell = row.insertCell();
            cell.innerHTML = eventList[i].dayOfWeek;
            count++;
            cell = row.insertCell();
            cell.innerHTML = eventList[i].startTime;
            count++;
            if (count % columns == 0) {
                row = table.insertRow();
            }
        }

        document.getElementById('showtimes-container').appendChild(table);
        //$("#showtimes-container").text();
    });
}
    //End script for Bulk Add