/* Sticky-kit v1.1.2 */
(function(){var b,f;b=this.jQuery||window.jQuery;f=b(window);b.fn.stick_in_parent=function(d){var A,w,J,n,B,K,p,q,k,E,t;null==d&&(d={});t=d.sticky_class;B=d.inner_scrolling;E=d.recalc_every;k=d.parent;q=d.offset_top;p=d.spacer;w=d.bottoming;null==q&&(q=0);null==k&&(k=void 0);null==B&&(B=!0);null==t&&(t="is_stuck");A=b(document);null==w&&(w=!0);J=function(a,d,n,C,F,u,r,G){var v,H,m,D,I,c,g,x,y,z,h,l;if(!a.data("sticky_kit")){a.data("sticky_kit",!0);I=A.height();g=a.parent();null!=k&&(g=g.closest(k));
if(!g.length)throw"failed to find stick parent";v=m=!1;(h=null!=p?p&&a.closest(p):b("<div />"))&&h.css("position",a.css("position"));x=function(){var c,f,e;if(!G&&(I=A.height(),c=parseInt(g.css("border-top-width"),10),f=parseInt(g.css("padding-top"),10),d=parseInt(g.css("padding-bottom"),10),n=g.offset().top+c+f,C=g.height(),m&&(v=m=!1,null==p&&(a.insertAfter(h),h.detach()),a.css({position:"",top:"",width:"",bottom:""}).removeClass(t),e=!0),F=a.offset().top-(parseInt(a.css("margin-top"),10)||0)-q,
u=a.outerHeight(!0),r=a.css("float"),h&&h.css({width:a.outerWidth(!0),height:u,display:a.css("display"),"vertical-align":a.css("vertical-align"),"float":r}),e))return l()};x();if(u!==C)return D=void 0,c=q,z=E,l=function(){var b,l,e,k;if(!G&&(e=!1,null!=z&&(--z,0>=z&&(z=E,x(),e=!0)),e||A.height()===I||x(),e=f.scrollTop(),null!=D&&(l=e-D),D=e,m?(w&&(k=e+u+c>C+n,v&&!k&&(v=!1,a.css({position:"fixed",bottom:"",top:c}).trigger("sticky_kit:unbottom"))),e<F&&(m=!1,c=q,null==p&&("left"!==r&&"right"!==r||a.insertAfter(h),
h.detach()),b={position:"",width:"",top:""},a.css(b).removeClass(t).trigger("sticky_kit:unstick")),B&&(b=f.height(),u+q>b&&!v&&(c-=l,c=Math.max(b-u,c),c=Math.min(q,c),m&&a.css({top:c+"px"})))):e>F&&(m=!0,b={position:"fixed",top:c},b.width="border-box"===a.css("box-sizing")?a.outerWidth()+"px":a.width()+"px",a.css(b).addClass(t),null==p&&(a.after(h),"left"!==r&&"right"!==r||h.append(a)),a.trigger("sticky_kit:stick")),m&&w&&(null==k&&(k=e+u+c>C+n),!v&&k)))return v=!0,"static"===g.css("position")&&g.css({position:"relative"}),
a.css({position:"absolute",bottom:d,top:"auto"}).trigger("sticky_kit:bottom")},y=function(){x();return l()},H=function(){G=!0;f.off("touchmove",l);f.off("scroll",l);f.off("resize",y);b(document.body).off("sticky_kit:recalc",y);a.off("sticky_kit:detach",H);a.removeData("sticky_kit");a.css({position:"",bottom:"",top:"",width:""});g.position("position","");if(m)return null==p&&("left"!==r&&"right"!==r||a.insertAfter(h),h.remove()),a.removeClass(t)},f.on("touchmove",l),f.on("scroll",l),f.on("resize",
y),b(document.body).on("sticky_kit:recalc",y),a.on("sticky_kit:detach",H),setTimeout(l,0)}};n=0;for(K=this.length;n<K;n++)d=this[n],J(b(d));return this}}).call(this);

