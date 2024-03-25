// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    var currentSection = 1;

    // Function to show the next section
    $(".next-section").click(function () {
        $("#section" + currentSection).css("opacity", "0");
    setTimeout(function () {
        $("#section" + currentSection).css("display", "none");

        currentSection++;

        if (currentSection < 2) {
            currentSection = 2;
        }
       
        $("#section" + currentSection).css("opacity", "1").css("display", "block");
            }, 500); 
        });

    // Function to show the previous section
    $(".prev-section").click(function () {
        $("#section" + currentSection).css("opacity", "0");
    setTimeout(function () {
        $("#section" + currentSection).css("display", "none");
        
        if (currentSection = 1) {
            currentSection = 1;
        }
        else {
            currenSetcion--; 
        }
        $("#section" + currentSection).css("opacity", "1").css("display", "block");
            }, 500); 
        });
    });