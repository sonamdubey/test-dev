/*
* @name BeautyTips
* @desc a tooltips/baloon-help plugin for jQuery
*
* @author Jeff Robbins - Lullabot - http://www.lullabot.com
* @version 0.9.5-rc1  (5/20/2009)
*/
jQuery.bt = { version: "0.9.5-rc1" }; (function ($) { jQuery.fn.bt = function (content, options) { if (typeof content != "string") { var contentSelect = true; options = content; content = false; } else { var contentSelect = false; } if (jQuery.fn.hoverIntent && jQuery.bt.defaults.trigger == "hover") { jQuery.bt.defaults.trigger = "hoverIntent"; } return this.each(function (index) { var opts = jQuery.extend(false, jQuery.bt.defaults, jQuery.bt.options, options); opts.spikeLength = numb(opts.spikeLength); opts.spikeGirth = numb(opts.spikeGirth); opts.overlap = numb(opts.overlap); var ajaxTimeout = false; if (opts.killTitle) { $(this).find("[title]").andSelf().each(function () { if (!$(this).attr("bt-xTitle")) { $(this).attr("bt-xTitle", $(this).attr("title")).attr("title", ""); } }); } if (typeof opts.trigger == "string") { opts.trigger = [opts.trigger]; } if (opts.trigger[0] == "hoverIntent") { var hoverOpts = jQuery.extend(opts.hoverIntentOpts, { over: function () { this.btOn(); }, out: function () { this.btOff(); } }); $(this).hoverIntent(hoverOpts); } else { if (opts.trigger[0] == "hover") { $(this).hover(function () { this.btOn(); }, function () { this.btOff(); }); } else { if (opts.trigger[0] == "now") { if ($(this).hasClass("bt-active")) { this.btOff(); } else { this.btOn(); } } else { if (opts.trigger[0] == "none") { } else { if (opts.trigger.length > 1 && opts.trigger[0] != opts.trigger[1]) { $(this).bind(opts.trigger[0], function () { this.btOn(); }).bind(opts.trigger[1], function () { this.btOff(); }); } else { $(this).bind(opts.trigger[0], function () { if ($(this).hasClass("bt-active")) { this.btOff(); } else { this.btOn(); } }); } } } } } this.btOn = function () { if (typeof $(this).data("bt-box") == "object") { this.btOff(); } opts.preBuild.apply(this); $(jQuery.bt.vars.closeWhenOpenStack).btOff(); $(this).addClass("bt-active " + opts.activeClass); if (contentSelect && opts.ajaxPath == null) { if (opts.killTitle) { $(this).attr("title", $(this).attr("bt-xTitle")); } content = $.isFunction(opts.contentSelector) ? opts.contentSelector.apply(this) : eval(opts.contentSelector); if (opts.killTitle) { $(this).attr("title", ""); } } if (opts.ajaxPath != null && content == false) { if (typeof opts.ajaxPath == "object") { var url = eval(opts.ajaxPath[0]); url += opts.ajaxPath[1] ? " " + opts.ajaxPath[1] : ""; } else { var url = opts.ajaxPath; } var off = url.indexOf(" "); if (off >= 0) { var selector = url.slice(off, url.length); url = url.slice(0, off); } var cacheData = opts.ajaxCache ? $(document.body).data("btCache-" + url.replace(/\./g, "")) : null; if (typeof cacheData == "string") { content = selector ? $("<div/>").append(cacheData.replace(/<script(.|\s)*?\/script>/g, "")).find(selector) : cacheData; } else { var target = this; var ajaxOpts = jQuery.extend(false, { type: opts.ajaxType, data: opts.ajaxData, cache: opts.ajaxCache, url: url, complete: function (XMLHttpRequest, textStatus) { if (textStatus == "success" || textStatus == "notmodified") { if (opts.ajaxCache) { $(document.body).data("btCache-" + url.replace(/\./g, ""), XMLHttpRequest.responseText); } ajaxTimeout = false; content = selector ? $("<div/>").append(XMLHttpRequest.responseText.replace(/<script(.|\s)*?\/script>/g, "")).find(selector) : XMLHttpRequest.responseText; } else { if (textStatus == "timeout") { ajaxTimeout = true; } content = opts.ajaxError.replace(/%error/g, XMLHttpRequest.statusText); } if ($(target).hasClass("bt-active")) { target.btOn(); } } }, opts.ajaxOpts); jQuery.ajax(ajaxOpts); content = opts.ajaxLoading; } } var shadowMarginX = 0; var shadowMarginY = 0; var shadowShiftX = 0; var shadowShiftY = 0; if (opts.shadow && !shadowSupport()) { opts.shadow = false; jQuery.extend(opts, opts.noShadowOpts); } if (opts.shadow) { if (opts.shadowBlur > Math.abs(opts.shadowOffsetX)) { shadowMarginX = opts.shadowBlur * 2; } else { shadowMarginX = opts.shadowBlur + Math.abs(opts.shadowOffsetX); } shadowShiftX = (opts.shadowBlur - opts.shadowOffsetX) > 0 ? opts.shadowBlur - opts.shadowOffsetX : 0; if (opts.shadowBlur > Math.abs(opts.shadowOffsetY)) { shadowMarginY = opts.shadowBlur * 2; } else { shadowMarginY = opts.shadowBlur + Math.abs(opts.shadowOffsetY); } shadowShiftY = (opts.shadowBlur - opts.shadowOffsetY) > 0 ? opts.shadowBlur - opts.shadowOffsetY : 0; } if (opts.offsetParent) { var offsetParent = $(opts.offsetParent); var offsetParentPos = offsetParent.offset(); var pos = $(this).offset(); var top = numb(pos.top) - numb(offsetParentPos.top) + numb($(this).css("margin-top")) - shadowShiftY; var left = numb(pos.left) - numb(offsetParentPos.left) + numb($(this).css("margin-left")) - shadowShiftX; } else { var offsetParent = ($(this).css("position") == "absolute") ? $(this).parents().eq(0).offsetParent() : $(this).offsetParent(); var pos = $(this).btPosition(); var top = numb(pos.top) + numb($(this).css("margin-top")) - shadowShiftY; var left = numb(pos.left) + numb($(this).css("margin-left")) - shadowShiftX; } var width = $(this).btOuterWidth(); var height = $(this).outerHeight(); if (typeof content == "object") { var original = content; var clone = $(original).clone(true).show(); var origClones = $(original).data("bt-clones") || []; origClones.push(clone); $(original).data("bt-clones", origClones); $(clone).data("bt-orig", original); $(this).data("bt-content-orig", { original: original, clone: clone }); content = clone; } if (typeof content == "null" || content == "") { return; } var $text = $('<div class="bt-content"></div>').append(content).css({ padding: opts.padding, position: "absolute", width: (opts.shrinkToFit ? "auto" : opts.width), zIndex: opts.textzIndex, left: shadowShiftX, top: shadowShiftY }).css(opts.cssStyles); var $box = $('<div class="bt-wrapper"></div>').append($text).addClass(opts.cssClass).css({ position: "absolute", width: opts.width, zIndex: opts.wrapperzIndex, visibility: "hidden" }).appendTo(offsetParent); if (jQuery.fn.bgiframe) { $text.bgiframe(); $box.bgiframe(); } $(this).data("bt-box", $box); var scrollTop = numb($(document).scrollTop()); var scrollLeft = numb($(document).scrollLeft()); var docWidth = numb($(window).width()); var docHeight = numb($(window).height()); var winRight = scrollLeft + docWidth; var winBottom = scrollTop + docHeight; var space = new Object(); var thisOffset = $(this).offset(); space.top = thisOffset.top - scrollTop; space.bottom = docHeight - ((thisOffset + height) - scrollTop); space.left = thisOffset.left - scrollLeft; space.right = docWidth - ((thisOffset.left + width) - scrollLeft); var textOutHeight = numb($text.outerHeight()); var textOutWidth = numb($text.btOuterWidth()); if (opts.positions.constructor == String) { opts.positions = opts.positions.replace(/ /, "").split(","); } if (opts.positions[0] == "most") { var position = "top"; for (var pig in space) { position = space[pig] > space[position] ? pig : position; } } else { for (var x in opts.positions) { var position = opts.positions[x]; if ((position == "left" || position == "right") && space[position] > textOutWidth + opts.spikeLength) { break; } else { if ((position == "top" || position == "bottom") && space[position] > textOutHeight + opts.spikeLength) { break; } } } } var horiz = left + ((width - textOutWidth) * 0.5); var vert = top + ((height - textOutHeight) * 0.5); var points = new Array(); var textTop, textLeft, textRight, textBottom, textTopSpace, textBottomSpace, textLeftSpace, textRightSpace, crossPoint, textCenter, spikePoint; switch (position) { case "top": $text.css("margin-bottom", opts.spikeLength + "px"); $box.css({ top: (top - $text.outerHeight(true)) + opts.overlap, left: horiz }); textRightSpace = (winRight - opts.windowMargin) - ($text.offset().left + $text.btOuterWidth(true)); var xShift = shadowShiftX; if (textRightSpace < 0) { $box.css("left", (numb($box.css("left")) + textRightSpace) + "px"); xShift -= textRightSpace; } textLeftSpace = ($text.offset().left + numb($text.css("margin-left"))) - (scrollLeft + opts.windowMargin); if (textLeftSpace < 0) { $box.css("left", (numb($box.css("left")) - textLeftSpace) + "px"); xShift += textLeftSpace; } textTop = $text.btPosition().top + numb($text.css("margin-top")); textLeft = $text.btPosition().left + numb($text.css("margin-left")); textRight = textLeft + $text.btOuterWidth(); textBottom = textTop + $text.outerHeight(); textCenter = { x: textLeft + ($text.btOuterWidth() * opts.centerPointX), y: textTop + ($text.outerHeight() * opts.centerPointY) }; points[points.length] = spikePoint = { y: textBottom + opts.spikeLength, x: ((textRight - textLeft) * 0.5) + xShift, type: "spike" }; crossPoint = findIntersectX(spikePoint.x, spikePoint.y, textCenter.x, textCenter.y, textBottom); crossPoint.x = crossPoint.x < textLeft + opts.spikeGirth / 2 + opts.cornerRadius ? textLeft + opts.spikeGirth / 2 + opts.cornerRadius : crossPoint.x; crossPoint.x = crossPoint.x > (textRight - opts.spikeGirth / 2) - opts.cornerRadius ? (textRight - opts.spikeGirth / 2) - opts.CornerRadius : crossPoint.x; points[points.length] = { x: crossPoint.x - (opts.spikeGirth / 2), y: textBottom, type: "join" }; points[points.length] = { x: textLeft, y: textBottom, type: "corner" }; points[points.length] = { x: textLeft, y: textTop, type: "corner" }; points[points.length] = { x: textRight, y: textTop, type: "corner" }; points[points.length] = { x: textRight, y: textBottom, type: "corner" }; points[points.length] = { x: crossPoint.x + (opts.spikeGirth / 2), y: textBottom, type: "join" }; points[points.length] = spikePoint; break; case "left": $text.css("margin-right", opts.spikeLength + "px"); $box.css({ top: vert + "px", left: ((left - $text.btOuterWidth(true)) + opts.overlap) + "px" }); textBottomSpace = (winBottom - opts.windowMargin) - ($text.offset().top + $text.outerHeight(true)); var yShift = shadowShiftY; if (textBottomSpace < 0) { $box.css("top", (numb($box.css("top")) + textBottomSpace) + "px"); yShift -= textBottomSpace; } textTopSpace = ($text.offset().top + numb($text.css("margin-top"))) - (scrollTop + opts.windowMargin); if (textTopSpace < 0) { $box.css("top", (numb($box.css("top")) - textTopSpace) + "px"); yShift += textTopSpace; } textTop = $text.btPosition().top + numb($text.css("margin-top")); textLeft = $text.btPosition().left + numb($text.css("margin-left")); textRight = textLeft + $text.btOuterWidth(); textBottom = textTop + $text.outerHeight(); textCenter = { x: textLeft + ($text.btOuterWidth() * opts.centerPointX), y: textTop + ($text.outerHeight() * opts.centerPointY) }; points[points.length] = spikePoint = { x: textRight + opts.spikeLength, y: ((textBottom - textTop) * 0.5) + yShift, type: "spike" }; crossPoint = findIntersectY(spikePoint.x, spikePoint.y, textCenter.x, textCenter.y, textRight); crossPoint.y = crossPoint.y < textTop + opts.spikeGirth / 2 + opts.cornerRadius ? textTop + opts.spikeGirth / 2 + opts.cornerRadius : crossPoint.y; crossPoint.y = crossPoint.y > (textBottom - opts.spikeGirth / 2) - opts.cornerRadius ? (textBottom - opts.spikeGirth / 2) - opts.cornerRadius : crossPoint.y; points[points.length] = { x: textRight, y: crossPoint.y + opts.spikeGirth / 2, type: "join" }; points[points.length] = { x: textRight, y: textBottom, type: "corner" }; points[points.length] = { x: textLeft, y: textBottom, type: "corner" }; points[points.length] = { x: textLeft, y: textTop, type: "corner" }; points[points.length] = { x: textRight, y: textTop, type: "corner" }; points[points.length] = { x: textRight, y: crossPoint.y - opts.spikeGirth / 2, type: "join" }; points[points.length] = spikePoint; break; case "bottom": $text.css("margin-top", opts.spikeLength + "px"); $box.css({ top: (top + height) - opts.overlap, left: horiz }); textRightSpace = (winRight - opts.windowMargin) - ($text.offset().left + $text.btOuterWidth(true)); var xShift = shadowShiftX; if (textRightSpace < 0) { $box.css("left", (numb($box.css("left")) + textRightSpace) + "px"); xShift -= textRightSpace; } textLeftSpace = ($text.offset().left + numb($text.css("margin-left"))) - (scrollLeft + opts.windowMargin); if (textLeftSpace < 0) { $box.css("left", (numb($box.css("left")) - textLeftSpace) + "px"); xShift += textLeftSpace; } textTop = $text.btPosition().top + numb($text.css("margin-top")); textLeft = $text.btPosition().left + numb($text.css("margin-left")); textRight = textLeft + $text.btOuterWidth(); textBottom = textTop + $text.outerHeight(); textCenter = { x: textLeft + ($text.btOuterWidth() * opts.centerPointX), y: textTop + ($text.outerHeight() * opts.centerPointY) }; points[points.length] = spikePoint = { x: ((textRight - textLeft) * 0.5) + xShift, y: shadowShiftY, type: "spike" }; crossPoint = findIntersectX(spikePoint.x, spikePoint.y, textCenter.x, textCenter.y, textTop); crossPoint.x = crossPoint.x < textLeft + opts.spikeGirth / 2 + opts.cornerRadius ? textLeft + opts.spikeGirth / 2 + opts.cornerRadius : crossPoint.x; crossPoint.x = crossPoint.x > (textRight - opts.spikeGirth / 2) - opts.cornerRadius ? (textRight - opts.spikeGirth / 2) - opts.cornerRadius : crossPoint.x; points[points.length] = { x: crossPoint.x + opts.spikeGirth / 2, y: textTop, type: "join" }; points[points.length] = { x: textRight, y: textTop, type: "corner" }; points[points.length] = { x: textRight, y: textBottom, type: "corner" }; points[points.length] = { x: textLeft, y: textBottom, type: "corner" }; points[points.length] = { x: textLeft, y: textTop, type: "corner" }; points[points.length] = { x: crossPoint.x - (opts.spikeGirth / 2), y: textTop, type: "join" }; points[points.length] = spikePoint; break; case "right": $text.css("margin-left", (opts.spikeLength + "px")); $box.css({ top: vert + "px", left: ((left + width) - opts.overlap) + "px" }); textBottomSpace = (winBottom - opts.windowMargin) - ($text.offset().top + $text.outerHeight(true)); var yShift = shadowShiftY; if (textBottomSpace < 0) { $box.css("top", (numb($box.css("top")) + textBottomSpace) + "px"); yShift -= textBottomSpace; } textTopSpace = ($text.offset().top + numb($text.css("margin-top"))) - (scrollTop + opts.windowMargin); if (textTopSpace < 0) { $box.css("top", (numb($box.css("top")) - textTopSpace) + "px"); yShift += textTopSpace; } textTop = $text.btPosition().top + numb($text.css("margin-top")); textLeft = $text.btPosition().left + numb($text.css("margin-left")); textRight = textLeft + $text.btOuterWidth(); textBottom = textTop + $text.outerHeight(); textCenter = { x: textLeft + ($text.btOuterWidth() * opts.centerPointX), y: textTop + ($text.outerHeight() * opts.centerPointY) }; points[points.length] = spikePoint = { x: shadowShiftX, y: ((textBottom - textTop) * 0.5) + yShift, type: "spike" }; crossPoint = findIntersectY(spikePoint.x, spikePoint.y, textCenter.x, textCenter.y, textLeft); crossPoint.y = crossPoint.y < textTop + opts.spikeGirth / 2 + opts.cornerRadius ? textTop + opts.spikeGirth / 2 + opts.cornerRadius : crossPoint.y; crossPoint.y = crossPoint.y > (textBottom - opts.spikeGirth / 2) - opts.cornerRadius ? (textBottom - opts.spikeGirth / 2) - opts.cornerRadius : crossPoint.y; points[points.length] = { x: textLeft, y: crossPoint.y - opts.spikeGirth / 2, type: "join" }; points[points.length] = { x: textLeft, y: textTop, type: "corner" }; points[points.length] = { x: textRight, y: textTop, type: "corner" }; points[points.length] = { x: textRight, y: textBottom, type: "corner" }; points[points.length] = { x: textLeft, y: textBottom, type: "corner" }; points[points.length] = { x: textLeft, y: crossPoint.y + opts.spikeGirth / 2, type: "join" }; points[points.length] = spikePoint; break; } var canvas = document.createElement("canvas"); $(canvas).attr("width", (numb($text.btOuterWidth(true)) + opts.strokeWidth * 2 + shadowMarginX)).attr("height", (numb($text.outerHeight(true)) + opts.strokeWidth * 2 + shadowMarginY)).appendTo($box).css({ position: "absolute", zIndex: opts.boxzIndex }); if (typeof G_vmlCanvasManager != "undefined") { canvas = G_vmlCanvasManager.initElement(canvas); } if (opts.cornerRadius > 0) { var newPoints = new Array(); var newPoint; for (var i = 0; i < points.length; i++) { if (points[i].type == "corner") { newPoint = betweenPoint(points[i], points[(i - 1) % points.length], opts.cornerRadius); newPoint.type = "arcStart"; newPoints[newPoints.length] = newPoint; newPoints[newPoints.length] = points[i]; newPoint = betweenPoint(points[i], points[(i + 1) % points.length], opts.cornerRadius); newPoint.type = "arcEnd"; newPoints[newPoints.length] = newPoint; } else { newPoints[newPoints.length] = points[i]; } } points = newPoints; } var ctx = canvas.getContext("2d"); if (opts.shadow && opts.shadowOverlap !== true) { var shadowOverlap = numb(opts.shadowOverlap); switch (position) { case "top": if (opts.shadowOffsetX + opts.shadowBlur - shadowOverlap > 0) { $box.css("top", (numb($box.css("top")) - (opts.shadowOffsetX + opts.shadowBlur - shadowOverlap))); } break; case "right": if (shadowShiftX - shadowOverlap > 0) { $box.css("left", (numb($box.css("left")) + shadowShiftX - shadowOverlap)); } break; case "bottom": if (shadowShiftY - shadowOverlap > 0) { $box.css("top", (numb($box.css("top")) + shadowShiftY - shadowOverlap)); } break; case "left": if (opts.shadowOffsetY + opts.shadowBlur - shadowOverlap > 0) { $box.css("left", (numb($box.css("left")) - (opts.shadowOffsetY + opts.shadowBlur - shadowOverlap))); } break; } } drawIt.apply(ctx, [points], opts.strokeWidth); ctx.fillStyle = opts.fill; if (opts.shadow) { ctx.shadowOffsetX = opts.shadowOffsetX; ctx.shadowOffsetY = opts.shadowOffsetY; ctx.shadowBlur = opts.shadowBlur; ctx.shadowColor = opts.shadowColor; } ctx.closePath(); ctx.fill(); if (opts.strokeWidth > 0) { ctx.shadowColor = "rgba(0, 0, 0, 0)"; ctx.lineWidth = opts.strokeWidth; ctx.strokeStyle = opts.strokeStyle; ctx.beginPath(); drawIt.apply(ctx, [points], opts.strokeWidth); ctx.closePath(); ctx.stroke(); } opts.preShow.apply(this, [$box[0]]); $box.css({ display: "none", visibility: "visible" }); opts.showTip.apply(this, [$box[0]]); if (opts.overlay) { var overlay = $('<div class="bt-overlay"></div>').css({ position: "absolute", backgroundColor: "blue", top: top, left: left, width: width, height: height, opacity: ".2" }).appendTo(offsetParent); $(this).data("overlay", overlay); } if ((opts.ajaxPath != null && opts.ajaxCache == false) || ajaxTimeout) { content = false; } if (opts.clickAnywhereToClose) { jQuery.bt.vars.clickAnywhereStack.push(this); $(document).click(jQuery.bt.docClick); } if (opts.closeWhenOthersOpen) { jQuery.bt.vars.closeWhenOpenStack.push(this); } opts.postShow.apply(this, [$box[0]]); }; this.btOff = function () { var box = $(this).data("bt-box"); opts.preHide.apply(this, [box]); var i = this; i.btCleanup = function () { var box = $(i).data("bt-box"); var contentOrig = $(i).data("bt-content-orig"); var overlay = $(i).data("bt-overlay"); if (typeof box == "object") { $(box).remove(); $(i).removeData("bt-box"); } if (typeof contentOrig == "object") { var clones = $(contentOrig.original).data("bt-clones"); $(contentOrig).data("bt-clones", arrayRemove(clones, contentOrig.clone)); } if (typeof overlay == "object") { $(overlay).remove(); $(i).removeData("bt-overlay"); } jQuery.bt.vars.clickAnywhereStack = arrayRemove(jQuery.bt.vars.clickAnywhereStack, i); jQuery.bt.vars.closeWhenOpenStack = arrayRemove(jQuery.bt.vars.closeWhenOpenStack, i); $(i).removeClass("bt-active " + opts.activeClass); opts.postHide.apply(i); }; opts.hideTip.apply(this, [box, i.btCleanup]); }; var refresh = this.btRefresh = function () { this.btOff(); this.btOn(); }; }); function drawIt(points, strokeWidth) { this.moveTo(points[0].x, points[0].y); for (i = 1; i < points.length; i++) { if (points[i - 1].type == "arcStart") { this.quadraticCurveTo(round5(points[i].x, strokeWidth), round5(points[i].y, strokeWidth), round5(points[(i + 1) % points.length].x, strokeWidth), round5(points[(i + 1) % points.length].y, strokeWidth)); i++; } else { this.lineTo(round5(points[i].x, strokeWidth), round5(points[i].y, strokeWidth)); } } } function round5(num, strokeWidth) { var ret; strokeWidth = numb(strokeWidth); if (strokeWidth % 2) { ret = num; } else { ret = Math.round(num - 0.5) + 0.5; } return ret; } function numb(num) { return parseInt(num) || 0; } function arrayRemove(arr, elem) { var x, newArr = new Array(); for (x in arr) { if (arr[x] != elem) { newArr.push(arr[x]); } } return newArr; } function canvasSupport() { var canvas_compatible = false; try { canvas_compatible = !!(document.createElement("canvas").getContext("2d")); } catch (e) { canvas_compatible = !!(document.createElement("canvas").getContext); } return canvas_compatible; } function shadowSupport() { try { var userAgent = navigator.userAgent.toLowerCase(); if (/webkit/.test(userAgent)) { return true; } else { if (/gecko|mozilla/.test(userAgent) && parseFloat(userAgent.match(/firefox\/(\d+(?:\.\d+)+)/)[1]) >= 3.1) { return true; } } } catch (err) { } return false; } function betweenPoint(point1, point2, dist) { var y, x; if (point1.x == point2.x) { y = point1.y < point2.y ? point1.y + dist : point1.y - dist; return { x: point1.x, y: y }; } else { if (point1.y == point2.y) { x = point1.x < point2.x ? point1.x + dist : point1.x - dist; return { x: x, y: point1.y }; } } } function centerPoint(arcStart, corner, arcEnd) { var x = corner.x == arcStart.x ? arcEnd.x : arcStart.x; var y = corner.y == arcStart.y ? arcEnd.y : arcStart.y; var startAngle, endAngle; if (arcStart.x < arcEnd.x) { if (arcStart.y > arcEnd.y) { startAngle = (Math.PI / 180) * 180; endAngle = (Math.PI / 180) * 90; } else { startAngle = (Math.PI / 180) * 90; endAngle = 0; } } else { if (arcStart.y > arcEnd.y) { startAngle = (Math.PI / 180) * 270; endAngle = (Math.PI / 180) * 180; } else { startAngle = 0; endAngle = (Math.PI / 180) * 270; } } return { x: x, y: y, type: "center", startAngle: startAngle, endAngle: endAngle }; } function findIntersect(r1x1, r1y1, r1x2, r1y2, r2x1, r2y1, r2x2, r2y2) { if (r2x1 == r2x2) { return findIntersectY(r1x1, r1y1, r1x2, r1y2, r2x1); } if (r2y1 == r2y2) { return findIntersectX(r1x1, r1y1, r1x2, r1y2, r2y1); } var r1m = (r1y1 - r1y2) / (r1x1 - r1x2); var r1b = r1y1 - (r1m * r1x1); var r2m = (r2y1 - r2y2) / (r2x1 - r2x2); var r2b = r2y1 - (r2m * r2x1); var x = (r2b - r1b) / (r1m - r2m); var y = r1m * x + r1b; return { x: x, y: y }; } function findIntersectY(r1x1, r1y1, r1x2, r1y2, x) { if (r1y1 == r1y2) { return { x: x, y: r1y1 }; } var r1m = (r1y1 - r1y2) / (r1x1 - r1x2); var r1b = r1y1 - (r1m * r1x1); var y = r1m * x + r1b; return { x: x, y: y }; } function findIntersectX(r1x1, r1y1, r1x2, r1y2, y) { if (r1x1 == r1x2) { return { x: r1x1, y: y }; } var r1m = (r1y1 - r1y2) / (r1x1 - r1x2); var r1b = r1y1 - (r1m * r1x1); var x = (y - r1b) / r1m; return { x: x, y: y }; } }; jQuery.fn.btPosition = function () { function num(elem, prop) { return elem[0] && parseInt(jQuery.curCSS(elem[0], prop, true), 10) || 0; } var left = 0, top = 0, results; if (this[0]) { var offsetParent = this.offsetParent(), offset = this.offset(), parentOffset = /^body|html$/i.test(offsetParent[0].tagName) ? { top: 0, left: 0} : offsetParent.offset(); offset.top -= num(this, "marginTop"); offset.left -= num(this, "marginLeft"); parentOffset.top += num(offsetParent, "borderTopWidth"); parentOffset.left += num(offsetParent, "borderLeftWidth"); results = { top: offset.top - parentOffset.top, left: offset.left - parentOffset.left }; } return results; }; jQuery.fn.btOuterWidth = function (margin) { function num(elem, prop) { return elem[0] && parseInt(jQuery.curCSS(elem[0], prop, true), 10) || 0; } return this["innerWidth"]() + num(this, "borderLeftWidth") + num(this, "borderRightWidth") + (margin ? num(this, "marginLeft") + num(this, "marginRight") : 0); }; jQuery.fn.btOn = function () { return this.each(function (index) { if (jQuery.isFunction(this.btOn)) { this.btOn(); } }); }; jQuery.fn.btOff = function () { return this.each(function (index) { if (jQuery.isFunction(this.btOff)) { this.btOff(); } }); }; jQuery.bt.vars = { clickAnywhereStack: [], closeWhenOpenStack: [] }; jQuery.bt.docClick = function (e) { if (!e) { var e = window.event; } if (!$(e.target).parents().andSelf().filter(".bt-wrapper, .bt-active").length && jQuery.bt.vars.clickAnywhereStack.length) { $(jQuery.bt.vars.clickAnywhereStack).btOff(); $(document).unbind("click", jQuery.bt.docClick); } }; jQuery.bt.defaults = { trigger: "hover", clickAnywhereToClose: true, closeWhenOthersOpen: false, shrinkToFit: false, width: "200px", padding: "10px", spikeGirth: 10, spikeLength: 15, overlap: 0, overlay: false, killTitle: true, textzIndex: 9999, boxzIndex: 9998, wrapperzIndex: 9997, offsetParent: null, positions: ["most"], fill: "rgb(255, 255, 102)", windowMargin: 10, strokeWidth: 1, strokeStyle: "#000", cornerRadius: 5, centerPointX: 0.5, centerPointY: 0.5, shadow: false, shadowOffsetX: 2, shadowOffsetY: 2, shadowBlur: 3, shadowColor: "#000", shadowOverlap: false, noShadowOpts: { strokeStyle: "#999" }, cssClass: "", cssStyles: {}, activeClass: "bt-active", contentSelector: "$(this).attr('title')", ajaxPath: null, ajaxError: "<strong>ERROR:</strong> <em>%error</em>", ajaxLoading: "<blink>Loading...</blink>", ajaxData: {}, ajaxType: "GET", ajaxCache: true, ajaxOpts: {}, preBuild: function () { }, preShow: function (box) { }, showTip: function (box) { $(box).show(); }, postShow: function (box) { }, preHide: function (box) { }, hideTip: function (box, callback) { $(box).hide(); callback(); }, postHide: function () { }, hoverIntentOpts: { interval: 300, timeout: 500} }; jQuery.bt.options = {}; })(jQuery);

