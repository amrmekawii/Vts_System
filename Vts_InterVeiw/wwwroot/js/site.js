// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var inactivityTimeout = 20000; // 5 minutes of inactivity
var logoutUrl = '/User/ShowDialog'; // URL for the logout action

var timeout;

function resetTimeout() {
    clearTimeout(timeout);
    timeout = setTimeout(logout, inactivityTimeout);
}

function logout() {
    // Perform the logout action
    window.location.href = logoutUrl;
}

$(document).on('mousemove keydown', resetTimeout);
resetTimeout();