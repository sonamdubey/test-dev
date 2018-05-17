import React from 'react'
import {withRouter} from 'react-router-dom'
import { hideElement , onRoadPricePopupDataObject , popupState , getCookie , openCityAreaSelectionPopup , closeCityAreaSelectionPopup , GetGlobalCityArea , showElement, resetOnRoadPricePopup, SetCookieInDays , gtmCodeAppender , onCookieObj} from '../utils/popUpUtils'
import {triggerGA} from '../utils/analyticsUtils'
import CityAreaAutocomplete from '../components/CityAreaAutocomplete'
import SpinnerRelative from './Shared/SpinnerRelative'
var OnRoadPricePopupInitialized = false;

class OnRoadPricePopup extends React.Component {
	
	constructor(props) {
		super(props);
		this.state = {}; 
		this.showCitySelectionPopup = this.showCitySelectionPopup.bind(this);
		this.showAreaSelectionPopup = this.showAreaSelectionPopup.bind(this);
		this.closeCityAreaSelectionPopup = this.closeCityAreaSelectionPopup.bind(this);
		this.priceLoaderBtnClicked = this.priceLoaderBtnClicked.bind(this);
		this.initializePQ = this.initializePQ.bind(this);
		this.findCityById = this.findCityById.bind(this);
		this.findAreaById = this.findAreaById.bind(this);
		this.createMPQ = this.createMPQ.bind(this);
		this.closeOnRoadPricePopUp = this.closeOnRoadPricePopUp.bind(this);
		this.citySelect = this.citySelect.bind(this);
		this.areaSelect = this.areaSelect.bind(this);
		this.formPostDataString = this.formPostDataString.bind(this);
		this.resetPopup = this.resetPopup.bind(this);
		this.triggerABTestGA = this.triggerABTestGA.bind(this);
	}
	citySelect(selectedCity) {
		var selectedCityId = selectedCity.id;
		var loadingText = '';
		var isPersistance = this.state.IsPersistence;
		if(selectedCity != null && !selectedCity.hasAreas) {
		    loadingText = "Fetching on-road price for " + selectedCity.name;
            isPersistance = false;
		}
		else {
			loadingText = "Loading areas for " + selectedCity.name;
		}

		this.setState({
			SelectedCity : selectedCity , 
			SelectedCityId : selectedCityId, 
		    SelectedArea : null , 
		    SelectedAreaId : 0,
		    BookingAreas : [],
		    IsPersistence : isPersistance,
		    LoadingText : loadingText,
		    state : popupState.areaPopupOpen
		})
		if(selectedCity && (selectedCity.hasAreas || selectedCity.id != onCookieObj.PQCitySelectedId)) { 
			OnRoadPricePopupInitialized = false;
			
		}
		this.closeCityAreaSelectionPopup();

		
	}
	areaSelect(selectedArea){
		
		var selectedAreaId = selectedArea.id;
		var IsPersistence = false;
		var loadingText = "";
		if(selectedArea.id != onCookieObj.PQAreaSelectedId) {
			OnRoadPricePopupInitialized = false ;
			loadingText = "Fetching on-road price for " + selectedArea.name + ", " + this.state.SelectedCity.name;
		}
		this.setState({
			SelectedArea : selectedArea , 
			SelectedAreaId : selectedArea.id,
			IsPersistence : false,
			LoadingText : loadingText,
			state : popupState.cityPopupOpen
		})
		this.closeCityAreaSelectionPopup();
	}
	closeOnRoadPricePopUp() {
		hideElement(document.getElementById('popupWrapper'));
		this.resetPopup();
		window.history.back();	
    }
	showCitySelectionPopup() {
		showElement(document.getElementById('popupContent'));
		showElement(document.getElementById('bw-city-popup-box'));
		openCityAreaSelectionPopup();
		this.setState({
			state: popupState.cityPopupOpen
		})
	}
	closeCityAreaSelectionPopup(){
		closeCityAreaSelectionPopup();
	}
	showAreaSelectionPopup() {
		showElement(document.getElementById('popupContent'));
		showElement(document.getElementById('bw-area-popup-box'));
		openCityAreaSelectionPopup();
		this.setState({
			state: popupState.areaPopupOpen
		})
	}
	priceLoaderBtnClicked() {
		this.setState({IsPersistence : false});

		this.initializePQ();
	}
	findCityById(cities , id) {
		if(cities && cities.length > 0) {
			var filteredCity = cities.filter(function(city) {
				return (city.id == id);
			});
			if(filteredCity && filteredCity.length > 0)
				return filteredCity[0];
			return null;
		}
		return null;
	}
	findAreaById(areas , id) {
		if(areas && areas.length > 0) {
			var filteredArea = areas.filter(function(area) {
				return (area.id == id);
			});
			if(filteredArea && filteredArea.length > 0)
				return filteredArea[0];
			return null;
		}
		return null;
	}
	createMPQ(pqId) {
        if (this.state.SelectedCityId && this.state.SelectedModelId && this.state.VersionId && pqId) {
            return btoa("CityId=" + this.state.SelectedCityId + "&AreaId=" + this.state.SelectedAreaId +"&PQId=" + pqId +"&VersionId=" + this.state.VersionId + "&DealerId=" + this.state.DealerId);
        }
    }
    resetPopup() {
    	resetOnRoadPricePopup();
    }
    triggerABTestGA() {
        try {
            var obj = this.state; 
            var abuser = getCookie("_bwtest"),
                bikeandcity = obj.MakeName + "_" + obj.ModelName + "_" + (obj.SelectedCity ? obj.SelectedCity.name : ""),               
            bikeandcity = bikeandcity.replace("/\s+/gi", "_");
            triggerGA("PQ_Popup", "PQSuccess", abuser + "_" + gaObj.id + "_" + bikeandcity);
        }
        catch (e) {
            console.log(e.message)
        }
    }
	initializePQ(isLocChanged) {
		var obj = this.state; 
		if(obj.SelectedModelId != null && obj.SelectedModelId > 0) {
			var postData = {
				'CityId' : obj.SelectedCityId,
				"AreaId" : obj.SelectedAreaId,
                "ModelId" : obj.SelectedModelId,
                "ClientIP" : "",
                "SourceType" : "2",
                "VersionId" : obj.VersionId,	
                "pQLeadId" : obj.PageSourceId,
                'deviceId' : getCookie('BWC'),
                'isPersistance' : obj.IsPersistance,
                'refPQId' : typeof pqId != 'undefined' ? pqId : '', 
                'isReload' : obj.IsReload
			} 

			var xhr = new XMLHttpRequest();
			var url = "/api/generatepq/"; 
			xhr.open('POST',url);
			xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded')
			xhr.setRequestHeader('utma', getCookie('__utma'));
            xhr.setRequestHeader('utmz', getCookie('_bwutmz'));
			xhr.onreadystatechange = function() {
				if(xhr.readyState == 4) {
					if(xhr.status == 200) {
						var responseData = JSON.parse(xhr.responseText);
						var SelectedCity  = this.state.SelectedCity ,
							SelectedArea  = this.state.SelectedArea ,
							SelectedAreaId  = this.state.SelectedAreaId ,
							BookingCities = this.state.BookingCities,
							BookingAreas = this.state.BookingAreas,
							HasAreas = this.state.HasAreas;
						
						if(responseData && responseData.pqCities && responseData.pqCities.length > 0) {
							var cities = responseData.pqCities;

							if(cities!=null && cities.length > 0) {
								BookingCities = cities;
								HasAreas = (this.state.SelectedCity != null && this.state.SelectedCity.hasAreas) ? true : false ;
								if(this.state.SelectedCityId > 0) { 
									SelectedCity = this.findCityById(BookingCities,this.state.SelectedCityId); 
									
									var areas = responseData.pqAreas;
									if(areas!=null && areas.length > 0) {
										BookingAreas = areas; 
										if(this.state.SelectedAreaId > 0) {
											SelectedArea = findAreaById(BookingAreas,this.state.SelectedAreaId); 
										}
										else {
											SelectedArea = null; 
										}
									}
									else {
										BookingAreas = [] 
										SelectedArea = null; 
										SelectedAreaId = 0; 

									}
								}
							}
							this.setState({
								SelectedCity : SelectedCity,
								SelectedArea : SelectedArea,
								SelectedAreaId : SelectedAreaId,
								BookingCities : BookingCities,
								BookingAreas : BookingAreas,
								HasAreas : HasAreas,
								LoadingText : ''
							}) 
						}
						else if(responseData.priceQuote != null) {
							var jsonObj = responseData.priceQuote;

							var gaLabel = GetGlobalCityArea() + ', ';
							
							if(this.state.MakeName || this.state.ModelName) {
								gaLabel += this.state.MakeName + ',' + this.state.ModelName ;
							}
							if(this.state.SelectedCityId > 0) {
								if(this.state.SelectedCity && this.state.SelectedCity.id > 0) {
									var lbText = "Fetching on-road price for " + this.state.SelectedCity.name;
									var cookieValue = this.state.SelectedCity.id + "_" + this.state.SelectedCity.name;
									if(this.state.SelectedArea && jsonObj.isDealerAvailable) {
										cookieValue += ("_" + this.state.SelectedArea.id + "_" + this.state.SelectedArea.name);
										lbtext = "Fetching on-road price for " + (this.state.SelectedArea.name + ", " + this.state.SelectedCity.name);
									}
									if(this.state.SelectedCityId != onCookieObj.PQCitySelectedId || (this.state.SelectedAreaId > 0 && jsonObj.isDealerAvailable)) {
										SetCookieInDays("location",cookieValue, 365); 
									}
									
									
								}
							}
                            
							if(jsonObj.dealerId > 0)
                                triggerGA(gaObj.name, 'Dealer_PriceQuote_Success_Submit', gaLabel);
                            else triggerGA(gaObj.name, 'BW_PriceQuote_Success_Submit', gaLabel);
							this.triggerABTestGA();
                            if (this.state.DealerId > 0 && responseData.qStr.length) {// TODO where is dealer id being set
                                responseData.qStr = this.createMPQ(responseData.priceQuote.quoteId);
                            }
                            if(!this.state.IsReload && responseData.qStr!='')
                            {
                            	this.resetPopup();
                            	window.location.hash ='';
                            	window.location.href = "/m/pricequote/dealer/" + "?MPQ=" + responseData.qStr;
                             
                            }
                            else   window.location.reload(true);

						}
						else {
							if (this.state.SelectedCityId > 0 ) {
	                            if (this.state.SelectedCity && this.state.SelectedCity.id > 0) {
	                                var lbtext = "Fetching on-road price for " + this.state.SelectedCity.name;
	                                var cookieValue = this.state.SelectedCity.id + "_" + this.state.SelectedCity.name;
	                                if (this.state.SelectedArea && this.state.SelectedArea.id > 0 ) {
	                                    cookieValue += ("_" + this.state.SelectedArea.id + "_" + this.state.SelectedArea.name);
	                                    lbtext = "Fetching on-road price for " + (this.state.SelectedArea.name + ", " + this.state.SelectedCity.name);
	                                }
	                                if (this.state.state.SelectedCityId != onCookieObj.PQCitySelectedId || this.state.SelectedAreaId > 0)
	                                    SetCookieInDays("location", cookieValue, 365);

	                                this.setState({
										LoadingText : lbText
									});
	                            }

                            }
                            window.location.reload(true);
						}

				}
				else {

				}	
				}
				
			}.bind(this);
			xhr.send(this.formPostDataString(postData)); 
			
		}

	}
	formPostDataString(postData) {
		var str = '';
		var keys = Object.keys(postData);
		for(var i = 0 ; i < keys.length ; i++) {
			str += keys[i]+'=';
			str += (postData[keys[i]] == null || postData[keys[i]] == undefined ? '' : postData[keys[i]]) +'&';
		}
		return str;
		
	}
	componentWillReceiveProps(nextProps) {
		try {
			if((this.state.SelectedModelId == null || this.state.SelectedModelId <= 0) || 
				(this.state.SelectedModelId != null && nextProps.onRoadPriceDataObject != null &&
					this.state.SelectedModelId != nextProps.onRoadPriceDataObject.SelectedModelId)) {
				OnRoadPricePopupInitialized = false;
				this.setState(nextProps.onRoadPriceDataObject);	
				
			}
			else if(this.props.location != null && this.props.location != undefined && 
				nextProps.location != null &&  nextProps.location != undefined && 
				this.props.location.hash == '#onRoadPrice' && nextProps.location.hash == '') {
				OnRoadPricePopupInitialized = false;
				this.setState({
					SelectedModelId : 0
				})
			}
	
		}
		catch(e){}

	
		
	}

	

