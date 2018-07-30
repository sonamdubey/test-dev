/*
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
(function(e){function n(f,c){var a=e(c);return a.length<2?a:f.parent().find(c)}function t(f,c){var a=this,l=f.add(a),g=f.children(),k=0,m=c.vertical;j||(j=a);if(g.length>1)g=e(c.items,f);e.extend(a,{getConf:function(){return c},getIndex:function(){return k},getSize:function(){return a.getItems().size()},getNaviButtons:function(){return o.add(p)},getRoot:function(){return f},getItemWrap:function(){return g},getItems:function(){return g.children(c.item).not("."+c.clonedClass)},move:function(b,d){return a.seekTo(k+
b,d)},next:function(b){return a.move(1,b)},prev:function(b){return a.move(-1,b)},begin:function(b){return a.seekTo(0,b)},end:function(b){return a.seekTo(a.getSize()-1,b)},focus:function(){return j=a},addItem:function(b){b=e(b);if(c.circular){g.children("."+c.clonedClass+":last").before(b);g.children("."+c.clonedClass+":first").replaceWith(b.clone().addClass(c.clonedClass))}else g.append(b);l.trigger("onAddItem",[b]);return a},seekTo:function(b,d,h){b.jquery||(b*=1);if(c.circular&&b===0&&k==-1&&d!==
0)return a;if(!c.circular&&b<0||b>a.getSize()||b<-1)return a;var i=b;if(b.jquery)b=a.getItems().index(b);else i=a.getItems().eq(b);var q=e.Event("onBeforeSeek");if(!h){l.trigger(q,[b,d]);if(q.isDefaultPrevented()||!i.length)return a}i=m?{top:-i.position().top}:{left:-i.position().left};k=b;j=a;if(d===undefined)d=c.speed;g.animate(i,d,c.easing,h||function(){l.trigger("onSeek",[b])});return a}});e.each(["onBeforeSeek","onSeek","onAddItem"],function(b,d){e.isFunction(c[d])&&e(a).bind(d,c[d]);a[d]=function(h){e(a).bind(d,
h);return a}});if(c.circular){var r=a.getItems().slice(-1).clone().prependTo(g),s=a.getItems().eq(1).clone().appendTo(g);r.add(s).addClass(c.clonedClass);a.onBeforeSeek(function(b,d,h){if(!b.isDefaultPrevented())if(d==-1){a.seekTo(r,h,function(){a.end(0)});return b.preventDefault()}else d==a.getSize()&&a.seekTo(s,h,function(){a.begin(0)})});a.seekTo(0,0,function(){})}var o=n(f,c.prev).click(function(){a.prev()}),p=n(f,c.next).click(function(){a.next()});!c.circular&&a.getSize()>1&&a.onBeforeSeek(function(b,
d){setTimeout(function(){if(!b.isDefaultPrevented()){o.toggleClass(c.disabledClass,d<=0);p.toggleClass(c.disabledClass,d>=a.getSize()-1)}},1)});c.mousewheel&&e.fn.mousewheel&&f.mousewheel(function(b,d){if(c.mousewheel){a.move(d<0?1:-1,c.wheelSpeed||50);return false}});c.keyboard&&e(document).bind("keydown.scrollable",function(b){if(!(!c.keyboard||b.altKey||b.ctrlKey||e(b.target).is(":input")))if(!(c.keyboard!="static"&&j!=a)){var d=b.keyCode;if(m&&(d==38||d==40)){a.move(d==38?-1:1);return b.preventDefault()}if(!m&&
(d==37||d==39)){a.move(d==37?-1:1);return b.preventDefault()}}});c.initialIndex&&a.seekTo(c.initialIndex,0,function(){})}e.tools=e.tools||{version:"1.2.4"};e.tools.scrollable={conf:{activeClass:"active",circular:false,clonedClass:"cloned",disabledClass:"disabled",easing:"swing",initialIndex:0,item:null,items:".items",keyboard:true,mousewheel:false,next:".next",prev:".prev",speed:400,vertical:false,wheelSpeed:0}};var j;e.fn.scrollable=function(f){var c=this.data("scrollable");if(c)return c;f=e.extend({},
e.tools.scrollable.conf,f);this.each(function(){c=new t(e(this),f);e(this).data("scrollable",c)});return f.api?c:this}})(jQuery);
(function(d){function p(b,g){var h=d(g);return h.length<2?h:b.parent().find(g)}var m=d.tools.scrollable;m.navigator={conf:{navi:".navi",naviItem:null,activeClass:"active",indexed:false,idPrefix:null,history:false}};d.fn.navigator=function(b){if(typeof b=="string")b={navi:b};b=d.extend({},m.navigator.conf,b);var g;this.each(function(){function h(a,c,i){e.seekTo(c);if(j){if(location.hash)location.hash=a.attr("href").replace("#","")}else return i.preventDefault()}function f(){return k.find(b.naviItem||
"> *")}function n(a){var c=d("<"+(b.naviItem||"a")+"/>").click(function(i){h(d(this),a,i)}).attr("href","#"+a);a===0&&c.addClass(l);b.indexed&&c.text(a+1);b.idPrefix&&c.attr("id",b.idPrefix+a);return c.appendTo(k)}function o(a,c){a=f().eq(c.replace("#",""));a.length||(a=f().filter("[href="+c+"]"));a.click()}var e=d(this).data("scrollable"),k=b.navi.jquery?b.navi:p(e.getRoot(),b.navi),q=e.getNaviButtons(),l=b.activeClass,j=b.history&&d.fn.history;if(e)g=e;e.getNaviButtons=function(){return q.add(k)};
f().length?f().each(function(a){d(this).click(function(c){h(d(this),a,c)})}):d.each(e.getItems(),function(a){n(a)});e.onBeforeSeek(function(a,c){setTimeout(function(){if(!a.isDefaultPrevented()){var i=f().eq(c);!a.isDefaultPrevented()&&i.length&&f().removeClass(l).eq(c).addClass(l)}},1)});e.onAddItem(function(a,c){c=n(e.getItems().index(c));j&&c.history(o)});j&&f().history(o)});return b.api?g:this}})(jQuery);

/*
 * @name BeautyTips
 * @desc a tooltips/baloon-help plugin for jQuery
 *
 * @author Jeff Robbins - Lullabot - http://www.lullabot.com
 * @version 0.9.5-rc1  (5/20/2009)
 */