$(document).ready(function () {
    //set filters
    filters.set.all();
});

$('#listing-left-column').stick_in_parent();
$('.city-chosen-select').chosen();

/* budget slider */
var budgetValue = ['0', '10,000', '20,000', '35,000', '50,000', '80,000', '1,25,000', '2,00,000+'],
    budgetKey = [0, 1, 2, 3, 4, 5, 6, 7];

$('#budget-range-slider').slider({
    orientation: 'horizontal',
    range: true,
    min: 0,
    max: 7,
    step: 1,
    values: [0, 7],
    slide: function (event, ui) {
        var left = event.keyCode != $.ui.keyCode.RIGHT,
            right = event.keyCode != $.ui.keyCode.LEFT,
            value = findNearest(left, right, ui.value);

        if (ui.values[0] == ui.values[1]) {
            return false;
        }

        filters.budgetAmount(ui.values);
        filters.selection.set.slider('budget-amount');
    }
});

function findNearest(left, right, value) {
    var nearest = null;
    var diff = null;
    for (var i = 0; i < budgetKey.length; i++) {
        if ((left && budgetKey[i] <= value) || (right && budgetKey[i] >= value)) {
            var newDiff = Math.abs(value - budgetKey[i]);
            if (diff == null || newDiff < diff) {
                nearest = budgetKey[i];
                diff = newDiff;
            }
        }
    }
    return nearest;
}

function getRealValue(sliderValue) {
    for (var i = 0; i < budgetKey.length; i++) {
        if (budgetKey[i] >= sliderValue) {
            return budgetValue[i];
        }
    }
    return 0;
}

/* kms slider */
$("#kms-range-slider").slider({
    range: 'min',
    value: 80000,
    min: 5000,
    max: 80000,
    step: 5000,
    slide: function (event, ui) {
        filters.kilometerAmount(ui.value);
        filters.selection.set.slider('kms-amount'); // div id of slider values
    }
});

/* bike age slider */
$("#bike-age-slider").slider({
    range: 'min',
    value: 8,
    min: 1,
    max: 8,
    step: 1,
    slide: function (event, ui) {
        filters.bikeAgeAmount(ui.value);
        filters.selection.set.slider('bike-age-amount');
    }
});

$('#previous-owners-list').on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        $(this).addClass('active');
        filters.selection.set.owner(item);
    }
    else {
        $(this).removeClass('active');
        filters.selection.reset.owner(item);
    }
});

$('#seller-type-list').on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('checked')) {
        $(this).addClass('checked');
        filters.selection.set.seller(item);
    }
    else {
        $(this).removeClass('checked');
        filters.selection.reset.seller(item);
    }
});

$('#reset-filters').on('click', function () {
    filters.reset.all();
    accordion.resetAll();
    $('#selected-filters li').empty();
    filters.set.clearButton();
});

$('#reset-bikes-filter').on('click', function () {
    accordion.resetAll();
    filters.set.clearButton();
    $('#bike').empty();
});

/* bike filters */

var bikeFilterList =  $('#filter-bike-list');

bikeFilterList.on('click', '.accordion-label-tab', function () {
    var tab = $(this).closest('.accordion-tab');
    if (!tab.hasClass('active')) {
        accordion.open(tab);
    }
    else {
        accordion.close(tab);
    }
});

bikeFilterList.on('click', '.accordion-tab .accordion-checkbox', function () {
    var tab = $(this).closest('.accordion-tab');

    if (!tab.hasClass('tab-checked')) {
        tab.addClass('tab-checked');
        accordion.setTab(tab);
        filters.selection.set.make(tab);
    }
    else {
        tab.removeClass('tab-checked');
        filters.selection.reset.make(tab);
        accordion.resetTab(tab);
    }

    filters.set.clearButton();
});