/**
* hoverIntent r5 // 2007.03.27 // jQuery 1.1.2+
* <http://cherne.net/brian/resources/jquery.hoverIntent.html>
* 
* @param  f  onMouseOver function || An object with configuration options
* @param  g  onMouseOut function  || Nothing (use configuration options object)
* @author    Brian Cherne <brian@cherne.net>
*/
(function($){$.fn.hoverIntent=function(f,g){var cfg={sensitivity:7,interval:100,timeout:0};cfg=$.extend(cfg,g?{over:f,out:g}:f);var cX,cY,pX,pY;var track=function(ev){cX=ev.pageX;cY=ev.pageY;};var compare=function(ev,ob){ob.hoverIntent_t=clearTimeout(ob.hoverIntent_t);if((Math.abs(pX-cX)+Math.abs(pY-cY))<cfg.sensitivity){$(ob).unbind("mousemove",track);ob.hoverIntent_s=1;return cfg.over.apply(ob,[ev]);}else{pX=cX;pY=cY;ob.hoverIntent_t=setTimeout(function(){compare(ev,ob);},cfg.interval);}};var delay=function(ev,ob){ob.hoverIntent_t=clearTimeout(ob.hoverIntent_t);ob.hoverIntent_s=0;return cfg.out.apply(ob,[ev]);};var handleHover=function(e){var p=(e.type=="mouseover"?e.fromElement:e.toElement)||e.relatedTarget;while(p&&p!=this){try{p=p.parentNode;}catch(e){p=this;}}if(p==this){return false;}var ev=jQuery.extend({},e);var ob=this;if(ob.hoverIntent_t){ob.hoverIntent_t=clearTimeout(ob.hoverIntent_t);}if(e.type=="mouseover"){pX=ev.pageX;pY=ev.pageY;$(ob).bind("mousemove",track);if(ob.hoverIntent_s!=1){ob.hoverIntent_t=setTimeout(function(){compare(ev,ob);},cfg.interval);}}else{$(ob).unbind("mousemove",track);if(ob.hoverIntent_s==1){ob.hoverIntent_t=setTimeout(function(){delay(ev,ob);},cfg.timeout);}}};return this.mouseover(handleHover).mouseout(handleHover);};})(jQuery);

