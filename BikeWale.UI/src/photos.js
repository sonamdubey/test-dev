$(document).ready(function () {
    var photosLength = $('.model-grid-images li').length,
        photoLimitCount = 24;

    // add 'more photos count' if photo grid contains 24 images
    if (photosLength == photoLimitCount) {
        morePhotosOverlay(photoLimitCount);
    }
    else if ((photoCount - 1) % 8 == 1) { // remainder 1 image
        $('.model-grid-images li').eq(11).css('float', 'left');
        morePhotosOverlay(photosLength);
    }

});

function morePhotosOverlay(limitCount) {
    var lastPhoto = $('.model-grid-images li').last(),
        countOverlay = '<span class="black-overlay"><span class="font18 text-bold">+' + (photoCount - limitCount) + '</span><br /><span class="font16">images</span></span>';   
    lastPhoto.append(countOverlay);
};

(function () {
    $(function () {
        var gallerySwiper = $('.gallery-swiper');

        gallerySwiper.find('.jcarousel')
            .on('jcarousel:targetin', 'li', function () {
                $(this).addClass('target');
            })
            .on('jcarousel:targetout', 'li', function () {
                $(this).removeClass('target');
            })
            .jcarousel({
                center: true,

            });

        gallerySwiper.find('.jcarousel-control-prev').jcarouselControl({
            target: '-=1'
        });

        gallerySwiper.find('.jcarousel-control-next').jcarouselControl({
            target: '+=1'
        });

        gallerySwiper.find('.jcarousel li:eq(0)')
            .addClass('target');

        gallerySwiper.find('.jcarousel')
            .on('jcarousel:visiblein', 'li', function (event, carousel) {
                $(this).find("img.lazy").trigger("imgLazyLoad");
            });

        /*
        $('.gallery-swiper .jcarousel li:eq(1)')
            .addClass('next-sibling');
        */
    });
})();

/*
$('.gallery-swiper .jcarousel-control-prev').click(function () {
    var index = $('.gallery-swiper .jcarousel').jcarousel('target').index(),
        prevSiblingIndex = index - 2,
        activeIndex = index - 1,
        nextSiblingIndex = index;

    $('.gallery-swiper .jcarousel li:eq(' + prevSiblingIndex + ')').addClass('prev-sibling');
    $('.gallery-swiper .jcarousel li:eq(' + activeIndex + ')').removeClass('prev-sibling next-sibling');
    $('.gallery-swiper .jcarousel li:eq(' + nextSiblingIndex + ')').addClass('next-sibling');
});

$('.gallery-swiper .jcarousel-control-next').click(function () {
    var index = $('.gallery-swiper .jcarousel').jcarousel('target').index(),
        prevSiblingIndex = index,
        activeIndex = index + 1,
        nextSiblingIndex = index + 2;

    $('.gallery-swiper .jcarousel li:eq(' + prevSiblingIndex + ')').addClass('prev-sibling');
    $('.gallery-swiper .jcarousel li:eq(' + activeIndex + ')').removeClass('prev-sibling next-sibling');
    $('.gallery-swiper .jcarousel li:eq(' + nextSiblingIndex + ')').addClass('next-sibling');
});
*/

function applyLazyLoad() {
    $("img.lazy").lazyload({
        event: "imgLazyLoad",
        effect: "fadeIn"
    });
}

applyLazyLoad();

var modelGallery = function () {
    var self = this;

    self.photosTabActive = ko.observable(true);

    self.togglePhotoTab = function () {
        if (!self.photosTabActive()) {
            self.photosTabActive(true);
        }
        else {
            self.photosTabActive(false);
        }
    };
};

var vmModelGallery = new modelGallery();

ko.applyBindings(vmModelGallery, document.getElementById('gallery-root'));

/*
   ========================================================
        script: image-flow
        author: Gerard Ferrandez - [Ge1doot]
          date: March 15, 2008
           maj: May 24, 2008 - AJAXification bank images
          site: http://www.dhteumeuleu.com
       license: CC-BY-NC - do not remove this notice
   ========================================================
*/

