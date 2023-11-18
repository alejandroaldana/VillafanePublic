// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function mostrarSpinner() {
    $("body").prepend("<div class='overlay-spinner'><div class='spinner'></div><span></span></div>");
}
function ocultarSpinner() {
    $(".overlay-spinner").remove();
}