// Jquery easing.. 
// t: current time, b: begInnIng value, c: change In value, d: duration
jQuery.easing['jswing'] = jQuery.easing['swing'];
jQuery.extend(jQuery.easing, { def: 'easeOutQuad', swing: function (x, t, b, c, d) { return jQuery.easing[jQuery.easing.def](x, t, b, c, d) }, easeInQuad: function (x, t, b, c, d) { return c * (t /= d) * t + b }, easeOutQuad: function (x, t, b, c, d) { return -c * (t /= d) * (t - 2) + b }, easeInOutQuad: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return c / 2 * t * t + b; return -c / 2 * ((--t) * (t - 2) - 1) + b }, easeInCubic: function (x, t, b, c, d) { return c * (t /= d) * t * t + b }, easeOutCubic: function (x, t, b, c, d) { return c * ((t = t / d - 1) * t * t + 1) + b }, easeInOutCubic: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return c / 2 * t * t * t + b; return c / 2 * ((t -= 2) * t * t + 2) + b }, easeInQuart: function (x, t, b, c, d) { return c * (t /= d) * t * t * t + b }, easeOutQuart: function (x, t, b, c, d) { return -c * ((t = t / d - 1) * t * t * t - 1) + b }, easeInOutQuart: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b; return -c / 2 * ((t -= 2) * t * t * t - 2) + b }, easeInQuint: function (x, t, b, c, d) { return c * (t /= d) * t * t * t * t + b }, easeOutQuint: function (x, t, b, c, d) { return c * ((t = t / d - 1) * t * t * t * t + 1) + b }, easeInOutQuint: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b; return c / 2 * ((t -= 2) * t * t * t * t + 2) + b }, easeInSine: function (x, t, b, c, d) { return -c * Math.cos(t / d * (Math.PI / 2)) + c + b }, easeOutSine: function (x, t, b, c, d) { return c * Math.sin(t / d * (Math.PI / 2)) + b }, easeInOutSine: function (x, t, b, c, d) { return -c / 2 * (Math.cos(Math.PI * t / d) - 1) + b }, easeInExpo: function (x, t, b, c, d) { return (t == 0) ? b : c * Math.pow(2, 10 * (t / d - 1)) + b }, easeOutExpo: function (x, t, b, c, d) { return (t == d) ? b + c : c * (-Math.pow(2, -10 * t / d) + 1) + b }, easeInOutExpo: function (x, t, b, c, d) { if (t == 0) return b; if (t == d) return b + c; if ((t /= d / 2) < 1) return c / 2 * Math.pow(2, 10 * (t - 1)) + b; return c / 2 * (-Math.pow(2, -10 * --t) + 2) + b }, easeInCirc: function (x, t, b, c, d) { return -c * (Math.sqrt(1 - (t /= d) * t) - 1) + b }, easeOutCirc: function (x, t, b, c, d) { return c * Math.sqrt(1 - (t = t / d - 1) * t) + b }, easeInOutCirc: function (x, t, b, c, d) { if ((t /= d / 2) < 1) return -c / 2 * (Math.sqrt(1 - t * t) - 1) + b; return c / 2 * (Math.sqrt(1 - (t -= 2) * t) + 1) + b }, easeInElastic: function (x, t, b, c, d) { var s = 1.70158; var p = 0; var a = c; if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3; if (a < Math.abs(c)) { a = c; var s = p / 4 } else var s = p / (2 * Math.PI) * Math.asin(c / a); return -(a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b }, easeOutElastic: function (x, t, b, c, d) { var s = 1.70158; var p = 0; var a = c; if (t == 0) return b; if ((t /= d) == 1) return b + c; if (!p) p = d * .3; if (a < Math.abs(c)) { a = c; var s = p / 4 } else var s = p / (2 * Math.PI) * Math.asin(c / a); return a * Math.pow(2, -10 * t) * Math.sin((t * d - s) * (2 * Math.PI) / p) + c + b }, easeInOutElastic: function (x, t, b, c, d) { var s = 1.70158; var p = 0; var a = c; if (t == 0) return b; if ((t /= d / 2) == 2) return b + c; if (!p) p = d * (.3 * 1.5); if (a < Math.abs(c)) { a = c; var s = p / 4 } else var s = p / (2 * Math.PI) * Math.asin(c / a); if (t < 1) return -.5 * (a * Math.pow(2, 10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p)) + b; return a * Math.pow(2, -10 * (t -= 1)) * Math.sin((t * d - s) * (2 * Math.PI) / p) * .5 + c + b }, easeInBack: function (x, t, b, c, d, s) { if (s == undefined) s = 1.70158; return c * (t /= d) * t * ((s + 1) * t - s) + b }, easeOutBack: function (x, t, b, c, d, s) { if (s == undefined) s = 1.70158; return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b }, easeInOutBack: function (x, t, b, c, d, s) { if (s == undefined) s = 1.70158; if ((t /= d / 2) < 1) return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b; return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b }, easeInBounce: function (x, t, b, c, d) { return c - jQuery.easing.easeOutBounce(x, d - t, 0, c, d) + b }, easeOutBounce: function (x, t, b, c, d) { if ((t /= d) < (1 / 2.75)) { return c * (7.5625 * t * t) + b } else if (t < (2 / 2.75)) { return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b } else if (t < (2.5 / 2.75)) { return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b } else { return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b } }, easeInOutBounce: function (x, t, b, c, d) { if (t < d / 2) return jQuery.easing.easeInBounce(x, t * 2, 0, c, d) * .5 + b; return jQuery.easing.easeOutBounce(x, t * 2 - d, 0, c, d) * .5 + c * .5 + b } });
/*
 * jQuery history plugin
 *
 * Copyright (c) 2006 Taku Sano (Mikage Sawatari)
 * Licensed under the MIT License:
 *   http://www.opensource.org/licenses/mit-license.php
 *
 * Modified by Lincoln Cooper to add Safari support and only call the callback once during initialization
 * for msie when no initial hash supplied.
 * API rewrite by Lauris Buk?is-Haberkorns
 */

