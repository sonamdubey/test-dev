var newBikeSearch, newBikeSearchVM;

var appendText = $(".filter-select-title"),
	currentList = $(".filter-selection-div"),
	liList = $(".filter-selection-div ul li"),
	liToggelFilter = $(".bw-tabs-new li"),
	defaultText = $(".default-text"),
	sortCriteria = $('#sort'),
	sortByDiv = $(".sort-div"),
	sortListDiv = $(".sort-selection-div"),
	sortListLI = $(".sort-selection-div ul li");
var arr, a, p, minDataValue, maxDataValue;
var count = 0, counter = 0;
var searchUrl = "";
var minAmount = $(".minAmount");
var maxAmount = $(".maxAmount");
var maxInputVal = "";
var minList = $("#minList");
var maxList = $("#maxList");
var minInput = $("#minInput");
var maxInput = $("#maxInput");
var budgetListContainer = $("#budgetListContainer");

arr = [
{ amount: "0", value: 0 },
{ amount: "30K", value: 30000 },
{ amount: "40K", value: 40000 },
{ amount: "50K", value: 50000 },
{ amount: "60K", value: 60000 },
{ amount: "70K", value: 70000 },
{ amount: "80K", value: 80000 },
{ amount: "90K", value: 90000 },
{ amount: "1L", value: 100000 },
{ amount: "1.5L", value: 150000 },
{ amount: "2L", value: 200000 },
{ amount: "2.5L", value: 250000 },
{ amount: "3L", value: 300000 },
{ amount: "3.5L", value: 350000 },
{ amount: "5L", value: 500000 },
{ amount: "7.5L", value: 750000 },
{ amount: "10L", value: 1000000 },
{ amount: "12.5L", value: 1250000 },
{ amount: "15L", value: 1500000 },
{ amount: "30L", value: 3000000 },
{ amount: "60L", value: 6000000 }
];
$.pageNo = "";
$.so = "";
$.sc = "";
$.nextPageUrl = "";
$.totalCount;
$.lazyLoadingStatus = true;
var $window = $(window),
	$menu = $('#filter-container'),
	menuTop = $menu.offset().top,
	$searchList = $('#searchList');

var stateChangeDown = function (allDiv, clickedDiv) {
	allDiv.removeClass("open");
	allDiv.next(".filter-selection-div").slideUp();
	clickedDiv.addClass("open");
	clickedDiv.next(".filter-selection-div").show().css({ "overflow": "inherit" });
	clickedDiv.next(".filter-selection-div").addClass("open");
};

var stateChangeUp = function (allDiv, clickedDiv) {
	allDiv.removeClass("open");
	allDiv.next(".filter-selection-div").slideUp();
	clickedDiv.removeClass("open");
	clickedDiv.next(".filter-selection-div").slideUp();
	clickedDiv.next(".filter-selection-div").removeClass("open");
};

var resetBWTabs = function () {
	$(".bw-tabs-new li").removeClass("active");
};

var moreLessTextChange = function (p) {
	var morelessFilter = $("#more-less-filter-text");
	var q = p.find(morelessFilter);
	q.text(q.text() === "More" ? "Less" : "More");
};