bikeFilterList.on('click', '.bike-model-list li', function () {
    var item = $(this);

    if (!item.hasClass('active')) {
        item.addClass('active');
        accordion.setCount(item);
        filters.selection.set.model(item);
    }
    else {
        item.removeClass('active');
        var tab = item.closest('.bike-model-list-content').siblings('.accordion-tab');

        if (!tab.hasClass('tab-checked')) {
            filters.selection.reset.model(item);
        }
        else {
            filters.selection.reset.tab(tab);
        }

        accordion.setCount(item);
    }
    
    filters.set.clearButton();
});

/* remove selected filters */
/* bike */
$('#selected-filters').on('click', '#bike p', function () {
    var item = $(this);

    if ($(this).attr('data-type') != 'make') {
        filters.selection.cancel.model(item);
    }
    else {
        filters.selection.cancel.make(item);
    }
});

/* sliders */
$('#selected-filters').on('click', '.type-slider p', function () {
    var contentType = $(this).closest('li').attr('data-id');
    filters.selection.cancel.slider(contentType);

    switch (contentType) {
        case 'budget-amount':
            filters.reset.budget();
            break;

        case 'kms-amount':
            filters.reset.kilometers();
            break;

        case 'bike-age-amount':
            filters.reset.bikeAge();
            break;

        default:
            break;
    }
    //filters.reset.kilometers();
});

/* owner */
$('#selected-filters').on('click', '#owners p', function () {
    filters.selection.cancel.owner($(this));
});

/* seller */
$('#selected-filters').on('click', '#seller p', function () {
    filters.selection.cancel.seller($(this));
});

