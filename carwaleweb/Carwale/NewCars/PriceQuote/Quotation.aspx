<!doctype html>
<html itemscope itemtype="http://schema.org/WebPage">
<head>
    <%
    PageId = 106;
    Title = "Instant Free New Car Price Quote";
    Description = "Carwale.com New car free price quote.";
    Keywords = "";
    Revisit = "5";
    DocumentState = "Dynamic";
    System.Web.HttpContext.Current.Response.Cache.SetNoTransforms();
    AdPath="/1017752/PriceQuote_";
    noIndex = true;
    %>
    
    <!-- #include file="/includes/global/head-script.aspx" -->
    <link rel="stylesheet" href="/static/css/colorbox.css" type="text/css">

    <link rel="stylesheet" href="/static/css/pq-style.css" type="text/css">
    <link rel="stylesheet" href="/static/css/pq-offer-page.css" type="text/css">
    <script src="/static/src/jquery.dfp.min.js"></script>
    <script language="c#" runat="server">
        int _offersAbTestMinValue = Convert.ToInt32(ConfigurationManager.AppSettings["OffersAbTestMinValue"] ?? "0");
        int _offersAbTestMaxValue = Convert.ToInt32(ConfigurationManager.AppSettings["OffersAbTestMaxValue"] ?? "0");
        int abTestValue = CustomerCookie.AbTest;
        int showAddAnotherCarMinValue = Convert.ToInt32(ConfigurationManager.AppSettings["ShowAddAnotherCarAbTestMinValue"] ?? "0");
        int showAddAnotherCarMaxValue = Convert.ToInt32(ConfigurationManager.AppSettings["ShowAddAnotherCarAbTestMaxValue"] ?? "0");
    </script>
    <script type="text/javascript">
        var abTestValue = "<%= Carwale.Utility.CustomerCookie.AbTest%>";
        var abTestMinValForNewPqDesktop = "<%= CustomParser.parseIntObject(System.Configuration.ConfigurationManager.AppSettings["AbTestMinValForNewPqDesktop"])%>";
        var abTestMaxValForNewPqDesktop = "<%= CustomParser.parseIntObject(System.Configuration.ConfigurationManager.AppSettings["AbTestMaxValForNewPqDesktop"])%>";
    </script>
    <script type="text/javascript">

        /*******************History Plugin****************/
        // Added By Vinayak M on 15/05/2015
        window.JSON || (window.JSON = {}), function () { function f(a) { return a < 10 ? "0" + a : a } function quote(a) { return escapable.lastIndex = 0, escapable.test(a) ? '"' + a.replace(escapable, function (a) { var b = meta[a]; return typeof b == "string" ? b : "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4) }) + '"' : '"' + a + '"' } function str(a, b) { var c, d, e, f, g = gap, h, i = b[a]; i && typeof i == "object" && typeof i.toJSON == "function" && (i = i.toJSON(a)), typeof rep == "function" && (i = rep.call(b, a, i)); switch (typeof i) { case "string": return quote(i); case "number": return isFinite(i) ? String(i) : "null"; case "boolean": case "null": return String(i); case "object": if (!i) return "null"; gap += indent, h = []; if (Object.prototype.toString.apply(i) === "[object Array]") { f = i.length; for (c = 0; c < f; c += 1) h[c] = str(c, i) || "null"; return e = h.length === 0 ? "[]" : gap ? "[\n" + gap + h.join(",\n" + gap) + "\n" + g + "]" : "[" + h.join(",") + "]", gap = g, e } if (rep && typeof rep == "object") { f = rep.length; for (c = 0; c < f; c += 1) d = rep[c], typeof d == "string" && (e = str(d, i), e && h.push(quote(d) + (gap ? ": " : ":") + e)) } else for (d in i) Object.hasOwnProperty.call(i, d) && (e = str(d, i), e && h.push(quote(d) + (gap ? ": " : ":") + e)); return e = h.length === 0 ? "{}" : gap ? "{\n" + gap + h.join(",\n" + gap) + "\n" + g + "}" : "{" + h.join(",") + "}", gap = g, e } } "use strict", typeof Date.prototype.toJSON != "function" && (Date.prototype.toJSON = function (a) { return isFinite(this.valueOf()) ? this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z" : null }, String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function (a) { return this.valueOf() }); var JSON = window.JSON, cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, rep; typeof JSON.stringify != "function" && (JSON.stringify = function (a, b, c) { var d; gap = "", indent = ""; if (typeof c == "number") for (d = 0; d < c; d += 1) indent += " "; else typeof c == "string" && (indent = c); rep = b; if (!b || typeof b == "function" || typeof b == "object" && typeof b.length == "number") return str("", { "": a }); throw new Error("JSON.stringify") }), typeof JSON.parse != "function" && (JSON.parse = function (text, reviver) { function walk(a, b) { var c, d, e = a[b]; if (e && typeof e == "object") for (c in e) Object.hasOwnProperty.call(e, c) && (d = walk(e, c), d !== undefined ? e[c] = d : delete e[c]); return reviver.call(a, b, e) } var j; text = String(text), cx.lastIndex = 0, cx.test(text) && (text = text.replace(cx, function (a) { return "\\u" + ("0000" + a.charCodeAt(0).toString(16)).slice(-4) })); if (/^[\],:{}\s]*$/.test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) return j = eval("(" + text + ")"), typeof reviver == "function" ? walk({ "": j }, "") : j; throw new SyntaxError("JSON.parse") }) }(), function (a, b) { "use strict"; var c = a.History = a.History || {}, d = a.jQuery; if (typeof c.Adapter != "undefined") throw new Error("History.js Adapter has already been loaded..."); c.Adapter = { bind: function (a, b, c) { d(a).bind(b, c) }, trigger: function (a, b, c) { d(a).trigger(b, c) }, extractEventData: function (a, c, d) { var e = c && c.originalEvent && c.originalEvent[a] || d && d[a] || b; return e }, onDomLoad: function (a) { d(a) } }, typeof c.init != "undefined" && c.init() }(window), function (a, b) { "use strict"; var c = a.document, d = a.setTimeout || d, e = a.clearTimeout || e, f = a.setInterval || f, g = a.History = a.History || {}; if (typeof g.initHtml4 != "undefined") throw new Error("History.js HTML4 Support has already been loaded..."); g.initHtml4 = function () { if (typeof g.initHtml4.initialized != "undefined") return !1; g.initHtml4.initialized = !0, g.enabled = !0, g.savedHashes = [], g.isLastHash = function (a) { var b = g.getHashByIndex(), c; return c = a === b, c }, g.saveHash = function (a) { return g.isLastHash(a) ? !1 : (g.savedHashes.push(a), !0) }, g.getHashByIndex = function (a) { var b = null; return typeof a == "undefined" ? b = g.savedHashes[g.savedHashes.length - 1] : a < 0 ? b = g.savedHashes[g.savedHashes.length + a] : b = g.savedHashes[a], b }, g.discardedHashes = {}, g.discardedStates = {}, g.discardState = function (a, b, c) { var d = g.getHashByState(a), e; return e = { discardedState: a, backState: c, forwardState: b }, g.discardedStates[d] = e, !0 }, g.discardHash = function (a, b, c) { var d = { discardedHash: a, backState: c, forwardState: b }; return g.discardedHashes[a] = d, !0 }, g.discardedState = function (a) { var b = g.getHashByState(a), c; return c = g.discardedStates[b] || !1, c }, g.discardedHash = function (a) { var b = g.discardedHashes[a] || !1; return b }, g.recycleState = function (a) { var b = g.getHashByState(a); return g.discardedState(a) && delete g.discardedStates[b], !0 }, g.emulated.hashChange && (g.hashChangeInit = function () { g.checkerFunction = null; var b = "", d, e, h, i; return g.isInternetExplorer() ? (d = "historyjs-iframe", e = c.createElement("iframe"), e.setAttribute("id", d), e.style.display = "none", c.body.appendChild(e), e.contentWindow.document.open(), e.contentWindow.document.close(), h = "", i = !1, g.checkerFunction = function () { if (i) return !1; i = !0; var c = g.getHash() || "", d = g.unescapeHash(e.contentWindow.document.location.hash) || ""; return c !== b ? (b = c, d !== c && (h = d = c, e.contentWindow.document.open(), e.contentWindow.document.close(), e.contentWindow.document.location.hash = g.escapeHash(c)), g.Adapter.trigger(a, "hashchange")) : d !== h && (h = d, g.setHash(d, !1)), i = !1, !0 }) : g.checkerFunction = function () { var c = g.getHash(); return c !== b && (b = c, g.Adapter.trigger(a, "hashchange")), !0 }, g.intervalList.push(f(g.checkerFunction, g.options.hashChangeInterval)), !0 }, g.Adapter.onDomLoad(g.hashChangeInit)), g.emulated.pushState && (g.onHashChange = function (b) { var d = b && b.newURL || c.location.href, e = g.getHashByUrl(d), f = null, h = null, i = null, j; return g.isLastHash(e) ? (g.busy(!1), !1) : (g.doubleCheckComplete(), g.saveHash(e), e && g.isTraditionalAnchor(e) ? (g.Adapter.trigger(a, "anchorchange"), g.busy(!1), !1) : (f = g.extractState(g.getFullUrl(e || c.location.href, !1), !0), g.isLastSavedState(f) ? (g.busy(!1), !1) : (h = g.getHashByState(f), j = g.discardedState(f), j ? (g.getHashByIndex(-2) === g.getHashByState(j.forwardState) ? g.back(!1) : g.forward(!1), !1) : (g.pushState(f.data, f.title, f.url, !1), !0)))) }, g.Adapter.bind(a, "hashchange", g.onHashChange), g.pushState = function (b, d, e, f) { if (g.getHashByUrl(e)) throw new Error("History.js does not support states with fragement-identifiers (hashes/anchors)."); if (f !== !1 && g.busy()) return g.pushQueue({ scope: g, callback: g.pushState, args: arguments, queue: f }), !1; g.busy(!0); var h = g.createStateObject(b, d, e), i = g.getHashByState(h), j = g.getState(!1), k = g.getHashByState(j), l = g.getHash(); return g.storeState(h), g.expectedStateId = h.id, g.recycleState(h), g.setTitle(h), i === k ? (g.busy(!1), !1) : i !== l && i !== g.getShortUrl(c.location.href) ? (g.setHash(i, !1), !1) : (g.saveState(h), g.Adapter.trigger(a, "statechange"), g.busy(!1), !0) }, g.replaceState = function (a, b, c, d) { if (g.getHashByUrl(c)) throw new Error("History.js does not support states with fragement-identifiers (hashes/anchors)."); if (d !== !1 && g.busy()) return g.pushQueue({ scope: g, callback: g.replaceState, args: arguments, queue: d }), !1; g.busy(!0); var e = g.createStateObject(a, b, c), f = g.getState(!1), h = g.getStateByIndex(-2); return g.discardState(f, e, h), g.pushState(e.data, e.title, e.url, !1), !0 }), g.emulated.pushState && g.getHash() && !g.emulated.hashChange && g.Adapter.onDomLoad(function () { g.Adapter.trigger(a, "hashchange") }) }, typeof g.init != "undefined" && g.init() }(window), function (a, b) { "use strict"; var c = a.console || b, d = a.document, e = a.navigator, f = a.sessionStorage || !1, g = a.setTimeout, h = a.clearTimeout, i = a.setInterval, j = a.clearInterval, k = a.JSON, l = a.alert, m = a.History = a.History || {}, n = a.history; k.stringify = k.stringify || k.encode, k.parse = k.parse || k.decode; if (typeof m.init != "undefined") throw new Error("History.js Core has already been loaded..."); m.init = function () { return typeof m.Adapter == "undefined" ? !1 : (typeof m.initCore != "undefined" && m.initCore(), typeof m.initHtml4 != "undefined" && m.initHtml4(), !0) }, m.initCore = function () { if (typeof m.initCore.initialized != "undefined") return !1; m.initCore.initialized = !0, m.options = m.options || {}, m.options.hashChangeInterval = m.options.hashChangeInterval || 100, m.options.safariPollInterval = m.options.safariPollInterval || 500, m.options.doubleCheckInterval = m.options.doubleCheckInterval || 500, m.options.storeInterval = m.options.storeInterval || 1e3, m.options.busyDelay = m.options.busyDelay || 250, m.options.debug = m.options.debug || !1, m.options.initialTitle = m.options.initialTitle || d.title, m.intervalList = [], m.clearAllIntervals = function () { var a, b = m.intervalList; if (typeof b != "undefined" && b !== null) { for (a = 0; a < b.length; a++) j(b[a]); m.intervalList = null } }, m.debug = function () { (m.options.debug || !1) && m.log.apply(m, arguments) }, m.log = function () { var a = typeof c != "undefined" && typeof c.log != "undefined" && typeof c.log.apply != "undefined", b = d.getElementById("log"), e, f, g, h, i; a ? (h = Array.prototype.slice.call(arguments), e = h.shift(), typeof c.debug != "undefined" ? c.debug.apply(c, [e, h]) : c.log.apply(c, [e, h])) : e = "\n" + arguments[0] + "\n"; for (f = 1, g = arguments.length; f < g; ++f) { i = arguments[f]; if (typeof i == "object" && typeof k != "undefined") try { i = k.stringify(i) } catch (j) { } e += "\n" + i + "\n" } return b ? (b.value += e + "\n-----\n", b.scrollTop = b.scrollHeight - b.clientHeight) : a || l(e), !0 }, m.getInternetExplorerMajorVersion = function () { var a = m.getInternetExplorerMajorVersion.cached = typeof m.getInternetExplorerMajorVersion.cached != "undefined" ? m.getInternetExplorerMajorVersion.cached : function () { var a = 3, b = d.createElement("div"), c = b.getElementsByTagName("i"); while ((b.innerHTML = "<!--[if gt IE " + ++a + "]><i></i><![endif]-->") && c[0]); return a > 4 ? a : !1 }(); return a }, m.isInternetExplorer = function () { var a = m.isInternetExplorer.cached = typeof m.isInternetExplorer.cached != "undefined" ? m.isInternetExplorer.cached : Boolean(m.getInternetExplorerMajorVersion()); return a }, m.emulated = { pushState: !Boolean(a.history && a.history.pushState && a.history.replaceState && !/ Mobile\/([1-7][a-z]|(8([abcde]|f(1[0-8]))))/i.test(e.userAgent) && !/AppleWebKit\/5([0-2]|3[0-2])/i.test(e.userAgent)), hashChange: Boolean(!("onhashchange" in a || "onhashchange" in d) || m.isInternetExplorer() && m.getInternetExplorerMajorVersion() < 8) }, m.enabled = !m.emulated.pushState, m.bugs = { setHash: Boolean(!m.emulated.pushState && e.vendor === "Apple Computer, Inc." && /AppleWebKit\/5([0-2]|3[0-3])/.test(e.userAgent)), safariPoll: Boolean(!m.emulated.pushState && e.vendor === "Apple Computer, Inc." && /AppleWebKit\/5([0-2]|3[0-3])/.test(e.userAgent)), ieDoubleCheck: Boolean(m.isInternetExplorer() && m.getInternetExplorerMajorVersion() < 8), hashEscape: Boolean(m.isInternetExplorer() && m.getInternetExplorerMajorVersion() < 7) }, m.isEmptyObject = function (a) { for (var b in a) return !1; return !0 }, m.cloneObject = function (a) { var b, c; return a ? (b = k.stringify(a), c = k.parse(b)) : c = {}, c }, m.getRootUrl = function () { var a = d.location.protocol + "//" + (d.location.hostname || d.location.host); if (d.location.port || !1) a += ":" + d.location.port; return a += "/", a }, m.getBaseHref = function () { var a = d.getElementsByTagName("base"), b = null, c = ""; return a.length === 1 && (b = a[0], c = b.href.replace(/[^\/]+$/, "")), c = c.replace(/\/+$/, ""), c && (c += "/"), c }, m.getBaseUrl = function () { var a = m.getBaseHref() || m.getBasePageUrl() || m.getRootUrl(); return a }, m.getPageUrl = function () { var a = m.getState(!1, !1), b = (a || {}).url || d.location.href, c; return c = b.replace(/\/+$/, "").replace(/[^\/]+$/, function (a, b, c) { return /\./.test(a) ? a : a + "/" }), c }, m.getBasePageUrl = function () { var a = d.location.href.replace(/[#\?].*/, "").replace(/[^\/]+$/, function (a, b, c) { return /[^\/]$/.test(a) ? "" : a }).replace(/\/+$/, "") + "/"; return a }, m.getFullUrl = function (a, b) { var c = a, d = a.substring(0, 1); return b = typeof b == "undefined" ? !0 : b, /[a-z]+\:\/\//.test(a) || (d === "/" ? c = m.getRootUrl() + a.replace(/^\/+/, "") : d === "#" ? c = m.getPageUrl().replace(/#.*/, "") + a : d === "?" ? c = m.getPageUrl().replace(/[\?#].*/, "") + a : b ? c = m.getBaseUrl() + a.replace(/^(\.\/)+/, "") : c = m.getBasePageUrl() + a.replace(/^(\.\/)+/, "")), c.replace(/\#$/, "") }, m.getShortUrl = function (a) { var b = a, c = m.getBaseUrl(), d = m.getRootUrl(); return m.emulated.pushState && (b = b.replace(c, "")), b = b.replace(d, "/"), m.isTraditionalAnchor(b) && (b = "./" + b), b = b.replace(/^(\.\/)+/g, "./").replace(/\#$/, ""), b }, m.store = {}, m.idToState = m.idToState || {}, m.stateToId = m.stateToId || {}, m.urlToId = m.urlToId || {}, m.storedStates = m.storedStates || [], m.savedStates = m.savedStates || [], m.normalizeStore = function () { m.store.idToState = m.store.idToState || {}, m.store.urlToId = m.store.urlToId || {}, m.store.stateToId = m.store.stateToId || {} }, m.getState = function (a, b) { typeof a == "undefined" && (a = !0), typeof b == "undefined" && (b = !0); var c = m.getLastSavedState(); return !c && b && (c = m.createStateObject()), a && (c = m.cloneObject(c), c.url = c.cleanUrl || c.url), c }, m.getIdByState = function (a) { var b = m.extractId(a.url), c; if (!b) { c = m.getStateString(a); if (typeof m.stateToId[c] != "undefined") b = m.stateToId[c]; else if (typeof m.store.stateToId[c] != "undefined") b = m.store.stateToId[c]; else { for (; ;) { b = (new Date).getTime() + String(Math.random()).replace(/\D/g, ""); if (typeof m.idToState[b] == "undefined" && typeof m.store.idToState[b] == "undefined") break } m.stateToId[c] = b, m.idToState[b] = a } } return b }, m.normalizeState = function (a) { var b, c; if (!a || typeof a != "object") a = {}; if (typeof a.normalized != "undefined") return a; if (!a.data || typeof a.data != "object") a.data = {}; b = {}, b.normalized = !0, b.title = a.title || "", b.url = m.getFullUrl(m.unescapeString(a.url || d.location.href)), b.hash = m.getShortUrl(b.url), b.data = m.cloneObject(a.data), b.id = m.getIdByState(b), b.cleanUrl = b.url.replace(/\??\&_suid.*/, ""), b.url = b.cleanUrl, c = !m.isEmptyObject(b.data); if (b.title || c) b.hash = m.getShortUrl(b.url).replace(/\??\&_suid.*/, ""), /\?/.test(b.hash) || (b.hash += "?"), b.hash += "&_suid=" + b.id; return b.hashedUrl = m.getFullUrl(b.hash), (m.emulated.pushState || m.bugs.safariPoll) && m.hasUrlDuplicate(b) && (b.url = b.hashedUrl), b }, m.createStateObject = function (a, b, c) { var d = { data: a, title: b, url: c }; return d = m.normalizeState(d), d }, m.getStateById = function (a) { a = String(a); var c = m.idToState[a] || m.store.idToState[a] || b; return c }, m.getStateString = function (a) { var b, c, d; return b = m.normalizeState(a), c = { data: b.data, title: a.title, url: a.url }, d = k.stringify(c), d }, m.getStateId = function (a) { var b, c; return b = m.normalizeState(a), c = b.id, c }, m.getHashByState = function (a) { var b, c; return b = m.normalizeState(a), c = b.hash, c }, m.extractId = function (a) { var b, c, d; return c = /(.*)\&_suid=([0-9]+)$/.exec(a), d = c ? c[1] || a : a, b = c ? String(c[2] || "") : "", b || !1 }, m.isTraditionalAnchor = function (a) { var b = !/[\/\?\.]/.test(a); return b }, m.extractState = function (a, b) { var c = null, d, e; return b = b || !1, d = m.extractId(a), d && (c = m.getStateById(d)), c || (e = m.getFullUrl(a), d = m.getIdByUrl(e) || !1, d && (c = m.getStateById(d)), !c && b && !m.isTraditionalAnchor(a) && (c = m.createStateObject(null, null, e))), c }, m.getIdByUrl = function (a) { var c = m.urlToId[a] || m.store.urlToId[a] || b; return c }, m.getLastSavedState = function () { return m.savedStates[m.savedStates.length - 1] || b }, m.getLastStoredState = function () { return m.storedStates[m.storedStates.length - 1] || b }, m.hasUrlDuplicate = function (a) { var b = !1, c; return c = m.extractState(a.url), b = c && c.id !== a.id, b }, m.storeState = function (a) { return m.urlToId[a.url] = a.id, m.storedStates.push(m.cloneObject(a)), a }, m.isLastSavedState = function (a) { var b = !1, c, d, e; return m.savedStates.length && (c = a.id, d = m.getLastSavedState(), e = d.id, b = c === e), b }, m.saveState = function (a) { return m.isLastSavedState(a) ? !1 : (m.savedStates.push(m.cloneObject(a)), !0) }, m.getStateByIndex = function (a) { var b = null; return typeof a == "undefined" ? b = m.savedStates[m.savedStates.length - 1] : a < 0 ? b = m.savedStates[m.savedStates.length + a] : b = m.savedStates[a], b }, m.getHash = function () { var a = m.unescapeHash(d.location.hash); return a }, m.unescapeString = function (b) { var c = b, d; for (; ;) { d = a.unescape(c); if (d === c) break; c = d } return c }, m.unescapeHash = function (a) { var b = m.normalizeHash(a); return b = m.unescapeString(b), b }, m.normalizeHash = function (a) { var b = a.replace(/[^#]*#/, "").replace(/#.*/, ""); return b }, m.setHash = function (a, b) { var c, e, f; return b !== !1 && m.busy() ? (m.pushQueue({ scope: m, callback: m.setHash, args: arguments, queue: b }), !1) : (c = m.escapeHash(a), m.busy(!0), e = m.extractState(a, !0), e && !m.emulated.pushState ? m.pushState(e.data, e.title, e.url, !1) : d.location.hash !== c && (m.bugs.setHash ? (f = m.getPageUrl(), m.pushState(null, null, f + "#" + c, !1)) : d.location.hash = c), m) }, m.escapeHash = function (b) { var c = m.normalizeHash(b); return c = a.escape(c), m.bugs.hashEscape || (c = c.replace(/\%21/g, "!").replace(/\%26/g, "&").replace(/\%3D/g, "=").replace(/\%3F/g, "?")), c }, m.getHashByUrl = function (a) { var b = String(a).replace(/([^#]*)#?([^#]*)#?(.*)/, "$2"); return b = m.unescapeHash(b), b }, m.setTitle = function (a) { var b = a.title, c; b || (c = m.getStateByIndex(0), c && c.url === a.url && (b = c.title || m.options.initialTitle)); try { d.getElementsByTagName("title")[0].innerHTML = b.replace("<", "&lt;").replace(">", "&gt;").replace(" & ", " &amp; ") } catch (e) { } return d.title = b, m }, m.queues = [], m.busy = function (a) { typeof a != "undefined" ? m.busy.flag = a : typeof m.busy.flag == "undefined" && (m.busy.flag = !1); if (!m.busy.flag) { h(m.busy.timeout); var b = function () { var a, c, d; if (m.busy.flag) return; for (a = m.queues.length - 1; a >= 0; --a) { c = m.queues[a]; if (c.length === 0) continue; d = c.shift(), m.fireQueueItem(d), m.busy.timeout = g(b, m.options.busyDelay) } }; m.busy.timeout = g(b, m.options.busyDelay) } return m.busy.flag }, m.busy.flag = !1, m.fireQueueItem = function (a) { return a.callback.apply(a.scope || m, a.args || []) }, m.pushQueue = function (a) { return m.queues[a.queue || 0] = m.queues[a.queue || 0] || [], m.queues[a.queue || 0].push(a), m }, m.queue = function (a, b) { return typeof a == "function" && (a = { callback: a }), typeof b != "undefined" && (a.queue = b), m.busy() ? m.pushQueue(a) : m.fireQueueItem(a), m }, m.clearQueue = function () { return m.busy.flag = !1, m.queues = [], m }, m.stateChanged = !1, m.doubleChecker = !1, m.doubleCheckComplete = function () { return m.stateChanged = !0, m.doubleCheckClear(), m }, m.doubleCheckClear = function () { return m.doubleChecker && (h(m.doubleChecker), m.doubleChecker = !1), m }, m.doubleCheck = function (a) { return m.stateChanged = !1, m.doubleCheckClear(), m.bugs.ieDoubleCheck && (m.doubleChecker = g(function () { return m.doubleCheckClear(), m.stateChanged || a(), !0 }, m.options.doubleCheckInterval)), m }, m.safariStatePoll = function () { var b = m.extractState(d.location.href), c; if (!m.isLastSavedState(b)) c = b; else return; return c || (c = m.createStateObject()), m.Adapter.trigger(a, "popstate"), m }, m.back = function (a) { return a !== !1 && m.busy() ? (m.pushQueue({ scope: m, callback: m.back, args: arguments, queue: a }), !1) : (m.busy(!0), m.doubleCheck(function () { m.back(!1) }), n.go(-1), !0) }, m.forward = function (a) { return a !== !1 && m.busy() ? (m.pushQueue({ scope: m, callback: m.forward, args: arguments, queue: a }), !1) : (m.busy(!0), m.doubleCheck(function () { m.forward(!1) }), n.go(1), !0) }, m.go = function (a, b) { var c; if (a > 0) for (c = 1; c <= a; ++c) m.forward(b); else { if (!(a < 0)) throw new Error("History.go: History.go requires a positive or negative integer passed."); for (c = -1; c >= a; --c) m.back(b) } return m }; if (m.emulated.pushState) { var o = function () { }; m.pushState = m.pushState || o, m.replaceState = m.replaceState || o } else m.onPopState = function (b, c) { var e = !1, f = !1, g, h; return m.doubleCheckComplete(), g = m.getHash(), g ? (h = m.extractState(g || d.location.href, !0), h ? m.replaceState(h.data, h.title, h.url, !1) : (m.Adapter.trigger(a, "anchorchange"), m.busy(!1)), m.expectedStateId = !1, !1) : (e = m.Adapter.extractEventData("state", b, c) || !1, e ? f = m.getStateById(e) : m.expectedStateId ? f = m.getStateById(m.expectedStateId) : f = m.extractState(d.location.href), f || (f = m.createStateObject(null, null, d.location.href)), m.expectedStateId = !1, m.isLastSavedState(f) ? (m.busy(!1), !1) : (m.storeState(f), m.saveState(f), m.setTitle(f), m.Adapter.trigger(a, "statechange"), m.busy(!1), !0)) }, m.Adapter.bind(a, "popstate", m.onPopState), m.pushState = function (b, c, d, e) { if (m.getHashByUrl(d) && m.emulated.pushState) throw new Error("History.js does not support states with fragement-identifiers (hashes/anchors)."); if (e !== !1 && m.busy()) return m.pushQueue({ scope: m, callback: m.pushState, args: arguments, queue: e }), !1; m.busy(!0); var f = m.createStateObject(b, c, d); return m.isLastSavedState(f) ? m.busy(!1) : (m.storeState(f), m.expectedStateId = f.id, n.pushState(f.id, f.title, f.url), m.Adapter.trigger(a, "popstate")), !0 }, m.replaceState = function (b, c, d, e) { if (m.getHashByUrl(d) && m.emulated.pushState) throw new Error("History.js does not support states with fragement-identifiers (hashes/anchors)."); if (e !== !1 && m.busy()) return m.pushQueue({ scope: m, callback: m.replaceState, args: arguments, queue: e }), !1; m.busy(!0); var f = m.createStateObject(b, c, d); return m.isLastSavedState(f) ? m.busy(!1) : (m.storeState(f), m.expectedStateId = f.id, n.replaceState(f.id, f.title, f.url), m.Adapter.trigger(a, "popstate")), !0 }; if (f) { try { m.store = k.parse(f.getItem("History.store")) || {} } catch (p) { m.store = {} } m.normalizeStore() } else m.store = {}, m.normalizeStore(); m.Adapter.bind(a, "beforeunload", m.clearAllIntervals), m.Adapter.bind(a, "unload", m.clearAllIntervals), m.saveState(m.storeState(m.extractState(d.location.href, !0))), f && (m.onUnload = function () { var a, b; try { a = k.parse(f.getItem("History.store")) || {} } catch (c) { a = {} } a.idToState = a.idToState || {}, a.urlToId = a.urlToId || {}, a.stateToId = a.stateToId || {}; for (b in m.idToState) { if (!m.idToState.hasOwnProperty(b)) continue; a.idToState[b] = m.idToState[b] } for (b in m.urlToId) { if (!m.urlToId.hasOwnProperty(b)) continue; a.urlToId[b] = m.urlToId[b] } for (b in m.stateToId) { if (!m.stateToId.hasOwnProperty(b)) continue; a.stateToId[b] = m.stateToId[b] } m.store = a, m.normalizeStore(), f.setItem("History.store", k.stringify(a)) }, m.intervalList.push(i(m.onUnload, m.options.storeInterval)), m.Adapter.bind(a, "beforeunload", m.onUnload), m.Adapter.bind(a, "unload", m.onUnload)); if (!m.emulated.pushState) { m.bugs.safariPoll && m.intervalList.push(i(m.safariStatePoll, m.options.safariPollInterval)); if (e.vendor === "Apple Computer, Inc." || (e.appCodeName || "") === "Mozilla") m.Adapter.bind(a, "hashchange", function () { m.Adapter.trigger(a, "popstate") }), m.getHash() && m.Adapter.onDomLoad(function () { m.Adapter.trigger(a, "hashchange") }) } }, m.init() }(window);
        /**************************************************/

        //showImageLoading();
    </script>

    <style>
        .version-box #drpPqCity {
            padding-left: 20px;
            margin-bottom: 10px;
        }

        .pq-map-pointer {
            padding-left: 10px;
            top: 3px;
        }

        h2#addHeader {
            font-size: 18px;
        }

        .bluetxtP {
            color: #0288d1;
        }
        /*.pq-car-thumb {display:inline-block;}
        .pq-car-data {display:inline-block;}
        .car-city-details { display:inline-block; width:100%; margin:0 auto; }
        .quotation-tabs li { margin:0 auto; }
        .quotation-tabs li span.pq-car-data { float:left !important; }
        .quotation-tabs li span.car-name { }
        .quotation-tabs li.quo-active .car-name {width:95%;}*/
        .contact-details1 li {
            margin-bottom: 10px;
            height: 50px;
        }

        .ui-autocomplete {
            z-index: 9999999;
            margin: -1px 0 0 -3px;
            padding: 0;
            width: auto;
            display: inline-block;
            background: #aaa;
            border-radius: 0 0 2px 2px;
            border: 1px solid #ccc;
            border-top: 0;
        }

        .dlp-cw-address li {
            pointer-events: none;
        }

        .dealer-quote-box {
            width: 230px;
            margin: 10px auto;
        }

            .dealer-quote-box .calling-num {
                color: #82888b;
            }

        #addHeader {
            margin-bottom: 5px;
        }

        .insurance-ad-div {
            top: -10px;
            right: -5px;
        }

        #gb-window #gb-title {
            margin-top: 0;
            margin-bottom: 0;
        }

        .gb-close {
            margin-top: -3px;
        }
    </style>
