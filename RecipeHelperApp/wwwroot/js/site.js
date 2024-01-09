// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
        var currentSection = 1;

    // Function to show the next section
    $(".next-section").click(function () {
        $("#section" + currentSection).css("opacity", "0");
    setTimeout(function () {
        $("#section" + currentSection).css("display", "none");
    currentSection++;
        $("#section" + currentSection).css("opacity", "1").css("display", "block");
            }, 500); // 500ms is the transition duration
        });

    // Function to show the previous section
    $(".prev-section").click(function () {
        $("#section" + currentSection).css("opacity", "0");
    setTimeout(function () {
        $("#section" + currentSection).css("display", "none");
    currentSection--;
        $("#section" + currentSection).css("opacity", "1").css("display", "block");
            }, 500); // 500ms is the transition duration
        });
    });