(function($) {

function History()
{
	this._curHash = '';
	this._callback = function(hash){};
};

$.extend(History.prototype, {
	init: function(callback) {
		this._callback = callback;
		this._curHash = location.hash;

		if($.browser.msie) {
			// To stop the callback firing twice during initilization if no hash present
			if (this._curHash == '') {
				this._curHash = '#';
			}

			// add hidden iframe for IE
			$("body").prepend('<iframe id="jQuery_history" style="display: none;"></iframe>');
			var iframe = $("#jQuery_history")[0].contentWindow.document;
			iframe.open();
			iframe.close();
			iframe.location.hash = this._curHash;
		}
		else if ($.browser.safari) {
			// etablish back/forward stacks
			this._historyBackStack = [];
			this._historyBackStack.length = history.length;
			this._historyForwardStack = [];
			this._isFirst = true;
			this._dontCheck = false;
		}
		this._callback(this._curHash.replace(/^#/, ''));
		setInterval(this._check, 100);
	},

	add: function(hash) {
		// This makes the looping function do something
		this._historyBackStack.push(hash);
		
		this._historyForwardStack.length = 0; // clear forwardStack (true click occured)
		this._isFirst = true;
	},
	
	_check: function() {
		if($.browser.msie) {
			// On IE, check for location.hash of iframe
			var ihistory = $("#jQuery_history")[0];
			var iframe = ihistory.contentDocument || ihistory.contentWindow.document;
			var current_hash = iframe.location.hash;
			if(current_hash != $.history._curHash) {
			
				location.hash = current_hash;
				$.history._curHash = current_hash;
				$.history._callback(current_hash.replace(/^#/, ''));
				
			}
		} else if ($.browser.safari) {
			if (!$.history._dontCheck) {
				var historyDelta = history.length - $.history._historyBackStack.length;
				
				if (historyDelta) { // back or forward button has been pushed
					$.history._isFirst = false;
					if (historyDelta < 0) { // back button has been pushed
						// move items to forward stack
						for (var i = 0; i < Math.abs(historyDelta); i++) $.history._historyForwardStack.unshift($.history._historyBackStack.pop());
					} else { // forward button has been pushed
						// move items to back stack
						for (var i = 0; i < historyDelta; i++) $.history._historyBackStack.push($.history._historyForwardStack.shift());
					}
					var cachedHash = $.history._historyBackStack[$.history._historyBackStack.length - 1];
					if (cachedHash != undefined) {
						$.history._curHash = location.hash;
						$.history._callback(cachedHash);
					}
				} else if ($.history._historyBackStack[$.history._historyBackStack.length - 1] == undefined && !$.history._isFirst) {
					// back button has been pushed to beginning and URL already pointed to hash (e.g. a bookmark)
					// document.URL doesn't change in Safari
					if (document.URL.indexOf('#') >= 0) {
						$.history._callback(document.URL.split('#')[1]);
					} else {
						$.history._callback('');
					}
					$.history._isFirst = true;
				}
			}
		} else {
			// otherwise, check for location.hash
			var current_hash = location.hash;
			if(current_hash != $.history._curHash) {
				$.history._curHash = current_hash;
				$.history._callback(current_hash.replace(/^#/, ''));
			}
		}
	},

	load: function(hash) {
		var newhash;
		
		if ($.browser.safari) {
			newhash = hash;
		} else {		   
			newhash = '#' + hash;
			location.hash = newhash;			
		}		
		this._curHash = newhash;
		
		if ($.browser.msie) {			
            var ihistory = $("#jQuery_history")[0]; // TODO: need contentDocument?
			var iframe = ihistory.contentWindow.document;			
            iframe.open();           
			iframe.close();          
			iframe.location.hash = newhash;           
			this._callback(hash);            
		}
		else if ($.browser.safari) {
			this._dontCheck = true;
			// Manually keep track of the history values for Safari
			this.add(hash);
			
			// Wait a while before allowing checking so that Safari has time to update the "history" object
			// correctly (otherwise the check loop would detect a false change in hash).
			var fn = function() {$.history._dontCheck = false;};
			window.setTimeout(fn, 200);
			this._callback(hash);
			// N.B. "location.hash=" must be the last line of code for Safari as execution stops afterwards.
			//      By explicitly using the "location.hash" command (instead of using a variable set to "location.hash") the
			//      URL in the browser and the "history" object are both updated correctly.
			location.hash = newhash;
		}
		else {
		  this._callback(hash);
		}
	}
});

$(document).ready(function() {
	$.history = new History(); // singleton instance
    $("#drpCity").find("option[value=-1]").attr('disabled', 'disabled'); // disable particular options in the select menu
    $("#txtMobile").keypress(function(e){
        getSellerDetails(e, box_obj);
    });

    appliedFiltersCities();

    //$("#drpCity").change(function () {    
    //    filter_change = true;
    //    addHashParam();
               
    //    var redirectUrl = "/used-bikes-in-" + $(this).val().split('_')[1]; //$(this).find("option:selected").text().toLowerCase().replace(/ /g, '');
    //    if(hashParams != "")
    //    {
    //        redirectUrl += "/#" + hashParams;
    //    }
    //    window.location.href = redirectUrl;
    //});
});

})(jQuery);

/****************************************************************************************************************/
var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;
var re = /^[0-9]*$/;
//var regUserName = /^[a-zA-Z0-9_-]{3,50}$/;
var hashParams, selectedModels = "";
var filter_change = false;
var box_obj;
var profileId_g = "", inquiryId_g = "", isDealer_g = "";
var buyersName = "", buyersEmail = "", buyersMobile = "", isDetailsViewed = "0", bikeModel_g  = "", makeYear_g = "";
var isParamChecked = false;
var clickedMakeObj;

$("#parms").ready(function () {
    bindEventsOnPageLoad();

    //if (location.search.indexOf("?_escaped_fragment_=") < 0) {
    $.history.init(loadHistory);
    //}
});

function loadHistory(hash) {
    hashParams = hash;
    if (validateHash(hashParams)) {
        if (hashParams == "") {
            //$("#searchRes").html( $("#alert_msg").html() );	        
            //$("#app_filt").hide();	        
            //$("#selectCity").removeClass("hide").show();
        } else {
            //$("#selectCity").hide();
            //processingWait(true);
            setCheckedStatus(hashParams);
            filterResults();
        }
    }
    if (isFirstLoad) {
        // Set query string on page load
        if (hashParams == "") {
            hashParams = queryString;
            setCheckedStatus(hashParams);
            isFirstLoad = false;
        }
    }
}

function setCheckedStatus(hash){
	var paramCollection = hash.split("&");
	selectedModels = "";
	
	resetModelCount();
	clearAppliedCriteria();

	for( var i=0; i<paramCollection.length; i++){
		var param = paramCollection[i].split('=');
		if( param.length == 2 ){			
			var obj_ul_params = $("#"+ param[0]);
			obj_ul_params.removeClass("hide"); // show the respective ul of selected param
			
			$("#"+ param[0] + "_exp_col").attr("title","collapse").removeClass("expnd").addClass("collapse"); // manage expend/collapse icons
			
			// manage checkbox state
			var objFilter = obj_ul_params.find("a[name="+ param[1] +"]");
			objFilter.removeClass("unchecked").addClass("checked");

			if( param[0] == "model" ){
				selectedModels += param[1] + ",";
				var modelName = param[1].split('.')[0];
				
				var modelCountObj = $("#model_count_"+ modelName),
				modelCount = modelCountObj.html();					
				modelCountObj.next().text(" bike selected");
				modelCountObj.html( modelCount == "" ? "1" : parseInt(modelCount) + 1);				
			}
			
			if( param[0] == "make" ){
				showModelLink(objFilter);				
			}
			
			if( param[0] == "mm" ){
				if(param[1] == "1"){
					$("#more_makes").parent().nextAll().removeClass("hide");
					$("#more_makes").find("span").hide();
				}
			}
				

			if ($("#drpCity").val() <= 0) {
			    $("#drpCityDist").attr('disabled', true).val(-1);
			} else {
			    $("#drpCityDist").removeAttr("disabled");
			}

			if (param[0] == "city") {
			    $('#drpCity option[value^="' + param[1] + '_"]').attr('selected', true);
			}
			
			if( param[0] == "dist" )
				$('#drpCityDist option[value='+ param[1] +']').attr('selected',true);
			
			if (param[0] == "city" || param[0] == "dist") {
			    appliedFiltersCities(cityId);
			}
			else {
			 
			    appliedFilters(objFilter, param[1]);
			}
		}				
	}

	if (paramCollection.length > 0) {	    
	    $("#app_filt").removeClass("hide").show();
	}
	
	if( selectedModels.length > 0 )
		selectedModels = selectedModels.substring(0, selectedModels.length-1);											
}

// New appliedFilters function Added By Ashish G. Kamble on 19/3/2012
function appliedFilters(paramObj, paramVal) {
    var ulCriteria = $(paramObj).closest('ul').attr('id'),
	appCriteriaObj = $('#_' + ulCriteria), // parent <ul>		
	append_param_id = ulCriteria + paramVal;    
    if ($(paramObj).hasClass('checked')) {        
        var child_length = appCriteriaObj.children().length;
        var clause_name = child_length == 1 ? " " + $(paramObj).html() : "" + $(paramObj).html(),
		append_param = '<span id="' + ulCriteria + "_" + paramVal + '" class="text-grey2 sel_parama">' + clause_name + '<span id="removeSel">X</span></span>';        
        appCriteriaObj.removeClass("hide").append(append_param);
    } else {        
        $('#' + append_param_id).remove();
    }
}

function appliedFiltersCities(yourCity){	
    $("#_city span:not(:first-child)").remove();

    if ($('#drpCity option:selected').val() != '0')
        $("#_city").removeClass("hide").append("<span id='your_city' class='text-grey2'>" + $('#drpCity option:selected').text() + " within " + $('#drpCityDist option:selected').text() + "</span>");
}

function showModelLink(objFilter){
    objFilter.prev().removeClass("hide").show();
}

function resetModelCount(){
	$("span.modc").html("");
}
	
function bindEventsOnPageLoad() {
    initURLParameters();
    onChkBoxClick();
	onCityChange();	
    //rowExpend();
    //bikeDetails();
	//onSortOrderChange();
	removeOnSelCriteriaClick();
}

/************************************************************************************/
	// Page Load Event Bindings
/************************************************************************************/
function initURLParameters() {
    //$("#drpCity").find("option[value=-1]").attr('disabled', 'disabled'); // disable particular options in the select menu

    if ($("#drpCity").val() <= 0) {
        $("#drpCityDist").attr('disabled', true).val(-1);
    }
}
function onChkBoxClick(){
    $("#parms a.filter, sortLink").live('click', function (e) {
        e.preventDefault();
        
        //if (isCitySelected()) {
            $(this).toggleClass("checked unchecked");

            var paramName = this.href.split('#')[1];
            
            filter_change = true;
           
            if ($(this).hasClass("checked")) {
                isParamChecked = true;
                hashParams += hashParams == "" ? paramName : "&" + paramName;
            } else {                
                removeParamFromHashURL(paramName);
                isParamChecked = false;
            }
                    
            addHashParam(); // Reload contents

            if (!isParamChecked && hashParams == "") {
                clearAppliedCriteria();
                filterResults();
                appliedFiltersCities();
            }
        //}
    });		
}

//written By : Ashwini Todkar 
function onSortOrderChange()
{
    $("#ddlSort").change(function (e) {
        //e.preventDefault();

        clearAppliedCriteria();

        var navi_lnk = this.href;
        var qs = $(this).val();

        sortRegEx = /sc=[0-9]&so=[0-9]/;

        if (hashParams == "") {
            //hashParams = "city=" + $("#drpCity").val().split('_')[0] + "&dist=" + $("#drpCityDist").val();
                hashParams +=  qs;
        }
        else {
            if (sortRegEx.test(hashParams)) {
                    hashParams = hashParams.replace(sortRegEx, qs);
            } else {
                    hashParams += "&" + qs;
            }
        }

        // Regex for sort by relevance
        regexRel = /&?sc=-1&so=-1/;
        if (regexRel.test(hashParams)) {
            hashParams = hashParams.replace(regexRel, "");
        }

        if (hashParams.indexOf('&') == 0) {
           hashParams = hashParams.replace("&", "");
        }

        $.history.load(hashParams);

        if (hashParams == "") {
            loadSearchResults();
        }
    });
}

//Modified By : Ashwini Todkar
function onCityChange() {
    $("#drpCity,#drpCityDist").change(function () {

        if (isCitySelected()) {

            if (($(this).attr("id") == "drpCity") && ($(this).val() == "0")) {

                var objParam = eval('(' + $(this).attr('class') + ')'), paramVal = $(this).val();
                completeParam = objParam.param + "=" + paramVal;

                var reg = /(&?city).[0-9]*/;
                var regDist = /(&?dist).[0-9]*/;

                if (hashParams.indexOf('dist') >= 0) {
                    hashParams = hashParams.replace(regDist, "");
                }

                if (hashParams.indexOf('city') >= 0) {
                    hashParams = hashParams.replace(reg, "");
                }

                hashParams += hashParams == "" ? completeParam : "&" + completeParam;
            }
            else {
                $("#drpCityDist").removeAttr("disabled");

                if ($(this).attr("id") == "drpCity") {
                    $("#drpCityDist").val("50");
                }
                var objParam = eval('(' + $(this).attr('class') + ')'), paramVal = $(this).val().split('_')[0];
                completeParam = objParam.param + "=" + paramVal;
                var reg = "";

                if (objParam.param == "city") {
                    reg = /(&?city).[0-9]*/;

                    if (hashParams.indexOf('dist') < 0)
                        completeParam += "&dist=" + $("#drpCityDist").val();
                }
                else
                    reg = /(&?dist).[0-9]*/;

                hashParams = hashParams.replace(reg, "");// remove paramater
                hashParams += hashParams == "" ? completeParam : "&" + completeParam;
            }
            replaceFirstAmp();
            clearAppliedCriteria();

            if (hashParams != "")
                $("#app_filt").removeClass("hide").show(); // show applied filters

            if ($.browser.msie)
                $('#drpCityDist').focus();

            addHashParam();

        }
	});	
}

$(".dgNavDivTop a").live('click', function (e) {
    e.preventDefault();  
    clearAppliedCriteria();
    
    removeParamFromHashURL("pn=" + $(".pgSel[navid]").attr("navid"));
    hashParams += hashParams == "" ? "pn=" + $(this).attr("navid") : "&pn=" + $(this).attr("navid");

    $.history.load(hashParams);
});


//$("a.sortLink").live('click', function (e) {
//    e.preventDefault();
//    //$('html,body').animate({ scrollTop: "200px" }, 0);
//    clearAppliedCriteria();
//    //processingWait(true);
//    var navi_lnk = this.href;
//    var qs = navi_lnk.split("?")[1];

//    sortRegEx = /sc=[0-9]&so=[0-9]/;

//    if (hashParams == "") {
//        hashParams += qs;
//    }
//    else {
//        if (sortRegEx.test(hashParams)) {
//            hashParams = hashParams.replace(sortRegEx, qs);
//        } else {
//            hashParams += "&" + qs;
//        }
//    }

//    $.history.load(hashParams);    
//});

$("#entire_state").live('click', function () {
    clearAppliedCriteria();
    replaceKeyFromHashURL("dist");
    hashParams += "&dist=1";
    addHashParam();
});

function initGetSellerDetails()
{
        $(".btnShowinterst").bt({
            contentSelector: "$('#contact').html()", fill: '#ffffff', strokeWidth: 1, strokeStyle: '#D3D3D3', trigger: ['click', 'none'], width: '370px', spikeLength: 7, shadow: true, positions: ['right', 'left', 'bottom'],
            preShow: function (box) {
                $("div.bt-wrapper").hide();
            }, showTip: function (box) {
                boxObj = $(box);
                boxObj.removeClass("hide").show();
                boxObj.find("#closeBox").click(function () {
                    boxObj.hide();
                });

                profileId_g = $(this).parent().attr('ProfileId');
                inquiryId_g = profileId_g.substring(1, profileId_g.length);
                isDealer_g = $(this).parent().attr('sellerType') == 1 ? 1 : 0;
                //alert(isDealer_g);
                makeYear_g = $(this).parent().attr('ModelYear');
                bikeModel_g = $(this).parent().attr('ModelName');
          
                initBT_Box(boxObj);
            }
        });

        inquiryId = inquiryId_g;
        isDealer = isDealer_g;

   
     //call a ajax function to update view count of this bike
       //$.ajax({
       //    type: "POST",
       //    url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedSearch,Bikewale.ashx",
       //    data: '{"inquiryId":"' + inquiryId + '", "isDealer":"' + isDealer + '"}',
       //    beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateViewCount"); },
       //    success: function (response) { }
       // });

        //$(".front_image").css({ 'cursor': 'pointer' });

        //$('.front_image').click(function () {
        //    profileId = $(this).attr('profileId');
        //    photoCount = $(this).attr('photoCount');
        //    if(photoCount > 0)
        //        showPhotos(profileId);
        //    });
  
}

function rowExpend(){
    $(".dt_body").live('click', function () {
        var currRow = $(this);
        
        currRow.hide(); // hide clicked row

        var profileId = currRow.attr('id');
        var inquiryId = profileId.substring(1, profileId.length); // needed for fatcing seller details
        var seller_type_str = currRow.find("#seller_type").text();
        var isDealer = seller_type_str == "Dealer" ? "1" : "0";

        var expnd_row = $("#expended_row");

        var front_img_url = currRow.find("#imgUrl").val();
        
        if (front_img_url != "") {
            var hostUrl = "https://" + currRow.find("#host_url").val();
            var compImgUrl = hostUrl + front_img_url;
           
            expnd_row.find("#front_image").attr('src', compImgUrl).css({ 'cursor': 'pointer' });
            expnd_row.find("#photo_count").text(currRow.find("#photo_count").val() + " Photos");
        } else {
            expnd_row.find("#front_image").attr('src', 'https://imgd.aeplcdn.com/0x0/bikewaleimg/images/noimage.png').css({ 'cursor': 'default' });
            expnd_row.find("#photo_count").text("");
        }

        var areaName = currRow.find("#_areaName").val() == "" ? "" : currRow.find("#_areaName").val() + ", ";        

        expnd_row.find("#bike_row").text(currRow.find("#bike").text());
        expnd_row.find("#price_row").text(currRow.find("td:eq(2)").text());
        expnd_row.find("#kms_row").text(currRow.find("td:eq(4)").text());
        expnd_row.find("#profileId_row").text("(ProfileId: " + profileId + ")");
        expnd_row.find("#model_year_row").text(currRow.find("#make_year").val());
        expnd_row.find("#city_row").text(currRow.find("td:eq(5)").text());
        expnd_row.find("#color_row").text(currRow.find("#color").text());
        expnd_row.find("#last_update_row").text("Last updated: " + currRow.find("td:eq(6)").text());
        expnd_row.find("#seller_row").text(seller_type_str);
        //expnd_row.find("#fuel_type").text(getFuelTypeText(currRow.find("#_fueltype").val()));
        expnd_row.find("#_trans").text(getTransmissionText(currRow.find("#_transm").val()) + ", " + getFuelTypeText(currRow.find("#_fueltype").val()));
        //        expnd_row.find("#go_profile").attr('href', '/used/bikedetails.aspx?bike=' + profileId);
        expnd_row.find("#go_profile").attr('href', currRow.find(".go_bike_profile").attr('href'));

        var clickedRow = currRow.find("#chkcomp");

        var compCheckedStatus = clickedRow.hasClass("unchecked_grey") ? "unchecked_grey" : "checked_grey";
        $("<tr id=\"epnd_" + profileId + "\"><td colspan='7' class='expend_tr'><div class='expend_row grey-bg content-block'>" + $("#expended_row").html() + "</div></td></tr>").insertAfter(currRow).hide().fadeIn(300);

        $("#epnd_" + profileId).find("#chkcomp").live("click", function () {
            if ($(this).hasClass("unchecked_grey")) {
                clickedRow.removeClass("checked_grey").addClass("unchecked_grey");
                currRow.addClass("freeze_row");
            } else {
                clickedRow.removeClass("unchecked_grey freeze_row").addClass("checked_grey");
                currRow.removeClass("freeze_row");
            }
        });

        var expendedRow = currRow.next();
        var epnd_next = expendedRow.next().find("td");
        epnd_next.addClass("expend_next_row");

        expendedRow.find("#close_row, #bike_row").click(function () {
            currRow.removeClass("hide").fadeIn(500);
            expendedRow.remove();
            epnd_next.removeClass("expend_next_row");
        });

        if (front_img_url != "") {
            expendedRow.find("#photo_count,#front_image").click(function () {
                showPhotos(profileId);
            });
        }

        expendedRow.find("#btnShowinterst").bt({contentSelector: "$('#contact').html()", fill: '#ffffff', strokeWidth: 1, strokeStyle: '#D3D3D3', trigger: ['click', 'none'], width: '370px', spikeLength: 7, shadow: true, positions: ['right', 'left', 'bottom'],
            preShow: function (box) {
                $("div.bt-wrapper").hide();
            }, showTip: function (box) {
                boxObj = $(box);
                boxObj.removeClass("hide").show();
                boxObj.find("#closeBox").click(function () {
                    boxObj.hide();
                });

                profileId_g = currRow.attr("id");
                inquiryId_g = currRow.attr("id").substring(1, profileId.length);
                isDealer_g = currRow.find("#seller_type").text() == "Dealer" ? "1" : "0";
                bikeModel_g = currRow.find("#model_name").val(), makeYear_g = currRow.find("#bike_year").text();

                initBT_Box(boxObj);
            }
        });

        // call a ajax function to update view count of this bike
        $.ajax({
            type: "POST",
            url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedSearch,Bikewale.ashx",
            data: '{"inquiryId":"' + inquiryId + '", "isDealer":"' + isDealer + '"}',
            beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UpdateViewCount"); },
            success: function (response) { }
        });
    }).live('mouseenter', function () {
        var _objRow = $(this);
        _objRow.addClass('dt_body_hover').find("#expandables").removeClass("hide").show();//.find("#expandables,#chkcomp").show();
    }).live('mouseleave', function () {
        var _objRow = $(this);
        _objRow.removeClass('dt_body_hover').find("#expandables").hide();
    });
}

// Event Binding Ends Here
/****************************************************************************/
function addHashParam() {    
    if (filter_change) {
        //alert(hashParams);
		if( hashParams.indexOf("pn") >= 0){
			var reg_str = "(&?pn=)[0-9]*";
			var regExp = new RegExp(reg_str);
			hashParams = hashParams.replace(regExp, "");		
			if (hashParams.indexOf("&") == 0)
			{
                hashParams = hashParams.replace("&", "");;
			}
		}        
	}	
	$.history.load(hashParams);
}

function isCitySelected(){
    var sel_city = $("#drpCity").val();
    //alert(sel_city);
	//if( sel_city == "0"){
	//	alert("Please select your city");
	//	return false;
    //} else
	if (sel_city == "-1" || sel_city == "") {
		alert("You have selected invalid city");
		return false;	
	}else return true;	
}

function unselectModels(makeId){	
	var regExp = new RegExp('(&?model=)'+ makeId  + '.[0-9]*', "g");
	hashParams = hashParams.replace(regExp, '');
}

function removeParamFromHashURL(paramName) {
    var _hashParams = "";
    if (hashParams != "") {
        var hashParamArray = hashParams.split('&');        
        if (hashParamArray.length > 0){
            for (var i = 0; i < hashParamArray.length; i++) {                
                if (hashParamArray[i] != paramName){
                    _hashParams = _hashParams + hashParamArray[i] + "&";
                }
            }
        }
    }
    /*alert("_hashParams : " + _hashParams);
    if (_hashParams != "") {
        hashParams = _hashParams.substr(0, _hashParams.length - 1); // remove the last '&'
    } else {
        hashParams
    }*/
    hashParams = _hashParams.substr(0, _hashParams.length - 1); // remove the last '&'
    //hashParams = _hashParams;
}

function replaceKeyFromHashURL(paramKey){ //remove on the basis of only parameter key
	var regEx = new RegExp("&?("+ paramKey +"=)[0-9]*", "g");
	hashParams = hashParams.replace(regEx, "");
	replaceFirstAmp();	
}

function replaceFirstAmp(){
	if( hashParams.indexOf("&") == 0 )
		hashParams = hashParams.replace("&", "");
}

function loadingDone(){
	processingDone();	
}

function removeSelection(){	
	beginNewSearch();
	$("#searchRes").html( $("#alert_msg").html() );
}

function clearAppliedCriteria(){	
	$("#app_filters li span:not(:first-child)").remove();
	$("#app_filters li").hide();
}

function filterResults() {
    processingWait();
    if (hashParams != "") { // param selected
        loadSearchResults();

    } else {
        $("#searchRes").load("/used/searchresult.aspx?city=" + cityId);
    }
    processingDone();
}

function loadSearchResults() {
    if (hashParams != "") {
        if (hashParams.indexOf('city') >= 0) {
            $("#searchRes").load("/used/searchresult.aspx?" + hashParams);
        } else {
            $("#searchRes").load("/used/searchresult.aspx?city=" + cityId + "&" + hashParams);
        }
    } else {
        //alert("inside else part");
        $("#searchRes").load("/used/searchresult.aspx?city=" + cityId );
    }
}

function initBT_Box(boxObj) {
	boxObj.find("#initWait").css({"display":"inline-block"});
	boxObj.find("#buyer_form").hide();
	
	shownInterestInThisBike(boxObj);
}

function shownInterestInThisBike(boxObj) {
    //alert(inquiryId_g);
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedBuyer,Bikewale.ashx",
        data: '{"inquiryId":"' + inquiryId_g + '", "isDealer":"' + isDealer_g + '"}',
        dataType: 'json',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ShownInterestInThisBike"); },
        success: function (response) {                       
            var ds = eval('(' + response.value + ')');
            if (ds.ShownInterest == "True") { // buyer already shown interest; Show seller information;         
                prepareSellInfo(boxObj, ds);
            } else { // first time showing interest
                boxObj.find("#txtName").val(ds.BuyerName); //Prefill buyer information
                boxObj.find("#txtEmail").val(ds.BuyerEmail);
                boxObj.find("#txtMobile").val(ds.BuyerMobile);

                boxObj.find("#initWait").hide();
                boxObj.find("#buyer_form").removeClass("hide").show();

                boxObj.find("#get_details").click(function (e) {// event binding for "Get Details" Button                                                         
                    // The code below is moved into separate function getSellerDetails so that it can be used globally.
                    getSellerDetails(e,box_obj);
                });
                
            }
        }
    });
}

// Function to get seller details on press of enter
$("#txtMobile").live("keypress",function(e){
    if (e.keyCode == 13) {
        getSellerDetails(e, box_obj);
    }
});

/*
// Function to go back to the verification process on press of back
$("#backToVerification").live("click", function (e) {
    boxObj.find("#self_verify_form").hide();
    boxObj.find("#verifiy_mobile").show();
    $(this).hide();
});
*/

// Function to get seller information process
function getSellerDetails(e, box_obj) {
    buyersName = boxObj.find("#txtName").val();
    buyersEmail = boxObj.find("#txtEmail").val();
    buyersMobile = boxObj.find("#txtMobile").val();

    if (validateForm()) { // validate form before submit
        boxObj.find("#process_img").css({ "display": "inline-block" });
        processPurchaseInq(boxObj);
    }
}

function processPurchaseInq(boxObj) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedBuyer,Bikewale.ashx",
        data: '{"profileId":"' + profileId_g + '", "buyerName":"' + buyersName + '", "buyerEmail":"' + buyersEmail + '","buyerMobile":"' + buyersMobile + '","bikeModel":"' + bikeModel_g + '","makeYear":"' + makeYear_g + '","pageUrl":"' + location.href + '"}',
        dataType: 'json',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ProcessUsedBikePurchaseInquiry"); },
        success: function (response) {
            var ds = eval('(' + response.value + ')');
            processPurchaseInqResponse(boxObj, ds);
        }
    });
}

function processPurchaseInqResponse(boxObj, ds) {
    if (ds.Status == "1") {
        prepareSellInfo(boxObj, ds);
        hideBuyerForm(boxObj);
    } else if (ds.Status == "2") {
        boxObj.find("#not_auth").removeClass("hide").show().html(ds.Message);
        hideBuyerForm(boxObj);
    } else if (ds.Status == "3") { // mobile is not verified
        hideBuyerForm(boxObj);
        showVerificationForm(boxObj);
    } else if (ds.Status == "4") {
        boxObj.find("#not_auth").removeClass("hide").show().html(ds.Message);
        hideBuyerForm(boxObj);
    } else if (ds.Status == "5") {
        boxObj.find("#not_auth").removeClass("hide").show().html(ds.Message);
        hideBuyerForm(boxObj);
    }
}

function hideBuyerForm(boxObj) {
    boxObj.find("#buyer_form").hide();
}

function showVerificationForm(boxObj) {
    boxObj.find("#verifiy_mobile").removeClass("hide").fadeIn(500);

    boxObj.find("#btnVerifyCode").click(function () {
        var verificationCode = boxObj.find("#txtCwiCode").val();

        if (validateCode(verificationCode)) {
            boxObj.find("#processCode").css({ "display": "inline-block" });
            isValidCode(verificationCode, boxObj);           
        }
    });

    //Self verification Process
    boxObj.find("#self_verify").click(function (e) {
        e.preventDefault();

        // Call ajax function to get the cuiCode from the database for current email id and mobile number
        getCUICode(buyersMobile, buyersEmail, boxObj);

        boxObj.find("#verifiy_mobile").hide();
        boxObj.find("#self_verify_form").removeClass("hide").show();
        boxObj.find("#backToVerification").removeClass("hide").show();

        boxObj.find("#btn_selfverify").click(function () {
            selfVerification(buyersMobile, boxObj.find("#self_verify_code").text(), boxObj);
        });
    });
}

function isValidCode(cwiCode, boxObj) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedBuyer,Bikewale.ashx",
        data: '{"mobile":"' + buyersMobile + '", "cwiCode":"' + cwiCode + '"}',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "CheckVerificationCode"); },
        success: function (response) {
            responseObj = eval('(' + response + ')');
            setTimeout(function () { verificationResponse(boxObj, responseObj); }, 500);
        }
    });
}

