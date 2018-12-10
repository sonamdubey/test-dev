var hideGetOffersBtn = false;
function HideCompareCarBox(divComparePopUp) {

    $(divComparePopUp).removeClass("collapsCompareCar").addClass("expandCompareCar");
    $("#divCompareCarContainer").slideUp(1000);

    $(divComparePopUp).find(":nth-child(2)").removeClass("arrowDownGray").addClass("arrowUpGray");

    //add bottom padding as equal to divComparePopUp height
    $("#divFooterCompareCar").attr("style", "padding-top:" + $("#divComparePopUp").height() + "px;");
}
function formatURL(str) {
    str = str.toLowerCase();
    str = str.replace(/[^0-9a-zA-Z]/g, '');
    return str;
}
function GetCookieVal() {
    var theCookie = "" + document.cookie;
    var ind = theCookie.indexOf("CompareVersions");
    if (ind == -1 || "CompareVersions" == "" || "CompareVersions" == "|") return "";
    var ind1 = theCookie.indexOf(';', ind);
    if (ind1 == -1) ind1 = theCookie.length;
    return unescape(theCookie.substring(ind + "CompareVersions".length + 1, ind1));
}
function RemoveCar(versionID, carName) {
    //alert("carName" + carName);
    $("#divCarHeader").fadeOut(300);
    setTimeout(function () {
        //$("#divCarHeader").css("opacity", "1");
        $("#divCarHeader").fadeIn();
    }, 300);
    if (compCarName1 == carName) {
        compCarMakeName1 = compCarMakeName2;
        compCarMaskingName1 = compCarMaskingName2;
        compCarName1 = compCarName2;
        compCarVerImg1 = compCarVerImg2;
        compCarVerPrice1 = compCarVerPrice2;
        compCarVerUrl1 = compCarVerUrl2;

        compCarMakeName2 = "";
        compCarMaskingName2 = "";
        compCarName2 = "";
        compCarVerImg2 = "";
        compCarVerPrice2 = "";
        compCarVerUrl2 = "";

        $("#divClose2").hide();
    }
    else if (compCarName2 == carName) {
        compCarMakeName2 = "";
        compCarMaskingName2 = "";
        compCarName2 = "";
        compCarVerImg2 = "";
        compCarVerPrice2 = "";
        compCarVerUrl2 = "";

        $("#divClose2").hide();
    }
    var cookieVal = GetCookieVal();

    var splitVal = cookieVal.split(versionID + "|");
    cookieVal = splitVal[0] + splitVal[1];
    cookieVal = jQuery.trim(cookieVal);
    cookieVal = cookieVal.substring(0, cookieVal.length);
    document.cookie = "CompareVersions=" + cookieVal + ";path=/;domain=" + location.hostname.replace("www.", "");

    if (compCarName1 == "" && compCarName2 == "") {
        HideCompareCarBox($("#divComparePopUp").find("div:first"));
    }
}

filterTable(true);
$(".filter__option").change(filterTable);
function filterTable(firstRun) {
	var filters = $(".filter__option:checked");
	var variants = $(".variant");
	if (filters.length > 0) {
		variants.hide();
		$(".filter__no-result,.variant-list__more").hide();
		var fuelTypeFilters = filters.filter("[id^=ft__]");
		var transmissionTypeFilters = filters.filter("[id^=tr__]");
		if (fuelTypeFilters.length > 0 && transmissionTypeFilters.length > 0) {
			var filteredVariants = [];
			fuelTypeFilters.each(function () { filteredVariants.push.apply(filteredVariants, variants.filter("[fuel-type='" + this.value + "']")); });
			transmissionTypeFilters.each(function () { $(filteredVariants).filter("[transmission-type='" + this.value + "']").show(); });
		}
		else {
			fuelTypeFilters.each(function () { variants.filter("[fuel-type='" + this.value + "']").show(); });
			transmissionTypeFilters.each(function () { variants.filter("[transmission-type='" + this.value + "']").show(); });
		}

		if (variants.filter(":visible").length == 0) { $(".filter__no-result").show(); }
	}
	else if (firstRun===true) {
		var i = 0;
		variants.hide();
		while (i < Math.min(variants.length, 4)) {
			variants.eq(i).show(); i++;
		}
	}
	else {
		variants.show();
		$(".filter__types").text("Fuel type & Transmission");
	}
}
var sessionId;
$(".filter__option").change(trackFilters);
function trackFilters() {
	sessionId = sessionId || $.cookie("_cwv").split(".")[1];
	var timeStamp = new Date().getTime();
	var label = sessionId + '-' + timeStamp + '-' + ModelName + '-' + ($(this).is(':checked') ? "S" : "DS") + ':' + this.value;
	Common.utils.trackAction("CWInteractive", "ModelPage", 'VersionTableFilterUsage', label);
}
toggleFilterTags.call($(".filter__toggle"));
$(".filter__toggle").change(toggleFilterTags);
function toggleFilterTags() {

	if (!$(this).is(":checked")) {
		var filters = $(".filter__option:checked");
		if (filters.length > 0) {
			var tags = "";
			filters.each(function () { tags = tags + "<span class='filter__tag close'>" + this.value + "</span>" })
			$(".filter__types").html(tags);
			$(".filter__tag").click(function () { $(this).hide(); $(".filter__option[value='" + this.innerHTML + "']").trigger('click'); });
		}
	}
	else {
		$(".filter__types").text("Fuel type & Transmission");
	}
}


