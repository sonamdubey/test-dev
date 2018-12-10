//Function to initailize the nested accordion on click of the parent accordion element
function initializeAccordions(activeElement) {
    var featuresAccordion = new Accordion('.js-accordion-child', {
        multipleOpen: true,
        animationDelay: 400,
        onExpandTransitionStart: function (activeElement) {
            onExpandRecalculate(activeElement);
            childAccordianOpened(activeElement);
        },
        animateOnFocus: false,
        beforeCollapseEvent: childAccordianClosed,
        baseElement: activeElement
    });
}
// tracking module
var trackSpecInfoUsage = function () { 
    var CWC,
    isScrolling,
    setCWC= function () {
        var value = "; " + document.cookie;
        var parts = value.split("; CWC=");
        if (parts.length == 2) CWC = parts.pop().split(";").shift();
        else CWC = "";
    },
    getCWC = function () {
        return CWC
    },
    isVisibleToUser = function (elem) {
        var box = elem.getBoundingClientRect();
        if (box.height <= 0) { return false; }
        var pos = window.pageYOffset + window.innerHeight;
        var pageYOffset = window.pageYOffset || document.documentElement.scrollTop;
        var docTop = document.documentElement.clientTop || 0;

        var top = box.top + (box.height * 0.5) + (pageYOffset - docTop);
        var bottom = box.bottom - (box.height * 0.5) -docTop
        return top < pos && bottom > 0;
    },
    fireContentTracking = function () {
        var selectedTab = document.querySelector(".js-cw-tab__list-item.active")
        if (selectedTab) {
            var elementId = selectedTab.attributes["data-tabs"].value
            var specsInfoElements = document.getElementById(elementId).getElementsByClassName('js-specInfoContent')
            for (var i = 0; i < specsInfoElements.length; i++) {
                if (isVisibleToUser(specsInfoElements[i])) {
                    if (specsInfoElements[i].attributes["data-seen"].value != "true") {
                        var itemName = specsInfoElements[i].closest(".js-itemList").attributes["data-itemname"].value
                        cwTracking.trackAction("CWInteractive", "VersionPage", "ContentDisplay", (getCWC() + '-' + CarDetails.carModelName + '-' + defaultVerName + '-' + itemName));
                        specsInfoElements[i].setAttribute("data-seen","true")
                    }
                }
                else {
                    specsInfoElements[i].setAttribute("data-seen", "false")
                }
            }
        }
    },
    fireTracking = function (nodeName,state,name) {
        if (typeof (cwTracking) != "undefined") {
            cwTracking.trackAction("CWInteractive", "VersionPage", nodeName + "-" + state, (CarDetails.carModelName + '-' + defaultVerName + '-' + name));
        }
    }
    return {
        setCWC: setCWC,
        fireTracking: fireTracking,
        isVisibleToUser: isVisibleToUser,
        fireContentTracking: fireContentTracking
    }
}();
var parentAccordionHeight = 0;
//callback functions
function childAccordianOpened(activeElement) {
    var parentAccordionElement = activeElement.closest('.js-accordion-content-wrapper');
    var parentAccordionBody = parentAccordionElement.querySelector('.js-accordion-content-list');
    parentAccordionHeight = parentAccordionElement.querySelector('.js-accordion-content-list').scrollHeight;
    if (!activeElement.classList.contains("loaded")) {
        var callback = function (response, status) {
            if (status == 200) {
                activeElement.querySelector('.specification-wrapper .js-image-wrapper').innerHTML = response;
                activeElement.classList += " loaded"
            }
            else {
                activeElement.querySelector('.specification-wrapper .js-image-wrapper').innerHTML = ''
            }
            activeElement.querySelector('.js-specs-description-wrapper').classList.remove('hide');
            var tipEle = activeElement.querySelector('.js-specs-tip-wrapper')
            if (tipEle) {
                tipEle.classList.remove('hide');
            }
            onExpandRecalculate(activeElement);
            initReadmoreCollapse(activeElement);
        }
        apiRequest.get({ url: '/specsinfo/?' + activeElement.attributes["data-qs"].value, callback: callback })
    }
    else {
        onExpandRecalculate(activeElement);
        initReadmoreCollapse(activeElement);
    }
    trackSpecInfoUsage.fireTracking(activeElement.attributes["data-child"].value, "Open", activeElement.attributes["data-itemname"].value);
}
function initReadmoreCollapse(activeElement) {
    //initialize read-more-collapse of the child accordion on its click 

    var activeElementDescWrapper = activeElement.querySelector('.js-specs-description-wrapper');
    var activeElementDesc = activeElementDescWrapper.querySelector('.js-specs-description');
    var containerClass = activeElementDesc.classList[1];
    var customReadMoreCollapse = new ReadMoreCollapse('.' + containerClass, {
        ellipsis: false,
        concatData: false,
        onExpandClick: onTextExpand,
        onCollapseClick: onTextExpand,
        baseElement: activeElementDescWrapper
    });
}
function motherAccordianOpened(activeElement) {
    if (!activeElement.classList.contains("loaded")) {
        initializeAccordions(activeElement);
        activeElement.classList += " loaded";
    }
    trackSpecInfoUsage.fireTracking(activeElement.attributes["data-mother"].value, "Open", activeElement.attributes["data-category"].value);
}
function childAccordianClosed (activeElement) {
    resetAnimation(activeElement);
    trackSpecInfoUsage.fireTracking(activeElement.attributes["data-child"].value, "Close", activeElement.attributes["data-itemname"].value);
}
function motherAccordianClosed(activeElement) {
    resetTransition(activeElement)
    trackSpecInfoUsage.fireTracking(activeElement.attributes["data-mother"].value, "Close", activeElement.attributes["data-category"].value);
}
//Function to set and reset animation when parent accordion starts to collapse 
//First the animation duration is set and on transition end the animaton is reset

