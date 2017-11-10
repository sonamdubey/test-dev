﻿
docReady(function () {

    $('.find-specs-tabs-wrapper .specs-btn').on('click', function () {

        var specBtn = $(this).attr('data-swipe');
        var swiperSlide = $('#rightBikeSwiper').find('.swiper-slide[data-model-index="' + specBtn + '"]');
        var indexOfSlide = swiperSlide.index();

        if (indexOfSlide >= 0)
        {
            //if (indexOfSlide > 0)
            //{
            //    $('#' + swiperSlide.attr('id') + ' .popular-tag').text('Recommended Bike');
            //}
            $('#rightBikeSwiper').data('swiper').slideTo(indexOfSlide, 1000, false);
        }

    });

    //collapsible content

    $('.foldable-content .read-more-button').on('click', function () {
        var readMoreButton = $(this);
        var collapsibleContent = readMoreButton.closest('.foldable-content');
        var isDataToggle = collapsibleContent.attr('data-toggle');
        var dataTruncate = collapsibleContent.find('.truncatable-content');
        var dataLessText;
        var readLessText;
        switch (isDataToggle) {
            case 'no':
                dataTruncate.attr('data-readtextflag', '0');
                readMoreButton.hide();
                break;
            case 'yes':
                dataLessText = readMoreButton.attr('data-text');
                readLessText = !dataLessText || dataLessText.length === 0 ? 'Collapse' : dataLessText;
                dataTruncate.attr('data-readtextflag', '0');
                readMoreButton.attr('data-text', readMoreButton.text()).text(readLessText);
                collapsibleContent.attr('data-toggle', 'hide');
                break;
            case 'hide':
                dataTruncate.attr('data-readtextflag', '1');
                dataLessText = readMoreButton.attr('data-text');
                readMoreButton.attr('data-text', readMoreButton.text()).text(dataLessText);
                collapsibleContent.attr('data-toggle', 'yes');
                break;
        }
    });

});