$(".variant-list__more").click(showMoreVariants);
function showMoreVariants() {
	$(".variant").show();
	$(".variant-list__more").hide();
	Common.utils.trackAction("CWInteractive", "ModelPage", "VersionTableFilterUsage", "MoreVariantsViewed");
}

toggleCompareLinks.call($(".switch__chkbox"),true);
$(".switch__chkbox").change(toggleCompareLinks);
function toggleCompareLinks(firstView) {
	if ($(this).is(":checked")) {
		$(".variant__ctas").hide();
		$(".compare-cta").show();
		if(firstView!=true)Common.utils.trackAction("CWInteractive", "ModelPage", "VersionTableFilterUsage", "CompareSwitchhit:ON");
	}
	else {
		$(".variant__ctas").show();
		$(".compare-cta").hide();
		if (firstView!=true)Common.utils.trackAction("CWInteractive", "ModelPage", "VersionTableFilterUsage", "CompareSwitchhit:OFF");
	}
}

function CheckSelectedVersion(cookieVal) {
 
	var compareCtas = $("div[name=chkVersion]");
	compareCtas.each(function () {
        if ($(this).hasClass("divCheck")) {
        	$(this).removeClass("divCheck").addClass("divUnCheck").find(".compare-cta__text").text("Add to compare");
        }
    });
    var splitCookie = cookieVal.split("|");
    for (var i = 0; i < splitCookie.length; i++) {
        if (splitCookie[i] != "") {
        	compareCtas.each(function () {
        	    if ($(this).attr("versionId") == splitCookie[i]) {
        	        if (i==0) {
        	            compCarName1 = "";
        	        }
        	        else{
        	            compCarName2 = ""
        	        }
                    $(this).removeClass("divUnCheck").addClass("divCheck").find(".compare-cta__text").text("Do not compare");
                    var carName = CarDetails.carMakeName + " " + CarDetails.carModelName + " " + $(this).attr("versionName");
                    var verImg = $(this).attr("ImgUrl");
                    var verUrl = $(this).parent().find("a:first").attr('href');
                    var verPrice = $(this).attr("price");
                    setCompareData(CarDetails.carMakeName, CarDetails.carMaskingName, carName, verImg, verPrice, verUrl);
                }
            });
        }

    }
}
function SelectCarRemove(divClose) {
    var cookieVal = GetCookieVal();
    if (cookieVal != "") {
        var splitVal = cookieVal.split("|");
        var sver1 = splitVal[0];
        var sver2 = splitVal[1];


        if ($(divClose).attr("id") == "divClose1") {
            if (sver1 != "") {
                RemoveCar(sver1, compCarName1);
                $("#divClose1").hide();
            }
        }

        else if ($(divClose).attr("id") == "divClose2") {
            if (sver2 != "") {
                RemoveCar(sver2, compCarName2);
                $("#divClose2").hide();
            }
        }

    }
    if (GetCookieVal() != "")
        CheckSelectedVersion(GetCookieVal());
    else {
        $("div[name='chkVersion']").each(function () {
            //$(this).attr("checked", false);
            if ($(this).hasClass("divCheck")) {
            	$(this).removeClass("divCheck").addClass("divUnCheck").find(".compare-cta__text").text("Add to compare");
            }
        });
    }
    ShowComparePopUp(GetCookieVal());
    LoadCompareCarDetails();
}