var imf = function () {
    /* //////////// ==== private methods ==== //////////// */
    var lf = 0;
    var instances = [];
    
    /* ===== custom getElementsByClass ==== */
    function getElementsByClass(object, tag, className) {
        var o = object.getElementsByTagName(tag);
        for (var i = 0, n = o.length, ret = []; i < n; i++)
            if (o[i].className == className) ret.push(o[i]);
        if (ret.length == 1) ret = ret[0];
        return ret;
    }
    /* ===== crossbrowsers addEvent ==== */
    function addEvent(o, e, f) {
        if (window.addEventListener) o.addEventListener(e, f, false);
        else if (window.attachEvent) r = o.attachEvent('on' + e, f);
    }

    /* //////////// ==== ImageFlow Constructor ==== //////////// */
    function ImageFlow(oCont, xmlfile, horizon, size, zoom, border, start, interval) {
        this.diapos = [];
        this.scr = false;
        this.size = size;
        this.zoom = zoom;
        this.horizon = horizon;
        this.bdw = border;
        this.oCont = oCont;
        this.oc = document.getElementById(oCont);
        this.scrollbar = getElementsByClass(this.oc, 'div', 'scrollbar');
        this.text = getElementsByClass(this.oc, 'div', 'text');
        this.title = getElementsByClass(this.text, 'div', 'title');
        this.legend = getElementsByClass(this.text, 'div', 'legend');
        this.bar = getElementsByClass(this.oc, 'img', 'bar');
        this.arL = getElementsByClass(this.oc, 'img', 'arrow-left');
        this.arR = getElementsByClass(this.oc, 'img', 'arrow-right');
        this.bw = this.bar.width;
        this.alw = this.arL.width - 5;
        this.arw = this.arR.width - 5;
        this.bar.parent = this.oc.parent = this;
        this.arL.parent = this.arR.parent = this;
        this.view = this.back = -1;
        this.time_start = start * 62.5 || 0;
        this.time_inter = interval * 62.5 || 0;
        this.time_out = this.time_start;
        this.time = 0;
        this.time_dir = 1;
        this.resize();
        this.oc.onselectstart = function () { return false; }
        /* ---- parse xml ---- */
        //var bank = AJAX(xmlfile).responseXML.getElementsByTagName("bank").item(0).getElementsByTagName("img");
        var bank = xmlfile.bank.img;
        this.NF = bank.length;
        for (var i = 0; i < this.NF; i++) {
            var img = bank[i];
            var val = function (tag) {
                return img.src;
            }
            /* ---- create images ---- */
            this.diapos[i] = new Diapo(this, i, val("src"), val("title"), val("caption"), val("link"), val("target") || '_self');
        }
        /* ==== add mouse wheel events ==== */
        if (window.addEventListener)
            this.oc.addEventListener('DOMMouseScroll', function (e) {
                this.parent.scroll(-e.detail);
                return false;
            }, false);
        this.oc.onmousewheel = function () {
            this.parent.scroll(event.wheelDelta);
            return false;
        }
        /* ==== scrollbar drag N drop ==== */
        this.bar.onmousedown = function (e) {
            if (!e) e = window.event;
            var scl = e.screenX - this.offsetLeft;
            var self = this.parent;
            /* ---- move bar ---- */
            this.parent.oc.onmousemove = function (e) {
                if (!e) e = window.event;
                self.bar.style.left = Math.round(Math.min((self.ws - self.arw - self.bw), Math.max(self.alw, e.screenX - scl))) + 'px';
                self.view = Math.round(((e.screenX - scl)) / (self.ws - self.alw - self.arw - self.bw) * (self.NF - 1));
                if (self.view != self.back) self.calc();
                return false;
            }
            /* ---- release scrollbar ---- */
            this.parent.oc.onmouseup = function (e) {
                self.oc.onmousemove = null;
                return false;
            }
            return false;
        }
        /* ==== right arrow ==== */
        this.arR.onclick = function () {
            if (this.parent.view < this.parent.NF - 1)
                this.parent.calc(1);
        }
        /* ==== Left arrow ==== */
        this.arL.onclick = function () {
            if (this.parent.view > 0) {
                this.parent.calc(-1);
                
            }
        }
    }
    /* //////////// ==== ImageFlow prototype ==== //////////// */
    ImageFlow.prototype = {
        /* ==== targets ==== */
        calc: function (inc) {
            if (inc) {
                this.view += inc;
                /* ---- stop autoscroll ---- */
                this.time = 0;
                this.time_out = this.time_start;
            }
            var tw = 0;
            var lw = 0;
            var o = this.diapos[this.view];
            if (o && o.loaded) {
                /* ---- reset ---- */
                var ob = this.diapos[this.back];
                if (ob && ob != o) {
                    ob.img.className = 'diapo';
                    ob.z1 = 1;
                }
                /* ---- update legend ---- */
                this.title.innerHTML = o.title;
                this.legend.innerHTML = o.text;
                /* ---- update hyperlink ---- */
                if (o.url) {
                    o.img.className = 'diapo link target-image';
                    window.status = 'hyperlink: ' + o.url;
                } else {
                    o.img.className = 'diapo';
                    window.status = '';
                }
                /* ---- calculate target sizes & positions ---- */
                if (o.r < 1) o.w1 = Math.min(o.iw, this.wh * 1, Math.round(this.ht * this.horizon / o.r)) * o.z1;
                else o.w1 = Math.round(this.ht * this.horizon / o.r) * o.z1;
                var x0 = o.x1 = (this.wh * .5) - (o.w1 * .5);
                var x = x0 + o.w1 + this.bdw;
                for (var i = this.view + 1, o; o = this.diapos[i]; i++) {
                    if (o.loaded) {
                        o.x1 = x;
                        o.w1 = (this.ht / o.r) * this.size;
                        x += o.w1 + this.bdw;
                        tw += o.w1 + this.bdw;
                    }
                }
                x = x0 - this.bdw;
                for (var i = this.view - 1, o; o = this.diapos[i]; i--) {
                    if (o.loaded) {
                        o.w1 = (this.ht / o.r) * this.size;
                        o.x1 = x - o.w1;
                        x -= o.w1 + this.bdw;
                        tw += o.w1 + this.bdw;
                        lw += o.w1 + this.bdw;
                    }
                }
                /* ---- move scrollbar ---- */
                if (!this.scr && tw) {
                    var r = (this.ws - this.alw - this.arw - this.bw) / tw;
                    this.bar.style.left = Math.round(this.alw + lw * r) + 'px';
                }
                /* ---- save preview view ---- */
                this.back = this.view;
            }
        },
        /* ==== mousewheel scrolling ==== */
        scroll: function (sc) {
            if (sc < 0) {
                if (this.view < this.NF - 1) this.calc(1);
            } else {
                if (this.view > 0) this.calc(-1);
            }
        },
        /* ==== resize  ==== */
        resize: function () {
            this.wh = this.oc.clientWidth;
            this.ht = this.oc.clientHeight;
            this.ws = this.scrollbar.offsetWidth;
            this.calc();
            this.run(true);
        },
        /* ==== animation loop ==== */
        run: function (res) {
            /* ---- move all images ---- */
            var i = this.NF;
            while (i--) this.diapos[i].move(res);
            /* ---- autoscroll ---- */
            if (this.time_out) {
                this.time++;
                if (this.time > this.time_out) {
                    this.view += this.time_dir;
                    if (this.view >= this.NF || this.view < 0) {
                        this.time_dir = -this.time_dir;
                        this.view += this.time_dir * 2;
                    }
                    this.calc();
                    this.time = 0;
                    this.time_out = this.time_inter;
                }
            }
        }
    }
    /* //////////// ==== Diapo Constructor ==== //////////// */
    Diapo = function (parent, N, src, title, text, url, target) {
        this.parent = parent;
        this.loaded = false;
        this.title = title;
        this.text = text;
        this.url = url;
        this.target = target;
        this.N = N;
        this.img = document.createElement('img');
        this.img.src = src;
        this.img.parent = this;
        this.img.className = 'diapo';
        this.x0 = this.parent.oc.clientWidth;
        this.x1 = this.x0;
        this.w0 = 0;
        this.w1 = 0;
        this.z1 = 1;
        this.z2 = 0;
        this.img.parent = this;
        this.img.onclick = function () { this.parent.click(); }
        this.parent.oc.appendChild(this.img);
        /* ---- display external link ---- */
        if (url) {
            this.img.onmouseover = function () { this.className = 'diapo link'; }
            this.img.onmouseout = function () { this.className = 'diapo'; }
        }
    }
    /* //////////// ==== Diapo prototype ==== //////////// */
    Diapo.prototype = {
        /* ==== HTML rendering ==== */
        move: function (res) {
            var that = this.parent;
            if (this.loaded) {
                var sx = this.x1 - this.x0;
                var sw = this.w1 - this.w0;
                if (Math.abs(sx) > 2 || Math.abs(sw) > 2 || res) {
                    /* ---- paint only when moving ---- */
                    this.x0 += sx * .1;
                    this.w0 += sw * .1;
                    var h = this.w0 * this.r;
                    this.z2 = Math.ceil((that.ht * that.horizon + 1 - this.z2 - h) * .5);
                    if (this.x0 < that.wh && this.x0 + this.w0 > 0) {
                        /* ---- paint only visible images ---- */
                        this.visible = true;
                        var o = this.img.style;
                        /* ---- diapo ---- */
                        o.left = Math.floor(this.x0) + 'px';
                        o.bottom = Math.floor(that.ht * (1 - that.horizon) + this.z2) + 'px';
                        o.width = Math.floor(this.w0) + 'px';
                        o.height = Math.floor(h) + 'px';
                        /* ---- reflexion ---- */
                        if (this.flx) {
                            var o = this.flx.style;
                            o.left = Math.floor(this.x0) + 'px';
                            o.top = Math.ceil(that.ht * that.horizon + 1 - this.z2) + 'px';
                            o.width = Math.floor(this.w0) + 'px';
                            o.height = Math.floor(h) + 'px';
                        }
                    } else {
                        /* ---- disable invisible images ---- */
                        if (this.visible) {
                            this.visible = false;
                            this.img.style.width = '0px';
                            if (this.flx) this.flx.style.width = '0px';
                        }
                    }
                }
            } else {
                /* ==== image onload ==== */
                if (this.img.complete && this.img.width) {
                    /* ---- get size image ---- */
                    this.iw = this.img.width;
                    this.ih = this.img.height;
                    this.r = this.ih / this.iw;
                    this.loaded = true;
                    /* ---- create reflexion ---- */
                    //this.flx = createReflexion(that.oc, this.img);
                    if (that.view < 0) that.view = this.N;
                    else if (this.N == 0) that.view = this.N;
                    that.calc();
                }
            }
        },
        /* ==== diapo onclick ==== */
        click: function () {
            /* ---- stop autoscroll ---- */
            this.parent.time = 0;
            this.parent.time_out = this.parent.time_start;
            if (this.parent.view != this.N) {
                /* ---- select diapo ---- */
                this.parent.view = this.N;
                this.parent.calc();
            }
            return false;
        }
    }
    /* //////////// ==== public methods ==== //////////// */
    return {
        /* ==== initialize script ==== */
        create: function (div, xmlfile, horizon, size, zoom, border, start, interval) {
            /* ---- instanciate imageFlow ---- */
            var load = function () {
                var loaded = false;
                var i = instances.length;
                while (i--) if (instances[i].oCont == div) loaded = true;
                if (!loaded) {
                    /* ---- push new imageFlow instance ---- */
                    instances.push(
						new ImageFlow(div, xmlfile, horizon, size, zoom, border, start, interval)
					);
                    /* ---- init script (once) ---- */
                    if (!imf.initialized) {
                        imf.initialized = true;
                        /* ---- window resize event ---- */
                        addEvent(window, 'resize', function () {
                            var i = instances.length;
                            while (i--) instances[i].resize();
                        });
                        /* ---- stop drag N drop ---- */
                        addEvent(document.getElementById(div), 'mouseout', function (e) {
                            if (!e) e = window.event;
                            var tg = e.relatedTarget || e.toElement;
                            if (tg && tg.tagName == 'HTML') {
                                var i = instances.length;
                                while (i--) instances[i].oc.onmousemove = null;
                            }
                            return false;
                        });
                        /* ---- set interval loop ---- */
                        setInterval(function () {
                            var i = instances.length;
                            while (i--) instances[i].run();
                        }, 16);
                    }
                }
            }
            /* ---- window onload event ---- */
            addEvent(window, 'load', function () { load(); });
        }
    }
}();

var xmlJSON = {
  "bank": {
    "img": [
      {
          "src": "https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Side-66791.jpg?1"
      },
      {
          "src": "https://imgd.aeplcdn.com//958x539//bikewaleimg/ec/15504/img/l/Bajaj-Pulsar-RS200-Front-three-quarter-50256.jpg?2"
      },
      {
          "src": "https://imgd.aeplcdn.com//958x539//bw/ec/19963/Honda-Activa-3G-Side-64339.jpg?3"
      },
      {
          "src": "https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Front-threequarter-66794.jpg?4"
      },
      {
          "src": "https://imgd.aeplcdn.com//958x539//bw/ec/19963/Honda-Activa-3G-Front-threequarter-64338.jpg?5"
      },
      {
          "src": "https://imgd.aeplcdn.com//958x539//bw/ec/22012/Honda-CB-Shine-Rear-threequarter-66799.jpg?6"
      }
    ]
  }
}

//imf.create("imageFlow", xmlJSON, 0.85, 0.4, 1, 20, 0, 0);