docReady(function () {
	//Other  functions
	newBikeSearch = function () {
		var self = this;
		self.IsInitialized = ko.observable(false);
		self.IsLoading = ko.observable(false);
		self.searchResult = ko.observableArray([]);
		self.Filters = ko.observable({ pageno: 1, pagesize: 30 });
		self.PreviousQS = ko.observable("");
		self.IsReset = ko.observable(false);
		self.LoadMoreTarget = ko.observable();
		self.IsLoadMore = ko.observable(false);
		self.NewPageNo = ko.observable(0);
		self.IsMoreBikesAvailable = ko.observable(false);
		self.FirstLoad = ko.observable(true);
		self.models = ko.observable([]);
		self.TotalBikes = ko.observable();
		self.noBikes = ko.observable(self.TotalBikes() == 0);
		self.curPageNo = ko.observable();
		self.Filters()['budget'] = $('#min-max-budget').val();
		self.init = function (e) {
			if (!self.IsInitialized()) {
				self.IsLoading(true);
				self.TotalBikes(1); //handle container for loader
				if (self.FirstLoad()) {
					var bikeSearchResult = JSON.parse(Base64.decode($('#pageLoadData').val()));
					self.models(bikeSearchResult.searchResult);
					self.LoadMoreTarget(bikeSearchResult.pageUrl.nextUrl);
					if (self.LoadMoreTarget()) {
						self.IsMoreBikesAvailable(true);
					}
				}
				self.FirstLoad(false);
				var eleSection = $(".search-bike-list");
				ko.applyBindings(self, eleSection[0]);
				self.IsInitialized(true);
				self.IsLoading(false);
			}
		};

		self.setFilters = function (url) {
			try {
				count = 0;
				var params = url.split('&');
				for (var index in params) {
					var pair = params[index].split('=');
					self.Filters()[pair[0]] = pair[1];
					var node = $('div[name=' + pair[0] + ']');
					if (pair[0] !== 'budget') {
						var values = pair[1].split('+'),
							selText = '';

						for (var j = 0; j < values.length; j++) {
							node.find('li[filterid=' + values[j] + ']').addClass('active');
							selText += node.find('li[filterid=' + values[j] + ']').text() + ', ';
						}
						count++;
						node.find('ul').parent().prev(".filter-div").find('.filter-select-title .default-text').text(selText.substring(0, selText.length - 2));
					} else {
						var values = pair[1].split('-');
						self.setMaxAmount(values[1]);
						self.setMinAmount(values[0]);
						count++;
					}
				}
				$('.filter-counter').text(count);
				self.FirstLoad(false);
				self.getBikeSearchResult('through-filters');
				self.init();
				self.Filters()['pageno'] = 1;
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.bindNextSearchResult = function (json) {
			try {
				if (json.searchResult.length > 0) {
					$.each(json.searchResult, function (index, val) {
						self.searchResult.push(val);
					});
				}
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.pushGTACode = function (noOfRecords, filterName) {
			dataLayer.push({ 'event': 'Bikewale_all', 'cat': 'Search_Page', 'act': 'Filter_Select_' + noOfRecords, 'lab': filterName });
		};

		self.setMinAmount = function (userMinAmount) {
			try {
				if (userMinAmount == "") {
					minInput.val("").attr("data-value", "");
					minAmount.html("0");
				}
				else {
					$("#budgetBtn").hide();
					var formattedValue = $.newUserInputPrice(userMinAmount);
					minAmount.text(formattedValue);
					minInput.val(formattedValue).attr('data-value', userMinAmount);
				}
				if ($("#budgetBtn").is(':visible'))
					minAmount.html("");
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.setMaxAmount = function (userMaxAmount) {
			try {
				if (e.keyCode == 8 && userMaxAmount.length == 0)
					maxAmount.html(" - MAX");

				if (userMaxAmount.length == 0 && $('#budgetBtn').not(':visible')) {
					maxInput.val("").attr("data-value", "");
					maxAmount.html("- MAX");
				}
				else {
					$("#budgetBtn").hide();
					var userMinAmount = minInput.val();
					if (userMinAmount == "")
						minAmount.html("0");

					var formattedValue = $.newUserInputPrice(userMaxAmount);
					maxAmount.html("- " + formattedValue);
					maxInput.val(formattedValue).attr('data-value', userMaxAmount);
				}
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.getSelectedQSFilterText = function () {
			try {
				count = 0;
				$('.bw-tabs-new').find('li').each(function () {
					$(this).removeClass('active');
				});
				$('.filter-select-title .default-text').each(function () {
					$(this).text($(this).prev().text());
				});
				for (var param in self.Filters()) {
					if (param) {
						var node = $('div[name=' + param + ']');
						if (param != 'pageno' && param != 'pagesize' && param != 'so' && param != 'sc' && param != 'budget') {
							var values = self.Filters()[param].split('+'),
								selText = '';

							for (var j = 0; j < values.length; j++) {
								node.find('li[filterid=' + values[j] + ']').addClass('active');
								selText += node.find('li[filterid=' + values[j] + ']').text() + ', ';
							}
							count++;
							if (selText.length > 2)
								node.find('ul').parent().prev(".filter-div").find('.filter-select-title .default-text').text(selText.substring(0, selText.length - 2));
						} else if (param == 'budget') {
							var values = self.Filters()[param].split('-');
							self.setMaxAmount(values[1]);
							self.setMinAmount(values[0]);
							count++;
						}
					}
				}
				$('.filter-counter').text(count);
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.getBikeSearchResult = function (filterName) {
			try {
				var qs = "";
				if (self.IsLoadMore()) {
					qs = self.LoadMoreTarget().split('?')[1].toLowerCase();
				} else {
					for (var param in self.Filters()) {
						if (self.Filters()[param]) {
							qs += "&" + param + "=" + self.Filters()[param];
						}
					}
					qs = qs.substr(1);
				}

				if (self.PreviousQS() != qs) {
					if (!self.IsLoadMore()) {
						self.models([]);
					}

					if (self.noBikes()) {
						self.TotalBikes(1); // to show bikes container
						self.noBikes(false);
					}
					self.IsLoading(true);
					self.PreviousQS(qs);
					var apiUrl = '/api/NewBikeSearch/?' + qs;
					$.getJSON(apiUrl)
						.done(function (response) {
							if (self.IsLoadMore()) {
								self.bindNextSearchResult(response);
								self.NewPageNo(self.NewPageNo() + 1);
							}
							else {
								self.models(response.searchResult);
								self.NewPageNo(0);
								self.searchResult([]);
							}
							self.TotalBikes(response.totalCount);
							$('#bikecount').text(self.TotalBikes() + ' Bikes');
							self.noBikes(false);
							self.LoadMoreTarget(response.pageUrl.nextUrl);
							if (response.pageUrl.nextUrl) {
								self.IsMoreBikesAvailable(true);
							} else {
								self.IsMoreBikesAvailable(false);
							}
							if (filterName) {
								self.pushGTACode(self.TotalBikes(), filterName);
							}
						})
						.fail(function () {
							self.IsMoreBikesAvailable(false);
							self.noBikes(true);
							self.TotalBikes(0);
							if (filterName) {
								self.pushGTACode(self.TotalBikes(), filterName);
							}
							self.searchResult([]);
							self.models([]);
							$('#bikecount').text('No bikes found');
							$('#nobikeresults').show();
						})
						.always(function () {
							window.location.hash = qs;
							self.IsLoading(false);
							self.getSelectedQSFilterText();
							self.IsLoadMore(false);
						});
				}
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.validateInputValue = function () {
			try {
				minDataValue = parseInt(minInput.attr("data-value")) || 0;
				maxDataValue = parseInt(maxInput.attr("data-value")) || 0;
				var isValid = false;
				if (minDataValue <= maxDataValue) {
					maxInput.removeClass("border-red");
					$(".bw-blackbg-tooltip-max").hide();
					isValid = true;
				}
				else if (maxDataValue > 0 && minDataValue > maxDataValue) {
					maxInput.addClass("border-red");
					$(".bw-blackbg-tooltip-max").show();
					isValid = false;
				}
				else if (maxDataValue == 0)
					isValid = true;
				return isValid;
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.removeValueFromCheckBoxInQS = function (name, value) {
			try {
				var values = self.Filters()[name].split('+');
				var temp = '';
				for (var i = 0; i < values.length; i++) {
					if (values[i] != value) {
						if (temp.length > 0)
							temp = temp + "+" + values[i];
						else
							temp = values[i];
					}
				}
				self.Filters()[name] = temp;
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.getFilterFromQS = function (name) {
			try {
				if (self.Filters()[name]) {
					return self.Filters()[name]
				} else {
					return "";
				}
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.updateCheckBoxFilterInQS = function (name, value, toAdd) {
			try {
				if (toAdd == true) {
					self.Filters()[name] = self.Filters()[name] + '+' + value;
				}
				else {
					self.removeValueFromCheckBoxInQS(name, value);
				}
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.applyCheckBoxFilter = function (name, value, node) {
			try {
				var checked = 'active';
				if (node.hasClass(checked)) {
					node.removeClass(checked);
					self.updateCheckBoxFilterInQS(name, value, false);
				}
				else {
					node.addClass(checked);
					if (self.Filters()[name]) {
						self.updateCheckBoxFilterInQS(name, value, true);
					}
					else {
						self.Filters()[name] = value;
					}
				}
				self.getBikeSearchResult(name);
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.applyToggelFilter = function (name, value, node) {
			try {
				var checked = 'active';

				if (!node.find('li[filterid=' + value + ']').hasClass(checked)) {
					node.removeClass(checked);
					self.Filters()[name] = value;
				}
				else {
					delete self.Filters()[name];
				}

				self.getBikeSearchResult(name);
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.applyMinMaxFilter = function (name, value, node) {
			try {
				if (name && value) {
					self.Filters()[name] = value;
				}
				else {
					delete self.Filters()[name];
				}

				self.getBikeSearchResult(name);
			} catch (e) {
				console.warn(e.message);
			}
		};
		self.applySortFilter = function (so, sc, sortByText) {
			try {
				self.Filters()['so'] = so;
				self.Filters()['sc'] = sc;
				self.getBikeSearchResult(sortByText);
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.updateFilters = function (node, name, value, type) {
			try {
				if (type == 1)
					self.applyCheckBoxFilter(name, value, node);
				else if (type == 2)
					self.applyToggelFilter(name, value, node);
				else if (type == 5)
					self.applyMinMaxFilter(name, value, node);
			} catch (e) {
				console.warn(e.message);
			}
		};

		self.resetAll = function () {
			try {
				$("span.selected").remove();
				$(".filter-selection-div li").each(function () {
					$(this).removeClass("active").addClass("uncheck");
				});
				$('.filter-select-title .default-text').each(function () {
					$(this).text($(this).prev().text());
				});
				$('#minInput').val('').attr("data-value", '');
				$('#maxInput').val('').attr("data-value", '');
				minAmount.text('');
				maxAmount.text('');
				defaultText.show();
				count = 0;
				resetBWTabs();
				var a = $(".more-filters-btn");
				if (a.hasClass("open"))
					moreLessTextChange(a);
				$(".more-filters-btn").removeClass("open");
				$(".more-filters-container").slideUp();
				$('.filter-counter').text(count);
				for (var param in self.Filters()) {
					if (param && param != 'pageno' && param != 'pagesize' && param != 'so' && param != 'sc') {
						delete self.Filters()[param];
					}
				}
				self.getBikeSearchResult("resetButton");
			} catch (e) {
				console.warn(e.message);
			}
		}
	};

	newBikeSearchVM = new newBikeSearch();


	$('#loadMoreBikes').click(function (e) {
		if (newBikeSearchVM && !newBikeSearchVM.IsInitialized() ) {
			newBikeSearchVM.init(e);
		}
		newBikeSearchVM.LoadMoreTarget($("#loadMoreBikes").attr("data-url"));
		newBikeSearchVM.IsLoadMore(true);
		newBikeSearchVM.getBikeSearchResult('load-more');
	});

	$(".filter-div").on("click", function () {
		var allDiv = $(".filter-div");
		var clickedDiv = $(this);
		if (!clickedDiv.hasClass("open")) {
			stateChangeDown(allDiv, clickedDiv);
			//$(".more-filters-container").slideUp();
			$.sortChangeUp(sortByDiv);
		}
		else {

			stateChangeUp(allDiv, clickedDiv);
		}
	});

	$(".more-filters-btn").click(function () {
		if (!$(this).hasClass("open")) {
			$(this).addClass("open");
			$(".more-filters-container").show().css({ "overflow": "inherit" });
			var a = $(".filter-div");
			a.removeClass("open");
			a.next(".filter-selection-div").slideUp();
			a.next(".filter-selection-div").removeClass("open");
			moreLessTextChange($(this));
		}
		else {
			$(this).removeClass("open");
			$(".more-filters-container").slideUp();
			moreLessTextChange($(this));
		}
	});

	$(".filter-done-btn").click(function () {
		$(".more-filters-container").slideUp();
		stateChangeUp($('.filter-div'), $('.filter-div'));
		var a = $(".more-filters-btn");
		moreLessTextChange(a);
		$(".more-filters-btn").removeClass("open");
	});

	$("#btnReset").click(function () {

		newBikeSearchVM.resetAll();
		if (newBikeSearchVM && !newBikeSearchVM.IsInitialized()) {
			newBikeSearchVM.init(e);
		}
	});

	$window.scroll(function () {
		if ($window.scrollTop() > menuTop) {
			$menu.addClass('stick');

			if ($window.scrollTop() > $searchList.height()) {
				$menu.removeClass('stick');
			}
		}
		else {
			$menu.removeClass('stick');
		}
	});

	$.fn.onCheckBoxClick = function () {
		return this.click(function () {
			var clickedLI = $(this);
			var name = $(this).parents().find('.filter-selection-div').attr('name');
			var clickedLIText = clickedLI.text();
			var fid = clickedLI.attr("filterId");
			var a = clickedLI.parent().parent().prev(".filter-div");
			newBikeSearchVM.updateFilters(clickedLI, name, fid, 1);
		});
	};

	$.fn.onToggelFilterClick = function () {
		return this.click(function () {
			var panel = $(this).closest(".bw-tabs-panel");

			newBikeSearchVM.updateFilters(panel, $(this).parent().parent().parent().parent().attr('name'), $(this).attr('filterid'), 2);
		});
	};

	liList.onCheckBoxClick();

	liToggelFilter.onToggelFilterClick();

	$.valueFormatter = function (num) {
		if (num >= 100000) {
			return (num / 100000).toFixed(1).replace(/\.0$/, '') + 'L';
		}
		if (num >= 1000) {
			return (num / 1000).toFixed(1).replace(/\.0$/, '') + 'K';
		}
		return num;
	}

	$.generateMaxList = function (dataValue) {
		counter = 0;
		maxList.empty();
		dataValue = parseInt(dataValue);
		for (p in arr) {
			var a = arr[p].value;
			if (dataValue < a && counter <= 8) {
				maxList.append("<li data-value=" + arr[p].value + ">" + arr[p].amount + "</li>");
				counter++;
			}
		}
	};

	sortByDiv.click(function () {
		if (!sortByDiv.hasClass("open"))
			$.sortChangeDown(sortByDiv);
		else
			$.sortChangeUp(sortByDiv);
	});

	$.sortChangeDown = function (sortByDiv) {
		sortByDiv.addClass("open");
		sortListDiv.show();
	};

	$.sortChangeUp = function (sortByDiv) {
		sortByDiv.removeClass("open");
		sortListDiv.slideUp();
	};

	$.fn.applySortFilter = function () {
		return $(this).click(function () {
			var node = $(this);
			sortListLI.removeClass('selected');
			node.addClass('selected');
			var sortByText = $(this).text();
			$(".sort-by-title").find(".sort-select-btn").html(sortByText);
			$.sortChangeUp(sortByDiv);

			var so = node.attr('so');
			var sc = node.attr('sc');
			newBikeSearchVM.applySortFilter(so, sc, sortByText);    
		});
	};

	sortListLI.applySortFilter();

	$(document).mouseup(function (e) {
		var filterDivContainer = $(".filter-div");
		var filterDivTitle = $(".filter-select-title");
		var filterSelectedText = $(".filter-select-title span.selected");
		var filterArrow = $(".fa-angle-down");
		var filterSelectBtn = $(".filter-select-btn");
		var filterSelectionDiv = $(".filter-selection-div");
		var filterSelectionUL = $(".filter-selection-div ul");
		var filterSelectionList = $(".filter-selection-div ul li");
		var filterSelectionLIText = filterSelectionList.find("span");
		if (filterSelectionDiv.hasClass("open")) {
			if (!filterDivContainer.is(e.target) && !filterDivTitle.is(e.target) && !filterSelectedText.is(e.target) && !filterArrow.is(e.target) && !filterSelectBtn.is(e.target) && !filterSelectionUL.is(e.target) && !filterSelectionList.is(e.target) && !filterSelectionLIText.is(e.target)) {
				filterSelectionDiv.slideUp();
				filterDivContainer.removeClass("open");
				filterSelectionDiv.removeClass("open");
			}
		}
		var container = $("#budgetListContainer");
		if (container.hasClass('show') && $("#budgetListContainer").is(":visible")) {
			if (!container.is(e.target) && container.has(e.target).length === 0) {
				var elementId = $('#' + e.target.id).parent().attr('id');
				var elementClass = $('#' + e.target.id).parent().attr('class');
				minDataValue = parseInt(minInput.attr("data-value")) || 0;
				maxDataValue = parseInt(maxInput.attr("data-value")) || 0;

				if (elementId != "minMaxContainer" && elementClass != "budget-box" && (minDataValue <= maxDataValue || maxDataValue == 0)) {
					$('#minMaxContainer').trigger('click');
					maxList.hide();
					maxInput.removeClass("border-red");
					$(".bw-blackbg-tooltip-max").hide();
					container.removeClass("invalid show").addClass("hide");

					$.inputValFormatting(minInput);
					$.inputValFormatting(maxInput);
				}
				else if (minDataValue > maxDataValue) {
					maxInput.addClass("border-red");
					$(".bw-blackbg-tooltip-max").show();
				}
			}
		}

	});

	$(".budget-box").click(function () {
		minDataValue = parseInt(minInput.attr("data-value")) || 0;
		maxDataValue = parseInt(maxInput.attr("data-value")) || 0;

		if (minDataValue <= maxDataValue || maxDataValue == 0) {
			$("#minMaxContainer").toggleClass("open");
			budgetListContainer.toggleClass("hide show");
			minList.show();
			maxList.hide();
			maxInput.removeClass("border-red");
			$(".bw-blackbg-tooltip-max").hide();
		}
		else if (minDataValue > 1)
			$.validateInputValue();
	});

	for (p in arr) {
		if (counter <= 8) {
			minList.append("<li data-value=" + arr[p].value + ">" + arr[p].amount + "</li>");
			counter++;
		}
	}

	minInput.on("click focus", function () {
		minList.show();
		maxList.hide();

		$.inputValFormatting(maxInput);

		minInputVal = $(this).val();
		if (minInputVal == "")
			minInput.val("");
		else {
			var userMinInput = minInput.attr("data-value");
			minInput.val($.formatPrice(userMinInput));
		}

		minInput.on("keyup", function () {
			maxInputVal = maxInput.val();
			userMinAmount = $(this).val();
			if (userMinAmount == "") {
				minInput.val("");
				minInput.attr("data-value", "");
				minAmount.html("0");
			}
			else {
				$("#budgetBtn").hide();
				minInput.attr("data-value", parseInt(userMinAmount.replace(/\D/g, ''), 10)).val($.formatPrice(userMinAmount.replace(/\D/g, ''), 10));
				formattedValue = $.newUserInputPrice(userMinAmount);
				minAmount.html(formattedValue);
				if (maxInputVal == "")
					maxAmount.html(" - MAX");
			}
			if ($("#budgetBtn").is(':visible'))
				minAmount.html("");
		});
	});

	maxInput.on("click focus", function () {
		maxInput.removeClass("border-red");
		$(".bw-blackbg-tooltip-max").hide();

		if (!maxList.hasClass("refMinList")) {
			var defaultValue = 30000;
			$.generateMaxList(defaultValue);
		}

		minList.hide();
		maxList.show();

		$.inputValFormatting(minInput);

		var userMaxAmount = $(this).val();
		if (userMaxAmount == "")
			maxInput.val("");
		else {
			var userMaxInput = maxInput.attr("data-value");
			maxInput.val($.formatPrice(userMaxInput));
		}

		maxInput.on("keyup", function (e) {
			userMaxAmount = $(this).val();
			/* when the user deletes the last digit left in the input field */
			if (e.keyCode == 8 && userMaxAmount.length == 0)
				maxAmount.html(" - MAX");

			if (userMaxAmount.length == 0 && $('#budgetBtn').not(':visible')) {
				maxInput.val("");
				maxInput.attr("data-value", "");
			}
			else {
				$("#budgetBtn").hide();
				userMinAmount = minInput.val();
				if (userMinAmount == "")
					minAmount.html("0");

				formattedValue = $.newUserInputPrice(userMaxAmount);
				maxInput.attr("data-value", parseInt(userMaxAmount.replace(/\D/g, ''), 10)).val($.formatPrice(userMaxAmount.replace(/\D/g, ''), 10));
				maxAmount.html("- " + formattedValue);
			}
		});

		/* based on user input value generate max list */
		var userInputMin = minInput.val();
		var userInputDataValue = minInput.attr("data-value");
		if (userInputMin.length != 0)
			$.generateMaxList(userInputDataValue);
	});

	minInput.on('focusout', function () {
		if (newBikeSearchVM.validateInputValue())
			newBikeSearchVM.applyMinMaxFilter('budget', minInput.attr('data-value') + '-' + (maxInput.attr('data-value') == 0 || maxInput.attr('data-value') == undefined ? '' : maxInput.attr('data-value')), $(this));
	});

	maxInput.on('focusout', function () {
		if (newBikeSearchVM.validateInputValue())
			newBikeSearchVM.applyMinMaxFilter('budget', minInput.attr('data-value') + '-' + (maxInput.attr('data-value') == 0 || maxInput.attr('data-value') == undefined ? '' : maxInput.attr('data-value')), $(this));
	});

	/* allow only digits in the input field */
	$.isNumberKey = function (evt) {
		var charCode = (evt.which) ? evt.which : event.keyCode;
		if (charCode > 31 && (charCode < 48 || charCode > 57))
			return false;
		return true;
	}

	$.fn.validateKeyPress = function () {
		return this.keypress(function () {
			return $.isNumberKey(event);
		});
	};
	minInput.validateKeyPress();
	maxInput.validateKeyPress();

	minList.delegate("li", "click", function () {
		var clickedLI = $(this);
		var amount = clickedLI.text();
		var dataValue = clickedLI.attr("data-value");
		var prevVal = minInput.attr('data-value');
		minInput.attr('data-value', dataValue);
		maxInputVal = maxInput.val();
		if (maxInputVal == "")
			$(".maxAmount").html("- MAX");
		$("#budgetBtn").hide();
		$.generateMaxList(dataValue);
		newBikeSearchVM.setMinAmount(dataValue);

		minList.hide();
		maxList.show().addClass("refMinList");
		if (parseInt(dataValue) <= parseInt(maxInput.attr('data-value')))
			newBikeSearchVM.applyMinMaxFilter('budget', dataValue + '-' + (maxDataValue == 0 ? '' : maxDataValue), clickedLI);
	});

	maxList.delegate("li", "click", function () {
		if (newBikeSearchVM && !newBikeSearchVM.IsInitialized()) {

			newBikeSearchVM.init(e);
		}
		var clickedLI = $(this);
		var amount = clickedLI.text();
		var dataValue = clickedLI.attr("data-value");
		maxInput.attr('data-value', dataValue);

		if (minInput.val() == 0) {
			$("#budgetBtn").hide();
			minInput.val(0);
			minInput.attr("data-value", 0);
			minAmount.html("0");
		}

		$("#minMaxContainer").removeClass("open");
		if (newBikeSearchVM.validateInputValue()) {
			maxList.hide();
			budgetListContainer.removeClass('show').addClass('hide');
			newBikeSearchVM.applyMinMaxFilter('budget', minInput.attr('data-value') + '-' + dataValue, clickedLI);
		}
	});

	$.newUserInputPrice = function (newMinMaxValue) {
		var newAmount = parseInt(newMinMaxValue.replace(/\D/g, ''), 10);
		var formattedValue = $.valueFormatter(newAmount);
		return formattedValue;
	}

	/* convert the comma seperated price into INR format*/
	$.inputValFormatting = function (priceInput) {
		var inputVal = priceInput.val();
		var str = '';
		var regex = new RegExp(',', 'g');
		if (inputVal.length > 0) {
			str = String(priceInput.attr("data-value"));
			str = str.replace(regex, '');
			priceInput.val($.valueFormatter(str));
		}
	};

	/* priceFormatter */
	$.formatPrice = function (price) {
		var thMatch = /(\d+)(\d{3})$/;
		var thRest = thMatch.exec(price);
		if (!thRest) return price;
		return (thRest[1].replace(/\B(?=(\d{2})+(?!\d))/g, ",") + "," + thRest[2]);
	}

	/* Initiate knockout binding*/
	$(".filter-selection-div li, .more-filter-item-data li, .sort-selection-div li").click(function (e) {
		if (newBikeSearchVM && !newBikeSearchVM.IsInitialized()) {
			newBikeSearchVM.init(e);
		}
	});

	if (window.location.hash) {
		var url = window.location.hash.replace('#', '');
		newBikeSearchVM.setFilters(url);
	}

    // Added By    : Rajan Chauhan on 21st Dec 2017
    // Description : For adding href on Load More button for crawler and stopping redirect
	$('#loadMoreBikes').click(function (event) {
	    // For stopping redirection on load more button
	    event.preventDefault();
	});
});