function setCompareData(makeName, maskingName, carName, verImg, verPrice, verUrl) {
    if (compCarName1 == "" || compCarName1.length == 0) {
        compCarMakeName1 = makeName;
        compCarMaskingName1 = maskingName;
        compCarName1 = carName;
        compCarVerImg1 = verImg;
        compCarVerPrice1 = verPrice;
        compCarVerUrl1 = verUrl;
    }
    else if (compCarName2 == "" || compCarName2.length == 0) {
        compCarMakeName2 = makeName;
        compCarMaskingName2 = maskingName;
        compCarName2 = carName;
        compCarVerImg2 = verImg;
        compCarVerPrice2 = verPrice;
        compCarVerUrl2 = verUrl;
    }
}
function SelectVersion(div) {
    var versionID = $(div).attr("versionID");
    var carName = CarDetails.carMakeName + " " + CarDetails.carModelName + " " + $(div).attr("versionName");
    var verImg = $(div).attr("ImgUrl");
    var verUrl = $(div).parent().find("a:first").attr('href');//.parent("td").prev("td").find("a").attr("href");
    var verPrice = $(div).attr("price");

    if ($(div).hasClass("divUnCheck")) {
    	$(div).removeClass("divUnCheck").addClass("divCheck").find(".compare-cta__text").text("Do not compare");;
        var cookieVal = GetCookieVal();
        if (cookieVal == "") {
            document.cookie = "CompareVersions=" + versionID + "|;path=/;domain="+location.hostname.replace("www.","");
        }
        else {
            var splitVal = cookieVal.split("|");
            if (splitVal.length > 2) {
                alert("You can select only 2 versions for comparison. Please remove one version to add another.");
                $(div).removeClass("divCheck").addClass("divUnCheck").find(".compare-cta__text").text("Add to compare");
            }
            else {
                document.cookie = "CompareVersions=" + cookieVal + versionID + "|;path=/;domain=" + location.hostname.replace("www.", "");
            }
        }
        setCompareData(CarDetails.carMakeName, CarDetails.carMaskingName, carName, verImg, verPrice, verUrl);
    }
    else {
        if ($(div).hasClass("divCheck")) {
        	$(div).removeClass("divCheck").addClass("divUnCheck").find(".compare-cta__text").text("Add to compare");
            RemoveCar(versionID, carName);
        }
    }
    ShowComparePopUp(GetCookieVal());
    LoadCompareCarDetails();

}
function AddBorder() {
    var divCarColorFirstChild = $("#divCarHeader").find(":nth-child(1)");
    //alert("1 : " + divCarColorFirstChild.height() + " 2: " + divCarColorFirstChild.next().height());

    if (divCarColorFirstChild.find(".compareCarBox").hasClass("colorborderRt")) {
        //alert("colorborderRt");
        divCarColorFirstChild.find(".compareCarBox").removeClass("colorborderRt");
    }
    if (divCarColorFirstChild.next().find(".compareCarBox").hasClass("colorborderLt")) {
        //alert("colorborderLt");
        divCarColorFirstChild.next().find(".compareCarBox").removeClass("colorborderLt")
    }

    if (divCarColorFirstChild.height() > divCarColorFirstChild.next().height()) {
        divCarColorFirstChild.find(".compareCarBox").addClass("colorborderRt");
    }
    else {
        divCarColorFirstChild.next().find(".compareCarBox").addClass("colorborderLt");
    }
}
function AddAnotherCarOption() {
    //if ($("#hdnImgContainer2").val() == "") {
    var htVs = (parseInt($("#divCarHeader").height()) - 26) / 3;
    var widthAnother = (parseInt($("#divImgContainer2").closest(".compareCarContainer").width()) - 45) / 2;
    var addAnotherDiv = $(document.createElement("div")).addClass("addAnotherBtn").attr("style", "left:" + widthAnother + "px;top:" + htVs + "px;");
    $("#divImgContainer2").find("a").attr("href", "/m/new/").html(addAnotherDiv);

    if ($(addAnotherDiv).is(":visible"))
        var htAnother = (parseInt(htVs)) + 45;
    else
        var htAnother = (parseInt((parseInt($("#divCarHeader").height())) / 2));

    var wdAnother = (parseInt($("#divImgContainer2").closest(".compareCarContainer").width()) - 100) / 2;
    $("#divCarName2").find("a").attr("href", "/m/new/").attr("style", "color:black;text-decoration:none;font-weight:bold;position:absolute;left:" + wdAnother + "px;top:" + htAnother + "px;").html("Add Another Car");
    $("#divPrice2").html("");
    $("#divPrice2").parent().hide();
    AddBorder();
}
function FixFooterCompareCar() {
    var htBottom = parseInt($("#divComparePopUp").height() + $("#divCompareCarContainer").height());
    $("#divFooterCompareCar").attr("style", "padding-top:" + htBottom + "px;");
}
function AddVs() {
    var htVs = (parseInt($("#divCarHeader").height()) - 24) / 2;
    var vsDiv = $(document.createElement("div")).addClass("vsDiv").attr("style", "top:" + htVs + "px;");
    $(vsDiv).attr("id", "divVs");
    $("#divCarHeader").find("div:first").append(vsDiv);
}
function ShowCompareCarBox(divComparePopUp) {
    $(divComparePopUp).removeClass("expandCompareCar").addClass("collapsCompareCar");
    $("#divComparePopUp").slideDown(1000);
    $("#divCompareCarContainer").slideDown(1000, function () {
        //to position compare car pop up at bottom of page to show all the content of page visible to user
        FixFooterCompareCar();
    });

    $(divComparePopUp).find(":nth-child(2)").removeClass("arrowUpGray").addClass("arrowDownGray");

    AddBorder();


    if ($("#divVs").length > 0)
        $("#divVs").remove();
    AddVs();

    if (compCarVerImg2 == "") {
        AddAnotherCarOption();
    }
}
function ToggleCompareCarBox(divComparePopUp) {
    if ($(divComparePopUp).attr("class") == "expandCompareCar") {
        ShowCompareCarBox(divComparePopUp);
        var addBtn = $('#divCompareCarContainer').find('.addAnotherBtn');
        if (addBtn.is(':visible') && addBtn.length > 0)
            AddAnotherCarOption();
    }
    else if ($(divComparePopUp).attr("class") == "collapsCompareCar") {
        HideCompareCarBox(divComparePopUp);
    }
}
function LoadCompareCarDetails() {
    $("#divCarName1").find("a").attr("style", "color:black;text-decoration:none;font-weight:bold;").attr("href", compCarVerUrl1).html(compCarName1);
    $("#divImgContainer1").find("a").attr("href", compCarVerUrl1).html($(document.createElement("img")).attr("src", compCarVerImg1));
    $("#divPrice1").html(compCarVerPrice1);
    $("#divClose1").show();

    if (compCarName2 == "" || compCarName2.length == 0) {
        AddAnotherCarOption();
    }
    else {
        $("#divCarName2").find("a").attr("style", "color:black;text-decoration:none;font-weight:bold;").html(compCarName2).attr("href", compCarVerUrl2);
        $("#divImgContainer2").find("a").attr("href", compCarVerUrl2).html($(document.createElement("img")).attr("src", compCarVerImg2));
        $("#divPrice2").html(compCarVerPrice2).parent().show();
        $("#divClose2").show();
    }


    ////to expand compare cars pop up in case two versions selected
    if (compCarName1 != "" && compCarName2 != "")
        ShowCompareCarBox($("#divComparePopUp").find("div:first"));
}
function ShowComparePopUp(cookieVal) {
    if (cookieVal != "") {
        $("#divComparePopUp").show();
        $("#divFooterCompareCar").attr("style", "padding-top:" + $("#divComparePopUp").height() + "px;");
        if (hideGetOffersBtn && $('#callslug').is(":visible")) {
            $('#callslug').hide();
        }
        //alert($("#divComparePopUp").find(":nth-child(1)").height());
    }
    else {
        $("#divComparePopUp").hide();
        $("#divCompareCarContainer").hide();
        $("#divFooterCompareCar").attr("style", "padding-top:0px;");//hide bottom padding
        if (hideGetOffersBtn) {
            $('#callslug').show();
        }
    }
}
function VersionSelectByChkBox(chkBox) {
    SelectVersion(chkBox);
}
function VerifyVersion(pageSource) {
    var isError = false;
    ver1 = "";
    ver2 = "";
    var cookieVal = GetCookieVal();
    if (cookieVal != "") {
        var splitVal = cookieVal.split("|");
        for (var i = 0; i < splitVal.length; i++) {
            if (splitVal[i] != "") {
                if (ver1 == "")
                    ver1 = splitVal[i];
                else if (ver2 == "")
                    ver2 = splitVal[i];
            }
        }
        if (ver1 == ver2) {
            //   $("#spnError").html("Please choose different cars for comparison <br>");
            // $("#popupDialog").popup("open");
            alert("Please choose different cars for comparison ");
            isError = true;
        }
        if (ver1 == "" || ver2 == "") {
            // $("#spnError").html("Please select one more car to compare <br>");
            // $("#popupDialog").popup("open");
            alert("Please select one more car to compare ");
            isError = true;
        }
    }
    else {
        // $("#spnError").html("Please select one more car to compare <br>");
        //  $("#popupDialog").popup("open");
        alert("Please select one more car to compare ");
        isError = true;
    }

    if (isError)
        return false;
    else
        GenerateURL(pageSource);
}
function GenerateURL(pageSource) {
    var qs = '';
    var dataList = [];
    var count = 1;
    if (ver1 > 0) {
        dataList.push({ id: compCarModelId1, text: formatURL(compCarMakeName1) + '-' + compCarMaskingName1 });
        qs += 'c' + count.toString() + '=' + ver1 + '&';
        count++;
    }
    if (ver2 > 0) {
        dataList.push({ id: compCarModelId2, text: formatURL(compCarMakeName2) + '-' + compCarMaskingName2 });
        qs += 'c' + count.toString() + '=' + ver2;
    }    
    location.href = "/m/comparecars/" + Common.getCompareUrl(dataList) + '/?' + qs+ ( Number(pageSource) > 0 ? '&source='+pageSource:'');
}

