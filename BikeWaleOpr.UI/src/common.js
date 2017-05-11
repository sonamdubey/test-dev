// Initialize collapse button for side nav
$(".buttons").sideNav();
$('.modal').modal();
$('.collapsible').collapsible();
// Do not remove this code. Its written to get values from checkbox to controller
$("input:checkbox").change(function () { $(this).val($(this).is(':checked')); });
$('select').material_select();
$(document).ready(function () {
    pageFooter.setPosition();
});

var pageFooter = {
    bodyElement: $('body'),

    container: $('#page-footer'),

    setPosition: function () {
        var flag = pageFooter.bodyElement.height() - window.innerHeight > pageFooter.container.height();

        if (pageFooter.bodyElement.height() > window.innerHeight && flag) {
            pageFooter.container.addClass('footer-relative');
        }
        else {
            pageFooter.container.addClass('footer-fixed');
        }
    }
}