jQuery.bt={version:"0.9.5-rc1"};(function($){jQuery.fn.bt=function(content,options){if(typeof content!="string"){var contentSelect=true;options=content;content=false;}else{var contentSelect=false;}if(jQuery.fn.hoverIntent&&jQuery.bt.defaults.trigger=="hover"){jQuery.bt.defaults.trigger="hoverIntent";}return this.each(function(index){var opts=jQuery.extend(false,jQuery.bt.defaults,jQuery.bt.options,options);opts.spikeLength=numb(opts.spikeLength);opts.spikeGirth=numb(opts.spikeGirth);opts.overlap=numb(opts.overlap);var ajaxTimeout=false;if(opts.killTitle){$(this).find("[title]").andSelf().each(function(){if(!$(this).attr("bt-xTitle")){$(this).attr("bt-xTitle",$(this).attr("title")).attr("title","");}});}if(typeof opts.trigger=="string"){opts.trigger=[opts.trigger];}if(opts.trigger[0]=="hoverIntent"){var hoverOpts=jQuery.extend(opts.hoverIntentOpts,{over:function(){this.btOn();},out:function(){this.btOff();}});$(this).hoverIntent(hoverOpts);}else{if(opts.trigger[0]=="hover"){$(this).hover(function(){this.btOn();},function(){this.btOff();});}else{if(opts.trigger[0]=="now"){if($(this).hasClass("bt-active")){this.btOff();}else{this.btOn();}}else{if(opts.trigger[0]=="none"){}else{if(opts.trigger.length>1&&opts.trigger[0]!=opts.trigger[1]){$(this).bind(opts.trigger[0],function(){this.btOn();}).bind(opts.trigger[1],function(){this.btOff();});}else{$(this).bind(opts.trigger[0],function(){if($(this).hasClass("bt-active")){this.btOff();}else{this.btOn();}});}}}}}this.btOn=function(){if(typeof $(this).data("bt-box")=="object"){this.btOff();}opts.preBuild.apply(this);$(jQuery.bt.vars.closeWhenOpenStack).btOff();$(this).addClass("bt-active "+opts.activeClass);if(contentSelect&&opts.ajaxPath==null){if(opts.killTitle){$(this).attr("title",$(this).attr("bt-xTitle"));}content=$.isFunction(opts.contentSelector)?opts.contentSelector.apply(this):eval(opts.contentSelector);if(opts.killTitle){$(this).attr("title","");}}if(opts.ajaxPath!=null&&content==false){if(typeof opts.ajaxPath=="object"){var url=eval(opts.ajaxPath[0]);url+=opts.ajaxPath[1]?" "+opts.ajaxPath[1]:"";}else{var url=opts.ajaxPath;}var off=url.indexOf(" ");if(off>=0){var selector=url.slice(off,url.length);url=url.slice(0,off);}var cacheData=opts.ajaxCache?$(document.body).data("btCache-"+url.replace(/\./g,"")):null;if(typeof cacheData=="string"){content=selector?$("<div/>").append(cacheData.replace(/<script(.|\s)*?\/script>/g,"")).find(selector):cacheData;}else{var target=this;var ajaxOpts=jQuery.extend(false,{type:opts.ajaxType,data:opts.ajaxData,cache:opts.ajaxCache,url:url,complete:function(XMLHttpRequest,textStatus){if(textStatus=="success"||textStatus=="notmodified"){if(opts.ajaxCache){$(document.body).data("btCache-"+url.replace(/\./g,""),XMLHttpRequest.responseText);}ajaxTimeout=false;content=selector?$("<div/>").append(XMLHttpRequest.responseText.replace(/<script(.|\s)*?\/script>/g,"")).find(selector):XMLHttpRequest.responseText;}else{if(textStatus=="timeout"){ajaxTimeout=true;}content=opts.ajaxError.replace(/%error/g,XMLHttpRequest.statusText);}if($(target).hasClass("bt-active")){target.btOn();}}},opts.ajaxOpts);jQuery.ajax(ajaxOpts);content=opts.ajaxLoading;}}var shadowMarginX=0;var shadowMarginY=0;var shadowShiftX=0;var shadowShiftY=0;if(opts.shadow&&!shadowSupport()){opts.shadow=false;jQuery.extend(opts,opts.noShadowOpts);}if(opts.shadow){if(opts.shadowBlur>Math.abs(opts.shadowOffsetX)){shadowMarginX=opts.shadowBlur*2;}else{shadowMarginX=opts.shadowBlur+Math.abs(opts.shadowOffsetX);}shadowShiftX=(opts.shadowBlur-opts.shadowOffsetX)>0?opts.shadowBlur-opts.shadowOffsetX:0;if(opts.shadowBlur>Math.abs(opts.shadowOffsetY)){shadowMarginY=opts.shadowBlur*2;}else{shadowMarginY=opts.shadowBlur+Math.abs(opts.shadowOffsetY);}shadowShiftY=(opts.shadowBlur-opts.shadowOffsetY)>0?opts.shadowBlur-opts.shadowOffsetY:0;}if(opts.offsetParent){var offsetParent=$(opts.offsetParent);var offsetParentPos=offsetParent.offset();var pos=$(this).offset();var top=numb(pos.top)-numb(offsetParentPos.top)+numb($(this).css("margin-top"))-shadowShiftY;var left=numb(pos.left)-numb(offsetParentPos.left)+numb($(this).css("margin-left"))-shadowShiftX;}else{var offsetParent=($(this).css("position")=="absolute")?$(this).parents().eq(0).offsetParent():$(this).offsetParent();var pos=$(this).btPosition();var top=numb(pos.top)+numb($(this).css("margin-top"))-shadowShiftY;var left=numb(pos.left)+numb($(this).css("margin-left"))-shadowShiftX;}var width=$(this).btOuterWidth();var height=$(this).outerHeight();if(typeof content=="object"){var original=content;var clone=$(original).clone(true).show();var origClones=$(original).data("bt-clones")||[];origClones.push(clone);$(original).data("bt-clones",origClones);$(clone).data("bt-orig",original);$(this).data("bt-content-orig",{original:original,clone:clone});content=clone;}if(typeof content=="null"||content==""){return;}var $text=$('<div class="bt-content"></div>').append(content).css({padding:opts.padding,position:"absolute",width:(opts.shrinkToFit?"auto":opts.width),zIndex:opts.textzIndex,left:shadowShiftX,top:shadowShiftY}).css(opts.cssStyles);var $box=$('<div class="bt-wrapper"></div>').append($text).addClass(opts.cssClass).css({position:"absolute",width:opts.width,zIndex:opts.wrapperzIndex,visibility:"hidden"}).appendTo(offsetParent);if(jQuery.fn.bgiframe){$text.bgiframe();$box.bgiframe();}$(this).data("bt-box",$box);var scrollTop=numb($(document).scrollTop());var scrollLeft=numb($(document).scrollLeft());var docWidth=numb($(window).width());var docHeight=numb($(window).height());var winRight=scrollLeft+docWidth;var winBottom=scrollTop+docHeight;var space=new Object();var thisOffset=$(this).offset();space.top=thisOffset.top-scrollTop;space.bottom=docHeight-((thisOffset+height)-scrollTop);space.left=thisOffset.left-scrollLeft;space.right=docWidth-((thisOffset.left+width)-scrollLeft);var textOutHeight=numb($text.outerHeight());var textOutWidth=numb($text.btOuterWidth());if(opts.positions.constructor==String){opts.positions=opts.positions.replace(/ /,"").split(",");}if(opts.positions[0]=="most"){var position="top";for(var pig in space){position=space[pig]>space[position]?pig:position;}}else{for(var x in opts.positions){var position=opts.positions[x];if((position=="left"||position=="right")&&space[position]>textOutWidth+opts.spikeLength){break;}else{if((position=="top"||position=="bottom")&&space[position]>textOutHeight+opts.spikeLength){break;}}}}var horiz=left+((width-textOutWidth)*0.5);var vert=top+((height-textOutHeight)*0.5);var points=new Array();var textTop,textLeft,textRight,textBottom,textTopSpace,textBottomSpace,textLeftSpace,textRightSpace,crossPoint,textCenter,spikePoint;switch(position){case"top":$text.css("margin-bottom",opts.spikeLength+"px");$box.css({top:(top-$text.outerHeight(true))+opts.overlap,left:horiz});textRightSpace=(winRight-opts.windowMargin)-($text.offset().left+$text.btOuterWidth(true));var xShift=shadowShiftX;if(textRightSpace<0){$box.css("left",(numb($box.css("left"))+textRightSpace)+"px");xShift-=textRightSpace;}textLeftSpace=($text.offset().left+numb($text.css("margin-left")))-(scrollLeft+opts.windowMargin);if(textLeftSpace<0){$box.css("left",(numb($box.css("left"))-textLeftSpace)+"px");xShift+=textLeftSpace;}textTop=$text.btPosition().top+numb($text.css("margin-top"));textLeft=$text.btPosition().left+numb($text.css("margin-left"));textRight=textLeft+$text.btOuterWidth();textBottom=textTop+$text.outerHeight();textCenter={x:textLeft+($text.btOuterWidth()*opts.centerPointX),y:textTop+($text.outerHeight()*opts.centerPointY)};points[points.length]=spikePoint={y:textBottom+opts.spikeLength,x:((textRight-textLeft)*0.5)+xShift,type:"spike"};crossPoint=findIntersectX(spikePoint.x,spikePoint.y,textCenter.x,textCenter.y,textBottom);crossPoint.x=crossPoint.x<textLeft+opts.spikeGirth/2+opts.cornerRadius?textLeft+opts.spikeGirth/2+opts.cornerRadius:crossPoint.x;crossPoint.x=crossPoint.x>(textRight-opts.spikeGirth/2)-opts.cornerRadius?(textRight-opts.spikeGirth/2)-opts.CornerRadius:crossPoint.x;points[points.length]={x:crossPoint.x-(opts.spikeGirth/2),y:textBottom,type:"join"};points[points.length]={x:textLeft,y:textBottom,type:"corner"};points[points.length]={x:textLeft,y:textTop,type:"corner"};points[points.length]={x:textRight,y:textTop,type:"corner"};points[points.length]={x:textRight,y:textBottom,type:"corner"};points[points.length]={x:crossPoint.x+(opts.spikeGirth/2),y:textBottom,type:"join"};points[points.length]=spikePoint;break;case"left":$text.css("margin-right",opts.spikeLength+"px");$box.css({top:vert+"px",left:((left-$text.btOuterWidth(true))+opts.overlap)+"px"});textBottomSpace=(winBottom-opts.windowMargin)-($text.offset().top+$text.outerHeight(true));var yShift=shadowShiftY;if(textBottomSpace<0){$box.css("top",(numb($box.css("top"))+textBottomSpace)+"px");yShift-=textBottomSpace;}textTopSpace=($text.offset().top+numb($text.css("margin-top")))-(scrollTop+opts.windowMargin);if(textTopSpace<0){$box.css("top",(numb($box.css("top"))-textTopSpace)+"px");yShift+=textTopSpace;}textTop=$text.btPosition().top+numb($text.css("margin-top"));textLeft=$text.btPosition().left+numb($text.css("margin-left"));textRight=textLeft+$text.btOuterWidth();textBottom=textTop+$text.outerHeight();textCenter={x:textLeft+($text.btOuterWidth()*opts.centerPointX),y:textTop+($text.outerHeight()*opts.centerPointY)};points[points.length]=spikePoint={x:textRight+opts.spikeLength,y:((textBottom-textTop)*0.5)+yShift,type:"spike"};crossPoint=findIntersectY(spikePoint.x,spikePoint.y,textCenter.x,textCenter.y,textRight);crossPoint.y=crossPoint.y<textTop+opts.spikeGirth/2+opts.cornerRadius?textTop+opts.spikeGirth/2+opts.cornerRadius:crossPoint.y;crossPoint.y=crossPoint.y>(textBottom-opts.spikeGirth/2)-opts.cornerRadius?(textBottom-opts.spikeGirth/2)-opts.cornerRadius:crossPoint.y;points[points.length]={x:textRight,y:crossPoint.y+opts.spikeGirth/2,type:"join"};points[points.length]={x:textRight,y:textBottom,type:"corner"};points[points.length]={x:textLeft,y:textBottom,type:"corner"};points[points.length]={x:textLeft,y:textTop,type:"corner"};points[points.length]={x:textRight,y:textTop,type:"corner"};points[points.length]={x:textRight,y:crossPoint.y-opts.spikeGirth/2,type:"join"};points[points.length]=spikePoint;break;case"bottom":$text.css("margin-top",opts.spikeLength+"px");$box.css({top:(top+height)-opts.overlap,left:horiz});textRightSpace=(winRight-opts.windowMargin)-($text.offset().left+$text.btOuterWidth(true));var xShift=shadowShiftX;if(textRightSpace<0){$box.css("left",(numb($box.css("left"))+textRightSpace)+"px");xShift-=textRightSpace;}textLeftSpace=($text.offset().left+numb($text.css("margin-left")))-(scrollLeft+opts.windowMargin);if(textLeftSpace<0){$box.css("left",(numb($box.css("left"))-textLeftSpace)+"px");xShift+=textLeftSpace;}textTop=$text.btPosition().top+numb($text.css("margin-top"));textLeft=$text.btPosition().left+numb($text.css("margin-left"));textRight=textLeft+$text.btOuterWidth();textBottom=textTop+$text.outerHeight();textCenter={x:textLeft+($text.btOuterWidth()*opts.centerPointX),y:textTop+($text.outerHeight()*opts.centerPointY)};points[points.length]=spikePoint={x:((textRight-textLeft)*0.5)+xShift,y:shadowShiftY,type:"spike"};crossPoint=findIntersectX(spikePoint.x,spikePoint.y,textCenter.x,textCenter.y,textTop);crossPoint.x=crossPoint.x<textLeft+opts.spikeGirth/2+opts.cornerRadius?textLeft+opts.spikeGirth/2+opts.cornerRadius:crossPoint.x;crossPoint.x=crossPoint.x>(textRight-opts.spikeGirth/2)-opts.cornerRadius?(textRight-opts.spikeGirth/2)-opts.cornerRadius:crossPoint.x;points[points.length]={x:crossPoint.x+opts.spikeGirth/2,y:textTop,type:"join"};points[points.length]={x:textRight,y:textTop,type:"corner"};points[points.length]={x:textRight,y:textBottom,type:"corner"};points[points.length]={x:textLeft,y:textBottom,type:"corner"};points[points.length]={x:textLeft,y:textTop,type:"corner"};points[points.length]={x:crossPoint.x-(opts.spikeGirth/2),y:textTop,type:"join"};points[points.length]=spikePoint;break;case"right":$text.css("margin-left",(opts.spikeLength+"px"));$box.css({top:vert+"px",left:((left+width)-opts.overlap)+"px"});textBottomSpace=(winBottom-opts.windowMargin)-($text.offset().top+$text.outerHeight(true));var yShift=shadowShiftY;if(textBottomSpace<0){$box.css("top",(numb($box.css("top"))+textBottomSpace)+"px");yShift-=textBottomSpace;}textTopSpace=($text.offset().top+numb($text.css("margin-top")))-(scrollTop+opts.windowMargin);if(textTopSpace<0){$box.css("top",(numb($box.css("top"))-textTopSpace)+"px");yShift+=textTopSpace;}textTop=$text.btPosition().top+numb($text.css("margin-top"));textLeft=$text.btPosition().left+numb($text.css("margin-left"));textRight=textLeft+$text.btOuterWidth();textBottom=textTop+$text.outerHeight();textCenter={x:textLeft+($text.btOuterWidth()*opts.centerPointX),y:textTop+($text.outerHeight()*opts.centerPointY)};points[points.length]=spikePoint={x:shadowShiftX,y:((textBottom-textTop)*0.5)+yShift,type:"spike"};crossPoint=findIntersectY(spikePoint.x,spikePoint.y,textCenter.x,textCenter.y,textLeft);crossPoint.y=crossPoint.y<textTop+opts.spikeGirth/2+opts.cornerRadius?textTop+opts.spikeGirth/2+opts.cornerRadius:crossPoint.y;crossPoint.y=crossPoint.y>(textBottom-opts.spikeGirth/2)-opts.cornerRadius?(textBottom-opts.spikeGirth/2)-opts.cornerRadius:crossPoint.y;points[points.length]={x:textLeft,y:crossPoint.y-opts.spikeGirth/2,type:"join"};points[points.length]={x:textLeft,y:textTop,type:"corner"};points[points.length]={x:textRight,y:textTop,type:"corner"};points[points.length]={x:textRight,y:textBottom,type:"corner"};points[points.length]={x:textLeft,y:textBottom,type:"corner"};points[points.length]={x:textLeft,y:crossPoint.y+opts.spikeGirth/2,type:"join"};points[points.length]=spikePoint;break;}var canvas=document.createElement("canvas");$(canvas).attr("width",(numb($text.btOuterWidth(true))+opts.strokeWidth*2+shadowMarginX)).attr("height",(numb($text.outerHeight(true))+opts.strokeWidth*2+shadowMarginY)).appendTo($box).css({position:"absolute",zIndex:opts.boxzIndex});if(typeof G_vmlCanvasManager!="undefined"){canvas=G_vmlCanvasManager.initElement(canvas);}if(opts.cornerRadius>0){var newPoints=new Array();var newPoint;for(var i=0;i<points.length;i++){if(points[i].type=="corner"){newPoint=betweenPoint(points[i],points[(i-1)%points.length],opts.cornerRadius);newPoint.type="arcStart";newPoints[newPoints.length]=newPoint;newPoints[newPoints.length]=points[i];newPoint=betweenPoint(points[i],points[(i+1)%points.length],opts.cornerRadius);newPoint.type="arcEnd";newPoints[newPoints.length]=newPoint;}else{newPoints[newPoints.length]=points[i];}}points=newPoints;}var ctx=canvas.getContext("2d");if(opts.shadow&&opts.shadowOverlap!==true){var shadowOverlap=numb(opts.shadowOverlap);switch(position){case"top":if(opts.shadowOffsetX+opts.shadowBlur-shadowOverlap>0){$box.css("top",(numb($box.css("top"))-(opts.shadowOffsetX+opts.shadowBlur-shadowOverlap)));}break;case"right":if(shadowShiftX-shadowOverlap>0){$box.css("left",(numb($box.css("left"))+shadowShiftX-shadowOverlap));}break;case"bottom":if(shadowShiftY-shadowOverlap>0){$box.css("top",(numb($box.css("top"))+shadowShiftY-shadowOverlap));}break;case"left":if(opts.shadowOffsetY+opts.shadowBlur-shadowOverlap>0){$box.css("left",(numb($box.css("left"))-(opts.shadowOffsetY+opts.shadowBlur-shadowOverlap)));}break;}}drawIt.apply(ctx,[points],opts.strokeWidth);ctx.fillStyle=opts.fill;if(opts.shadow){ctx.shadowOffsetX=opts.shadowOffsetX;ctx.shadowOffsetY=opts.shadowOffsetY;ctx.shadowBlur=opts.shadowBlur;ctx.shadowColor=opts.shadowColor;}ctx.closePath();ctx.fill();if(opts.strokeWidth>0){ctx.shadowColor="rgba(0, 0, 0, 0)";ctx.lineWidth=opts.strokeWidth;ctx.strokeStyle=opts.strokeStyle;ctx.beginPath();drawIt.apply(ctx,[points],opts.strokeWidth);ctx.closePath();ctx.stroke();}opts.preShow.apply(this,[$box[0]]);$box.css({display:"none",visibility:"visible"});opts.showTip.apply(this,[$box[0]]);if(opts.overlay){var overlay=$('<div class="bt-overlay"></div>').css({position:"absolute",backgroundColor:"blue",top:top,left:left,width:width,height:height,opacity:".2"}).appendTo(offsetParent);$(this).data("overlay",overlay);}if((opts.ajaxPath!=null&&opts.ajaxCache==false)||ajaxTimeout){content=false;}if(opts.clickAnywhereToClose){jQuery.bt.vars.clickAnywhereStack.push(this);$(document).click(jQuery.bt.docClick);}if(opts.closeWhenOthersOpen){jQuery.bt.vars.closeWhenOpenStack.push(this);}opts.postShow.apply(this,[$box[0]]);};this.btOff=function(){var box=$(this).data("bt-box");opts.preHide.apply(this,[box]);var i=this;i.btCleanup=function(){var box=$(i).data("bt-box");var contentOrig=$(i).data("bt-content-orig");var overlay=$(i).data("bt-overlay");if(typeof box=="object"){$(box).remove();$(i).removeData("bt-box");}if(typeof contentOrig=="object"){var clones=$(contentOrig.original).data("bt-clones");$(contentOrig).data("bt-clones",arrayRemove(clones,contentOrig.clone));}if(typeof overlay=="object"){$(overlay).remove();$(i).removeData("bt-overlay");}jQuery.bt.vars.clickAnywhereStack=arrayRemove(jQuery.bt.vars.clickAnywhereStack,i);jQuery.bt.vars.closeWhenOpenStack=arrayRemove(jQuery.bt.vars.closeWhenOpenStack,i);$(i).removeClass("bt-active "+opts.activeClass);opts.postHide.apply(i);};opts.hideTip.apply(this,[box,i.btCleanup]);};var refresh=this.btRefresh=function(){this.btOff();this.btOn();};});function drawIt(points,strokeWidth){this.moveTo(points[0].x,points[0].y);for(i=1;i<points.length;i++){if(points[i-1].type=="arcStart"){this.quadraticCurveTo(round5(points[i].x,strokeWidth),round5(points[i].y,strokeWidth),round5(points[(i+1)%points.length].x,strokeWidth),round5(points[(i+1)%points.length].y,strokeWidth));i++;}else{this.lineTo(round5(points[i].x,strokeWidth),round5(points[i].y,strokeWidth));}}}function round5(num,strokeWidth){var ret;strokeWidth=numb(strokeWidth);if(strokeWidth%2){ret=num;}else{ret=Math.round(num-0.5)+0.5;}return ret;}function numb(num){return parseInt(num)||0;}function arrayRemove(arr,elem){var x,newArr=new Array();for(x in arr){if(arr[x]!=elem){newArr.push(arr[x]);}}return newArr;}function canvasSupport(){var canvas_compatible=false;try{canvas_compatible=!!(document.createElement("canvas").getContext("2d"));}catch(e){canvas_compatible=!!(document.createElement("canvas").getContext);}return canvas_compatible;}function shadowSupport(){try{var userAgent=navigator.userAgent.toLowerCase();if(/webkit/.test(userAgent)){return true;}else{if(/gecko|mozilla/.test(userAgent)&&parseFloat(userAgent.match(/firefox\/(\d+(?:\.\d+)+)/)[1])>=3.1){return true;}}}catch(err){}return false;}function betweenPoint(point1,point2,dist){var y,x;if(point1.x==point2.x){y=point1.y<point2.y?point1.y+dist:point1.y-dist;return{x:point1.x,y:y};}else{if(point1.y==point2.y){x=point1.x<point2.x?point1.x+dist:point1.x-dist;return{x:x,y:point1.y};}}}function centerPoint(arcStart,corner,arcEnd){var x=corner.x==arcStart.x?arcEnd.x:arcStart.x;var y=corner.y==arcStart.y?arcEnd.y:arcStart.y;var startAngle,endAngle;if(arcStart.x<arcEnd.x){if(arcStart.y>arcEnd.y){startAngle=(Math.PI/180)*180;endAngle=(Math.PI/180)*90;}else{startAngle=(Math.PI/180)*90;endAngle=0;}}else{if(arcStart.y>arcEnd.y){startAngle=(Math.PI/180)*270;endAngle=(Math.PI/180)*180;}else{startAngle=0;endAngle=(Math.PI/180)*270;}}return{x:x,y:y,type:"center",startAngle:startAngle,endAngle:endAngle};}function findIntersect(r1x1,r1y1,r1x2,r1y2,r2x1,r2y1,r2x2,r2y2){if(r2x1==r2x2){return findIntersectY(r1x1,r1y1,r1x2,r1y2,r2x1);}if(r2y1==r2y2){return findIntersectX(r1x1,r1y1,r1x2,r1y2,r2y1);}var r1m=(r1y1-r1y2)/(r1x1-r1x2);var r1b=r1y1-(r1m*r1x1);var r2m=(r2y1-r2y2)/(r2x1-r2x2);var r2b=r2y1-(r2m*r2x1);var x=(r2b-r1b)/(r1m-r2m);var y=r1m*x+r1b;return{x:x,y:y};}function findIntersectY(r1x1,r1y1,r1x2,r1y2,x){if(r1y1==r1y2){return{x:x,y:r1y1};}var r1m=(r1y1-r1y2)/(r1x1-r1x2);var r1b=r1y1-(r1m*r1x1);var y=r1m*x+r1b;return{x:x,y:y};}function findIntersectX(r1x1,r1y1,r1x2,r1y2,y){if(r1x1==r1x2){return{x:r1x1,y:y};}var r1m=(r1y1-r1y2)/(r1x1-r1x2);var r1b=r1y1-(r1m*r1x1);var x=(y-r1b)/r1m;return{x:x,y:y};}};jQuery.fn.btPosition=function(){function num(elem,prop){return elem[0]&&parseInt(jQuery.curCSS(elem[0],prop,true),10)||0;}var left=0,top=0,results;if(this[0]){var offsetParent=this.offsetParent(),offset=this.offset(),parentOffset=/^body|html$/i.test(offsetParent[0].tagName)?{top:0,left:0}:offsetParent.offset();offset.top-=num(this,"marginTop");offset.left-=num(this,"marginLeft");parentOffset.top+=num(offsetParent,"borderTopWidth");parentOffset.left+=num(offsetParent,"borderLeftWidth");results={top:offset.top-parentOffset.top,left:offset.left-parentOffset.left};}return results;};jQuery.fn.btOuterWidth=function(margin){function num(elem,prop){return elem[0]&&parseInt(jQuery.curCSS(elem[0],prop,true),10)||0;}return this["innerWidth"]()+num(this,"borderLeftWidth")+num(this,"borderRightWidth")+(margin?num(this,"marginLeft")+num(this,"marginRight"):0);};jQuery.fn.btOn=function(){return this.each(function(index){if(jQuery.isFunction(this.btOn)){this.btOn();}});};jQuery.fn.btOff=function(){return this.each(function(index){if(jQuery.isFunction(this.btOff)){this.btOff();}});};jQuery.bt.vars={clickAnywhereStack:[],closeWhenOpenStack:[]};jQuery.bt.docClick=function(e){if(!e){var e=window.event;}if(!$(e.target).parents().andSelf().filter(".bt-wrapper, .bt-active").length&&jQuery.bt.vars.clickAnywhereStack.length){$(jQuery.bt.vars.clickAnywhereStack).btOff();$(document).unbind("click",jQuery.bt.docClick);}};jQuery.bt.defaults={trigger:"hover",clickAnywhereToClose:true,closeWhenOthersOpen:false,shrinkToFit:false,width:"200px",padding:"10px",spikeGirth:10,spikeLength:15,overlap:0,overlay:false,killTitle:true,textzIndex:9999,boxzIndex:9998,wrapperzIndex:9997,offsetParent:null,positions:["most"],fill:"rgb(255, 255, 102)",windowMargin:10,strokeWidth:1,strokeStyle:"#000",cornerRadius:5,centerPointX:0.5,centerPointY:0.5,shadow:false,shadowOffsetX:2,shadowOffsetY:2,shadowBlur:3,shadowColor:"#000",shadowOverlap:false,noShadowOpts:{strokeStyle:"#999"},cssClass:"",cssStyles:{},activeClass:"bt-active",contentSelector:"$(this).attr('title')",ajaxPath:null,ajaxError:"<strong>ERROR:</strong> <em>%error</em>",ajaxLoading:"<blink>Loading...</blink>",ajaxData:{},ajaxType:"GET",ajaxCache:true,ajaxOpts:{},preBuild:function(){},preShow:function(box){},showTip:function(box){$(box).show();},postShow:function(box){},preHide:function(box){},hideTip:function(box,callback){$(box).hide();callback();},postHide:function(){},hoverIntentOpts:{interval:300,timeout:500}};jQuery.bt.options={};})(jQuery);

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

