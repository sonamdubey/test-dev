var immediateAds = [];
var deferredAds = [];

function processAdSlots() {
    googletag.cmd.push(function () {
        googletag.pubads().addEventListener('slotRenderEnded', function (event) {
            if (event.isEmpty) {
                var id = event.slot.getSlotElementId();
                var x = document.getElementById(id);
                if (x.parentElement.classList.contains("ad-slot")) {
                    x.parentElement.style.display = "none";
                }
                $("img.lazy").lazyload({ skip_invisible: true });
            }
        });
    });
    if (typeof adSlot != "undefined") {
        var adElements = document.querySelectorAll('div[id^=div-gpt-ad]');
        googletag.cmd.push(function () {
            googletag.display();
            for (var i = 0; i < adElements.length; i++) {
                if (adElements[i].getAttribute('data-load-immediate') == "true") {
                    immediateAds.push(adSlot[adElements[i].id]);
                } else {
                    deferredAds.push(adSlot[adElements[i].id]);
                }
            }
            if (immediateAds.length > 0) {
                googletag.pubads().refresh(immediateAds);
            }
        });
        var loadAdsOnScroll = function () {
            googletag.cmd.push(function () {
                if (deferredAds.length > 0) {
                    googletag.pubads().refresh(deferredAds);
                }
            });
            window.removeEventListener('scroll', loadAdsOnScroll, false);
        }
        window.addEventListener('scroll', loadAdsOnScroll, false);
    }
}
    docReady(processAdSlots);