</head>
<body class="bg-white header-fixed-inner special-skin-body no-bg-color quotation-special-page">
    <!-- Image Loading and Black-Out Window code Starts  -->
    <div class="blackOut-window" style="display: block;"></div>
    <div id="loadingCarImg">
        <div class="loading-popup">
            <span class="loading-icon"></span>
            <div class="clear"></div>
        </div>
    </div>
    <form runat="server">
        <!-- #include file="/includes/header.aspx" -->
        <section class="container">
            <div class="grid-12">
                <div class="padding-bottom15 text-center">
                    <div class="inline-block ad-slot" id="leaderBoard">
                    </div>
                </div>
            </div>
        </section>
        <section style="min-height: 500px;">
            <div id="pq-container" class="hide">
                <section class="container">
                    <div class="grid-12">
                        <div class="breadcrumb margin-bottom15 special-skin-text">
                            <!-- breadcrumb code starts here -->
                            <ul class="special-skin-text">
                                <li><a href="/">Home</a></li>
                                <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/">New Cars</a></li>
                                <li><span class="fa fa-angle-right margin-right10"></span><a href="/new/prices.aspx">On-Road Price Quote</a></li>
                            </ul>
                            <div class="clear"></div>
                        </div>
                        <h1 class="font30 text-black special-skin-text">On-Road Price Quote</h1>
                        <div class="border-solid-bottom margin-top10 margin-bottom15"></div>
                    </div>
                    <div class="clear"></div>

                    <div id="pagetop" class="grid-12 margin-top10 margin-bottom40">
                        <div class="content-box-shadow position-rel" style="border-top: 0px;">
                            <div class="quotation-tabs-main" style="display: block;">
                                <div class="tabs">
                                    <div id="orp-car-tab" class="quotation-tabs">
                                        <div class="stPrev stNav hide"><span class="ui-icon orp-prev-icon">Next tab</span></div>
                                        <div id="pq-jcarousel" class="jcarousel-clip jcarousel-clip-horizontal">
                                            <ul id="pqTabs" data-bind="foreach: { data: priceQuoteCollection, afterRender: refreshCarousel }">
                                                <li data-bind="attr: { 'PQId': $data.carPriceQuote().EnId, 'make': $data.carPriceQuote().carDetails.MakeName, 'model': $data.carPriceQuote().carDetails.ModelName, 'version': $data.carPriceQuote().carDetails.VersionName, 'city': $data.carPriceQuote().cityDetail.CityName }" class="pq-car-thumb">
                                                    <span class="car-city-details">
                                                        <span class="pq-car-thumb">
                                                            <img border="0" alt="" title="" data-bind="attr: { 'src': $data.carPriceQuote().carDetails.HostURL + '110X61' + $data.carPriceQuote().carDetails.OriginalImgPath }">
                                                        </span>
                                                        <span class="pq-car-data">
                                                            <span class="car-name">
                                                                <span data-bind="text: $data.carPriceQuote().carDetails.MakeName">&nbsp;</span>
                                                                <span data-bind="text: $data.carPriceQuote().carDetails.ModelName">&nbsp;</span>
                                                                <span data-bind="text: $data.carPriceQuote().carDetails.VersionName">&nbsp;</span>
                                                            </span>
                                                            <span class="pq-price">
                                                                <!-- ko 'if':$data.onRoadPrice() != 0 -->
                                                                ₹
                                                                <span class="price-text" data-bind="text: formatNumeric($data.onRoadPrice())"></span>
                                                                <!--/ko-->
                                                                <!-- ko 'if':$data.carPriceQuote().IsSponsoredCar==true -->
                                                                <span class="spd-box">
                                                                    <img src="https://img.carwale.com/images/ratings/1.gif" align="absmiddle" style="width: 11px;">&nbsp;Featured
                                                                </span>
                                                                <!--/ko-->
                                                            </span>
                                                            <div class="clear"></div>
                                                        </span>
                                                        <span title="Close this tab" data-bind="click: $parent.removeTab" class="close-btn fa fa-close ui-icon-close"></span>
                                                    </span>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="stNext stNav hide"><span class="ui-icon orp-next-icon">Prev tab</span></div>
                                        <%if (abTestValue >= showAddAnotherCarMinValue && abTestValue <= showAddAnotherCarMaxValue) { %>
                                            <div class="orp-add-btn position-rel" id="addCarTab">
                                                <a class="ui-state-default ui-corner-all ad-tooltip" id="addNewPQ" data-bind="click: addNewPQ" href="#">
                                                    <span class="ui-icon ui-icon-plus leftfloat"></span>
                                                    <div class="add-text" id="addText"><strong>Add Another Car</strong></div>
                                                    <span class="addcartabtext hide">Check prices of multiple cars at once by adding further cars
                                                    </span>
                                                    <div class="clear"></div>
                                                </a>
                                                <div class="coachmark-box step4-box hide">
                                                    <span class="coachmark-arrow-right"></span>
                                                    <p class="font16 text-bold margin-bottom5">Add Another Car</p>
                                                    <p class="font16 inline-block">
                                                        Check prices of multiple cars at once<br />
                                                        by adding further cars
                                                    </p>
                                                    <p class="inline-block margin-left20"><a id="stepFourbtn" href="#" class="btn btn-green btn-green-sm font14">Got it</a></p>
                                                    <div class="clear"></div>
                                                </div>
                                            </div>
                                        <div class="clear"></div>
                                      <% } %>
                                    </div>
                                    <div class="clear"></div>
                                </div>

                            </div>

                            <!-- car 1 starts here  -->
                            <!-- TAB CODE ENDS HERE -->
                            <div class="clear"></div>
                            <!-- CAR LEFT PART CODE STARTS HERE -->
                            <div id="pqCarContainer" class="inner-container price-quote-div light-shadow" data-bind="foreach: priceQuoteCollection">
                                <div class="quotation-box hide price-quote-section" data-bind=" attr: { 'PQId': carPriceQuote().EnId }">
                                    <div class="pq-data-highlight">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tbody>
                                                <tr>
                                                    <td width="200" valign="top">
                                                        <div class="quotation-cars">
                                                            <ul>
                                                                <li class="car-data car-data-new">
                                                                    <div class="quo-car-place">
                                                                        <a target="_blank" data-bind="attr: { href: '/' + FormatSpecial($data.carPriceQuote().carDetails.MakeName) + '-cars/' + $data.carPriceQuote().carDetails.MaskingName.toLowerCase() + '/' }">
                                                                            <img border="0" title="" alt="" data-bind="attr: { src: $data.carPriceQuote().carDetails.HostURL + '227x128' + $data.carPriceQuote().carDetails.OriginalImgPath }">
                                                                        </a>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <div class="margin-top5 position-rel">
                                                                        <div class="margin-right5 margin-bottom5  align-center" id="modelLink">
                                                                            <h3>
                                                                                <a target="_blank" class="text-black font18" data-bind="attr: { href: '/' + FormatSpecial($data.carPriceQuote().carDetails.MakeName) + '-cars/' + $data.carPriceQuote().carDetails.MaskingName.toLowerCase() + '/' }"><span data-bind="    text: $data.carPriceQuote().carDetails.MakeName + ' '"></span><span data-bind="    text: $data.carPriceQuote().carDetails.ModelName"></span></a>
                                                                            </h3>
                                                                        </div>
                                                                        <div class="version-box margin-top5">
                                                                            <div class="form-control-box selectcustom-container">
                                                                                <div id="selectcustom-input-box-holder" class="selectcustom-input-box-holder" data-bind="click: $data.toggleVariantCustomDropDown">
                                                                                    <div class="selectcustom-input" data-bind="text: $data.carPriceQuote().carDetails.VersionName"></div>
                                                                                </div>
                                                                                <div class="selectcustom-content">
                                                                                    <ul id="selectOptionList" class="selectcustom" data-bind="foreach: carPriceQuote().CarVersions">
                                                                                        <li data-cwtccat="QuotationPage" data-cwtcact="VersionChange"
                                                                                            data-bind=" attr: { 'data-cwtclbl': 'NewVersoin:' + ID + '|OldVersoin:' + $parent.carPriceQuote().carDetails.VersionId }, css: { 'selected': ID == $parent.carPriceQuote().carDetails.VersionId }, event: { mouseover: $parent.changeDropDownClassOnHover, click: function () { $parent.getPQOnVersionChange($data, $parent); } }">
                                                                                            <a class="clickcustom">
                                                                                                <span class="pop-version-name" data-bind="text: Name"></span>
                                                                                                <span class="pop-version-info">
                                                                                                    <span data-bind="css: { 'pop-version-manual': FuelType != null }, text: FuelType"></span>
                                                                                                    <span data-bind="css: { 'pop-version-manual': Transmission != null }, text: Transmission"></span>
                                                                                                    <span class="pop-version-price">
                                                                                                        <!-- ko if:OnRoadPrice != 0 -->
                                                                                                        <span class="price-available"><span class="padding-right5 rupeecolor">₹</span><span class="pricecolor" data-bind="text: formatNumeric(OnRoadPrice)"></span></span>
                                                                                                        <!-- /ko -->
                                                                                                        <!-- ko if:OnRoadPrice == 0 -->
                                                                                                        <span class="price-not-available pop-grey">Price not available</span>
                                                                                                        <!-- /ko -->
                                                                                                    </span>
                                                                                                </span>
                                                                                            </a>
                                                                                        </li>
                                                                                    </ul>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="coachmark-box step1-box">
                                                                            <span class="coachmark-arrow coachmark-arrow1"></span>
                                                                            <p class="font16 text-bold">Change Versions</p>
                                                                            <p class="font16 inline-block">Check the prices of different versions</p>
                                                                            <p class="inline-block margin-left20"><a id="stepOnebtn" href="#" class="btn btn-green btn-green-sm font14">Next</a></p>
                                                                            <div class="clear"></div>
                                                                            <p><a class="nomoreTips" href="#">Don't show anymore tips</a></p>
                                                                        </div>
                                                                    </div>
                                                                    <div class="clear"></div>
                                                                    <!--ko if: $data.enableMetallicFilter -->
                                                                    <div class="color-type margin-top10">
                                                                        <ul data-bind="foreach: $data.metallicFilters">
                                                                            <li data-bind="click: $parent.setActiveFilter, value: title, text: title, css: style"></li>
                                                                        </ul>
                                                                    </div>
                                                                    <!--/ko-->
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </td>
                                                    <!-- CAR LEFT PART CODE ENDS HERE -->
                                                    <!-- CAR RIGHT PART CODE STARTS HERE -->
                                                    <!-- CAR RIGHT PART(PriceQuote part) CODE STARTS HERE -->
                                                    <td width="430" valign="top">
                                                        <div class="car-price">

                                                            <div class="version-box">

                                                                <div class="form-control-box margin-bottom10 leftfloat  position-rel">
                                                                    <div id="inputCityPriceQuote">
                                                                        <span class="location-image">
                                                                            <img src="https://imgd.aeplcdn.com/0x0/cw/static/pq/grey-icons/dark-gray-location-icon-39x54.png"></span>
                                                                        <input type="text" class="form-control right-input-box-arrow" data-bind="value: ($data.carPriceQuote().cityDetail.AreaId > 0 ? $data.carPriceQuote().cityDetail.AreaName + ', ' : '') + $data.carPriceQuote().cityDetail.CityName" placeholder="Select City" readonly="readonly" style="width: 250px;">
                                                                    </div>
                                                                    <div class="coachmark-box step2-box hide">
                                                                        <span class="coachmark-arrow coachmark-arrow2"></span>
                                                                        <p class="font16 text-bold">Change City</p>
                                                                        <p class="font16 inline-block">Check prices in other cities without having to change your default city</p>
                                                                        <p class="inline-block text-left"><a id="stepTwobtn" href="#" class="btn btn-green btn-green-sm font14">Next</a></p>
                                                                        <div class="clear"></div>
                                                                        <p><a class="nomoreTips" href="#">Don't show anymore tips</a></p>
                                                                    </div>
                                                                </div>
                                                                <div class="clear"></div>
                                                                <!-- ko 'if':$data.carPriceQuote().PriceQuoteList != "" -->
                                                                <div class="border-bottom"></div>
                                                                <!-- /ko -->
                                                                <div class="gst-est-tooltip-content hide font12">This is the CarWale estimated price on the basis of new GST rates, as declared by the Government of India. This price is indicative and may vary from the actual.</div>
                                                                <div class="gst-tooltip-content hide font12">The price is in accordance with the GST rates as declared  by the Government of India.</div>
                                                                <table width="100%" border="0" style="display: table;" cellspacing="0" class="tblDefault" id="price-quote-table" cellpadding="0" data-bind="foreach: $data.filteredPrices()">

                                                                    <tr>
                                                                        <td align="left" valign="top" class="lightgray font14">
                                                                            <!-- ko 'if': CategoryItemId == 2 -->
                                                                            <div class="font14" data-bind="html: $data.Key"></div>
                                                                            <!-- /ko -->
                                                                            <!-- ko 'if': CategoryItemId != 2 -->
                                                                            <div class="font14"><span data-bind="text: $data.Key"></span>
                                                                                <!-- ko 'if':CategoryItemId == PQ.categoryItem.Insurance-->
                                                                                <a href="javascript:void(0)" id="insurance-tooltip" class="insurance-tooltip-icon class-ad-tooltip bt-active" title="Insurance Tooltip" bt-xtitle=""></a>
                                                                                <span class="insurance-tooltip-content hide font12">Insurance amount is inclusive of 3-year third party cover. This is in accordance with the law effective 1-Sep-2018 where in all the new cars sold in India should have a 3-year Third party cover. Contact nearest dealer to know more.</span>
                                                                                <!-- /ko -->
                                                                            </div>
                                                                            <!-- /ko -->
                                                                            <!-- ko 'if':CategoryItemId == PQ.categoryItem.Insurance && $parent.carPriceQuote().CampaignTemplates != null && $parent.carPriceQuote().CampaignTemplates[PQ.pageProperties.Insurance] -->
                                                                            <div data-bind="attr: { onload: PQ.advertisement.TrackImpressions('PQ_Insurance_d', 'SponsoredLink_Shown', 'campaign_'+$parent.carPriceQuote().CampaignTemplates[PQ.pageProperties.Insurance].id) }">
                                                                                <div data-role="click-tracking" data-event="CWInteractive" data-action="PQ_Insurance_d" data-cat="SponsoredLink_Click" id="insuranceLink"
                                                                                    data-bind="html: $parent.carPriceQuote().CampaignTemplates[PQ.pageProperties.Insurance].name, attr: { 'data-label': 'campaign_'+$parent.carPriceQuote().CampaignTemplates[PQ.pageProperties.Insurance].id }">
                                                                                </div>
                                                                            </div>
                                                                            <!-- /ko -->
                                                                        </td>
                                                                        <td colspan="2" align="right" valign="top" class="font14" data-bind="text: formatNumeric(Value)"></td>
                                                                    </tr>

                                                                </table>

                                                                <!-- ko 'if':$data.carPriceQuote().PriceQuoteList != "" -->
                                                                <div class="border-bottom"></div>
                                                                <!-- /ko -->
                                                                <table class="tblDefault" width="100%">
                                                                    <tbody>
                                                                        <!-- ko 'if':$data.carPriceQuote().PriceQuoteList != "" -->
                                                                        <tr>
                                                                            <td style="border-bottom: none!important; font-size: 15px; padding-bottom: 0;"><b>On-Road Price</b></td>
                                                                            <td align="right" style="border-bottom: none!important; font-size: 15px; padding-bottom: 0;">₹ <b data-bind="text: formatNumeric($data.onRoadPrice())"></b></td>

                                                                        </tr>
                                                                        <!-- /ko -->
                                                                        <tr class="gst-unit-row">
                                                                            <td>
                                                                                <div data-bind="if: $data.showGstLabel">(This is the CarWale estimated GST price)</div>
                                                                            </td>
                                                                        </tr>
                                                                        <!-- ko 'if':$data.carPriceQuote().PriceQuoteList == "" -->
                                                                        <tr class="price-not-avalable-row">
                                                                            <td colspan="2">
                                                                                <div class="price-not-avalable content-box-shadow">
                                                                                    <div class="inline-block padding-left5">
                                                                                        <img src="https://imgd.aeplcdn.com/0x0/cw/static/icons/m/no-price.png" alt="price not available" title="">
                                                                                    </div>
                                                                                    <div class="no-price-message inline-block font14 text-dark-grey">
                                                                                        <span class="text-black">Sorry, price not available in </span>
                                                                                        <span class="text-black text-bold" data-bind="text: $data.carPriceQuote().cityDetail.CityName"></span>
                                                                                        <p class="font12 text-light-grey">You can try the following</p>
                                                                                    </div>
                                                                                    <div class="clear"></div>
                                                                                    <ul class="change-options margin-left25 margin-top20">
                                                                                        <li>
                                                                                            <span class="text-grey text-bold">Change Version</span>
                                                                                            <p class="margin-top8 font12 text-light-grey">Select another version of this car</p>
                                                                                            <span class="or-text text-light-grey"></span>
                                                                                        </li>
                                                                                        <li>
                                                                                            <span class="text-grey text-bold">Change City</span>
                                                                                            <p class="margin-top8 font12 text-light-grey">Select another city of your choice</p>
                                                                                            <!-- ko if:nearByCities() != null && nearByCities().length > 0 -->
                                                                                            <span class="or-text text-light-grey"></span>
                                                                                            <!-- /ko -->
                                                                                        </li>
                                                                                        <!-- ko if:nearByCities() != null && nearByCities().length > 0 -->
                                                                                        <li>
                                                                                            <span class="text-grey text-bold">See price in nearby cities </span>
                                                                                            <ul class="blue margin-top8 font12" data-bind="foreach: nearByCities()">
                                                                                                <li class="inline-block">
                                                                                                    <a data-bind="text: name, attr: { 'data-cityId': id, 'data-cityName': name }" class="cur-pointer nearbycity"></a>
                                                                                                    <!-- ko if:($parent.nearByCities().length - 1) != $index() -->
                                                                                                    <span>,</span>
                                                                                                    <!-- /ko -->
                                                                                                </li>
                                                                                            </ul>
                                                                                        </li>
                                                                                        <!-- /ko -->
                                                                                    </ul>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <!-- /ko -->
                                                                        <!--ko 'if':$data.carPriceQuote().SponsoredDealer != null -->
                                                                        <!--ko 'if':$data.carPriceQuote().SponsoredDealer.LinkText != "" -->
                                                                        <!--ko 'if':$data.carPriceQuote().SponsoredDealer.LinkText != null -->
                                                                        <tr data-bind="css: { hide: $data.carPriceQuote().SponsoredDealer.LinkText == '' }">

                                                                            <td align="right" style="border-bottom: none!important; padding-top: 0px;" colspan="2">
                                                                                <span class="margin-right5"></span>
                                                                                <a class="campaignLinkCTA btnOfferLink cur-pointer text-bold font14"
                                                                                    id="A1" data-cwtccat="QuotationPage" data-cwtcact="PQLinkClick" data-bind="text: carPriceQuote().SponsoredDealer.LinkText, attr: { 'data-cwtclbl': 'make:' + $data.carPriceQuote().carDetails.MakeName + '|model:' + $data.carPriceQuote().carDetails.ModelName + '|version:' + $data.carPriceQuote().carDetails.VersionName + '|city:' + $data.carPriceQuote().cityDetail.CityName }"></a></td>
                                                                        </tr>
                                                                        <!-- /ko -->
                                                                        <!-- /ko -->
                                                                        <!-- /ko -->
                                                                        <!-- ko 'if':$data.carPriceQuote().IsSponsoredCar==false -->
                                                                        <!-- ko 'if':$data.carPriceQuote().discountSummary!=null -->
                                                                        <!-- ko 'if':$data.carPriceQuote().discountSummary.MaxDiscount > 0 || ($data.carPriceQuote().discountSummary.MaxDiscount == 0 && $data.carPriceQuote().discountSummary.MaskingName != null && $data.carPriceQuote().discountSummary.Offers != null && $data.carPriceQuote().discountSummary.Offers != "") -->
                                                                        <tr>
                                                                            <td align="right" style="border-bottom: none!important; padding-top: 0px;" colspan="2">
                                                                                <span class="margin-right5"></span>
                                                                                <a target="_blank" class="btnOfferLink cur-pointer text-bold font14" data-bind="attr: { href: '/' + FormatSpecial($data.carPriceQuote().carDetails.MakeName) + '-cars/' + $data.carPriceQuote().discountSummary.MaskingName.toLowerCase() + '/advantage/?src=d07&cityid=' + $data.carPriceQuote().cityDetail.CityId, onload: PQ.advantage.advantageTracking('CWNonInteractive', 'dealsimpression_desktop') }, click: function (data, event) { PQ.advantage.advantageTracking('CWInteractive', 'dealsaccess_desktop'); return true; }">
                                                                                    <!-- ko 'if':$data.carPriceQuote().discountSummary.MaxDiscount > 0 -->
                                                                                    <span>Save</span>&nbsp;
                                                                                        ₹ 
                                                                                        <span data-bind="    text: Common.utils.formatNumeric(carPriceQuote().discountSummary.MaxDiscount)"></span>&nbsp;
                                                                                        <span>on <span data-bind="text: Common.utils.removeYearFromModelName(carPriceQuote().discountSummary.ModelName)"></span></span>
                                                                                    <!-- /ko -->
                                                                                    <!-- ko 'if':$data.carPriceQuote().discountSummary.MaxDiscount == 0 && $data.carPriceQuote().discountSummary.Offers != null && $data.carPriceQuote().discountSummary.Offers != "" -->
                                                                                    Offer available on <span data-bind="text: Common.utils.removeYearFromModelName(carPriceQuote().discountSummary.ModelName)"></span>
                                                                                    <!-- /ko -->
                                                                                </a>
                                                                            </td>
                                                                        </tr>
                                                                        <!-- /ko -->
                                                                        <!-- /ko -->
                                                                        <!-- /ko -->
                                                                        <!-- ko 'if': $data.carPriceQuote().PriceQuoteList.length > 1 -->
                                                                        <!--ko 'if': ($data.carPriceQuote().SponsoredDealer == null || ($data.carPriceQuote().SponsoredDealer != null &&  ($data.carPriceQuote().SponsoredDealer.LinkText == null || $data.carPriceQuote().SponsoredDealer.LinkText == "")))-->
                                                                        <!-- ko 'if':$data.carPriceQuote().discountSummary == null ||  ($data.carPriceQuote().discountSummary != null && $data.carPriceQuote().IsSponsoredCar == true)-->
                                                                        <!-- ko 'if':$data.carPriceQuote().CampaignTemplates != null && $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.Overview] -->
                                                                        <tr>
                                                                            <td align="right" class="bankbazaar" colspan="2">
                                                                                <div data-bind="attr: { onload: PQ.advertisement.TrackImpressions('PQ_Overview_d', 'SponsoredLink_Shown', 'campaign_'+ $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.Overview].id) }">
                                                                                    <div class="margin-right5" data-role="click-tracking" data-event="CWInteractive" data-action="PQ_Overview_d" data-cat="SponsoredLink_Click"
                                                                                        data-bind="html: $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.Overview].name, attr: { 'data-label': 'campaign_' + $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.Overview].id }">
                                                                                    </div>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <!-- /ko -->
                                                                        <!-- /ko -->
                                                                        <!-- /ko -->
                                                                        <!-- /ko -->
                                                                        <!-- /ko -->
                                                                    </tbody>
                                                                </table>

                                                                <span class="spnLoadingPQ hide">
                                                                    <img src="https://imgd.aeplcdn.com/0x0/statics/loader.gif" class="imgloading-pricequote"><span style="margin-left: 271px;">Loading Price Quote</span></span>
                                                                <div class="clear"></div>
                                                            </div>
                                                    </td>
                                                    <!-- CAR RIGHT PART(PriceQuote part) CODE STARTS HERE -->
                                                </tr>
                                            </tbody>
                                        </table>

                                    </div>
                                    <!-- CAR RIGHT PART(DealerAd) CODE STARTS HERE -->

                                    <!-- New structure of pq redesign slot starts here -->
                                    <div class="position-rel">
                                        <div id="Div8" style="display: block;" class="animate-div">
                                            <!--ko 'if':$data.carPriceQuote().SponsoredDealer != null -->
                                            <!-- ko 'if':carPriceQuote().SponsoredDealer.DealerId != "0"-->
                                            <span data-bind="html: carPriceQuote().SponsoredDealer.TemplateHtml"></span>
                                            <!-- /ko -->
                                            <!-- /ko -->
                                            <!-- ko 'if': !isCrossSellExists -->
                                            <!-- ko 'if':$data.carPriceQuote().SponsoredDealer == null || carPriceQuote().SponsoredDealer.DealerId == "0" -->
                                            <!-- ko 'if':$data.carPriceQuote().discountSummary == null ||  ($data.carPriceQuote().discountSummary.MaxDiscount ==  0 && ($data.carPriceQuote().discountSummary.Offers == null || $data.carPriceQuote().discountSummary.Offers == "")) -->
                                            <!-- ko 'if':$data.carPriceQuote().CampaignTemplates != null && $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.TurboWidget] -->
                                            <div data-bind="attr: { onload: PQ.advertisement.TrackImpressions('PQ_TurboWidget_d', 'SponsoredLink_Shown', 'campaign_'+ $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.TurboWidget].id) }">
                                                <div class="margin-right5" data-role="click-tracking" data-event="CWInteractive" data-action="PQ_TurboWidget_d" data-cat="SponsoredLink_Click"
                                                    data-bind="html: $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.TurboWidget].name, attr: { 'data-label': 'campaign_' + $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.TurboWidget].id }">
                                                </div>
                                            </div>
                                            <!-- /ko -->
                                            <!-- /ko -->
                                            <!-- /ko -->
                                            <!-- /ko -->

                                            <!-- cross-sell slug starts here -->
                                            <!-- ko 'if': isCrossSellExists -->

                                            <div class="position-rel multiple-ad-container" data-bind="css: { 'cross-sell-carousel': $data.carPriceQuote().CrossSellCampaignList.length > 1 }">
                                                <div class="jcarousel-wrapper margin-top10">
                                                    <div class="jcarousel">
                                                        <ul data-bind="foreach: $data.carPriceQuote().CrossSellCampaignList">
                                                            <li>
                                                                <div class="contentWrapper" data-tracking="carsWithSavings" data-savings="7400000" data-model="Gurkha" data-city="Thane">
                                                                    <div class="pq-data-highlight-call cross-cell-slug" id="crossSellDiv">
                                                                        <div id="mainDealerSponsor2" class="default-data position-rel">
                                                                            <div id="divToyotaDealer2" class="default-data position-rel" style="min-height: 115px;">
                                                                                <h2 id="addHeader2" class="font16 bg-light-grey text-left text-bold content-inner-block-10 padding-top10 padding-bottom10 rounded-corner-no-bottom5">Recommended For You</h2>

                                                                                <div class="slug-car-place padding-top15 cursor" onclick="newPqByCrossSell(this)" data-bind="attr: { modelId: $data.ModelId, versionId: $data.VersionId }">

                                                                                    <div class="car-img-wrap leftfloat" target="_blank" data-bind="attr: { 'lab': $parent.carPriceQuote().cityDetail.CityName + ' - ' + $parent.carPriceQuote().carDetails.ModelName + ' - ' + $data.ModelName + ' - ' + $data.DealerName + ' - ' + $data.CampaignId }">
                                                                                        <img border="0" title="" alt="" data-bind="attr: { src: HostURL + '110X61' + OriginalImgPath }">
                                                                                    </div>
                                                                                    <div class="inline-block rightfloat cross-sell-car-detail padding-right10">
                                                                                        <div class="text-black text-bold font14" data-bind="text: $data.MakeName + ' ' + $data.ModelName"></div>
                                                                                        <div class="font12 text-black padding-top5">₹<span class="font14 text-bold" data-bind="text: formatNumeric(OnRoadPrice)"></span> <span class="font12">On-Road Price, <span data-bind="    text: $parent.carPriceQuote().cityDetail.CityName"></span></span></div>
                                                                                    </div>
                                                                                    <div class="clear"></div>
                                                                                </div>

                                                                                <div class="clear border-bottom margin-left10 margin-right10 margin-top10 margin-bottom10"></div>
                                                                                <div class="padding-left10 padding-right10 text-left padding-bottom5">
                                                                                    <div class="font14 text-black text-bold margin-bottom5 contact-dealer-detail">Contact <span data-bind="text: $data.DealerName"></span></div>
                                                                                    <div class="padding-bottom10 text-dark-grey authorized-dealer-text">Authorized <span class="text-black text-bold" data-bind="text: $data.MakeName"></span>dealer</div>
                                                                                    <a class="btn btn-white action-button red-btn align-center font14 text-uppercase crossSellCTA" >Assist Me</a>
                                                                                    <!-- ko 'if' : $data.MaskingNumber != null && $data.MaskingNumber != "" -->
                                                                                    <span class="margin-left5 margin-right5">or</span>
                                                                                    <div class="font18 margin-top5 inline-block margin-bottom10 text-bold"><span class="fa fa-phone"></span><span data-bind="text: $data.MaskingNumber"></span></div>
                                                                                    <!-- /ko -->
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="clear"></div>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                    <div>
                                                        <!-- ko 'if': $data.carPriceQuote().CrossSellCampaignList.length > 1 -->
                                                        <span class="jcarousel-control-left"><a class="cwsprite jcarousel-control-prev"></a></span>
                                                        <span class="jcarousel-control-right"><a class="cwsprite jcarousel-control-next"></a></span>
                                                        <!-- /ko -->
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /ko -->
                                            <!-- cross-sell slug ends here -->

                                            <!-- Advantage Ad Slug Starts Here -->
                                            <!-- ko 'if': !isCrossSellExists -->
                                            <!-- ko 'if':$data.carPriceQuote().IsSponsoredCar==false -->
                                            <!-- ko 'if':$data.carPriceQuote().discountSummary!=null -->
                                            <!-- ko 'if':$data.carPriceQuote().discountSummary.MaxDiscount > 0 || ($data.carPriceQuote().discountSummary.MaxDiscount == 0 && $data.carPriceQuote().discountSummary.Offers != null && $data.carPriceQuote().discountSummary.Offers != "")-->
                                            <div id="advantageAdDiv" class="pq-data-highlight-call sds-box advantage-ad-slug" style="background: #fff; margin: 10px 10px 10px 0;">
                                                <div class="default-data position-rel">
                                                    <div class="sponsored-dealer-slot default-data" style="min-height: 115px;">
                                                        <!-- ko 'if':$data.carPriceQuote().discountSummary.MaxDiscount > 0 -->
                                                        <h3 class="text-black margin-bottom10">Save <span class="fa fa-inr"></span>
                                                            <span data-bind="text: Common.utils.formatNumeric(carPriceQuote().discountSummary.MaxDiscount)"></span>
                                                            &nbsp;on <span data-bind="text: Common.utils.removeYearFromModelName(carPriceQuote().discountSummary.ModelName)"></span>
                                                        </h3>
                                                        <!-- /ko -->
                                                        <!-- ko 'if':$data.carPriceQuote().discountSummary.MaxDiscount == 0 && $data.carPriceQuote().discountSummary.Offers != null && $data.carPriceQuote().discountSummary.Offers != ""-->
                                                        <h3 class="text-black margin-bottom10">Attractive Offer on <span data-bind="text: Common.utils.removeYearFromModelName(carPriceQuote().discountSummary.ModelName)"></span>
                                                        </h3>
                                                        <!-- /ko -->
                                                        <ul class="margin-bottom15">
                                                            <li><span class="price-quote-all-imgs pq-tick-icon margin-right5"></span>Brand New Showroom Stock</li>
                                                            <!-- ko 'if':$data.carPriceQuote().discountSummary.DealsCount == 1 -->
                                                            <li><span class="price-quote-all-imgs pq-tick-icon margin-right5"></span>Only <span data-bind="    text: $data.carPriceQuote().discountSummary.DealsCount"></span>&nbsp;Car Available</li>
                                                            <!-- /ko -->
                                                            <!-- ko 'if':$data.carPriceQuote().discountSummary.DealsCount > 1 && $data.carPriceQuote().discountSummary.DealsCount < 6 -->
                                                            <li><span class="price-quote-all-imgs pq-tick-icon margin-right5"></span>Only <span data-bind="    text: $data.carPriceQuote().discountSummary.DealsCount"></span>&nbsp;Cars Available</li>
                                                            <!-- /ko -->
                                                            <!-- ko 'if':$data.carPriceQuote().discountSummary.DealsCount > 5 -->
                                                            <li><span class="price-quote-all-imgs pq-tick-icon margin-right5"></span>Only <span>Few</span> Cars Available</li>
                                                            <!-- /ko -->
                                                            <li><span class="price-quote-all-imgs pq-tick-icon margin-right5"></span>Attractive EMI Options</li>
                                                            <li><span class="price-quote-all-imgs pq-tick-icon margin-right5"></span>Immediate Availability</li>
                                                        </ul>
                                                    </div>
                                                    <a target="_blank" data-cwtccat="QuotationPage" data-cwtcact="AdvantageSectionClick"
                                                        class="btn btn-orange btn-full-width action-button red-btn align-center" data-bind="    attr: {
        'data-cwtclbl': 'make:' + $data.carPriceQuote().carDetails.MakeName + '|model:' + $data.carPriceQuote().carDetails.ModelName + '|version:' + $data.carPriceQuote().carDetails.VersionName + '|city:' + $data.carPriceQuote().cityDetail.CityName,
        href: '/' + FormatSpecial($data.carPriceQuote().carDetails.MakeName) + '-cars/' + $data.carPriceQuote().discountSummary.MaskingName.toLowerCase() + '/advantage/?src=d06&cityid=' + $data.carPriceQuote().cityDetail.CityId,
        onload: PQ.advantage.advantageAdTracking('CWNonInteractive', 'dealsimpression_desktop')
    }, click: function (data, event) { PQ.advantage.advantageAdTracking('CWInteractive', 'dealsaccess_desktop'); return true; }">Check This Offer Out</a>
                                                </div>
                                            </div>
                                            <!-- /ko -->
                                            <!-- /ko -->
                                            <!-- /ko -->
                                            <!-- /ko -->
                                            <!-- Advantage Ad Slug Ends Here -->

                                            <!-- New structure of pq redesign slot end here -->
                                        </div>
                                        <!--ko 'if':$data.carPriceQuote().SponsoredDealer != null -->
                                        <!-- ko 'if':carPriceQuote().SponsoredDealer.DealerId != "0"-->
                                        <div class="coachmark-box step3-box hide">
                                            <span class="coachmark-arrow coachmark-arrow3"></span>
                                            <p class="font16 text-bold margin-bottom5">Contact Dealer</p>
                                            <p class="font16 margin-bottom5">Want more information or help in buying this car? Call up the dealer or leave your</p>
                                            <p class="font16 inline-block">number behind for a callback</p>
                                            <p class="inline-block margin-left20"><a id="stepThreebtn" href="#" class="btn btn-green btn-green-sm font14">Next</a></p>
                                            <div class="clear"></div>
                                            <p><a class="nomoreTips" href="#">Don't show anymore tips</a></p>
                                        </div>
                                        <!-- /ko -->
                                        <!-- /ko -->
                                    </div>
                                    <!-- CAR RIGHT PART(DealerAd part) CODE Ends HERE -->
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <!-- car 1 Ends here  -->
                        </div>
                        <!-- OEM OFFERS STARTS -->
                        <div data-bind="foreach: priceQuoteCollection" class="pqBelowContainer">
                            <!-- ko 'if': $data.carPriceQuote().PriceQuoteList.length > 1 && $data.carPriceQuote().OfferAndDealerAd && $data.carPriceQuote().OfferAndDealerAd.Offer && $data.carPriceQuote().OfferAndDealerAd.Offer.offerDetails -->
                            <div class="oemWrapper pq-below-container hide" data-bind="attr: { 'PQId': carPriceQuote().EnId }">
                                <div id="oemOffersHeader" class="content-box-shadow border-bottom0 margin-top30">
                                    <div class="content-inner-block-10  position-rel">
                                        <span class="icon-wrapper-58 inline-block"></span>
                                        <div class="inline-block padding-left20 ">
                                            <p class="font18 text-bold text-black" data-bind="text: $data.carPriceQuote().OfferAndDealerAd.Offer.offerDetails.heading"></p>
                                            <p class="font14" data-bind="text: $data.carPriceQuote().OfferAndDealerAd.Offer.offerDetails.callOutLine"></p>
                                        </div>
                                    </div>
                                </div>

                                <div id="oemOffersFooter" class="content-box-shadow">
                                    <div class="content-inner-block-10  position-rel">
                                        <div class="list-container">
                                            <ul class="oemOfferList oem-offers__ul" data-bind="foreach: $data.carPriceQuote().OfferAndDealerAd.Offer.categoryDetails">
                                                <li class="oem-offers__li hide">
                                                    <span class="icon-wrapper-47 inline-block">
                                                        <img class="oem-offers__icon" border="0" width="40" data-bind="attr: { src: 'https://imgd.aeplcdn.com/0x0' + $data.originalImgPath }">
                                                    </span>
                                                    <div class="inline-block margin-left20 offerPara">
                                                        <p class="font14 text-black truncate-offer inline-block offerList" data-bind="text: $data.offerText"></p>
                                                        <!-- ko 'if': $parent.carPriceQuote().OfferAndDealerAd.Offer.categoryDetails.length > 1 -->
                                                        <span class="open-all-offers text-bold font16 addOffers" data-bind="text: ('+' + ($parent.carPriceQuote().OfferAndDealerAd.Offer.categoryDetails.length - 1) + ' Offers')"></span>
                                                        <!-- /ko -->
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>

                                        <!-- ko 'if': $data.carPriceQuote().SponsoredDealer != null && $data.carPriceQuote().SponsoredDealer.LinkText != null && $data.carPriceQuote().SponsoredDealer.LinkText != "" -->
                                        <% if (_offersAbTestMinValue <= abTestValue && abTestValue <= _offersAbTestMaxValue) { %>
                                            <a class="open-all-offers text-bold font16 addOffers dealer-assist-link" data-cwtccat="QuotationPage" data-cwtcact="PQOfferButtonClick" data-bind="attr: { 'data-cwtclbl': 'make:' + $data.carPriceQuote().carDetails.MakeName + '|model:' + $data.carPriceQuote().carDetails.ModelName + '|version:' + $data.carPriceQuote().carDetails.VersionName + '|city:' + $data.carPriceQuote().cityDetail.CityName }"><%= System.Configuration.ConfigurationManager.AppSettings["OffersCtaButtonText"] ?? string.Empty %></a>
                                        <% } else { %>
                                            <input class="dealer-assist-btn" type="button" onclick="AdOfferLinkClick(this)" value="<%= System.Configuration.ConfigurationManager.AppSettings["OffersCtaButtonText"] ?? string.Empty %>" data-cwtccat="QuotationPage" data-cwtcact="PQOfferButtonClick" data-bind="    text: $data.carPriceQuote().SponsoredDealer.LinkText, attr: { 'data-cwtclbl': 'make:' + $data.carPriceQuote().carDetails.MakeName + '|model:' + $data.carPriceQuote().carDetails.ModelName + '|version:' + $data.carPriceQuote().carDetails.VersionName + '|city:' + $data.carPriceQuote().cityDetail.CityName }" />
                                        <% } %>
                                        <!-- /ko -->

                                        <div class="position-abt info-wrapper">
                                            <div class="text-black font11 inline-block" data-bind="text: $data.carPriceQuote().OfferAndDealerAd.Offer.offerDetails.validityText"></div>
                                            <div class="inline-block padding-left20 oemTnC">
                                                <span class="font11 tncText">T&ampC Apply</span>
                                                <span class="instruction-icon"></span>
                                                <div class="oemTnC-tooltip">
                                                    <!-- ko 'if': $data.carPriceQuote().OfferAndDealerAd.Offer.offerDetails.disclaimer -->
                                                    <div class="offer-oem-disclaimer">
                                                        <span data-bind="text: $data.carPriceQuote().OfferAndDealerAd.Offer.offerDetails.disclaimer"></span>
                                                    </div>
                                                    <!-- /ko -->
                                                    <ul class="tncData">
                                                        <li>The offer is subject to car availability at the dealerships</li>
                                                        <li>CarWale has brought this offer information on best effort basis and CarWale is not liable for any consequential (or otherwise) loss / damage caused by the information</li>
                                                        <li>All applicable terms and conditions would be as per the dealers and the same may change without any prior intimation</li>
                                                        <li>For the final price, offer availability and exact details of the offers, kindly contact the dealerships</li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- /ko -->
                        </div>
                        <!-- End of OEM offers -->

                    </div>
                    <div class="clear"></div>
                </section>


                <div class="clear"></div>
                <section class="bg-light-grey no-bg-color">
                    <div class="container">
                        <div class="pqBelowContainer pqBelowCar dealerlocator" id="pqBelowContainer" data-bind="foreach: priceQuoteCollection">
                            <div id="pq-below-container-car" class="inner-container price-quote-div light-shadow pq-below-container hide" data-bind=" attr: { 'PQId': carPriceQuote().EnId }">
                                <div class="grid-12" style="display: none">
                                    <div class="dfp_pq_ad"></div>
                                </div>

                                <!--ko 'if': $data.carPriceQuote().ShowSellCarLink == true -->
                                <div class="grid-12 margin-top20">
                                    <div class="content-box-shadow content-inner-block-20">
                                        <div class="inline-grid-10 vertical-middle">
                                            <span class="icon-wrapper-40 inline-block" data-role="impression-tracking" data-event="CWNonInteractive" data-cat="SellCarLink_Desk" data-action="Link" data-label="PQPage">
                                                <img src="https://imgd.aeplcdn.com/0x0/cw/static/icons/circle/sell-icon-40.png" alt="Sell car" width="40" height="40" />
                                            </span>
                                            <div class="inline-block padding-left20">
                                                <p class="font20 text-black text-bold inline-block">Sell Car</p>
                                                <ol class="list-type-numeric list-item-left inline-block font14">
                                                    <li>Fill Car Details</li>
                                                    <li>Fill Personal Details</li>
                                                    <li>Know Exact Valuation for your car and list</li>
                                                </ol>
                                            </div>
                                        </div>
                                        <div class="inline-grid-2 vertical-middle text-right">
                                            <a href="/used/sell/" data-role="click-tracking" data-event="CWInteractive" data-action="Link_Clicked" data-cat="SellCarLink_Desk" data-label="PQPage" target="_blank" class="font14">List Your Car for Free</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear"></div>
                                <!--/ko-->
                                <div class="grid-12">
                                    <!--ko 'if':$data.carPriceQuote().DealerShowroom != null -->
                                    <!-- ko 'if':$data.carPriceQuote().DealerShowroom.objDealerDetails != null  -->
                                    <div data-bind="template: { afterRender: showroomPostRender }">
                                        <h2 class="font20 margin-top30 margin-bottom10 special-skin-text">
                                            <span>Authorized Dealer : </span>
                                            <span id="divDealerName" class="dealername text-bold" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Name"></span>&nbsp;
                                            <!-- ko 'if':$data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile != null && $data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile != "" -->
                                            <span class="price-quote-all-imgs circle-call-icon"></span>
                                            <span class="calling-num special-skin-text" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile"></span>
                                            <!-- /ko -->
                                        </h2>
                                    </div>
                                    <div>
                                        <div class="cw-tabs-panel content-box-shadow">
                                            <div class="cw-tabs cw-tabs-flex cw-tabs-inner-margin10">
                                                <ul>
                                                    <li data-tabs="buying_assistance" class="active">Buying Assistance <span class="dlparrow dealer-sprite"></span></li>
                                                    <li data-tabs="contact_details" data-bind="attr: { 'PQId': carPriceQuote().EnId }" class="">Dealer Details</li>
                                                    <li data-tabs="gallery">Photo Gallery</li>
                                                </ul>
                                            </div>
                                            <span id="latitude" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Latitude" class="latitude hide"></span>
                                            <span id="longitude" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Longitude" class="longitude hide"></span>


                                            <div id="buying_assistance" class="cw-tabs-data">
                                                <div class="content-inner-block-10">
                                                    <!-- select services stsrts here-->
                                                    <div class="grid-12 selectservices buy-asstnc-services">
                                                        <h3 class="margin-bottom20">1. Select The Services You Want</h3>
                                                        <div class="select-services" style="border-bottom: 0px;">
                                                            <ul class="checkboxes" id="checkboxes">
                                                                <li onclick="toggleThisCheckbox(this)" class="checked" id="CompleteProductBrochure">Complete Product Brochure </li>
                                                                <li onclick="toggleThisCheckbox(this)" id="AvailabilityEnquiry">Availability Enquiry</li>
                                                                <li onclick="toggleThisCheckbox(this)" id="Door-stepTestDrive">Door-step Test Drive</li>
                                                                <li onclick="toggleThisCheckbox(this)" class="checked" id="Offer&amp;DiscountInformation">Offer &amp; Discount Information</li>
                                                                <li onclick="toggleThisCheckbox(this)" id="OtherAssistance">Other Assistance</li>
                                                            </ul>
                                                            <%-- <p class="font13 text-grey padding-bottom20">To avail all the services please provide your details so that our dealer partner can get in touch with you.</p>--%>
                                                        </div>
                                                    </div>
                                                    <!-- select services ends here-->
                                                    <!-- contact details stsrts here -->
                                                    <div class="grid-12 contactdetails margin-top10">
                                                        <h3 class="margin-bottom15">2. Provide Your Details To Get Help</h3>
                                                        <ul class="contact-details1 buy-asstnc-form" <%--style="width: 300px;"--%>>
                                                            <li>
                                                                <div class="form-control-box baNameField">
                                                                    <input id="custName" class="customername form-control" type="text" placeholder="Enter Your Name" maxlength="100">
                                                                    <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-name-icon hide" id="errNameIcon"></span>
                                                                    <div class="cw-blackbg-tooltip err-name-msg hide">Please enter your name</div>
                                                                </div>
                                                            </li>
                                                            <%--<li data-bind="if: ($data.carPriceQuote().SponsoredDealer.ShowEmail)">
                                                                <div class="form-control-box">
                                                                    <input id="custEmail" class="customeremail form-control" type="text" placeholder="Enter Your Email" maxlength="50">
                                                                    <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-email-icon hide"></span>
                                                                    <div class="cw-blackbg-tooltip hide err-email-msg">Please enter your email</div>
                                                                </div>
                                                            </li>--%>
                                                            <li>
                                                                <div class="form-control-box baMobileField">
                                                                    <span class="form-mobile-prefix">+91</span>
                                                                    <input id="custMobile" class="customermobile form-control padding-left40" type="text" placeholder="Enter your mobile number" maxlength="20">
                                                                    <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-mobile-icon hide"></span>
                                                                    <div class="cw-blackbg-tooltip hide err-mobile-msg">Please enter your mobile number</div>
                                                                </div>
                                                            </li>
                                                        </ul>
                                                        <div class="inline-block buy-asstnc-btn">
                                                            <input type="button" data-df-cwtccat="QuotationPage" data-df-cwtcact="DealerLocatorLeadSubmit" data-bind="attr: { 'data-df-cwtclbl': 'make:' + $data.carPriceQuote().carDetails.MakeName + '|model:' + $data.carPriceQuote().carDetails.ModelName + '|version:' + $data.carPriceQuote().carDetails.VersionName + '|city:' + $data.carPriceQuote().cityDetail.CityName }" onclick="    SubmitLead(this, false);" value="Submit" name="price" class="btn btn-orange" id="btnSubmitDealer">
                                                        </div>
                                                        <div class="clear"></div>
                                                    </div>
                                                    <div class="padding-left85">
                                                        <div class="thank-you-msg hide" data-bind="if: (!$data.carPriceQuote().SponsoredDealer.ShowEmail)">
                                                            <input type="hidden" id="encryptedResponse" />
                                                            <h2 class="tyHeading"></h2>
                                                            <div class="margin-top20 font14">
                                                                <p class="margin-bottom10">You can provide your email id below to receive the price-list,brochure and other collaterals over email.</p>
                                                                <div class="form-control-box grid-4 alpha omega position-rel">
                                                                    <input class="customeremail form-control" type="text" id="custEmailOptional" placeholder="Enter Your Email" maxlength="50">
                                                                    <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-email-icon hide"></span>
                                                                    <div class="cw-blackbg-tooltip hide err-email-msg">Please enter your email</div>
                                                                </div>
                                                                <div class="clear"></div>
                                                                <div class="margin-top10">
                                                                    <input type="button" class="red-btn leftfloat btn btn-orange back-btn" onclick="SubmitLead(this, true)" name="price" value="Done" style="width: 90px;">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="thank-you-msg hide" data-bind="if: ($data.carPriceQuote().SponsoredDealer.ShowEmail)">
                                                            <h2 class="tyHeading"></h2>
                                                            <div class="margin-top20 font14">
                                                                <p>The dealer would get in touch with you shortly with assistance on your car purchase.</p>
                                                                <!-- ko 'if':$data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile != null && $data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile != "" -->
                                                                <p class="margin-top10">You can also reach out to the dealer at</p>
                                                                <div class="dealer-mob-no margin-top5">
                                                                    <span class="dealer-sprite dealer-call-icon-red"></span>
                                                                    <span class="cw-nc-pg-sprite cw-nc-error-red-icon err-email-icon hide"></span>
                                                                    <div class="cw-blackbg-tooltip hide"></div>
                                                                    <p class="font20"><strong data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile"></strong></p>
                                                                    <div class="clear"></div>
                                                                </div>
                                                                <!-- /ko -->
                                                                <div class="margin-top10">
                                                                    <input type="button" class="red-btn leftfloat btn btn-orange back-btn" onclick="dealerShowroom.thankYouScreen.hideScreen(this)" name="price" value="Done" style="width: 90px;">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="clear"></div>
                                                    <!-- contact details ends here -->
                                                </div>
                                            </div>
                                            <div id="contact_details" class="cw-tabs-data hide">
                                                <div class="content-inner-block-10">
                                                    <div class="grid-6">
                                                        <ul class="dlp-cw-address" style="margin-top: 0px; padding-top: 0px; vertical-align: top; overflow: hidden">
                                                            <li>
                                                                <span class="dealer-sprite dlp-cw-search leftfloat margin-right5"></span>
                                                                <div id="address" class="leftfloat address" style="width: 350px;"><span data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Address"></span>, <span data-bind="    text: $data.carPriceQuote().DealerShowroom.objDealerDetails.CityName"></span>, <span data-bind="    text: $data.carPriceQuote().DealerShowroom.objDealerDetails.StateName"></span><span data-bind="    text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Pincode"></span></div>
                                                                <div class="clear"></div>
                                                            </li>
                                                            <!-- ko 'if':$data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile != null && $data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile != "" -->
                                                            <li>
                                                                <span class="dealer-sprite dlp-cw-call-icon-grey leftfloat margin-right5"></span>
                                                                <div id="mobileNo" class="leftfloat mobileno" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile"></div>
                                                                <div class="clear"></div>
                                                            </li>
                                                            <!-- /ko -->
                                                            <li>
                                                                <span class="dealer-sprite dlp-cw-email-icon-sm leftfloat margin-right5"></span>
                                                                <div id="dealerEmailId" class="leftfloat" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.EMailId"></div>
                                                                <div class="clear"></div>
                                                            </li>
                                                            <li>
                                                                <span class="dealer-sprite dlp-cw-clock-pic leftfloat margin-right5"></span>
                                                                <div class="leftfloat"><span data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.ShowroomStartTime"></span>&nbsp;to <span data-bind="    text: $data.carPriceQuote().DealerShowroom.objDealerDetails.ShowroomEndTime"></span></div>
                                                                <div class="clear"></div>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                    <div class="grid-6 map-hide">
                                                        <h4 class="margin-bottom10 hide map-text" style="font-size: 14px; font-weight: normal;">How to get there?</h4>
                                                        <div id="map-canvas" class="map-canvas" style="height: 231px;"></div>
                                                        <div class="clear"></div>
                                                    </div>
                                                    <div class="clear"></div>
                                                </div>
                                            </div>
                                            <div id="gallery" class="cw-tabs-data hide">
                                                <div class="content-inner-block-10 dealer-photo-gallery">

                                                    <ul data-bind="foreach: $data.carPriceQuote().DealerShowroom.objImageList">
                                                        <!-- ko 'if': originalImgPath != null -->
                                                        <li>
                                                            <a name="front1" class="cboxElement" title="" data-bind="attr: { 'href': hostUrl + '559x314' + originalImgPath }">
                                                                <img border="0" alt="" title="click to view" data-bind="attr: { 'src': hostUrl + '227x128' + originalImgPath }">
                                                            </a>
                                                        </li>
                                                        <!-- /ko -->
                                                    </ul>
                                                    <div class="clear"></div>
                                                    <!-- ko 'if':$data.carPriceQuote().DealerShowroom.objImageList.length == 0 || ($data.carPriceQuote().DealerShowroom.objImageList != null && $data.carPriceQuote().DealerShowroom.objImageList[0].originalImgPath == null) -->
                                                    <div class="temp-height">There are currently no images available for this dealership.</div>
                                                    <!-- /ko -->
                                                </div>
                                            </div>
                                        </div>
                                        <!-- /ko -->
                                        <!-- /ko -->
                                        <!--Dealer Locator code Starts Here-->
                                        <span id="DealerId" class="hide" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.DealerId"></span>
                                        <div>
                                            <div class="content-inner-block">
                                                <%--<h2>
                                                <div id="divDealerName" class="leftfloat margin-right5 dealername" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Name"></div>
                                                <div class="dealer-sprite dlp-cw-call-ico leftfloat margin-right5"></div>
                                                <div class="leftfloat" data-bind="text: $data.carPriceQuote().DealerShowroom.objDealerDetails.Mobile"></div>
                                                <div class="clear"></div>
                                            </h2>--%>
                                                <div class="clear"></div>
                                            </div>

                                            <div class="clear"></div>
                                        </div>
                                    </div>
                                    <!-- Dealer Locator code ends Here-->
                                    <!-- /ko -->
                                    <!-- /ko -->
                                    <!-- EMI Quote code Starts Here -->
                                    <!--ko 'if': carPriceQuote().PriceQuoteList.length > 1 -->
                                    <div class="emi-block">
                                        <h2 class="font20 margin-top20 margin-bottom10 special-skin-text">Indicative EMI Quote</h2>
                                        <div class="content-box-shadow content-inner-block-10">
                                            <div class="">
                                                <div class="dlp-cw-slider-box-l">
                                                    <div class="dlp-cw-slider-box">
                                                        <div>
                                                            <div class="dlp-cw-slider-left">
                                                                <h3>Down Payment</h3>
                                                                <div id="budget-range-down-pay"
                                                                    data-bind="attr: { sliderName: 'DownPayment' }, slider: downPayment, sliderOptions: { min: $data.minPayableAmount(), max: $data.onRoadPrice(), range: 'min', step: 1, change: sliderChangeTrack }, event: { click: sliderChangeTrack }">
                                                                </div>
                                                                <div class="dlp-cw-range-points">
                                                                    <ul>
                                                                        <li class="dealer-sprite dlp-slider-bar">
                                                                            <div class="dlp-slider-bar-first" data-bind="text: FormatFullPrice($data.minPayableAmount().toString(), $data.minPayableAmount().toString())"></div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 5%;">
                                                                            <div data-bind="text: formatPrice($data.exshowroom(), 0.25, $data.minPayableAmount())"></div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 10%;">
                                                                            <div data-bind="text: formatPrice($data.exshowroom(), 0.50, $data.minPayableAmount())"></div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 15%;">
                                                                            <div data-bind="text: formatPrice($data.exshowroom(), 0.75, $data.minPayableAmount())"></div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 20%;">
                                                                            <div data-bind="text: formatPrice($data.onRoadPrice(), 1)" class="dlp-slider-bar-last"></div>
                                                                        </li>
                                                                    </ul>
                                                                </div>

                                                            </div>
                                                            <div class="dlp-cw-slider-right">
                                                                <input type="hidden" id="hdnDownPayment" />
                                                                <span style="margin-right: 2px;">₹</span><span id="txtDownPayment" class="dlp-position-set" data-bind="text: formatNumeric($data.downPayment())"></span>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </div>
                                                        <div class="clear"></div>
                                                    </div>
                                                    <div class="dlp-cw-slider-box">
                                                        <div>
                                                            <div class="dlp-cw-slider-left">
                                                                <h3>Loan Amount</h3>
                                                                <div id="budget-range-loan"
                                                                    data-bind="attr: { sliderName: 'Loan' }, slider: Loan, sliderOptions: { min: 0, max: $data.exshowroom(), range: 'min', step: 1, change: sliderChangeTrack }, event: { click: sliderChangeTrack }">
                                                                </div>
                                                                <div class="dlp-cw-range-points">
                                                                    <ul>
                                                                        <li class="dealer-sprite dlp-slider-bar">
                                                                            <div class="dlp-slider-bar-first">0</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 5%;">
                                                                            <div data-bind="text: formatPrice($data.exshowroom(), 0.25)"></div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 10%;">
                                                                            <div data-bind="text: formatPrice($data.exshowroom(), 0.50)"></div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 15%;">
                                                                            <div data-bind="text: formatPrice($data.exshowroom(), 0.75)"></div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 20%;">
                                                                            <div data-bind="text: formatPrice($data.exshowroom(), 1)" class="dlp-slider-bar-last"></div>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                            <div class="dlp-cw-slider-right">
                                                                <input type="hidden" id="hdnLoanAmount" />
                                                                <span style="margin-right: 2px;">₹</span><span id="txtLoanAmount" class="dlp-position-set" data-bind="text: formatNumeric($data.Loan())"></span>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </div>
                                                        <div class="clear"></div>
                                                    </div>
                                                    <div class="dlp-cw-slider-box">
                                                        <div>
                                                            <div class="dlp-cw-slider-left">
                                                                <h3>Tenure</h3>
                                                                <div id="budget-range-tenure"
                                                                    data-bind="attr: { sliderName: 'Tenure' }, slider: tenure, sliderOptions: { min: 12, max: 84, range: 'min', step: 6, change: sliderChangeTrack }, event: { click: sliderChangeTrack }">
                                                                </div>
                                                                <div class="dlp-cw-range-points">
                                                                    <ul class="tenure">
                                                                        <li class="dealer-sprite dlp-slider-bar">
                                                                            <div class="dlp-slider-bar-first">1</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 3%;">
                                                                            <div>2</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 6%;">
                                                                            <div>3</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 8.2%;">
                                                                            <div>4</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 11%;">
                                                                            <div>5</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 13.3%;">
                                                                            <div>6</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 16.5%;">
                                                                            <div>7</div>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                            <div class="dlp-cw-slider-right">
                                                                <input type="hidden" id="hdnTenure" value="60" />
                                                                <span class="cw-sprite"></span><span id="txtTenure" class="dlp-position-set" data-bind="text: tenure" style="margin-right: 2px;"></span><span style="font-weight: normal;">Months</span>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </div>
                                                        <div class="clear"></div>
                                                    </div>
                                                    <div class="dlp-cw-slider-box" style="border-bottom: 0px; padding-bottom: 0px;">
                                                        <div>
                                                            <div class="dlp-cw-slider-left">
                                                                <h3>Interest Rate</h3>
                                                                <div id="budget-range-interest"
                                                                    data-bind="attr: { sliderName: 'InterestRate' }, slider: interestRate, sliderOptions: { min: 5, max: 15, range: 'min', step: 0.25, change: sliderChangeTrack }, event: { click: sliderChangeTrack }">
                                                                </div>
                                                                <div class="dlp-cw-range-points interest-rate-slider">
                                                                    <ul>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 0%;">
                                                                            <div class="dlp-slider-bar-first">5</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 5%;">
                                                                            <div style="margin-left: -3px;">7</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 10%;">
                                                                            <div style="margin-left: -3px;">9</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 15%;">
                                                                            <div style="margin-left: -7px;">11</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 20%;">
                                                                            <div style="margin-left: -7px;">13</div>
                                                                        </li>
                                                                        <li class="dealer-sprite dlp-slider-bar" style="left: 25%;">
                                                                            <div class="dlp-slider-bar-last">15</div>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                            <div class="dlp-cw-slider-right">
                                                                <input type="hidden" id="hdnInterestRate" value="12.5" />
                                                                <span class="cw-sprite" style="margin-right: 2px;"></span><span id="txtInterestRate" class="dlp-position-set" data-bind="text: interestRate"></span><span class="dlp-position-set">%</span>
                                                            </div>
                                                            <div class="clear"></div>
                                                        </div>
                                                        <div class="clear"></div>
                                                    </div>
                                                </div>
                                                <div class="dlp-cw-slider-box-r">
                                                    <div class="dlp-cw-indicative-head">Indicative EMI</div>
                                                    <div class="dlp-cw-indicative-rupee">
                                                        ₹
                                                        <span id="emiCalculate" data-bind="text: formatNumeric(monthlyEMI())"></span>
                                                    </div>
                                                    <div class="margin-top10">
                                                        <span class="text-bold font16">Per Month</span>
                                                    </div>
                                                    <!-- ko 'if': $data.carPriceQuote().SponsoredDealer != null -->
                                                    <!-- ko 'if': $data.carPriceQuote() != null && $data.carPriceQuote().SponsoredDealer.DealerId  != 0 -->
                                                    <div class="dlp-cw-indicative-btn">
                                                        <a id="dlp-cw-navigate" class="btn btn-orange"
                                                            data-cwtccat="QuotationPage" data-cwtcact="EMIDealerButtonClick"
                                                            data-bind="attr: { 'data-cwtclbl': 'make:' + $data.carPriceQuote().carDetails.MakeName + '|model:' + $data.carPriceQuote().carDetails.ModelName + '|version:' + $data.carPriceQuote().carDetails.VersionName + '|city:' + $data.carPriceQuote().cityDetail.CityName }">Get EMI Quote from Dealer</a>
                                                    </div>
                                                    <!-- /ko -->
                                                    <!-- /ko -->
                                                    <!-- ko 'if': $data.carPriceQuote() == null || $data.carPriceQuote().SponsoredDealer == null || $data.carPriceQuote().SponsoredDealer.DealerId  == 0 -->
                                                    <!-- ko 'if':$data.carPriceQuote().CampaignTemplates != null && $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.EmiCalculator] -->
                                                    <div data-bind="attr: { onload: PQ.advertisement.TrackImpressions('PQ_EmiCalculator_d', 'SponsoredLink_Shown', 'campaign_'+ $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.EmiCalculator].id) }">
                                                        <div class="margin-right5" data-role="click-tracking" data-event="CWInteractive" data-action="PQ_EmiCalculator_d" data-cat="SponsoredLink_Click"
                                                            data-bind="html: $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.EmiCalculator].name, attr: { 'data-label':'campaign_'+ $data.carPriceQuote().CampaignTemplates[PQ.pageProperties.EmiCalculator].id }">
                                                        </div>
                                                    </div>
                                                    <!-- /ko -->
                                                    <!-- /ko -->
                                                </div>
                                                <!-- /ko -->
                                                <div class="clear"></div>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                    </div>
                                    <!-- /ko -->
                                    <!-- EMI Quote code ends Here -->
                                    <div class="clear"></div>
                                    <div>
                                        <!-- ko 'if': carPriceQuote().AlternateCars.length>0 -->
                                        <h2 class="margin-top30 margin-bottom10 font20 special-skin-text"><span data-bind="text: $data.carPriceQuote().carDetails.MakeName"></span><span>&nbsp;</span><span data-bind="    text: $data.carPriceQuote().carDetails.ModelName"></span> Alternatives</h2>
                                        <div class="jcarousel-wrapper alternate-cars-carousel">
                                            <div class="jcarousel">
                                                <ul data-bind=" foreach: carPriceQuote().AlternateCars">
                                                    <li class="front">
                                                        <div class="contentWrapper">
                                                            <div class="imageWrapper">
                                                                <a target="_blank" data-cwtccat="QuotationPage" data-cwtcact="AlternateCarClick"
                                                                    data-bind="attr: { href: '/' + FormatSpecial(makeName) + '-cars/' + maskingName.toLowerCase() + '/', 'data-cwtclbl': 'make:' + $parent.carPriceQuote().carDetails.MakeName + '|model:' + $parent.carPriceQuote().carDetails.ModelName + '|version:' + $parent.carPriceQuote().carDetails.VersionName + '|city:' + $parent.carPriceQuote().cityDetail.CityName + '|AlternateCarMake:' + makeName + '|AlternateCarModel:' + modelName }">
                                                                    <img data-bind="attr: { src: HostUrl + '310x174' + ModelImageOriginal, 'title': makeName + ' ' + modelName }" alt="img title">
                                                                </a>
                                                            </div>
                                                            <div class="carDescWrapper">
                                                                <div class="carTitle margin-bottom20">
                                                                    <h3><a data-cwtccat="QuotationPage" data-cwtcact="AlternateCarClick"
                                                                        target="_blank" data-bind="attr: { href: '/' + FormatSpecial(makeName) + '-cars/' + maskingName.toLowerCase() + '/', 'title': makeName + ' ' + modelName, 'data-cwtclbl': 'make:' + $parent.carPriceQuote().carDetails.MakeName + '|model:' + $parent.carPriceQuote().carDetails.ModelName + '|version:' + $parent.carPriceQuote().carDetails.VersionName + '|city:' + $parent.carPriceQuote().cityDetail.CityName + '|AlternateCarMake:' + makeName + '|AlternateCarModel:' + modelName }"><span data-bind="    text: makeName" style="margin-right: 2px;"></span><span data-bind="    text: modelName"></span></a></h3>
                                                                </div>
                                                                <div class="margin-bottom10 font22 text-light-grey">
                                                                    ₹
                                                                    <span class="font22">&nbsp;<span data-bind="text: FormatFullPrice(PricesOverview.Price.toString(), PricesOverview.Price.toString())"></span>
                                                                        <!-- ko 'if': PricesOverview.PriceVersionCount > 1 -->
                                                                        <span>onwards</span>
                                                                        <!-- /ko -->
                                                                    </span>
                                                                </div>
                                                                <div class="font14 text-light-grey" data-bind="css: { oliveText: PricesOverview.PriceStatus == PriceBucket.PriceNotAvailable, redText: PricesOverview.PriceStatus == PriceBucket.CarNotSold }, text: PricesOverview.PriceStatus == PriceBucket.HaveUserCity ? (PricesOverview.PriceLabel + ', ' + PricesOverview.City.CityName) : PricesOverview.PriceLabel"></div>
                                                                <!-- ko 'if': PricesOverview.PriceStatus == PriceBucket.HaveUserCity -->
                                                                <div class="font14 text-light-grey cur-pointer">
                                                                    <a data-bind="attr: { modelid: modelId, 'data-cwtclbl': 'make:' + $parent.carPriceQuote().carDetails.MakeName + '|model:' + $parent.carPriceQuote().carDetails.ModelName + '|version:' + $parent.carPriceQuote().carDetails.VersionName + '|city:' + $parent.carPriceQuote().cityDetail.CityName + '|AlternateCarMake:' + makeName + '|AlternateCarModel:' + modelName }"
                                                                        data-cwtccat="QuotationPage" data-cwtcact="AlternateCarClick" class="quotation-alternate-car-link">View Price Breakup</a>
                                                                </div>
                                                                <!-- /ko -->
                                                            </div>
                                                        </div>
                                                    </li>
                                                </ul>

                                            </div>
                                            <!-- ko 'if': $data.carPriceQuote().AlternateCars.length>3 -->
                                            <span class="jcarousel-control-left"><a class="cwsprite jcarousel-control-prev cur-pointer"></a></span>
                                            <span class="jcarousel-control-right"><a class="cwsprite jcarousel-control-next cur-pointer"></a></span>
                                            <!-- /ko -->
                                        </div>
                                        <!-- /ko -->
                                    </div>
                                    <div class="clear"></div>
                                    <div class="margin-bottom10 margin-top10">
                                        <p class="special-skin-text">
                                            <strong class="font14">Disclaimer:</strong> CarWale takes utmost care in gathering precise and accurate information about car prices, financial quotes and promotional offers.  However, this information is only indicative and may not reflect the final price you may pay.
                            For more information please read <a style="font-size: 13px" href="/offerTermsAndConditions.aspx" target="_blank">terms and conditions</a><a href="/visitoragreement.aspx" target="_blank">, visitor agreement</a> and 
                                <a href="/privacypolicy.aspx" target="_blank">privacy policy</a>.
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="clear"></div>
                    </div>
                </section>
            </div>
        </section>
        <!-- Code for Grey Box Starts  -->
        <!--popup control-->
        
        <div id="pqPopup" class="hide info" style="float: left; width: 390px;">
            <div id="step1Container">
                <table class="tbl_pq_widget" width="100%" border="0">
                    <%--                    <tr>
                        <td colspan="2">
                            <div id="addCarError" style="color: #ce0001;"></div>
                        </td>
                    </tr>--%>
                    <tr id="Make" class="tbl_row">
                        <td>
                            <div class="form-control-box">
                                <input class="form-control ui-autocomplete-input blur" type="text" placeholder="<%=System.Configuration.ConfigurationManager.AppSettings["UpcommingCarPlaceholder"] ?? "Type to select car name"%>" id="addPQAutocomplete" autofocus>
                                <span class="cwsprite error-icon PriceCarErrIcn cityErrIcon hide"></span>
                                <div class="cw-blackbg-tooltip PriceCarErrMsg cityErrorMsg hide"></div>
                            </div>
                            <div id="suggestCars">
                                <!-- ko if: $data.length > 0 -->
                                <div class="form-control-box margin-top5 font12" style="margin-bottom: -10px; overflow: hidden; text-overflow: ellipsis; width: 455px; white-space: nowrap;">
                                    <span>Suggested: </span>
                                    <span class="bluetxtP">
                                        <!-- ko foreach: $data -->
                                        <span class="cursor" data-bind="text: $data.makeName + ' ' + $data.modelName, value: $data.modelId, click: fillAutoComplete"></span>
                                        <!-- ko if: $index()+1 < $parent.length -->
                                        ,
                                        <!-- /ko -->
                                        <!-- /ko -->
                                    </span>
                                </div>
                                <!-- /ko -->
                            </div>
                        </td>
                    </tr>
                    <tr id="City">
                        <td>
                            <div class="form-control-box margin-top20 margin-bottom20" id="addCarPopUp">
                                <div class="form-control-box city-input-box"></div>
                                <div class="form-control-box area-input-box"></div>
                            </div>
                        </td>
                    </tr>
                    <tr class="tbl_row">
                        <td>
                            <input id="btnAddCarSubmit" type="button" value="CHECK NOW" class="btn btn-orange text-uppercase">
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <!--T&C Start-->
        <br />
        <div id="Conditions-content" class="hide">
            <div class="Conditions-container">
            </div>
        </div>
        <!--T&C End-->
        <div class="hide" id="puneZoneContent">
            <p class="font12">Prices for Pune area vary according to the administrative unit that a car buyer resides in.</p>
            <p class="font12 margin-top10">PMC stands for Pune Municipal Corporation. PCMC stands for Pimpri-Chinchwad Municipal Corporation. Pune without LBT is all parts of Pune and surroundings which doesn't come under either of these corporations.</p>
        </div>
        <div class="hide" id="thaneZoneContent">
            <p class="font12">Prices for Thane area vary according to the administrative unit that a car buyer resides in.</p>
            <p class="font12 margin-top10">Parts of Thane city come under the LBT bracket. For users from all other parts, in and around Thane, the prices without LBT are applicable.</p>
        </div>
        <div class="clear"></div>
        <!-- #include file="/includes/footer.aspx" -->
        <div class="desktop-quotation">
            <!-- #include file="/includes/Slider.html" -->
        </div>
        
    </form>
    <!-- all other js plugins -->
    <!-- #include file="/includes/global/footer-script.aspx" -->
    <!-- #include file="/includes/NewCarsScripts.aspx" -->
    <script src="https://st.aeplcdn.com/v2/src/jquery.colorbox.js"></script>
    <script src="/static/src/graybox.js"></script>
    <script src="/static/src/dealershowroomdesktop.js"></script>
    <script src="/static/src/quotation.js"></script>
    <script src="/static/js/pricebreakup.js"></script>

    <script type="text/javascript">
        var LeadFormObj;
        Location.USERIP = "<%= Carwale.Utility.UserTracker.GetUserIp()%>";
        var nearByCityCount = '<%= ConfigurationManager.AppSettings["PQNearByCityCount"] ?? "" %>';
        var nonRecommendationsCampaignIds = "<%= ConfigurationManager.AppSettings["nonRecommendationsCampaignIds"] ?? "" %>".split(',');
        var LeadFormFlowCitiesArray = "<%= ConfigurationManager.AppSettings["multiLeadAssignmentABTestCities"] ?? "" %>".split(',');
        $(document).ready(function (e) {
            var areaId = Common.utils.getValueFromQS('areaId');
            var pqIds = Common.utils.getValueFromQS('pqid');

            if (pqIds != "") {
                // showImageLoading();
                bindCarOnRefresh();
            } else {
                applyKOBindings();
                if ($.cookie('_PQPageId') != "" && $.cookie('_PQPageId') != null) {
                    pageId = $.cookie('_PQPageId');
                    document.cookie = '_PQPageId=' + '' + '; expires = ' + permanentCookieTime() + "; domain=" + defaultCookieDomain + '; path =/';
                } else
                    pageId = pqPageId.PQOnPageLoad;

                if (!validatePQCookies())
                    window.location = '/new/prices.aspx';

                _viewModel.selectedAreaId = areaId;
                locationData = { cityId: $.cookie('_CustCityId'), cityName: $.cookie('_CustCity'), zoneId: $.cookie('_PQZoneId') };
                var pqData = { 'modelId': $.cookie('_PQModelId'), 'versionId': $.cookie('_PQVersionId'), 'location': locationData };
                PriceBreakUp.Quotation.setPQCityCookies(pqData);
                var pqRequest = getPQInputes(pageId);
                getNewPQ(pqRequest);
            }

            $("#btnAddCarSubmit").live("click", function () {             // on click of submit button of Add Car Popup
                $('#pqPopup').addClass('hide');
                if (locationPluginObj.validateLocation()) {
                    var locObj = locationPluginObj.getLocation();
                    PriceBreakUp.Quotation.setPQLocation(locObj);
                    _viewModel.selectedAreaId = locObj.areaId;
                    _viewModel.selectedAreaName = locObj.areaName;
                }
                var pqRequest = getPQInputes(pqPageId.PQByNewTab);
                getNewPQ(pqRequest);
                dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "NewTabAdded", lab: "Tab-No-" + getNoOfTabs() })
            });

            $('#pq-jcarousel').jcarousel({
                vertical: false
            });

            $(".stNext.stNav").live('click', function () {
                tabsCarousel.jcarousel('scroll', '+=2');
                dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "RightCarouselClick", lab: '' });
            });

            $(".stPrev.stNav").live('click', function () {
                tabsCarousel.jcarousel('scroll', '-=2')
                dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "LeftCarouselClick", lab: '' });
            });

            $('.nomoreTips').live('click', function () {
                Location.globalSearch.hideGlobalCityCoachmark();
                hideCoachmarks();
                Location.globalSearch.setCoachMarkCookie();
            });

            alternateCars();

            $('#pqTabs li').live('click', function () {        // on Tab Click vent
                if (!$(this).hasClass('quo-active')) {
                    hideCoachmarks();
                    cwTracking.trackCustomData('QuotationPage', 'CarToggle', 'make:' + $(this).attr("make") + '|model:' + $(this).attr("model") + '|version:' + $(this).attr("version") + '|city:' + $(this).attr("city"), false);
                }

                var pqId = this.getAttribute('pqid');
                hideShowCurrentTabPrice(pqId);
                cityVersionCookieChange(pqId);
                alternateCars();
                var pqIdsList = getParameterByName('pqid');
                var pqIds = pqIdsList.split(',');
                var lastpqId = pqIds[pqIds.length - 1];
                showHideContainers(pqId);
                UpdateActivetab(pqId);
                dataLayer.push({ event: 'PQ-Page-Tracking', cat: "PQ-Panel", act: "TabChange", lab: '' });
            });

            hideCoachmarksOnElementClick();
            $('html, body').animate({
                scrollTop: $("#orp-car-tab").offset().top - 50
            }, 1000);

            $("#crossSellDiv .quotation-cars a").live('click', function (e) {
                e.preventDefault();
                var href = $(this).attr("href");
                dataLayer.push({ event: 'PQ-Page-Tracking', cat: 'Cross-Sell-Unit', act: 'Image-Clicked', lab: $(this).attr('lab') });
                window.open(href, '_blank')
            })
            PQ.slider.registerEvents();
            PQ.advertisement.BankBazaarLink.registerEvents();
            PQ.advertisement.BhartiAxa.registerEvents();

            $(document).on('click', 'body', function (e) {
                var $variantDropdownDOM = $(".selectcustom-input-box-holder");
                if ($variantDropdownDOM != e.target && !$variantDropdownDOM.has(e.target).length) {
                    $('.selectcustom-content').hide();
                }
            });

            window.popup_promise = window.popup_promise || $.Deferred();
            window.popup_promise.resolve();
        });


    </script>

    <!-- Facebook Conversion Code for PQ - Carwale SEM 1 -->
    <script>
        (function () {
            var _fbq = window._fbq || (window._fbq = []);
            if (!_fbq.loaded) {
                var fbds = document.createElement('script');
                fbds.async = true;
                fbds.src = '//connect.facebook.net/en_US/fbds.js';
                var s = document.getElementsByTagName('script')[0];
                s.parentNode.insertBefore(fbds, s);
                _fbq.loaded = true;
            }
        })();
    </script>

    <noscript>
        <img height="1" width="1" alt="" style="display: none" src="https://www.facebook.com/tr?ev=6026520911635&amp;cd[value]=0.00&amp;cd[currency]=INR&amp;noscript=1" />
    </noscript>
    <div class="desktop-quotation">
        <script type="text/javascript" src="/static/js/lead-form-support.js" defer></script>     
        <script type="text/javascript" src="/static/m/js/lead-form.js" defer></script>
        <script type="text/javascript" src="/static/m/js/form-misc.js" defer></script>
        <script type="text/javascript" src="/static/js/location-plugin.js" defer></script>
        <!-- #include file="/static/m/ui/newCarLeadPopup.html" -->
    </div>
</body>
</html>
