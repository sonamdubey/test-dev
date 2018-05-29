var list = $(".dropdown-toggle__submenu"),
    selectedValue = $(".dropdown-toggle__selected-value"),
    dropdownToggleMenu = $("#dropdown-toggle__main-menu");
dropdownToggleMenu.on('click', function () {
    $(".dropdown-toggle__submenu, .dropdown-toggle__tabs").toggleClass("dropdown-toggle--active");
    $('html, body').animate({ scrollTop: dropdownToggleMenu.offset().top - 44 }, 'slow');
});
list.children().on("click", function () {
    selectedValue.html($(this).find(".dropdown-toggle__feature").html());
    $(".dropdown-toggle__submenu, .dropdown-toggle__tabs").toggleClass("dropdown-toggle--active");
    $('html, body').animate({ scrollTop: dropdownToggleMenu.offset().top - 44 }, 'slow');
});