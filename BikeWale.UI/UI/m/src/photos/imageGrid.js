var ImageGrid = (function () {
    var isDesktop = (/Mobi|Android/i.test(navigator.userAgent)) ? false : true;

    function _calculateHeight(parent, width) {
        var parentWidth = parent.width();
        var minHeight = (parentWidth * width / 100) * 9 / 16
        return (minHeight + 'px')
    };

    function resizePortraitImageContainer(element) {
        var imageElement = new Image();
        imageElement.src = element.attr('data-original') || element.attr('src');

        if ((imageElement.width / imageElement.height) < 1.5) {
            var elementParent = element.parent();
            elementParent.css({
                'height': Math.floor(elementParent[0].getBoundingClientRect().width * 9 / 16) + 'px'
            });
        }
    }

    var container = isDesktop ? $('#imageGridTop') : null;
    function alignRemainderImage() {
        var imageList = $('.image-grid__list');

        imageList.each(function () {
            var gridSize = Number($(this).attr('data-grid'));
            var imageListItem = $(this).find('.image-grid-list__item');
            var imageListItemCount = imageListItem.length;

            var remainder = imageListItemCount % gridSize;

            switch (remainder) {
                case 1:
                    if (gridSize === 7) {
                        var remainderImage = imageListItem.slice(imageListItemCount - 1);

                        _gridOne(remainderImage);
                    }
                    break;

                case 2:
                    var remainderImage = imageListItem.slice(imageListItemCount - 2);

                    _gridTwo(remainderImage);
                    break;

                case 3:
                    var remainderImage = imageListItem.slice(imageListItemCount - 3);
                    if (isDesktop && gridSize === 8) {
                        _gridThreeDesktop(remainderImage);
                    }
                    else if (gridSize === 6) {
                        _gridThree(remainderImage);
                        remainderImage.first().css({
                            'float': 'right'
                        })
                    }
                    break;

                case 5:
                    if (isDesktop && gridSize === 8) {
                        var remainderImage = imageListItem.slice(imageListItemCount - 5);
                        _gridFiveDesktop(remainderImage);
                    }
                    else if (gridSize === 6) {
                        var remainderImage = imageListItem.slice(imageListItemCount - 1);

                        _gridOne(remainderImage);
                    }
                    else if (gridSize === 7) {
                        var remainderImage = imageListItem.slice(imageListItemCount - 2);

                        _gridTwo(remainderImage);
                    }
                    break;

                case 6:
                    var remainderImage = imageListItem.slice(imageListItemCount - 4);
                    if (isDesktop && gridSize === 8) {
                        _gridSixDesktop(remainderImage);
                    }
                    break;

                case 7:
                    var remainderImage = imageListItem.slice(imageListItemCount - 7);
                    if (isDesktop && gridSize === 8) {
                        _gridSevenDesktop(remainderImage);
                    }
                    break;

                default:
                    break;
            }

        });
    }

    function _gridOne(image) {
        image.css({
            'width': '100%'
        });
    }

    function _gridTwo(images) {
        var style = {
            'width': '50%'
        };
        if (isDesktop) {
            var minHeight = _calculateHeight(container, 50)
            style = {
                'width': '50%',
                'min-height': minHeight
            };
        }
        images.css(style);
    }

    function _gridThree(images) {
        images.each(function (index) {
            if (!index) {
                $(this).css({
                    'width': '66.67%'
                })
            }
            else {
                $(this).css({
                    'width': '33.33%'
                })
            }
        });
    }
    function _gridThreeDesktop(images) {
        images.each(function () {
            var style = {
                'width': '33.33%'
            }
            if (isDesktop) {
                var minHeight = _calculateHeight(container, 33.33)
                style = {
                    'width': '33.33%',
                    'min-height': minHeight
                };
            }
            $(this).css(style);
        });
    }
    function _gridFiveDesktop(images) {
        images.each(function (index) {
            if (!index) {
                var minHeight = _calculateHeight(container, 50)
                $(this).css({
                    'width': '50%',
                    'min-height': minHeight
                })
            }
            else {
                var minHeight = _calculateHeight(container, 25)
                $(this).css({
                    'width': '25%',
                    'min-height': minHeight
                })
            }
        });
    }
    function _gridSixDesktop(images) {
        images.each(function (index) {
            if (!index) {
                $(this).css({
                    'margin-bottom': '5px'
                })
            }
            else if (index === 2) {
                $(this).css({
                    'width': '66.67%',
                    'float': 'right'
                })
            }
            else {
                var minHeight = _calculateHeight(container, 33.33)
                $(this).css({
                    'width': '33.33%',
                    'min-height': minHeight
                })
            }
        });
    }
    function _gridSevenDesktop(images) {
        images.each(function (index) {
            if (!index) {
                var minHeight = _calculateHeight(container, 50)
                $(this).css({
                    'width': '50%',
                    'min-height': minHeight
                })
            }
            else if (index <= 4) {
                var minHeight = _calculateHeight(container, 25)
                $(this).css({
                    'width': '25%',
                    'min-height': minHeight
                })
            }
            else {
                $(this).css({
                    'width': '33.33%'
                })
            }
        });
    }
    return {
        alignRemainderImage: alignRemainderImage,
        resizePortraitImageContainer: resizePortraitImageContainer
    }
})();