/* set slider default values */
var filters = {
    
    budgetAmount: function (units) {
        var budgetminValue = getRealValue(units[0]),
            budgetmaxValue = getRealValue(units[1]);

        if (units[0] == 0 && units[1] == 7) {
            $("#budget-amount").html('<span class="bwsprite inr-sm-dark"></span> 0 - <span class="bwsprite inr-sm-dark"></span> ' + budgetmaxValue);
        }
        else {
            $("#budget-amount").html('<span class="bwsprite inr-sm-dark"></span> ' + budgetminValue + ' - <span class="bwsprite inr-sm-dark"></span> ' + budgetmaxValue);
        }
    },

    kilometerAmount: function (unit) {
        var kilometerValue = unit.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

        if (unit == 80000) {
            $("#kms-amount").html('0 - ' + kilometerValue + '+ kms');
        }
        else {
            $("#kms-amount").html('0 - ' + kilometerValue + ' kms');
        }

    },

    bikeAgeAmount: function (unit) {
        if (unit == 8) {
            $("#bike-age-amount").html('0 - ' + unit + '+ years');
        }
        else {
            $("#bike-age-amount").html('0 - ' + unit + ' years');
        }
    },

    set: {

        all: function () {
            //filters.set.city();
            filters.set.bike();
            filters.set.budget();
            filters.set.kilometers();
            filters.set.bikeAge();
            filters.set.previousOwners();
            filters.set.sellerType();
        },

        city: function () {
            $('#filter-type-city .selected-filters').text('All India');
        },

        bike: function () {
            //filterTypeBike.find('.selected-filters').text('All Bikes');
            var inputBoxes = $('.getModelInput');
            inputBoxes.each(function () {
                var filterList = $(this).closest('.bike-model-list-content').find('.bike-model-list');
                $(this).fastLiveFilter(filterList);
            });
        },

        budget: function () {
            var values = [3, 5];
            $('#budget-range-slider').slider('option', 'values', values);

            filters.budgetAmount(values);
        },

        kilometers: function () {
            var kilometerSlider = $('#kms-range-slider'),
                kmSliderValue;

            kilometerSlider.slider('option', 'value', 50000);
            kmSliderValue = kilometerSlider.slider('value');

            filters.kilometerAmount(kmSliderValue);
        },

        bikeAge: function () {
            var ageSlider = $('#bike-age-slider'),
                ageSliderValue;

            ageSlider.slider('option', 'value', 5);
            ageSliderValue = ageSlider.slider('value');

            filters.bikeAgeAmount(ageSliderValue);
        },

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        },

        clearButton: function () {
            var activeTab = $('.accordion-tab.tab-checked').length,
                activeModel = $('.bike-model-list li.active').length,
                bikeFilter = $('#filter-type-bike');

            if (activeTab > 0 || activeModel > 0) {
                bikeFilter.addClass('active-clear');
            }
            else {
                bikeFilter.removeClass('active-clear');
            }

        }

    },

    reset: {

        all: function () {
            //filters.reset.city();
            //filters.reset.bike();
            filters.reset.budget();
            filters.reset.kilometers();
            filters.reset.bikeAge();
            filters.reset.previousOwners();
            filters.reset.sellerType();
        },

        city: function () {
            $('#filter-type-city .selected-filters').text('All India');
        },

        bike: function () {
            filterTypeBike.find('.selected-filters').text('All Bikes');
        },

        budget: function () {
            $('#budget-range-slider').slider('option', 'values', [0, 7]);
            $('#budget-amount').html('<span class="bwmsprite inr-xxsm-icon"></span>0 - <span class="bwmsprite inr-xxsm-icon"></span>2,00,000+');
        },

        kilometers: function () {
            $('#kms-range-slider').slider('option', 'value', 80000);
            $("#kms-amount").html('0 - ' + $("#kms-range-slider").slider("value").toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + ' kms');
        },

        bikeAge: function () {
            $('#bike-age-slider').slider('option', 'value', 8);
            $("#bike-age-amount").html('0 - ' + $("#bike-age-slider").slider("value") + '+ years');
        },

        previousOwners: function () {
            $('#previous-owners-list li.active').removeClass('active');
        },

        sellerType: function () {
            $('.filter-type-seller.checked').removeClass('checked');
        }
    },

    selection: {
        list: $('#selected-filters'),

        bikeList: $('#bike'),

        ownerList: $('#owners'),

        sellerList: $('#seller'),

        set: {
            make: function (item) {
                var itemID = item.attr('id'),
                    itemLabel = item.find('.category-label').text();

                filters.selection.bikeList.append('<p data-id="' + itemID + '" data-type="make">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');

                var selectedBike = filters.selection.bikeList.find('p'),
                    selectedBikeLength = selectedBike.length,
                    modelList = item.siblings('.bike-model-list-content').find('.bike-model-list li'),
                    modelListLength = modelList.length,
                    i;

                selectedBike.each(function (index) {
                    if (selectedBikeLength > 0) {
                        for (i = 0; i < modelListLength; i++) {
                            var item = modelList[i];
                            if ($(this).attr('data-id') == $(item).attr('id')) {
                                $(this).remove();
                            }
                        }
                    }
                });
            },

            model: function (item) {
                var itemID = item.attr('id'),
                    itemLabel = item.find('.category-label').text();

                filters.selection.bikeList.append('<p data-id="' + itemID + '" data-type="model">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');
            },

            slider: function (content) {
                var amount = $('#'+ content).html(),
                    sliderItem = filters.selection.list.find('li[data-id="' + content + '"]');
               
                sliderItem.empty().append('<p>' + amount + '<span class="bwsprite cross-icon"></span></p>');
            },

            owner: function (item) {
                var itemID = item.attr('id'),
                    itemLabel = item.text();

                switch (itemLabel) {
                    case '1':
                        itemLabel = itemLabel + 'st owner';
                        break;

                    case '2':
                        itemLabel = itemLabel + 'nd owner';
                        break;

                    case '3':
                        itemLabel = itemLabel + 'rd owner';
                        break;

                    case '4':
                        itemLabel = itemLabel + 'th owner';
                        break;

                    case '4+':
                        itemLabel = itemLabel + ' owner';
                        break;

                    default:
                        break;
                }

                filters.selection.ownerList.append('<p data-id="' + itemID + '">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');
            },

            seller: function (item) {
                var itemID = item.attr('id'),
                    itemLabel = item.text();

                filters.selection.sellerList.append('<p data-id="' + itemID + '">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');
            }
        },

        reset: {
            make: function (tab) {
                var itemID = tab.closest('.accordion-tab').attr('id');

                filters.selection.bikeList.find($('p[data-id="' + itemID + '"]')).remove();
            },

            model: function (item) {
                var itemID = item.attr('id');

                filters.selection.bikeList.find('p[data-id="' + itemID + '"]').remove();
            },

            tab: function (tab) {
                var activeModels = tab.siblings('.bike-model-list-content').find('li.active'),
                    activeModelLength = activeModels.length,
                    i;

                filters.selection.bikeList.find('p[data-id="' + tab.attr('id') + '"').remove();

                for (i = 0; i < activeModelLength; i++) {
                    var item = activeModels[i],
                        itemID = $(item).attr('id'),
                        itemLabel = $(item).find('.category-label').text();

                    filters.selection.bikeList.append('<p data-id="' + itemID + '" data-type="model">' + itemLabel + '<span class="bwsprite cross-icon"></span></p>');
                }                

            },
            
            owner: function (item) {
                var itemID = item.attr('id');

                filters.selection.ownerList.find('p[data-id="' + itemID + '"]').remove();
            },

            seller: function (item) {
                var itemID = item.attr('id');

                filters.selection.sellerList.find('p[data-id="' + itemID + '"]').remove();
            }
            
        },

        cancel: {
            make: function (item) {
                var itemID = item.attr('data-id');

                $('#filter-sidebar').find($('#' + itemID)).find('.accordion-checkbox').trigger('click');
                item.remove();
            },

            model: function (item) {
                var itemID = item.attr('data-id');

                $('#filter-sidebar').find($('#' + itemID)).trigger('click');
                item.remove();
            },

            slider: function (content) {
                var sliderItem = filters.selection.list.find('li[data-id="' + content + '"]');
                sliderItem.empty();
            },

            owner: function (item) {
                var itemID = item.attr('data-id');

                $('#previous-owners-list').find('li[id="' + itemID + '"]').trigger('click');
            },

            seller: function (item) {
                var itemID = item.attr('data-id');

                $('#seller-type-list').find('li[id="' + itemID + '"]').trigger('click');
            }
        }
    }
};