function verificationResponse(boxObj, responseObj) {    
    if (responseObj.value == true) {
        processPurchaseInq(boxObj);
    } else {
        boxObj.find("#processCode").hide();
        alert("Invalid code! Please re-enter valid code.");
    }
}

function prepareSellInfo(boxObj, ds) {       
    boxObj.find("#seller_name").text(ds.SellerName);
    boxObj.find("#seller_email").text(ds.SellerEmail);
    boxObj.find("#seller_mobile").text(ds.SellerContact);
    boxObj.find("#seller_address").text(ds.SellerAddress);
   
    if (isDealer_g == "1") {
        boxObj.find("#contact_person").text(ds.SellerContactPerson);
    } else {
        boxObj.find("#contact_person").parent().parent().hide();
    }

    boxObj.find("#initWait,#buyer_form,#verifiy_mobile").hide();
    boxObj.find("#seller_details").removeClass("hide").fadeIn(500);
    boxObj.find("#processCode").hide();
}

function requestProcessAlert(){
    alert("Your request is under process. Please have patiance.");
}

function loadNavigation(navi_lnk) {
    $("#searchRes").load(navi_lnk, function () {
        loadingDone();
    });
}

function alertResponse(imgAjaxProcess){
    imgAjaxProcess.hide();
}

// Function to validate email
function validEmail(email){    
    if(reEmail.test(email)){
        return true;
    }else{
        return false;
    }
}   // End validEmail

