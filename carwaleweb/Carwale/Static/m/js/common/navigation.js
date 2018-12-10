// nav bar code starts
$(".navbarBtn").click(function () {
    trackTopMenu('Hamburger-Icon-Click', window.location.href);
    navbarShow();
});

$(".blackOut-window,.blackOut-window-pq").click(function (e) {
    var nav = $("#nav");
    if (nav.is(":visible")) {
        navbarHide();
    }
});

function navbarShow() {
    $('body').addClass('lock-browser-scroll');
    $("#nav").addClass('open').animate({ 'left': '0px' });
    $(".blackOut-window").show();
}

$(".navUL > li > a").click(function (e) {

    if (!$(this).hasClass("open")) {
        var mainItem = e.target.innerText + ' ' + window.location.href;
        trackNavigation('Main-Menu-item-Click', mainItem);
        var a = $(".navUL li a");
        a.removeClass("open").next("ul").slideUp(350);
        $(this).addClass("open").next("ul").slideDown(350);

        if ($(this).siblings().size() == 0) {
            navbarHide();
        }



        $(".nestedUL > li > a").click(function () {
            var subMenuItem = this.innerText + ' ' + window.location.href;
            trackNavigation('Sub-Menu-item-Click', subMenuItem);
            $(".nestedUL li a").removeClass("open");
            $(this).addClass("open");
            navbarHide();
        });

    }
    else if ($(this).hasClass("open")) {
        $(this).removeClass("open").next("ul").slideUp(350);
    }
}); // nav bar code ends here

$("#navSpecials").click(function (event) {

    if ($(event.target).hasClass("specialCarDropDown") || $(event.target).parent().hasClass("specialCarDropDown")) {
        if (!$('#navSpecials ul li').hasClass('sponsoredLi')) {
            SponsoredNavigation.showSponsoredNavigation(2, 43);
        }
        setTimeout(function () {
            var modelName = $("#navSpecials .sponsoredLi");
            if ($("#navSpecials a").hasClass("open")) {
                var totalModel = modelName.length;
                for (var i = 0; i < totalModel; i++) {
                    Common.utils.trackAction('CWNonInteractive', 'SponsoredNavigation_m', modelName[i].textContent.trim() + '_shown', 'Specials');
                }
            }
        }, 1000)
    }
});

$("#navNewCars").click(function (event) {
    if ($(event.target).hasClass("newCarDropDown") || $(event.target).parent().hasClass("newCarDropDown")) {
        if (!$('#navNewCars ul li').hasClass('sponsoredLi')) {
            SponsoredNavigation.showSponsoredNavigation(1, 43);
        }
        setTimeout(function () {
            var modelName = $("#navNewCars .sponsoredLi");
            if ($("#navNewCars a").hasClass("open")) {
                var totalModel = modelName.length;
                for (var i = 0; i < totalModel; i++) {
                    Common.utils.trackAction('CWNonInteractive', 'SponsoredNavigation_m', modelName[i].textContent.trim() + '_shown', 'NewCars');
                }
            }
        }, 1000)
    }
});


function navbarHide() {
    $('body').removeClass('lock-browser-scroll');
    $("#nav").removeClass('open').animate({ 'left': '-300px' });
    $(".blackOut-window").hide();
}

function trackTopMenu(action, label) {
    dataLayer.push({ event: 'TopMenu', cat: 'TopMenu-Mobile', act: action, lab: label });
}

function trackNavigation(action, label) {
    dataLayer.push({ event: 'Navigation-drawer', cat: 'Navigation-drawer-Mobile', act: action, lab: label });
}

   var  Insurance = {
        promiseObjects: {},
        getStateByCityIdPromise: function (cityId) {
            if (!Common.Insurance.promiseObjects[cityId]) {
                Common.Insurance.promiseObjects[cityId] = $.ajax({
                    url: '/webapi/GeoCity/GetStateByCityId/?cityId=' + cityId,
                });
            }
            return Common.Insurance.promiseObjects[cityId];
        },
        showInsurance: function (cityId, hideInsuranceLinkId) {
            Common.Insurance.getStateByCityIdPromise(cityId).done(function (data) {
                if (data != null) {
                    if ($.inArray(Number(data.StateId), insuranceKeys.cholaStates) < 0)
                        $(document).find(hideInsuranceLinkId).hide();
                    else
                        $(document).find(hideInsuranceLinkId).show();
                }
            });
        },
        showOrHideInsurance: function (hideInsuranceLinkId, cityId) {
            if (insuranceKeys.cholaStates[0] == -1)
                $(document).find(hideInsuranceLinkId).show();
            else if (insuranceKeys.cholaStates[0] == 0)
                $(document).find(hideInsuranceLinkId).hide();
        }
    }
$(document).ready(function(){
    Insurance.showOrHideInsurance('li#navInsuranceLink', $.cookie("_CustCityIdMaster"));
});