/* fastLiveFilter jQuery plugin 1.0.3 */
jQuery.fn.fastLiveFilter = function(list, options) {
	// Options: input, list, timeout, callback
	options = options || {};
	list = jQuery(list);
	var input = this;
	var lastFilter = '', noResultLen = 0;
	var timeout = options.timeout || 0;
	var callback = options.callback || function (total) {
	    noResultLen = list.siblings('.no-result').length;

	    if (total == 0 && noResultLen < 1)
	        list.after(noResultDiv).show();
	    else if (total > 0 && noResultLen > 0)
	        $('.no-result').remove();
	};

	var keyTimeout;

	var noResultDiv = '<div class="no-result content-inner-block-10 text-light-grey">No search found!</div>';

	var lis = list.children();
	var len = lis.length;
	var oldDisplay = len > 0 ? lis[0].style.display : "block";
	callback(len); // do a one-time callback on initialization to make sure everything's in sync

	input.change(function() {
		var filter = input.val().toLowerCase();
		var li, innerText;
		var numShown = 0;
		for (var i = 0; i < len; i++) {
			li = lis[i];
			innerText = !options.selector ?
				(li.textContent || li.innerText || "") :
				$(li).find(options.selector).text();

			if (innerText.toLowerCase().indexOf(filter) >= 0) {
				if (li.style.display == "none") {
					li.style.display = oldDisplay;
				}
				numShown++;
			} else {
				if (li.style.display != "none") {
					li.style.display = "none";
				}
			}
		}

		callback(numShown);
		return false;
	}).keydown(function() {
		clearTimeout(keyTimeout);
		keyTimeout = setTimeout(function() {
			if( input.val() === lastFilter ) return;
			lastFilter = input.val();
			input.change();
		}, timeout);
	});
	return this; // maintain jQuery chainability
}

