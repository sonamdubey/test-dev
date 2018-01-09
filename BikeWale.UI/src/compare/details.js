var floatingCard = $('#comparison-floating-card'),
    floatingCardHeight = floatingCard.height() - 44,
    comparisonFooter = $('#comparison-footer'),
    overallSpecsTabs = $('#overall-tabs'),
    $window = $(window),
    windowScrollTop,
    hideCheckbox = $(".hideCheckbox");
var data = {};

docReady(function() {
    $('.chosen-select').chosen();
    var bikeDetails = new Array();
    var basicInfo = JSON.parse(Base64.decode(document.getElementById("bike-comparison-grid").getAttribute("data-basicInfo")));

    $.each(basicInfo, function (i, val) {
        if (Number(document.getElementById("bike-comparison-grid").getAttribute("data-sponseredId")) != val.VersionId) {
            var data = {};
            data.makemasking = val.MakeMaskingName;
            data.modelmasking = val.ModelMaskingName;
            data.modelId = val.ModelId;
            data.versionId = val.VersionId;
            data.index = 0;

            bikeDetails.push(data);
        }
    });
    selectDropdownBox = $('.select-box-no-input');

    selectDropdownBox.each(function () {
        var text = $(this).find('.chosen-select').attr('data-title'),
            searchBox = $(this).find('.chosen-search')

        searchBox.empty().append('<p class="no-input-label">' + text + '</p>');
    });

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
    $('#bike-comparison-container').on('click', '#btnCompare', function ()  {
        getUrl();
    });
    
    $('.compare-bike-list').on('click', '.cancel-selected-item', function () {
        var listItem = $(this).closest('.list-item'),
            listItemIndex = listItem.index();
    
        var bikeList = $('.compare-bike-list');
        for(var i = 0; i < bikeList.length; i++) {
            $(bikeList[i]).find('li:eq('+ listItemIndex +')').remove();
        }
        if (listItem[0].getAttribute('data-value') && Number(listItem[0].getAttribute('data-value')) + 1 <= bikeDetails.length) {
            bikeDetails.splice(Number(listItem[0].getAttribute('data-value')), 1);
            if(bikeDetails.length>1)
            getUrl();
        }
            
        var mainList = $('#bike-comparison-grid').find('.compare-bike-list');

        compareBox.emptyTableData(listItemIndex);
        compareBox.checkForPlaceholderBox(mainList);

        compareBox.resizeColumn(mainList[0]);
        compareBox.setSponsoredIndex();

    });
    var getUrl = function ()
    {
        if(data!=null)
        bikeDetails.push(data);
        var queryStringSponser = "";
        if (/sponsoredbike/g.test(window.location.search))
        {
            var index = -1;
            var obj = queryStringSponser.split("&");
            $.each(obj, function (i, val) {
                if (val.match(/sponsoredbike/g))
                    index = i;

            });
            if (index > -1)
            {
                queryStringSponser = index > 0 ? "&" + obj[index] : "&sponsoredbike=" + obj[index].split("=")[1];

            }


        }

        $.each(bikeDetails, function (i, val) {
            val.index = i + 1;

        });
        bikeDetails.sort(function (a, b) {
                return a.modelId - b.modelId;
            
        });
        var result = "";
        $.each(bikeDetails, function (i, val) {
            if (i != bikeDetails.length - 1)
                result += (val.makemasking + "-" + val.modelmasking + "-vs-");
            else
                result += (val.makemasking + "-" + val.modelmasking + "/?");

        });
        var querystring = "";
        $.each(bikeDetails, function (i, val) {
            if (i != bikeDetails.length - 1)
                querystring += ("bike" + (val.index) + "=" + val.versionId + "&");
            else
                querystring += ("bike" + (val.index) + "=" + val.versionId);

        });
        window.location = '/comparebikes/' + result + querystring + queryStringSponser + "&source=" + '7';

    }
    $('.compare-bike-list').on('click', '.add-bike-form', function () {
        var listItem = $(this).closest('.list-item');
        $(this).hide();
        compareBox.setDropdown(listItem[0]);
        var bikeNo = listItem.attr("data-add-value"), liFloatingBike = $(".floating-add-compare-btn").closest("li[data-add-value=" + bikeNo + "]"),
        floatingBtn = liFloatingBike.find(".floating-add-compare-btn");
        floatingBtn.attr("data-selection-done", 1);
        floatingBtn.text("Choose bikes");
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
                var dataRows = $(".hide-features").find(".row-type-data");

                compareBox.detectEquivalentData(dataRows);
                equivalentDataFound = true;
            }

            compareBox.setFeaturesStatus(btn, 'Show all features', 1); 
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

        addToCompareButton: '<div class="text-center margin-top5"><button id="btnCompare" class="btn btn-white btn-160-32 hide">Add to compare</button></div> <span class="error-text"></span>',

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
            
            var listItemIndex = $('#bike-comparison-grid .compare-bike-list .sponsored-list-item').index() + 1;

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
            var sameversion=false;
            if($(selectBox).hasClass('select-brand')) {
                compareBox.resetDropdownSelection(selectBox);
                compareBox.getModels(elementValue, selectBox.next()[0]);
            }
            else if($(selectBox).hasClass('select-model')) {
                compareBox.resetDropdownSelection(selectBox);
                compareBox.getVersions(elementValue, selectBox.next()[0]);
            }
            else if ($(selectBox).hasClass('select-version')) {
                var listItem = selectBox.closest('.list-item'),
                    list = listItem.closest('.compare-box-list')[0];
                if (listItem[0].getAttribute('data-value') && bikeDetails[Number(listItem[0].getAttribute('data-value'))]!=null&&bikeDetails[Number(listItem[0].getAttribute('data-value'))].versionId != elementValue) {
                    $.each(bikeDetails, function (i, val) {
                        if (val.versionId == elementValue)
                            sameversion = true;

                    });
                    if (!sameversion) {
                       
                        compareBox.getVersionData(elementValue, listItem);
                        getUrl();
                    }
                    else {
                        listItem.first().find('.error-text').text("Please choose different bikes for comparison.");
                        listItem.first().find('.error-text').show();
                    }
                }
                else {
                    if (JSON.parse(Base64.decode(document.getElementById("bike-comparison-grid").getAttribute("data-basicInfo"))).length-1 == listItem[0].getAttribute('data-value')) {
                        var currentURL = window.location.href;
                        var queryString = window.location.search;
                        if (queryString != "") {
                            var index = 0;
                            var obj = queryString.split("&");
                            $.each(obj, function (i, val) {
                                if (val.match(/sponsoredbike/g))
                                    index = i;
                                    
                            });
                            if (index > 0)
                                obj[index] = "sponsoredbike=" + elementValue;
                            else
                                obj.push("sponsoredbike=" + elementValue);

                            currentURL = window.location.pathname + obj.join('&');

                        }
                           
                        else
                            currentURL += "?sponsoredbike=" + elementValue;
                        window.location = currentURL;
                    }
                    else {
                        listItem.first().find('.error-text').hide();
                    compareBox.getVersionData(elementValue, listItem);

                    } 
                }
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
                url: "/api/MakeList/?requesttype=2",
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
                url: "/api/modellist/?requestType=2&makeId=" + makeId,
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
        getVersionData: function (versionId, selectBox) {
            isSameVersion = false;
            $.each(bikeDetails, function (i, val) {
                if (val.versionId == versionId)
                    isSameVersion = true;
            });
            if (!isSameVersion) {
                selectBox.first().find('.error-text').hide();
                    $.ajax({
                        type: "Get",
                        async: false,
                        url: "/api/v2/Version/?versionId=" + versionId,
                        contentType: "application/json",
                        dataType: 'json',
                        success: function (response) {
                             data = {};
                            data.makemasking = response.makeDetails.maskingName;
                            data.modelmasking = response.modelDetails.maskingName;
                            data.modelId = response.modelDetails.modelId;
                            data.versionId = versionId;
                            data.index = 0;
                            if (selectBox[0].getAttribute('data-value'))
                                bikeDetails[Number(selectBox[0].getAttribute('data-value'))] = data;
                           
                            selectBox.first().find('.error-text').hide();
                            selectBox.first().find('#btnCompare').show();
                        }
                       
                    });
             
            }
            else {
                selectBox.first().find('#btnCompare').hide();
                selectBox.first().find('.error-text').text("Please choose different bikes for comparison.");
                selectBox.first().find('.error-text').show();

            }

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

    if ($(".sponsored-list-item") && $(".sponsored-list-item").length > 0) {
        var sponsoredBike = $(".sponsored-list-item").data("bikename");
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Comparison_Page", "act": "Sponsored_Version_Shown", "lab": sponsoredBike });
    }

    if ($(".know-more-btn-shown") && $(".know-more-btn-shown").length > 0) {
        var sponsoredBike = $(".sponsored-list-item").data("bikename");
        dataLayer.push({ "event": "Bikewale_noninteraction", "cat": "Comparison_Page", "act": "Sponsored_Comparison_Know_more_shown", "lab": sponsoredBike });
    }

    var panel = $('#overallSpecsTabContainer'),
        floatingTabs = panel.find('.overall-specs-tabs-wrapper');
    $('.overall-specs-tabs-wrapper').on('click', 'li', function () {
        var elementIndex = $(this).index(),
            tabId = $(this).attr('data-tabs');
        if (elementIndex < 4) {
            $('html, body').animate({ scrollTop: Math.round($('.bw-tabs-data[data-id=' + tabId+']').offset().top - (floatingCardHeight + 48)) }, 500);
        }
        else {
            $('html, body').animate({ scrollTop: Math.round($('.bw-tabs-data[data-id=' + tabId + ']').offset().top - 40) }, 500);
        }
    });
    var windowHeight = $window.height();

    $window.on('scroll', function () {
        var overallSpecsOffset = overallSpecsTabs.offset().top - floatingCardHeight,
            bwTab = overallSpecsTabs.offset().top - floatingCardHeight,
                    footerOffsetForCard = Math.round($('.bw-tabs-panel').height() - overallSpecsOffset);

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
        if (windowScrollTop > overallSpecsOffset) {
            floatingCard.addClass('fixed-card');
            floatingCard.find('.overall-specs-tabs-wrapper').removeClass('fixed-overall-tab');
            if (windowScrollTop > (footerOffsetForCard + overallSpecsOffset)) {
                floatingCard.removeClass('fixed-card');
                floatingCard.find('.overall-specs-tabs-wrapper').addClass('fixed-overall-tab');
            }
        }
        else if (windowScrollTop < overallSpecsOffset) {
            floatingCard.removeClass('fixed-card');
        }
        $('#overallSpecsTabContainer .bw-tabs-data').each(function () {
            var top, bottom;
            if ($(this).index() != 0) {
                top = $(this).offset().top - (floatingCardHeight + 50);
            }
            else {modelSimilarContent
                top = $(this).offset().top - 44;
            }
            bottom = top + $(this).outerHeight();
            if (windowScrollTop >= top && windowScrollTop <= bottom) {
                floatingTabs.find('li').removeClass('active');
                $('#overallSpecsTabContainer .bw-tabs-data').removeClass('active');

                $(this).addClass('active');

                var currentActiveTab = floatingTabs.find('li[data-tabs="' + $(this).attr('data-id') + '"]');
                floatingTabs.find(currentActiveTab).addClass('active');
            }
        });

    });
    $(".floating-add-compare-btn").on('click', function () {
        var ele = $(this), isSelectionDone = ele.attr("data-selection-done");
        var bikeNo = ele.closest("li.list-item").attr("data-add-value"), liBike = $(".add-compare-btn").closest("li[data-add-value=" + bikeNo + "]");
        $('html, body').animate({ scrollTop: 0 }, 500);
        if (!isSelectionDone)
        {
            liBike.find(".add-compare-btn").click();
            ele.attr("data-selection-done", 1);
            ele.text("Choose bikes");
        }
       
    });
    
    $(".reviewTab").on('click', function () {
        hideCheckbox.hide();
    })

    $(".quickAcessTab").on('click', function () {
        if (hideCheckbox.is(":hidden"))
        {
            hideCheckbox.show();
        }
    })

    $(document).on('click', '#modelSimilarContent .jcarousel-control-right , #modelSimilarContent .jcarousel-control-left', function () {  
        triggerGA("Compare_Bikes", "Clicked_on_carousel", $("#modelSimilarContent").data('comptext'));
    });
});