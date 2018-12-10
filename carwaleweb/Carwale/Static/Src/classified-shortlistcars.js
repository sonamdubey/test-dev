var classifiedShortlistCars = {
    countIncrement: 0,

    shortlistListing: function (node) {
        var element;
        if (!$(node).parent().hasClass('img-placer')) {
            $(node).toggleClass('shortlist-icon--active');
            var pId = $('#pg_seller_details').attr('profileid');
            element = $("a.slideShow[profileid='" + pId + "']").nextAll('span.shortlist');
        }
        else
            element = $(node);

        var classValue = element.attr('class');
        element.css("pointer-events", "none");
        var arr = classifiedShortlistCars.getShortlistOperation(element);
        element = arr[0];
        var operation = arr[1];
        if (operation == 1)
            classifiedShortlistCars.animateShortlistButton(element, classValue);
        classifiedShortlistCars.addOrRemoveShortlist(element, operation);
        classifiedShortlistCars.countShortlistCars();
        return false;
    },

    getShortlistOperation: function (element, operation) {
        var objClass = element.attr('class');
        var operation;
        if (objClass.search("shortlist-icon--active") > 0) {
            element.removeClass('shortlist-icon--active').addClass("shortlist-icon--inactive");
            operation = 0;
        } else
        {
            var ln = 0;
            var slCookie = JSON.parse($.cookie('SLCookie'));
            if (slCookie)
                ln = slCookie.length;
            if (ln < 6) {
                element.removeClass('shortlist-icon--inactive').addClass("shortlist-icon--active");
                operation = 1;
                classifiedShortlistCars.countIncrement++;
                $('.search-count').html(classifiedShortlistCars.countIncrement);
            }
            else {
                operation = 2;
            }
        }
        var arr = [element, operation];
        return arr;
    },

    animateShortlistButton: function (element,classValue) {
        if (element && classValue.search('.shortlist-icon--inactive') > 0) {
            if ($("#photoGallery").is(':visible'))
                element = $("#descriptions").nextAll('span.shortlistphotogallery');

            var listClone = element.clone();
            listClone.offset({ top: element.offset().top, left: element.offset().left });
            listClone.addClass('shortlist-cars');
            listClone.appendTo($('body'));
            listClone.animate({
                'top': $('.shortlistBtn').offset().top + 15,
                'left': $('.shortlistBtn').offset().left + 24,
                'width': 24,
                'height': 24
            }, 1000, 'easeInCubic');
            listClone.animate({ 'width': 0, 'height': 0 }, function () { $(this).detach() });
        }
    },

    addOrRemoveShortlist: function (element, operation) {
       switch (operation) {
            case 0:
                classifiedShortlistCars.removeFromShortlist($(element).parent().find('img').attr('profileid'));
                break;
            case 1:
                classifiedShortlistCars.addtoShortList(element);
                dataLayer.push({ event: 'ShortlistClicked', cat: 'UsedSearchPage', act: 'ShortlistClicked' });
                break;
            case 2:
                alert("Nice to see that you like so many cars! \nCurrently only 6 cars can be shortlisted. \nWe'll soon increase the limit.");
                $('#galleryHolder span').addClass('shortlist-icon--inactive').removeClass('shortlist-icon--active');
                dataLayer.push({ event: 'ShortlistLimitReached', cat: 'UsedSearchPage', act: 'ShortlistLimitReached' });
                break;
        }
        setTimeout(function () {
            element.css("pointer-events", "");
        }, 500);
    },

    addtoShortList: function(slCar){
        slCar = slCar.parents().eq(4);
        var WlObj = classifiedShortlistCars.makeSlObject();
        var tempObj = classifiedShortlistCars.getSlCarObj(slCar);
        var sltemplate = classifiedShortlistCars.createSlTemplate(tempObj);
        var temp = $('.wishlist-data');
        temp.each(function () {
            var innertemp = $(this).find('ul li');
            if (innertemp.length > 0) {
                $(this).find('ul li').last().after(sltemplate);
            }
            else {
                $(this).find('ul').html(sltemplate);
            }
        });
        WlObj.push(tempObj);
        classifiedShortlistCars.setSlCookie(JSON.stringify(WlObj));
        classifiedShortlistCars.toggleShortlistButton(tempObj.profileId, 1);
    },

    removeFromShortlist: function (profileId) {
        var wishlist = $('.wishlist-data');
        wishlist.each(function () {
            var deletedCar = $(this).find('li .shortlistCardetails .slpid a[profileid="' + profileId + '"]');
            deletedCar = deletedCar.parents('li');
            deletedCar.remove();
        });
        classifiedShortlistCars.countIncrement--;
        $('.search-count').html(classifiedShortlistCars.countIncrement);
        var WlObj = classifiedShortlistCars.makeSlObject();
        classifiedShortlistCars.setSlCookie(JSON.stringify(WlObj));
        classifiedShortlistCars.toggleShortlistButton(profileId, 0);
        if (classifiedShortlistCars.isShowDefaultText())
            classifiedShortlistCars.showShortlistDefaultText();
    },

    makeSlObject: function () {
        var ShortList = [];
        var innerWishlist = $('.wishlist-data').eq(0).find('li');
        innerWishlist.each(function () {
            var temp = $(this);
            if (!temp.hasClass('deleted')) {
                var imageUrl = temp.find('.shortlistCarthumb img').attr('src');
                var detailsLink = temp.find('.shortlistCardetails p a').attr('href');
                var headerYear = temp.find('.shortlistCardetails p a span').eq(0).text();
                var headerName = temp.find('.shortlistCardetails p a span').eq(1).text();
                var price = temp.find('.slprice').text().trim().replace(/,/g, "").replace(/₹(?=₹)/g, '').trim();
                var kms = temp.find('.slkms').text().trim().replace(/,/g, "").replace(/km/g, "").trim();
                var makeYear = temp.find('.shortlistCardetails p.slyear').text();
                var date = temp.find('.sldate').text().trim();
                var fuel = temp.find('.shortlistCardetails p.slfuel').text();
                var profileId = temp.find('.shortlistCardetails .slpid a').attr('profileid');
                ShortList.push({
                    "img": imageUrl,
                    "lnk": detailsLink,
                    "yr": headerYear,
                    "makeyr": makeYear,
                    "nm": headerName,
                    "fuel": fuel,
                    "prc": price,
                    "kms": kms,
                    "dt": date,
                    "profileId": profileId
                });
            }
        });
        return ShortList;
    },

    getSlCarObj: function (slCar) {
        var price;
        var imageUrl = slCar.find('.img-placer img').attr('src');
        var detailsLink = slCar.find('#linkToDetails').attr('href');
        var temp = slCar.find('#linkToDetails span').eq(0);
        var headerYear = temp.text();
        var makeYear = slCar.find('.otherDetails span.slYear').text();
        var fuel = slCar.find('.otherDetails span.slfuel').text();
        var profileId = slCar.find("#linkToDetails").attr('profileid');
        temp = slCar.find('#linkToDetails span').eq(1);
        var headerName = temp.text();
        if ($(".gridView").length > 0)
            price = slCar.find('.gridViewPrice .slprice').text().replace(/,/g, "").trim();
        else
            price = slCar.find('.otherDetails .slprice').text().replace(/,/g, "").trim();
        var kms = slCar.find('.slkms').first().text().trim().replace(/,/g, "").replace(/km/g, "").trim();

        var d = new Date();
        var date = d.toDateString();

        var tempObj = {
            "img": imageUrl,
            "lnk": detailsLink,
            "yr": headerYear,
            "makeyr": makeYear,
            "nm": headerName,
            "prc": price,
            "kms": kms,
            "fuel": fuel,
            "dt": date,
            "profileId": profileId
        }
        return tempObj;
    },

    createSlTemplate: function (slCar) {
        var price, kms;
        if (slCar.kms)
            kms = slCar.kms.replace(/(\d)(?=(\d\d)+\d$)/g, "$1,") + " km";
        if (slCar.prc)
            price = slCar.prc.replace(/(\d)(?=(\d\d)+\d$)/g, "$1,").replace(/₹(?=₹)/g, '');
        var template = '<li class="ui-state-default">'
            + '<span class="wishlist-item-close cw-used-sprite shortlist-close-ic cur-pointer position-abt pos-right0 pos-top0" title="Remove from Shortlist"></span>'
            + '<div class="shortlistCarthumb">'
            + '<a  href="' + slCar.lnk + '" target="_blank"  data-role="click-tracking"  data-cat ="UsedCarSearch" data-event="CWNonInteractive" data-action ="ShortListedCar_Click"><img src="' + slCar.img + '" alt="' + slCar.yr + '">'
            + '</div></a>'
            + '<div class="shortlistCardetails">'
            + '<p class="car-details font12 text-bold" title="' + slCar.yr + '"><a target="_blank"  href="' + slCar.lnk + '"  data-role="click-tracking"  data-cat ="UsedCarSearch" data-event="CWNonInteractive" data-action ="ShortListedCar_Click"><span>' + slCar.yr + ' </span></a></p>'
            + '<p class="slprice shortlist-detail-strips">' + price + '</p>'
            + '<p class="margin-top5 slkms">' + kms + '</p>'
            + '<p class="margin-top5 shortlist-detail-strips slyear">' + slCar.makeyr + '</p>'
            + '<p class="margin-top5 slfuel">' + slCar.fuel + '</p>'
            + '<p class="font12 margin-top5 slpid"><a  href="' + slCar.lnk + '"profileid="' + slCar.profileId + '" target="_blank"  data-role="click-tracking"  data-cat ="UsedCarSearch" data-event="CWNonInteractive" data-action ="ShortListedCar_Click">' + "More details &raquo" + '</a></p>'
            + '</div>'
            + '</li>';
        return template;
    },
    setSlCookie: function (cookieContent) {
        var d = new Date();
        d.setTime(d.getTime() + (60 * 24 * 60 * 60 * 1000));
        document.cookie = "SLCookie=" + cookieContent + "; expires=" + d.toUTCString() + "; path=/";
    },

    toggleShortlistButton: function (profileId,operation) {
        if (operation == 0) {
            var deletedLi = $('.stock-list').find('li[profileid="' + profileId + '"]');
            deletedLi.each(function () {
                var current = $(this).find('.shortlist-icon--active');
                $(current).removeClass('shortlist-icon--active').addClass('shortlist-icon--inactive');
            });
        }
        else {
            var addedCar = $('.stock-list').find('li[profileid="' + profileId + '"]');
            addedCar.each(function () {
                var current = $(this).find('.shortlist-icon--inactive');
                if (current) {
                    $(current).removeClass('shortlist-icon--inactive').addClass('shortlist-icon--active');
                }
            });
        }
    },

    shortlistCarsOnFilterSelection: function () {
        var temp = setTimeout(function () {
            var listingsArray = JSON.parse($.cookie('SLCookie'));
            if (listingsArray && listingsArray.length) {
                for (var currentListing in listingsArray) {
                    classifiedShortlistCars.toggleShortlistButton(listingsArray[currentListing].profileId, 1);
                }
            }
            clearTimeout(temp);
        }, 1000)
    },

    countShortlistCars : function(){
        var listWidth = $('.usedsearch-floating-strip .shortlist-popup li').width() + 24;
        var listCount = $('.usedsearch-floating-strip .shortlist-popup li').length;
        var fullWidth = listWidth * listCount;
        if (listCount == 0) {
            $('div.wishlist-data').css('min-height', '322px')
        }
        if (listCount > 0) {
            $('.usedsearch-floating-strip .shortlist-popup ul').css('width', fullWidth);
        }
    },

    removeCarFromWishList: function (node) {
        classifiedShortlistCars.removeFromShortlist($(node).parent().find('.shortlistCardetails .slpid a').attr('profileid'));
        classifiedShortlistCars.countShortlistCars();
    },
    
    shortListButtonClick : function(node){
        var shortListPopUp = $('div.shortlist-popup');
        if (shortListPopUp.is(':hidden')) {
            classifiedShortlistCars.countShortlistCars();
            shortListPopUp.slideDown(function () {
                $(node).addClass('overflowVisible')
            });
        }
        else {
            shortListPopUp.removeClass('overflowVisible').slideUp();
        }
    },

    setSlonPgLoad: function () {
        var tempObj = JSON.parse($.cookie('SLCookie'));
        for (var tempcar in tempObj) {
            var sltemplate = classifiedShortlistCars.createSlTemplate(tempObj[tempcar]);
            var temp = $('.wishlist-data');
            temp.each(function () {
                var innertemp = $(this).find('ul li');
                if (innertemp.length > 0) {
                    $(this).find('ul li').last().after(sltemplate);
                }
                else {
                    $(this).find('ul').html(sltemplate);
                }
            });
        }
        if (tempObj != undefined && tempObj != null)
            $('.search-count').html(tempObj.length);

        classifiedShortlistCars.slButtonToggleForLL();
        classifiedShortlistCars.countIncrement = $('.search-count').html();
    },

    slButtonToggleForLL: function () {
        var listingsArray = JSON.parse($.cookie('SLCookie'));
        for (var currentListing in listingsArray) {
            classifiedShortlistCars.toggleShortlistButton(listingsArray[currentListing].profileId, 1);
        }
    },

    resetShortList: function () {
        $('.stock-list ul li span.shortlist-icon--active').removeClass('shortlist-icon--active').addClass('shortlist-icon--inactive');
        classifiedShortlistCars.showShortlistDefaultText();
        classifiedShortlistCars.setSlCookie(null);
        classifiedShortlistCars.countIncrement = 0;
        $('.search-count').text(classifiedShortlistCars.countIncrement);
    },

    registerEvents: function () {
        $(document).on('click', '.wishlist-item-close', function () { classifiedShortlistCars.removeCarFromWishList(this) });
        $(document).on('click', '.shortlist,.shortlistphotogallery', function () { classifiedShortlistCars.shortlistListing(this) });
        $(document).on('click', '.shortlistBtn', function () { classifiedShortlistCars.shortListButtonClick(this) });
        $(document).on('click', 'span.shortlist-clearAll', function () { classifiedShortlistCars.resetShortList(this) });
    },
    isShowDefaultText : function(){
        return ($('.search-count').html() < 1);
    },
    showShortlistDefaultText: function () {
        var temp = $('.wishlist-data');
        temp.each(function () {
            $(this).find('ul').html("<p class='font16 text-light-grey'>Use this space to save the cars you like.</p>");
        });
    }
}
$(document).ready(function () {
    classifiedShortlistCars.registerEvents();
    classifiedShortlistCars.setSlonPgLoad();
});