function validateForm(){
    if ($.trim(buyersName) == "") {
        alert("Please enter your name");
        return false;
    }
    //   $.trim(str)
//} else if (regUserName.test(buyersName) == false) {
//        alert("Your name seems invalid. Please re-enter your valid name.");
//        return false;
//    }
	
	if(buyersEmail == "") {
		alert("Please enter your email address");
		return false;
	} else if (!reEmail.test(buyersEmail.toLowerCase())){
		alert("Invalid email address");
		return false;
	}
	
	if(buyersMobile == ""){
		alert("Please enter your mobile number");
		return false;
	}else if (buyersMobile != "" && re.test(buyersMobile) == false) {
		alert("Invalid mobile number");
		return false;
	} else if ( buyersMobile != "" && ( !re.test(buyersMobile) || buyersMobile.length < 10 || buyersMobile.length > 10 ) ) {
		alert("Your mobile number should be of 10 digits only");
		return false;
	}
	
	return true;
}

function validateCode(code) {
	if( code == "" ){
		alert("Please enter 5-digit verification code to proceed");
		return false;
	}else if( code != "" && ( !re.test(code) || code.length > 5 || code.length < 5 ) ){
		alert("Invalid code! Please re-enter valid code.");
		return false;
	}
	
	return true;
}

function showPhotos(profileId){
	window.open('/used/ViewAlbum.aspx?profileNo=' + profileId,'UploadPhoto',"top=50,left=250,width=770,height=600,scrollbars=yes");
}

