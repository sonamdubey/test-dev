var CustomDropDown = {
	closeOnDocClick: true,
    customDropDownDocReady: function () {
        CustomDropDown.setSelectors();
        CustomDropDown.registerEvents();
    },
    //Variables declared for selectors
    setSelectors: function () {
        CustomDropDown.customInputBoxHolder = '.js-selectcustom-input-box-holder';
        CustomDropDown.customContent = '.selectcustom-content';
        CustomDropDown.cusDropDownArrow = '.drop-arrow img';
        CustomDropDown.showCusDropDown = 'show-custom-dropdown';
        CustomDropDown.rotateArrow = 'rotate-arrow';
    },
    //All events for the selectors
    registerEvents: function () {
        $(CustomDropDown.customInputBoxHolder).on('click', function (event) {
            if (!$(this).next(CustomDropDown.customContent).is(':visible')) {
				CustomDropDown.showCustomDropdown(this);
				closeOnDocClick = false;
            }
            else {
                CustomDropDown.hideCustomDropdown(this);
            }
        });

		$(document).on('click', function (event) {
			//var toClose = event.currentTarget.className.search("js-selectcustom-input-box-holder") < 0;
			if ($(CustomDropDown.customContent).is(':visible') && closeOnDocClick ) {
                CustomDropDown.hideCustomDropdown(CustomDropDown.customInputBoxHolder);
			}
			closeOnDocClick = true;
        });
    },

    showCustomDropdown: function (dropdownField) {
        $(dropdownField).next(CustomDropDown.customContent).addClass(CustomDropDown.showCusDropDown);
        $(dropdownField).find(CustomDropDown.cusDropDownArrow).addClass(CustomDropDown.rotateArrow);
    },

    hideCustomDropdown: function (dropdownField) {
        $(dropdownField).next(CustomDropDown.customContent).removeClass(CustomDropDown.showCusDropDown);
        $(dropdownField).find(CustomDropDown.cusDropDownArrow).removeClass(CustomDropDown.rotateArrow);
    },
}

$(document).ready(function () {
    CustomDropDown.customDropDownDocReady();
});