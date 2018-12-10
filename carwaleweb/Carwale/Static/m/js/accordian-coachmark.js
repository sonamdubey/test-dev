var ShowCoachMark = (function () {

    var parentDivContainer, plusIcon, accordianHead, isCoachmarkActive, specsCoachmark, self, gotItElement, blackout

    function _setSelectors() {
        parentDivContainer = document.getElementById('tabSpecs'),
        accordianHead = document.querySelectorAll(".accordion-outer-head"),
        plusIcon = parentDivContainer.querySelectorAll('.accordion-plus-icon');
        isCoachmarkActive = false;
        specsCoachmark = document.getElementById("specsCoachmark");
        gotItElement = document.getElementsByClassName("js-coachmark-gotit");
        blackoutWindow = document.getElementsByClassName('blackOut-window')[0];
    }
    function checkPlusIcon(self) {
        if (specsCoachmark) {
            self.preventDefault;
            self.nextElementSibling.querySelectorAll('.accordion-plus-icon:not(.plus-icon--hidden)')[0].parentNode.before(specsCoachmark);
        }
        
    }

    function listener(event) {
        event.preventDefault();
        event.stopPropagation();
    }

    function showCoachmark(callback, event) {
        if (!isCoachmarkActive && specsCoachmark) {
            setTimeout(function () {
                specsCoachmark.classList.add("open")
                isCoachmarkActive = true;
                document.addEventListener('touchmove', listener, { passive: false });
                document.getElementsByTagName('html')[0].classList.add('lock-browser-scroll');
                blackoutWindow.style.display = 'block';
            }, 1000)
        }
        return
    }

    function hideCoachmark(event) {
        isCoachmarkActive = false;
        specsCoachmark.classList.remove("open");
        document.removeEventListener('touchmove', listener, { passive: false });
        document.getElementsByTagName('html')[0].classList.remove('lock-browser-scroll');
        window.localStorage.setItem('specscoachmark', true);
        blackoutWindow.style.display = 'none';
        setTimeout(function () {
            specsCoachmark.remove();
            specsCoachmark = null;
        }, 1000)
    }
    function animateCoackMark() {
        if (specsCoachmark) {
            for (var i = 0 ; i < accordianHead.length; i++) {
                accordianHead[i].addEventListener('click', function (event) {
                    self = this
                    showCoachmark(checkPlusIcon(self), event);                  
                }, { once: true });
            }

            gotItElement[0].addEventListener('click', function (event) {
                if (isCoachmarkActive) {
                    hideCoachmark(event);
                }
            }, false);

            blackoutWindow[0].addEventListener('click', listener);
        }
    }
    function registerEvents() {
        _setSelectors();
        animateCoackMark();
    }
    return { registerEvents: registerEvents }
})();
if (window.localStorage.getItem('specscoachmark') !== 'true') {
    ShowCoachMark.registerEvents();
}
