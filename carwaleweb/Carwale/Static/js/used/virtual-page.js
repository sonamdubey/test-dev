window.addEventListener('DOMContentLoaded', function () {
    if (screen.width < 1024) {
        var scrollSpy = new ScrollSpy('#pageContainer');
    }
    else {
        var scrollSpy = new ScrollSpy('#pageContainer', { navigationTabContainer: '.navigation-menu-container' });
    }
    virtualPage.registerEvents();
})

var virtualPage = (function() {
    var registerEvents = function () {

        document.querySelector('#viewAllCars')
        .addEventListener('click', function () {
            document.querySelectorAll('.card-list-container.hide')
                    .forEach(function (cardContainer) {
                        cardContainer.classList.remove('hide');
                    });
            this.classList.add('hide');
        });
        document.querySelector('#referrerLink')
            .addEventListener('click', function () {
                history.back();
            });
    }
    return { registerEvents: registerEvents };
})();