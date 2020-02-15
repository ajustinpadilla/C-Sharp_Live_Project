﻿//Simple welcome message that prints to the console on App Start
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

// Script for sticky navbar
window.onscroll = function () { stickyNav() };

// Get the navbar
var menu = document.getElementById("menu");

// Get the offset position of the navbar
var sticky = menu.offsetTop;

// Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
function stickyNav() {
    if (window.pageYOffset >= sticky) {
        menu.classList.add("sticky")
    } else {
        menu.classList.remove("sticky");
    }
}
