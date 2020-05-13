



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

if (document.getElementById("scroll-container") != null) {
    var ajaxCompleted = true; //this variable is used to ensure that the GetData function is only called once per server ping.
    $(document).ready(PhotoScroll());
    function PhotoScroll() {
        var pageIndex = 0;
        var pageSize = 20;         //this variable is used to set the number of retrieved items

        $(document).ready(function () {   //this block fires off the initial ajax call to populate the table
            GetData(pageIndex, pageSize);
            pageIndex++;
            console.log("1st get data")

            $(window).scroll(function () {  //this block sends out subsequent calls to append the table with more results when the scrollbar reaches the bottom.
                if (Math.ceil($(window).scrollTop()) >=  // window scrolltop is rounded up with math.ceil() because it was returning inconsistent values. That's also why it's set to >= instead of ==
                    $(document).height() - $(window).height() && ajaxCompleted) {
                    GetData(pageIndex, pageSize);
                    pageIndex++;

                }
            });
        });
    }

    function GetData(pageIndex, pageSize) {
        console.log("index: " + pageIndex + " pagesize: " + pageSize + " photos.length: "/* + photos.length*/);
        ajaxCompleted = false;
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
                                                    "<a href = '/photo/Edit/" + photos[i].PhotoId + "'>Edit | </a>" +
                                                    "<a href = '/photo/Details/" + photos[i].PhotoId + "'>Details | </a>" +
                                                    "<a href = '/photo/Delete/" + photos[i].PhotoId + "'>Delete</a>" +
                                                "</td>" +
                                          "</tr>")
                    }
                    ajaxCompleted = true;
                    if (photos.length == 0) {  // this prevents the function from being called when there are no photos left to display
                        ajaxCompleted = false;
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
}

// End infinite scrolling for Photo/Index page
