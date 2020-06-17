



// Script for shrinking logo
window.onscroll = function () { shrinkFunction() };

function shrinkFunction() {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.getElementById("logo").style.height = "40px";
        document.getElementById("menu").style.padding = " 1px 20px";
    } else {
        document.getElementById("logo").style.height = "90px";
        document.getElementById("menu").style.padding = "20px";
    }
}

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


//============================================================================  Script for ~/Photo/Index page  =====================================================

// This script handles displaying and animating the photo modal 

$('#photo-modal').on('click', function () {
    console.log("modal clicked");
    $('#photo-modal').toggleClass('photo-modal--animation');
    $('body').css('overflow', 'auto')
});
function ShowModel(id) {
    $('#photo-modal').toggleClass('photo-modal--animation');
    $("body").css('overflow', 'hidden') // removes scrollbar when modal is open
    document.getElementById("photo-modal").style.display = "flex";
    document.getElementById("photo-modal--content").src = document.getElementById("photo-index-img-" + id).src;
    document.getElementById("photo-modal").onclick = function () {
        $('.photo-modal').fadeToggle(600);
    }
}
//End script for ~/Photo/Index modal


// This script handles the dynamic scrolling feature of the Photo/Index page.
// A set of photos is retrieved from the database, then when the vertical scrollbar reaches the bottom,
// a new set of photos is retrieved.

if (document.getElementById("scroll-container") != null) {
    // ajaxCompleted is used to ensure that the getPhotos function is only called once per ping to the server.
    var ajaxCompleted = true;
    $(document).ready(PhotoScroll());

    function PhotoScroll() {
        var pageIndex = 0;
        // The pageSize variable can be changed to alter the number of retrieved items
        var pageSize = 20;         

        // this block calls the getData function when the vertical scrollbar reaches the bottom,
        // and loads the next set of photos.
        $(document).ready(function () {   
            getPhotos(pageIndex, pageSize);
            pageIndex++;
            $(window).scroll(function () { 
                // window scrolltop is rounded up with math.ceil() because it was returning inconsistent values. That's also why it's set to >= instead of ==
                if (Math.ceil($(window).scrollTop()) >=  
                    $(document).height() - $(window).height() && ajaxCompleted) {
                    getPhotos(pageIndex, pageSize);
                    pageIndex++;
                }
            });
        });
    };

    // This function makes an AJAX call to the GetPhotos action method, sending pageIndex and pageSize as arguments.
    // The next set of photos is retrieved from the database and returned. When addPhotoRows is called, the photos are added to the Index page.
    function getPhotos(pageIndex, pageSize) {
        console.log("index: " + pageIndex + " pagesize: " + pageSize + " photos.length: "/* + photos.length*/);
        ajaxCompleted = false;
        $.ajax({
            type: 'GET',
            url: '/Photo/GetPhotos',
            data: { "pageIndex": pageIndex, "pageSize": pageSize },
            dataType: 'json',
            success: function (photos) {
                console.log("%index: " + pageIndex);
                addPhotoRows(photos);
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
    };

    function addPhotoRows(photos) {
        if (photos != "[]") {
            photos = jQuery.parseJSON(photos);
            for (var i = 0; i < photos.length; i++) {
                $("table").append("<tr class='tr-styling scroll--container'>" +
                    // This td is for the photo
                    "<td class='td-styling'> <img id='photo-index-img-" +
                        photos[i].PhotoId + "' onclick='ShowModel(" +
                        photos[i].PhotoId + ")' class='thumbnail_size photo-index-img' src='/photo/displayphoto/" +
                        photos[i].PhotoId + "' }) /></td>" +
                    "<td class='td-styling'>" + photos[i].OriginalHeight + "</td>" +
                    "<td class='td-styling'>" + photos[i].OriginalWidth + "</td>" +
                    "<td class='td-styling'>" + photos[i].Title + "</td>" +
                    "<td class='td-styling'>" +
                    "<a href = '/photo/Edit/" + photos[i].PhotoId + "'>Edit | </a>" +
                    "<a href = '/photo/Details/" + photos[i].PhotoId + "'>Details | </a>" +
                    "<a href = '/photo/Delete/" + photos[i].PhotoId + "'>Delete</a>" +
                    "</td>" +
                    "</tr>")
            };
            ajaxCompleted = true;
        };
    };
};
// End infinite scrolling for Photo/Index page

// =============================================================================== End Script for Photo/Index page =============================================================

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

