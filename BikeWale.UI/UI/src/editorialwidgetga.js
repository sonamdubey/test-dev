$('#widgetTab li').on('click', function (ele) {
    if (ele) {
        triggerGA(pageName, 'Clicked_on_Tab', ele.currentTarget.getAttribute('data-tabs') + '_' + ele.currentTarget.getAttribute('data-other-tabs'));
    }
});

$('.sidebar-list--overflow ul li a').on('click', function (ele) {
    if (ele) {
        triggerGA(pageName, 'Clicked_on_Model_Card', ele.currentTarget.getAttribute('title'));
    }
});

$('.read-more a').on('click', function (ele) {
    if (ele) {
        triggerGA(pageName, 'Clicked_on_View_All', ele.currentTarget.getAttribute('data-tab'));
    }
});