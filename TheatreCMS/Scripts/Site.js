﻿



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

// Infinite scrolling for Photo/Index page

$(document).ready(PhotoScroll());
function PhotoScroll() {
    var pageIndex = 0;
    var pageSize = 10;

    $(document).ready(function () {
        GetData(pageIndex, pageSize);
        pageIndex++;
        console.log("1st get data")

        $(window).scroll(function () {
            if ($(window).scrollTop() ==
                $(document).height() - $(window).height()) {
                GetData(pageIndex, pageSize);
                pageIndex++;
                console.log("2nd get data")

            }
        });
    });
}

function GetData(pageIndex, pageSize) {
    $.ajax({
        type: 'GET',
        url: '/Photo/GetPhotos',
        data: { "pageindex": pageIndex, "pagesize": pageSize },
        dataType: 'json',
        success: function (data) {
            if (data != null) {
                var photos = jQuery.parseJSON(data);
                for (var i = 0; i < photos.length; i++) {
                    $("table").append("<tr class='tr-styling scroll--container'>" +
                                        "<td class='td-styling'> <img class='thumbnail_size' src='/photo/displayphoto/" + photos[i].PhotoId + "' }) /></td>" +
                                        "<td class='td-styling'>" + photos[i].OriginalHeight + "</td>" +
                                        "<td class='td-styling'>" + photos[i].OriginalWidth + "</td>" +
                                        "<td class='td-styling'>" + photos[i].Title + "</td>" +
                                      "</tr>")
                }
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
}
