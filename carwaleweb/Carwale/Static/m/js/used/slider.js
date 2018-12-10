var slider = (function () {
    var effect = "slide";
    var options = { direction: 'right' };
    var duration = 500;

    var budgetRange = {
        start: 0,
        end: 20,
        step: 1
    };
    var kmsRange = {
        start: 0,
        end: 80000,
        step: 5000
    };
    var ageRange = {
        start: 0,
        end: 8,
        step:1
    };

    function bindSliders() {
        _bindSlider($('#mSlider-km'), kmsRange.start, kmsRange.end, kmsRange.step, "#kmText", "km");
        _bindSlider($('#mSlider-age'), ageRange.start, ageRange.end, ageRange.step, "#ageText", "year");
        _bindSlider($('#mSlider-budget'), budgetRange.start, budgetRange.end, budgetRange.step, "#budgetText", "lakh");
    }

    function _bindSlider($element, start, end, interval, sliderTextId, sliderType) {
        $element.slider({
            range: true,
            min: start,
            max: end,
            values: [start, end],
            step: interval
        });
        bindSliderText($element, sliderTextId, sliderType, start, end);
        filterValues.setSliderRangeQS($element, start, end);
    }

    function bindSliderText(element, sliderTextId, sliderType, start, end) {
        var minValue, maxValue;
        $(element).slider().on({
            slide: function (event, ui) {
                minValue = ui.values[0];
                maxValue = ui.values[1];
                computeSliderText(sliderTextId, sliderType, minValue, maxValue, start, end)
            },
        });
    }

    function computeSliderText(sliderTextId, sliderType, min, max, start, end) {
        var minText, maxText, minSliderText, maxSliderText;

        if (sliderType === 'km') {
            maxSliderText = 'k';
            minSliderText = 'k';
            minText = (min >= 1000) ? parseInt(min / 1000) : min;
            maxText = (max >= 1000) ? parseInt(max / 1000) : max;
        }
        else {
            minText = min;
            maxText = max;
            
            minSliderText = (min > 1) ? sliderType + 's' : sliderType;
            maxSliderText = (max > 1) ? sliderType + 's' : sliderType;
        }
        var minWithComma, maxWithComma;
        if (max !== undefined) {
            maxWithComma = commonUtilities.numberWithCommas(maxText);
        }
        minWithComma = commonUtilities.numberWithCommas(minText);

        var $sliderId = $(sliderTextId);
        var $sliderMinPointer = $($sliderId.closest('.accordion-list__item').find('.slider-tooltip--min-value'));
        var $sliderMaxPointer = $($sliderId.closest('.accordion-list__item').find('.slider-tooltip--max-value'));
        var minSliderTooltipText, maxSliderTooltipText, sliderRangeText;
        minSliderTooltipText = (minWithComma + ' ' + minSliderText);
        maxSliderTooltipText = (maxWithComma + ' ' + maxSliderText);
        sliderRangeText = 'All Range';
        if (max < 1) 
            return false;

        if (min === start && max === end) {
            minSliderTooltipText = (min + ' ' + minSliderText);
        }
        else if (min === max) {
            sliderRangeText = (minWithComma + ' ' + minSliderText);
        }
        else if (max < end && min === 0) {
            sliderRangeText = ('Below ' + maxWithComma + ' ' + maxSliderText);
            minSliderTooltipText = (min + ' ' + minSliderText);
        }
        else if (min <= end && max === end) {
            sliderRangeText = ('Above ' + minWithComma + ' ' + minSliderText);
        }
        else if (min <= end && max <= end) {
            sliderRangeText = (minWithComma + ' ' + minSliderText + ' ' + 'to' + ' ' + ' ' + maxWithComma + ' ' + maxSliderText);
        }
        $sliderId.text(sliderRangeText);
        $sliderMinPointer.text(minSliderTooltipText);
        $sliderMaxPointer.text(maxSliderTooltipText);
        _saveSliderValue(min, max, sliderType);
    }

    function computeSliderTextforLPage(sliderTextId, sliderType, min, max, start, end) {
        var $sliderId = $(sliderTextId);
        var $sliderMinPointer = $($sliderId.closest('.accordion-list__item').find('.slider-tooltip--min-value'));
        var $sliderMaxPointer = $($sliderId.closest('.accordion-list__item').find('.slider-tooltip--max-value'));
        if (max) {
            max = parseFloat(max);
        }
        min = parseFloat(min);
        
        var minText, maxText, minSliderText, maxSliderText, endText;
        if (sliderType === 'km') {
            if (max === '') {
                endText = parseInt(end / 1000);
            }
            else {
                maxText = parseInt(max / 1000);
            }

            minText = (min >= 1000) ? parseInt(min / 1000) : min;
            maxSliderText = 'k';
            minSliderText = 'k';
        }
        else {
            minText = min;
            maxText = max;
            endText = end;

            minSliderText = (min > 1) ? sliderType + 's' : sliderType;
            maxSliderText = (max > 1 || max == '') ? sliderType + 's' : sliderType;
        }
            
        var minSliderTooltipText, maxSliderTooltipText, sliderRangeText;
        minSliderTooltipText = (minText + ' ' + minSliderText);
        maxSliderTooltipText = (maxText + ' ' + maxSliderText);
        sliderRangeText = ('All Range');
        if (min >= 0 && max > min) {
            sliderRangeText = (minText + ' ' + minSliderText + ' ' + 'to' + ' ' + ' ' + maxText + ' ' + maxSliderText);
        }
            else if (min === max) {
                sliderRangeText = (minText + ' ' + minSliderText);
        }
        else if (min === 0 && max === "") {
            maxSliderTooltipText = (endText + ' ' + maxSliderText);
        }
        else if (max === "") {
            sliderRangeText = ('Above ' + minText + ' ' + minSliderText);
            maxSliderTooltipText = (endText + ' ' + maxSliderText);
        }
        else if (max > min && min === 0) {
            sliderRangeText = ('Below ' + maxText + ' ' + maxSliderText);

        }
        $sliderId.text(sliderRangeText);
        $sliderMinPointer.text(minSliderTooltipText);
        $sliderMaxPointer.text(maxSliderTooltipText);
        _saveSliderValueforLpage(min, max, sliderType);
    }

    function _saveSliderValue(minValue, maxValue, sliderValueType) {
        switch (sliderValueType) {
            case "year":
                var $element = $("#ageValues");
                if (parseInt(maxValue) === 8 || !maxValue)
                    $element.text(minValue + "-");
                else
                    $element.text(minValue + "-" + maxValue)
                break;
            case "km":
                var $element = $("#kmsValues");
                if (parseInt(maxValue) === 80000 || !maxValue)
                    $element.text(parseInt(minValue) / 1000 + "-");
                else
                    $element.text(parseInt(minValue) / 1000 + "-" + parseInt(maxValue) / 1000);
                break;
            case "lakh":
                var $element = $("#budgetValues");
                if (parseInt(maxValue) === 20 || !maxValue)
                    $element.text(minValue + "-");
                else
                    $element.text(minValue + "-" + maxValue);
                break;
            default:
                return true;
        }
    }

    function _saveSliderValueforLpage(minValue, maxValue, sliderValueType) {
        switch (sliderValueType) {
            case "year":
                var $element = $("#ageValues");
                if (parseInt(maxValue) === 8 || !maxValue)
                    $element.text(minValue + "-");
                else
                    $element.text(minValue + "-" + maxValue)
                break;
            case "km":
                var $element = $("#kmsValues");
                if (parseInt(maxValue) === 80000 || !maxValue)
                    $element.text(parseInt(minValue) / 1000 + "-");
                else
                    $element.text(parseInt(minValue) / 1000 + "-" + parseInt(maxValue) / 1000);
                break;
            case "lakh":
                var $element = $("#budgetValues");
                if (!maxValue)
                    $element.text(minValue + "-");
                else
                    $element.text(minValue + "-" + maxValue);
                break;
            default:
                return true;
        }
    }

    return {
        effect: effect,
        options: options,
        duration: duration,
        budgetRange: budgetRange,
        kmsRange: kmsRange,
        ageRange: ageRange,
        bindSliders: bindSliders,
        computeSliderText: computeSliderText,
        computeSliderTextforLPage: computeSliderTextforLPage
    };
})();