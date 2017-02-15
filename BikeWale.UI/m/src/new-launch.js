// read more - collapse
$('.read-more-target').on('click', function () {
    var element = $(this),
        parentElemtent = element.closest('.collapsible-content');

    if (!parentElemtent.hasClass('active')) {
        parentElemtent.addClass('active');
        element.text(' Collapse');
    }
    else {
        parentElemtent.removeClass('active');
        element.text('...Read more');
    }
});

// more brand - collapse
$('.view-brandType').click(function () {
    var element = $(this),
        elementParent = element.closest('.collapsible-brand-content'),
        moreBrandContainer = elementParent.find('.brandTypeMore');

    if (!moreBrandContainer.is(':visible')) {
        moreBrandContainer.slideDown();
        element.attr('href', 'javascript:void(0)');
        element.text('View less brands');
    }
    else {
        element.attr('href', '#brand-type-container');
        moreBrandContainer.slideUp();
        element.text('View more brands');
    }
});

var directionRight = { direction: 'right' };

// brand
$('#brand-filter-selection').on('click', function () {
    slideInDrawer.open($('#brand-slideIn-drawer'));
    appendState('brand-drawer');
});

$('#close-brand-slideIn-drawer').on('click', function () {
    slideInDrawer.close($('#brand-slideIn-drawer'));
    history.back();
});

// year
$('#year-filter-selection').on('click', function () {
    slideInDrawer.open($('#year-slideIn-drawer'));
    appendState('year-drawer');
});

$('#close-year-slideIn-drawer').on('click', function () {
    slideInDrawer.close($('#year-slideIn-drawer'));
    history.back();
});

var slideInDrawer = {
    open: function (container) {
        container.show(effect, directionRight, duration, function () {
            container.addClass('fix-header-input');
        });
        windowScreen.lock();
    },

    close: function (container) {
        container.hide(effect, directionRight, duration, function () { });
        container.removeClass('fix-header-input');
        windowScreen.unlock();
    }
};

var windowScreen = {
    htmlElement: $('html'),

    bodyElement: $('body'),

    lock: function () {
        if ($(document).height() > $(window).height()) {
            var windowScrollTop = windowScreen.htmlElement.scrollTop() ? windowScreen.htmlElement.scrollTop() : windowScreen.bodyElement.scrollTop();
            if (windowScrollTop < 0) {
                windowScrollTop = 0;
            }
            windowScreen.htmlElement.addClass('lock-browser-scroll').css('top', -windowScrollTop);
        }
    },

    unlock: function () {
        var windowScrollTop = parseInt(windowScreen.htmlElement.css('top'));

        windowScreen.htmlElement.removeClass('lock-browser-scroll');
        $('html, body').scrollTop(-windowScrollTop);
    }
};

/* popup state */
var appendState = function (state) {
    window.history.pushState(state, '', '');
};

$(window).on('popstate', function (event) {
    if ($('#brand-slideIn-drawer').is(':visible')) {
        slideInDrawer.close($('#brand-slideIn-drawer'));
    }
    if ($('#year-slideIn-drawer').is(':visible')) {
        slideInDrawer.close($('#year-slideIn-drawer'));
    }
});

var vmPagination = function (curPgNum, pgSize, totalRecords) {
    var self = this;
    self.totalData = ko.observable(totalRecords);
    self.pageNumber = ko.observable(curPgNum);
    self.pageSize = ko.observable(pgSize);
    self.pageSlot = ko.observable(5);
    self.totalPages = ko.computed(function () {
        var div = Math.ceil(self.totalData() / self.pageSize());
        return div;
    });
    self.paginated = ko.computed(function () {
        var pgSlot;

        if (self.pageNumber() < 4) {
            pgSlot = self.pageSlot();
        } else {
            pgSlot = self.pageNumber() + self.pageSlot() - 3;
        }

        if (self.totalPages() > pgSlot) {
            return pgSlot;
        } else {
            return self.totalPages();
        }

    });
    self.hasPrevious = ko.computed(function () {
        return self.pageNumber() != 1;
    });
    self.hasNext = ko.computed(function () {
        return self.pageNumber() != self.totalPages();
    });
    self.next = function () {
        if (self.pageNumber() < self.totalPages())
            return self.pageNumber() + 1;
        return self.pageNumber();
    }
    self.previous = function () {
        if (self.pageNumber() > 1) {
            return self.pageNumber() - 1;
        }
        return self.pageNumber();
    }
};

ko.bindingHandlers.CurrencyText = {
    update: function (element, valueAccessor) {
        var amount = valueAccessor();
        var formattedAmount = ko.unwrap(amount) !== null ? formatPrice(amount) : 0;
        $(element).text(formattedAmount);
    }
};

var makeFilter = function () {
    var self = this;
    self.setMakeFilter = function (d, e) {

        $.getJSON("/api/v2/newlaunched/")
        {

        }
    };
};

var makeFilter = function () {
    var self = this;
    self.setYearFilter = function (d, e) {

    };
};

var newLaunches = function () {
    var self = this;
    self.Filters = ko.observable();
    self.MakeFilter = ko.observable(new makeFilter);
    self.YearFilter = ko.observable(new yearFilter);
    self.Pagination = ko.observable(new vmPagination());
    self.Filters.subscribe(function () {

    });
};