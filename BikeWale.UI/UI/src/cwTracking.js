var cwTracking = {
    hostPath: location.host,
    isMobileSite: function () {
        var url = document.location.href;
        if (url.indexOf('/m/') > 0)
            return true;
        else
            return false;
    },
    type: {
        impression: 'imp',
        click: 'click'
    },

    getSource: function () {
        var url = document.referrer;
        if (url.length == 0)
            return 'src=direct';
        else if (url.indexOf(cwTracking.hostPath) < 0)
            return 'src=' + url;

        return '';
    },

    getPageUrl: function () {
        var url = document.location.href;
        if (url.indexOf('?') > 0)
            return 'pi=' + url.split('?')[0];
        else if (url.indexOf('#') > 0)
            return 'pi=' + url.split('#')[0];
        else
            return 'pi=' + url;
    },

    getQSFromUrl: function () {
        var url = document.location.href;
        var qs = '';
        if (url.indexOf('?') > 0) {
            qs = url.split('?')[1];
        }
        else if (url.indexOf('#') > 0) {
            qs = url.split('#')[1];
        }
        if (qs.length > 0) {
            qs = qs.replace(/&+/g, '|');
            return 'qs=' + qs;
        }
        else
            return '';
    },

    getReferrer: function () {
        var url = document.referrer;
        var host = location.host;
        if (url.length > 0 && url.indexOf(host) >= 0) {
            var relativePath = url.split(cwTracking.hostPath)[1];
            return 'ref=' + (relativePath == undefined || relativePath == null ? '' : relativePath.replace(/&+/g, '|'));
        }
        return '';
    },

    addToQS: function (qs, value) {
        if (qs.length > 0)
            qs += '&' + value;
        else
            qs = value;
        return qs;
    },

    getAttributeValue: function (type) {
        switch (type) {
            case 'imp': return 'cwti'; break;
            case 'click': return 'cwtc'; break;
            default: return 'cwcti';
        }
    },

    getCompleteQS: function (node, type) {
        var cat, lbl, act, qs = '', attrValue = cwTracking.getAttributeValue(type), sendQS = false;
        if (node.is('[qs]'))
            sendQS = true;
        cat = node.data(attrValue + 'cat'), lbl = node.data(attrValue + 'lbl'), act = node.data(attrValue + 'act');
        qs = cwTracking.getFinalQS(cat, act, lbl, sendQS);
        return qs;
    },

    getFinalQS: function (cat, act, lbl, sendQS) {
        var src, pageUrl, urlQs, ref, qs = '';
        src = cwTracking.getSource(), pageUrl = cwTracking.getPageUrl(), ref = cwTracking.getReferrer();
        if (cat != undefined && cat.length > 0)
            qs = cwTracking.addToQS(qs, 'cat=' + cat);
        if (act != undefined && act.length > 0)
            qs = cwTracking.addToQS(qs, 'act=' + act);
        if (lbl != undefined && lbl.length > 0)
            qs = cwTracking.addToQS(qs, 'lbl=' + lbl);
        if (src != undefined && src.length > 0)
            qs = cwTracking.addToQS(qs, src);
        if (pageUrl != undefined && pageUrl.length > 0)
            qs = cwTracking.addToQS(qs, pageUrl);

        if (ref != undefined && ref.length > 0)
            qs = cwTracking.addToQS(qs, ref);

        if (sendQS) {
            urlQs = cwTracking.getQSFromUrl();
            if (urlQs != undefined && urlQs.length > 0)
                qs = cwTracking.addToQS(qs, urlQs);
        }

        return qs;
    },

    trackCustomData: function (cat, act, lbl, sendQS) {
        var qs = cwTracking.getFinalQS(cat, act, lbl, sendQS);
        cwTracking.sendRequest(qs);
    },

    trackDataFromNode: function (node, type) {
        var qs = cwTracking.getCompleteQS(node, type);
        cwTracking.sendRequest(qs);
    },

    sendRequest: function (qs) {
        if (qs.length > 0)
            qs = '&' + qs;
        var img = new Image();
        img.src = cwTracking.getHandlerUrl() + $.now() + qs;
    },

    getHandlerUrl: function () {
        return "/bhrigu/pixel.gif?t=";
    },

    trackImpression: function () {
        $('[cwt]').unbind('inview');
        $('[cwt]').bind('inview', function (event, visible) {
            if (visible == true) {
                cwTracking.trackDataFromNode($(this), cwTracking.type.impression);
            }
            else {
                var node = $(this);
                node.unbind('inview');
                node.removeAttr('cwt');
            }
        });
    },

    trackInviewJcarouselImpression: function () {
        $('.jcarousel').on('jcarousel:visiblein', 'li', function () {
            var node = $(this).find("[data-role*='inview-imp']");

            if (node.length > 0) {
                cwTracking.callImpressionTracking(node);
            }
        });
    },

    trackInviewImpression: function () {
        $("[data-role*='inview-imp']").unbind('inview');
        $("[data-role*='inview-imp']").bind('inview', function (event, visible) {

            var node = $(this);

            if (node.closest('.jcarousel').length == 1 && visible == true) {
                if (node.closest('.jcarousel').jcarousel('visible').has(this).length > 0) {
                    cwTracking.callImpressionTracking(node);
                }
            }
            else if (visible == true) {
                cwTracking.callImpressionTracking(node);
            }
        });
    },

    trackPerformace: function () {
        var len = 0, action = '', performanceQS = '', performanceTimings;
        if (window.performance != undefined && window.performance.timing != undefined) {
            performanceTimings = JSON.stringify(window.performance.timing);
            performanceTimings = $.parseJSON(performanceTimings);
            for (var prop in performanceTimings) {
                performanceQS += prop + "=" + performanceTimings[prop] + "|";
            }
            len = performanceQS.length;
            if (len > 0)
                performanceQS = performanceQS.substr(0, len - 1);
            if (cwTracking.isMobileSite())
                action = 'Msite';
            else
                action = 'DesktopSite'
            cwTracking.trackCustomData('BWPerformance', action, performanceQS, false);
        }
    },

    trackInviewGA: function (node) {
        var cat, lbl, act, role;
        cat = node.data('cat'), lbl = node.data('label'), act = node.data('action'), role = node.data('role');
        if (role.indexOf('click') >= 0 && act.indexOf('shown') === -1) {
            act = act + '_shown'
        }
        Common.utils.trackAction('CWNonInteractive', cat, act, lbl);
    },

    uiEvents: function () {

        var cat = (typeof bwTrackingCat === "undefined" || bwTrackingCat === null) ? "BWPageViews" : bwTrackingCat;
        var act = (typeof bwTrackingAct === "undefined" || bwTrackingAct === null) ? "" : bwTrackingAct;
        var lab = (typeof bwTrackingLab === "undefined" || bwTrackingLab === null) ? "NA" : bwTrackingLab;

        cwTracking.trackCustomData(cat, act, lab, true);
        $(document).on('click', '[data-cwtccat]', function () {
            cwTracking.trackDataFromNode($(this), cwTracking.type.click);
        });
        cwTracking.trackImpression();
        cwTracking.trackInviewImpression();
        cwTracking.trackInviewJcarouselImpression();
        $(document).ajaxComplete(function () {
            cwTracking.trackImpression();
            cwTracking.trackInviewImpression();
        });
    },

    prepareLabel: function (trackingparam) {
        var label = '';
        if (trackingparam && typeof (trackingparam) == 'object') {
            for (var property in trackingparam) {
                label = label + property + '=' + trackingparam[property] + '|';
            }
        }
        return label.substring(0, label.length - 1);
    },

    callImpressionTracking: function (node) {
        var role = node.data('role');
        if (role.indexOf('inview-imp') >= 0) {
            node.unbind('inview');
            node.data('role', role.replace('inview-imp', ''));
            cwTracking.trackInviewGA(node);
            return true;
        }
        return false;
    },
    trackUserReview: function (eventName, label) {
        cwTracking.trackCustomData("BWUserReviews", eventName, label, !1)
    },
    trackImagesInteraction: function (category, action, label) {
        cwTracking.trackCustomData(category, action, label, !1)
    },
};

$(window).load(function () {
    setTimeout(function () {
        cwTracking.trackPerformace();
    }, 100);
});

$(document).ready(function () {
    cwTracking.uiEvents();
});