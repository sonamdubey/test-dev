window.$zopim || (function (d, s) {
    var z = $zopim = function (c) { z._.push(c) }, $ = z.s =
    d.createElement(s), e = d.getElementsByTagName(s)[0]; z.set = function (o) {
        z.set.
        _.push(o)
    }; z._ = []; z.set._ = []; $.async = !0; $.setAttribute("charset", "utf-8");
    $.src = "//v2.zopim.com/?3NNfyy3sItIkD5XmRd7DwANt1suGKvbK"; z.t = +new Date; $.
    type = "text/javascript"; e.parentNode.insertBefore($, e)
})(document, "script");

if (typeof $zopim !== "undefined") {
    $zopim(function () {
        if (typeof isChatOn != "undefined" && isChatOn == 'true') {
            $zopim.livechat.setOnConnected(function () {
                if (!$zopim.livechat.window.getDisplay()) {
                    $zopim.livechat.button.show();
                }
            })
        }
        $zopim.livechat.window.onHide(function () { $zopim.livechat.button.show(); });
    });
}
