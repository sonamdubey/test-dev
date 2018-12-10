var classifiedFinance = {
    targetDestination: '',
    iframeError :'',
    getFinance: function (iframeUrl, node, loadIframeIn) {
        classifiedFinance.targetDestination = iframeUrl;
        return new Promise(function (fulfill, reject) {
            classifiedFinance.loadIframe(iframeUrl, "100%", "100%", loadIframeIn).then(function (response) {
                classifiedFinance.addEventListenerForIframe();
                fulfill(response);
            }).catch(function (errResponse) {
                reject(errResponse);
            });
        });
    },

    loadIframe: function (src, width, height, loadFrameIn) {
        if ($(loadFrameIn).find($("iframe")).length > 0) {
            $("iframe").remove();
        }
        iframe = $("<iframe></iframe>").attr({
            "src": src,
            "width": width,
            "height": height
        });
        iframe.appendTo($(loadFrameIn));
        return new Promise(function (fulfill, reject) {
            classifiedFinance.iframeError = setTimeout(function () {
                reject("Failed to load Iframe");
            }, 20000);
            iframe.load(function () {
                clearTimeout(classifiedFinance.iframeError);
                fulfill("Iframe loaded successfully");
            })
        });
    },
    addEventListenerForIframe: function () {
        window.addEventListener('message', classifiedFinance.receiveMessage, false);
    },
    removeEventListenerOfIframe: function () {
        window.removeEventListener('message', classifiedFinance.receiveMessage, false);
    },
    receiveMessage: function (event) {
        var origin = event.origin || event.originalEvent.origin;
        if (classifiedFinance.targetDestination.indexOf(origin) >= 0) {
            if (event.data.toLowerCase() === 'true') 
            {
                classifiedFinance.closeIframe();
            } 
        } 
    },
    closeIframe: function () {
        try {
            if (typeof (M_usedSearch) != 'undefined') {
                M_usedSearch.Finance.btnBackFinanceForm();
            } else if (typeof (carDetails) != 'undefined') {
                carDetails.Finance.btnBackFinanceForm();
            } else if (typeof (D_usedSearch) != 'undefined') {
                D_usedSearch.Finance.closeFinanceForm();
            } else if (typeof (D_carDetailsPage) !='undefined') {
                D_carDetailsPage.Finance.closeFinanceForm();
            } else {
                window.close();
            }
            classifiedFinance.removeEventListenerOfIframe();
        } catch (error) { console.log("Error in closing iframe. Message: " + error.message)}
    },
    openIframeInNewWindow: function (qs) {
        window.open("/m/used/finance.aspx?" + qs, "_blank");
    },
    getQueryString: function (node) {
        var qs = '';
        try
        {
            var makeYear = ($(node).attr('makeyearformatted')) ? $(node).attr('makeyearformatted') : $(node).attr("makeyear");
            qs = "makeid=" + $(node).attr("makeid") + "&modelid=" + $(node).attr("modelid") + "&mfgyear=" + makeYear + "&cityid=" + $(node).attr("cityid")
                       + "&price=" + $(node).attr("pricenumeric")
                       + "&owner=" + $(node).attr('owner')
                       + "&mfgMonth=" + $(node).attr('makemonth')
                       + "&profileId=" + $(node).attr("profileid");
        }
        catch (err) {
            console.log(err);
        }
        return qs;
    }
}