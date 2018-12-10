window.addEventListener('load', function () {
    var button = document.getElementsByClassName('js-button');
    var element = document.getElementById('jsFormContainer');
    var title = document.querySelector('.pq-box__title');
    var elementTop;
    var titleTopMargin;
    button[0].addEventListener('click', function () {
        titleTopMargin = parseInt(window.getComputedStyle(title).getPropertyValue('margin-top'));
        elementTop = element.getBoundingClientRect().top + (window.pageYOffset || document.documentElement.scrollTop) - titleTopMargin;
        scrollToTop(window, elementTop, 600);
    })
})