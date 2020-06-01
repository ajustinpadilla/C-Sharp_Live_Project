



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

    class CalendarEvent {
        constructor(production, date, dayOfWeek, startTime) {
            this.production = production;
            this.date = date;
            this.dayOfWeek = dayOfWeek;
            this.startTime = startTime;
        }



    }
    $("#generate-button").click(function () {
        $("#showtimes-container").text();
        let production = $("#generate__production-field").children("option").filter(":selected").text();
        let startDate = moment($("#generate__start-date-field").val());
        let endDate = moment($("#generate__end-date-field").val());
        let dateRange = endDate.diff(startDate, 'days');
        let interval = $("#interval").children("option").filter(":selected").val();
        let startTimes = [];
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
            if ($('#monday').is(':checked')) {
                productionDays.push('monday');
            }
            if ($('#tuesday').is(':checked')) {
                productionDays.push('tuesday');
            }
            if ($('#wednesday').is(':checked')) {
                productionDays.push('wednesday');
            }
            if ($('#thursday').is(':checked')) {
                productionDays.push('thursday');
            }
            if ($('#friday').is(':checked')) {
                productionDays.push('friday');
            }
            if ($('#saturday').is(':checked')) {
                productionDays.push('saturday');
            }
            if ($('#sunday').is(':checked')) {
                productionDays.push('sunday');
        }

            console.log(startDate.format('ll'))
        for (i = startDate.day(); i < dateRange; i += 7) {
            console.log(startDate.add('7', 'days').format('ll'))
            
        }
    });

}









    //End script for Bulk Add