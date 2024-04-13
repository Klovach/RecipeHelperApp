﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    var currentSection = 1;
    var isAnimating = false;

    // Function to show the next section
    $(".next-section").click(function () {
        if (!isAnimating) {
            isAnimating = true;
            $("#section" + currentSection).css("opacity", "0");
            setTimeout(function () {
                $("#section" + currentSection).css("display", "none");
                currentSection++;
                if (currentSection < 2) {
                    currentSection = 2;
                }
                $("#section" + currentSection).css("opacity", "1").css("display", "block");
                isAnimating = false;
            }, 500);
        }
    });

    // Function to show the previous section
    $(".prev-section").click(function () {
        if (!isAnimating) {
            isAnimating = true;
            $("#section" + currentSection).css("opacity", "0");
            setTimeout(function () {
                $("#section" + currentSection).css("display", "none");
                if (currentSection === 1) {
                    currentSection = 1;
                } else {
                    currentSection--;
                }
                $("#section" + currentSection).css("opacity", "1").css("display", "block");
                isAnimating = false;
            }, 500);
        }
    });
});




document.addEventListener('DOMContentLoaded', function () {
    // This code executes when the DOM content is loaded
    document.getElementById('loader').style.display = 'block';
    document.getElementById('page-contents').style.display = 'none';

    // Form submit event listener
    document.querySelector('form').addEventListener('submit', function (event) {
      
        document.getElementById('submit-button-group').style.display = 'none';
        document.getElementById('loading-button-group').style.display = 'block';
        // Perform form submission or other actions here
    });
});

window.onload = function () {
    // This code executes when all the resources have finished loading
    document.getElementById('loader').style.display = 'none';
    document.getElementById('page-contents').style.display = 'block';
    document.getElementById('submit-button-group').style.display = 'block';
    document.getElementById('loading-button-group').style.display = 'none';
};

