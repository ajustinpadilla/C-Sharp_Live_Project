



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

var pageIndex = 0;
var pageSize = 2;
var getDataIsReady = true; //this variable is used to ensure that the GetData function is only called once per server ping.
$(document).ready(PhotoScroll());
function PhotoScroll() {

    //$(document).ready(function () {
    //    GetData();
    //    //pageIndex++;
    //    console.log("get data 1")

        $(window).scroll(function () {
            //console.log("1: " + $(window).scrollTop())
            //console.log("2: " + $(document).height())
            //console.log("3: " + $(window).height())
            if (Math.ceil($(window).scrollTop()) >=
                $(document).height() - $(window).height() && getDataIsReady) {
                GetData();
                pageIndex++;
                //pageIndex++;
                
            }
        });
    //});
}

function GetData() {
    console.log("index: " + pageIndex + " pagesize: " + pageSize + " photos.length: "/* + photos.length*/);
    getDataIsReady = false;
    $.ajax({
        type: 'GET',
        url: '/Photo/GetPhotos',
        data: { "pageIndex": pageIndex, "pageSize": pageSize },
        dataType: 'json',
        success: function (data) {
            console.log("%index: " + pageIndex);
            if (data != null) {
                var photos = jQuery.parseJSON(data);
                for (var i = 0; i < photos.length; i++) {
                    $("table").append("<tr class='tr-styling scroll--container'>" +
                                        "<td class='td-styling'> <img class='thumbnail_size' src='/photo/displayphoto/" + photos[i].PhotoId + "' }) /></td>" +
                                        "<td class='td-styling'>" + photos[i].OriginalHeight + "</td>" +
                                        "<td class='td-styling'>" + photos[i].OriginalWidth + "</td>" +
                                        "<td class='td-styling'>" + photos[i].Title + "</td>" +
                                        "<td class='td-styling'>" +
                                            "<a href = '/photo/Edit/" + photos[i].PhotoId + "'>Edit</a>" +
                                            "<a href = '/photo/Details/" + photos[i].PhotoId + "'>Details</a>" +
                                            "<a href = '/photo/Delete/" + photos[i].PhotoId + "'>Delete</a>" +
                                        "</td>" +
                                      "</tr>")
                }
                getDataIsReady = true;
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
