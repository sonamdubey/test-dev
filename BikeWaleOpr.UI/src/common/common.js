function hasSpecialCharacters(strModel) {
    var isValid = true;
    if (/^[0-9a-z\-\s]+$/.test(strModel)) {
        isValid = false;
    }
    return isValid;
}

function removeHyphens(str) {
    str = str.replace(/^\-+|\-+$/g, '');
    return str;
}

function showHideMatchError(element, error) {
    if (error) {
        element.parent().find('.error-icon').removeClass('hide');
        element.parent().find('.bw-blackbg-tooltip').removeClass('hide');
        element.addClass('border-red')
    }
    else {
        element.parent().find('.error-icon').addClass('hide');
        element.parent().find('.bw-blackbg-tooltip').addClass('hide');
        element.removeClass('border-red');
    }
}
function showToast(msg) {
    $('.toast').text(msg).stop().fadeIn(400).delay(3000).fadeOut(400);
}
