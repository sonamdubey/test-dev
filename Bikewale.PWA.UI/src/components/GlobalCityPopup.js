import React from 'react'
import Autocomplete from '../components/Autocomplete'
import {closeGlobalCityPopUpByButtonClick , SetCookieInDays,showElement, autocomplete , showHideMatchError , globalCityCache, getStrippedTerm, getGlobalCity ,highlightText, setGlobalCity} from '../utils/popUpUtils'
import {isServer} from '../utils/commonUtils'
import {GetCatForNav} from '../utils/analyticsUtils'
class GlobalCityPopup extends React.Component {
	constructor(props) {
		super(props);
		var globalCity = getGlobalCity();
		var globalCityId = ( globalCity && globalCity.id>0 ) ? globalCity.id : null;
		var globalCityName = ( globalCity && globalCity.name.length>0 ) ? globalCity.name : '';
		
		this.state = {
			value : globalCityName ,
			cityList : [] ,
			globalCityId : globalCityId, 
			strippedValue : getStrippedTerm(globalCityName)
		}
		this.onChange = this.onChange.bind(this);
		this.onSelect = this.onSelect.bind(this);
		this.submitCity = this.submitCity.bind(this);
		this.closeGlobalCityPopUpByButtonClick = this.closeGlobalCityPopUpByButtonClick.bind(this);
		this.renderMenu = this.renderMenu.bind(this);
		this.renderMenuItem = this.renderMenuItem.bind(this);
	}
	submitCity() {
		var ele = document.getElementById('globalCityPopUp');
        if ((this.state.globalCityId > 0) && (this.state.value != "")) {
            showHideMatchError(ele, false);
            closeGlobalCityPopUpByButtonClick();
        }
        else {
            showHideMatchError(ele, true);
        }
        return false;
	}
	onSelect(state) {
	    var globalCityId = this.state.globalCityId;
	    var city = new Object();
	    city.cityId = state.payload.cityId;
	    city.maskingName = state.payload.cityMaskingName;
	    var cityName = state.label.split(',')[0];
	    setGlobalCity(city.cityId, cityName, globalCityId);
        closeGlobalCityPopUpByButtonClick();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': GetCatForNav(), 'act': 'City_Popup_Default', 'lab': cityName });
        if (city.cityId) {
            location.reload();
        }

	}
	onChange() {
		try {
			var value = document.getElementById('globalCityPopUp').value;
			if(value == undefined || value == null)
				return;
			if(value.replace(/\s/g, '').length == 0) {
	 			this.setState({
					value : value,  
					strippedValue : getStrippedTerm(value),
					cityList : []
				});
	 			return;
	 		}
	 		else {
	 			this.setState({
					value : value,  
					strippedValue : getStrippedTerm(value)
				});	
	 		}
			autocomplete({
				year :'',
				recordCount : 5,
				source : 3,
				afterfetch : this.afterfetch.bind(this),
				cache : globalCityCache

			},value);	
		}catch(err){}
		
	}

	afterfetch(result, searchtext) {
		var element = document.getElementById("globalCityPopUp");
		if (result != undefined && result.length > 0) {
            showHideMatchError(element, false);
            this.setState({cityList : result})
        }
        else {
            showHideMatchError(element, true);
        	this.setState({cityList : [] })
        }


	}
	closeGlobalCityPopUpByButtonClick() {
		closeGlobalCityPopUpByButtonClick();
		var globalCity = getGlobalCity();
		var globalCityId = ( globalCity && globalCity.id>0 ) ? globalCity.id : null;
		var globalCityName = ( globalCity && globalCity.name.length>0 ) ? globalCity.name : '';
		this.setState({
			value : globalCityName ,
			cityList : [] ,
			globalCityId : globalCityId, 
			strippedValue : getStrippedTerm(globalCityName)
		})
	}
	shouldComponentUpdate(nextProps, nextState) {
		var popupElement = document.getElementById('globalcity-popup');
		if(popupElement.style.display == 'none') {
			try{
				if(this.state.value!=null && nextState.value != null && this.state.value !== nextState.value) {
					return true;
				}
			}
			catch(err) {
				return false;
			}
			return false;
		}
		else return true;
		return true;
	}
	renderMenuItem(item,index) {
		return (<li className={"autocomplete-menu--item" + (index == 0 ? " menu--item-focus": "")} onClick={function(){this.onSelect(item)}.bind(this)}>
					<span className="menu--item-icon bwmsprite search-icon"></span>
					<a className="menu--item-label" dangerouslySetInnerHTML={{__html: highlightText(item.label, this.state.strippedValue) }} ></a>
				</li>)
	}
	renderMenu(items, value) {
		if(value == undefined || value == null || items == undefined || items == null)
			return <ul/>
		if(value === '' || (items && items.length == 0)) {
			return <ul/>;
		}
		else {
			return (<ul className="autocomplete-menu menu--full-width-item menu--no-item-icon">
						{
							items.map(function(item,index){
								return this.renderMenuItem(item,index);
							}.bind(this))

						}
					</ul>);
		}
	}
	render() {

		return (
			<div className="globalcity-popup bwm-fullscreen-popup hide" id="globalcity-popup">
		        <div className="globalcity-popup-data text-center">
		            <div id="globalcity-close-btn" className="globalcity-close-btn position-abt pos-top10 pos-right10 bwmsprite cross-lg-lgt-grey cur-pointer" onClick={this.closeGlobalCityPopUpByButtonClick}></div>
		            <p className="font20 margin-bottom5 text-capitalize">Please tell us your city</p>
		            <p className="text-light-grey margin-bottom5">This allows us to provide relevant content for you.</p>
		            <div className="autocomplete-wrapper form-control-box margin-bottom20">
						<Autocomplete
							value={this.state.value}
							items={this.state.cityList}
							inputProps = {{
									placeholder:'Type to select city' ,
									id:'globalCityPopUp' ,
									className:'form-control padding-right30'
								}}
							onChange = {this.onChange}
							renderMenu = {this.renderMenu}
							wrapperStyle = {{
								'width': '100%'
							}}
						/>
						<span id="loaderGlobalCity" className="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style={{'display':'none'}}></span>
						<span id="error-icon" className="bwmsprite error-icon hide"></span>
						<div id="bw-blackbg-tooltip" className="bw-blackbg-tooltip hide">No city found. Try a different search.</div>
		            </div>
		            <div>
		                <button id="btnGlobalCityPopup" className="btn btn-orange btn-full-width font18" onClick={this.submitCity}>Confirm city</button>
		            </div>
		        </div>
		        <div className="clear"></div>
		    </div>)
	}
}

module.exports = GlobalCityPopup;