jQuery.extend( jQuery.easing,
{
	def: 'easeOutQuad',
	swing: function (x, t, b, c, d) {
		//alert(jQuery.easing.default);
		return jQuery.easing[jQuery.easing.def](x, t, b, c, d);
	},
	easeInQuad: function (x, t, b, c, d) {
		return c*(t/=d)*t + b;
	},
	easeOutQuad: function (x, t, b, c, d) {
		return -c *(t/=d)*(t-2) + b;
	},
	easeInOutQuad: function (x, t, b, c, d) {
		if ((t/=d/2) < 1) return c/2*t*t + b;
		return -c/2 * ((--t)*(t-2) - 1) + b;
	},
	easeInCubic: function (x, t, b, c, d) {
		return c*(t/=d)*t*t + b;
	},
	easeOutCubic: function (x, t, b, c, d) {
		return c*((t=t/d-1)*t*t + 1) + b;
	},
	easeInOutCubic: function (x, t, b, c, d) {
		if ((t/=d/2) < 1) return c/2*t*t*t + b;
		return c/2*((t-=2)*t*t + 2) + b;
	},
	easeInQuart: function (x, t, b, c, d) {
		return c*(t/=d)*t*t*t + b;
	},
	easeOutQuart: function (x, t, b, c, d) {
		return -c * ((t=t/d-1)*t*t*t - 1) + b;
	},
	easeInOutQuart: function (x, t, b, c, d) {
		if ((t/=d/2) < 1) return c/2*t*t*t*t + b;
		return -c/2 * ((t-=2)*t*t*t - 2) + b;
	},
	easeInQuint: function (x, t, b, c, d) {
		return c*(t/=d)*t*t*t*t + b;
	},
	easeOutQuint: function (x, t, b, c, d) {
		return c*((t=t/d-1)*t*t*t*t + 1) + b;
	},
	easeInOutQuint: function (x, t, b, c, d) {
		if ((t/=d/2) < 1) return c/2*t*t*t*t*t + b;
		return c/2*((t-=2)*t*t*t*t + 2) + b;
	},
	easeInSine: function (x, t, b, c, d) {
		return -c * Math.cos(t/d * (Math.PI/2)) + c + b;
	},
	easeOutSine: function (x, t, b, c, d) {
		return c * Math.sin(t/d * (Math.PI/2)) + b;
	},
	easeInOutSine: function (x, t, b, c, d) {
		return -c/2 * (Math.cos(Math.PI*t/d) - 1) + b;
	},
	easeInExpo: function (x, t, b, c, d) {
		return (t==0) ? b : c * Math.pow(2, 10 * (t/d - 1)) + b;
	},
	easeOutExpo: function (x, t, b, c, d) {
		return (t==d) ? b+c : c * (-Math.pow(2, -10 * t/d) + 1) + b;
	},
	easeInOutExpo: function (x, t, b, c, d) {
		if (t==0) return b;
		if (t==d) return b+c;
		if ((t/=d/2) < 1) return c/2 * Math.pow(2, 10 * (t - 1)) + b;
		return c/2 * (-Math.pow(2, -10 * --t) + 2) + b;
	},
	easeInCirc: function (x, t, b, c, d) {
		return -c * (Math.sqrt(1 - (t/=d)*t) - 1) + b;
	},
	easeOutCirc: function (x, t, b, c, d) {
		return c * Math.sqrt(1 - (t=t/d-1)*t) + b;
	},
	easeInOutCirc: function (x, t, b, c, d) {
		if ((t/=d/2) < 1) return -c/2 * (Math.sqrt(1 - t*t) - 1) + b;
		return c/2 * (Math.sqrt(1 - (t-=2)*t) + 1) + b;
	},
	easeInElastic: function (x, t, b, c, d) {
		var s=1.70158;var p=0;var a=c;
		if (t==0) return b;  if ((t/=d)==1) return b+c;  if (!p) p=d*.3;
		if (a < Math.abs(c)) { a=c; var s=p/4; }
		else var s = p/(2*Math.PI) * Math.asin (c/a);
		return -(a*Math.pow(2,10*(t-=1)) * Math.sin( (t*d-s)*(2*Math.PI)/p )) + b;
	},
	easeOutElastic: function (x, t, b, c, d) {
		var s=1.70158;var p=0;var a=c;
		if (t==0) return b;  if ((t/=d)==1) return b+c;  if (!p) p=d*.3;
		if (a < Math.abs(c)) { a=c; var s=p/4; }
		else var s = p/(2*Math.PI) * Math.asin (c/a);
		return a*Math.pow(2,-10*t) * Math.sin( (t*d-s)*(2*Math.PI)/p ) + c + b;
	},
	easeInOutElastic: function (x, t, b, c, d) {
		var s=1.70158;var p=0;var a=c;
		if (t==0) return b;  if ((t/=d/2)==2) return b+c;  if (!p) p=d*(.3*1.5);
		if (a < Math.abs(c)) { a=c; var s=p/4; }
		else var s = p/(2*Math.PI) * Math.asin (c/a);
		if (t < 1) return -.5*(a*Math.pow(2,10*(t-=1)) * Math.sin( (t*d-s)*(2*Math.PI)/p )) + b;
		return a*Math.pow(2,-10*(t-=1)) * Math.sin( (t*d-s)*(2*Math.PI)/p )*.5 + c + b;
	},
	easeInBack: function (x, t, b, c, d, s) {
		if (s == undefined) s = 1.70158;
		return c*(t/=d)*t*((s+1)*t - s) + b;
	},
	easeOutBack: function (x, t, b, c, d, s) {
		if (s == undefined) s = 1.70158;
		return c*((t=t/d-1)*t*((s+1)*t + s) + 1) + b;
	},
	easeInOutBack: function (x, t, b, c, d, s) {
		if (s == undefined) s = 1.70158; 
		if ((t/=d/2) < 1) return c/2*(t*t*(((s*=(1.525))+1)*t - s)) + b;
		return c/2*((t-=2)*t*(((s*=(1.525))+1)*t + s) + 2) + b;
	},
	easeInBounce: function (x, t, b, c, d) {
		return c - jQuery.easing.easeOutBounce (x, d-t, 0, c, d) + b;
	},
	easeOutBounce: function (x, t, b, c, d) {
		if ((t/=d) < (1/2.75)) {
			return c*(7.5625*t*t) + b;
		} else if (t < (2/2.75)) {
			return c*(7.5625*(t-=(1.5/2.75))*t + .75) + b;
		} else if (t < (2.5/2.75)) {
			return c*(7.5625*(t-=(2.25/2.75))*t + .9375) + b;
		} else {
			return c*(7.5625*(t-=(2.625/2.75))*t + .984375) + b;
		}
	},
	easeInOutBounce: function (x, t, b, c, d) {
		if (t < d/2) return jQuery.easing.easeInBounce (x, t*2, 0, c, d) * .5 + b;
		return jQuery.easing.easeOutBounce (x, t*2-d, 0, c, d) * .5 + c*.5 + b;
	}
});

