var SelectDropdown = (function () {

    var list, selectedValue, submenu, dropdownToggleMenu,
            _activeClassElement = "select-box--active";

    function _setSelectors() {
        list = $(".select-box__submenu");
        submenu = $(".select-box__submenu-list");
        selectedValue = $(".select-box__selected-value");
        dropdownToggleMenu = $(".select-box__menu-tab");
    }

    function _menuClickEvent() {
        dropdownToggleMenu.on('click', function () {
            $(this).parent(".select-box__tabs").toggleClass(_activeClassElement);
            $(this).parent(".select-box__tabs").siblings(".select-box__submenu").toggleClass(_activeClassElement);
            $('html, body').animate({ scrollTop: $(this).parents('.select-box ').offset().top - 44 }, 'slow');
        });
    }

    function _menuListClickEvent() {
        submenu.on("click", function () {
            var exm = $(this).parent(".select-box__submenu").siblings(".select-box__tabs").find(".select-box__selected-value");
            $(this).parent(".select-box__submenu").siblings(".select-box__tabs").find(".select-box__selected-value").html($(this).find(".select-box__title-line").html());
            $(this).parent(".select-box__submenu").siblings(".select-box__tabs").toggleClass(_activeClassElement);
            $(this).parent(".select-box__submenu").toggleClass(_activeClassElement);
            $('html, body').animate({ scrollTop: $(this).parents('.select-box ').offset().top - 44 }, 'slow');
        });
    }

    function selectDropdownEvents() {
        _setSelectors();
        _menuClickEvent();
        _menuListClickEvent();
    }

    return {
        selectDropdownEvents: selectDropdownEvents
    }

})();
$(document).ready(function () {
    SelectDropdown.selectDropdownEvents();
})