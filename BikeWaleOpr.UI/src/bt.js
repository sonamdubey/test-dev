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

 /* Copyright (c) 2006 Brandon Aaron (http://brandonaaron.net)
 * Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php) 
 * and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
 *
 * $LastChangedDate: 2007-07-21 18:45:56 -0500 (Sat, 21 Jul 2007) $
 * $Rev: 2447 $
 *
 * Version 2.1.1
 */
(function($){$.fn.bgIframe=$.fn.bgiframe=function(s){if($.browser.msie&&/6.0/.test(navigator.userAgent)){s=$.extend({top:'auto',left:'auto',width:'auto',height:'auto',opacity:true,src:'javascript:false;'},s||{});var prop=function(n){return n&&n.constructor==Number?n+'px':n;},html='<iframe class="bgiframe"frameborder="0"tabindex="-1"src="'+s.src+'"'+'style="display:block;position:absolute;z-index:-1;'+(s.opacity!==false?'filter:Alpha(Opacity=\'0\');':'')+'top:'+(s.top=='auto'?'expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+\'px\')':prop(s.top))+';'+'left:'+(s.left=='auto'?'expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+\'px\')':prop(s.left))+';'+'width:'+(s.width=='auto'?'expression(this.parentNode.offsetWidth+\'px\')':prop(s.width))+';'+'height:'+(s.height=='auto'?'expression(this.parentNode.offsetHeight+\'px\')':prop(s.height))+';'+'"/>';return this.each(function(){if($('> iframe.bgiframe',this).length==0)this.insertBefore(document.createElement(html),this.firstChild);});}return this;};})(jQuery);