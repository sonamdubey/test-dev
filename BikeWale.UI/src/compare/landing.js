var bike1 = {
    makeName: 'Honda',
    modelName: 'CB Shine',
    versionName: 'Kick/Drum/Spokes',
    hostUrl: 'https://imgd.aeplcdn.com/',
    originalImagePath: '/bw/models/honda-shine.jpg',
    price: '70,147'
}

var bike2 = {
    makeName: 'Bajaj',
    modelName: 'Pulsar RS200',
    versionName: 'Standard',    
    hostUrl: 'https://imgd.aeplcdn.com/',
    originalImagePath: 'bw/models/bajaj-pulsar-rs200.jpg',
    price: '1,38,826'
}

var bikeBrands = {
    brandList: [
        {
            makeId: 2,
            makeName: "Aprilia"
        },
        {
            makeId: 1,
            makeName: "Bajaj"
        },
        {
            makeId: 40,
            makeName: "Benelli"
        },
        {
            makeId: 7,
            makeName: "Honda"
        },
        {
            makeId: 11,
            makeName: "Royal Enfield"
        },
    ]
}

docReady(function () {  

    var compareBike = function () {
        var self = this;

        self.makeArray = ko.observable(bike1);
    };

    var bikeSelectionTemplate = function (data) {
        var self = this;

        self.bikeData = ko.observable({
            makeArrayObj: data
        });
    };

    var bikeTemplate = function (data) {
        var self = this;

        self.modelData = data;
    };

    var vmCompareBike = new compareBike(),
        bikeComparisonBox = document.getElementById('bike-comparison-box');

    ko.applyBindings(vmCompareBike, bikeComparisonBox);

    $('.compare-box-list').on('change', '.select-box select', function () {
        var elementValue = $(this).val();

        if(elementValue > 0) {
            var selectBox = $(this).closest('.select-box');

            selectBox.addClass('done');
            compareBox.dropdownChange(selectBox, elementValue);
        }
    });

    $('.compare-box-list').on('click', '.box-placeholder', function () {
        var elementListItem = $(this).closest('.item-box');

        $(elementListItem).find('.box-placeholder').hide();
        compareBox.setDropdown(elementListItem[0]);

        /*
        $(this).remove();

        var data = bikeBrands.brandList;

        var koHTML = '<div data-bind="template: { name: \'bike-selection-template\', data: bikeData }"></div>';

        elementListItem.append(koHTML);
        ko.applyBindings(new bikeSelectionTemplate(data), elementListItem[0]);
        */
    });

    $('.compare-box-list').on('click', '.cancel-selected-item', function () {
        var elementListItem = $(this).closest('.item-box'),
            listParent = elementListItem.closest('.compare-box-list')[0];

        elementListItem.remove();
        compareBox.setPlaceholderBox(listParent);
    });

    function setBikeDetails(elementListItem, data) {
        var koHTML = '<div data-bind="template: { name: \'bike-template\', data: modelData }"></div>';

        elementListItem.find('.bike-selection-box').hide();
        elementListItem.append(koHTML);

        ko.applyBindings(new bikeTemplate(data), elementListItem[0]);
    };

    var compareBox = {
        classPrefix: 'select-type-',
        dropdownArr: ['brand', 'model', 'version'],

        placeholderTemplate: '<div class="box-placeholder"><span class="box-label"><span class="add-icon"></span><br />Add bike <span class="label-count"></span></span></div>',

        setPlaceholderBox: function (list) {
            var li = document.createElement("li");

            li.className = "item-box";
            li.innerHTML = compareBox.placeholderTemplate;
            list.appendChild(li);

            compareBox.setBikeCount(list, false);
            compareBox.setPlaceholderLabel(list);
        },

        setPlaceholderLabel: function (list) {
            var listItem = list.getElementsByClassName('item-box'),
                labelElement = list.getElementsByClassName('label-count'),
                selectionCount = Number(list.getAttribute('data-selection-count')),
                elementLength = labelElement.length;

            for(var i = selectionCount; i < elementLength; i++) {
                labelElement[i].innerHTML = i + 1;
            }
        },

        setDropdown: function (elementListItem) {
            var div = document.createElement("div");
            div.className = "bike-selection-box";

            var divTemplate = "";            
            for(var i = 0; i < compareBox.dropdownArr.length; i++) {
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

            div.innerHTML = divTemplate;
            elementListItem.appendChild(div);
            
            compareBox.getMakes(elementListItem);
        },

        dropdownChange: function (selectBox, elementValue) {
            if($(selectBox).hasClass('select-brand')) {
                compareBox.resetDropdownSelection(selectBox);
                compareBox.getModels(elementValue, selectBox.next()[0]);
            }
            else if($(selectBox).hasClass('select-model')) {
                compareBox.resetDropdownSelection(selectBox);
                compareBox.getVersions(elementValue, selectBox.next()[0]);
            }
            if($(selectBox).hasClass('select-version')) {
                var listItem = selectBox.closest('.item-box'),
                    list = listItem.closest('.compare-box-list');

                setBikeDetails(listItem, bike1);
                compareBox.setBikeCount(list, true); // true flag, to increment count
            }
        },

        setBikeCount: function (list, incrementFlag) {
            var selectionCount = Number($(list).attr('data-selection-count')),
                listItemElement = $(list).find('.item-box');

            if(!incrementFlag) {
                selectionCount--;
            }
            else {
                selectionCount++;
            }

            $(list).attr('data-selection-count', selectionCount);
            listItemElement.addClass('cur-disabled');

            if(!selectionCount) {
                selectionCount++;
            }

            for(var i = 0; i < listItemElement.length; i++) {
                if(i <= selectionCount) {
                    $(listItemElement[i]).removeClass('cur-disabled');
                }
            }

            compareBox.setSubmitButton();
        },

        resetDropdownSelection: function (selectBox) {
            var selectBoxSiblings = $(selectBox).nextAll('.select-box');
            
            selectBoxSiblings.removeClass('done');
            selectBoxSiblings.find('.chosen-select').empty().prop('disabled', true).trigger('chosen:updated');
        },

        getMakes: function (elementListItem) {
            $.ajax({
                type: "Get",
                async: false,
                url: "/api/MakeList/?requesttype=7",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    compareBox.setMakesDropdown(response.makes, elementListItem);
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

        setMakesDropdown: function (response, elementListItem) {
            var dropdownClass = compareBox.classPrefix + compareBox.dropdownArr[0],
                dropdownOptions = "<option value></option>",
                resoponseLength = response.length;

            for(var i = 0; i < resoponseLength; i++) {
                dropdownOptions += '<option value="' + response[i].makeId + '">' + response[i].makeName + '</option>';
            }

            $(elementListItem).find('.' + dropdownClass).html(dropdownOptions);
            compareBox.initChosen(elementListItem);

            /*var dropdownClass = compareBox.classPrefix + compareBox.dropdownArr[0],
                dropdownSelect = elementListItem.getElementsByClassName(dropdownClass);

            var brands = bikeBrands.brandList
                dropdownOptions = "<option value></option>";

            for(var i = 0; i < brands.length; i++) {
                dropdownOptions += '<option value="' + brands[i].makeId + '">' + brands[i].makeName + '</option>';
            }

            $(elementListItem).find('.' + dropdownClass).html(dropdownOptions);
            compareBox.initChosen(elementListItem);
            */
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

        initChosen: function (elementListItem) {
            $(elementListItem).find('.chosen-select').chosen();
        },

        updateChosen: function (selectBox) {
            $(selectBox).find('.chosen-select').prop('disabled', false).trigger("chosen:updated");
        },

        setSubmitButton: function () {
            var list = $(bikeComparisonBox).find('.compare-box-list'),
                selectionCount = Number(list.attr('data-selection-count'));

            if(selectionCount > 1) {
                $(bikeComparisonBox).find('.compare-now-btn').show();
            }
            else {
                $(bikeComparisonBox).find('.compare-now-btn').hide();
            }
        }
    }
});