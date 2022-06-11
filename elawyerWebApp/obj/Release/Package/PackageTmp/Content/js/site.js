// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("mySidenav").style.borderRightWidth = "3px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("mySidenav").style.borderRightWidth = "0";
}

function loading() {
    document.getElementById("loginForm").classList.add = "visually-hidden";
    document.getElementById("spinner").classList.remove = "visually-hidden";
}