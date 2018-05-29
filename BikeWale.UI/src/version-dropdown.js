$('#version').on('click', function () {
    $(".version-dropdown__submenu, .version-dropdown__tabs").toggleClass("version-active");
    $('html, body').animate({ scrollTop: $('#version').offset().top - 44 }, 'slow');
});