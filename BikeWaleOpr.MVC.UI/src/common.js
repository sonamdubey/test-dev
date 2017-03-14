// Initialize collapse button for side nav
$(".buttons").sideNav();
$('.modal').modal();
$('.collapsible').collapsible();
// Do not remove this code. Its written to get values from checkbox to controller
$("input:checkbox").change(function () {$(this).val($(this).is(':checked'));});