function resetTransition(activeElement) {
    var activeElementBody = activeElement.querySelector('.js-accordion-content-wrapper');
    activeElementBody.style.transitionDuration = "400ms";
    activeElementBody.addEventListener('transitionend', function () {
        activeElementBody.style.transitionDuration = "0ms";
    });
}

//Callback Function: function to recalculate the height of the child body and accordingly recalculate and set the parents height
function onExpandRecalculate(activeElement) {
    var activeElementBody = activeElement.querySelector('.js-accordion-content-wrapper');
    var parentAccordionElement = activeElement.closest('.js-accordion-content-wrapper');
    var activeElementContent = activeElement.querySelector('.js-accordion-content-list');
    var floatingHeaderHeight = document.querySelector('#model-main-menu').clientHeight;
    parentAccordionElement.style.transitionDuration = "400ms";
    activeElementBody.style.transitionDuration = "400ms";
    //scroll child if contents not in viewport
    if (!(ViewPortDetect.isInViewport(activeElementContent))) {
        scrollToTop(window, activeElement.getBoundingClientRect().top + (window.pageYOffset || document.documentElement.scrollTop) - floatingHeaderHeight, 500);
    }
    var parentAccordionBody = parentAccordionElement.querySelector('.js-accordion-content-list');
    var activeElementBodyHeight = activeElementContent.scrollHeight;
    parentAccordionHeight += activeElementBodyHeight;
    activeElementBody.style.height = activeElementBodyHeight + "px";
    parentAccordionElement.style.height = parentAccordionHeight + "px";
    parentAccordionBody.addEventListener('transitionend', function () {
        parentAccordionElement.style.transitionDuration = "0ms";
        activeElementBody.style.transitionDuration = "0ms";
    });

}

//set animaton duration of the child accordion which has be removed on transition end in the previous function (onExpandRecalculate)
function resetAnimation(activeElement) {
    var activeElementBody = activeElement.querySelector('.js-accordion-content-wrapper');
    var parentAccordionElement = activeElement.closest('.js-accordion-content-wrapper');
    activeElementBody.style.transitionDuration = "300ms";
    parentAccordionElement.style.transitionDuration = "300ms";
    var activeElementBodyHeight = activeElement.querySelector('.js-accordion-content-list').scrollHeight;
    var parentAccordionBodyHeight = parentAccordionElement.querySelector('.js-accordion-content-list').scrollHeight;
    activeElementBody.style.height = activeElementBodyHeight + "px";
    parentAccordionElement.style.height = parentAccordionBodyHeight + "px";
    setTimeout(function () {
        parentAccordionBodyHeight -= activeElementBodyHeight;
        parentAccordionElement.style.height = parentAccordionBodyHeight + "px";
    });
}

//recalculate the height when read-more is click and then apply the updated height
function onTextExpand(readMoreCollapse) {
    var descContainer = readMoreCollapse.container;
    var childElementBody = descContainer.closest('.js-accordion-content-wrapper');
    var parentAccordionElement = childElementBody.closest('.js-parent-accordion');
    var childElementBodyHeight = childElementBody.querySelector('.js-accordion-content-list').scrollHeight;
    childElementBody.style.height = childElementBodyHeight + "px";
    setTimeout(function () {
        var parentAccordionBodyHeight = parentAccordionElement.querySelector('.js-accordion-content-list').scrollHeight;
        parentAccordionElement.style.height = parentAccordionBodyHeight + "px";
    }, 0); 
}

$(document).ready(function () {
    var floatingHeaderHeight = document.querySelector('#model-main-menu').clientHeight;

    var specsAccordion = new Accordion("#tabSpecs", {
        multipleOpen: true,
        animateOnFocus: true,
        animationDelay: 0,
        onExpandTransitionStart: motherAccordianOpened,
        accordionMargin: floatingHeaderHeight,
        beforeCollapseEvent: motherAccordianClosed,
        afterExpandEvent: function (activeElement) {
            if (parentAccordionHeight === 0) {
                parentAccordionHeight = activeElement.offsetHeight;
            }
        }
    });

    var featuresAccordion = new Accordion("#tabFeatures", {
        multipleOpen: true,
        animateOnFocus: true,
        animationDelay: 0,
        onExpandTransitionStart: motherAccordianOpened,
        accordionMargin: floatingHeaderHeight,
        beforeCollapseEvent: motherAccordianClosed
    });
    trackSpecInfoUsage.setCWC();
    window.addEventListener('scroll', debounce(trackSpecInfoUsage.fireContentTracking,100))
})