	render() {
		var obj = this.state;
		if(!obj)
			return null;
		if(!OnRoadPricePopupInitialized && obj.SelectedModelId != null && obj.SelectedModelId > 0) {
			OnRoadPricePopupInitialized = true;
			this.initializePQ();
		}
		var citySelectionText = (obj.SelectedCity != null && obj.SelectedCity.name != '') ? obj.SelectedCity.name : 'Select City';
		var areaSelectionText = (obj.SelectedArea != null && obj.SelectedArea.name != '') ? obj.SelectedArea.name : 'Select Area';
		var btnPriceLoaderVisibility = (obj.SelectedCityId > 0 && obj.IsPersistence && (!obj.HasAreas || obj.SelectedAreaId > 0)) ? '' : 'hide';
		var autocompleteMenu = (this.state.state == popupState.areaPopupOpen) ? 
										<CityAreaAutocomplete completeCityAreaList={obj.BookingAreas} type={"area"} areaSelect={this.areaSelect}/> :
										<CityAreaAutocomplete completeCityAreaList={obj.BookingCities} type={"city"} citySelect={this.citySelect}/>
		var  innerContainerHtml =  (<div>
										<div className="city-area-banner"></div>
										<div className="popup-inner-container">
											<div id="onroad-price-close-btn" className="bwmsprite onroad-price-close-btn close-btn position-abt pos-top10 pos-right10 cur-pointer" id="onroad-price-close-btn" onClick={this.closeOnRoadPricePopUp}></div>
											<div id="popupHeading" className="content-inner-block-20">
												<p className="font18 margin-bottom5 text-capitalize">Please Tell Us Your Location</p>
												<div className="text-light-grey margin-bottom5"><span className="red">*</span>Get on-road prices by just sharing your location!</div>
												<div id="citySelection" className="form-control text-left input-sm position-rel margin-bottom10" onClick = {this.showCitySelectionPopup}>
													<div className="selected-city ">
														{citySelectionText}
													</div>
													<span className="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>
												</div>
												<div id="areaSelection" className={"form-control text-left input-sm position-rel margin-bottom10 " + (obj.BookingAreas != null && obj.BookingAreas.length == 0 ? "hide" : "")} onClick={this.showAreaSelectionPopup}>
													<div className="selected-area" >
														{areaSelectionText}
													</div>
													<span className="bwmsprite fa-angle-right position-abt pos-top10 pos-right10"></span>

												</div>

												<div id="btnPriceLoader" className="center-align margin-top20 text-center position-rel">
													<a href="javascript:void(0)" id="btnDealerPricePopup" className={"btn btn-orange btn-full-width font18 " + btnPriceLoaderVisibility} onClick={this.priceLoaderBtnClicked} rel="nofollow">
														Show on-road price
													</a>
												</div>

												<div id="popupContent" className="bwm-city-area-popup-wrapper">
													{autocompleteMenu}
												</div>		            
											</div>

										</div>
									</div>) 
		var loadingTextHtml =   (<div id="popup-loader-container" style={{'display':this.state.LoadingText === ""?'none' : 'block'}}>
									 	<SpinnerRelative/>
									 	<div id="popup-loader-text">
									 		<p className="font14">{this.state.LoadingText}</p>
									 	</div>
								</div>)
		return (
			<div className="bw-city-popup bwm-fullscreen-popup bw-popup-sm text-center hide" id="popupWrapper">
			{innerContainerHtml}
			{loadingTextHtml}
		</div>
		)
	}
}

module.exports = withRouter(OnRoadPricePopup);