var cookieVal = GetCookieVal();
if (cookieVal != "") {
    ShowComparePopUp(cookieVal);
    CheckSelectedVersion(cookieVal);
    LoadCompareCarDetails();
}

var shortReviewPosition;
function FullReview() {
    $("#divShortSynopsis").hide();
    $("#divFullSynopsis").show().find('img.lazy').lazyload();
    var scollTop = $(window).scrollTop();
    shortReviewPosition = scollTop;
}

function HideReview() {
    $("#divFullSynopsis").hide();
    $("#divShortSynopsis").show();
    if (shortReviewPosition != null) {
        $(window).scrollTop(shortReviewPosition);
    }
}


//to position divFooterCompareCar on resize as image is hidden in landscape mode
if ($("#divComparePopUp").find("div:first").hasClass("collapsCompareCar"))
        FixFooterCompareCar();

$(window).bind('resize', function (e) {
    //to position divVs on resize as image is hidden in landscape mode
    if ($("#divVs").length > 0)
        $("#divVs").remove();
    AddVs();
    //to position divAnother on resize as image is hidden in landscape mode
    if (compCarVerImg2 == "") {
        AddAnotherCarOption();
    }
    //to position divFooterCompareCar on resize as image is hidden in landscape mode
    if ($("#divComparePopUp").find("div:first").hasClass("collapsCompareCar"))
        FixFooterCompareCar();
});
$("#divFullSynopsis").find("img").addClass("fullWidth");
$('#divShortDescContent > p').contents().unwrap();
(function () {
    var abTestVal = $.cookie('_abtest') != null ? parseInt($.cookie('_abtest')) : 0
    hideGetOffersBtn = (abTestVal > 0 && abTestVal <= 50) && $('#callslug').length > 0 
    hideGetOffersBtn && GetCookieVal() != "" && $('#callslug').hide();
})();