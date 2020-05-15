



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
    console.log("sdf");

    $("#generate__production-field").click(function () {
        var productionId = $("#generate__production-field").val();
        console.log(productionId);

        $.ajax({
            method: 'GET',
            url: '/CalendarEvents/GetDates',
            data: productionId,
            dataType: 'json',
            success: function (data) {
                
                if (data != "[]") {
                    var production = jQuery.parseJSON(data);
                    $("#generate__start-date-field").val("1212-12-12"); //code for adding a date to the start date field
                    $("#generate__end_date_field").val("3434-34-34");

                    ajaxCompleted = true;
                }
            },
            beforeSend: function () {
                $("#progress").show();
            },
            complete: function () {
                $("#progress").hide();
            },
            error: function () {
                alert("Error while retrieving data!");
            }
        });







    });
    
}









    //End script for Bulk Add