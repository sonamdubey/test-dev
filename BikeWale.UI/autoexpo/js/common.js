﻿/*
* jQuery Tools 1.2.4 - The missing UI library for the Web
* 
* [scrollable, scrollable.navigator]
* 
* NO COPYRIGHTS OR LICENSES. DO WHAT YOU LIKE.
* 
* http://flowplayer.org/tools/
* 
* File generated: Mon Aug 30 09:30:05 GMT 2010
*/
(function (e) {
    function n(f, c) { var a = e(c); return a.length < 2 ? a : f.parent().find(c) } function t(f, c) {
        var a = this, l = f.add(a), g = f.children(), k = 0, m = c.vertical; j || (j = a); if (g.length > 1) g = e(c.items, f); e.extend(a, { getConf: function () { return c }, getIndex: function () { return k }, getSize: function () { return a.getItems().size() }, getNaviButtons: function () { return o.add(p) }, getRoot: function () { return f }, getItemWrap: function () { return g }, getItems: function () { return g.children(c.item).not("." + c.clonedClass) }, move: function (b, d) {
            return a.seekTo(k +
b, d)
        }, next: function (b) { return a.move(1, b) }, prev: function (b) { return a.move(-1, b) }, begin: function (b) { return a.seekTo(0, b) }, end: function (b) { return a.seekTo(a.getSize() - 1, b) }, focus: function () { return j = a }, addItem: function (b) { b = e(b); if (c.circular) { g.children("." + c.clonedClass + ":last").before(b); g.children("." + c.clonedClass + ":first").replaceWith(b.clone().addClass(c.clonedClass)) } else g.append(b); l.trigger("onAddItem", [b]); return a }, seekTo: function (b, d, h) {
            b.jquery || (b *= 1); if (c.circular && b === 0 && k == -1 && d !==
0) return a; if (!c.circular && b < 0 || b > a.getSize() || b < -1) return a; var i = b; if (b.jquery) b = a.getItems().index(b); else i = a.getItems().eq(b); var q = e.Event("onBeforeSeek"); if (!h) { l.trigger(q, [b, d]); if (q.isDefaultPrevented() || !i.length) return a } i = m ? { top: -i.position().top} : { left: -i.position().left }; k = b; j = a; if (d === undefined) d = c.speed; g.animate(i, d, c.easing, h || function () { l.trigger("onSeek", [b]) }); return a
        } 
        }); e.each(["onBeforeSeek", "onSeek", "onAddItem"], function (b, d) {
            e.isFunction(c[d]) && e(a).bind(d, c[d]); a[d] = function (h) {
                e(a).bind(d,
h); return a
            } 
        }); if (c.circular) { var r = a.getItems().slice(-1).clone().prependTo(g), s = a.getItems().eq(1).clone().appendTo(g); r.add(s).addClass(c.clonedClass); a.onBeforeSeek(function (b, d, h) { if (!b.isDefaultPrevented()) if (d == -1) { a.seekTo(r, h, function () { a.end(0) }); return b.preventDefault() } else d == a.getSize() && a.seekTo(s, h, function () { a.begin(0) }) }); a.seekTo(0, 0, function () { }) } var o = n(f, c.prev).click(function () { a.prev() }), p = n(f, c.next).click(function () { a.next() }); !c.circular && a.getSize() > 1 && a.onBeforeSeek(function (b,
d) { setTimeout(function () { if (!b.isDefaultPrevented()) { o.toggleClass(c.disabledClass, d <= 0); p.toggleClass(c.disabledClass, d >= a.getSize() - 1) } }, 1) }); c.mousewheel && e.fn.mousewheel && f.mousewheel(function (b, d) { if (c.mousewheel) { a.move(d < 0 ? 1 : -1, c.wheelSpeed || 50); return false } }); c.keyboard && e(document).bind("keydown.scrollable", function (b) {
    if (!(!c.keyboard || b.altKey || b.ctrlKey || e(b.target).is(":input"))) if (!(c.keyboard != "static" && j != a)) {
        var d = b.keyCode; if (m && (d == 38 || d == 40)) { a.move(d == 38 ? -1 : 1); return b.preventDefault() } if (!m &&
(d == 37 || d == 39)) { a.move(d == 37 ? -1 : 1); return b.preventDefault() } 
    } 
}); c.initialIndex && a.seekTo(c.initialIndex, 0, function () { })
    } e.tools = e.tools || { version: "1.2.4" }; e.tools.scrollable = { conf: { activeClass: "active", circular: false, clonedClass: "cloned", disabledClass: "disabled", easing: "swing", initialIndex: 0, item: null, items: ".items", keyboard: true, mousewheel: false, next: ".next", prev: ".prev", speed: 400, vertical: false, wheelSpeed: 0} }; var j; e.fn.scrollable = function (f) {
        var c = this.data("scrollable"); if (c) return c; f = e.extend({},
e.tools.scrollable.conf, f); this.each(function () { c = new t(e(this), f); e(this).data("scrollable", c) }); return f.api ? c : this
    } 
})(jQuery);
(function (d) {
    function p(b, g) { var h = d(g); return h.length < 2 ? h : b.parent().find(g) } var m = d.tools.scrollable; m.navigator = { conf: { navi: ".navi", naviItem: null, activeClass: "active", indexed: false, idPrefix: null, history: false} }; d.fn.navigator = function (b) {
        if (typeof b == "string") b = { navi: b }; b = d.extend({}, m.navigator.conf, b); var g; this.each(function () {
            function h(a, c, i) { e.seekTo(c); if (j) { if (location.hash) location.hash = a.attr("href").replace("#", "") } else return i.preventDefault() } function f() {
                return k.find(b.naviItem ||
"> *")
            } function n(a) { var c = d("<" + (b.naviItem || "a") + "/>").click(function (i) { h(d(this), a, i) }).attr("href", "#" + a); a === 0 && c.addClass(l); b.indexed && c.text(a + 1); b.idPrefix && c.attr("id", b.idPrefix + a); return c.appendTo(k) } function o(a, c) { a = f().eq(c.replace("#", "")); a.length || (a = f().filter("[href=" + c + "]")); a.click() } var e = d(this).data("scrollable"), k = b.navi.jquery ? b.navi : p(e.getRoot(), b.navi), q = e.getNaviButtons(), l = b.activeClass, j = b.history && d.fn.history; if (e) g = e; e.getNaviButtons = function () { return q.add(k) };
            f().length ? f().each(function (a) { d(this).click(function (c) { h(d(this), a, c) }) }) : d.each(e.getItems(), function (a) { n(a) }); e.onBeforeSeek(function (a, c) { setTimeout(function () { if (!a.isDefaultPrevented()) { var i = f().eq(c); !a.isDefaultPrevented() && i.length && f().removeClass(l).eq(c).addClass(l) } }, 1) }); e.onAddItem(function (a, c) { c = n(e.getItems().index(c)); j && c.history(o) }); j && f().history(o)
        }); return b.api ? g : this
    } 
})(jQuery);
