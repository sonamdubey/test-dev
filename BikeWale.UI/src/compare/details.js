var floatingCard = $('#comparison-floating-card'),
    floatingCardHeight = floatingCard.height() - 44,
    comparisonFooter = $('#comparison-footer'),
    overallSpecsTabs = $('#overall-tabs'),
    $window = $(window),
    windowScrollTop;

docReady(function() {
    $('.chosen-select').chosen();

    // version dropdown
    selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

    // convert 'yes' and 'no' to their respective icons
    var dataRows = $('.table-content td'),
        tickIcon = '<span class="bwsprite tick-grey"></span>',
        crossIcon = '<span class="bwsprite cross-grey"></span>';

    dataRows.each(function () {
        var td = $(this),
            tdText = $.trim(td.text());

        if (tdText == "Yes") {
            td.html(tickIcon);
        }
        else if (tdText == "No") {
            td.html(crossIcon);
        }
    });

    $('.compare-bike-list').on('click', '.cancel-selected-item', function () {
        var listItem = $(this).closest('.list-item'),
            listItemIndex = listItem.index();

        var bikeList = $('.compare-bike-list');
        for(var i = 0; i < bikeList.length; i++) {
            $(bikeList[i]).find('li:eq('+ listItemIndex +')').remove();
        }

        var mainList = $('#bike-comparison-grid').find('.compare-bike-list');

        compareBox.emptyTableData(listItemIndex);
        compareBox.checkForPlaceholderBox(mainList);

        compareBox.resizeColumn(mainList[0]);
        compareBox.setSponsoredIndex();
    });

    $('.compare-bike-list').on('click', '.add-bike-form', function () {
        var listItem = $(this).closest('.list-item');

        $(this).hide();
        compareBox.setDropdown(listItem[0]);
    });

    $('.compare-bike-list').on('change', '.select-box select', function () {
        var elementValue = $(this).val();

        if(elementValue > 0) {
            var selectBox = $(this).closest('.select-box');

            compareBox.dropdownChange(selectBox, elementValue);
        }
    });

    var hideCommonFeatures = false,
        equivalentDataFound = false;

    var sponsoredColumn = document.getElementById('sponsored-column-active');

    $('.toggle-features-btn').on('click', function () {
        var btn = $('.toggle-features-btn');
        
        if (!hideCommonFeatures) {
            if (!equivalentDataFound) {
                var dataRows = document.getElementsByClassName('row-type-data');

                compareBox.detectEquivalentData(dataRows);
                equivalentDataFound = true;
            }

            compareBox.setFeaturesStatus(btn, 'Show all features', 1); // (btn, message, flag)
        }
        else {
            compareBox.setFeaturesStatus(btn, 'Hide common features', 0);
        }

    });

    var compareBox = {
        minColumnLimit: 2,
        classPrefix: 'select-type-',
        dropdownArr: ['brand', 'model', 'version'],

        placeholderTemplate: '<p class="font15 text-bold margin-bottom20">Add bike to compare</p><div class="add-bike-form"><div class="box-placeholder"><span class="box-label"><span class="add-icon"></span></span></div><div class="text-center"><button type="button" class="add-compare-btn btn btn-white btn-160-32">Add another bike</button></div></div>',

        addToCompareButton: '<div class="text-center margin-top5"><a href="" class="btn btn-white btn-160-32">Add to compare</a></div>',

        checkForPlaceholderBox: function (list) {
            if(list.find('.list-item').length > 1) {
                if(!list.find('.box-placeholder').length) {
                    compareBox.setPlaceholderBox(list[0]);
                }
            }
            else {
                window.location.href = "/compare/details/";
            }
        },

        setPlaceholderBox: function (list) {
            var li = document.createElement("li");

            li.className = "list-item";
            li.innerHTML = compareBox.placeholderTemplate;
            list.appendChild(li);
        },

        emptyTableData: function (listItemIndex) {
            var dataRows = $('.table-content tr'),
                dataRowLength = dataRows.length;
            
            // increment index by 1, since table's 1st td contains heading
            listItemIndex = listItemIndex + 1;

            for (var i = 0; i < dataRowLength; i++) {
                $(dataRows[i]).find('td:eq(' + listItemIndex + ')').remove();
                $(dataRows[i]).append('<td></td>');
            };

            if(equivalentDataFound) {
                compareBox.resetEquivalentData();
            }
        },

        resizeColumn: function (list) {
            var listItem = list.getElementsByClassName('list-item'),
                listItemLength = listItem.length;

            if(listItemLength != compareBox.minColumnLimit) {
                document.getElementById('bike-comparison-container').setAttribute('data-column-count', listItemLength);
            }
        },

        detectEquivalentData: function (dataRows) {
            var list = document.getElementById('bike-comparison-grid'),
                bikeItemCount = list.getElementsByClassName('bike-details-block').length;

            var dataRowLength = dataRows.length;

            for(var i = 0; i < dataRowLength; i++) {
                compareBox.compareTableData(bikeItemCount, dataRows[i]);
            }
        },

        resetEquivalentData: function () {
            var dataRows = document.getElementsByClassName('row-type-data'),
                dataRowLength = dataRows.length;

            for(var i = 0; i < dataRowLength; i++) {
                dataRows[i].className = 'row-type-data';
            }

            compareBox.setFeaturesStatus($('.toggle-features-btn'), 'Hide common features', 0);
            equivalentDataFound = false;
        },

        compareTableData: function (bikeItemCount, rowElement) {
            var rowColumns = rowElement.getElementsByTagName('td'),
                isEquivalent = false;

            for(var i = 1; i < bikeItemCount; i++) {
                if(rowColumns[i].innerHTML === rowColumns[i+1].innerHTML) {
                    isEquivalent = true;
                }
                else {
                    isEquivalent = false;
                    break;
                }
            }

            if(isEquivalent) {
                rowElement.className += ' equivalent-data';
            }
        },

        setFeaturesStatus: function (btn, message, flag) {
            document.getElementsByTagName("body")[0].setAttribute('data-equivalent', flag);
            compareBox.setBtnLabel(btn, message);

            hideCommonFeatures =  Boolean(flag);
        },

        setBtnLabel: function (btn, message) {
            btn.find('.btn-label').html(message);
        },

        setSponsoredIndex: function () {
            var listItemIndex = $('#bike-comparison-grid .compare-bike-list .sponsored-list-item').index() + 1; // increment by 1, since index starts with 0 and nth-child with 1

            $(sponsoredColumn).attr('data-sponsored-column', listItemIndex);
        },

        setDropdown: function (listItem) {
            var div = document.createElement("div");
            div.className = "bike-selection-box";

            var divTemplate = '',
                dropdownLength = compareBox.dropdownArr.length;

            for(var i = 0; i < dropdownLength; i++) {
                var dropdownLabel = compareBox.dropdownArr[i];

                divTemplate += '<div class="select-box select-box-size-13 select-' + dropdownLabel + '">';
                divTemplate += '<p class="select-label">'+ dropdownLabel +'</p>';
                if(i != 0) {
                    divTemplate += '<select class="chosen-select ' + compareBox.classPrefix + dropdownLabel + '" data-title="Select ' +  dropdownLabel + '" disabled></select>';
                }
                else {
                    divTemplate += '<select class="chosen-select ' + compareBox.classPrefix + dropdownLabel + '" data-title="Select ' +  dropdownLabel + '"></select>';
                }
                divTemplate += '</div>';
            }
            
            divTemplate += compareBox.addToCompareButton;

            div.innerHTML = divTemplate;
            listItem.appendChild(div);
            
            compareBox.getMakes(listItem);
        },

        dropdownChange: function (selectBox, elementValue) {
            selectBox.addClass('done');

            if($(selectBox).hasClass('select-brand')) {
                compareBox.resetDropdownSelection(selectBox);
                compareBox.getModels(elementValue, selectBox.next()[0]);
            }
            else if($(selectBox).hasClass('select-model')) {
                compareBox.resetDropdownSelection(selectBox);
                compareBox.getVersions(elementValue, selectBox.next()[0]);
            }
            else if($(selectBox).hasClass('select-version')) {
                
            }
        },

        resetDropdownSelection: function (selectBox) {
            var selectBoxSiblings = $(selectBox).nextAll('.select-box');
            
            selectBoxSiblings.removeClass('done');
            selectBoxSiblings.find('.chosen-select').empty().prop('disabled', true).trigger('chosen:updated');
        },

        getMakes: function (listItem) {
            $.ajax({
                type: "Get",
                async: false,
                url: "/api/MakeList/?requesttype=7",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    compareBox.setMakesDropdown(response.makes, listItem);
                }
            });
        },

        getModels: function (makeId, selectBox) {
            $.ajax({
                type: "Get",
                async: false,
                url: "/api/modellist/?requestType=3&makeId=" + makeId,
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    compareBox.setModelDropdown(response.modelList, selectBox);
                }
            });
        },

        getVersions: function (modelId, selectBox) {
            $.ajax({
                type: "Get",
                async: false,
                url: "/api/versionList/?requestType=2&modelId=" + modelId,
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {                    
                    compareBox.setVersionDropdown(response.Version, selectBox);
                }
            });
        },

        setMakesDropdown: function (response, listItem) {
            var dropdownOptions = "<option value></option>",
                responseLength = response.length;

            for(var i = 0; i < responseLength; i++) {
                dropdownOptions += '<option value="' + response[i].makeId + '">' + response[i].makeName + '</option>';
            }

            var dropdownClass = compareBox.classPrefix + compareBox.dropdownArr[0];

            $(listItem).find('.' + dropdownClass).html(dropdownOptions);
            compareBox.initChosen(listItem);
        },

        setModelDropdown: function (response, selectBox) {
            var dropdownOptions = "<option value></option>",
                resoponseLength = response.length;

            for(var i = 0; i < resoponseLength; i++) {
                dropdownOptions += '<option value="' + response[i].modelId + '">' + response[i].modelName + '</option>';
            }

            $(selectBox).find('.chosen-select').html(dropdownOptions);
            compareBox.updateChosen(selectBox);
        },

        setVersionDropdown: function (response, selectBox) {
            var dropdownOptions = "<option value></option>",
                resoponseLength = response.length;

            for(var i = 0; i < resoponseLength; i++) {
                dropdownOptions += '<option value="' + response[i].versionId + '">' + response[i].versionName + '</option>';
            }

            $(selectBox).find('.chosen-select').html(dropdownOptions);
            compareBox.updateChosen(selectBox);
        },

        initChosen: function (listItem) {
            $(listItem).find('.chosen-select').chosen();
        },

        updateChosen: function (selectBox) {
            $(selectBox).find('.chosen-select').prop('disabled', false).trigger("chosen:updated");
        }
    }

    if(typeof sponsoredColumn !== 'undefined') {
        compareBox.setSponsoredIndex();
    }

    var windowHeight = $window.height();

    $window.on('scroll', function () {
        var overallSpecsOffset = overallSpecsTabs.offset().top - floatingCardHeight,
            footerOffsetForButton = comparisonFooter.offset().top - windowHeight,
            footerOffsetForCard = comparisonFooter.offset().top - floatingCardHeight - 88;

        windowScrollTop = $window.scrollTop();

        if (windowScrollTop > overallSpecsOffset) {
            floatingCard.addClass('fixed-card');

            if (windowScrollTop > footerOffsetForCard) {
                floatingCard.removeClass('fixed-card');
            }
        }
        else if (windowScrollTop < overallSpecsOffset) {
            floatingCard.removeClass('fixed-card');
        }

    });

    /* floating tabs */
    $('.overall-specs-tabs-wrapper').on('click', 'li', function () {
        var elementIndex = $(this).index(),
            tabId = $(this).attr('data-tabs'),
            panel = $(this).closest('.bw-tabs-panel'),
            floatingTabs = panel.find('.overall-specs-tabs-wrapper');

        floatingTabs.find('li.active').removeClass('active');
        for(var i = 0; i < floatingTabs.length; i++) {
            $(floatingTabs[i]).find('li:eq(' + elementIndex + ')').addClass('active');
        };

        panel.find('.bw-tabs-data').removeClass('active').hide();
        $('#' + tabId).addClass('active').show();
        $('html, body').animate({ scrollTop: overallSpecsTabs.offset().top - floatingCardHeight }, 500); // 44px accordion tab height
    });

});