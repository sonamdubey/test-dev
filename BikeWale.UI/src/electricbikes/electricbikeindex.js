$(document).ready(function () {

    $(".nav-tab").click(function () {

        try {
            triggerGA('Electric_Bikes', 'Floating_Navigation_Clicked', $(this).data("lab"));
        }
        catch (e) {
            console.log(e);
        }
    });
    // floating tabs
    var makeOverallTabs = $('#makeOverallTabs'),
        overallMakeDetailsFooter = $('#overallMakeDetailsFooter'),
        makeTabsContentWrapper = $('#makeTabsContentWrapper');

    makeOverallTabs.find('.overall-specs-tabs-wrapper a').first().addClass('active');



    if (makeOverallTabs.length > 0) {
        attachListener('scroll', window, highlightSpecTabs);
    }

    $('.overall-specs-tabs-wrapper a[href^="#"]').click(function () {
        var target = $(this.hash);
        if (target.length == 0) target = $('a[name="' + this.hash.substr(1) + '"]');
        if (target.length == 0) target = $('html');
        $('html, body').animate({ scrollTop: target.offset().top - makeOverallTabs.height() }, 1000);
        return false;
    });

    function highlightSpecTabs() {
        var windowScrollTop = $(window).scrollTop(),
            makeOverallTabsOffsetTop = makeOverallTabs.offset().top,
   
            makeTabsContentWrapperOffsetTop = makeTabsContentWrapper.offset().top;

        if (windowScrollTop > makeOverallTabsOffsetTop) {
            makeOverallTabs.addClass('fixed-tab');
        }

        else if (windowScrollTop < makeTabsContentWrapperOffsetTop) {
            makeOverallTabs.removeClass('fixed-tab');
        }
        $('#makeTabsContentWrapper .bw-model-tabs-data').each(function () {
            var top = $(this).offset().top - makeOverallTabs.height(),
            bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                makeOverallTabs.find('a').removeClass('active');
                $('#makeTabsContentWrapper .bw-mode-tabs-data').removeClass('active');

                $(this).addClass('active');
                makeOverallTabs.find('a[href="#' + $(this).attr('id') + '"]').addClass('active');
            }
        });
    }
    });
