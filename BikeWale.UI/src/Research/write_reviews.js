/*
	This javascript used @ write user review page in research section.	
*/

document.getElementById("butSave").onclick = verifyForm;

function showCharactersLeft(ftb){
	var maxSize = 6000;
	var size = ftb.GetHtml().length;
	
	if(size >= maxSize){
		ftb.SetHtml( ftb.GetHtml().substring(0, maxSize -1) );
		size = maxSize;
	}
	
	document.getElementById("spnDesc").innerHTML = "Characters Left : " + (maxSize - size);
}

function previewReview(e){
	if(verifyForm() == false)
		return false;
		
	document.getElementById("divPreview").style.display = "";
	document.getElementById("divAddEdit").style.display = "none";
	window.scroll(0,0);
}
		
function verifyForm() {    
	var isError = false;	
	var reEmail = /^[a-z]+(([a-z_0-9]*)|([a-z_0-9]*\.[a-z_0-9]+))*@([a-z_0-9\-]+)((\.[a-z]{3})|((\.[a-z]{2})+)|(\.[a-z]{3}(\.[a-z]{2})+))$/;
	
	if(displayVersion == "display;"){
		if(document.getElementById("drpVersions")){
			if( document.getElementById("drpVersions").value == ""){
				isError = true;
				document.getElementById("spnVersions").innerHTML = "Required";
			}else
				document.getElementById("spnVersions").innerHTML = "";
		}
	}
		
	if(document.getElementById("hdnRateST").value == "0"){
		isError = true;
		document.getElementById("spnRateST").innerHTML = "Required";
	}else
		document.getElementById("spnRateST").innerHTML = "";
		
	if(document.getElementById("hdnRateCM").value == "0"){
		isError = true;
		document.getElementById("spnRateCM").innerHTML = "Required";
	}else
		document.getElementById("spnRateCM").innerHTML = "";	
		
	if(document.getElementById("hdnRatePE").value == "0"){
		isError = true;
		document.getElementById("spnRatePE").innerHTML = "Required";
	}else
		document.getElementById("spnRatePE").innerHTML = "";	
		
	if(document.getElementById("hdnRateVC").value == "0"){
		isError = true;
		document.getElementById("spnRateVC").innerHTML = "Required";
	}else
		document.getElementById("spnRateVC").innerHTML = "";		
		
	if(document.getElementById("hdnRateFE").value == "0"){
		isError = true;
		document.getElementById("spnRateFE").innerHTML = "Required";
	}else
		document.getElementById("spnRateFE").innerHTML = "";		
	
	var desc = tinyMCE.get('ftbDescription_txtContent').getContent();
	
	var descWithoutHtml = "";
	var mydiv = document.createElement("div");
	mydiv.innerHTML = desc;

	if (document.all){// IE Stuff
		descWithoutHtml = mydiv.innerText;
	   
	}else{// Mozilla does not work with innerText
		descWithoutHtml = mydiv.textContent;
	}                           
  
	var descWithoutHtmlArr = descWithoutHtml.split(" ");
	
	if( desc == "" ){
		isError = true;
		document.getElementById("spnDescription").innerHTML = "Required";
	}else if(desc.length > 8000){	
		isError = true;
		document.getElementById("spnDescription").innerHTML = "The typed detailed review contains more characters than allowed";
	}else if(descWithoutHtmlArr.length < 50){
		isError = true;
		document.getElementById("spnDescription").innerHTML = "Detailed review should contain minimum 50 words";
	}else
		document.getElementById("spnDescription").innerHTML = "";
		
	if(document.getElementById("txtTitle").value == ""){
		isError = true;
		document.getElementById("spnTitle").innerHTML = "Required";
	}else
		document.getElementById("spnTitle").innerHTML = "";
		
	if(document.getElementById("txtPros").value == ""){
		isError = true;
		document.getElementById("spnPros").innerHTML = "Required";
	}else
		document.getElementById("spnPros").innerHTML = "";	
		
	if(document.getElementById("txtCons").value == ""){
		isError = true;
		document.getElementById("spnCons").innerHTML = "Required";
	}else
		document.getElementById("spnCons").innerHTML = "";
		
	if(document.getElementById("ddlFamiliar").value == "0"){
		isError = true;
		document.getElementById("spnFamiliar").innerHTML = "Required";
	}else
		document.getElementById("spnFamiliar").innerHTML = "";			
			
	if(document.getElementById("txtEmail")){
		if(document.getElementById("txtEmail").value == ""){
			isError = true;
			document.getElementById("spnEmail").innerHTML = "Required";
		}else if ( !reEmail.test ( document.getElementById("txtEmail").value ) ){// verify valid email!
			document.getElementById("spnEmail").innerHTML = "Invalid Email";
			isError = true;
		}else
			document.getElementById("spnEmail").innerHTML = "";
	}
	
	document.getElementById("spnMileage").innerHTML = "";
	
	var mileageEntered = trim(document.getElementById("txtMileage").value);
	if (mileageEntered.length != 0){
		if (!MileageNumeric(mileageEntered)){
			document.getElementById("spnMileage").innerHTML = "Fuel Economy can have only numbers and maximum one decimal";
			isError = true;
		}
	}	
	
	if(isError == true)
		return false;
}

