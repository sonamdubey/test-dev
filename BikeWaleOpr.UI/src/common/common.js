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