function showMoreMakes(){
	$("#more_makes").click( function(){
		$(this).parent().nextAll().removeClass("hide");	
		$(this).find('span').hide();
		hashParams != "" ? hashParams += "&mm=1" : hashParams += "mm=1";
		addHashParam();
	});
}

function validateHash(hash){
	if(hash != ""){		
		var regEx = new RegExp("^(&?[a-z]*=[0-9]*([.][0-9]*)?)+$");		
		return regEx.test(hash);
	}else {return true};
}

function beginNewSearch() {
	$("a.checked").removeClass("checked").addClass("unchecked");
	$(".choose_models").hide();
	clearAppliedCriteria();

	if (hashParams.indexOf("city") >= 0) {
	    if ($("#drpCity").val() == "0")
	        $.history.load("city=" + $("#drpCity").val());
        else
	        $.history.load("city=" + $("#drpCity").val().split('_')[0] + "&dist=50");
	} else {

	    hashParams = "";
	    if ($("#drpCity").val() == 0)
	        $.history.load("city=" + $("#drpCity").val());
        else
	        $.history.load("");
	    //filterResults();
	}

	//$.history.load("");
	//loadSearchResults();
	//appliedFiltersCities();
	//$("#drpCity").val("0");
}

function getFuelTypeText(id) {   
    var fuelVal = "-";    
    switch(id)
    {
        case "1":
            fuelVal = "Petrol";
            break;
        case "2":
            fuelVal = "Diesel";
            break;
        case "3":
            fuelVal = "CNG";
            break;
        case "4":
            fuelVal = "LPG";
            break;
        case "5":
            fuelVal = "Electric";
            break;
    }    

    return fuelVal;
}

