docReady(function () {
    $(document).on('click', '.js-leaderboard__tabs .leader-board-tab', function () {
        var tabBodySelector = this.getAttribute('data-tabs');
        var tabBody = document.getElementById(tabBodySelector);
        if (!(tabBody.classList.contains('animate'))) {
            setTimeout(function () {
                tabBody.classList.add('animate');
            }, 300)
        }
    })

    $(document).on('click', '.js-leaderboard-popup-link', function () {
        var currentTab = $('.js-leaderboard__tabs .leader-board-tab.active');
        var bodyselector = currentTab.attr('data-tabs');
        var tabBody = $('#' + bodyselector);
        if (!(tabBody.hasClass('animate'))) {
            setTimeout(function () {
                tabBody.addClass('animate');
            }, 800)

        }
    })

    var leaderboardTabs = $('#leaderboardTabs');

    function lazyLoadTabContentImages(tabId) {
        $('#' + tabId).find('img.lazy').lazyload();
    }

    // Trigger impression tracking
    triggerNonInteractiveGA('leaderboard', 'TVS leaderboard_shown', 'TVS leaderboard');

    // Initialize leadboard popup
    var isInitialTabContentLazyLoaded = false;

    var leaderboardPopup = new Popup('.js-leaderboard-popup-link', {
        closeButtonClass: 'js-leaderboard-popup-close',
        onPopupOpen: function () {
          if (!isInitialTabContentLazyLoaded) {
            var activeTab = leaderboardTabs.find('.js-leaderboard__tabs li.active');

            lazyLoadTabContentImages(activeTab.attr('data-tabs'));

            $('.js-leaderboard__tabs img.lazy').lazyload({
                container: $('.js-leaderboard__tabs')
            })

            isInitialTabContentLazyLoaded = true;
          }
        }
    });

    // Handle image lazy loading for tabs content and reset it's scroll position
    leaderboardTabs.on('click', '.bw-tabs li', function () {
        var tabId = $(this).attr('data-tabs');

        leaderboardTabs.find('.js-tab-body__head').scrollTop(0);
        lazyLoadTabContentImages(tabId);
    })
});
