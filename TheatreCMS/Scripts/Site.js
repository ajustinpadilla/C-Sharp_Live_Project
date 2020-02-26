//Simple welcome message that prints to the console on App Start
console.log("Welcome to the theatre!");

//Script for Landing Page
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

//Begin script to fix overlapping navbar
//var menu = document.querySelectorAll('.nav-link dropdown-toggle');
//if (menu.checked = true) {
//    menu.addEventListener('hover', function removeBar() {
//        $('.dropdown-toggle').dropdown();
//    })
//}
//else {
//     menu.addEventListener('click', function removeBar () {
//        $(.dropdown-toggle').dropdown();
//        })
//}
$('.dropdown-toggle').on('mouseenter mouseleave click tap', function () {
    $(this).toggleClass("collapse");
    console.log('triggered')
});

// Script for shrinking logo
window.onscroll = function () { shrinkFunction() };

function shrinkFunction() {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.getElementById("logo").style.height = "30px";
    } else {
        document.getElementById("logo").style.height = "90px";
    }
}
