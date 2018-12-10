﻿"use strict";

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function MckCallingService(u, h, m, b, c, t, f, g, s, j) {
    var q = this; var n = $applozic("#mck-vid-icon"); var e = $applozic("#mck-sidebox"); q.callStartTime = t; q.mckMessageService = f; q.identity = u; q.token = h; q.callId = m; q.isCallHost = c; q.toUserImage = g; q.toUserDisplayName = b; q.isAudioCall = s; q.disconectedByHost; q.rejectedByReceiver; q.twilioService; console.log("is AudioCall:" + s); q.ringTone = j; var r = $applozic("#mck-msg-to"); var i = $applozic(".applozic-vid-container"); var l = $applozic("#mck-sidebox"); var k = $applozic("#mck-video-call-indicator"); var a = $applozic("#mck-unmute-icon"); var o = $applozic("#mck-mute-icon"); var p = $applozic("#mck-microfone-mute-btn"); $applozic("#mck-microfone-mute-btn").off("click").on("click", function () {
        var v = r.val(); var y = q.twilioService.activeRoom; if (y) {
            var x = y.localParticipant.media; if (x.isMuted) {
                a.addClass("vis").removeClass("n-vis"); o.addClass("n-vis").removeClass("vis"); p.addClass("mck-unmuted").removeClass("mck-muted"); x.unmute();
            } else {
                o.addClass("vis").removeClass("n-vis"); a.addClass("n-vis").removeClass("vis"); p.addClass("mck-muted").removeClass("mck-unmuted"); x.mute();
            }
        }
    }); $applozic("#mck-vid-disconnect").off("click").on("click", function () {
        var v = r.val(); if (c) {
            q.disconectedByHost = true; if (q.twilioService.callReceivedAt) {
                var x = new Date().getTime() - q.twilioService.callReceivedAt.getTime(); alMessageService.sendVideoCallEndMessage(m, "CALL_END", 103, false, x, v, function (y) {
                    f.sendMessage(y);
                });
            } else {
                q.ringTone.stop(); alMessageService.sendVideoCallMessage(m, "CALL_MISSED", 103, false, v, function (y) {
                    f.sendMessage(y);
                });
            }
        } q.twilioService.leaveRoomIfJoined(); q.removeTracks(["#mck-vid-media > video", "#mck-vid-media > audio", "#local-media > video", "#local-media > audio"]); i.addClass("n-vis").removeClass("vis"); k.addClass("n-vis").removeClass("vis");
    }); q.startVideoCall = function () {
        if (!navigator.webkitGetUserMedia && !navigator.mozGetUserMedia) {
            alert("WebRTC is not available in your browser. can not start video call");
        } if (!q.token) {
            alert("missing token.. can not make video call"); i.removeClass("vis").addClass("n-vis");
        } q.twilioService = new TwilioService(q.identity, q.token, q.callId, q.ringTone, q.isAudioCall, q.mckMessageService, q.isCallHost); var v = q.twilioService.InitializeVideoClient(); if (c) {
            n.html(g + "<span> Calling " + b + "</span>"); n.removeClass("n-vis").addClass("vis"); setTimeout(function () {
                if (!q.twilioService.isCallReceived && !q.disconectedByHost && !q.rejectedByReceiver) {
                    console.log("call is not answered...."); q.ringTone.stop(); alMessageService.sendVideoCallMessage(m, "CALL_MISSED", 103, s, toUser, function (x) {
                        f.sendMessage(x);
                    }); alert(b + " not Available...."); q.twilioService.leaveRoomIfJoined(); q.removeTracks(["#mck-vid-media > video", "#mck-vid-media > audio", "#local-media > video", "#local-media > audio"]); i.addClass("n-vis").removeClass("vis");
                }
            }, 70000);
        } window.addEventListener("beforeunload", q.twilioService.leaveRoomIfJoined); q.twilioService.attachUserMedia(c, s); q.twilioService.joinCall(s); i.removeClass("n-vis").addClass("vis"); a.addClass("vis").removeClass("n-vis"); o.addClass("n-vis").removeClass("vis"); p.addClass("mck-unmuted").removeClass("mck-muted");
    }; q.removeTracks = function (v) {
        $applozic.each(v, function (x, y) {
            $applozic(y).remove();
        });
    };
} function TwilioService(t, g, l, j, s, e, c) {
    var p = this; p.identity = t; p.token = g; p.callId = l; p.videoClient; p.activeRoom; p.previewMedia; p.ringTone = j; p.isCallReceived; p.callReceivedAt; p.isAudioCall = s; p.mckMessageService = e; p.isCallHost = c; var r = $applozic("#mck-msg-to"); var m = $applozic("#mck-vid-icon"); var a = "#local-media"; var f = "#mck-vid-media"; var b = $applozic("#local-media > video"); var q = $applozic("#mck-vid-media > video"); var o = $applozic("#local-media"); var h = $applozic(".applozic-vid-container"); var k = $applozic("#mck-sidebox"); var i = $applozic("#mck-video-call-indicator"); p.InitializeVideoClient = function () {
        p.videoClient = new Twilio.Video.Client(p.token);
    }; p.joinCall = function (u) {
        if (p.videoClient) {
            p.videoClient.connect({ to: p.callId }).then(p.roomJoined, function (v) {
                console.log("Could not connect to Twilio: " + v.message);
            });
        }
    }; p.attachUserMedia = function (v, u) {
        if (!p.previewMedia) {
            p.previewMedia = new Twilio.Video.LocalMedia(); Twilio.Video.getUserMedia().then(function (x) {
                p.previewMedia.addStream(x); if (v) {
                    p.previewMedia.attach(f); p.ringTone.play();
                } else {
                    p.previewMedia.attach(a); o.removeClass("n-vis").addClass("vis");
                }
            }, function (x) {
                console.error("Unable to access local media", x); console.log("Unable to access Camera and Microphone");
            });
        }
    }; p.roomJoined = function (u) {
        console.log("room detail : " + u); p.activeRoom = u; if (!p.previewMedia) {
            u.localParticipant.media.attach(f);
        } u.participants.forEach(function (v) {
            console.log("Already in Room: '" + v.identity + "'"); v.media.attach("#mck-vid-media");
        }); u.on("participantConnected", function (v) {
            console.log("participent- " + v.identity + "connected to the room- " + u); p.isCallReceived = true; p.callReceivedAt = new Date(); console.log("callReceivedAt : " + p.callReceivedAt); p.ringTone.stop(); m.addClass("n-vis"); $applozic("#mck-vid-media > video").remove(); $applozic("#mck-vid-media > audio").remove(); u.localParticipant.media.attach(a); o.removeClass("n-vis").addClass("vis"); v.media.attach(f);
        }); u.on("participantDisconnected", function (v) {
            console.log("Participant '" + v.identity + "' left the room"); if (p.isCallHost) {
                var x = r.val(); var y = new Date().getTime(); p.callReceivedAt.getTime(); alMessageService.sendVideoCallEndMessage(l, "CALL_END", 103, s, y, x, function (z) {
                    e.sendMessage(z);
                });
            } v.media.detach(); p.leaveRoomIfJoined(); h.addClass("n-vis").removeClass("vis"); i.addClass("n-vis").removeClass("vis");
        }); u.on("disconnected", function () {
            console.log("Left"); u.localParticipant.media.detach(); u.participants.forEach(function (v) {
                v.media.detach();
            }); p.activeRoom = null;
        });
    }; p.leaveRoomIfJoined = function n() {
        if (p.activeRoom) {
            p.activeRoom.disconnect();
        } if (p.previewMedia) {
            p.previewMedia.stop();
        }
    };
} !function (a) {
    if ("object" == (typeof exports === "undefined" ? "undefined" : _typeof(exports)) && "undefined" != typeof module) {
        module.exports = a();
    } else {
        if ("function" == typeof define && define.amd) {
            define([], a);
        } else {
            var b; "undefined" != typeof window ? b = window : "undefined" != typeof global ? b = global : "undefined" != typeof self && (b = self), b.SockJS = a();
        }
    }
}(function () {
    var a; return function b(e, j, g) {
        function c(n, k) {
            if (!j[n]) {
                if (!e[n]) {
                    var m = "function" == typeof require && require; if (!k && m) {
                        return m(n, !0);
                    } if (h) {
                        return h(n, !0);
                    } var o = new Error("Cannot find module '" + n + "'"); throw o.code = "MODULE_NOT_FOUND", o;
                } var i = j[n] = { exports: {} }; e[n][0].call(i.exports, function (l) {
                    var p = e[n][1][l]; return c(p ? p : l);
                }, i, i.exports, b, e, j, g);
            } return j[n].exports;
        } for (var h = "function" == typeof require && require, f = 0; f < g.length; f++) {
            c(g[f]);
        } return c;
    }({
        1: [function (c, f) {
            (function (g) {
                var e = c("./transport-list"); f.exports = c("./main")(e), "_sockjs_onload" in g && setTimeout(g._sockjs_onload, 1);
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "./main": 14, "./transport-list": 16 }], 2: [function (f, h) {
            function j() {
                c.call(this), this.initEvent("close", !1, !1), this.wasClean = !1, this.code = 0, this.reason = "";
            } var g = f("inherits"),
                c = f("./event"); g(j, c), h.exports = j;
        }, { "./event": 4, inherits: 54 }], 3: [function (f, h) {
            function j() {
                c.call(this);
            } var g = f("inherits"),
                c = f("./eventtarget"); g(j, c), j.prototype.removeAllListeners = function (e) {
                    e ? delete this._listeners[e] : this._listeners = {};
                }, j.prototype.once = function (l, o) {
                    function p() {
                        m.removeListener(l, p), k || (k = !0, o.apply(this, arguments));
                    } var m = this,
                        k = !1; this.on(l, p);
                }, j.prototype.emit = function (i) {
                    var l = this._listeners[i]; if (l) {
                        for (var m = Array.prototype.slice.call(arguments, 1), k = 0; k < l.length; k++) {
                            l[k].apply(this, m);
                        }
                    }
                }, j.prototype.on = j.prototype.addListener = c.prototype.addEventListener, j.prototype.removeListener = c.prototype.removeEventListener, h.exports.EventEmitter = j;
        }, { "./eventtarget": 5, inherits: 54 }], 4: [function (c, f) {
            function g(e) {
                this.type = e;
            } g.prototype.initEvent = function (h, i, j) {
                return this.type = h, this.bubbles = i, this.cancelable = j, this.timeStamp = +new Date(), this;
            }, g.prototype.stopPropagation = function () { }, g.prototype.preventDefault = function () { }, g.CAPTURING_PHASE = 1, g.AT_TARGET = 2, g.BUBBLING_PHASE = 3, f.exports = g;
        }, {}], 5: [function (c, f) {
            function g() {
                this._listeners = {};
            } g.prototype.addEventListener = function (h, i) {
                h in this._listeners || (this._listeners[h] = []); var j = this._listeners[h]; -1 === j.indexOf(i) && (j = j.concat([i])), this._listeners[h] = j;
            }, g.prototype.removeEventListener = function (h, j) {
                var k = this._listeners[h]; if (k) {
                    var i = k.indexOf(j); return -1 !== i ? void (k.length > 1 ? this._listeners[h] = k.slice(0, i).concat(k.slice(i + 1)) : delete this._listeners[h]) : void 0;
                }
            }, g.prototype.dispatchEvent = function (j) {
                var l = j.type,
                    m = Array.prototype.slice.call(arguments, 0); if (this["on" + l] && this["on" + l].apply(this, m), l in this._listeners) {
                        for (var k = this._listeners[l], h = 0; h < k.length; h++) {
                            k[h].apply(this, m);
                        }
                    }
            }, f.exports = g;
        }, {}], 6: [function (f, h) {
            function j(e) {
                c.call(this), this.initEvent("message", !1, !1), this.data = e;
            } var g = f("inherits"),
                c = f("./event"); g(j, c), h.exports = j;
        }, { "./event": 4, inherits: 54 }], 7: [function (f, h) {
            function j(e) {
                this._transport = e, e.on("message", this._transportMessage.bind(this)), e.on("close", this._transportClose.bind(this));
            } var g = f("json3"),
                c = f("./utils/iframe"); j.prototype._transportClose = function (i, k) {
                    c.postMessage("c", g.stringify([i, k]));
                }, j.prototype._transportMessage = function (e) {
                    c.postMessage("t", e);
                }, j.prototype._send = function (e) {
                    this._transport.send(e);
                }, j.prototype._close = function () {
                    this._transport.close(), this._transport.removeAllListeners();
                }, h.exports = j;
        }, { "./utils/iframe": 47, json3: 55 }], 8: [function (m, j) {
            var g = m("./utils/url"),
                c = m("./utils/event"),
                h = m("json3"),
                f = m("./facade"),
                p = m("./info-iframe-receiver"),
                k = m("./utils/iframe"),
                l = m("./location"); j.exports = function (n, o) {
                    var q = {}; o.forEach(function (e) {
                        e.facadeTransport && (q[e.facadeTransport.transportName] = e.facadeTransport);
                    }), q[p.transportName] = p; var i; n.bootstrap_iframe = function () {
                        var t; k.currentWindowId = l.hash.slice(1); var r = function r(z) {
                            if (z.source === parent && ("undefined" == typeof i && (i = z.origin), z.origin === i)) {
                                var y; try {
                                    y = h.parse(z.data);
                                } catch (A) {
                                    return;
                                } if (y.windowId === k.currentWindowId) {
                                    switch (y.type) {
                                        case "s":
                                            var x; try {
                                                x = h.parse(y.data);
                                            } catch (A) {
                                                break;
                                            } var C = x[0],
                                                B = x[1],
                                                u = x[2],
                                                e = x[3]; if (C !== n.version) {
                                                    throw new Error('Incompatibile SockJS! Main site uses: "' + C + '", the iframe: "' + n.version + '".');
                                                } if (!g.isOriginEqual(u, l.href) || !g.isOriginEqual(e, l.href)) {
                                                    throw new Error("Can't connect to different domain from within an iframe. (" + l.href + ", " + u + ", " + e + ")");
                                                } t = new f(new q[B](u, e)); break; case "m":
                                                    t._send(y.data); break; case "c":
                                                        t && t._close(), t = null;
                                    }
                                }
                            }
                        }; c.attachEvent("message", r), k.postMessage("s");
                    };
                };
        }, { "./facade": 7, "./info-iframe-receiver": 10, "./location": 13, "./utils/event": 46, "./utils/iframe": 47, "./utils/url": 52, debug: void 0, json3: 55 }], 9: [function (f, j) {
            function l(o, p) {
                h.call(this); var q = this,
                    m = +new Date(); this.xo = new p("GET", o), this.xo.once("finish", function (s, x) {
                        var v, i; if (200 === s) {
                            if (i = +new Date() - m, x) {
                                try {
                                    v = k.parse(x);
                                } catch (n) { }
                            } g.isObject(v) || (v = {});
                        } q.emit("finish", v, i), q.removeAllListeners();
                    });
            } var h = f("events").EventEmitter,
                c = f("inherits"),
                k = f("json3"),
                g = f("./utils/object"); c(l, h), l.prototype.close = function () {
                    this.removeAllListeners(), this.xo.close();
                }, j.exports = l;
        }, { "./utils/object": 49, debug: void 0, events: 3, inherits: 54, json3: 55 }], 10: [function (g, k) {
            function m(i) {
                var n = this; f.call(this), this.ir = new c(i, h), this.ir.once("finish", function (e, o) {
                    n.ir = null, n.emit("message", l.stringify([e, o]));
                });
            } var j = g("inherits"),
                f = g("events").EventEmitter,
                l = g("json3"),
                h = g("./transport/sender/xhr-local"),
                c = g("./info-ajax"); j(m, f), m.transportName = "iframe-info-receiver", m.prototype.close = function () {
                    this.ir && (this.ir.close(), this.ir = null), this.removeAllListeners();
                }, k.exports = m;
        }, { "./info-ajax": 9, "./transport/sender/xhr-local": 37, events: 3, inherits: 54, json3: 55 }], 11: [function (c, f) {
            (function (p) {
                function k(i, q) {
                    var n = this; h.call(this); var s = function s() {
                        var o = n.ifr = new g(m.transportName, q, i); o.once("message", function (u) {
                            if (u) {
                                var v; try {
                                    v = j.parse(u);
                                } catch (y) {
                                    return n.emit("finish"), void n.close();
                                } var r = v[0],
                                    x = v[1]; n.emit("finish", r, x);
                            } n.close();
                        }), o.once("close", function () {
                            n.emit("finish"), n.close();
                        });
                    }; p.document.body ? s() : e.attachEvent("load", s);
                } var h = c("events").EventEmitter,
                    l = c("inherits"),
                    j = c("json3"),
                    e = c("./utils/event"),
                    g = c("./transport/iframe"),
                    m = c("./info-iframe-receiver"); l(k, h), k.enabled = function () {
                        return g.enabled();
                    }, k.prototype.close = function () {
                        this.ifr && this.ifr.close(), this.removeAllListeners(), this.ifr = null;
                    }, f.exports = k;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "./info-iframe-receiver": 10, "./transport/iframe": 22, "./utils/event": 46, debug: void 0, events: 3, inherits: 54, json3: 55 }], 12: [function (z, q) {
            function j(c, f) {
                var i = this; g.call(this), setTimeout(function () {
                    i.doXhr(c, f);
                }, 0);
            } var g = z("events").EventEmitter,
                m = z("inherits"),
                h = z("./utils/url"),
                A = z("./transport/sender/xdr"),
                x = z("./transport/sender/xhr-cors"),
                y = z("./transport/sender/xhr-local"),
                v = z("./transport/sender/xhr-fake"),
                k = z("./info-iframe"),
                p = z("./info-ajax"); m(j, g), j._getReceiver = function (c, f, i) {
                    return i.sameOrigin ? new p(f, y) : x.enabled ? new p(f, x) : A.enabled && i.sameScheme ? new p(f, A) : k.enabled() ? new k(c, f) : new p(f, v);
                }, j.prototype.doXhr = function (f, n) {
                    var l = this,
                        c = h.addPath(f, "/info"); this.xo = j._getReceiver(f, c, n), this.timeoutRef = setTimeout(function () {
                            l._cleanup(!1), l.emit("finish");
                        }, j.timeout), this.xo.once("finish", function (i, o) {
                            l._cleanup(!0), l.emit("finish", i, o);
                        });
                }, j.prototype._cleanup = function (c) {
                    clearTimeout(this.timeoutRef), this.timeoutRef = null, !c && this.xo && this.xo.close(), this.xo = null;
                }, j.prototype.close = function () {
                    this.removeAllListeners(), this._cleanup(!1);
                }, j.timeout = 8000, q.exports = j;
        }, { "./info-ajax": 9, "./info-iframe": 11, "./transport/sender/xdr": 34, "./transport/sender/xhr-cors": 35, "./transport/sender/xhr-fake": 36, "./transport/sender/xhr-local": 37, "./utils/url": 52, debug: void 0, events: 3, inherits: 54 }], 13: [function (c, f) {
            (function (e) {
                f.exports = e.location || { origin: "http://localhost:80", protocol: "http", host: "localhost", port: 80, href: "http://localhost/", hash: "" };
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, {}], 14: [function (c, f) {
            (function (D) {
                function A(r, s, x) {
                    if (!(this instanceof A)) {
                        return new A(r, s, x);
                    } if (arguments.length < 1) {
                        throw new TypeError("Failed to construct 'SockJS: 1 argument required, but only 0 present");
                    } e.call(this), this.readyState = A.CONNECTING, this.extensions = "", this.protocol = "", x = x || {}, x.protocols_whitelist && E.warn("'protocols_whitelist' is DEPRECATED. Use 'transports' instead."), this._transportsWhitelist = x.transports; var p = x.sessionId || 8; if ("function" == typeof p) {
                        this._generateSessionId = p;
                    } else {
                        if ("number" != typeof p) {
                            throw new TypeError("If sessionId is used in the options, it needs to be a number or a function.");
                        } this._generateSessionId = function () {
                            return L.string(p);
                        };
                    } this._server = x.server || L.numberString(1000); var v = new z(r); if (!v.host || !v.protocol) {
                        throw new SyntaxError("The URL '" + r + "' is invalid");
                    } if (v.hash) {
                        throw new SyntaxError("The URL must not contain a fragment");
                    } if ("http:" !== v.protocol && "https:" !== v.protocol) {
                        throw new SyntaxError("The URL's scheme must be either 'http:' or 'https:'. '" + v.protocol + "' is not allowed.");
                    } var h = "https:" === v.protocol; if ("https" === I.protocol && !h) {
                        throw new Error("SecurityError: An insecure SockJS connection may not be initiated from a page loaded over HTTPS");
                    } s ? Array.isArray(s) || (s = [s]) : s = []; var m = s.sort(); m.forEach(function (i, l) {
                        if (!i) {
                            throw new SyntaxError("The protocols entry '" + i + "' is invalid.");
                        } if (l < m.length - 1 && i === m[l + 1]) {
                            throw new SyntaxError("The protocols entry '" + i + "' is duplicated.");
                        }
                    }); var g = J.getOrigin(I.href); this._origin = g ? g.toLowerCase() : null, v.set("pathname", v.pathname.replace(/\/+$/, "")), this.url = v.href, this._urlInfo = { nullOrigin: !q.hasDomain(), sameOrigin: J.isOriginEqual(this.url, I.href), sameScheme: J.isSchemeEqual(this.url, I.href) }, this._ir = new O(this.url, this._urlInfo), this._ir.once("finish", this._receiveInfo.bind(this));
                } function G(g) {
                    return 1000 === g || g >= 3000 && 4999 >= g;
                } c("./shims"); var C,
                    z = c("url-parse"),
                    N = c("inherits"),
                    t = c("json3"),
                    L = c("./utils/random"),
                    F = c("./utils/escape"),
                    J = c("./utils/url"),
                    H = c("./utils/event"),
                    K = c("./utils/transport"),
                    B = c("./utils/object"),
                    q = c("./utils/browser"),
                    E = c("./utils/log"),
                    M = c("./event/event"),
                    e = c("./event/eventtarget"),
                    I = c("./location"),
                    k = c("./event/close"),
                    j = c("./event/trans-message"),
                    O = c("./info-receiver"); N(A, e), A.prototype.close = function (g, h) {
                        if (g && !G(g)) {
                            throw new Error("InvalidAccessError: Invalid code");
                        } if (h && h.length > 123) {
                            throw new SyntaxError("reason argument has an invalid length");
                        } if (this.readyState !== A.CLOSING && this.readyState !== A.CLOSED) {
                            var i = !0; this._close(g || 1000, h || "Normal closure", i);
                        }
                    }, A.prototype.send = function (g) {
                        if ("string" != typeof g && (g = "" + g), this.readyState === A.CONNECTING) {
                            throw new Error("InvalidStateError: The connection has not been established yet");
                        } this.readyState === A.OPEN && this._transport.send(F.quote(g));
                    }, A.version = c("./version"), A.CONNECTING = 0, A.OPEN = 1, A.CLOSING = 2, A.CLOSED = 3, A.prototype._receiveInfo = function (g, h) {
                        if (this._ir = null, !g) {
                            return void this._close(1002, "Cannot connect to server");
                        } this._rto = this.countRTO(h), this._transUrl = g.base_url ? g.base_url : this.url, g = B.extend(g, this._urlInfo); var i = C.filterToEnabled(this._transportsWhitelist, g); this._transports = i.main, this._connect();
                    }, A.prototype._connect = function () {
                        for (var h = this._transports.shift() ; h; h = this._transports.shift()) {
                            if (h.needBody && (!D.document.body || "undefined" != typeof D.document.readyState && "complete" !== D.document.readyState && "interactive" !== D.document.readyState)) {
                                return this._transports.unshift(h), void H.attachEvent("load", this._connect.bind(this));
                            } var m = this._rto * h.roundTrips || 5000; this._transportTimeoutId = setTimeout(this._transportTimeout.bind(this), m); var l = J.addPath(this._transUrl, "/" + this._server + "/" + this._generateSessionId()),
                                g = new h(l, this._transUrl); return g.on("message", this._transportMessage.bind(this)), g.once("close", this._transportClose.bind(this)), g.transportName = h.transportName, void (this._transport = g);
                        } this._close(2000, "All transports failed", !1);
                    }, A.prototype._transportTimeout = function () {
                        this.readyState === A.CONNECTING && this._transportClose(2007, "Transport timed out");
                    }, A.prototype._transportMessage = function (h) {
                        var m,
                            s = this,
                            l = h.slice(0, 1),
                            g = h.slice(1); switch (l) {
                                case "o":
                                    return void this._open(); case "h":
                                        return void this.dispatchEvent(new M("heartbeat"));
                            } if (g) {
                                try {
                                    m = t.parse(g);
                                } catch (p) { }
                            } if ("undefined" != typeof m) {
                                switch (l) {
                                    case "a":
                                        Array.isArray(m) && m.forEach(function (i) {
                                            s.dispatchEvent(new j(i));
                                        }); break; case "m":
                                            this.dispatchEvent(new j(m)); break; case "c":
                                                Array.isArray(m) && 2 === m.length && this._close(m[0], m[1], !0);
                                }
                            }
                    }, A.prototype._transportClose = function (g, h) {
                        return this._transport && (this._transport.removeAllListeners(), this._transport = null, this.transport = null), G(g) || 2000 === g || this.readyState !== A.CONNECTING ? void this._close(g, h) : void this._connect();
                    }, A.prototype._open = function () {
                        this.readyState === A.CONNECTING ? (this._transportTimeoutId && (clearTimeout(this._transportTimeoutId), this._transportTimeoutId = null), this.readyState = A.OPEN, this.transport = this._transport.transportName, this.dispatchEvent(new M("open"))) : this._close(1006, "Server lost session");
                    }, A.prototype._close = function (h, l, m) {
                        var g = !1; if (this._ir && (g = !0, this._ir.close(), this._ir = null), this._transport && (this._transport.close(), this._transport = null, this.transport = null), this.readyState === A.CLOSED) {
                            throw new Error("InvalidStateError: SockJS has already been closed");
                        } this.readyState = A.CLOSING, setTimeout(function () {
                            this.readyState = A.CLOSED, g && this.dispatchEvent(new M("error")); var i = new k("close"); i.wasClean = m || !1, i.code = h || 1000, i.reason = l, this.dispatchEvent(i), this.onmessage = this.onclose = this.onerror = null;
                        }.bind(this), 0);
                    }, A.prototype.countRTO = function (g) {
                        return g > 100 ? 4 * g : 300 + g;
                    }, f.exports = function (g) {
                        return C = K(g), c("./iframe-bootstrap")(A, g), A;
                    };
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "./event/close": 2, "./event/event": 4, "./event/eventtarget": 5, "./event/trans-message": 6, "./iframe-bootstrap": 8, "./info-receiver": 12, "./location": 13, "./shims": 15, "./utils/browser": 44, "./utils/escape": 45, "./utils/event": 46, "./utils/log": 48, "./utils/object": 49, "./utils/random": 50, "./utils/transport": 51, "./utils/url": 52, "./version": 53, debug: void 0, inherits: 54, json3: 55, "url-parse": 56 }], 15: [function () {
            function I(c) {
                var f = +c; return f !== f ? f = 0 : 0 !== f && f !== 1 / 0 && f !== -(1 / 0) && (f = (f > 0 || -1) * Math.floor(Math.abs(f))), f;
            } function Z(c) {
                return c >>> 0;
            } function P() { } var L,
                V = Array.prototype,
                N = Object.prototype,
                J = Function.prototype,
                ad = String.prototype,
                G = V.slice,
                ab = N.toString,
                R = function R(c) {
                    return "[object Function]" === N.toString.call(c);
                },
                Y = function Y(c) {
                    return "[object Array]" === ab.call(c);
                },
                W = function W(c) {
                    return "[object String]" === ab.call(c);
                },
                aa = Object.defineProperty && function () {
                    try {
                        return Object.defineProperty({}, "x", {}), !0;
                    } catch (c) {
                        return !1;
                    }
                }(); L = aa ? function (c, g, h, f) {
                    !f && g in c || Object.defineProperty(c, g, { configurable: !0, enumerable: !1, writable: !0, value: h });
                } : function (c, g, h, f) {
                    !f && g in c || (c[g] = h);
                }; var M = function M(f, g, h) {
                    for (var c in g) {
                        N.hasOwnProperty.call(g, c) && L(f, c, g[c], h);
                    }
                },
                    F = function F(c) {
                        if (null == c) {
                            throw new TypeError("can't convert " + c + " to object");
                        } return Object(c);
                    }; M(J, {
                        bind: function bind(h) {
                            var m = this; if (!R(m)) {
                                throw new TypeError("Function.prototype.bind called on incompatible " + m);
                            } for (var l = G.call(arguments, 1), g = function g() {
                              if (this instanceof p) {
                                var c = m.apply(this, l.concat(G.call(arguments))); return Object(c) === c ? c : this;
                            } return m.apply(h, l.concat(G.call(arguments)));
                            }, n = Math.max(0, m.length - l.length), j = [], f = 0; n > f; f++) {
                                j.push("$" + f);
                            } var p = Function("binder", "return function (" + j.join(",") + "){ return binder.apply(this, arguments); }")(g); return m.prototype && (P.prototype = m.prototype, p.prototype = new P(), P.prototype = null), p;
                        }
                    }), M(Array, { isArray: Y }); var Q = Object("a"),
                    ac = "a" !== Q[0] || !(0 in Q),
                    A = function A(c) {
                        var f = !0,
                            g = !0; return c && (c.call("foo", function (e, i, h) {
                                "object" != (typeof h === "undefined" ? "undefined" : _typeof(h)) && (f = !1);
                            }), c.call([1], function () {
                                g = "string" == typeof this;
                            }, "x")), !!c && f && g;
                    }; M(V, {
                        forEach: function forEach(f) {
                            var h = F(this),
                                l = ac && W(this) ? this.split("") : h,
                                g = arguments[1],
                                c = -1,
                                j = l.length >>> 0; if (!R(f)) {
                                    throw new TypeError();
                                } for (; ++c < j;) {
                                    c in l && f.call(g, l[c], c, h);
                                }
                        }
                    }, !A(V.forEach)); var X = Array.prototype.indexOf && -1 !== [0, 1].indexOf(1, 2); M(V, {
                        indexOf: function indexOf(g) {
                            var h = ac && W(this) ? this.split("") : F(this),
                                f = h.length >>> 0; if (!f) {
                                    return -1;
                                } var c = 0; for (arguments.length > 1 && (c = I(arguments[1])), c = c >= 0 ? c : Math.max(0, f + c) ; f > c; c++) {
                                    if (c in h && h[c] === g) {
                                        return c;
                                    }
                                } return -1;
                        }
                    }, X); var D = ad.split; 2 !== "ab".split(/(?:ab)*/).length || 4 !== ".".split(/(.?)(.?)/).length || "t" === "tesst".split(/(s)*/)[1] || 4 !== "test".split(/(?:)/, -1).length || "".split(/.?/).length || ".".split(/()()/).length > 1 ? !function () {
                        var c = void 0 === /()??/.exec("")[1]; ad.split = function (i, e) {
                            var g = this; if (void 0 === i && 0 === e) {
                                return [];
                            } if ("[object RegExp]" !== ab.call(i)) {
                                return D.call(this, i, e);
                            } var y,
                                v,
                                x,
                                j,
                                p = [],
                                m = (i.ignoreCase ? "i" : "") + (i.multiline ? "m" : "") + (i.extended ? "x" : "") + (i.sticky ? "y" : ""),
                                t = 0; for (i = new RegExp(i.source, m + "g"), g += "", c || (y = new RegExp("^" + i.source + "$(?!\\s)", m)), e = void 0 === e ? -1 >>> 0 : Z(e) ; (v = i.exec(g)) && (x = v.index + v[0].length, !(x > t && (p.push(g.slice(t, v.index)), !c && v.length > 1 && v[0].replace(y, function () {
                              for (var f = 1; f < arguments.length - 2; f++) {
                                void 0 === arguments[f] && (v[f] = void 0);
                                }
                                }), v.length > 1 && v.index < g.length && V.push.apply(p, v.slice(1)), j = v[0].length, t = x, p.length >= e))) ;) {
                                    i.lastIndex === v.index && i.lastIndex++;
                                } return t === g.length ? (j || !i.test("")) && p.push("") : p.push(g.slice(t)), p.length > e ? p.slice(0, e) : p;
                        };
                    }() : "0".split(void 0, 0).length && (ad.split = function (c, f) {
                        return void 0 === c && 0 === f ? [] : D.call(this, c, f);
                    }); var B = "\t\n\x0B\f\r  \u1680\u180E\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200A\u202F\u205F\u3000\u2028\u2029\uFEFF",
                        ae = "",
                        H = "[" + B + "]",
                        q = new RegExp("^" + H + H + "*"),
                        z = new RegExp(H + H + "*$"),
                        k = ad.trim && (B.trim() || !ae.trim()); M(ad, {
                            trim: function trim() {
                                if (void 0 === this || null === this) {
                                    throw new TypeError("can't convert " + this + " to object");
                                } return String(this).replace(q, "").replace(z, "");
                            }
                        }, k); var U = ad.substr,
                        K = "".substr && "b" !== "0b".substr(-1); M(ad, {
                            substr: function substr(c, f) {
                                return U.call(this, 0 > c && (c = this.length + c) < 0 ? 0 : c, f);
                            }
                        }, K);
        }, {}], 16: [function (c, f) {
            f.exports = [c("./transport/websocket"), c("./transport/xhr-streaming"), c("./transport/xdr-streaming"), c("./transport/eventsource"), c("./transport/lib/iframe-wrap")(c("./transport/eventsource")), c("./transport/htmlfile"), c("./transport/lib/iframe-wrap")(c("./transport/htmlfile")), c("./transport/xhr-polling"), c("./transport/xdr-polling"), c("./transport/lib/iframe-wrap")(c("./transport/xhr-polling")), c("./transport/jsonp-polling")];
        }, { "./transport/eventsource": 20, "./transport/htmlfile": 21, "./transport/jsonp-polling": 23, "./transport/lib/iframe-wrap": 26, "./transport/websocket": 38, "./transport/xdr-polling": 39, "./transport/xdr-streaming": 40, "./transport/xhr-polling": 41, "./transport/xhr-streaming": 42 }], 17: [function (c, f) {
            (function (h) {
                function e(i, s, x, l) {
                    var u = this; k.call(this), setTimeout(function () {
                        u._start(i, s, x, l);
                    }, 0);
                } var k = c("events").EventEmitter,
                    g = c("inherits"),
                    v = c("../../utils/event"),
                    q = c("../../utils/url"),
                    t = h.XMLHttpRequest; g(e, k), e.prototype._start = function (u, y, B, s) {
                        var z = this; try {
                            this.xhr = new t();
                        } catch (A) { } if (!this.xhr) {
                            return this.emit("finish", 0, "no xhr support"), void this._cleanup();
                        } y = q.addQuery(y, "t=" + +new Date()), this.unloadRef = v.unloadAdd(function () {
                            z._cleanup(!0);
                        }); try {
                            this.xhr.open(u, y, !0), this.timeout && "timeout" in this.xhr && (this.xhr.timeout = this.timeout, this.xhr.ontimeout = function () {
                                z.emit("finish", 0, ""), z._cleanup(!1);
                            });
                        } catch (r) {
                            return this.emit("finish", 0, ""), void this._cleanup(!1);
                        } if (s && s.noCredentials || !e.supportsCORS || (this.xhr.withCredentials = "true"), s && s.headers) {
                            for (var x in s.headers) {
                                this.xhr.setRequestHeader(x, s.headers[x]);
                            }
                        } this.xhr.onreadystatechange = function () {
                            if (z.xhr) {
                                var i,
                                    o,
                                    C = z.xhr; switch (C.readyState) {
                                        case 3:
                                            try {
                                                o = C.status, i = C.responseText;
                                            } catch (l) { } 1223 === o && (o = 204), 200 === o && i && i.length > 0 && z.emit("chunk", o, i); break; case 4:
                                                o = C.status, 1223 === o && (o = 204), (12005 === o || 12029 === o) && (o = 0), z.emit("finish", o, C.responseText), z._cleanup(!1);
                                    }
                            }
                        }; try {
                            z.xhr.send(B);
                        } catch (r) {
                            z.emit("finish", 0, ""), z._cleanup(!1);
                        }
                    }, e.prototype._cleanup = function (i) {
                        if (this.xhr) {
                            if (this.removeAllListeners(), v.unloadDel(this.unloadRef), this.xhr.onreadystatechange = function () { }, this.xhr.ontimeout && (this.xhr.ontimeout = null), i) {
                                try {
                                    this.xhr.abort();
                                } catch (l) { }
                            } this.unloadRef = this.xhr = null;
                        }
                    }, e.prototype.close = function () {
                        this._cleanup(!0);
                    }, e.enabled = !!t; var p = ["Active"].concat("Object").join("X"); !e.enabled && p in h && (t = function t() {
                        try {
                            return new h[p]("Microsoft.XMLHTTP");
                        } catch (i) {
                            return null;
                        }
                    }, e.enabled = !!new t()); var j = !1; try {
                        j = "withCredentials" in new t();
                    } catch (m) { } e.supportsCORS = j, f.exports = e;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "../../utils/event": 46, "../../utils/url": 52, debug: void 0, events: 3, inherits: 54 }], 18: [function (c, f) {
            (function (e) {
                f.exports = e.EventSource;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, {}], 19: [function (c, f) {
            (function (e) {
                f.exports = e.WebSocket || e.MozWebSocket;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, {}], 20: [function (g, k) {
            function m(e) {
                if (!m.enabled()) {
                    throw new Error("Transport created when disabled");
                } f.call(this, e, "/eventsource", l, h);
            } var j = g("inherits"),
                f = g("./lib/ajax-based"),
                l = g("./receiver/eventsource"),
                h = g("./sender/xhr-cors"),
                c = g("eventsource"); j(m, f), m.enabled = function () {
                    return !!c;
                }, m.transportName = "eventsource", m.roundTrips = 2, k.exports = m;
        }, { "./lib/ajax-based": 24, "./receiver/eventsource": 29, "./sender/xhr-cors": 35, eventsource: 18, inherits: 54 }], 21: [function (f, j) {
            function l(e) {
                if (!c.enabled) {
                    throw new Error("Transport created when disabled");
                } g.call(this, e, "/htmlfile", c, k);
            } var h = f("inherits"),
                c = f("./receiver/htmlfile"),
                k = f("./sender/xhr-local"),
                g = f("./lib/ajax-based"); h(l, g), l.enabled = function (e) {
                    return c.enabled && e.sameOrigin;
                }, l.transportName = "htmlfile", l.roundTrips = 2, j.exports = l;
        }, { "./lib/ajax-based": 24, "./receiver/htmlfile": 30, "./sender/xhr-local": 37, inherits: 54 }], 22: [function (x, m) {
            function h(l, u, o) {
                if (!h.enabled()) {
                    throw new Error("Transport created when disabled");
                } g.call(this); var c = this; this.origin = q.getOrigin(o), this.baseUrl = o, this.transUrl = u, this.transport = l, this.windowId = j.string(8); var n = q.addPath(o, "/iframe.html") + "#" + this.windowId; this.iframeObj = v.createIframe(n, function (e) {
                    c.emit("close", 1006, "Unable to load an iframe (" + e + ")"), c.close();
                }), this.onmessageCallback = this._message.bind(this), p.attachEvent("message", this.onmessageCallback);
            } var f = x("inherits"),
                k = x("json3"),
                g = x("events").EventEmitter,
                y = x("../version"),
                q = x("../utils/url"),
                v = x("../utils/iframe"),
                p = x("../utils/event"),
                j = x("../utils/random"); f(h, g), h.prototype.close = function () {
                    if (this.removeAllListeners(), this.iframeObj) {
                        p.detachEvent("message", this.onmessageCallback); try {
                            this.postMessage("c");
                        } catch (c) { } this.iframeObj.cleanup(), this.iframeObj = null, this.onmessageCallback = this.iframeObj = null;
                    }
                }, h.prototype._message = function (c) {
                    if (q.isOriginEqual(c.origin, this.origin)) {
                        var l; try {
                            l = k.parse(c.data);
                        } catch (o) {
                            return;
                        } if (l.windowId === this.windowId) {
                            switch (l.type) {
                                case "s":
                                    this.iframeObj.loaded(), this.postMessage("s", k.stringify([y, this.transport, this.transUrl, this.baseUrl])); break; case "t":
                                        this.emit("message", l.data); break; case "c":
                                            var i; try {
                                                i = k.parse(l.data);
                                            } catch (o) {
                                                return;
                                            } this.emit("close", i[0], i[1]), this.close();
                            }
                        }
                    }
                }, h.prototype.postMessage = function (c, i) {
                    this.iframeObj.post(k.stringify({ windowId: this.windowId, type: c, data: i || "" }), this.origin);
                }, h.prototype.send = function (c) {
                    this.postMessage("m", c);
                }, h.enabled = function () {
                    return v.iframeEnabled;
                }, h.transportName = "iframe", h.roundTrips = 2, m.exports = h;
        }, { "../utils/event": 46, "../utils/iframe": 47, "../utils/random": 50, "../utils/url": 52, "../version": 53, debug: void 0, events: 3, inherits: 54, json3: 55 }], 23: [function (c, f) {
            (function (l) {
                function j(i) {
                    if (!j.enabled()) {
                        throw new Error("Transport created when disabled");
                    } k.call(this, i, "/jsonp", e, h);
                } var g = c("inherits"),
                    k = c("./lib/sender-receiver"),
                    h = c("./receiver/jsonp"),
                    e = c("./sender/jsonp"); g(j, k), j.enabled = function () {
                        return !!l.document;
                    }, j.transportName = "jsonp-polling", j.roundTrips = 1, j.needBody = !0, f.exports = j;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "./lib/sender-receiver": 28, "./receiver/jsonp": 31, "./sender/jsonp": 33, inherits: 54 }], 24: [function (f, j) {
            function l(e) {
                return function (t, u, q) {
                    var o = {}; "string" == typeof u && (o.headers = { "Content-type": "text/plain" }); var p = k.addPath(t, "/xhr_send"),
                        m = new e("POST", p, u, o); return m.once("finish", function (i) {
                            return m = null, 200 !== i && 204 !== i ? q(new Error("http status " + i)) : void q();
                        }), function () {
                            m.close(), m = null; var i = new Error("Aborted"); i.code = 1000, q(i);
                        };
                };
            } function h(n, p, o, m) {
                g.call(this, n, p, l(m), o, m);
            } var c = f("inherits"),
                k = f("../../utils/url"),
                g = f("./sender-receiver"); c(h, g), j.exports = h;
        }, { "../../utils/url": 52, "./sender-receiver": 28, debug: void 0, inherits: 54 }], 25: [function (f, h) {
            function j(i, k) {
                c.call(this), this.sendBuffer = [], this.sender = k, this.url = i;
            } var g = f("inherits"),
                c = f("events").EventEmitter; g(j, c), j.prototype.send = function (e) {
                    this.sendBuffer.push(e), this.sendStop || this.sendSchedule();
                }, j.prototype.sendScheduleWait = function () {
                    var i,
                        k = this; this.sendStop = function () {
                            k.sendStop = null, clearTimeout(i);
                        }, i = setTimeout(function () {
                            k.sendStop = null, k.sendSchedule();
                        }, 25);
                }, j.prototype.sendSchedule = function () {
                    var i = this; if (this.sendBuffer.length > 0) {
                        var k = "[" + this.sendBuffer.join(",") + "]"; this.sendStop = this.sender(this.url, k, function (l) {
                            i.sendStop = null, l ? (i.emit("close", l.code || 1006, "Sending error: " + l), i._cleanup()) : i.sendScheduleWait();
                        }), this.sendBuffer = [];
                    }
                }, j.prototype._cleanup = function () {
                    this.removeAllListeners();
                }, j.prototype.stop = function () {
                    this._cleanup(), this.sendStop && (this.sendStop(), this.sendStop = null);
                }, h.exports = j;
        }, { debug: void 0, events: 3, inherits: 54 }], 26: [function (c, f) {
            (function (j) {
                var g = c("inherits"),
                    e = c("../iframe"),
                    h = c("../../utils/object"); f.exports = function (i) {
                        function k(l, m) {
                            e.call(this, i.transportName, l, m);
                        } return g(k, e), k.enabled = function (n, m) {
                            if (!j.document) {
                                return !1;
                            } var l = h.extend({}, m); return l.sameOrigin = !0, i.enabled(l) && e.enabled();
                        }, k.transportName = "iframe-" + i.transportName, k.needBody = !0, k.roundTrips = e.roundTrips + i.roundTrips - 1, k.facadeTransport = i, k;
                    };
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "../../utils/object": 49, "../iframe": 22, inherits: 54 }], 27: [function (f, h) {
            function j(i, k, l) {
                c.call(this), this.Receiver = i, this.receiveUrl = k, this.AjaxObject = l, this._scheduleReceiver();
            } var g = f("inherits"),
                c = f("events").EventEmitter; g(j, c), j.prototype._scheduleReceiver = function () {
                    var i = this,
                        k = this.poll = new this.Receiver(this.receiveUrl, this.AjaxObject); k.on("message", function (l) {
                            i.emit("message", l);
                        }), k.once("close", function (l, e) {
                            i.poll = k = null, i.pollIsClosing || ("network" === e ? i._scheduleReceiver() : (i.emit("close", l || 1006, e), i.removeAllListeners()));
                        });
                }, j.prototype.abort = function () {
                    this.removeAllListeners(), this.pollIsClosing = !0, this.poll && this.poll.abort();
                }, h.exports = j;
        }, { debug: void 0, events: 3, inherits: 54 }], 28: [function (f, j) {
            function l(o, q, v, p, i) {
                var m = c.addPath(o, q),
                    s = this; k.call(this, o, v), this.poll = new g(p, m, i), this.poll.on("message", function (e) {
                        s.emit("message", e);
                    }), this.poll.once("close", function (n, r) {
                        s.poll = null, s.emit("close", n, r), s.close();
                    });
            } var h = f("inherits"),
                c = f("../../utils/url"),
                k = f("./buffered-sender"),
                g = f("./polling"); h(l, k), l.prototype.close = function () {
                    this.removeAllListeners(), this.poll && (this.poll.abort(), this.poll = null), this.stop();
                }, j.exports = l;
        }, { "../../utils/url": 52, "./buffered-sender": 25, "./polling": 27, debug: void 0, inherits: 54 }], 29: [function (f, h) {
            function k(i) {
                c.call(this); var l = this,
                    m = this.es = new j(i); m.onmessage = function (e) {
                        l.emit("message", decodeURI(e.data));
                    }, m.onerror = function (e) {
                        var n = 2 !== m.readyState ? "network" : "permanent"; l._cleanup(), l._close(n);
                    };
            } var g = f("inherits"),
                c = f("events").EventEmitter,
                j = f("eventsource"); g(k, c), k.prototype.abort = function () {
                    this._cleanup(), this._close("user");
                }, k.prototype._cleanup = function () {
                    var e = this.es; e && (e.onmessage = e.onerror = null, e.close(), this.es = null);
                }, k.prototype._close = function (i) {
                    var l = this; setTimeout(function () {
                        l.emit("close", null, i), l.removeAllListeners();
                    }, 200);
                }, h.exports = k;
        }, { debug: void 0, events: 3, eventsource: 18, inherits: 54 }], 30: [function (c, f) {
            (function (h) {
                function e(n) {
                    p.call(this); var o = this; g.polluteGlobalNamespace(), this.id = "a" + q.string(6), n = t.addQuery(n, "c=" + decodeURIComponent(g.WPrefix + "." + this.id)); var l = e.htmlfileEnabled ? g.createHtmlfile : g.createIframe; h[g.WPrefix][this.id] = {
                        start: function start() {
                            o.iframeObj.loaded();
                        }, message: function message(i) {
                            o.emit("message", i);
                        }, stop: function stop() {
                            o._cleanup(), o._close("network");
                        }
                    }, this.iframeObj = l(n, function () {
                        o._cleanup(), o._close("permanent");
                    });
                } var k = c("inherits"),
                    g = c("../../utils/iframe"),
                    t = c("../../utils/url"),
                    p = c("events").EventEmitter,
                    q = c("../../utils/random"); k(e, p), e.prototype.abort = function () {
                        this._cleanup(), this._close("user");
                    }, e.prototype._cleanup = function () {
                        this.iframeObj && (this.iframeObj.cleanup(), this.iframeObj = null), delete h[g.WPrefix][this.id];
                    }, e.prototype._close = function (i) {
                        this.emit("close", null, i), this.removeAllListeners();
                    }, e.htmlfileEnabled = !1; var m = ["Active"].concat("Object").join("X"); if (m in h) {
                        try {
                            e.htmlfileEnabled = !!new h[m]("htmlfile");
                        } catch (j) { }
                    } e.enabled = e.htmlfileEnabled || g.iframeEnabled, f.exports = e;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "../../utils/iframe": 47, "../../utils/random": 50, "../../utils/url": 52, debug: void 0, events: 3, inherits: 54 }], 31: [function (c, f) {
            (function (p) {
                function k(i) {
                    var o = this; m.call(this), h.polluteGlobalNamespace(), this.id = "a" + l.string(6); var n = e.addQuery(i, "c=" + encodeURIComponent(h.WPrefix + "." + this.id)); p[h.WPrefix][this.id] = this._callback.bind(this), this._createScript(n), this.timeoutId = setTimeout(function () {
                        o._abort(new Error("JSONP script loaded abnormally (timeout)"));
                    }, k.timeout);
                } var h = c("../../utils/iframe"),
                    l = c("../../utils/random"),
                    j = c("../../utils/browser"),
                    e = c("../../utils/url"),
                    g = c("inherits"),
                    m = c("events").EventEmitter; g(k, m), k.prototype.abort = function () {
                        if (p[h.WPrefix][this.id]) {
                            var i = new Error("JSONP user aborted read"); i.code = 1000, this._abort(i);
                        }
                    }, k.timeout = 35000, k.scriptErrorTimeout = 1000, k.prototype._callback = function (i) {
                        this._cleanup(), this.aborting || (i && this.emit("message", i), this.emit("close", null, "network"), this.removeAllListeners());
                    }, k.prototype._abort = function (i) {
                        this._cleanup(), this.aborting = !0, this.emit("close", i.code, i.message), this.removeAllListeners();
                    }, k.prototype._cleanup = function () {
                        if (clearTimeout(this.timeoutId), this.script2 && (this.script2.parentNode.removeChild(this.script2), this.script2 = null), this.script) {
                            var i = this.script; i.parentNode.removeChild(i), i.onreadystatechange = i.onerror = i.onload = i.onclick = null, this.script = null;
                        } delete p[h.WPrefix][this.id];
                    }, k.prototype._scriptError = function () {
                        var i = this; this.errorTimer || (this.errorTimer = setTimeout(function () {
                            i.loadedOkay || i._abort(new Error("JSONP script loaded abnormally (onerror)"));
                        }, k.scriptErrorTimeout));
                    }, k.prototype._createScript = function (s) {
                        var x,
                            v = this,
                            q = this.script = p.document.createElement("script"); if (q.id = "a" + l.string(8), q.src = s, q.type = "text/javascript", q.charset = "UTF-8", q.onerror = this._scriptError.bind(this), q.onload = function () {
                          v._abort(new Error("JSONP script loaded abnormally (onload)"));
                            }, q.onreadystatechange = function () {
                          if (/loaded|closed/.test(q.readyState)) {
                            if (q && q.htmlFor && q.onclick) {
                              v.loadedOkay = !0; try {
                                q.onclick();
                            } catch (i) { }
                            } q && v._abort(new Error("JSONP script loaded abnormally (onreadystatechange)"));
                            }
                            }, "undefined" == typeof q.async && p.document.attachEvent) {
                                if (j.isOpera()) {
                                    x = this.script2 = p.document.createElement("script"), x.text = "try{var a = document.getElementById('" + q.id + "'); if(a)a.onerror();}catch(x){};", q.async = x.async = !1;
                                } else {
                                    try {
                                        q.htmlFor = q.id, q.event = "onclick";
                                    } catch (n) { } q.async = !0;
                                }
                            } "undefined" != typeof q.async && (q.async = !0); var o = p.document.getElementsByTagName("head")[0]; o.insertBefore(q, o.firstChild), x && o.insertBefore(x, o.firstChild);
                    }, f.exports = k;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "../../utils/browser": 44, "../../utils/iframe": 47, "../../utils/random": 50, "../../utils/url": 52, debug: void 0, events: 3, inherits: 54 }], 32: [function (f, h) {
            function j(i, k) {
                c.call(this); var l = this; this.bufferPosition = 0, this.xo = new k("POST", i, null), this.xo.on("chunk", this._chunkHandler.bind(this)), this.xo.once("finish", function (m, o) {
                    l._chunkHandler(m, o), l.xo = null; var n = 200 === m ? "network" : "permanent"; l.emit("close", null, n), l._cleanup();
                });
            } var g = f("inherits"),
                c = f("events").EventEmitter; g(j, c), j.prototype._chunkHandler = function (l, o) {
                    if (200 === l && o) {
                        for (var p = -1; ; this.bufferPosition += p + 1) {
                            var m = o.slice(this.bufferPosition); if (p = m.indexOf("\n"), -1 === p) {
                                break;
                            } var k = m.slice(0, p); k && this.emit("message", k);
                        }
                    }
                }, j.prototype._cleanup = function () {
                    this.removeAllListeners();
                }, j.prototype.abort = function () {
                    this.xo && (this.xo.close(), this.emit("close", null, "user"), this.xo = null), this._cleanup();
                }, h.exports = j;
        }, { debug: void 0, events: 3, inherits: 54 }], 33: [function (c, f) {
            (function (m) {
                function k(i) {
                    try {
                        return m.document.createElement('<iframe name="' + i + '">');
                    } catch (o) {
                        var n = m.document.createElement("iframe"); return n.name = i, n;
                    }
                } function h() {
                    l = m.document.createElement("form"), l.style.display = "none", l.style.position = "absolute", l.method = "POST", l.enctype = "application/x-www-form-urlencoded", l.acceptCharset = "UTF-8", j = m.document.createElement("textarea"), j.name = "d", l.appendChild(j), m.document.body.appendChild(l);
                } var l,
                    j,
                    e = c("../../utils/random"),
                    g = c("../../utils/url"); f.exports = function (o, r, u) {
                        l || h(); var s = "a" + e.string(8); l.target = s, l.action = g.addQuery(g.addPath(o, "/jsonp_send"), "i=" + s); var i = k(s); i.id = s, i.style.display = "none", l.appendChild(i); try {
                            j.value = r;
                        } catch (q) { } l.submit(); var p = function p(n) {
                            i.onerror && (i.onreadystatechange = i.onerror = i.onload = null, setTimeout(function () {
                                i.parentNode.removeChild(i), i = null;
                            }, 500), j.value = "", u(n));
                        }; return i.onerror = function () {
                            p();
                        }, i.onload = function () {
                            p();
                        }, i.onreadystatechange = function (n) {
                            "complete" === i.readyState && p();
                        }, function () {
                            p(new Error("Aborted"));
                        };
                    };
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "../../utils/random": 50, "../../utils/url": 52, debug: void 0 }], 34: [function (c, f) {
            (function (m) {
                function k(i, p, q) {
                    var o = this; h.call(this), setTimeout(function () {
                        o._start(i, p, q);
                    }, 0);
                } var h = c("events").EventEmitter,
                    l = c("inherits"),
                    j = c("../../utils/event"),
                    e = c("../../utils/browser"),
                    g = c("../../utils/url"); l(k, h), k.prototype._start = function (q, u, s) {
                        var p = this,
                            v = new m.XDomainRequest(); u = g.addQuery(u, "t=" + +new Date()), v.onerror = function () {
                                p._error();
                            }, v.ontimeout = function () {
                                p._error();
                            }, v.onprogress = function () {
                                p.emit("chunk", 200, v.responseText);
                            }, v.onload = function () {
                                p.emit("finish", 200, v.responseText), p._cleanup(!1);
                            }, this.xdr = v, this.unloadRef = j.unloadAdd(function () {
                                p._cleanup(!0);
                            }); try {
                                this.xdr.open(q, u), this.timeout && (this.xdr.timeout = this.timeout), this.xdr.send(s);
                            } catch (n) {
                                this._error();
                            }
                    }, k.prototype._error = function () {
                        this.emit("finish", 0, ""), this._cleanup(!1);
                    }, k.prototype._cleanup = function (i) {
                        if (this.xdr) {
                            if (this.removeAllListeners(), j.unloadDel(this.unloadRef), this.xdr.ontimeout = this.xdr.onerror = this.xdr.onprogress = this.xdr.onload = null, i) {
                                try {
                                    this.xdr.abort();
                                } catch (n) { }
                            } this.unloadRef = this.xdr = null;
                        }
                    }, k.prototype.close = function () {
                        this._cleanup(!0);
                    }, k.enabled = !(!m.XDomainRequest || !e.hasDomain()), f.exports = k;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "../../utils/browser": 44, "../../utils/event": 46, "../../utils/url": 52, debug: void 0, events: 3, inherits: 54 }], 35: [function (f, h) {
            function j(i, l, m, k) {
                c.call(this, i, l, m, k);
            } var g = f("inherits"),
                c = f("../driver/xhr"); g(j, c), j.enabled = c.enabled && c.supportsCORS, h.exports = j;
        }, { "../driver/xhr": 17, inherits: 54 }], 36: [function (f, h) {
            function j() {
                var e = this; g.call(this), this.to = setTimeout(function () {
                    e.emit("finish", 200, "{}");
                }, j.timeout);
            } var g = f("events").EventEmitter,
                c = f("inherits"); c(j, g), j.prototype.close = function () {
                    clearTimeout(this.to);
                }, j.timeout = 2000, h.exports = j;
        }, { events: 3, inherits: 54 }], 37: [function (f, h) {
            function j(i, k, l) {
                c.call(this, i, k, l, { noCredentials: !0 });
            } var g = f("inherits"),
                c = f("../driver/xhr"); g(j, c), j.enabled = c.enabled, h.exports = j;
        }, { "../driver/xhr": 17, inherits: 54 }], 38: [function (g, k) {
            function m(i) {
                if (!m.enabled()) {
                    throw new Error("Transport created when disabled");
                } h.call(this); var n = this,
                    p = f.addPath(i, "/websocket"); p = "https" === p.slice(0, 5) ? "wss" + p.slice(5) : "ws" + p.slice(4), this.url = p, this.ws = new c(this.url), this.ws.onmessage = function (e) {
                        n.emit("message", e.data);
                    }, this.unloadRef = j.unloadAdd(function () {
                        n.ws.close();
                    }), this.ws.onclose = function (e) {
                        n.emit("close", e.code, e.reason), n._cleanup();
                    }, this.ws.onerror = function (e) {
                        n.emit("close", 1006, "WebSocket connection broken"), n._cleanup();
                    };
            } var j = g("../utils/event"),
                f = g("../utils/url"),
                l = g("inherits"),
                h = g("events").EventEmitter,
                c = g("./driver/websocket"); l(m, h), m.prototype.send = function (i) {
                    var n = "[" + i + "]"; this.ws.send(n);
                }, m.prototype.close = function () {
                    this.ws && this.ws.close(), this._cleanup();
                }, m.prototype._cleanup = function () {
                    var e = this.ws; e && (e.onmessage = e.onclose = e.onerror = null), j.unloadDel(this.unloadRef), this.unloadRef = this.ws = null, this.removeAllListeners();
                }, m.enabled = function () {
                    return !!c;
                }, m.transportName = "websocket", m.roundTrips = 2, k.exports = m;
        }, { "../utils/event": 46, "../utils/url": 52, "./driver/websocket": 19, debug: void 0, events: 3, inherits: 54 }], 39: [function (g, k) {
            function m(e) {
                if (!c.enabled) {
                    throw new Error("Transport created when disabled");
                } f.call(this, e, "/xhr", h, c);
            } var j = g("inherits"),
                f = g("./lib/ajax-based"),
                l = g("./xdr-streaming"),
                h = g("./receiver/xhr"),
                c = g("./sender/xdr"); j(m, f), m.enabled = l.enabled, m.transportName = "xdr-polling", m.roundTrips = 2, k.exports = m;
        }, { "./lib/ajax-based": 24, "./receiver/xhr": 32, "./sender/xdr": 34, "./xdr-streaming": 40, inherits: 54 }], 40: [function (f, j) {
            function l(e) {
                if (!g.enabled) {
                    throw new Error("Transport created when disabled");
                } c.call(this, e, "/xhr_streaming", k, g);
            } var h = f("inherits"),
                c = f("./lib/ajax-based"),
                k = f("./receiver/xhr"),
                g = f("./sender/xdr"); h(l, c), l.enabled = function (e) {
                    return e.cookie_needed || e.nullOrigin ? !1 : g.enabled && e.sameScheme;
                }, l.transportName = "xdr-streaming", l.roundTrips = 2, j.exports = l;
        }, { "./lib/ajax-based": 24, "./receiver/xhr": 32, "./sender/xdr": 34, inherits: 54 }], 41: [function (g, k) {
            function m(e) {
                if (!c.enabled && !h.enabled) {
                    throw new Error("Transport created when disabled");
                } f.call(this, e, "/xhr", l, h);
            } var j = g("inherits"),
                f = g("./lib/ajax-based"),
                l = g("./receiver/xhr"),
                h = g("./sender/xhr-cors"),
                c = g("./sender/xhr-local"); j(m, f), m.enabled = function (e) {
                    return e.nullOrigin ? !1 : c.enabled && e.sameOrigin ? !0 : h.enabled;
                }, m.transportName = "xhr-polling", m.roundTrips = 2, k.exports = m;
        }, { "./lib/ajax-based": 24, "./receiver/xhr": 32, "./sender/xhr-cors": 35, "./sender/xhr-local": 37, inherits: 54 }], 42: [function (c, f) {
            (function (p) {
                function k(i) {
                    if (!g.enabled && !e.enabled) {
                        throw new Error("Transport created when disabled");
                    } l.call(this, i, "/xhr_streaming", j, e);
                } var h = c("inherits"),
                    l = c("./lib/ajax-based"),
                    j = c("./receiver/xhr"),
                    e = c("./sender/xhr-cors"),
                    g = c("./sender/xhr-local"),
                    m = c("../utils/browser"); h(k, l), k.enabled = function (i) {
                        return i.nullOrigin ? !1 : m.isOpera() ? !1 : e.enabled;
                    }, k.transportName = "xhr-streaming", k.roundTrips = 2, k.needBody = !!p.document, f.exports = k;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "../utils/browser": 44, "./lib/ajax-based": 24, "./receiver/xhr": 32, "./sender/xhr-cors": 35, "./sender/xhr-local": 37, inherits: 54 }], 43: [function (c, f) {
            (function (e) {
                f.exports.randomBytes = e.crypto && e.crypto.getRandomValues ? function (g) {
                    var h = new Uint8Array(g); return e.crypto.getRandomValues(h), h;
                } : function (g) {
                    for (var h = new Array(g), i = 0; g > i; i++) {
                        h[i] = Math.floor(256 * Math.random());
                    } return h;
                };
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, {}], 44: [function (c, f) {
            (function (e) {
                f.exports = {
                    isOpera: function isOpera() {
                        return e.navigator && /opera/i.test(e.navigator.userAgent);
                    }, isKonqueror: function isKonqueror() {
                        return e.navigator && /konqueror/i.test(e.navigator.userAgent);
                    }, hasDomain: function hasDomain() {
                        if (!e.document) {
                            return !0;
                        } try {
                            return !!e.document.domain;
                        } catch (g) {
                            return !1;
                        }
                    }
                };
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, {}], 45: [function (f, h) {
            var k,
                g = f("json3"),
                c = /[\x00-\x1f\ud800-\udfff\ufffe\uffff\u0300-\u0333\u033d-\u0346\u034a-\u034c\u0350-\u0352\u0357-\u0358\u035c-\u0362\u0374\u037e\u0387\u0591-\u05af\u05c4\u0610-\u0617\u0653-\u0654\u0657-\u065b\u065d-\u065e\u06df-\u06e2\u06eb-\u06ec\u0730\u0732-\u0733\u0735-\u0736\u073a\u073d\u073f-\u0741\u0743\u0745\u0747\u07eb-\u07f1\u0951\u0958-\u095f\u09dc-\u09dd\u09df\u0a33\u0a36\u0a59-\u0a5b\u0a5e\u0b5c-\u0b5d\u0e38-\u0e39\u0f43\u0f4d\u0f52\u0f57\u0f5c\u0f69\u0f72-\u0f76\u0f78\u0f80-\u0f83\u0f93\u0f9d\u0fa2\u0fa7\u0fac\u0fb9\u1939-\u193a\u1a17\u1b6b\u1cda-\u1cdb\u1dc0-\u1dcf\u1dfc\u1dfe\u1f71\u1f73\u1f75\u1f77\u1f79\u1f7b\u1f7d\u1fbb\u1fbe\u1fc9\u1fcb\u1fd3\u1fdb\u1fe3\u1feb\u1fee-\u1fef\u1ff9\u1ffb\u1ffd\u2000-\u2001\u20d0-\u20d1\u20d4-\u20d7\u20e7-\u20e9\u2126\u212a-\u212b\u2329-\u232a\u2adc\u302b-\u302c\uaab2-\uaab3\uf900-\ufa0d\ufa10\ufa12\ufa15-\ufa1e\ufa20\ufa22\ufa25-\ufa26\ufa2a-\ufa2d\ufa30-\ufa6d\ufa70-\ufad9\ufb1d\ufb1f\ufb2a-\ufb36\ufb38-\ufb3c\ufb3e\ufb40-\ufb41\ufb43-\ufb44\ufb46-\ufb4e\ufff0-\uffff]/g,
                j = function j(i) {
                    var m,
                        o = {},
                        l = []; for (m = 0; 65536 > m; m++) {
                            l.push(String.fromCharCode(m));
                        } return i.lastIndex = 0, l.join("").replace(i, function (e) {
                            return o[e] = "\\u" + ("0000" + e.charCodeAt(0).toString(16)).slice(-4), "";
                        }), i.lastIndex = 0, o;
                }; h.exports = {
                    quote: function quote(i) {
                        var l = g.stringify(i); return c.lastIndex = 0, c.test(l) ? (k || (k = j(c)), l.replace(c, function (e) {
                            return k[e];
                        })) : l;
                    }
                };
        }, { json3: 55 }], 46: [function (c, f) {
            (function (l) {
                var j = c("./random"),
                    g = {},
                    k = !1,
                    h = l.chrome && l.chrome.app && l.chrome.app.runtime; f.exports = {
                        attachEvent: function attachEvent(i, m) {
                            "undefined" != typeof l.addEventListener ? l.addEventListener(i, m, !1) : l.document && l.attachEvent && (l.document.attachEvent("on" + i, m), l.attachEvent("on" + i, m));
                        }, detachEvent: function detachEvent(i, m) {
                            "undefined" != typeof l.addEventListener ? l.removeEventListener(i, m, !1) : l.document && l.detachEvent && (l.document.detachEvent("on" + i, m), l.detachEvent("on" + i, m));
                        }, unloadAdd: function unloadAdd(i) {
                            if (h) {
                                return null;
                            } var m = j.string(8); return g[m] = i, k && setTimeout(this.triggerUnloadCallbacks, 0), m;
                        }, unloadDel: function unloadDel(i) {
                            i in g && delete g[i];
                        }, triggerUnloadCallbacks: function triggerUnloadCallbacks() {
                            for (var i in g) {
                                g[i](), delete g[i];
                            }
                        }
                    }; var e = function e() {
                        k || (k = !0, f.exports.triggerUnloadCallbacks());
                    }; h || f.exports.attachEvent("unload", e);
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "./random": 50 }], 47: [function (c, f) {
            (function (j) {
                var g = c("./event"),
                    e = c("json3"),
                    h = c("./browser"); f.exports = {
                        WPrefix: "_jp", currentWindowId: null, polluteGlobalNamespace: function polluteGlobalNamespace() {
                            f.exports.WPrefix in j || (j[f.exports.WPrefix] = {});
                        }, postMessage: function postMessage(i, k) {
                            j.parent !== j && j.parent.postMessage(e.stringify({ windowId: f.exports.currentWindowId, type: i, data: k || "" }), "*");
                        }, createIframe: function createIframe(x, p) {
                            var n,
                                k,
                                y = j.document.createElement("iframe"),
                                r = function r() {
                                    clearTimeout(n); try {
                                        y.onload = null;
                                    } catch (i) { } y.onerror = null;
                                },
                                v = function v() {
                                    y && (r(), setTimeout(function () {
                                        y && y.parentNode.removeChild(y), y = null;
                                    }, 0), g.unloadDel(k));
                                },
                                q = function q(i) {
                                    y && (v(), p(i));
                                },
                                m = function m(i, l) {
                                    try {
                                        setTimeout(function () {
                                            y && y.contentWindow && y.contentWindow.postMessage(i, l);
                                        }, 0);
                                    } catch (o) { }
                                }; return y.src = x, y.style.display = "none", y.style.position = "absolute", y.onerror = function () {
                                    q("onerror");
                                }, y.onload = function () {
                                    clearTimeout(n), n = setTimeout(function () {
                                        q("onload timeout");
                                    }, 2000);
                                }, j.document.body.appendChild(y), n = setTimeout(function () {
                                    q("timeout");
                                }, 15000), k = g.unloadAdd(v), { post: m, cleanup: v, loaded: r };
                        }, createHtmlfile: function createHtmlfile(B, q) {
                            var m,
                                C,
                                z,
                                A = ["Active"].concat("Object").join("X"),
                                y = new j[A]("htmlfile"),
                                n = function n() {
                                    clearTimeout(m), z.onerror = null;
                                },
                                v = function v() {
                                    y && (n(), g.unloadDel(C), z.parentNode.removeChild(z), z = y = null, CollectGarbage());
                                },
                                r = function r(i) {
                                    y && (v(), q(i));
                                },
                                x = function x(i, l) {
                                    try {
                                        setTimeout(function () {
                                            z && z.contentWindow && z.contentWindow.postMessage(i, l);
                                        }, 0);
                                    } catch (o) { }
                                }; y.open(), y.write('<html><script>document.domain="' + j.document.domain + '";<\/script></html>'), y.close(), y.parentWindow[f.exports.WPrefix] = j[f.exports.WPrefix]; var k = y.createElement("div"); return y.body.appendChild(k), z = y.createElement("iframe"), k.appendChild(z), z.src = B, z.onerror = function () {
                                    r("onerror");
                                }, m = setTimeout(function () {
                                    r("timeout");
                                }, 15000), C = g.unloadAdd(v), { post: x, cleanup: v, loaded: n };
                        }
                    }, f.exports.iframeEnabled = !1, j.document && (f.exports.iframeEnabled = ("function" == typeof j.postMessage || "object" == _typeof(j.postMessage)) && !h.isKonqueror());
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "./browser": 44, "./event": 46, debug: void 0, json3: 55 }], 48: [function (c, f) {
            (function (e) {
                var g = {};["log", "debug", "warn"].forEach(function (i) {
                    var h = e.console && e.console[i] && e.console[i].apply; g[i] = h ? function () {
                        return e.console[i].apply(e.console, arguments);
                    } : "log" === i ? function () { } : g.log;
                }), f.exports = g;
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, {}], 49: [function (c, f) {
            f.exports = {
                isObject: function isObject(g) {
                    var h = typeof g === "undefined" ? "undefined" : _typeof(g); return "function" === h || "object" === h && !!g;
                }, extend: function extend(h) {
                    if (!this.isObject(h)) {
                        return h;
                    } for (var k, l, j = 1, g = arguments.length; g > j; j++) {
                        k = arguments[j]; for (l in k) {
                            Object.prototype.hasOwnProperty.call(k, l) && (h[l] = k[l]);
                        }
                    } return h;
                }
            };
        }, {}], 50: [function (c, g) {
            var h = c("crypto"),
                f = "abcdefghijklmnopqrstuvwxyz012345"; g.exports = {
                    string: function string(k) {
                        for (var m = f.length, j = h.randomBytes(k), n = [], l = 0; k > l; l++) {
                            n.push(f.substr(j[l] % m, 1));
                        } return n.join("");
                    }, number: function number(e) {
                        return Math.floor(Math.random() * e);
                    }, numberString: function numberString(i) {
                        var j = ("" + (i - 1)).length,
                            k = new Array(j + 1).join("0"); return (k + this.number(i)).slice(-j);
                    }
                };
        }, { crypto: 43 }], 51: [function (c, f) {
            f.exports = function (e) {
                return {
                    filterToEnabled: function filterToEnabled(h, i) {
                        var g = { main: [], facade: [] }; return h ? "string" == typeof h && (h = [h]) : h = [], e.forEach(function (j) {
                            j && ("websocket" !== j.transportName || i.websocket !== !1) && (h.length && -1 === h.indexOf(j.transportName) || j.enabled(i) && (g.main.push(j), j.facadeTransport && g.facade.push(j.facadeTransport)));
                        }), g;
                    }
                };
            };
        }, { debug: void 0 }], 52: [function (c, f) {
            var g = c("url-parse"); f.exports = {
                getOrigin: function getOrigin(h) {
                    if (!h) {
                        return null;
                    } var j = new g(h); if ("file:" === j.protocol) {
                        return null;
                    } var i = j.port; return i || (i = "https:" === j.protocol ? "443" : "80"), j.protocol + "//" + j.hostname + ":" + i;
                }, isOriginEqual: function isOriginEqual(h, i) {
                    var j = this.getOrigin(h) === this.getOrigin(i); return j;
                }, isSchemeEqual: function isSchemeEqual(h, i) {
                    return h.split(":")[0] === i.split(":")[0];
                }, addPath: function addPath(h, i) {
                    var j = h.split("?"); return j[0] + i + (j[1] ? "?" + j[1] : "");
                }, addQuery: function addQuery(h, i) {
                    return h + (-1 === h.indexOf("?") ? "?" + i : "&" + i);
                }
            };
        }, { debug: void 0, "url-parse": 56 }], 53: [function (c, f) {
            f.exports = "1.0.3";
        }, {}], 54: [function (c, f) {
            f.exports = "function" == typeof Object.create ? function (g, h) {
                g.super_ = h, g.prototype = Object.create(h.prototype, { constructor: { value: g, enumerable: !1, writable: !0, configurable: !0 } });
            } : function (g, h) {
                g.super_ = h; var i = function i() { }; i.prototype = h.prototype, g.prototype = new i(), g.prototype.constructor = g;
            };
        }, {}], 55: [function (f, g, c) {
            (function (h) {
                (function () {
                    function k(ao, aC) {
                        function au(E) {
                            if (au[E] !== av) {
                                return au[E];
                            } var x; if ("bug-string-char-index" == E) {
                                x = "a" != "a"[0];
                            } else {
                                if ("json" == E) {
                                    x = au("json-stringify") && au("json-parse");
                                } else {
                                    var F,
                                        C = "{\"a\":[1,true,false,null,\"\\u0000\\b\\n\\f\\r\\t\"]}"; if ("json-stringify" == E) {
                                            var D = aC.stringify,
                                                o = "function" == typeof D && aA; if (o) {
                                                    (F = function F() {
                                                        return 1;
                                                    }).toJSON = F; try {
                                                        o = "0" === D(0) && "0" === D(new ap()) && '""' == D(new at()) && D(ak) === av && D(av) === av && D() === av && "1" === D(F) && "[1]" == D([F]) && "[null]" == D([av]) && "null" == D(null) && "[null,null,null]" == D([av, ak, null]) && D({ a: [F, !0, !1, null, "\x00\b\n\f\r	"] }) == C && "1" === D(null, F) && "[\n 1,\n 2\n]" == D([1, 2], null, 1) && '"-271821-04-20T00:00:00.000Z"' == D(new aE(-8640000000000000)) && '"+275760-09-13T00:00:00.000Z"' == D(new aE(8640000000000000)) && '"-000001-01-01T00:00:00.000Z"' == D(new aE(-62198755200000)) && '"1969-12-31T23:59:59.999Z"' == D(new aE(-1));
                                                    } catch (A) {
                                                        o = !1;
                                                    }
                                                } x = o;
                                        } if ("json-parse" == E) {
                                            var y = aC.parse; if ("function" == typeof y) {
                                                try {
                                                    if (0 === y("0") && !y(!1)) {
                                                        F = y(C); var B = 5 == F.a.length && 1 === F.a[0]; if (B) {
                                                            try {
                                                                B = !y('"	"');
                                                            } catch (A) { } if (B) {
                                                                try {
                                                                    B = 1 !== y("01");
                                                                } catch (A) { }
                                                            } if (B) {
                                                                try {
                                                                    B = 1 !== y("1.");
                                                                } catch (A) { }
                                                            }
                                                        }
                                                    }
                                                } catch (A) {
                                                    B = !1;
                                                }
                                            } x = B;
                                        }
                                }
                            } return au[E] = !!x;
                        } ao || (ao = t.Object()), aC || (aC = t.Object()); var ap = ao.Number || t.Number,
                            at = ao.String || t.String,
                            aG = ao.Object || t.Object,
                            aE = ao.Date || t.Date,
                            aw = ao.SyntaxError || t.SyntaxError,
                            aB = ao.TypeError || t.TypeError,
                            az = ao.Math || t.Math,
                            aD = ao.JSON || t.JSON; "object" == (typeof aD === "undefined" ? "undefined" : _typeof(aD)) && aD && (aC.stringify = aD.stringify, aC.parse = aD.parse); var _ar,
                            _an,
                            av,
                            aF = aG.prototype,
                            ak = aF.toString,
                            aA = new aE(-3509827334573292); try {
                                aA = -109252 == aA.getUTCFullYear() && 0 === aA.getUTCMonth() && 1 === aA.getUTCDate() && 10 == aA.getUTCHours() && 37 == aA.getUTCMinutes() && 6 == aA.getUTCSeconds() && 708 == aA.getUTCMilliseconds();
                            } catch (am) { } if (!au("json")) {
                                var al = "[object Function]",
                                    aH = "[object Date]",
                                    af = "[object Number]",
                                    z = "[object String]",
                                    V = "[object Array]",
                                    u = "[object Boolean]",
                                    ay = au("bug-string-char-index"); if (!aA) {
                                        var ah = az.floor,
                                            X = [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334],
                                            aj = function aj(l, o) {
                                                return X[o] + 365 * (l - 1970) + ah((l - 1969 + (o = +(o > 1))) / 4) - ah((l - 1901 + o) / 100) + ah((l - 1601 + o) / 400);
                                            };
                                    } if ((_ar = aF.hasOwnProperty) || (_ar = function ar(l) {
                                      var o,
                                          x = {}; return (x.__proto__ = null, x.__proto__ = { toString: 1 }, x).toString != ak ? _ar = function ar(y) {
                                        var A = this.__proto__,
                                            B = y in (this.__proto__ = null, this); return this.__proto__ = A, B;
                                    } : (o = x.constructor, _ar = function ar(y) {
                                        var A = (this.constructor || o).prototype; return y in this && !(y in A && this[y] === A[y]);
                                    }), x = null, _ar.call(this, l);
                                    }), _an = function an(x, A) {
                                      var C,
                                          y,
                                          l,
                                          B = 0; (C = function C() {
                                        this.valueOf = 0;
                                    }).prototype.valueOf = 0, y = new C(); for (l in y) {
                                        _ar.call(y, l) && B++;
                                    } return C = y = null, B ? _an = 2 == B ? function (D, F) {
                                        var G,
                                            E = {},
                                            o = ak.call(D) == al; for (G in D) {
                                          o && "prototype" == G || _ar.call(E, G) || !(E[G] = 1) || !_ar.call(D, G) || F(G);
                                    }
                                    } : function (D, F) {
                                        var G,
                                            E,
                                            o = ak.call(D) == al; for (G in D) {
                                          o && "prototype" == G || !_ar.call(D, G) || (E = "constructor" === G) || F(G);
                                    } (E || _ar.call(D, G = "constructor")) && F(G);
                                    } : (y = ["valueOf", "toString", "toLocaleString", "propertyIsEnumerable", "isPrototypeOf", "hasOwnProperty", "constructor"], _an = function an(F, G) {
                                        var I,
                                            E,
                                            H = ak.call(F) == al,
                                            D = !H && "function" != typeof F.constructor && v[_typeof(F.hasOwnProperty)] && F.hasOwnProperty || _ar; for (I in F) {
                                          H && "prototype" == I || !D.call(F, I) || G(I);
                                    } for (E = y.length; I = y[--E]; D.call(F, I) && G(I)) { }
                                    }), _an(x, A);
                                    }, !au("json-stringify")) {
                                        var ax = { 92: "\\\\", 34: '\\"', 8: "\\b", 12: "\\f", 10: "\\n", 13: "\\r", 9: "\\t" },
                                            ab = "000000",
                                            Z = function Z(l, o) {
                                                return (ab + (o || 0)).slice(-l);
                                            },
                                            K = "\\u00",
                                            s = function s(x) {
                                                for (var B = '"', D = 0, A = x.length, l = !ay || A > 10, C = l && (ay ? x.split("") : x) ; A > D; D++) {
                                                    var y = x.charCodeAt(D); switch (y) {
                                                        case 8: case 9: case 10: case 12: case 13: case 34: case 92:
                                                            B += ax[y]; break; default:
                                                                if (32 > y) {
                                                                    B += K + Z(2, y.toString(16)); break;
                                                                } B += l ? C[D] : x.charAt(D);
                                                    }
                                                } return B + '"';
                                            },
                                            Q = function Q(H, aL, P, L, aI, O, J) {
                                                var aP, G, aN, S, aJ, aM, aO, aK, F, E, U, B, T, D, A, C; try {
                                                    aP = aL[H];
                                                } catch (y) { } if ("object" == (typeof aP === "undefined" ? "undefined" : _typeof(aP)) && aP) {
                                                    if (G = ak.call(aP), G != aH || _ar.call(aP, "toJSON")) {
                                                        "function" == typeof aP.toJSON && (G != af && G != z && G != V || _ar.call(aP, "toJSON")) && (aP = aP.toJSON(H));
                                                    } else {
                                                        if (aP > -1 / 0 && 1 / 0 > aP) {
                                                            if (aj) {
                                                                for (aJ = ah(aP / 86400000), aN = ah(aJ / 365.2425) + 1970 - 1; aj(aN + 1, 0) <= aJ; aN++) { } for (S = ah((aJ - aj(aN, 0)) / 30.42) ; aj(aN, S + 1) <= aJ; S++) { } aJ = 1 + aJ - aj(aN, S), aM = (aP % 86400000 + 86400000) % 86400000, aO = ah(aM / 3600000) % 24, aK = ah(aM / 60000) % 60, F = ah(aM / 1000) % 60, E = aM % 1000;
                                                            } else {
                                                                aN = aP.getUTCFullYear(), S = aP.getUTCMonth(), aJ = aP.getUTCDate(), aO = aP.getUTCHours(), aK = aP.getUTCMinutes(), F = aP.getUTCSeconds(), E = aP.getUTCMilliseconds();
                                                            } aP = (0 >= aN || aN >= 10000 ? (0 > aN ? "-" : "+") + Z(6, 0 > aN ? -aN : aN) : Z(4, aN)) + "-" + Z(2, S + 1) + "-" + Z(2, aJ) + "T" + Z(2, aO) + ":" + Z(2, aK) + ":" + Z(2, F) + "." + Z(3, E) + "Z";
                                                        } else {
                                                            aP = null;
                                                        }
                                                    }
                                                } if (P && (aP = P.call(aL, H, aP)), null === aP) {
                                                    return "null";
                                                } if (G = ak.call(aP), G == u) {
                                                    return "" + aP;
                                                } if (G == af) {
                                                    return aP > -1 / 0 && 1 / 0 > aP ? "" + aP : "null";
                                                } if (G == z) {
                                                    return s("" + aP);
                                                } if ("object" == (typeof aP === "undefined" ? "undefined" : _typeof(aP))) {
                                                    for (D = J.length; D--;) {
                                                        if (J[D] === aP) {
                                                            throw aB();
                                                        }
                                                    } if (J.push(aP), U = [], A = O, O += aI, G == V) {
                                                        for (T = 0, D = aP.length; D > T; T++) {
                                                            B = Q(T, aP, P, L, aI, O, J), U.push(B === av ? "null" : B);
                                                        } C = U.length ? aI ? "[\n" + O + U.join(",\n" + O) + "\n" + A + "]" : "[" + U.join(",") + "]" : "[]";
                                                    } else {
                                                        _an(L || aP, function (l) {
                                                            var o = Q(l, aP, P, L, aI, O, J); o !== av && U.push(s(l) + ":" + (aI ? " " : "") + o);
                                                        }), C = U.length ? aI ? "{\n" + O + U.join(",\n" + O) + "\n" + A + "}" : "{" + U.join(",") + "}" : "{}";
                                                    } return J.pop(), C;
                                                }
                                            }; aC.stringify = function (H, D, A) {
                                                var x, C, y, F; if (v[typeof D === "undefined" ? "undefined" : _typeof(D)] && D) {
                                                    if ((F = ak.call(D)) == al) {
                                                        C = D;
                                                    } else {
                                                        if (F == V) {
                                                            y = {}; for (var G, E = 0, B = D.length; B > E; G = D[E++], F = ak.call(G), (F == z || F == af) && (y[G] = 1)) { }
                                                        }
                                                    }
                                                } if (A) {
                                                    if ((F = ak.call(A)) == af) {
                                                        if ((A -= A % 1) > 0) {
                                                            for (x = "", A > 10 && (A = 10) ; x.length < A; x += " ") { }
                                                        }
                                                    } else {
                                                        F == z && (x = A.length <= 10 ? A : A.slice(0, 10));
                                                    }
                                                } return Q("", (G = {}, G[""] = H, G), C, y, x, "", []);
                                            };
                                    } if (!au("json-parse")) {
                                        var Y,
                                            i,
                                            aq = at.fromCharCode,
                                            ag = { 92: "\\", 34: '"', 47: "/", 98: "\b", 116: "	", 110: "\n", 102: "\f", 114: "\r" },
                                            ae = function ae() {
                                                throw Y = i = null, aw();
                                            },
                                            ai = function ai() {
                                                for (var x, B, D, A, l, C = i, y = C.length; y > Y;) {
                                                    switch (l = C.charCodeAt(Y)) {
                                                        case 9: case 10: case 13: case 32:
                                                            Y++; break; case 123: case 125: case 91: case 93: case 58: case 44:
                                                                return x = ay ? C.charAt(Y) : C[Y], Y++, x; case 34:
                                                                    for (x = "@", Y++; y > Y;) {
                                                                        if (l = C.charCodeAt(Y), 32 > l) {
                                                                            ae();
                                                                        } else {
                                                                            if (92 == l) {
                                                                                switch (l = C.charCodeAt(++Y)) {
                                                                                    case 92: case 34: case 47: case 98: case 116: case 110: case 102: case 114:
                                                                                        x += ag[l], Y++; break; case 117:
                                                                                            for (B = ++Y, D = Y + 4; D > Y; Y++) {
                                                                                                l = C.charCodeAt(Y), l >= 48 && 57 >= l || l >= 97 && 102 >= l || l >= 65 && 70 >= l || ae();
                                                                                            } x += aq("0x" + C.slice(B, Y)); break; default:
                                                                                                ae();
                                                                                }
                                                                            } else {
                                                                                if (34 == l) {
                                                                                    break;
                                                                                } for (l = C.charCodeAt(Y), B = Y; l >= 32 && 92 != l && 34 != l;) {
                                                                                    l = C.charCodeAt(++Y);
                                                                                } x += C.slice(B, Y);
                                                                            }
                                                                        }
                                                                    } if (34 == C.charCodeAt(Y)) {
                                                                        return Y++, x;
                                                                    } ae(); default:
                                                                        if (B = Y, 45 == l && (A = !0, l = C.charCodeAt(++Y)), l >= 48 && 57 >= l) {
                                                                            for (48 == l && (l = C.charCodeAt(Y + 1), l >= 48 && 57 >= l) && ae(), A = !1; y > Y && (l = C.charCodeAt(Y), l >= 48 && 57 >= l) ; Y++) { } if (46 == C.charCodeAt(Y)) {
                                                                                for (D = ++Y; y > D && (l = C.charCodeAt(D), l >= 48 && 57 >= l) ; D++) { } D == Y && ae(), Y = D;
                                                                            } if (l = C.charCodeAt(Y), 101 == l || 69 == l) {
                                                                                for (l = C.charCodeAt(++Y), (43 == l || 45 == l) && Y++, D = Y; y > D && (l = C.charCodeAt(D), l >= 48 && 57 >= l) ; D++) { } D == Y && ae(), Y = D;
                                                                            } return +C.slice(B, Y);
                                                                        } if (A && ae(), "true" == C.slice(Y, Y + 4)) {
                                                                            return Y += 4, !0;
                                                                        } if ("false" == C.slice(Y, Y + 5)) {
                                                                            return Y += 5, !1;
                                                                        } if ("null" == C.slice(Y, Y + 4)) {
                                                                            return Y += 4, null;
                                                                        } ae();
                                                    }
                                                } return "$";
                                            },
                                            ad = function ad(l) {
                                                var o, x; if ("$" == l && ae(), "string" == typeof l) {
                                                    if ("@" == (ay ? l.charAt(0) : l[0])) {
                                                        return l.slice(1);
                                                    } if ("[" == l) {
                                                        for (o = []; l = ai(), "]" != l; x || (x = !0)) {
                                                            x && ("," == l ? (l = ai(), "]" == l && ae()) : ae()), "," == l && ae(), o.push(ad(l));
                                                        } return o;
                                                    } if ("{" == l) {
                                                        for (o = {}; l = ai(), "}" != l; x || (x = !0)) {
                                                            x && ("," == l ? (l = ai(), "}" == l && ae()) : ae()), ("," == l || "string" != typeof l || "@" != (ay ? l.charAt(0) : l[0]) || ":" != ai()) && ae(), o[l.slice(1)] = ad(ai());
                                                        } return o;
                                                    } ae();
                                                } return l;
                                            },
                                            aa = function aa(l, x, y) {
                                                var o = ac(l, x, y); o === av ? delete l[x] : l[x] = o;
                                            },
                                            ac = function ac(o, y, A) {
                                                var x,
                                                    l = o[y]; if ("object" == (typeof l === "undefined" ? "undefined" : _typeof(l)) && l) {
                                                        if (ak.call(l) == V) {
                                                            for (x = l.length; x--;) {
                                                                aa(l, x, A);
                                                            }
                                                        } else {
                                                            _an(l, function (B) {
                                                                aa(l, B, A);
                                                            });
                                                        }
                                                    } return A.call(o, y, l);
                                            }; aC.parse = function (l, x) {
                                                var y, o; return Y = 0, i = "" + l, y = ad(ai()), "$" != ai() && ae(), Y = i = null, x && ak.call(x) == al ? ac((o = {}, o[""] = y, o), "", x) : y;
                                            };
                                    }
                            } return aC.runInContext = k, aC;
                    } var e = "function" == typeof a && a.amd,
                        v = { "function": !0, object: !0 },
                        r = v[typeof c === "undefined" ? "undefined" : _typeof(c)] && c && !c.nodeType && c,
                        t = v[typeof window === "undefined" ? "undefined" : _typeof(window)] && window || this,
                        q = r && v[typeof g === "undefined" ? "undefined" : _typeof(g)] && g && !g.nodeType && "object" == (typeof h === "undefined" ? "undefined" : _typeof(h)) && h; if (!q || q.global !== q && q.window !== q && q.self !== q || (t = q), r && !e) {
                            k(t, r);
                        } else {
                            var j = t.JSON,
                                n = t.JSON3,
                                m = !1,
                                p = k(t, t.JSON3 = {
                                    noConflict: function noConflict() {
                                        return m || (m = !0, t.JSON = j, t.JSON3 = n, j = n = null), p;
                                    }
                                }); t.JSON = { parse: p.parse, stringify: p.stringify };
                        } e && a(function () {
                            return p;
                        });
                }).call(this);
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, {}], 56: [function (g, k) {
            function m(B, s, A) {
                if (!(this instanceof m)) {
                    return new m(B, s, A);
                } var y,
                    o,
                    r,
                    q,
                    x = h.test(B),
                    i = typeof s === "undefined" ? "undefined" : _typeof(s),
                    z = this,
                    n = 0; for ("object" !== i && "string" !== i && (A = s, s = null), A && "function" != typeof A && (A = l.parse), s = f(s) ; n < c.length; n++) {
                        o = c[n], y = o[0], q = o[1], y !== y ? z[q] = B : "string" == typeof y ? ~(r = B.indexOf(y)) && ("number" == typeof o[2] ? (z[q] = B.slice(0, r), B = B.slice(r + o[2])) : (z[q] = B.slice(r), B = B.slice(0, r))) : (r = y.exec(B)) && (z[q] = r[1], B = B.slice(0, B.length - r[0].length)), z[q] = z[q] || (o[3] || "port" === q && x ? s[q] || "" : ""), o[4] && (z[q] = z[q].toLowerCase());
                    } A && (z.query = A(z.query)), j(z.port, z.protocol) || (z.host = z.hostname, z.port = ""), z.username = z.password = "", z.auth && (o = z.auth.split(":"), z.username = o[0] || "", z.password = o[1] || ""), z.href = z.toString();
            } var j = g("requires-port"),
                f = g("./lolcation"),
                l = g("querystringify"),
                h = /^\/(?!\/)/,
                c = [["#", "hash"], ["?", "query"], ["//", "protocol", 2, 1, 1], ["/", "pathname"], ["@", "auth", 1], [NaN, "host", void 0, 1, 1], [/\:(\d+)$/, "port"], [NaN, "hostname", void 0, 1, 1]]; m.prototype.set = function (p, q, r) {
                    var o = this; return "query" === p ? ("string" == typeof q && (q = (r || l.parse)(q)), o[p] = q) : "port" === p ? (o[p] = q, j(q, o.protocol) ? q && (o.host = o.hostname + ":" + q) : (o.host = o.hostname, o[p] = "")) : "hostname" === p ? (o[p] = q, o.port && (q += ":" + o.port), o.host = q) : "host" === p ? (o[p] = q, /\:\d+/.test(q) && (q = q.split(":"), o.hostname = q[0], o.port = q[1])) : o[p] = q, o.href = o.toString(), o;
                }, m.prototype.toString = function (i) {
                    i && "function" == typeof i || (i = l.stringify); var p,
                        q = this,
                        o = q.protocol + "//"; return q.username && (o += q.username, q.password && (o += ":" + q.password), o += "@"), o += q.hostname, q.port && (o += ":" + q.port), o += q.pathname, q.query && (p = "object" == _typeof(q.query) ? i(q.query) : q.query, o += ("?" === p.charAt(0) ? "" : "?") + p), q.hash && (o += q.hash), o;
                }, m.qs = l, m.location = f, k.exports = m;
        }, { "./lolcation": 57, querystringify: 58, "requires-port": 59 }], 57: [function (c, f) {
            (function (h) {
                var g,
                    e = { hash: 1, query: 1 }; f.exports = function (k) {
                        k = k || h.location || {}, g = g || c("./"); var l,
                            j = {},
                            i = typeof k === "undefined" ? "undefined" : _typeof(k); if ("blob:" === k.protocol) {
                                j = new g(unescape(k.pathname), {});
                            } else {
                                if ("string" === i) {
                                    j = new g(k, {}); for (l in e) {
                                        delete j[l];
                                    }
                                } else {
                                    if ("object" === i) {
                                        for (l in k) {
                                            l in e || (j[l] = k[l]);
                                        }
                                    }
                                }
                            } return j;
                    };
            }).call(this, "undefined" != typeof global ? global : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {});
        }, { "./": 56 }], 58: [function (f, h, k) {
            function g(i) {
                for (var m, o = /([^=?&]+)=([^&]*)/g, l = {}; m = o.exec(i) ; l[decodeURIComponent(m[1])] = decodeURIComponent(m[2])) { } return l;
            } function c(i, m) {
                m = m || ""; var o = []; "string" != typeof m && (m = "?"); for (var l in i) {
                    j.call(i, l) && o.push(encodeURIComponent(l) + "=" + encodeURIComponent(i[l]));
                } return o.length ? m + o.join("&") : "";
            } var j = Object.prototype.hasOwnProperty; k.stringify = c, k.parse = g;
        }, {}], 59: [function (c, f) {
            f.exports = function (g, h) {
                if (h = h.split(":")[0], g = +g, !g) {
                    return !1;
                } switch (h) {
                    case "http": case "ws":
                        return 80 !== g; case "https": case "wss":
                            return 443 !== g; case "ftp":
                                return 22 !== g; case "gopher":
                                    return 70 !== g; case "file":
                                        return !1;
                } return 0 !== g;
            };
        }, {}]
    }, {}, [1])(1);
}), function () {
    var b,
        f,
        h,
        c,
        a = {}.hasOwnProperty,
        g = [].slice; b = { LF: "\n", NULL: "\x00" }, h = function () {
            function i(k, l, m) {
                this.command = k, this.headers = null != l ? l : {}, this.body = null != m ? m : "";
            } var j; return i.prototype.toString = function () {
                var p, l, m, k, e; p = [this.command], m = this.headers["content-length"] === !1 ? !0 : !1, m && delete this.headers["content-length"], e = this.headers; for (l in e) {
                    a.call(e, l) && (k = e[l], p.push("" + l + ":" + k));
                } return this.body && !m && p.push("content-length:" + i.sizeOfUTF8(this.body)), p.push(b.LF + this.body), p.join(b.LF);
            }, i.sizeOfUTF8 = function (e) {
                return e ? encodeURI(e).match(/%..|./g).length : 0;
            }, j = function j(B) {
                var x, E, A, t, L, q, J, D, H, F, I, z, k, C, K, e, G; for (t = B.search(RegExp("" + b.LF + b.LF)), L = B.substring(0, t).split(b.LF), A = L.shift(), q = {}, z = function z(l) {
                  return l.replace(/^\s+|\s+$/g, "");
                }, e = L.reverse(), k = 0, K = e.length; K > k; k++) {
                    F = e[k], D = F.indexOf(":"), q[z(F.substring(0, D))] = z(F.substring(D + 1));
                } if (x = "", I = t + 2, q["content-length"]) {
                    H = parseInt(q["content-length"]), x = ("" + B).substring(I, I + H);
                } else {
                    for (E = null, J = C = I, G = B.length; (G >= I ? G > C : C > G) && (E = B.charAt(J), E !== b.NULL) ; J = G >= I ? ++C : --C) {
                        x += E;
                    }
                } return new i(A, q, x);
            }, i.unmarshall = function (l) {
                var k; return function () {
                    var m, p, n, e; for (n = l.split(RegExp("" + b.NULL + b.LF + "*")), e = [], m = 0, p = n.length; p > m; m++) {
                        k = n[m], (null != k ? k.length : void 0) > 0 && e.push(j(k));
                    } return e;
                }();
            }, i.marshall = function (m, k, e) {
                var l; return l = new i(m, k, e), l.toString() + b.NULL;
            }, i;
        }(), f = function () {
            function k(e) {
                this.ws = e, this.ws.binaryType = "arraybuffer", this.counter = 0, this.connected = !1, this.heartbeat = { outgoing: 10000, incoming: 10000 }, this.maxWebSocketFrameSize = 16384, this.subscriptions = {};
            } var j; return k.prototype.debug = function (i) {
                var l; return "undefined" != typeof window && null !== window ? (null != (l = window.console), void 0) : void 0;
            }, j = function j() {
                return Date.now ? Date.now() : new Date().valueOf;
            }, k.prototype._transmit = function (m, o, n) {
                var l; for (l = h.marshall(m, o, n), "function" == typeof this.debug && this.debug(">>> " + l) ; ;) {
                    if (!(l.length > this.maxWebSocketFrameSize)) {
                        return this.ws.send(l);
                    } this.ws.send(l.substring(0, this.maxWebSocketFrameSize)), l = l.substring(this.maxWebSocketFrameSize), "function" == typeof this.debug && this.debug("remaining = " + l.length);
                }
            }, k.prototype._setupHeartbeat = function (p) {
                var t, q, m, i, l, r; if ((l = p.version) === c.VERSIONS.V1_1 || l === c.VERSIONS.V1_2) {
                    return r = function () {
                        var o, u, s, e; for (s = p["heart-beat"].split(","), e = [], o = 0, u = s.length; u > o; o++) {
                            i = s[o], e.push(parseInt(i));
                        } return e;
                    }(), q = r[0], t = r[1], 0 !== this.heartbeat.outgoing && 0 !== t && (m = Math.max(this.heartbeat.outgoing, t), "function" == typeof this.debug && this.debug("send PING every " + m + "ms"), this.pinger = c.setInterval(m, function (n) {
                        return function () {
                            return n.ws.send(b.LF), "function" == typeof n.debug ? n.debug(">>> PING") : void 0;
                        };
                    }(this))), 0 !== this.heartbeat.incoming && 0 !== q ? (m = Math.max(this.heartbeat.incoming, q), "function" == typeof this.debug && this.debug("check PONG every " + m + "ms"), this.ponger = c.setInterval(m, function (e) {
                        return function () {
                            var n; return n = j() - e.serverActivity, n > 2 * m ? ("function" == typeof e.debug && e.debug("did not receive server activity for the last " + n + "ms"), e.ws.close()) : void 0;
                        };
                    }(this))) : void 0;
                }
            }, k.prototype._parseConnect = function () {
                var i, m, o, l; switch (i = 1 <= arguments.length ? g.call(arguments, 0) : [], l = {}, i.length) {
                    case 2:
                        l = i[0], m = i[1]; break; case 3:
                            i[1] instanceof Function ? (l = i[0], m = i[1], o = i[2]) : (l.login = i[0], l.passcode = i[1], m = i[2]); break; case 4:
                                l.login = i[0], l.passcode = i[1], m = i[2], o = i[3]; break; default:
                                    l.login = i[0], l.passcode = i[1], m = i[2], o = i[3], l.host = i[4];
                } return [l, m, o];
            }, k.prototype.connect = function () {
                var n, m, i, l; return n = 1 <= arguments.length ? g.call(arguments, 0) : [], l = this._parseConnect.apply(this, n), i = l[0], this.connectCallback = l[1], m = l[2], "function" == typeof this.debug && this.debug("Opening Web Socket..."), this.ws.onmessage = function (o) {
                    return function (e) {
                        var s, D, F, B, x, z, y, A, q, E, t, C; if (B = "undefined" != typeof ArrayBuffer && e.data instanceof ArrayBuffer ? (s = new Uint8Array(e.data), "function" == typeof o.debug ? o.debug("--- got data length: " + s.length) : void 0, function () {
                          var p, r, u; for (u = [], p = 0, r = s.length; r > p; p++) {
                            D = s[p], u.push(String.fromCharCode(D));
                        } return u;
                        }().join("")) : e.data, o.serverActivity = j(), B === b.LF) {
                            return void ("function" == typeof o.debug && o.debug("<<< PONG"));
                        } for ("function" == typeof o.debug && o.debug("<<< " + B), t = h.unmarshall(B), C = [], q = 0, E = t.length; E > q; q++) {
                            switch (x = t[q], x.command) {
                                case "CONNECTED":
                                    "function" == typeof o.debug && o.debug("connected to server " + x.headers.server), o.connected = !0, o._setupHeartbeat(x.headers), C.push("function" == typeof o.connectCallback ? o.connectCallback(x) : void 0); break; case "MESSAGE":
                                        A = x.headers.subscription, y = o.subscriptions[A] || o.onreceive, y ? (F = o, z = x.headers["message-id"], x.ack = function (p) {
                                            return null == p && (p = {}), F.ack(z, A, p);
                                        }, x.nack = function (p) {
                                            return null == p && (p = {}), F.nack(z, A, p);
                                        }, C.push(y(x))) : C.push("function" == typeof o.debug ? o.debug("Unhandled received MESSAGE: " + x) : void 0); break; case "RECEIPT":
                                            C.push("function" == typeof o.onreceipt ? o.onreceipt(x) : void 0); break; case "ERROR":
                                                C.push("function" == typeof m ? m(x) : void 0); break; default:
                                                    C.push("function" == typeof o.debug ? o.debug("Unhandled frame: " + x) : void 0);
                            }
                        } return C;
                    };
                }(this), this.ws.onclose = function (e) {
                    return function () {
                        var o; return o = "Whoops! Lost connection to " + e.ws.url, "function" == typeof e.debug && e.debug(o), e._cleanUp(), "function" == typeof m ? m(o) : void 0;
                    };
                }(this), this.ws.onopen = function (e) {
                    return function () {
                        return "function" == typeof e.debug && e.debug("Web Socket Opened..."), i["accept-version"] = c.VERSIONS.supportedVersions(), i["heart-beat"] = [e.heartbeat.outgoing, e.heartbeat.incoming].join(","), e._transmit("CONNECT", i);
                    };
                }(this);
            }, k.prototype.disconnect = function (i, l) {
                return null == l && (l = {}), this._transmit("DISCONNECT", l), this.ws.onclose = null, this.ws.close(), this._cleanUp(), "function" == typeof i ? i() : void 0;
            }, k.prototype._cleanUp = function () {
                return this.connected = !1, this.pinger && c.clearInterval(this.pinger), this.ponger ? c.clearInterval(this.ponger) : void 0;
            }, k.prototype.send = function (i, l, m) {
                return null == l && (l = {}), null == m && (m = ""), l.destination = i, this._transmit("SEND", l, m);
            }, k.prototype.subscribe = function (i, m, o) {
                var l; return null == o && (o = {}), o.id || (o.id = "sub-" + this.counter++), o.destination = i, this.subscriptions[o.id] = m, this._transmit("SUBSCRIBE", o), l = this, {
                    id: o.id, unsubscribe: function unsubscribe() {
                        return l.unsubscribe(o.id);
                    }
                };
            }, k.prototype.unsubscribe = function (e) {
                return delete this.subscriptions[e], this._transmit("UNSUBSCRIBE", { id: e });
            }, k.prototype.begin = function (i) {
                var l, m; return m = i || "tx-" + this.counter++, this._transmit("BEGIN", { transaction: m }), l = this, {
                    id: m, commit: function commit() {
                        return l.commit(m);
                    }, abort: function abort() {
                        return l.abort(m);
                    }
                };
            }, k.prototype.commit = function (e) {
                return this._transmit("COMMIT", { transaction: e });
            }, k.prototype.abort = function (e) {
                return this._transmit("ABORT", { transaction: e });
            }, k.prototype.ack = function (i, l, m) {
                return null == m && (m = {}), m["message-id"] = i, m.subscription = l, this._transmit("ACK", m);
            }, k.prototype.nack = function (i, l, m) {
                return null == m && (m = {}), m["message-id"] = i, m.subscription = l, this._transmit("NACK", m);
            }, k;
        }(), c = {
            VERSIONS: {
                V1_0: "1.0", V1_1: "1.1", V1_2: "1.2", supportedVersions: function supportedVersions() {
                    return "1.1,1.0";
                }
            }, client: function client(j, l) {
                var e, k; return null == l && (l = ["v10.stomp", "v11.stomp"]), e = c.WebSocketClass || WebSocket, k = new e(j, l), new f(k);
            }, over: function over(e) {
                return new f(e);
            }, Frame: h
        }, "undefined" != typeof exports && null !== exports && (exports.Stomp = c), "undefined" != typeof window && null !== window ? (c.setInterval = function (i, j) {
            return window.setInterval(j, i);
        }, c.clearInterval = function (e) {
            return window.clearInterval(e);
        }, window.Stomp = c) : exports || (self.Stomp = c);
}.call(undefined); var w = window,
    d = document; var MCK_LABELS; var MCK_BASE_URL; var MCK_CURR_LATITIUDE = 40.7324319; var MCK_CURR_LONGITUDE = -73.82480777777776; var mckUtils = new MckUtils(); var mckDateUtils = new MckDateUtils(); var mckContactUtils = new MckContactUtils(); var mckMapUtils = new MckMapUtils(); function MckUtils() {
        var e = this; var b = 3,
            a = 1,
            c = ["p", "div", "pre", "form"]; e.init = function () {
                var f = MCK_CONTEXTPATH ? MCK_CONTEXTPATH + "/v2/tab/initialize.page" : "https://apps.applozic.com/v2/tab/initialize.page"; var g = MCK_CONTEXTPATH ? MCK_CONTEXTPATH + "/rest/ws/message/list" : "https://apps.applozic.com/rest/ws/message/list"; $applozic.ajax({ url: f, contentType: "application/json", type: "OPTIONS" }).done(function (h) { }); $applozic.ajax({ url: g, contentType: "application/json", type: "OPTIONS" }).done(function (h) { });
            }; e.showElement = function (f) {
                if ((typeof f === "undefined" ? "undefined" : _typeof(f)) !== "object" && typeof f !== "undefined" && typeof f !== null || f && (typeof f === "undefined" ? "undefined" : _typeof(f)) === "object" && f.length !== 0) {
                    f.classList.remove("n-vis"); f.classList.add("vis");
                }
            }; e.hideElement = function (f) {
                if ((typeof f === "undefined" ? "undefined" : _typeof(f)) !== "object" && typeof f !== "undefined" && typeof f !== null || f && (typeof f === "undefined" ? "undefined" : _typeof(f)) === "object" && f.length !== 0) {
                    f.classList.remove("vis"); f.classList.add("n-vis");
                }
            }; e.badgeCountOnLaucher = function (g, f) {
                var h = document.getElementById("applozic-badge-count"); if (g === true && f > 0) {
                    if (f < 99) {
                        h.innerHTML = f;
                    } else {
                        h.innerHTML = "99+";
                    } h.classList.add("mck-badge-count");
                } if (g === true && f === 0) {
                    h.innerHTML = ""; h.classList.remove("mck-badge-count");
                }
            }; e.randomId = function () {
                return w.Math.random().toString(36).substring(7);
            }; e.textVal = function (j) {
                var h = []; var g = []; var f = function f() {
                    h.push(g.join("")); g = [];
                }; var m = function m(r) {
                    if (r.nodeType === b) {
                        g.push(r.nodeValue);
                    } else {
                        if (r.nodeType === a) {
                            var q = r.tagName.toLowerCase(); var n = c.indexOf(q) !== -1; if (n && g.length) {
                                f();
                            } if (q === "img") {
                                var s = r.getAttribute("alt") || ""; if (s) {
                                    g.push(s);
                                } return;
                            } else {
                                if (q === "style") {
                                    return;
                                } else {
                                    if (q === "br") {
                                        f();
                                    }
                                }
                            } var p = r.childNodes; for (var o = 0; o < p.length; o++) {
                                m(p[o]);
                            } if (n && g.length) {
                                f();
                            }
                        }
                    }
                }; var l = j.childNodes; for (var k = 0; k < l.length; k++) {
                    m(l[k]);
                } if (g.length) {
                    f();
                } return h.join("\n");
            }; e.mouseX = function (f) {
                if (f.pageX) {
                    return f.pageX;
                } else {
                    if (f.clientX) {
                        return f.clientX + (d.documentElement.scrollLeft ? d.documentElement.scrollLeft : d.body.scrollLeft);
                    } else {
                        return null;
                    }
                }
            }; e.mouseY = function (f) {
                if (f.pageY) {
                    return f.pageY;
                } else {
                    if (f.clientY) {
                        return f.clientY + (d.documentElement.scrollTop ? d.documentElement.scrollTop : d.body.scrollTop);
                    } else {
                        return null;
                    }
                }
            }; e.startsWith = function (g, h) {
                if (h === null || typeof g === "undefined") {
                    return false;
                } var f = h.length; if (g.length < f) {
                    return false;
                } for (--f; f >= 0 && g[f] === h[f]; --f) {
                    continue;
                } return f < 0;
            }; e.setEndOfContenteditable = function (g) {
                var f, h; if (document.createRange) {
                    f = document.createRange(); f.selectNodeContents(g); f.collapse(false); h = window.getSelection(); h.removeAllRanges(); h.addRange(f);
                } else {
                    if (document.selection) {
                        f = document.body.createTextRange(); f.moveToElementText(g); f.collapse(false); f.select();
                    }
                }
            }; this.encryptionKey = null; this.getEncryptionKey = function () {
                var f; if (this.encryptionKey === null) {
                    f = ALStorage.getEncryptionKey(); return f;
                } else {
                    return this.encryptionKey;
                }
            }; this.setEncryptionKey = function (f) {
                this.encryptionKey = f;
            }; e.b64EncodeUnicode = function (f) {
                return btoa(encodeURIComponent(f).replace(/%([0-9A-F]{2})/g, function (g, h) {
                    return String.fromCharCode("0x" + h);
                }));
            }; e.b64DecodeUnicode = function (f) {
                return decodeURIComponent(Array.prototype.map.call(atob(f), function (g) {
                    return "%" + ("00" + g.charCodeAt(0).toString(16)).slice(-2);
                }).join(""));
            }; e.ajax = function (j) {
                var m = $applozic.extend({}, {}, j); if (!(j.skipEncryption === true) && mckUtils.getEncryptionKey()) {
                    var l = aesjs.util.convertStringToBytes(mckUtils.getEncryptionKey()); var i = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]; if (m.type.toLowerCase() === "post") {
                        while (j.data && j.data.length % 16 != 0) {
                            j.data += " ";
                        } var k = new aesjs.ModeOfOperation.ecb(l); var g = aesjs.util.convertStringToBytes(j.data); var h = k.encrypt(g); var f = String.fromCharCode.apply(null, h); m.data = btoa(f);
                    } m.success = function (s) {
                        var r = atob(s); var n = []; for (var q = 0; q < r.length; q++) {
                            n.push(r.charCodeAt(q));
                        } var p = new aesjs.ModeOfOperation.ecb(l); var t = p.decrypt(n); var o = aesjs.util.convertBytesToString(t); o = o.replace(/\\u0000/g, "").replace(/^\s*|\s*[\x00-\x10]*$/g, ""); if (mckUtils.isJsonString(o)) {
                            j.success(JSON.parse(o));
                        } else {
                            j.success(o);
                        }
                    };
                } $applozic.ajax(m);
            }; e.isJsonString = function (g) {
                try {
                    JSON.parse(g);
                } catch (f) {
                    return false;
                } return true;
            };
    } function MckContactUtils() {
        var a = this; a.getContactId = function (b) {
            var c = b.contactId; return a.formatContactId(c);
        }; a.formatContactId = function (b) {
            if (b.indexOf("+") === 0) {
                b = b.substring(1);
            } b = decodeURIComponent(b); return $applozic.trim(b.replace(/\@/g, "AT").replace(/\./g, "DOT").replace(/\*/g, "STAR").replace(/\#/g, "HASH").replace(/\|/g, "VBAR").replace(/\+/g, "PLUS").replace(/\;/g, "SCOLON").replace(/\?/g, "QMARK").replace(/\,/g, "COMMA").replace(/\:/g, "COLON"));
        };
    } function MckMapUtils() {
        var a = this; a.getCurrentLocation = function (c, b) {
            w.navigator.geolocation.getCurrentPosition(c, b);
        }; a.getSelectedLocation = function () {
            return { lat: MCK_CURR_LATITIUDE, lon: MCK_CURR_LONGITUDE };
        };
    } function MckDateUtils() {
        var g = this; var f = "mmm d, h:MM TT"; var e = "mmm d"; var c = "h:MM TT"; var b = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]; g.getDate = function (j) {
            var i = new Date(parseInt(j, 10)); var h = new Date(); return h.getDate() === i.getDate() && h.getMonth() === i.getMonth() && h.getYear() === i.getYear() ? a(i, c, false) : a(i, f, false);
        }; g.getLastSeenAtStatus = function (j) {
            var i = new Date(parseInt(j, 10)); var h = new Date(); if (h.getDate() === i.getDate() && h.getMonth() === i.getMonth() && h.getYear() === i.getYear()) {
                var k = h.getHours() - i.getHours(); var l = w.Math.floor((h.getTime() - i.getTime()) / 60000); if (l < 60) {
                    return l <= 1 ? MCK_LABELS["last.seen"] + " 1 " + MCK_LABELS.min + " " + MCK_LABELS.ago : MCK_LABELS["last.seen"] + " " + l + MCK_LABELS.mins + " " + MCK_LABELS.ago;
                } return k === 1 ? MCK_LABELS["last.seen"] + " 1 " + MCK_LABELS.hour + " " + MCK_LABELS.ago : MCK_LABELS["last.seen"] + " " + k + MCK_LABELS.hours + " " + MCK_LABELS.ago;
            } else {
                if (h.getDate() - i.getDate() === 1 && h.getMonth() === i.getMonth() && h.getYear() === i.getYear()) {
                    return MCK_LABELS["last.seen.on"] + " " + MCK_LABELS.yesterday;
                } else {
                    return MCK_LABELS["last.seen.on"] + " " + a(i, e, false);
                }
            }
        }; g.getTimeOrDate = function (k, j) {
            var i = new Date(parseInt(k, 10)); var h = new Date(); if (j) {
                return h.getDate() === i.getDate() && h.getMonth() === i.getMonth() && h.getYear() === i.getYear() ? a(i, c, false) : a(i, e, false);
            } else {
                return a(i, f, false);
            }
        }; g.getSystemDate = function (i) {
            var h = new Date(parseInt(i, 10)); return a(h, f, false);
        }; g.convertMilisIntoTime = function (k) {
            var l; var i = parseInt(k % 1000 / 100),
                m = parseInt(k / 1000 % 60),
                j = parseInt(k / (1000 * 60) % 60),
                h = parseInt(k / (1000 * 60 * 60) % 24); if (h > 0) {
                    l = h + " Hr " + j + " Min " + m + " Sec";
                } else {
                    if (j > 0) {
                        l = j + " Min " + m + " Sec";
                    } else {
                        l = m + " Sec ";
                    }
                } return l;
        }; var a = function () {
            var h = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
                i = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
                k = /[^-+\dA-Z]/g,
                j = function j(m, l) {
                    m = String(m); l = l || 2; while (m.length < l) {
                        m = "0" + m;
                    } return m;
                }; return function (r, F, A) {
                    var p = a; if (arguments.length === 1 && Object.prototype.toString.call(r) === "[object String]" && !/\d/.test(r)) {
                        F = r; r = undefined;
                    } r = r ? new Date(r) : new Date(); if (isNaN(r)) {
                        throw SyntaxError("invalid date");
                    } F = String(F); if (F.slice(0, 4) === "UTC:") {
                        F = F.slice(4); A = true;
                    } var C = A ? "getUTC" : "get",
                        v = r[C + "Date"](),
                        l = r[C + "Day"](),
                        t = r[C + "Month"](),
                        z = r[C + "FullYear"](),
                        B = r[C + "Hours"](),
                        u = r[C + "Minutes"](),
                        E = r[C + "Seconds"](),
                        x = r[C + "Milliseconds"](),
                        n = A ? 0 : r.getTimezoneOffset(),
                        q = { d: v, dd: j(v), ddd: p.i18n.dayNames[l], dddd: p.i18n.dayNames[l + 7], m: t + 1, mm: j(t + 1), mmm: p.i18n.monthNames[t], mmmm: p.i18n.monthNames[t + 12], yy: String(z).slice(2), yyyy: z, h: B % 12 || 12, hh: j(B % 12 || 12), H: B, HH: j(B), M: u, MM: j(u), s: E, ss: j(E), l: j(x, 3), L: j(x > 99 ? w.Math.round(x / 10) : x), t: B < 12 ? "a" : "p", tt: B < 12 ? "am" : "pm", T: B < 12 ? "A" : "P", TT: B < 12 ? "AM" : "PM", Z: A ? "UTC" : (String(r).match(i) || [""]).pop().replace(k, ""), o: (n > 0 ? "-" : "+") + j(w.Math.floor(w.Math.abs(n) / 60) * 100 + w.Math.abs(n) % 60, 4), S: ["th", "st", "nd", "rd"][v % 10 > 3 ? 0 : (v % 100 - v % 10 !== 10) * v % 10] }; return F.replace(h, function (m) {
                            return m in q ? q[m] : m.slice(1, m.length - 1);
                        });
                };
        }(); a.masks = { "default": "mmm d, yyyy h:MM TT", fullDateFormat: "mmm d, yyyy h:MM TT", onlyDateFormat: "mmm d", onlyTimeFormat: "h:MM TT", mailDateFormat: "mmm d, yyyy", mediumDate: "mmm d, yyyy", longDate: "mmmm d, yyyy", fullDate: "dddd, mmmm d, yyyy", shortTime: "h:MM TT", mediumTime: "h:MM:ss TT", longTime: "h:MM:ss TT Z", isoDate: "yyyy-mm-dd", isoTime: "HH:MM:ss", isoDateTime: "yyyy-mm-dd'T'HH:MM:ss", isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'" }; a.i18n = { dayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], monthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"] };
    } (function (b) {
        function a() {
            var c = {}; c.init = function () { }; return c;
        } if (typeof Applozic === "undefined") {
            b.Applozic = a();
        } else {
            console.log("Applozic already defined.");
        }
    })(window); var ALStorage = function (h) {
        var a = []; var b = []; var f = []; var c = []; var e; var g; var i; return {
            setEncryptionKey: function setEncryptionKey(j) {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.setItem("encryptionKey", j);
                } else {
                    i = j;
                }
            }, getEncryptionKey: function getEncryptionKey(j) {
                return typeof w.sessionStorage !== "undefined" ? w.sessionStorage.getItem("encryptionKey") : i;
            }, removeEncryptionKey: function removeEncryptionKey() {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.removeItem("encryptionKey");
                }
            }, updateLatestMessage: function updateLatestMessage(k) {
                var j = []; j.push(k); ALStorage.updateLatestMessageArray(j); ALStorage.updateMckMessageArray(j);
            }, getLatestMessageArray: function getLatestMessageArray() {
                return typeof w.sessionStorage !== "undefined" ? $applozic.parseJSON(w.sessionStorage.getItem("mckLatestMessageArray")) : a;
            }, getFriendListGroupName: function getFriendListGroupName() {
                return typeof w.sessionStorage !== "undefined" ? w.sessionStorage.getItem("friendListGroupName") : e;
            }, setFriendListGroupName: function setFriendListGroupName(j) {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.setItem("friendListGroupName", j);
                } else {
                    e = j;
                }
            }, setFriendListGroupType: function setFriendListGroupType(j) {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.setItem("friendListGroupType", j);
                } else {
                    g = j;
                }
            }, getFriendListGroupType: function getFriendListGroupType() {
                return typeof w.sessionStorage !== "undefined" ? w.sessionStorage.getItem("friendListGroupType") : g;
            }, setLatestMessageArray: function setLatestMessageArray(j) {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.setItem("mckLatestMessageArray", w.JSON.stringify(j));
                } else {
                    a = j;
                }
            }, updateLatestMessageArray: function updateLatestMessageArray(j) {
                if (typeof w.sessionStorage !== "undefined") {
                    var k = $applozic.parseJSON(w.sessionStorage.getItem("mckLatestMessageArray")); if (k !== null) {
                        k = k.concat(j); w.sessionStorage.setItem("mckLatestMessageArray", w.JSON.stringify(k));
                    } else {
                        w.sessionStorage.setItem("mckLatestMessageArray", w.JSON.stringify(j));
                    } return j;
                } else {
                    a = a.concat(j); return a;
                }
            }, getMckMessageArray: function getMckMessageArray() {
                return typeof w.sessionStorage !== "undefined" ? $applozic.parseJSON(w.sessionStorage.getItem("mckMessageArray")) : b;
            }, clearMckMessageArray: function clearMckMessageArray() {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.removeItem("mckMessageArray"); w.sessionStorage.removeItem("mckLatestMessageArray");
                } else {
                    b.length = 0; a.length = 0;
                }
            }, clearAppHeaders: function clearAppHeaders() {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.removeItem("mckAppHeaders");
                }
            }, setAppHeaders: function setAppHeaders(j) {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.setItem("mckAppHeaders", w.JSON.stringify(j));
                }
            }, getAppHeaders: function getAppHeaders(j) {
                return typeof w.sessionStorage !== "undefined" ? $applozic.parseJSON(w.sessionStorage.getItem("mckAppHeaders")) : {};
            }, getMessageByKey: function getMessageByKey(j) {
                return f[j];
            }, updateMckMessageArray: function updateMckMessageArray(j) {
                for (var k = 0; k < j.length; k++) {
                    var l = j[k]; f[l.key] = l;
                } if (typeof w.sessionStorage !== "undefined") {
                    var m = $applozic.parseJSON(w.sessionStorage.getItem("mckMessageArray")); if (m !== null) {
                        m = m.concat(j); w.sessionStorage.setItem("mckMessageArray", w.JSON.stringify(m));
                    } else {
                        w.sessionStorage.setItem("mckMessageArray", w.JSON.stringify(j));
                    } return j;
                } else {
                    b = b.concat(j); return b;
                }
            }, getMckContactNameArray: function getMckContactNameArray() {
                return typeof w.sessionStorage !== "undefined" ? $applozic.parseJSON(w.sessionStorage.getItem("mckContactNameArray")) : c;
            }, setMckContactNameArray: function setMckContactNameArray(j) {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.setItem("mckContactNameArray", w.JSON.stringify(j));
                } else {
                    c = j;
                }
            }, updateMckContactNameArray: function updateMckContactNameArray(k) {
                if (typeof w.sessionStorage !== "undefined") {
                    var j = $applozic.parseJSON(w.sessionStorage.getItem("mckContactNameArray")); if (j !== null) {
                        k = k.concat(j);
                    } w.sessionStorage.setItem("mckContactNameArray", w.JSON.stringify(k)); return k;
                } else {
                    c = c.concat(k); return c;
                }
            }, clearMckContactNameArray: function clearMckContactNameArray() {
                if (typeof w.sessionStorage !== "undefined") {
                    w.sessionStorage.removeItem("mckContactNameArray");
                } else {
                    c.length = 0;
                }
            }
        };
    }(window); (function (a) {
        function b(x) {
            var Q = {}; var A = ""; var V = new MckUtils(); var M = "https://apps.applozic.com"; var r = "https://applozic.appspot.com"; var n = "/v2/tab/initialize.page"; var o = "/rest/ws/message/list"; var K = "/rest/ws/message/send"; var J = "/rest/ws/group/create"; var D = "/rest/ws/group/list"; var c = "/rest/ws/group/v2/info"; var y = "/rest/ws/group/add/member"; var E = "/rest/ws/group/remove/member"; var ac = "/rest/ws/group/left"; var u = "/rest/ws/group/update"; var X = "/rest/ws/group/check/user"; var P = "/rest/ws/group/user/count"; var i = "/rest/ws/group/"; var ag = "/rest/ws/user/v2/detail"; var k = "/rest/ws/user/update"; var H = "/rest/ws/user/filter"; var ah = "/rest/ws/device/logout"; var p = "/rest/ws/user/v3/filter"; var Y = "/rest/ws/user/block"; var L = "/rest/ws/user/unblock"; var aa = "/rest/ws/user/update/password"; var m = "/rest/ws/message/detail"; var g = "/rest/ws/message/delete"; var l = "/rest/ws/message/read"; var N = "/rest/ws/message/delivered"; var af = "/rest/ws/conversation/close"; var z = "/rest/ws/aws/file"; var t = "/rest/ws/aws/file/url"; var R = "/rest/ws/upload/file"; var O = "/rest/ws/aws/file/delete"; var h = "/rest/ws/message/add/inbox"; var S = "/rest/ws/message/read/conversation"; var ab = "/rest/ws/message/delete/conversation"; var U = "/rest/ws/user/chat/mute"; var q = "/rest/ws/group/user/update"; var W = "/rest/ws/register/client"; var C = "/rest/ws/user/chat/mute/list"; var B = "/rest/ws/conversation/topicId"; var s = "/rest/ws/user/info"; var G = "/rest/ws/user/chat/status"; var Z = "/rest/ws/conversation/get"; var e = "/rest/ws/conversation/id"; var R = "/rest/ws/upload/file"; var I = "/rest/ws/upload/image"; var j = "/rest/ws/device/logout"; var F = "/rest/ws/plugin/update/sw/id"; var T; var ae; var v; var f; function ad(aj) {
                var ai = ""; for (var ak in aj) {
                    ai += encodeURIComponent(ak) + "=" + encodeURIComponent(aj[ak]) + "&";
                } return ai.substring(0, ai.length - 1);
            } Q.getFileUrl = function () {
                return r;
            }; Q.initServerUrl = function (ai) {
                M = ai;
            }; Q.login = function (ai) {
                A = ai.data.alUser.applicationId; M = ai.data.baseUrl ? ai.data.baseUrl : "https://apps.applozic.com"; Q.ajax({
                    url: M + n, skipEncryption: true, type: "post", async: typeof ai.async !== "undefined" ? ai.async : true, data: JSON.stringify(ai.data.alUser), contentType: "application/json", headers: { "Application-Key": A }, success: function success(aj) {
                        V.setEncryptionKey(aj.encryptionKey); f = btoa(aj.userId + ":" + aj.deviceKey); ae = aj.deviceKey; T = ai.data.alUser.password; v = ai.data.alUser.appModuleName; Q.setAjaxHeaders(f, A, aj.deviceKey, ai.data.alUser.password, ai.data.alUser.appModuleName); if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.getAttachmentHeaders = function () {
                var ai = { "UserId-Enabled": true, Authorization: "Basic " + f, "Application-Key": A, "Device-Key": ae }; if (T) {
                    ai["Access-Token"] = T;
                } return ai;
            }, Q.setAjaxHeaders = function (al, am, ai, aj, ak) {
                A = am; f = al; ae = ai; T = aj; v = ak;
            }; Q.ajax = function (av) {
                function an() {
                    for (var ay = 1; ay < arguments.length; ay++) {
                        for (var ax in arguments[ay]) {
                            if (arguments[ay].hasOwnProperty(ax)) {
                                arguments[0][ax] = arguments[ay][ax];
                            }
                        }
                    } return arguments[0];
                } var aq = an({}, {}, av); if (!(av.skipEncryption === true) && V.getEncryptionKey()) {
                    var ar = aesjs.util.convertStringToBytes(V.getEncryptionKey()); var al = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]; if (aq.type.toLowerCase() === "post") {
                        while (av.data && av.data.length % 16 != 0) {
                            av.data += " ";
                        } var ak = new aesjs.ModeOfOperation.ecb(ar); var aw = aesjs.util.convertStringToBytes(av.data); var at = ak.encrypt(aw); var ai = String.fromCharCode.apply(null, at); aq.data = btoa(ai);
                    } aq.success = function (aC) {
                        var aB = atob(aC); var ax = []; for (var aA = 0; aA < aB.length; aA++) {
                            ax.push(aB.charCodeAt(aA));
                        } var az = new aesjs.ModeOfOperation.ecb(ar); var aD = az.decrypt(ax); var ay = aesjs.util.convertBytesToString(aD); ay = ay.replace(/\\u0000/g, "").replace(/^\s*|\s*[\x00-\x10]*$/g, ""); if (V.isJsonString(ay)) {
                            av.success(JSON.parse(ay));
                        } else {
                            av.success(ay);
                        }
                    };
                } var am = new XMLHttpRequest(); var ap; var ao = true; var au; if (typeof aq.async !== "undefined" || av.async) {
                    ao = aq.async;
                } var aj = aq.type.toUpperCase(); if (aj === "GET" && typeof aq.data !== "undefined") {
                    aq.url = aq.url + "?" + aq.data;
                } am.open(aj, aq.url, ao); if (aj === "POST" || aj === "GET") {
                    if (typeof aq.contentType === "undefined") {
                        au = "application/x-www-form-urlencoded; charset=UTF-8";
                    } else {
                        au = aq.contentType;
                    } am.setRequestHeader("Content-Type", au);
                } M = M ? M : "https://apps.applozic.com"; if (aq.url.indexOf(M) !== -1) {
                    am.setRequestHeader("UserId-Enabled", true); if (f) {
                        am.setRequestHeader("Authorization", "Basic " + f);
                    } am.setRequestHeader("Application-Key", A); if (ae) {
                        am.setRequestHeader("Device-Key", ae);
                    } if (T) {
                        am.setRequestHeader("Access-Token", T);
                    } if (v) {
                        am.setRequestHeader("App-Module-Name", v);
                    }
                } if (typeof aq.data === "undefined") {
                    am.send();
                } else {
                    am.send(aq.data);
                } am.onreadystatechange = function () {
                    if (am.readyState === 4) {
                        if (am.status === 200) {
                            var ax = am.getResponseHeader("Content-Type"); if (typeof ax === "undefined" || ax === "null" || ax === null) {
                                ax = "";
                            } if (ax.toLowerCase().indexOf("text/html") != -1) {
                                ap = am.responseXML;
                            } else {
                                if (ax.toLowerCase().indexOf("application/json") != -1) {
                                    ap = JSON.parse(am.responseText);
                                } else {
                                    ap = am.responseText;
                                }
                            } aq.success(ap);
                        } else {
                            aq.error(ap);
                        }
                    }
                };
            }; Q.getMessages = function (aj) {
                if (aj.data.userId || aj.data.groupId) {
                    if (aj.data.pageSize === "undefined") {
                        aj.data.pageSize = 30;
                    }
                } else {
                    if (typeof aj.data.mainPageSize === "undefined") {
                        aj.data.mainPageSize = 60;
                    }
                } var ak = ad(aj.data); var ai = new Object(); Q.ajax({
                    url: M + o + "?" + ak, async: typeof aj.async !== "undefined" ? aj.async : true, type: "get", success: function success(al) {
                        ai.status = "success"; ai.data = al; if (aj.success) {
                            aj.success(ai);
                        } return;
                    }, error: function error(an, am, al) {
                        ai.status = "error"; if (aj.error) {
                            aj.error(ai);
                        }
                    }
                });
            }; Q.sendMessage = function (ai) {
                Q.ajax({
                    type: "POST", url: M + K, global: false, data: JSON.stringify(ai.data.message), async: typeof ai.async !== "undefined" ? ai.async : true, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.sendDeliveryUpdate = function (ai) {
                Q.ajax({
                    url: M + N, data: "key=" + ai.data.key, global: false, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.sendReadUpdate = function (ai) {
                Q.ajax({
                    url: M + l, data: "key=" + ai.data.key, global: false, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.deleteMessage = function (ai) {
                Q.ajax({
                    url: M + g + "?key=" + ai.data.key, global: false, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.updateReplyMessage = function (ai) {
                Q.ajax({
                    url: M + m + "?keys=" + ai.data.key, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.deleteConversation = function (ai) {
                Q.ajax({
                    url: M + ab, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, data: ad(ai.data), success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.createGroup = function (ai) {
                Q.ajax({
                    url: M + J, global: false, data: JSON.stringify(ai.data.group), type: "post", async: typeof ai.async !== "undefined" ? ai.async : true, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.loadGroups = function (ai) {
                if (ai.baseUrl) {
                    M = ai.baseUrl;
                } Q.ajax({
                    url: M + D, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.getGroupInfo = function (ai) {
                var aj = ai.data.groupId ? "?groupId=" + ai.data.groupId : "?clientGroupId=" + ai.data.clientGroupId; Q.ajax({
                    url: M + c + aj, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(ak) {
                        if (ai.success) {
                            ai.success(ak);
                        }
                    }, error: function error(ak) {
                        if (ai.error) {
                            ai.error(ak);
                        }
                    }
                });
            }; Q.addGroupMember = function (ai) {
                Q.ajax({
                    url: M + y, type: "POST", data: JSON.stringify(ai.data.group), async: typeof ai.async !== "undefined" ? ai.async : true, global: false, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.removeGroupMember = function (ai) {
                Q.ajax({
                    url: M + E, type: "POST", data: JSON.stringify(ai.data), async: typeof ai.async !== "undefined" ? ai.async : true, global: false, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.groupLeave = function (ai) {
                Q.ajax({
                    url: M + ac, type: "POST", data: JSON.stringify(ai.data), async: typeof ai.async !== "undefined" ? ai.async : true, global: false, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.groupUpdate = function (ai) {
                Q.ajax({
                    url: M + u, type: "POST", data: JSON.stringify(ai.data), async: typeof ai.async !== "undefined" ? ai.async : true, global: false, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj, ai.data.group);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.isUserPresentInGroup = function (ai) {
                Q.ajax({
                    url: M + X + "?userId=" + ai.data.userId + "&clientGroupId=" + ai.data.clientGroupId, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.groupUserCount = function (ai) {
                Q.ajax({
                    url: M + P + "?clientGroupIds=" + ai.data.clientGroupId, type: "get", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.groupDelete = function (ai) {
                Q.ajax({
                    url: M + ac + "?clientGroupId=" + ai.data.clientGroupId, type: "GET", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.createUserFriendList = function (ai) {
                Q.ajax({
                    url: M + i + ai.data.group.groupName + "/add/", type: "POST", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, data: JSON.stringify(ai.data.group.groupMemberList), contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj); ALStorage.setFriendListGroupName(ai.data.group.groupName);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.createOpenFriendList = function (ai) {
                Q.ajax({
                    url: M + i + ai.data.group.groupName + "/add/members", type: "POST", data: JSON.stringify(ai.data.group), async: typeof ai.async !== "undefined" ? ai.async : true, global: false, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj); ALStorage.setFriendListGroupName(ai.data.group.groupName); ALStorage.setFriendListGroupType(ai.data.group.type);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.getFriendList = function (ai) {
                var aj = ai.data.type !== "null" ? "/get?groupType=9" : "/get"; ai.data.url = ai.data.url ? ai.data.url : aj; Q.ajax({
                    url: M + i + ai.data.groupName + ai.data.url, type: "GET", async: typeof ai.data.async !== "undefined" ? ai.data.async : true, global: false, contentType: "application/json", success: function success(ak) {
                        if (ai.success) {
                            ai.success(ak);
                        }
                    }, error: function error(ak) {
                        if (ai.error) {
                            ai.error(ak);
                        }
                    }
                });
            }; Q.removeUserFromFriendList = function (ai) {
                var aj = ai.group.type ? "/remove?userId=" + ai.group.userId + "&groupType=9" : "/remove?userId=" + ai.group.userId; Q.ajax({
                    url: M + i + ai.group.groupName + aj, type: "Post", contentType: "application/json", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(ak) {
                        if (ai.success) {
                            ai.success(ak);
                        }
                    }, error: function error(ak) {
                        if (ai.error) {
                            ai.error(ak);
                        }
                    }
                });
            }; Q.deleteFriendList = function (ai) {
                var _Q$ajax;

                var aj = ai.group.type ? "/delete?groupType=9" : "/delete"; Q.ajax((_Q$ajax = { url: M + i + ai.group.groupName + aj, type: "GET", async: false, contentType: "application/json" }, _defineProperty(_Q$ajax, "async", typeof ai.async !== "undefined" ? ai.async : true), _defineProperty(_Q$ajax, "success", function success(ak) {
                    if (ai.success) {
                        ai.success(ak);
                    }
                }), _defineProperty(_Q$ajax, "error", function error(ak) {
                    if (ai.error) {
                        ai.error(ak);
                    }
                }), _Q$ajax));
            }; Q.getUserDetail = function (ai) {
                Q.ajax({
                    url: M + ag, data: JSON.stringify({ userIdList: ai.data.userIdList }), type: "POST", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.updateUserDetail = function (ai) {
                Q.ajax({
                    url: M + k, data: JSON.stringify(ai.data), type: "POST", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.updatePassword = function (ai) {
                Q.ajax({
                    url: M + aa + "?oldPassword=" + ai.data.oldPassword + "&newPassword=" + ai.data.newPassword, type: "GET", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.getContactList = function (ai) {
                var aj = ai.baseUrl ? ai.baseUrl : M; Q.ajax({
                    url: aj + ai.url, type: "GET", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(ak) {
                        if (ai.success) {
                            ai.success(ak);
                        }
                    }, error: function error(ak) {
                        if (ai.error) {
                            ai.error(ak);
                        }
                    }
                });
            }; Q.userChatMute = function (ai) {
                Q.ajax({
                    url: M + U + "?userId=" + ai.data.userId + "&notificationAfterTime=" + ai.data.notificationAfterTime, type: "post", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.groupMute = function (ai) {
                var aj = {}; aj.clientGroupId = ai.data.clientGroupId; aj.notificationAfterTime = ai.data.notificationAfterTime; Q.ajax({
                    url: M + q, type: "post", data: JSON.stringify(aj), contentType: "application/json", success: function success(ak) {
                        if (ai.success) {
                            ai.success(ak);
                        }
                    }, error: function error(ak) {
                        if (ai.error) {
                            ai.error(ak);
                        }
                    }
                });
            }; Q.syncMuteUserList = function (ai) {
                Q.ajax({
                    url: M + C, type: "get", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.blockUser = function (ai) {
                Q.ajax({
                    url: M + Y + "?userId=" + ai.data.userId + "&block=" + ai.data.isBlock, type: "GET", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.unBlockUser = function (ai) {
                Q.ajax({
                    url: M + L + "?userId=" + ai.data.userId, type: "GET", async: typeof ai.async !== "undefined" ? ai.async : true, global: false, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.sendConversationCloseUpdate = function (ai) {
                var aj = "id=" + ai.conversationId; Q.ajax({ url: M + af, data: aj, global: false, type: "get", success: function success() { }, error: function error() { } });
            }; Q.fileUpload = function (ai) {
                Q.ajax({
                    type: "GET", skipEncryption: true, url: ai.data.url, global: false, data: "data=" + new Date().getTime(), crosDomain: true, success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.sendAttachment = function (ai) {
                var aj = new FormData(); var ak = new XMLHttpRequest(); ak.addEventListener("load", function (an) {
                    var al = this.responseText; var am = ai.data.messagePxy; if (al) {
                        am.fileMeta = JSON.parse(al); Applozic.ALApiService.sendMessage({
                            data: am, success: function success(ao) {
                                console.log(ao);
                            }, error: function error() { }
                        });
                    }
                }); aj.append("file", ai.data.file); ak.open("post", M + I, true); ak.setRequestHeader("UserId-Enabled", true); ak.setRequestHeader("Authorization", "Basic " + f); ak.setRequestHeader("Application-Key", A); ak.setRequestHeader("Device-Key", ae); if (T) {
                    ak.setRequestHeader("Access-Token", T);
                } ak.send(aj);
            }; Q.deleteFileMeta = function (ai) {
                Q.ajax({
                    url: ai.data.url, skipEncryption: true, type: "post", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.addMessageInbox = function (ai) {
                Q.ajax({
                    type: "GET", url: M + h, global: false, data: "sender=" + encodeURIComponent(ai.data.sender) + "&messageContent=" + encodeURIComponent(ai.data.messageContent), contentType: "text/plain", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.conversationReadUpdate = function (ai) {
                Q.ajax({
                    url: M + S, data: ai.data, global: false, type: "get", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.success(aj);
                        }
                    }
                });
            }; Q.sendSubscriptionIdToServer = function (aj) {
                var ai = aj.data.subscriptionId; Q.ajax({
                    url: M + F, skipEncryption: true, type: "post", data: "registrationId=" + ai, success: function success(ak) { }, error: function error(am, al, ak) {
                        if (am.status === 401) {
                            sessionStorage.clear(); console.log("Please reload page.");
                        }
                    }
                });
            }; Q.getTopicId = function (ai) {
                var aj = "id=" + ai.data.conversationId; Q.ajax({
                    url: M + B + "?" + aj, type: "get", success: function success(ak) {
                        if (ai.success) {
                            ai.success(ak);
                        }
                    }, error: function error(ak) {
                        if (ai.error) {
                            ai.success(ak);
                        }
                    }
                });
            }; Q.getContactDisplayName = function (aj) {
                var ai = aj.data.userIdArray; if (ai.length > 0 && ai[0]) {
                    var an = ""; var am = ai.filter(function (ao, ap) {
                        return ai.indexOf(ao) === ap;
                    }); for (var al = 0; al < am.length; al++) {
                        var ak = am[al]; if (typeof MCK_CONTACT_NAME_MAP[ak] === "undefined") {
                            an += "userIds=" + encodeURIComponent(ak) + "&";
                        }
                    } if (an.lastIndexOf("&") === an.length - 1) {
                        an = an.substring(0, an.length - 1);
                    } if (an) {
                        Q.ajax({
                            url: M + s, data: an, global: false, type: "get", success: function success(ao) {
                                if (aj.success) {
                                    aj.success(ao);
                                }
                            }, error: function error(ao) {
                                if (aj.error) {
                                    aj.success(ao);
                                }
                            }
                        });
                    }
                }
            }; Q.getUserStatus = function (ai) {
                Q.ajax({
                    url: M + G, type: "get", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.success(aj);
                        }
                    }
                });
            }; Q.fetchConversationByTopicId = function (ai) {
                var aj = "topic=" + ai.data.topicId; if (ai.data.tabId) {
                    aj += "" + ai.data.isGroup === "true" ? "&groupId=" + ai.data.tabId : "&userId=" + encodeURIComponent(ai.data.tabId);
                } else {
                    if (ai.data.clientGroupId) {
                        aj += "&clientGroupId=" + ai.data.clientGroupId;
                    } else {
                        return false;
                    }
                } if (ai.data.pageSize) {
                    aj += "&pageSize=" + ai.data.pageSize;
                } Q.ajax({
                    url: M + Z, data: aj, type: "get", success: function success(ak) {
                        if (ai.success) {
                            ai.success(ak);
                        }
                    }, error: function error(ak) {
                        if (ai.error) {
                            ai.success(ak);
                        }
                    }
                });
            }; Q.getConversationId = function (ai) {
                Q.ajax({
                    url: M + e, global: false, data: w.JSON.stringify(ai.data), type: "post", contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.success(aj);
                        }
                    }
                });
            }; Q.registerClientApi = function (ai) {
                Q.ajax({
                    url: M + W, type: "post", data: JSON.stringify(ai.data.userPxy), contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.success(aj);
                        }
                    }
                });
            }; Q.logout = function (ai) {
                Q.ajax({
                    url: M + ah, type: "post", async: typeof ai.async !== "undefined" ? ai.async : true, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; Q.getUsersByRole = function (ai) {
                var aj = ad(ai.data); Q.ajax({
                    url: M + p + "?" + aj, global: false, type: "get", contentType: "application/json", success: function success(ak) {
                        if (ai.success) {
                            ai.success(ak);
                        }
                    }, error: function error(ak) {
                        if (ai.error) {
                            ai.success(ak);
                        }
                    }
                });
            }; Q.pushNotificationLogout = function (ai) {
                Q.ajax({
                    url: M + j, type: "post", async: typeof ai.async !== "undefined" ? ai.async : true, contentType: "application/json", success: function success(aj) {
                        if (ai.success) {
                            ai.success(aj);
                        }
                    }, error: function error(aj) {
                        if (ai.error) {
                            ai.error(aj);
                        }
                    }
                });
            }; return Q;
        } if (typeof ALApiService === "undefined") {
            a.Applozic.ALApiService = b();
        } else {
            console.log("ALApiService already defined.");
        }
    })(window); (function (a) {
        function b() {
            var h = {}; var c; h.events = {}; var m = null; h.stompClient = null; var i = ""; h.typingSubscriber = null; var k = []; var j; var o; var g; h.mck_typing_status = 0; var n; var l = ""; var e = "https://apps.applozic.com"; h.MCK_TOKEN; h.USER_DEVICE_KEY; var f = new MckUtils(); h.init = function (s, q, r) {
                if (s) {
                    c = s;
                } if (typeof q !== "undefined") {
                    h.MCK_TOKEN = q.token; h.USER_DEVICE_KEY = q.deviceKey; e = q.websocketUrl;
                } h.events = r; if (typeof e !== "undefined") {
                    var p = !f.startsWith(e, "https") ? "15674" : "15675"; if (typeof SockJS === "function") {
                        if (!l) {
                            l = new SockJS(e + ":" + p + "/stomp");
                        } h.stompClient = Stomp.over(l); h.stompClient.heartbeat.outgoing = 0; h.stompClient.heartbeat.incoming = 0; h.stompClient.onclose = function () {
                            h.disconnect();
                        }; h.stompClient.connect("guest", "guest", h.onConnect, h.onError, "/"); a.addEventListener("beforeunload", function (u) {
                            var t = u.target.activeElement.href; if (!t || 0 === t.length) {
                                h.disconnect();
                            }
                        });
                    }
                }
            }; h.checkConnected = function (p) {
                if (h.stompClient.connected) {
                    if (j) {
                        clearInterval(j);
                    } if (o) {
                        clearInterval(o);
                    } j = setInterval(function () {
                        h.connectToSocket(p);
                    }, 600000); o = setInterval(function () {
                        h.sendStatus(1);
                    }, 1200000);
                } else {
                    h.connectToSocket(p);
                }
            }; h.connectToSocket = function (p) {
                if (typeof h.connectToSocket === "function") {
                    h.events.connectToSocket(p);
                }
            }; h.stopConnectedCheck = function () {
                if (j) {
                    clearInterval(j);
                } if (o) {
                    clearInterval(o);
                } j = ""; o = ""; h.disconnect();
            }; h.disconnect = function () {
                if (h.stompClient && h.stompClient.connected) {
                    h.sendStatus(0); h.stompClient.disconnect(); l = "";
                }
            }; h.unsubscibeToTypingChannel = function () {
                if (h.stompClient && h.stompClient.connected) {
                    if (h.typingSubscriber) {
                        if (h.mck_typing_status === 1) {
                            h.sendTypingStatus(0, i);
                        } h.typingSubscriber.unsubscribe();
                    }
                } h.typingSubscriber = null;
            }; h.unsubscibeToNotification = function () {
                if (h.stompClient && h.stompClient.connected) {
                    if (m) {
                        m.unsubscribe();
                    }
                } m = null;
            }; h.subscibeToTypingChannel = function (p) {
                if (h.stompClient && h.stompClient.connected) {
                    h.typingSubscriber = h.stompClient.subscribe("/topic/typing-" + c + "-" + p, h.onTypingStatus);
                } else {
                    h.reconnect();
                }
            }; h.subscribeToOpenGroup = function (q) {
                if (h.stompClient && h.stompClient.connected) {
                    var p = h.stompClient.subscribe("/topic/group-" + c + "-" + q.contactId, h.onOpenGroupMessage); k.push(p.id); g[q.contactId] = p.id;
                } else {
                    h.reconnect();
                }
            }; h.sendTypingStatus = function (p, s, r, q) {
                h.mck_typing_status = s; if (h.stompClient && h.stompClient.connected) {
                    if (p === 1 && h.mck_typing_status === 1) {
                        h.stompClient.send("/topic/typing-" + c + "-" + i, { "content-type": "text/plain" }, c + "," + r + "," + p);
                    } if (q) {
                        if (q === i && p === h.mck_typing_status && p === 1) {
                            return;
                        } i = q; h.stompClient.send("/topic/typing-" + c + "-" + q, { "content-type": "text/plain" }, c + "," + r + "," + p); setTimeout(function () {
                            h.mck_typing_status = 0;
                        }, 60000);
                    } else {
                        if (p === 0) {
                            h.stompClient.send("/topic/typing-" + c + "-" + i, { "content-type": "text/plain" }, c + "," + r + "," + p);
                        }
                    } h.mck_typing_status = p;
                }
            }; h.onTypingStatus = function (p) {
                if (typeof h.events.onTypingStatus === "function") {
                    h.events.onTypingStatus(p);
                }
            }; h.reconnect = function () {
                h.unsubscibeToTypingChannel(); h.unsubscibeToNotification(); h.disconnect(); var p = {}; p.token = h.MCK_TOKEN; p.deviceKey = h.USER_DEVICE_KEY; p.websocketUrl = e; h.init(c, p, h.events);
            }; h.onError = function (p) {
                console.log("Error in channel notification. " + p); if (typeof h.events.onConnectFailed === "function") {
                    h.events.onConnectFailed();
                }
            }; h.sendStatus = function (p) {
                if (h.stompClient && h.stompClient.connected) {
                    h.stompClient.send("/topic/status-v2", { "content-type": "text/plain" }, h.MCK_TOKEN + "," + h.USER_DEVICE_KEY + "," + p);
                }
            }; h.onConnect = function () {
                if (h.stompClient.connected) {
                    if (m) {
                        h.unsubscibeToNotification();
                    } m = h.stompClient.subscribe("/topic/" + h.MCK_TOKEN, h.onMessage); h.sendStatus(1); h.checkConnected(true);
                } else {
                    setTimeout(function () {
                        m = h.stompClient.subscribe("/topic/" + h.MCK_TOKEN, h.onMessage); h.sendStatus(1); h.checkConnected(true);
                    }, 5000);
                } if (typeof h.events.onConnect === "function") {
                    h.events.onConnect();
                }
            }; h.onOpenGroupMessage = function (p) {
                if (typeof h.events.onOpenGroupMessage === "function") {
                    h.events.onOpenGroupMessage(p);
                }
            }; h.onMessage = function (v) {
                if (m != null && m.id === v.headers.subscription) {
                    var x = JSON.parse(v.body); var s = x.type; if (typeof h.events.onMessage === "function") {
                        h.events.onMessage(x);
                    } if (s === "APPLOZIC_04" || s === "MESSAGE_DELIVERED") {
                        h.events.onMessageDelivered(x);
                    } else {
                        if (s === "APPLOZIC_08" || s === "MT_MESSAGE_DELIVERED_READ") {
                            h.events.onMessageRead(x);
                        } else {
                            if (s === "APPLOZIC_05") {
                                h.events.onMessageDeleted(x);
                            } else {
                                if (s === "APPLOZIC_27") {
                                    h.events.onConversationDeleted(x);
                                } else {
                                    if (s === "APPLOZIC_11") {
                                        h.events.onUserConnect(x.message);
                                    } else {
                                        if (s === "APPLOZIC_12") {
                                            var r = x.message.split(",")[1]; h.events.onUserDisconnect({ userId: q, lastSeenAtTime: r });
                                        } else {
                                            if (s === "APPLOZIC_29") {
                                                h.events.onConversationReadFromOtherSource(x);
                                            } else {
                                                if (s === "APPLOZIC_28") {
                                                    h.events.onConversationRead(x);
                                                } else {
                                                    if (s === "APPLOZIC_16") {
                                                        var p = x.message.split(":")[0]; var q = x.message.split(":")[1]; h.events.onUserBlocked({ status: p, userId: q });
                                                    } else {
                                                        if (s === "APPLOZIC_17") {
                                                            var p = x.message.split(":")[0]; var q = x.message.split(":")[1]; h.events.onUserUnblocked({ status: p, userId: q });
                                                        } else {
                                                            if (s === "APPLOZIC_18") {
                                                                h.events.onUserActivated();
                                                            } else {
                                                                if (s === "APPLOZIC_19") {
                                                                    h.events.onUserDeactivated();
                                                                } else {
                                                                    var u = x.message; if (s === "APPLOZIC_03") {
                                                                        h.events.onMessageSentUpdate({ messageKey: u.key });
                                                                    } else {
                                                                        if (s === "APPLOZIC_01" || s === "MESSAGE_RECEIVED") {
                                                                            var t = alMessageService.getMessageFeed(u); h.events.onMessageReceived({ message: t });
                                                                        } else {
                                                                            if (s === "APPLOZIC_02") {
                                                                                var t = alMessageService.getMessageFeed(u); h.events.onMessageSent({ message: t });
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }; return h;
        } if (typeof ALSocket === "undefined") {
            a.Applozic.ALSocket = b();
        } else {
            console.log("ALSocket already defined.");
        }
    })(window); var mckNotificationUtils = new MckNotificationUtils(); var alNotificationService = new AlNotificationService(); function AlNotificationService() {
        var i = this; var a; var c; var f; var e; var h; var b = "/rest/ws/plugin/update/sw/id"; var g = true; i.init = function (j) {
            a = typeof j.swNotification === "boolean" ? j.swNotification : false; c = j.contactDisplayImage; f = j.notificationIconLink; e = typeof j.desktopNotification === "boolean" ? j.desktopNotification : false;
        }; i.unsubscribeToServiceWorker = function () {
            if (h) {
                navigator.serviceWorker.ready.then(function (j) {
                    h.unsubscribe().then(function (k) {
                        h = null; console.log("Unsubscribed to notification successfully");
                    });
                });
            }
        }; i.sendSubscriptionIdToServer = function () {
            if (h) {
                var j = h.endpoint.split("/").slice(-1)[0]; if (j) {
                    window.Applozic.ALApiService.sendSubscriptionIdToServer({ data: { subscriptionId: j }, success: function success(k) { }, error: function error() { } });
                }
            }
        }; i.subscribeToServiceWorker = function () {
            if (a) {
                if ("serviceWorker" in navigator) {
                    navigator.serviceWorker.register("./service-worker.js", { scope: "./" }); navigator.serviceWorker.ready.then(function (j) {
                        j.pushManager.subscribe({ userVisibleOnly: true }).then(function (k) {
                            console.log("The reg ID is:: ", k.endpoint.split("/").slice(-1)); h = k; i.sendSubscriptionIdToServer();
                        });
                    });
                }
            }
        };
    } function MckNotificationUtils() {
        var i = this; var j = "default",
            b = "granted",
            h = "denied",
            e = [b, j, h],
            c = function () {
                var m = false; try {
                    m = !!(w.Notification || w.webkitNotifications || navigator.mozNotification || w.external && w.external.msIsSiteMode() !== undefined);
                } catch (n) { } return m;
            }(),
            g = function g(m) {
                return m && m.constructor === Function;
            },
            f = function f(m) {
                return m && m.constructor === String;
            },
            l = function l(m) {
                return m && m.constructor === Object;
            },
            a = Math.floor(Math.random() * 10 + 1),
            k = function k() { }; i.permissionLevel = function () {
                var m; if (!c) {
                    return;
                } if (w.Notification && w.Notification.permissionLevel) {
                    m = w.Notification.permissionLevel();
                } else {
                    if (w.webkitNotifications && w.webkitNotifications.checkPermission) {
                        m = e[w.webkitNotifications.checkPermission()];
                    } else {
                        if (w.Notification && w.Notification.permission) {
                            m = w.Notification.permission;
                        } else {
                            if (navigator.mozNotification) {
                                m = b;
                            } else {
                                if (w.external && w.external.msIsSiteMode() !== undefined) {
                                    m = w.external.msIsSiteMode() ? b : j;
                                }
                            }
                        }
                    }
                } return m;
            }; i.requestPermission = function (n) {
                var m = g(n) ? n : k; if (w.webkitNotifications && w.webkitNotifications.checkPermission) {
                    w.webkitNotifications.requestPermission(m);
                } else {
                    if (w.Notification && w.Notification.requestPermission) {
                        w.Notification.requestPermission(m);
                    }
                }
            }; i.isChrome = function () {
                return (/chrom(e|ium)/.test(w.navigator.userAgent.toLowerCase())
                );
            }; i.getNotification = function (m, n, r, q) {
                if (q) {
                    q.play(); setTimeout(function () {
                        q.stop();
                    }, 1000);
                } var p; if (w.Notification) {
                    var o = { icon: n, body: r }; p = new w.Notification(m, o); p.onclick = function () {
                        w.focus(); this.close();
                    };
                } else {
                    if (w.webkitNotifications) {
                        p = w.webkitNotifications.createNotification(n, m, r); if (q) {
                            p.show();
                        } if (i.isChrome()) {
                            p.onclick = function () {
                                w.focus(); this.cancel();
                            };
                        } p.show(); setTimeout(function () {
                            p.cancel();
                        }, 30000);
                    } else {
                        if (navigator.mozNotification) {
                            p = navigator.mozNotification.createNotification(m, r, n); p.show();
                        } else {
                            if (w.external && w.external.msIsSiteMode()) {
                                w.external.msSiteModeClearIconOverlay(); w.external.msSiteModeSetIconOverlay(n, m); w.external.msSiteModeActivate(); p = { ieVerification: a + 1 };
                            }
                        }
                    }
                } return p;
            }; i.sendDesktopNotification = function (m, n, r, q) {
                if (i.permissionLevel() !== b) {
                    w.Notification.requestPermission();
                } if (i.permissionLevel() === b) {
                    var p; if (q) {
                        p = i.getNotification(m, n, r, q);
                    } else {
                        p = i.getNotification(m, n, r);
                    } var o = i.getWrapper(p); if (p && !p.ieVerification && p.addEventListener) {
                        p.addEventListener("show", function () {
                            var s = o; w.setTimeout(function () {
                                s.close();
                            }, 30000);
                        });
                    }
                }
            }; i.getWrapper = function (m) {
                return {
                    close: function close() {
                        if (m) {
                            if (m.close) {
                                m.close();
                            } else {
                                if (m.cancel) {
                                    m.cancel();
                                } else {
                                    if (w.external && w.external.msIsSiteMode()) {
                                        if (m.ieVerification === a) {
                                            w.external.msSiteModeClearIconOverlay();
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            };
    } var mckGroupUtils = new MckGroupUtils(); var mckGroupService = new MckGroupService(); function MckGroupUtils() {
        var a = this; a.getGroup = function (b) {
            if (_typeof(MCK_GROUP_MAP[b]) === "object") {
                return MCK_GROUP_MAP[b];
            } else {
                return;
            }
        }; a.getGroupByClientGroupId = function (b) {
            if (_typeof(MCK_CLIENT_GROUP_MAP[b]) === "object") {
                return MCK_CLIENT_GROUP_MAP[b];
            } else {
                return;
            }
        }; a.addGroup = function (f) {
            var e = f.name ? f.name : f.id; var g = []; $applozic.each(f.groupUsers, function (j, h) {
                if (h.userId) {
                    g[h.userId] = h;
                }
            }); var c = typeof f.removedMembersId !== "undefined" ? f.removedMembersId : []; var b = { contactId: f.id.toString(), htmlId: mckContactUtils.formatContactId("" + f.id), displayName: e, value: f.id.toString(), adminName: f.adminId ? f.adminId : f.adminName, type: f.type, members: f.membersId ? f.membersId : f.membersName, imageUrl: f.imageUrl, users: g, userCount: f.userCount, removedMembersId: c, clientGroupId: f.clientGroupId, isGroup: true, deletedAtTime: f.deletedAtTime, metadata: f.metadata }; MCK_GROUP_MAP[f.id] = b; if (f.clientGroupId) {
                MCK_CLIENT_GROUP_MAP[f.clientGroupId] = b;
            } return b;
        }; a.createGroup = function (b) {
            var c = { contactId: b.toString(), htmlId: mckContactUtils.formatContactId("" + b), displayName: b.toString(), value: b.toString(), type: 2, adminName: "", imageUrl: "", userCount: "", users: [], removedMembersId: [], clientGroupId: "", isGroup: true, deletedAtTime: "" }; MCK_GROUP_MAP[b] = c; return c;
        };
    } function MckGroupService() {
        var j = this; var i; var h; var m; var k = []; var f = []; var g = "/rest/ws/group/list"; var a = "/rest/ws/group/info"; var l = "/rest/ws/group/left"; var e = "/rest/ws/group/update"; var c = "/rest/ws/group/add/member"; var b = "/rest/ws/group/remove/member"; var MCK_GROUP_ARRAY = new Array(); j.init = function (n) {
            i = n.visitor; h = i ? "guest" : $applozic.trim(n.userId); m = n.openGroupSettings;
        }; j.loadGroups = function (o) {
            var n = new Object(); window.Applozic.ALApiService.loadGroups({
                baseUrl: MCK_BASE_URL, success: function success(p) {
                    if (p.status === "success") {
                        n.status = "success"; n.data = p.response; if (o.apzCallback) {
                            o.apzCallback(n);
                        }
                    } else {
                        n.status = "error";
                    } if (o.callback) {
                        o.callback(n);
                    }
                }, error: function error() {
                    console.log("Unable to load groups. Please reload page."); n.status = "error"; if (o.callback) {
                        o.callback(n);
                    } if (o.apzCallback) {
                        o.apzCallback(n);
                    }
                }
            });
        }; j.getGroupFeed = function (p) {
            var o = {}; if (typeof p.callback === "function" || typeof p.apzCallback === "function") {
                var n = new Object();
            } else {
                return;
            } if (p.groupId) {
                o.groupId = p.groupId;
            } else {
                if (p.clientGroupId) {
                    o.clientGroupId = p.clientGroupId;
                } else {
                    if (typeof p.callback === "function") {
                        n.status = "error"; n.errorMessage = "GroupId or Client GroupId Required"; p.callback(n);
                    } return;
                }
            } if (p.conversationId) {
                o.conversationId = p.conversationId;
            } Applozic.ALApiService.getGroupInfo({
                data: o, success: function success(q) {
                    if (q.status === "success") {
                        var r = q.response; if (r + "" === "null" || (typeof r === "undefined" ? "undefined" : _typeof(r)) !== "object") {
                            q.status = "error"; q.errorMessage = "GroupId not found";
                        } else {
                            var s = mckGroupUtils.addGroup(r); q.status = "success"; q.data = s;
                        }
                    } else {
                        if (data.status === "error") {
                            q.status = "error"; q.errorMessage = data.errorResponse[0].description;
                        }
                    } if (p.callback) {
                        p.callback(q);
                    } if (p.apzCallback) {
                        if (q.status === "success") {
                            q.data = r;
                        } p.apzCallback(q, p);
                    }
                }, error: function error() {
                    console.log("Unable to load group. Please reload page."); n.status = "error"; n.errorMessage = "Please reload page."; if (p.callback) {
                        p.callback(n);
                    } if (p.apzCallback) {
                        p.apzCallback(n, p);
                    }
                }
            });
        }; j.leaveGroup = function (p) {
            var o = {}; var n = new Object(); if (p.groupId) {
                o.groupId = p.groupId;
            } else {
                if (p.clientGroupId) {
                    o.clientGroupId = p.clientGroupId;
                } else {
                    n.status = "error"; n.errorMessage = "GroupId or Client GroupId Required"; if (p.callback) {
                        p.callback(n);
                    } return;
                }
            } Applozic.ALApiService.groupLeave({
                data: o, success: function success(r) {
                    if (r.status === "success") {
                        if (p.clientGroupId) {
                            var q = mckGroupUtils.getGroupByClientGroupId(p.clientGroupId); if ((typeof q === "undefined" ? "undefined" : _typeof(q)) === "object") {
                                p.groupInfo = q.contactId;
                            }
                        } n.status = "success"; n.data = { groupId: p.groupId };
                    } else {
                        n.status = "error"; n.errorMessage = r.errorResponse[0].description;
                    } if (p.callback) {
                        p.callback(n);
                    } if (p.apzCallback) {
                        p.apzCallback(n, { groupId: p.groupId });
                    }
                }, error: function error() {
                    console.log("Unable to process your request. Please reload page."); n.status = "error"; n.errorMessage = ""; if (p.callback) {
                        p.callback(n);
                    } if (p.apzCallback) {
                        p.apzCallback(n);
                    }
                }
            });
        }; j.removeGroupMemberFromChat = function (p) {
            var o = {}; var n = new Object(); if (p.groupId) {
                o.groupId = p.groupId;
            } else {
                if (p.clientGroupId) {
                    o.clientGroupId = p.clientGroupId;
                } else {
                    n.status = "error"; n.errorMessage = "GroupId or Client GroupId Required"; if (typeof p.callback === "function") {
                        p.callback(n);
                    } return;
                }
            } o.userId = p.userId; Applozic.ALApiService.removeGroupMember({
                data: o, success: function success(q) {
                    if (q.status === "success") {
                        if (p.clientGroupId) {
                            var r = mckGroupUtils.getGroupByClientGroupId(p.clientGroupId); if ((typeof r === "undefined" ? "undefined" : _typeof(r)) === "object") {
                                p.groupId = r.contactId;
                            }
                        } q.status = "success";
                    } else {
                        q.status = "error"; q.errorMessage = data.errorResponse[0].description;
                    } if (p.callback) {
                        p.callback(q);
                    } if (p.apzCallback) {
                        p.apzCallback(q, p);
                    }
                }, error: function error() {
                    console.log("Unable to process your request. Please reload page."); n.status = "error"; n.errorMessage = ""; if (p.callback) {
                        p.callback(n);
                    } if (p.apzCallback) {
                        p.apzCallback(n);
                    } p.apzCallback(n);
                }
            });
        }; j.addGroupMember = function (p) {
            var o = {}; var n = new Object(); if (p.groupId) {
                o.groupId = p.groupId;
            } else {
                if (p.clientGroupId) {
                    o.clientGroupId = p.clientGroupId;
                } else {
                    if (typeof p.callback === "function") {
                        p.callback(n);
                    } return;
                }
            } o.userId = p.userId; if (typeof p.role !== "undefined") {
                o.role = p.role;
            } Applozic.ALApiService.addGroupMember({
                data: { group: o }, success: function success(q) {
                    if (q.status === "success") {
                        if (p.clientGroupId) {
                            var r = mckGroupUtils.getGroupByClientGroupId(p.clientGroupId); if ((typeof r === "undefined" ? "undefined" : _typeof(r)) === "object") {
                                p.groupId = r.contactId;
                            }
                        } n.status = "success"; n.data = q.response;
                    } else {
                        n.status = "error"; n.errorMessage = q.errorResponse[0].description;
                    } if (p.callback) {
                        p.callback(n);
                    } if (p.apzCallback) {
                        p.apzCallback(n, p);
                    }
                }, error: function error() {
                    console.log("Unable to process your request. Please reload page."); n.status = "error"; n.errorMessage = ""; if (p.callback) {
                        p.callback(n);
                    } if (p.apzCallback) {
                        p.apzCallback(n);
                    }
                }
            });
        }; j.updateGroupInfo = function (p) {
            var o = {}; var n = new Object(); if (p.groupId) {
                o.groupId = p.groupId;
            } else {
                if (p.clientGroupId) {
                    o.clientGroupId = p.clientGroupId;
                } else {
                    if (typeof p.callback === "function") {
                        n.status = "error"; n.errorMessage = "GroupId or Client GroupId Required"; p.callback(n);
                    } return;
                }
            } if (p.name) {
                o.newName = p.name;
            } if (p.imageUrl) {
                o.imageUrl = p.imageUrl;
            } if (p.users && p.users.length > 0) {
                o.users = p.users;
            } Applozic.ALApiService.groupUpdate({
                data: o, success: function success(q, r) {
                    if (q.status === "success") {
                        if (p.clientGroupId) {
                            var r = mckGroupLayout.getGroupByClientGroupId(p.clientGroupId); if ((typeof r === "undefined" ? "undefined" : _typeof(r)) === "object") {
                                p.groupId = r.contactId;
                            }
                        } n.status = "success"; n.data = q.response;
                    } else {
                        n.status = "error"; n.errorMessage = q.errorResponse[0].description;
                    } if (p.callback) {
                        p.callback(n);
                    } if (p.apzCallback) {
                        p.apzCallback(n, { groupId: p.groupId, groupInfo: r });
                    }
                }, error: function error() {
                    console.log("Unable to process your request. Please reload page."); n.status = "error"; n.errorMessage = "Unable to process your request. Please reload page."; if (p.callback) {
                        p.callback(n);
                    } if (p.apzCallback) {
                        p.apzCallback(n);
                    }
                }
            });
        }; j.sendGroupMessage = function (q) {
            if ((typeof q === "undefined" ? "undefined" : _typeof(q)) === "object") {
                q = $applozic.extend(true, {}, message_default_options, q); var o = q.message; if (!q.groupId && !q.clientGroupId) {
                    return "groupId or clientGroupId required";
                } if (typeof o === "undefined" || o === "") {
                    return "message field required";
                } if (q.type > 12) {
                    return "invalid message type";
                } o = $applozic.trim(o); var n = { type: q.messageType, contentType: q.type, message: o }; if (q.groupId) {
                    n.groupId = $applozic.trim(q.groupId);
                } else {
                    if (q.clientGroupId) {
                        var p = mckGroupUtils.getGroupByClientGroupId(q.clientGroupId); if (typeof p === "undefined") {
                            return "group not found";
                        } n.clientGroupId = $applozic.trim(q.clientGroupId);
                    }
                } mckMessageService.sendMessage(n); return "success";
            } else {
                return "Unsupported format. Please check format";
            }
        }; j.getContactFromGroupOfTwo = function (p, q) {
            var n; for (var o = 0; o < p.members.length; o++) {
                n = "" + p.members[o]; if (h === n) {
                    continue;
                } if (typeof q === "function") {
                    q(n);
                }
            }
        }; j.addGroupFromMessage = function (o, r, q) {
            var n = o.groupId; var p = mckGroupUtils.getGroup("" + n); if (typeof p === "undefined") {
                p = mckGroupUtils.createGroup(n); mckGroupService.loadGroups({ apzCallback: mckGroupLayout.loadGroups });
            } if (typeof q === "function") {
                q(p, o, r);
            }
        }; j.isGroupDeleted = function (o, p) {
            if (p) {
                var n = mckGroupLayout.getDeletedAtTime(o); return typeof n !== "undefined" && n > 0;
            } return false;
        }; j.loadGroupsCallback = function (o) {
            var n = o.data; MCK_GROUP_ARRAY.length = 0; $applozic.each(n, function (p, q) {
                if (typeof q.id !== "undefined") {
                    var q = mckGroupUtils.addGroup(q); MCK_GROUP_ARRAY.push(q);
                }
            });
        }; j.getGroupDisplayName = function (q) {
            if (_typeof(MCK_GROUP_MAP[q]) === "object") {
                var r = MCK_GROUP_MAP[q]; var o = r.displayName; if (r.type === 7) {
                    var n = j.getContactFromGroupOfTwo(r); if (typeof n !== "undefined") {
                        o = mckMessageLayout.getTabDisplayName(n.contactId, false);
                    }
                } if (r.type === 3) {
                    if (o.indexOf(h) !== -1) {
                        o = o.replace(h, "").replace(":", ""); if (typeof MCK_GETUSERNAME === "function") {
                            var p = MCK_GETUSERNAME(o); o = p ? p : o;
                        }
                    }
                } if (!o && r.type === 5) {
                    o = "Broadcast";
                } if (!o) {
                    o = r.contactId;
                } return o;
            } else {
                return q;
            }
        }; j.getGroupImage = function (n) {
            return n ? '<img src="' + n + '"/>' : '<img src="' + MCK_BASE_URL + '/resources/sidebox/css/app/images/mck-icon-group.png"/>';
        }; j.getGroupDefaultIcon = function () {
            return '<div class="mck-group-icon-default"></div>';
        }; j.addMemberToGroup = function (o, n) {
            if (_typeof(o.members) === "object") {
                if (o.members.indexOf(n) === -1) {
                    o.members.push(n);
                } if (_typeof(o.removedMembersId) === "object" && o.removedMembersId.indexOf(n) !== -1) {
                    o.removedMembersId.splice(o.removedMembersId.indexOf(n), 1);
                } MCK_GROUP_MAP[o.contactId] = o;
            } return o;
        }; j.removeMemberFromGroup = function (o, n) {
            if (_typeof(o.removedMembersId) !== "object" || o.removedMembersId.length < 1) {
                o.removedMembersId = []; o.removedMembersId.push(n);
            } else {
                if (o.removedMembersId.indexOf(n) === -1) {
                    o.removedMembersId.push(n);
                }
            } MCK_GROUP_MAP[o.contactId] = o; return o;
        }; j.authenticateGroupUser = function (p) {
            var o = mckGroupService.isGroupLeft(p); var q = false; if (!o && p.members.length > 0) {
                for (var n = 0; n < p.members.length; n++) {
                    if (h === "" + p.members[n]) {
                        q = true; return true;
                    }
                }
            } return q;
        }; j.isAppendOpenGroupContextMenu = function (n) {
            if (m.deleteChatAccess === 0) {
                return false;
            } var o = mckGroupService.authenticateGroupUser(n); if (!o) {
                return false;
            } if (n.adminName === h) {
                return true;
            } if (m.deleteChatAccess === 2) {
                return true;
            } return false;
        }; j.isGroupLeft = function (o) {
            var n = false; if (o.removedMembersId && o.removedMembersId.length > 0) {
                $applozic.each(o.removedMembersId, function (p, q) {
                    if (q === h) {
                        n = true;
                    }
                });
            } return n;
        };
    } var alUserService = new AlUserService(); function AlUserService() {
        var f = this; f.MCK_USER_DETAIL_MAP = []; f.MCK_BLOCKED_TO_MAP = []; var c = new Array(); var b = "/rest/ws/user/block"; var e = "/rest/ws/user/v2/detail"; var a = "/rest/ws/user/chat/status"; f.updateUserStatus = function (i, j) {
            if (_typeof(alUserService.MCK_USER_DETAIL_MAP[i.userId]) === "object") {
                var h = alUserService.MCK_USER_DETAIL_MAP[i.userId]; if (i.status === 0) {
                    h.connected = false; h.lastSeenAtTime = i.lastSeenAtTime;
                } else {
                    if (i.status === 1) {
                        h.connected = true;
                    }
                }
            } else {
                var g = new Array(); g.push(i.userId); if (typeof j === "function") {
                    j(g);
                }
            }
        }; f.getUserDetail = function (g) {
            if (_typeof(alUserService.MCK_USER_DETAIL_MAP[g]) === "object") {
                return alUserService.MCK_USER_DETAIL_MAP[g];
            } else {
                return;
            }
        }; f.checkUserConnectedStatus = function (i) {
            var g = new Array(); var h = new Array(); $applozic(".mck-user-ol-status").each(function () {
                var k = $applozic(this).data("mck-id"); if (typeof k !== "undefined" && k !== "") {
                    g.push(k); var j = mckContactUtils.formatContactId("" + k); $applozic(this).addClass(j); $applozic(this).next().addClass(j);
                }
            }); if (g.length > 0) {
                $applozic.each(g, function (k, j) {
                    if (typeof alUserService.MCK_USER_DETAIL_MAP[j] === "undefined") {
                        h.push(j);
                    }
                }); if (typeof i === "function") {
                    i(h);
                }
            }
        }; f.loadUserProfile = function (i) {
            if (typeof i !== "undefined") {
                var h = []; var g = "" + i.split(",")[0]; h.push(g); f.loadUserProfiles(h);
            }
        }; f.loadUserProfiles = function (h, i) {
            var g = []; if (typeof i === "function") {
                i(h, g);
            }
        }; f.getUserStatus = function (h, i) {
            var g = new Object(); window.Applozic.ALApiService.getUserStatus({
                success: function success(j) {
                    if (j.users.length > 0) {
                        c = []; if (typeof i === "function") {
                            i(j);
                        }
                    } g.status = "success"; g.data = j; if (h.callback) {
                        h.callback(g);
                    } return;
                }, error: function error() {
                    g.status = "error"; if (h.callback) {
                        h.callback(g);
                    }
                }
            });
        }; f.blockUser = function (h, g, j) {
            if (!h || typeof g === "undefined") {
                return;
            } var i = "userId=" + h + "&block=" + g; mckUtils.ajax({
                url: MCK_BASE_URL + b, type: "get", data: i, success: function success(k) {
                    if ((typeof k === "undefined" ? "undefined" : _typeof(k)) === "object") {
                        if (k.status === "success") {
                            alUserService.MCK_BLOCKED_TO_MAP[h] = g; if (typeof j === "function") {
                                j(h);
                            }
                        }
                    }
                }, error: function error() { }
            });
        };
    } var alFileService = new AlFileService(); function AlFileService() {
        var q = this; var o = 1024; var h = 1048576; var l = ["CREATE", "UPDATE"]; var g = "/rest/ws/aws/file/"; var j = "/rest/ws/aws/file/url"; var m = "/rest/ws/upload/file"; var r = "/rest/ws/aws/file/delete"; var n = "www.googleapis.com"; var e; var s; var p; var i; var b; var t; var a; var c; var f; var k; q.init = function (u) {
            s = u.fileBaseUrl; t = btoa(u.userId + ":" + u.deviceKey); a = u.deviceKey;
        }; q.get = function (u) {
            b = u.appId; e = u.customUploadUrl; i = u.fileupload; p = u.mapStaticAPIkey; c = u.accessToken; f = u.appModuleName; k = u.genereateCloudFileUrl;
        }; q.deleteFileMeta = function (u) {
            window.Applozic.ALApiService.deleteFileMeta({
                data: { blobKey: u, url: s + r + "?key=" + u }, success: function success(v) {
                    console.log(v);
                }, error: function error() { }
            });
        }; q.getFilePreviewPath = function (u) {
            return (typeof u === "undefined" ? "undefined" : _typeof(u)) === "object" ? '<a href="' + s + g + u.blobKey + '" target="_blank">' + u.name + "</a>" : "";
        }; q.getFilePreviewSize = function (u) {
            if (u) {
                if (u > h) {
                    return "(" + parseInt(u / h) + " MB)";
                } else {
                    if (u > o) {
                        return "(" + parseInt(u / o) + " KB)";
                    } else {
                        return "(" + parseInt(u) + " B)";
                    }
                }
            } return "";
        }; q.getFileurl = function (v) {
            if (_typeof(v.fileMeta) === "object") {
                if (v.fileMeta.hasOwnProperty("url")) {
                    if (v.fileMeta.url.indexOf(n) !== -1) {
                        var u; q.generateCloudUrl(v.fileMeta.blobKey, function (x) {
                            u = x;
                        }); return u;
                    } else {
                        return "" + v.fileMeta.url;
                    }
                } else {
                    if (v.fileMeta.thumbnailUrl === "thumbnail_" + v.fileMeta.name) {
                        return e + "/files/" + v.fileMeta.name;
                    } else {
                        return s + g + v.fileMeta.blobKey;
                    }
                }
            } return "";
        }; q.generateCloudUrl = function (v, y) {
            var u = k.replace("{key}", v); var x = window.Applozic.ALApiService.getAttachmentHeaders(); mckUtils.ajax({
                type: "get", async: false, headers: x, url: u, success: function success(z) {
                    if (typeof y === "function") {
                        y(z);
                    }
                }, error: function error(z) {
                    console.log("error while getting token" + z);
                }
            });
        }; q.getFilePath = function (z) {
            if (z.contentType === 2) {
                try {
                    var u = $applozic.parseJSON(z.message); if (u.lat && u.lon) {
                        return '<a href="http://maps.google.com/maps?z=17&t=m&q=loc:' + u.lat + "," + u.lon + '" target="_blank"><img src="https://maps.googleapis.com/maps/api/staticmap?zoom=17&size=200x150&center=' + u.lat + "," + u.lon + "&maptype=roadmap&markers=color:red|" + u.lat + "," + u.lon + "&key=" + p + '"/></a>';
                    }
                } catch (x) {
                    if (z.message.indexOf(",") !== -1) {
                        return '<a href="http://maps.google.com/maps?z=17&t=m&q=loc:' + z.message + '" target="_blank"><img src="https://maps.googleapis.com/maps/api/staticmap?zoom=17&size=200x150&center=' + z.message + "&maptype=roadmap&markers=color:red|" + z.message + "&key=" + p + '" /></a>';
                    }
                }
            } if (_typeof(z.fileMeta) === "object") {
                if (z.fileMeta.contentType.indexOf("image") !== -1) {
                    if (z.fileMeta.contentType.indexOf("svg") !== -1) {
                        return '<a href="#" role="link" target="_self" class="file-preview-link fancybox-media imageview" data-type="' + z.fileMeta.contentType + '" data-url="' + q.getFileurl(z) + '" data-name="' + z.fileMeta.name + '"><img src="' + q.getFileurl(z) + '" area-hidden="true"></img></a>';
                    } else {
                        if (z.contentType === 5) {
                            return '<a href="#" role="link" target="_self" class="file-preview-link fancybox-media imageview" data-type="' + z.fileMeta.contentType + '" data-url="' + z.fileMeta.blobKey + '" data-name="' + z.fileMeta.name + '"><img src="' + z.fileMeta.blobKey + '" area-hidden="true"></img></a>';
                        } else {
                            if (z.fileMeta.hasOwnProperty("url")) {
                                if (z.fileMeta.url.indexOf(n) !== -1) {
                                    var y; q.generateCloudUrl(z.fileMeta.thumbnailBlobKey, function (A) {
                                        y = A;
                                    }); return '<a href="#" role="link" target="_self" class="file-preview-link fancybox-media imageview" data-type="' + z.fileMeta.contentType + '" data-url="" data-blobKey="' + z.fileMeta.blobKey + '" data-name="' + z.fileMeta.name + '"><img src="' + y + '" area-hidden="true"></img></a>';
                                } else {
                                    return '<a href="#" role="link" target="_self" class="file-preview-link fancybox-media imageview" data-type="' + z.fileMeta.contentType + '" data-url="' + q.getFileurl(z) + '" data-name="' + z.fileMeta.name + '"><img src="' + z.fileMeta.thumbnailUrl + '" area-hidden="true"></img></a>';
                                }
                            } else {
                                if (z.fileMeta.thumbnailUrl === "thumbnail_" + z.fileMeta.name) {
                                    return '<a href="#" role="link" target="_self" class="file-preview-link fancybox-media imageview" data-type="' + z.fileMeta.contentType + '" data-url="' + q.getFileurl(z) + '" data-name="' + z.fileMeta.name + '"><img src="' + e + "/files/thumbnail_" + z.fileMeta.name + '" area-hidden="true"></img></a>';
                                } else {
                                    return '<a href="#" role="link" target="_self" class="file-preview-link fancybox-media imageview" data-type="' + z.fileMeta.contentType + '" data-url="' + q.getFileurl(z) + '" data-name="' + z.fileMeta.name + '"><img src="' + z.fileMeta.thumbnailUrl + '" area-hidden="true"></img></a>';
                                }
                            }
                        }
                    }
                } else {
                    if (z.fileMeta.contentType.indexOf("video") !== -1) {
                        if (z.fileMeta.hasOwnProperty("url") && z.fileMeta.url.indexOf(n) !== -1) {
                            var v; q.generateCloudUrl(z.fileMeta.blobKey, function (A) {
                                v = A;
                            }); return '<a href="#" target="_self"><video controls class="mck-video-player" onplay="alFileService.updateAudVidUrl(this);" data-cloud-service="google_cloud" data-blobKey="' + z.fileMeta.blobKey + '"><source src="' + v + '" type="video/mp4"><source src="' + v + '" type="video/ogg"></video>';
                        } else {
                            return '<a href= "#" target="_self"><video controls class="mck-video-player"><source src="' + q.getFileurl(z) + '" type="video/mp4"><source src="' + q.getFileurl(z) + '" type="video/ogg"></video></a>';
                        }
                    } else {
                        if (z.fileMeta.contentType.indexOf("audio") !== -1) {
                            if (z.fileMeta.hasOwnProperty("url") && z.fileMeta.url.indexOf(n) !== -1) {
                                var v; q.generateCloudUrl(z.fileMeta.blobKey, function (A) {
                                    v = A;
                                }); return '<a href="#" target="_self"><audio controls class="mck-audio-player" onplay="alFileService.updateAudVidUrl(this);" data-cloud-service="google_cloud" data-blobKey="' + z.fileMeta.blobKey + '"><source src="' + v + '" type="audio/ogg"><source src="' + v + '" type="audio/mpeg"></audio><p class="mck-file-tag"></p></a>';
                            } else {
                                return '<a href="#" target="_self"><audio controls class="mck-audio-player"><source src="' + q.getFileurl(z) + '" type="audio/ogg"><source src="' + q.getFileurl(z) + '" type="audio/mpeg"></audio><p class="mck-file-tag"></p></a>';
                            }
                        } else {
                            return '<a href="#" role="link" class="file-preview-link" target="_self"></a>';
                        }
                    }
                }
            } return "";
        }; q.updateAudVidUrl = function (y) {
            var x = y.dataset.blobkey; var z = new Date().getTime(); var u = y.currentSrc; var v = q.fetchQueryString("Expires", u); if (z >= v * 1000) {
                q.generateCloudUrl(x, function (A) {
                    getUrl = A;
                }); y.src = getUrl;
            }
        }; this.fetchQueryString = function (y, x) {
            y = y.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]"); var v = new RegExp("[\\?&]" + y + "=([^&#]*)"); var u = v.exec(x); if (u == null) {
                console.log("The parameter is null for the searchedquery");
            } return u[1];
        }; q.getFileAttachment = function (u) {
            if (_typeof(u.fileMeta) === "object") {
                if (u.fileMeta.contentType.indexOf("image") !== -1 || u.fileMeta.contentType.indexOf("audio") !== -1 || u.fileMeta.contentType.indexOf("video") !== -1) {
                    if (u.fileMeta.hasOwnProperty("url") && u.fileMeta.url.indexOf(n) !== -1) {
                        return '<a href="javascript:void(0);" role="link" target="_self"  class="file-preview-link" data-blobKey="' + u.fileMeta.blobKey + '" data-cloud-service="google_cloud"><span class="file-detail mck-image-download"><span class="mck-file-name"><span class="mck-icon-attachment"></span>&nbsp;' + u.fileMeta.name + '</span>&nbsp;<span class="file-size">' + alFileService.getFilePreviewSize(u.fileMeta.size) + "</span></span></a>";
                    } else {
                        return '<a href="' + q.getFileurl(u) + '" role="link" target="_self"  class="file-preview-link"><span class="file-detail mck-image-download"><span class="mck-file-name"><span class="mck-icon-attachment"></span>&nbsp;' + u.fileMeta.name + '</span>&nbsp;<span class="file-size">' + alFileService.getFilePreviewSize(u.fileMeta.size) + "</span></span></a>";
                    }
                } else {
                    return '<a href="' + q.getFileurl(u) + '" role="link" target="_self"  class="file-preview-link"><span class="file-detail mck-image-download"><span class="mck-file-name"><span class="mck-icon-attachment"></span>&nbsp;' + u.fileMeta.name + '</span>&nbsp;<span class="file-size">' + alFileService.getFilePreviewSize(u.fileMeta.size) + "</span></span></a>";
                } return "";
            }
        }; q.getFileIcon = function (u) {
            if (u.fileMetaKey && _typeof(u.fileMeta) === "object") {
                if (u.fileMeta.contentType.indexOf("image") !== -1) {
                    return '<span class="mck-icon-camera"></span>&nbsp;<span>Image</span>';
                } else {
                    if (u.fileMeta.contentType.indexOf("audio") !== -1) {
                        return '<span class="mck-icon-attachment"></span>&nbsp;<span>Audio</span>';
                    } else {
                        if (u.fileMeta.contentType.indexOf("video") !== -1) {
                            return '<span class="mck-icon-attachment"></span>&nbsp;<span>Video</span>';
                        } else {
                            return '<span class="mck-icon-attachment"></span>&nbsp;<span>File</span>';
                        }
                    }
                }
            } else {
                return "";
            }
        }; q.downloadfile = function () {
            var u = q.getFileurl(msg); var x = document.createElement("a"); x.download = thefilename; x.setAttribute("href", u); var v = "data:text/csv;charset=utf-8;base64," + someb64data; x.href = v; document.body.appendChild(x); x.click(); document.body.removeChild(x);
        };
    } (function (b) {
        function a() {
            var c = {}; c.logout = function () {
                if (typeof b.Applozic.ALSocket !== "undefined") {
                    b.Applozic.ALApiService.setAjaxHeaders("", "", "", "", ""); b.Applozic.ALSocket.disconnect(); ALStorage.clearMckMessageArray(); ALStorage.clearAppHeaders(); ALStorage.clearMckContactNameArray(); ALStorage.removeEncryptionKey(); w.sessionStorage.clear();
                } IS_LOGGED_IN = false;
            }; return c;
        } if (typeof AlCustomService === "undefined") {
            b.Applozic.AlCustomService = a();
        } else {
            console.log("ALCustomService already defined.");
        }
    })(window); var alMessageService = new AlMessageService(); function AlMessageService() {
        var i = this; var g; var c; var k; var n = "/rest/ws/conversation/topicId"; var a = "/rest/ws/conversation/id"; var l = "/rest/ws/conversation/get"; var h = "/rest/ws/message/add/inbox"; var b = "/rest/ws/conversation/close"; var m = "/rest/ws/message/delete/conversation"; var j = "/rest/ws/message/read/conversation"; var f = '<div id="mck-ofl-blk" class="mck-m-b"><div class="mck-clear"><div class="blk-lg-12 mck-text-light mck-text-muted mck-test-center">${userIdExpr} is offline now</div></div></div>'; var e; i.init = function (o) {
            k = o.fileBaseUrl; g = o.visitor; c = g ? "guest" : $applozic.trim(o.userId);
        }; i.addMessageToTab = function (p, o, r) {
            var q = { to: p.to, groupId: p.groupId, deviceKey: p.deviceKey, contentType: p.contentType, message: p.message, conversationId: p.conversationId, topicId: p.topicId, sendToDevice: true, createdAtTime: new Date().getTime(), key: p.key, storeOnDevice: true, sent: false, read: true, metadata: p.metadata ? p.metadata : "" }; q.type = p.type ? p.type : 5; if (p.fileMeta) {
                q.fileMeta = p.fileMeta;
            } if (typeof r === "function") {
                r(q, o);
            }
        }; i.getMessages = function (p) {
            var o = {}; if (p.startTime) {
                o.endTime = p.startTime;
            } if (typeof p.userId !== "undefined" && p.userId !== "") {
                if (p.isGroup) {
                    o.groupId = p.userId;
                } else {
                    o.userId = p.userId;
                } o.pageSize = 30; if ((IS_MCK_TOPIC_HEADER || IS_MCK_TOPIC_BOX) && p.conversationId) {
                    o.conversationId = p.conversationId; if (typeof MCK_TAB_CONVERSATION_MAP[p.userId] === "undefined") {
                        o.conversationReq = true;
                    }
                }
            } else {
                o.mainPageSize = 100;
            } window.Applozic.ALApiService.getMessages({ data: o, success: p.callback, error: p.callback });
        }; i.getMessageList = function (r, s) {
            var o = r.id; var p = {}; var q = {}; if (r.startTime) {
                p.endTime = r.startTime;
            } if (typeof r.clientGroupId !== "undefined" && r.clientGroupId !== "") {
                if (r.pageSize) {
                    p.pageSize = r.pageSize;
                } else {
                    p.pageSize = 50;
                } p.clientGroupId = r.clientGroupId; q = { clientGroupId: r.clientGroupId };
            } else {
                if (typeof o !== "undefined" && o !== "") {
                    if (r.pageSize) {
                        p.pageSize = r.pageSize;
                    } else {
                        p.pageSize = 50;
                    } if ("" + r.isGroup === "true") {
                        p.groupId = o;
                    } else {
                        p.userId = o;
                    } q = { id: o };
                } else {
                    if (r.mainPageSize) {
                        p.mainPageSize = r.pageSize;
                    } else {
                        p.mainPageSize = 50;
                    } q = { id: "" };
                }
            } if (r.topicId && (o || r.clientGroupId)) {
                if (r.conversationId) {
                    p.conversationId = r.conversationId;
                } if (r.topicId) {
                    q.topicId = r.topicId;
                }
            } window.Applozic.ALApiService.getMessages({
                data: p, success: function success(t) {
                    var x = t.data; q.status = "success"; if (typeof x.message === "undefined" || x.message.length === 0) {
                        q.messages = [];
                    } else {
                        var v = x.message; var u = new Array(); $applozic.each(v, function (y, z) {
                            if (typeof s === "function") {
                                s(z);
                            }
                        }); q.messages = u;
                    } if (x.groupFeeds.length > 0) {
                        q.id = x.groupFeeds[0].id;
                    } r.callback(x);
                }, error: function error(t) {
                    q.status = "error"; r.callback(q);
                }
            });
        }; i.getReplyMessageByKey = function (o) {
            var p = ALStorage.getMessageByKey(o); if (typeof p === "undefined") {
                window.Applozic.ALApiService.updateReplyMessage({
                    data: { key: o }, async: false, success: function success(q) {
                        ALStorage.updateMckMessageArray(q);
                    }
                });
            } return ALStorage.getMessageByKey(o);
        }; i.sendDeliveryUpdate = function (o) {
            window.Applozic.ALApiService.sendDeliveryUpdate({ data: { key: o.pairedMessageKey }, success: function success() { }, error: function error() { } });
        }; i.sendReadUpdate = function (o) {
            if (typeof o !== "undefined" && o !== "") {
                window.Applozic.ALApiService.sendReadUpdate({ data: { key: o }, success: function success() { }, error: function error() { } });
            }
        }; i.fetchConversationByTopicId = function (o, p) {
            window.Applozic.ALApiService.fetchConversationByTopicId({
                data: o, success: function success(r) {
                    if ((typeof r === "undefined" ? "undefined" : _typeof(r)) === "object" && r.status === "success") {
                        var q = r.response; if (q.length > 0) {
                            $applozic.each(q, function (v, x) {
                                MCK_CONVERSATION_MAP[x.id] = x; MCK_TOPIC_CONVERSATION_MAP[x.topicId] = [x.id]; if (x.topicDetail) {
                                    try {
                                        MCK_TOPIC_DETAIL_MAP[x.topicId] = $applozic.parseJSON(x.topicDetail);
                                    } catch (u) {
                                        w.console.log("Incorect Topic Detail!");
                                    }
                                } if (params.tabId && typeof MCK_TAB_CONVERSATION_MAP[params.tabId] !== "undefined") {
                                    var t = MCK_TAB_CONVERSATION_MAP[params.tabId]; t.push(x); MCK_TAB_CONVERSATION_MAP[params.tabId] = t;
                                }
                            });
                        } if (params.isExtMessageList) {
                            if (q.length > 0) {
                                params.conversationId = q[0].id; params.pageSize = 50; if (typeof p === "function") {
                                    p(params);
                                }
                            } else {
                                if (typeof params.callback === "function") {
                                    var s = {}; if (params.tabId) {
                                        s.id = params.tabId; s.isGroup = params.isGroup;
                                    } else {
                                        if (params.clientGroupId) {
                                            s.clientGroupId = params.clientGroupId;
                                        }
                                    } s.topicId = params.topicId; s.status = "success"; s.messages = []; params.callback(s);
                                }
                            }
                        }
                    } else {
                        if (params.isExtMessageList && typeof params.callback === "function") {
                            var s = {}; if (params.tabId) {
                                s.id = params.tabId;
                            } else {
                                if (params.clientGroupId) {
                                    s.clientGroupId = params.clientGroupId;
                                }
                            } s.topicId = params.topicId; s.status = "error"; s.errorMessage = "Unable to process request. Please try again."; params.callback(s);
                        }
                    }
                }, error: function error() {
                    if (typeof params.callback === "function") {
                        var q = {}; if (params.tabId) {
                            q.id = params.tabId;
                        } else {
                            if (params.clientGroupId) {
                                q.clientGroupId = params.clientGroupId;
                            }
                        } q.topicId = params.topicId; q.status = "error"; q.errorMessage = "Unable to process request. Please try again."; params.callback(q);
                    }
                }
            });
        }; i.getTopicId = function (p, q) {
            if (p.conversationId) {
                var o = "id=" + p.conversationId; window.Applozic.ALApiService.getTopicId({
                    data: { conversationId: p.conversationId }, success: function success(r) {
                        if ((typeof o === "undefined" ? "undefined" : _typeof(o)) === "object" && o.status === "success") {
                            var v = o.response; if ((typeof v === "undefined" ? "undefined" : _typeof(v)) === "object") {
                                MCK_TOPIC_CONVERSATION_MAP[v.topicId] = [p.conversationId]; MCK_CONVERSATION_MAP[p.conversationId] = v; if (v.topicDetail) {
                                    try {
                                        MCK_TOPIC_DETAIL_MAP[v.topicId] = $applozic.parseJSON(v.topicDetail);
                                    } catch (u) {
                                        w.console.log("Incorect Topic Detail!");
                                    }
                                } if (typeof MCK_PRICE_DETAIL === "function" && p.priceText) {
                                    MCK_PRICE_DETAIL({ custId: c, suppId: p.suppId, productId: v.topicId, price: p.priceText }); i.sendConversationCloseUpdate(p.conversationId);
                                } if (p.messageType && _typeof(p.message) === "object") {
                                    var t = p.message.groupId ? p.message.groupId : p.message.to; if (typeof MCK_TAB_CONVERSATION_MAP[t] !== "undefined") {
                                        var s = MCK_TAB_CONVERSATION_MAP[t]; s.push(v); MCK_TAB_CONVERSATION_MAP[t] = s;
                                    } if (typeof p.populate !== "undefined" ? p.populate : true) {
                                        if (typeof q === "function") {
                                            q(p);
                                        }
                                    }
                                } if (typeof p.callback === "function") {
                                    p.callback(v);
                                }
                            }
                        }
                    }, error: function error() { }
                });
            }
        }; i.sendConversationCloseUpdate = function (p) {
            if (p) {
                var o = "id=" + p; window.Applozic.ALApiService.sendConversationCloseUpdate({ conversationId: p, success: function success(q) { }, error: function error() { } });
            }
        }; i.dispatchMessage = function (s) {
            if (s.messagePxy === "object") {
                var o = s.messagePxy; if (s.topicId) {
                    var r = MCK_TOPIC_DETAIL_MAP[s.topicId]; if ((typeof r === "undefined" ? "undefined" : _typeof(r)) === "object" && r.title !== "undefined") {
                        if (!o.message) {
                            o.message = $applozic.trim(r.title);
                        } if (s.conversationId) {
                            o.conversationId = s.conversationId;
                        } else {
                            if (s.topicId) {
                                var q = { topicId: s.topicId }; if ((typeof r === "undefined" ? "undefined" : _typeof(r)) === "object") {
                                    q.topicDetail = w.JSON.stringify(r);
                                } o.conversationPxy = q;
                            }
                        }
                    } if (!o.message && r.link) {
                        var p = { blobKey: $applozic.trim(r.link), contentType: "image/png" }; o.fileMeta = p; o.contentType = 5; FILE_META = []; FILE_META.push(p);
                    }
                } if (s.isGroup) {
                    o.groupId = s.tabId;
                } else {
                    o.to = s.tabId;
                } mckMessageService.sendMessage(o);
            }
        }; i.sendVideoCallMessage = function (r, q, u, p, s, v) {
            var x = q == "CALL_MISSED" ? "Missed Call" : q == "CALL_REJECTED" ? "Call Rejected" : ""; if (x == "" || x == undefined) {
                x = "video message";
            } var t = { MSG_TYPE: q, CALL_ID: r, CALL_AUDIO_ONLY: p }; var o = { to: s, type: 5, contentType: u, message: x, metadata: t, senderName: c }; v(o); return o;
        }; i.sendVideoCallEndMessage = function (t, s, x, q, r, u, y) {
            var p = ""; if (r) {
                p = mckDateUtils.convertMilisIntoTime(r);
            } var z = s == "CALL_MISSED" ? "Missed Call" : s == "CALL_REJECTED" ? "Call Rejected" : s == "CALL_END" ? "Call End \n Duration: " + p : "video message"; if (z == "" || z == undefined) {
                z = "video message";
            } var v = { MSG_TYPE: s, CALL_ID: t, CALL_AUDIO_ONLY: q, CALL_DURATION: r }; var o = { to: u, type: 5, contentType: x, message: z, metadata: v }; y(o); return o;
        }; i.getMessageFeed = function (q) {
            var p = {}; k = window.Applozic.ALApiService.getFileUrl(); p.key = q.key; p.contentType = q.contentType; p.timeStamp = q.createdAtTime; p.message = q.message; p.from = q.type === 4 ? q.to : c; if (q.groupId) {
                p.to = q.groupId;
            } else {
                p.to = q.type === 5 ? q.to : c;
            } p.status = "read"; p.type = q.type === 4 ? "inbox" : "outbox"; if (q.type === 5) {
                if (q.status === 3) {
                    p.status = "sent";
                } else {
                    if (q.status === 4) {
                        p.status = "delivered";
                    }
                }
            } if (_typeof(q.fileMeta) === "object") {
                var o = Object.assign({}, q.fileMeta); if (typeof o.url === "undefined" || o.url === "") {
                    o.url = k + "/rest/ws/aws/file/" + q.fileMeta.blobKey;
                } delete o.blobKey; p.file = o;
            } p.source = q.source; p.metadata = q.metadata; return p;
        };
    };