/* Copyright (c) 2010 Brandon Aaron (http://brandonaaron.net)
* Licensed under the MIT License (LICENSE.txt).
*
* Version 2.1.2
*/
(function (a) { a.fn.bgiframe = (a.browser.msie && /msie 6\.0/i.test(navigator.userAgent) ? function (d) { d = a.extend({ top: "auto", left: "auto", width: "auto", height: "auto", opacity: true, src: "javascript:false;" }, d); var c = '<iframe class="bgiframe"frameborder="0"tabindex="-1"src="' + d.src + '"style="display:block;position:absolute;z-index:-1;' + (d.opacity !== false ? "filter:Alpha(Opacity='0');" : "") + "top:" + (d.top == "auto" ? "expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+'px')" : b(d.top)) + ";left:" + (d.left == "auto" ? "expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+'px')" : b(d.left)) + ";width:" + (d.width == "auto" ? "expression(this.parentNode.offsetWidth+'px')" : b(d.width)) + ";height:" + (d.height == "auto" ? "expression(this.parentNode.offsetHeight+'px')" : b(d.height)) + ';"/>'; return this.each(function () { if (a(this).children("iframe.bgiframe").length === 0) { this.insertBefore(document.createElement(c), this.firstChild) } }) } : function () { return this }); a.fn.bgIframe = a.fn.bgiframe; function b(c) { return c && c.constructor === Number ? c + "px" : c } })(jQuery);

