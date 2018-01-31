var bike1 = {
    makeName: '',
    modelName: '',
    versionName: '',
    hostUrl: '',
    originalImagePath: '',
    price: ''
}
var isSameVersion = false;
var bikeDetails = new Array();
docReady(function () {
    bwcache.removeAll(true);
    bwcache.setScope('CLPage');

    var bikeComparisonBox = document.getElementById('bike-comparison-box');

    $('.comparison-type-carousel .jcarousel-control-prev').jcarouselControl({
        target: '-=2'
    });

    $('.comparison-type-carousel .jcarousel-control-next').jcarouselControl({
        target: '+=2'
    });

    var bikeTemplate = function (data) {
        var self = this;
        self.modelData = data;
    };

    $('.compare-box-list').on('change', '.select-box select', function () {
        var elementValue = $(this).val();

        if (elementValue > 0) {
            var selectBox = $(this).closest('.select-box');
            compareBox.dropdownChange(selectBox, elementValue);
        }
    });
    $('#btnCompare').on('click', function () {
        $.each(bikeDetails, function (i, val) {
            val.index = i+1;

        });

        bikeDetails.sort(function (a, b) {
            return a.modelId - b.modelId;
        });
        var result = "/comparebikes/";
        $.each(bikeDetails, function (i, val) {
            if(i!=bikeDetails.length-1)
                result += (val.makemasking + "-" + val.modelmasking + "-vs-");
            else
                result += (val.makemasking + "-" + val.modelmasking +"/?");

        });
        var querystring = "";
        $.each(bikeDetails, function (i, val) {
            if (i != bikeDetails.length - 1)
                querystring += ("bike" + (val.index) + "=" + val.versionId + "&");
            else
                querystring += ("bike" + (val.index) + "=" + val.versionId);

        });
        window.location.href = window.location.origin + result + querystring+"&source=19";
    });

    var defaultActiveBoxCount = 2;

    $('.compare-box-list').on('click', '.box-placeholder', function () {
        var listItem = $(this).closest('.list-item'),
            listItemIndex = listItem.index(),
            selectionCount = Number(listItem.closest('.compare-box-list').attr('data-selection-count'));

        var validateClick = false;

        if (listItemIndex >= defaultActiveBoxCount) {
            if (listItemIndex - selectionCount == 0) {
                validateClick = true;
            }
            else {
                return false;
            }
        }
        else {
            validateClick = true;
        }

        if (validateClick) {
            $(listItem).find('.box-placeholder').hide();
            compareBox.setDropdown(listItem[0]);
        }
    });

    $('.compare-box-list').on('click', '.cancel-selected-item', function () {
        var listItem = $(this).closest('.list-item'),
            list = listItem.closest('.compare-box-list')[0];
        if (listItem[0].getAttribute('data-value'))
        bikeDetails.splice(listItem[0].getAttribute('data-value'), 1);
        listItem.remove();
        compareBox.setPlaceholderBox(list);
    });

    function setBikeDetails(listItem, data) {
        var koHTML = '<div data-bind="template: { name: \'bike-template\', data: modelData }"></div>';

        listItem.find('.bike-selection-box').hide();
        listItem.append(koHTML);

        ko.applyBindings(new bikeTemplate(data), listItem[0]);
    };

    var compareBox = {
        classPrefix: 'select-type-',
        dropdownArr: ['brand', 'model', 'version'],

        placeholderTemplate: '<div class="box-placeholder"><span class="box-label"><span class="add-icon"></span><br />Add bike <span class="label-count"></span></span></div>',

        setPlaceholderBox: function (list) {
            var li = document.createElement("li");

            li.className = "list-item cur-disabled";
            li.innerHTML = compareBox.placeholderTemplate;
            list.appendChild(li);

            compareBox.setBikeCount(list, false);
            compareBox.setPlaceholderLabel(list);
        },

        setPlaceholderLabel: function (list) {
            var listItem = list.getElementsByClassName('list-item'),
                labelElement = list.getElementsByClassName('label-count'),
                selectionCount = Number(list.getAttribute('data-selection-count')),
                elementLength = labelElement.length;

            for (var i = selectionCount; i < elementLength; i++) {
                labelElement[i].innerHTML = i + 1;
            }
        },

        setDropdown: function (listItem) {
            var div = document.createElement("div");
            div.className = "bike-selection-box";

            var divTemplate = "",
                dropdownLength = compareBox.dropdownArr.length;

            for (var i = 0; i < dropdownLength; i++) {
                var dropdownLabel = compareBox.dropdownArr[i];

                divTemplate += '<div class="select-box select-box-size-13 select-' + dropdownLabel + '">';
                divTemplate += '<p class="select-label">' + dropdownLabel + '</p>';
                if (i != 0) {
                    divTemplate += '<select class="chosen-select ' + compareBox.classPrefix + dropdownLabel + '" data-title="Select ' + dropdownLabel + '" disabled></select>';
                }
                else {
                    divTemplate += '<select class="chosen-select ' + compareBox.classPrefix + dropdownLabel + '" data-title="Select ' + dropdownLabel + '"></select>';
                }
                divTemplate += '</div>';
            }

            div.innerHTML = divTemplate + '<span class="error-text"></span>';
            listItem.appendChild(div);

            compareBox.getMakes(listItem);
        },

        dropdownChange: function (selectBox, elementValue) {
            selectBox.addClass('done');

            if ($(selectBox).hasClass('select-brand')) {
                compareBox.resetDropdownSelection(selectBox);
                compareBox.getModels(elementValue, selectBox.next()[0]);
            }
            else if ($(selectBox).hasClass('select-model')) {
                compareBox.resetDropdownSelection(selectBox);
                compareBox.getVersions(elementValue, selectBox.next()[0]);
            }
            else if ($(selectBox).hasClass('select-version')) {
                var listItem = selectBox.closest('.list-item'),
                    list = listItem.closest('.compare-box-list')[0];
                compareBox.getVersionData(elementValue, listItem);
               
                if (!isSameVersion )
                {
                    bike1.versionName = $(selectBox).find("option:selected").text();
                    setBikeDetails(listItem, bike1);
                    compareBox.setBikeCount(list, true);
                }
                
                // true flag, to increment count
            }
        },

        setBikeCount: function (list, incrementFlag) {
            var selectionCount = Number(list.getAttribute('data-selection-count')),
                listItem = list.getElementsByClassName('list-item');

            if (!incrementFlag) {
                selectionCount--;
            }
            else {
                selectionCount++;
            }

            list.className = "compare-box-list selection-start";
            list.setAttribute('data-selection-count', selectionCount);

            // if selection count is 0, remove selection start class and increment it by 1 to enable 0th - 1st list item from array in below for loop
            if (!selectionCount) {
                selectionCount++;
                list.className = "compare-box-list";
            }

            var listItemLength = listItem.length;

            for (var i = 0; i < listItemLength; i++) {
                if (i <= selectionCount) {
                    listItem[i].className = "list-item";
                    listItem[i].setAttribute('data-value',i);
                }
                else {
                    listItem[i].className = "list-item cur-disabled";
                    listItem[i].setAttribute('data-value', i);
                }
            }

            compareBox.setSubmitButton(list);
        },

        resetDropdownSelection: function (selectBox) {
            var selectBoxSiblings = $(selectBox).nextAll('.select-box');

            selectBoxSiblings.removeClass('done');
            selectBoxSiblings.find('.chosen-select').empty().prop('disabled', true).trigger('chosen:updated');
        },

        getMakes: function (listItem) {
            var key = "compare_makes";
            if (!compareBox.checkCache(key)) {
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/api/MakeList/?requesttype=2",
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                       
                        bwcache.set({ key: key, value: response.makes, expiryTime: 15 });
                        compareBox.setMakesDropdown(response.makes, listItem);

                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            
                            bwcache.set({ key: key, value: null, expiryTime: 15 });
                            compareBox.resetDropdownSelection(selectBox);
                        }
                    }
                });
            }
            else {
                data = bwcache.get(key);
                compareBox.setMakesDropdown(data, listItem);
            }



        },
        checkCache: function (bKey) {
            if (bwcache.get(bKey)) return true;
            else return false;
        },
        getModels: function (makeId, selectBox) {
            var key = "compare_make_" + makeId;

            if (!compareBox.checkCache(key)) {
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/api/modellist/?requestType=2&makeId=" + makeId,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {
                        
                        bwcache.set({ key: key, value: response.modelList, expiryTime: 15 });
                        compareBox.setModelDropdown(response.modelList, selectBox);
                    },
                    complete: function (xhr) {
                        if (xhr.status == 404 || xhr.status == 204) {
                            
                            bwcache.set({ key: key, value: null, expiryTime: 15 });
                            compareBox.resetDropdownSelection(selectBox);
                        }
                    }
                });
            }
            else {
                data = bwcache.get(key);
                compareBox.setModelDropdown(data, selectBox);
            }
           
        },

        getVersions: function (modelId, selectBox) {
            var key = "compare_versions_" + modelId;
           
            if (!compareBox.checkCache(key)) {
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/api/versionList/?requestType=2&modelId=" + modelId,
                    contentType: "application/json",
                    dataType: 'json',
                    success: function (response) {

                        bwcache.set({ key: key, value: response.Version, expiryTime: 15 });
                        compareBox.setVersionDropdown(response.Version, selectBox);
                    },
                    complete: function (xhr) {
                    if (xhr.status == 404 || xhr.status == 204) {

                        bwcache.set({ key: key, value: null, expiryTime: 15 });
                        compareBox.resetDropdownSelection(selectBox);
                    }
                }
                });
            }
            else {
                data = bwcache.get(key);
                compareBox.setVersionDropdown(data, selectBox);
            }
        },
        getVersionData: function (versionId, selectBox) {
            var key = "compare_versiondetails_" + versionId;
             isSameVersion = false;
            $.each(bikeDetails, function (i, val) {
                if (val.versionId == versionId)
                    isSameVersion = true;
            });
            if (!isSameVersion) {
                selectBox.first().find('.error-text').hide();
                if (!compareBox.checkCache(key)) {
                    $.ajax({
                        type: "Get",
                        async: false,
                        url: "/api/v2/Version/?versionId=" + versionId,
                        contentType: "application/json",
                        dataType: 'json',
                        success: function (response) {
                            bike1.makeName = response.makeDetails.makeName;
                            bike1.modelName = response.modelDetails.modelName;
                            bike1.hostUrl = response.hostUrl;
                            bike1.originalImagePath = response.originalImagePath;
                            bike1.price = compareBox.FormatPrice(response.price);
                            var data = {};
                            data.makemasking = response.makeDetails.maskingName;
                            data.modelmasking = response.modelDetails.maskingName;
                            data.modelId = response.modelDetails.modelId;
                            data.versionId = versionId;
                            data.index = 0;
                            bikeDetails.push(data);
                            selectBox.first().find('.error-text').hide();
                            
                            bwcache.set({ key: key, value: response, expiryTime: 15 });
                        },
                        complete: function (xhr) {
                            if (xhr.status == 404 || xhr.status == 204) {
                                //bwcache.set(key, null, 30);
                                bwcache.set({ key: key, value: null, expiryTime: 30 });
                            }
                        }
                    });
                }
                else {
                    response = bwcache.get(key);
                    bike1.makeName = response.makeDetails.makeName;
                    bike1.modelName = response.modelDetails.modelName;
                    bike1.hostUrl = response.hostUrl;
                    bike1.originalImagePath = response.originalImagePath;
                    bike1.price = compareBox.FormatPrice(response.price);
                    var data = {};
                    data.makemasking = response.makeDetails.maskingName;
                    data.modelmasking = response.modelDetails.maskingName;
                    data.modelId = response.modelDetails.modelId;
                    data.versionId = versionId;
                    data.index = 0;
                    bikeDetails.push(data);

                }
            }
            else {
                selectBox.first().find('.error-text').text("Please choose different bikes for comparison.");
            }

        },
        FormatPrice: function (price) {
            price = price.toString();
            var lastThree = price.substring(price.length - 3);
            var otherNumbers = price.substring(0, price.length - 3);
            if (otherNumbers != '')
                lastThree = ',' + lastThree;
            var price = otherNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree;
            return price;
        },
        setMakesDropdown: function (response, listItem) {
            var dropdownOptions = "<option value></option>",
                responseLength = response.length;

            for (var i = 0; i < responseLength; i++) {
                dropdownOptions += '<option value="' + response[i].makeId + '">' + response[i].makeName + '</option>';
            }

            var dropdownClass = compareBox.classPrefix + compareBox.dropdownArr[0];

            $(listItem).find('.' + dropdownClass).html(dropdownOptions);
            compareBox.initChosen(listItem);
        },

        setModelDropdown: function (response, selectBox) {
            var dropdownOptions = "<option value></option>",
                resoponseLength = response.length;

            for (var i = 0; i < resoponseLength; i++) {
                dropdownOptions += '<option value="' + response[i].modelId + '">' + response[i].modelName + '</option>';
            }

            $(selectBox).find('.chosen-select').html(dropdownOptions);
            compareBox.updateChosen(selectBox);
        },

        setVersionDropdown: function (response, selectBox) {
            var dropdownOptions = "<option value></option>",
                resoponseLength = response.length;

            for (var i = 0; i < resoponseLength; i++) {
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
        },

        setSubmitButton: function (list) {
            var selectionCount = Number($(list).attr('data-selection-count'));

          
                if (selectionCount > 1) {
                    $(bikeComparisonBox).find('.compare-now-btn').show();
                }
                else {
                    $(bikeComparisonBox).find('.compare-now-btn').hide();
                }
            
        }
    }
});