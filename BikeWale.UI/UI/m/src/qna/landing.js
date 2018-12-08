$(document).ready(function () {
    ToggleReadMore.registerEvents();
    $('.view-model-btn').on('click', function () {
        triggerGA("Question_Page", "Clicked_Model_GenericSlug", $(this).data('makename') + '_' + $(this).data('modelname'));
    });

})