// ColorBox v1.3.12 - a full featured, light-weight, customizable lightbox based on jQuery 1.3+
// Copyright (c) 2010 Jack Moore - jack@colorpowered.com
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
(function(b,kb){var u="none",N="LoadedContent",c=false,w="resize.",o="y",q="auto",f=true,M="nofollow",t="on",l="x";function e(a,c){a=a?' id="'+j+a+'"':"";c=c?' style="'+c+'"':"";return b("<div"+a+c+"/>")}function p(a,b){b=b===l?m.width():m.height();return typeof a==="string"?Math.round(/%/.test(a)?b/100*parseInt(a,10):parseInt(a,10)):a}function U(b){return a.photo||/\.(gif|png|jpg|jpeg|bmp)(?:\?([^#]*))?(?:#(\.*))?$/i.test(b)}function db(a){for(var c in a)if(b.isFunction(a[c])&&c.substring(0,2)!==t)a[c]=a[c].call(n);a.rel=a.rel||n.rel||M;a.href=a.href||b(n).attr("href");a.title=a.title||n.title;return a}function x(c,a){a&&a.call(n);b.event.trigger(c)}function lb(){var c,b=j+"Slideshow_",e="click."+j,f,k;if(a.slideshow&&h[1]){f=function(){F.text(a.slideshowStop).unbind(e).bind(V,function(){if(g<h.length-1||a.loop)c=setTimeout(d.next,a.slideshowSpeed)}).bind(W,function(){clearTimeout(c)}).one(e,k);i.removeClass(b+"off").addClass(b+t);c=setTimeout(d.next,a.slideshowSpeed)};k=function(){clearTimeout(c);F.text(a.slideshowStart).unbind(V+" "+W+" "+e).one(e,f);i.removeClass(b+t).addClass(b+"off")};F.bind(eb,function(){clearTimeout(c)});i.hasClass(b+t)||a.slideshowAuto&&!i.hasClass(b+"off")?f():k()}}function fb(c){if(!O){n=c;a=db(b.extend({},b.data(n,r)));h=b(n);g=0;if(a.rel!==M){h=b("."+H).filter(function(){return (b.data(this,r).rel||this.rel)===a.rel});g=h.index(n);if(g===-1){h=h.add(n);g=h.length-1}}if(!v){v=G=f;i.show();X=n;try{X.blur()}catch(e){}y.css({opacity:+a.opacity,cursor:a.overlayClose?"pointer":q}).show();a.w=p(a.initialWidth,l);a.h=p(a.initialHeight,o);d.position(0);Y&&m.bind(w+P+" scroll."+P,function(){y.css({width:m.width(),height:m.height(),top:m.scrollTop(),left:m.scrollLeft()})}).trigger("scroll."+P);x(gb,a.onOpen);Z.add(I).add(J).add(F).add(ab).hide();bb.html(a.close).show()}d.load(f)}}var hb={transition:"elastic",speed:300,width:c,initialWidth:"600",innerWidth:c,maxWidth:c,height:c,initialHeight:"450",innerHeight:c,maxHeight:c,scalePhotos:f,scrolling:f,inline:c,html:c,iframe:c,photo:c,href:c,title:c,rel:c,opacity:.9,preloading:f,current:"image {current} of {total}",previous:"previous",next:"next",close:"close",open:c,loop:f,slideshow:c,slideshowAuto:f,slideshowSpeed:2500,slideshowStart:"start slideshow",slideshowStop:"stop slideshow",onOpen:c,onLoad:c,onComplete:c,onCleanup:c,onClosed:c,overlayClose:f,escKey:f,arrowKey:f},r="colorbox",j="cbox",gb=j+"_open",W=j+"_load",V=j+"_complete",ib=j+"_cleanup",eb=j+"_closed",Q=j+"_purge",jb=j+"_loaded",B=b.browser.msie&&!b.support.opacity,Y=B&&b.browser.version<7,P=j+"_IE6",y,i,C,s,cb,T,R,S,h,m,k,K,L,ab,Z,F,J,I,bb,D,E,z,A,n,X,g,a,v,G,O=c,d,H=j+"Element";d=b.fn[r]=b[r]=function(c,e){var a=this,d;if(!a[0]&&a.selector)return a;c=c||{};if(e)c.onComplete=e;if(!a[0]||a.selector===undefined){a=b("<a/>");c.open=f}a.each(function(){b.data(this,r,b.extend({},b.data(this,r)||hb,c));b(this).addClass(H)});d=c.open;if(b.isFunction(d))d=d.call(a);d&&fb(a[0]);return a};d.init=function(){var l="hover",n="clear:left";m=b(kb);i=e().attr({id:r,"class":B?j+"IE":""});y=e("Overlay",Y?"position:absolute":"").hide();C=e("Wrapper");s=e("Content").append(k=e(N,"width:0; height:0; overflow:hidden"),L=e("LoadingOverlay").add(e("LoadingGraphic")),ab=e("Title"),Z=e("Current"),J=e("Next"),I=e("Previous"),F=e("Slideshow").bind(gb,lb),bb=e("Close"));C.append(e().append(e("TopLeft"),cb=e("TopCenter"),e("TopRight")),e(c,n).append(T=e("MiddleLeft"),s,R=e("MiddleRight")),e(c,n).append(e("BottomLeft"),S=e("BottomCenter"),e("BottomRight"))).children().children().css({"float":"left"});K=e(c,"position:absolute; width:9999px; visibility:hidden; display:none");b("body").prepend(y,i.append(C,K));s.children().hover(function(){b(this).addClass(l)},function(){b(this).removeClass(l)}).addClass(l);D=cb.height()+S.height()+s.outerHeight(f)-s.height();E=T.width()+R.width()+s.outerWidth(f)-s.width();z=k.outerHeight(f);A=k.outerWidth(f);i.css({"padding-bottom":D,"padding-right":E}).hide();J.click(d.next);I.click(d.prev);bb.click(d.close);s.children().removeClass(l);b("."+H).live("click",function(a){if(!(a.button!==0&&typeof a.button!=="undefined"||a.ctrlKey||a.shiftKey||a.altKey)){a.preventDefault();fb(this)}});y.click(function(){a.overlayClose&&d.close()});b(document).bind("keydown",function(b){if(v&&a.escKey&&b.keyCode===27){b.preventDefault();d.close()}if(v&&a.arrowKey&&!G&&h[1])if(b.keyCode===37&&(g||a.loop)){b.preventDefault();I.click()}else if(b.keyCode===39&&(g<h.length-1||a.loop)){b.preventDefault();J.click()}})};d.remove=function(){i.add(y).remove();b("."+H).die("click").removeData(r).removeClass(H)};d.position=function(f,d){function b(a){cb[0].style.width=S[0].style.width=s[0].style.width=a.style.width;L[0].style.height=L[1].style.height=s[0].style.height=T[0].style.height=R[0].style.height=a.style.height}var e,h=Math.max(document.documentElement.clientHeight-a.h-z-D,0)/2+m.scrollTop(),g=Math.max(m.width()-a.w-A-E,0)/2+m.scrollLeft();e=i.width()===a.w+A&&i.height()===a.h+z?0:f;C[0].style.width=C[0].style.height="9999px";i.dequeue().animate({width:a.w+A,height:a.h+z,top:h,left:g},{duration:e,complete:function(){b(this);G=c;C[0].style.width=a.w+A+E+"px";C[0].style.height=a.h+z+D+"px";d&&d()},step:function(){b(this)}})};d.resize=function(b){if(v){b=b||{};if(b.width)a.w=p(b.width,l)-A-E;if(b.innerWidth)a.w=p(b.innerWidth,l);k.css({width:a.w});if(b.height)a.h=p(b.height,o)-z-D;if(b.innerHeight)a.h=p(b.innerHeight,o);if(!b.innerHeight&&!b.height){b=k.wrapInner("<div style='overflow:auto'></div>").children();a.h=b.height();b.replaceWith(b.children())}k.css({height:a.h});d.position(a.transition===u?0:a.speed)}};d.prep=function(o){var f="hidden";function n(t){var q,f,o,e,n=h.length,s=a.loop;d.position(t,function(){if(v){B&&p&&k.fadeIn(100);k.show();x(jb);ab.show().html(a.title);if(n>1){typeof a.current=="string"&&Z.html(a.current.replace(/\{current\}/,g+1).replace(/\{total\}/,n)).show();J[s||g<n-1?"show":"hide"]().html(a.next);I[s||g?"show":"hide"]().html(a.previous);q=g?h[g-1]:h[n-1];o=g<n-1?h[g+1]:h[0];a.slideshow&&F.show();if(a.preloading){e=b.data(o,r).href||o.href;f=b.data(q,r).href||q.href;e=b.isFunction(e)?e.call(o):e;f=b.isFunction(f)?f.call(q):f;if(U(e))b("<img/>")[0].src=e;if(U(f))b("<img/>")[0].src=f}}L.hide();if(a.transition==="fade")i.fadeTo(l,1,function(){if(B)i[0].style.filter=c});else if(B)i[0].style.filter=c;m.bind(w+j,function(){d.position(0)});x(V,a.onComplete)}})}if(v){var p,l=a.transition===u?0:a.speed;m.unbind(w+j);k.remove();k=e(N).html(o);k.hide().appendTo(K.show()).css({width:function(){a.w=a.w||k.width();a.w=a.mw&&a.mw<a.w?a.mw:a.w;return a.w}(),overflow:a.scrolling?q:f}).css({height:function(){a.h=a.h||k.height();a.h=a.mh&&a.mh<a.h?a.mh:a.h;return a.h}()}).prependTo(s);K.hide();b("#"+j+"Photo").css({cssFloat:u,marginLeft:q,marginRight:q});Y&&b("select").not(i.find("select")).filter(function(){return this.style.visibility!==f}).css({visibility:f}).one(ib,function(){this.style.visibility="inherit"});a.transition==="fade"?i.fadeTo(l,0,function(){n(0)}):n(l)}};d.load=function(t){var m,c,s,q=d.prep;G=f;n=h[g];t||(a=db(b.extend({},b.data(n,r))));x(Q);x(W,a.onLoad);a.h=a.height?p(a.height,o)-z-D:a.innerHeight&&p(a.innerHeight,o);a.w=a.width?p(a.width,l)-A-E:a.innerWidth&&p(a.innerWidth,l);a.mw=a.w;a.mh=a.h;if(a.maxWidth){a.mw=p(a.maxWidth,l)-A-E;a.mw=a.w&&a.w<a.mw?a.w:a.mw}if(a.maxHeight){a.mh=p(a.maxHeight,o)-z-D;a.mh=a.h&&a.h<a.mh?a.h:a.mh}m=a.href;L.show();if(a.inline){e().hide().insertBefore(b(m)[0]).one(Q,function(){b(this).replaceWith(k.children())});q(b(m))}else if(a.iframe){i.one(jb,function(){var c=b("<iframe name='"+(new Date).getTime()+"' frameborder=0"+(a.scrolling?"":" scrolling='no'")+(B?" allowtransparency='true'":"")+" style='width:100%; height:100%; border:0; display:block;'/>");c[0].src=a.href;c.appendTo(k).one(Q,function(){c[0].src="about:blank"})});q(" ")}else if(a.html)q(a.html);else if(U(m)){c=new Image;c.onload=function(){var e;c.onload=null;c.id=j+"Photo";b(c).css({border:u,display:"block",cssFloat:"left"});if(a.scalePhotos){s=function(){c.height-=c.height*e;c.width-=c.width*e};if(a.mw&&c.width>a.mw){e=(c.width-a.mw)/c.width;s()}if(a.mh&&c.height>a.mh){e=(c.height-a.mh)/c.height;s()}}if(a.h)c.style.marginTop=Math.max(a.h-c.height,0)/2+"px";h[1]&&(g<h.length-1||a.loop)&&b(c).css({cursor:"pointer"}).click(d.next);if(B)c.style.msInterpolationMode="bicubic";setTimeout(function(){q(c)},1)};setTimeout(function(){c.src=m},1)}else m&&K.load(m,function(d,c,a){q(c==="error"?"Request unsuccessful: "+a.statusText:b(this).children())})};d.next=function(){if(!G){g=g<h.length-1?g+1:0;d.load()}};d.prev=function(){if(!G){g=g?g-1:h.length-1;d.load()}};d.close=function(){if(v&&!O){O=f;v=c;x(ib,a.onCleanup);m.unbind("."+j+" ."+P);y.fadeTo("fast",0);i.stop().fadeTo("fast",0,function(){x(Q);k.remove();i.add(y).css({opacity:1,cursor:q}).hide();try{X.focus()}catch(b){}setTimeout(function(){O=c;x(eb,a.onClosed)},1)})}};d.element=function(){return b(n)};d.settings=hb;b(d.init)})(jQuery,this)

function initBT_Box(boxObj){	
	boxObj.find("#initWait").css({"display":"inline-block"});
	boxObj.find("#buyer_form").hide();
	
    boxObj.find("#closeBox").click(function(){ 
		boxObj.hide();
		if( !isBuyingReq && isPhotoReqDone){
			$("#requestPhotos").fadeOut(500);
		}
	});

    if( !isBuyingReq ){// photo request
		boxObj.find("#form_title").text("Request Photos");
		boxObj.find("#byline_text").text("Request the seller to upload photos of this bike");
		boxObj.find("#get_details").text("Request");
	}

	shownInterestInThisBike(boxObj);		
}

function shownInterestInThisBike(boxObj) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedBuyer,Bikewale.ashx",
        data: '{"inquiryId":"' + inquiryId + '", "isDealer":"' + _isDealer + '"}',
        dataType: 'json',
        beforeSend: function (xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "ShownInterestInThisBike"); },
        success: function (response) {            
            var ds = eval('(' + response.value + ')');
            if (ds.ShownInterest == "True") { // buyer already shown interest; Show seller information;                
                if (isBuyingReq) {
                    prepareSellInfo(boxObj, ds);
                } else {
                    submitPhotoRequest(boxObj);
                }
            } else { // first time showing interest
                boxObj.find("#txtName").val(ds.BuyerName); //Prefill buyer information
                boxObj.find("#txtEmail").val(ds.BuyerEmail);
                boxObj.find("#txtMobile").val(ds.BuyerMobile);

                boxObj.find("#initWait").hide();
                boxObj.find("#buyer_form").removeClass("hide").show();

                boxObj.find("#get_details").click(function () {// event binding for "Get Details" Button                  
                    buyersName = boxObj.find("#txtName").val();
                    buyersEmail = boxObj.find("#txtEmail").val();
                    buyersMobile = boxObj.find("#txtMobile").val();

                    if (validateForm()) { // validate form before submit
                        boxObj.find("#process_img").css({ "display": "inline-block" });
                        if (isBuyingReq) {
                            processPurchaseInq(boxObj);
                        } else {
                            submitPhotoRequest(boxObj);
                        }
                    }
                });
            }
        }
    });
}

function processPurchaseInq(boxObj) {
    $.ajax({
        type: "POST",
        url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedBuyer,Bikewale.ashx",
        data: '{"profileId":"' + profileId + '", "buyerName":"' + buyersName + '", "buyerEmail":"' + buyersEmail + '","buyerMobile":"' + buyersMobile + '","bikeModel":"' + bikeModel + '","makeYear":"' + makeYear + '","pageUrl":"' + location.href + '"}',
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
   
    if (_isDealer == "1") {
        boxObj.find("#contact_person").text(ds.SellerContactPerson);
    } else {
        boxObj.find("#contact_person").parent().parent().hide();
    }

    boxObj.find("#initWait,#buyer_form,#verifiy_mobile").hide();
    boxObj.find("#seller_details").removeClass("hide").fadeIn(500);
    boxObj.find("#processCode").hide();
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

function submitPhotoRequest(boxObj){
	var buyerMessage = "";		
	
	$.ajax({
		type: "POST",
		url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedBuyer,Bikewale.ashx",
		data:'{"sellInquiryId":"'+ inquiryId +'", "consumerType":"'+ consumerType +'", "buyerMessage":"'+ buyerMessage +'", "buyerName":"'+ buyersName +'", "buyerEmail":"'+ buyersEmail +'", "buyerMobile":"'+ buyersMobile +'", "bikeName":"'+ bikeName +'"}',			
		dataType: 'json',
		beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "UploadPhotosRequest"); },
		success: function(response) {
			boxObj.find("#buyer_form,#verifiy_mobile,#initWait").hide();
			boxObj.find("#photos_req_msg").removeClass("hide").fadeIn(500);
			if( response.value ){
				isPhotoReqDone = response.value;
			}else{
				boxObj.find("#photos_req_confirm").html("Error in submitting request to the seller. Please try again.");
			}
		}
	});
}

function validateForm(){	
	var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;		
	
	if(buyersName == "") {
		alert("Please enter your name");
		return false;
	}
	
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

function validateVerificationCode(code){
	if( code == "" ){
		alert("Please enter 5-digit verification code to proceed");
		return false;
	}else if( code != "" && ( !re.test(code) || code.length > 5 || code.length < 5 ) ){
		alert("Invalid code! Please re-enter valid code.");
		return false;
	}
	
	return true;
}

function loginCallBack() {
	bookmarkThisBike();
}
/***************************************************************************
	Ajax call to bookmark Used bike.
****************************************************************************/
function bookmarkThisBike() {
	$.ajax({
		type: "POST",
		url: "/ajaxpro/Bikewale.Ajax.AjaxClassifiedBuyer,Bikewale.ashx",
		data: '{"bikeProfileId":"'+ profileId +'"}',
		beforeSend: function(xhr) { xhr.setRequestHeader("X-AjaxPro-Method", "BookmarkThisBike"); },
		success: function(response){
			var val = eval('('+ response +')');				
			if(val.value){
				GB_hide();
				$("#liBookmark").addClass("bookmarked").text("This bike has been bookmarked");
			}
		}
	});
}