var objBikes = new Object(), focusedMakeModel = null, isMakeModelRedirected = false;
var nextPageUrl;
docReady(function () {
	$("#exploreBikesField").val("");
	function MakeModelPhotosRedirection(items) {
	    var makeMaskingName = items.payload.makeMaskingName;
	    var modelMaskingName = items.payload.modelMaskingName;
	    if (items.payload.modelId > 0 && makeMaskingName != null && makeMaskingName != "" && modelMaskingName != null && modelMaskingName != "") {
	        if (isMobile) {
	            window.location.href = "/m/" + makeMaskingName + "-bikes/" + modelMaskingName + "/images/";
	        }
	        else {
	            window.location.href = "/" + makeMaskingName + "-bikes/" + modelMaskingName + "/images/";
	        }
	        return true;
	    } else if (makeMaskingName != null && makeMaskingName != "") {
	        if (isMobile) {
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
		    if (isMobile) {
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

	function createImageUrl(image) {
	    if (image.originalImgPath.indexOf('?') > 0) {
	        return image.hostUrl + '476x268/' + image.originalImgPath + '&q=70';
	    }
	    else {
	        return image.hostUrl + '476x268/' + image.originalImgPath + '?q=70';
	    }
	}

	function createImagePageUrl(val) {
	    if (isMobile) {
	        return "/m/" + val.MakeBase.maskingName + "-bikes/" + val.ModelBase.maskingName + "/images/";
	    }
	    else {
	        return "/" + val.MakeBase.maskingName + "-bikes/" + val.ModelBase.maskingName + "/images/";
	    }
	}

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
	modelListViewModel = function () {
	    var self = this;
	    self.RedirectLoad = ko.observable(false);
	    self.modelList = ko.observableArray([]);
	    self.IsLoadMore = ko.observable(true);
	    self.Filters = ko.observable({ pageno: '1', pagesize: '30' });
	    self.IsLoading = ko.observable(false);
	    self.FirstPageLoad = ko.observable(true);
	    self.LoadMore = function () {
	        event.preventDefault();
	        self.IsLoading(true);
	        if (self.RedirectLoad()) {
	            nextPageUrl = "/api/images/pages/" + self.Filters()['pageno'] + "/";
	        }
	        else {
	            nextPageUrl = "/api/images/pages/" + (parseInt(self.Filters()['pageno']) + 1).toString() + "/";
	        }
	            
	        $.getJSON(nextPageUrl,
                function (res) {
                    var result = res;
                    if (result.RecordCount > 0) {
                        $.each(result.Models, function (index, val) {
                            var showcasedModelImageList = [];
                            $.each(val.ImagesList, function (index, image) {
                                var img = new modelImage();
                                img.alt = val.ModelBase.modelName + ' ' + 'Images';
                                img.src(createImageUrl(image));
                                showcasedModelImageList.push(img);
                            });
                            var newModel = new model();
                            newModel.modelTitle(val.ModelBase.modelName + ' ' + 'Images');
                            newModel.modelImagePageUrl(createImagePageUrl(val));
                            newModel.makeName(val.MakeBase.makeName);
                            newModel.modelName(val.ModelBase.modelName);
                            newModel.modelImages(showcasedModelImageList);
                            newModel.recordCount(val.recordCount);
                            self.modelList.push(newModel);
                        });
                        if (self.RedirectLoad()) {
                            window.location.hash = 'pageno=' + self.Filters()['pageno'].toString() + '&' + 'pagesize=' + self.Filters()['pagesize'];
                            self.RedirectLoad(false);
                        }
                        else {
                            window.location.hash = 'pageno=' + (parseInt(self.Filters()['pageno'].toString())+1).toString() + '&' + 'pagesize=' + self.Filters()['pagesize'];
                        }
                        var url = window.location.hash.replace('#', '');
                        self.setFilters(url);
                    }
                    if (result.RecordCount < parseInt(self.Filters()['pagesize'])) {
                        self.IsLoadMore(false);
                    }
                    self.IsLoading(false);
                });
	    };

	    self.setFilters = function (url) {
	        try {
	            var params = url.split('&');
	            for (var index in params) {
	                var pair = params[index].split('=');
	                self.Filters()[pair[0]] = pair[1];
	            }
	        } catch (e) {
	            console.warn(e.message);
	        }
	    };
	};
	var viewModel = new modelListViewModel();
	if (window.location.hash) {
	    var url = window.location.hash.replace('#', '');
	    viewModel.FirstPageLoad(false);
	    viewModel.RedirectLoad(true);
	    viewModel.setFilters(url);
	    viewModel.LoadMore();
	}
	ko.applyBindings(viewModel, document.getElementById("exploreModelListing"));
	});