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
            listItemIndex = listItem.index(),
            list = listItem.closest('.compare-bike-list');

        listItem.remove();

        if(!list.hasClass('floating-compare-list')) {
            if(list.find('.list-item').length > 1) {
                $('.floating-compare-list li:eq('+ listItemIndex +')').remove();
                compareBox.emptyTableData(listItemIndex);

                if(!list.find('.box-placeholder').length) {
                    compareBox.setPlaceholderBox(list[0]);
                }

                compareBox.resizeColumn(list[0]);
                compareBox.setSponsoredIndex();
            }
            else {
                //window.location.href = "/compare";
            }
        }
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

    var bodyElement = document.getElementsByTagName("body")[0],        
        hideCommonFeatures = true,
        equivalentDataFound = false;

    var sponsoredColumn = document.getElementById('sponsored-column-active');

    $('.toggle-features-btn').on('click', function () {
        if (hideCommonFeatures) {
            if (!equivalentDataFound) {
                var dataRows = document.getElementsByClassName('row-type-data');

                compareBox.detectEquivalentData(dataRows);
                equivalentDataFound = true;
            }

            bodyElement.setAttribute('data-equivalent', 1);
            compareBox.toggleBtnLabel($(this)[0], 'Show all features');
            hideCommonFeatures = false;
        }
        else {
            bodyElement.setAttribute('data-equivalent', 0);
            compareBox.toggleBtnLabel($(this)[0], 'Hide common features');
            hideCommonFeatures = true;
        }

    });

    var compareBox = {
        minColumnLimit: 2,
        classPrefix: 'select-type-',
        dropdownArr: ['brand', 'model', 'version'],

        placeholderTemplate: '<p class="font15 text-bold margin-bottom20">Add bike to compare</p><div class="add-bike-form"><div class="box-placeholder"><span class="box-label"><span class="add-icon"></span></span></div><div class="text-center"><button type="button" class="add-compare-btn btn btn-white btn-160-32">Add another bike</button></div></div>',

        addToCompareButton: '<div class="text-center margin-top5"><a href="" class="btn btn-white btn-160-32">Add to compare</a></div>',

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
        },

        resizeColumn: function (list) {
            var listItem = list.getElementsByClassName('list-item'),
                listItemLength = listItem.length;

            if(listItemLength != compareBox.minColumnLimit) {
                document.getElementById('bike-comparison-container').setAttribute('data-column-count', listItemLength);
            }
        },

        detectEquivalentData: function (dataRows) {
            var list = document.getElementsByClassName('compare-bike-list')[0],
                bikeItemCount = list.getElementsByClassName('bike-details-block').length;

            var dataRowLength = dataRows.length;

            for(var i = 0; i < dataRowLength; i++) {
                compareBox.compareTableData(bikeItemCount, dataRows[i]);
            }
        },

        compareTableData: function (bikeItemCount, rowElement) {
            var rowColumns = rowElement.getElementsByTagName("td"),
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
                rowElement.className += " equivalent-data";
            }
        },

        toggleBtnLabel: function (btn, message) {
            btn.getElementsByClassName('btn-label')[0].innerHTML = message;           
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

});