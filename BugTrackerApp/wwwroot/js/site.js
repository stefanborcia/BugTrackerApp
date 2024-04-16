// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", function () {
    var cookiePopup = document.getElementById("cookiePopup");
    var closeBtn = document.getElementById("closeCookiesBtn");

    if (localStorage.getItem('acceptedCookies') === 'true') {
        cookiePopup.style.display = "none";
        return;
    }

    closeBtn.addEventListener("click", function () {
        cookiePopup.style.display = "none";
        setAcceptedCookies(true);
    });

    if (cookiePopup.style.display === "none") {
        return;
    }

    window.addEventListener("beforeunload", function (event) {
        if (localStorage.getItem('acceptedCookies') !== 'true') {
            localStorage.setItem('previouslyVisited', 'true');
        }
    });

    if (localStorage.getItem('previouslyVisited') === 'true') {
        wixWindow.openLightbox("Cookies");
        localStorage.removeItem('previouslyVisited');
    }
});

function setAcceptedCookies(value) {
    localStorage.setItem('acceptedCookies', value);
    sendCookieAcceptanceToServer();
}