function getTransmissionText(id) {
    var transVal = "-";   
    switch (id) {
        case "1":
            transVal = "Automatic";
            break;
        case "2":
            transVal = "Manual";
            break;
    }
    return transVal;
}

// Function added by Ashish G. Kamble on 19/3/2012
/*  Function to highlight the selected criteria on hover     */
$(".sel_parama").live('mouseover', function () {
    $(this).removeClass("sel_parama").addClass("sel_parama_hover");
});

$(".sel_parama_hover").live('mouseout', function () {
    $(this).removeClass("sel_parama_hover").addClass("sel_parama");
});
// End of highlighting the selection criteria

// Function Added By Ashish G. Kamble on 19/3/2012
/* Function used to remove values from the selected criteria. Function will remove values from url and check or uncheck the filters from menu  */
function removeOnSelCriteriaClick() {
    $('#removeSel').live('click', function () {        
        var removeSelCrit = $(this).parent().attr("id");
        var removeId = removeSelCrit.split("_")[0];
        var removeIndexAtId = removeSelCrit.split("_")[1];
        
        // remove selection from the side selection menu
        var removeSelCritLink = $("#" + removeId).find("li a[name = " + removeIndexAtId + "]");
        removeSelCritLink.removeClass("checked").addClass("unchecked");
        
        // remove parameters from url
        removeParamFromHashURL(removeId + "=" + removeIndexAtId);
        
        if (removeId == 'make') {
            removeSelCritLink.prev().hide();
            removeSelCritLink.prev().find(".sel_model_txt").html("select models");
            unselectModels(removeSelCritLink.attr('name'));
        }

        clearAppliedCriteria();        
        if (hashParams != "") {
            addHashParam(); // Reload the page
        } else {
            $.history.load("");
            loadSearchResults();
        }        
        appliedFiltersCities();
    });
}   // End of removeOnSelCriteriaClick function

function processingWait() {
    $('#blackOut-window, #newLoading').removeClass('hide');
}

function processingDone() {
    $('#blackOut-window, #newLoading').addClass('hide');
}
