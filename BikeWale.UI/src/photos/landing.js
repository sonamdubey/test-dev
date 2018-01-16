var objBikes = new Object(), focusedMakeModel = null, isMakeModelRedirected = false;
docReady(function () {
	$("#exploreBikesField").val("");
	function MakeModelPhotosRedirection(items) {
	    var makeMaskingName = items.payload.makeMaskingName;
	    var modelMaskingName = items.payload.modelMaskingName;
	    if (items.payload.modelId > 0 && makeMaskingName != null && makeMaskingName != "" && modelMaskingName != null && modelMaskingName != "") {
	        if (isMobile === 'True') {
	            window.location.href = "/m/" + makeMaskingName + "-bikes/" + modelMaskingName + "/images/";
	        }
	        else {
	            window.location.href = "/" + makeMaskingName + "-bikes/" + modelMaskingName + "/images/";
	        }
	        return true;
	    } else if (makeMaskingName != null && makeMaskingName != "") {
	        if (isMobile === 'True') {
	            window.location.href = "/m/" + makeMaskingName + "-bikes/images/";
	        }
	        else {
	            window.location.href = "/" + makeMaskingName + "-bikes/images/";
	        }
	        return true;
	    }
	}
	$("#btnExploreBikesField").on("click", function () {
	    if (focusedMakeModel == undefined || focusedMakeModel == null) {
	        return false;
	    }
	    return MakeModelPhotosRedirection(focusedMakeModel);
	});
	// explore bikes search
	$("#exploreBikesField").bw_autocomplete({
		recordCount: 5,
		source: 8,
		click: function (event, ui, orgTxt) {
		    $("#exploreBikesField").val("");
		    MakeModelPhotosRedirection(ui.item);
		    isMakeModelRedirected = true;
		},
		open: function (result) {
		    $("ul.ui-menu").width($('#exploreBikesField').innerWidth());
		    objBikes.result = result;
		},
		focus: function () {
		    if (isMobile === 'True') {
		        $('html, body').animate({
		            scrollTop: $('#exploreBikesField').offset().top - 20
		        });
		    }
			focusedMakeModel = new Object();
			focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
		},
		focusout: function () {
		    if ($('#exploreBikesField').find('li.ui-state-focus a:visible').text() != "") {
		        focusedMakeModel = new Object();
		        focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
			}
			else {
				$('#errExploreBikes').hide();
			}
		},
		afterfetch: function (result, searchtext) {
		    if (result != undefined && result != null && result.length > 0 && searchtext.trim()) {
				$('#errExploreBikes').hide();
			}
		    else {
		        focusedMakeModel = null;
				if (searchtext.trim() != '') {
					$('#errExploreBikes').show();
				}
			}
		},
		keyup: function () {
		    if ($('#exploreBikesField').val().trim() != '' && $('li.ui-state-focus a:visible').text() != "") {
		        focusedMakeModel = new Object();
		        focusedMakeModel = objBikes.result ? objBikes.result[$('li.ui-state-focus').index()] : null;
		        $('#errExploreBikes').hide();
		    } else {
		        if ($('#exploreBikesField').val().trim() == '') {
		            $('#errExploreBikes').hide();
		        }
		    }

		    if ($('#exploreBikesField').val().trim() == '' || e.keyCode == 27 || e.keyCode == 13) {
		        if (focusedMakeModel == null || focusedMakeModel == undefined) {
		            if ($('#exploreBikesField').val().trim() != '') {
		                $('#errExploreBikes').show();
		            }
		        }
		        else {
		            $('#errExploreBikes').hide();
		        }

		    }
		}
	}).keydown(function (e) {
	    if (e.keyCode === 13) {
	        if (!isMakeModelRedirected)
	            $('#btnExploreBikesField').click();
	        else
	            isMakeModelRedirected = false;
	    }

	}).autocomplete("widget").addClass("bike-images-autocomplete")

	// body type filter
	$('#filterBodyType').on('click', '.body-type__item', function() {
		if(!$(this).hasClass('active')) {
			$(this).siblings('.body-type__item').removeClass('active');
			$(this).addClass('active');
		}
		else {
			$(this).removeClass('active');
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
				if (isMobile) {
					$('html, body').animate({
						scrollTop: collapsibleContent.offset().top
					}, 500);
				}
				break;

			default:
				break;
		}
	});
	$("#viewMoreBtn").click(function () {
	    event.preventDefault();
	});
	var modelImage = function () {
	    this.src = ko.observable();
	    this.alt = ko.observable();
	};
	var model = function () {
	    var self = this;
	    self.modelTitle = ko.observable();
	    self.modelImagePageUrl = ko.observable();
	    self.makeName = ko.observable();
	    self.modelName = ko.observable();
	    self.modelImages = ko.observableArray([]);
	    self.showcasedModelImages = ko.computed(function () {
	        if (self.modelImages().length === 2) {
	            return self.modelImages().splice(0, 1);
	        }
	        else {
	            return self.modelImages();
	        }
	    });
	    self.recordCount = ko.observable();
	    self.gridSize = ko.computed(function () {
	        return self.recordCount() >= 4 ? 4 : self.recordCount() >= 3 ? 3 : 1;
	    });
	};
	var modelListViewModel = function () {
	    var self = this;
	    self.modelList = ko.observableArray([]);
	    self.isLoadMore = ko.observable(true);
	    self.LoadMore = function () {
	        event.preventDefault();
	        console.log("Done");
	        var nextPageUrl = "/api/images/pages/2/30";
	        $.getJSON(nextPageUrl,
                function (res) {
                    var result = res;
                    if (result.RecordCount > 0) {
                        $.each(result.ModelsImages, function (index, val) {
                            var showcasedModelImageList = [];
                            $.each(val.ImagesList, function (index, image) {
                                var img = new modelImage();
                                img.alt = val.ModelBase.modelName + ' ' + 'Images';
                                if (image.originalImgPath.indexOf('?') > 0) {
                                    img.src = image.hostUrl + '476x268/' + image.originalImgPath + '&q=70';
                                }
                                else {
                                    img.src = image.hostUrl + '476x268/' + image.originalImgPath + '?q=70';
                                }
                                showcasedModelImageList.push(img);
                            });
                            var newModel = new model();
                            newModel.modelTitle(val.ModelBase.modelName + ' ' + 'Images');
                            if (isMobile === 'True') {
                                newModel.modelImagePageUrl("/m/" + val.MakeBase.maskingName + "-bikes/" + val.ModelBase.maskingName + "/images/");
                            }
                            else {
                                newModel.modelImagePageUrl("/" + val.MakeBase.maskingName + "-bikes/" + val.ModelBase.maskingName + "/images/");
                            }
                            newModel.makeName(val.MakeBase.makeName);
                            newModel.modelName(val.ModelBase.modelName);
                            newModel.modelImages(showcasedModelImageList);
                            newModel.recordCount(val.recordCount);
                            self.modelList.push(newModel);
                        });
                    }
                    else {
                        self.isLoadMore(false);
                    }
                });
	    };
	};
	    ko.applyBindings(new modelListViewModel(), document.getElementById("exploreModelListing"));
	});