function MileageNumeric(mileageToValidate){
	var regEx = /^\d+$/;
	
	var splittedMileage = mileageToValidate.split('.');
	if (splittedMileage.length > 2)
		return false;
		
	for (var i=0; i<splittedMileage.length; i++){
		if(trim(splittedMileage[i]).length !=0 && !regEx.test(splittedMileage[i])){
			return false;
		}
	}	
	return true;
}

function trim(val) {
	var ret = val.replace(/^\s+/, '');
	ret = ret.replace(/\s+$/, '');
	return ret;
}

//for changing the images
var imgs = document.getElementById("tblRatings").getElementsByTagName("img");

var msgs = new Array ( "Poor", "Fair", "Good", "Very Good", "Excellent" );
var path        = "https://imgd.aeplcdn.com/0x0/bw/static/design15/old-images/d/";
var imgN		= path + "white.gif";
var imgT 		= path + "1.gif";
var imgTOver 	= path + "greyRed.gif";
var imgTLess 	= path + "whiteRed.gif";
var imgF 		= path + "white.gif";
var imgFOver 	= path + "grey.gif";
var spnIdIn		= "divRate";
var imgIdIn 	= "imgRate";
var hdnIdIn 	= "hdnRate";

reInitialize();
function reInitialize(){
	for(var i = 1; i <= 6; i++){
		var type = getRateType(i);
		var imgId = imgIdIn + type;
		var rate  = Number(document.getElementById(hdnIdIn + type).value);
		
		//show the images
		for(var j = 1; j  <= 5; j++){
			if(j <= rate)
				document.getElementById(imgId + j).src = imgT;
			else
				document.getElementById(imgId + j).src = imgF;
				
		}
	}
}

function getRateType(val){
	var ret = "";
	switch(val){
		case 1:
			ret = "ST";
			break;
		case 2:
			ret = "CM";
			break;
		case 3:
			ret = "PE";
			break;
		case 4:
			ret = "VC";
			break;
		case 5:
			ret = "FE";
			break;
		case 6:
			ret = "OA";
			break;
		default:
			break;
	}
	return ret;
}

function mouseHover(e){
	var e1 = e ? e.target : event.srcElement;
	
	var imgType = e1.id.substring(7,9);
	var index   = Number(e1.id.substring(9,e1.id.length));
	
	var spnId = spnIdIn + imgType;
	var imgId = imgIdIn + imgType;
	var rate  = Number(document.getElementById(hdnIdIn + imgType).value);
	
	document.getElementById(spnId).innerHTML = msgs[index - 1];
	
	//show the images
	for(var i = 1; i  <= 5; i++){
		if(i <= rate && i <= index)
			document.getElementById(imgId + i).src = imgTOver;
		else if(i <= rate && i > index)
			document.getElementById(imgId + i).src = imgTLess;
		else if(i > rate && i <= index)
			document.getElementById(imgId + i).src = imgFOver;
		else
			document.getElementById(imgId + i).src = imgN;			
	}
}

function mouseOut(e){
	var e1 = e ? e.target : event.srcElement;
	
	var imgType = e1.id.substring(7,9);
	var index   = Number(e1.id.substring(9,e1.id.length));
	
	
	var spnId = spnIdIn + imgType;
	var imgId = imgIdIn + imgType;
	var rate  = Number(document.getElementById(hdnIdIn + imgType).value);
	
	document.getElementById(spnId).innerHTML = "&nbsp;";
			
	//show the images
	//show the images
	for(var i = 1; i  <= 5; i++){
		if(i <= rate)
			document.getElementById(imgId + i).src = imgT;
		else
			document.getElementById(imgId + i).src = imgF;
	}
}

function imgClick(e){
	var e1 = e ? e.target : event.srcElement;
	
	var imgType = e1.id.substring(7,9);
	var index   = Number(e1.id.substring(9,e1.id.length));
	
	var imgId = imgIdIn + imgType;
	//set the value
	document.getElementById(hdnIdIn + imgType).value = index;
	//for the images untill the index, set it as true else as false
	for(var i = 1; i <= 5; i++){
		if(i <= index)
			document.getElementById(imgId + i).src = imgT;
		else
			document.getElementById(imgId + i).src = imgF;
	}	
}

// create event handlers.
for ( var j = 0; j < imgs.length; j++ ){
	if ( imgs[j].id.indexOf("imgRate") != -1 ){
		imgs[j].onmouseover = mouseHover;
		imgs[j].onmouseout = mouseOut;
		imgs[j].onclick = imgClick;
	}
}