/* accordion */
var accordion = {

    tabs: $('#filter-bike-list .accordion-tab'),

    open: function (item) {
        accordion.inputBox($('#filter-bike-list .accordion-tab.active').siblings('.bike-model-list-content'));
        accordion.tabs.removeClass('active');
        accordion.tabs.siblings('.bike-model-list-content').slideUp();
        item.addClass('active');
        item.siblings('.bike-model-list-content').slideDown();
    },

    close: function (item) {
        item.removeClass('active');
        item.siblings('.no-result').remove();
        item.siblings('.bike-model-list-content').slideUp();
        accordion.inputBox(item.siblings('.bike-model-list-content'));
    },

    inputBox: function (listContent) {
        listContent.find('input[type="text"]').val('');
        listContent.find('li').show();
    },

    setCount: function (item) {
        var modelList = item.closest('.bike-model-list'),
            modelsCount = modelList.find('li.active').length,
            tab = modelList.closest('.bike-model-list-content').siblings('.accordion-tab'),
            tabCountLabel = tab.find('.accordion-count');

        if (tab.hasClass('tab-checked')) {
            tab.removeClass('tab-checked');
        }

        if (!modelsCount == 0) {
            if (modelsCount == 1) {
                tabCountLabel.html('(' + modelsCount + ' Model)');
            }
            else {
                tabCountLabel.html('(' + modelsCount + ' Models)');
            }
        }
        else {
            tabCountLabel.empty();
        }

    },

    setTab: function (tab) {
        var modelList = tab.siblings('.bike-model-list-content');

        modelList.find('li').addClass('active');
        tab.find('.accordion-count').html('(All models)');
    },

    resetTab: function (tab) {
        var modelList = tab.siblings('.bike-model-list-content');

        modelList.find('li').removeClass('active');
        tab.find('.accordion-count').empty();
    },

    resetAll: function () {
        var bikeList = $('#filter-bike-list');

        bikeList.find('.accordion-tab.active').siblings('.bike-model-list-content').hide();
        bikeList.find('.accordion-tab.active').removeClass('active');
        bikeList.find('.accordion-tab.tab-checked').removeClass('tab-checked');
        bikeList.find('.bike-model-list li.active').removeClass('active');
        accordion.tabs.find('.accordion-count').text('');
    }
};

/* sort by */
var sortByDiv = $(".sort-div"),
    sortListDiv = $(".sort-selection-div");

sortByDiv.on('click', function () {
    if (!sortByDiv.hasClass("open"))
        sortBy.open();
    else
        sortBy.close();
});

$('#sort-listing').on('click', 'li', function () {
    var item = $(this);

    if (!item.hasClass('selected')) {
        $('#sort-listing li.selected').removeClass('selected');
        item.addClass('selected');
        sortBy.selection(item);
    }
    else {
        sortBy.close();
    }
});

var sortBy = {
    open: function () {
        sortByDiv.addClass('open');
        sortListDiv.show();
    },

    close: function () {
        sortByDiv.removeClass('open');
        sortListDiv.slideUp();
    },

    selection: function (item) {
        var itemText = item.text();
        sortByDiv.find('.sort-select-btn').text(itemText);
        sortBy.close();
    }
}

/* close sortby */
$(document).mouseup(function (e) {
    var container = $('#sort-by-content');
    if (container.find('.sort-div').hasClass('open') && $('.sort-selection-div').is(':visible')) { 
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            sortByDiv.trigger('click');
        }
    }
});