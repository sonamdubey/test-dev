import React from 'react'
import Autocomplete from '../components/Autocomplete'
import GlobalSearchList from '../components/GlobalSearchList'
import {setDataForPriceQuotePopup , closeGlobalSearchPopUp, recentSearches,autocomplete,showElement, hideElement,getStrippedTerm ,highlightText, globalSearchCache,MakeModelRedirection, setPriceQuoteFlag , globalSearchStatus} from '../utils/popUpUtils'
import { isServer } from '../utils/commonUtils'
import { triggerGA, GetCatForNav } from '../utils/analyticsUtils'
class GlobalSearchPopup extends React.Component {

	constructor(props) {
		super(props);
		this.state = {
		    status : globalSearchStatus.RESET,
            globalDisplay : 'none',
			value : '' ,
			autocompleteList : [] ,
            recentSearchesLoaded: false,
			globalSearchList: { recentSearchList : [] , trendingSearchList : [] },
			strippedValue :  '' 
		}
		this.renderRecentSearchItems = this.renderRecentSearchItems.bind(this);
		this.renderAutocompleteItems = this.renderAutocompleteItems.bind(this);
		this.showAutocompleteList = this.showAutocompleteList.bind(this);
		this.showErrorMessage = this.showErrorMessage.bind(this);
		this.clickClearButton = this.clickClearButton.bind(this);
		this.getListByStatus = this.getListByStatus.bind(this);
		this.loaderStatus = this.loaderStatus.bind(this);
		this.afterfetch = this.afterfetch.bind(this);
		this.checkOnRoadLinkClick = this.checkOnRoadLinkClick.bind(this);
		this.resetListOnEmptyInput = this.resetListOnEmptyInput.bind(this);
		this.renderMenu = this.renderMenu.bind(this);
		this.renderMenuItem = this.renderMenuItem.bind(this);
		this.onChange = this.onChange.bind(this);
		this.onSelect = this.onSelect.bind(this);
		this.focusInput = this.focusInput.bind(this);
		this.afterTrending = this.afterTrending.bind(this);
		this.onSearchIconClick = this.onSearchIconClick.bind(this);
	}
	componentWillMount() {
	    try{
	        this.resetListOnEmptyInput();
	    }
        catch(err){}
	}
	componentDidMount() {
		try{
		    this.focusInput();
		}catch(err){}
	}
	focusInput() {
		document.getElementById('globalSearch').focus();
	}

	onSearchIconClick() {
	    var value = this.state.value;
	    var category = GetCatForNav();
	    var label = "Recently_Viewed_Bikes_" + (this.state.recentSearchesLoaded ? "Present" : "Not_Present");
	    if(value.trim() == '') {
	        this.resetListOnEmptyInput();
	        triggerGA(category, "Search_Bar_Clicked", label);
	    }
	}
	
	showAutocompleteList(autocompleteList) {
		this.setState({
			status: globalSearchStatus.AUTOCOMPLETE ,
			autocompleteList : autocompleteList
		})		
	}
	
	showErrorMessage() {
		this.setState({
			status: globalSearchStatus.ERROR 
		})
	}
	onChange() {
		try{
			var value = document.getElementById('globalSearch').value;
			
			if(value.replace(/\s/g, '').length == 0) {
	 			this.resetListOnEmptyInput();
			    hideElement(document.getElementById('gs-text-clear'));
		    	return;
			}
	 		else {
	 			this.setState({
					value : value,
					strippedValue : getStrippedTerm(value)
				})
	 		}
			autocomplete({
				year : '' , 
				recordCount : 5 ,
				source : 1 ,
				afterfetch : this.afterfetch,
				cache : globalSearchCache,
				loaderStatus : this.loaderStatus
			},value)

		}catch(err){}
	}
	resetListOnEmptyInput() {
	    var recentSearchList = recentSearches.getRecentSearches();
	    if(recentSearchList !=null) {
	        recentSearchList = recentSearchList.filter((item) => { return (item != null && typeof item != "undefined")}).map(function (item) {
                return { label: item.name, payload: item }
            });
            this.setState({ recentSearchesLoaded: true, value: ""});
	    }
	    var trendingSearchList = recentSearches.getTrendingSearches(this.afterTrending);
	    if(trendingSearchList !=null) {
	        trendingSearchList = trendingSearchList.filter((item) => { return (item != null && typeof item != "undefined")}).map(function (item) {
                return { label: item.BikeName, payload: {'expertReviewsCount':"0", 'modelId': item.objModel.modelId.toString(), 'modelMaskingName': item.objModel.maskingName, 'makeId': item.objMake.makeId.toString(), 'makeMaskingName' : item.objMake.maskingName, 'isNew' : "True", 'name' : item.BikeName} }
	        });
	       }
	    if(recentSearchList === null || recentSearchList.length === 0) {
	        recentSearchList = [];
	    }
	    if(trendingSearchList === null || trendingSearchList.length === 0) {
	        trendingSearchList = [];
	    }
        if(trendingSearchList.length !== 0 || recentSearchList.length !== 0) {
			this.setState({
				status: globalSearchStatus.RECENTSEARCH ,
                globalSearchList: { recentSearchList: recentSearchList, trendingSearchList: trendingSearchList },
                value: ""
			});
			
		}
		else {
            this.setState({
                status: globalSearchStatus.RESET, value: ""
			});
			
		}
	}
	loaderStatus(status) {
		if (!status) {
            showElement(document.getElementById('loaderGlobalSearch'));
        }
        else {
            hideElement(document.getElementById('loaderGlobalSearch'));
            document.getElementById('gs-text-clear').style.display = 'inline';
        }
	}
	clickClearButton() {
	    this.setState({ value : ''});
		this.resetListOnEmptyInput();
		hideElement(document.getElementById('gs-text-clear'));
		this.focusInput();
	}

	onSelect(state) {
		var keywrd = state.label + '_' + this.state.value;
		var category = GetCatForNav();
        triggerGA(category, 'Search_Keyword_Present_in_Autosuggest', keywrd);
      	this.setState({
      		value : state.label,
			strippedValue : getStrippedTerm(state.label)
		})
        MakeModelRedirection(state); 
	}

	afterfetch(result,searchtext) {
		if (result != undefined && result.length > 0 && searchtext.trim() != "") {
			this.showAutocompleteList(result);
	
        }
        else {
        	this.setState({
        		focusedMakeModel : null
        	})
            if (searchtext.trim() != "") {
            	this.showErrorMessage()
            }
            var keywrd = this.state.value;
            var category = GetCatForNav();
            triggerGA( category, 'Search_Keyword_Not_Present_in_Autosuggest', keywrd);
        }
	}

	afterTrending() {
	    this.resetListOnEmptyInput();
	}

	checkOnRoadLinkClick (item,event) {
		event.preventDefault();
		setDataForPriceQuotePopup(event,item);
		setPriceQuoteFlag();

	}
	renderRecentSearchItems(item) {
			var rightItem = null;
			var bikename = item.name || '';
			if (item.modelId > 0) {
                if (item.futuristic == 'True') {
                    rightItem  = <span className="menu--right-align-label">coming soon</span>;
                } else {
                    if (item.isNew == 'True') {                       
                        rightItem = <a data-pqSourceId="38" data-modelId={item.modelId} className="getquotation menu--right-align-label text-blue" onClick={this.checkOnRoadLinkClick.bind(this,item)}>Check On-Road Price</a>;

                    } else {
                       	rightItem = <span className="menu--right-align-label">discontinued</span>;
                    }
                }
                
            }
			return (
				<li data-makeid={item.makeId} data-modelid={item.modelId} className="autocomplete-menu--item" onClick={function(){this.onSelect(item)}.bind(this)}>
					<span className="menu--item-icon bwmsprite history-icon"></span>
			  		<a className="menu--item-label" href="javascript:void(0)" data-href={'/m/'+item.makeMaskingName+'-bikes/'+item.modelMaskingName} rel="nofollow"> 
			  			{bikename}
			  		</a>
			  		{rightItem}
				</li>
				
				)
	}
	renderAutocompleteItems(item,index) {
			var rightItem = null;
			if (item.payload.modelId > 0) {
	            if (item.payload.futuristic == 'True') {
	                rightItem = <span className="menu--right-align-label">coming soon</span>;
	            } else {
	                if (item.payload.isNew == 'True') {	                    
	                	rightItem = <a href="javascript:void(0)" data-pqSourceId="38" data-modelId={item.payload.modelId} className="getquotation menu--right-align-label text-blue" onClick={this.checkOnRoadLinkClick.bind(this,item)}>Check On-Road Price</a>;
	                }
	                else {
	                    rightItem = <span className="menu--right-align-label">discontinued</span>;
	                }

	            }
	        }
			return (
				<li className={"autocomplete-menu--item" + (index == 0 ? " menu--item-focus": "")} onClick={function(){this.onSelect(item)}.bind(this)}>
					<span className="menu--item-icon bwmsprite search-icon"></span>
			  		<a className="menu--item-label" dangerouslySetInnerHTML={{__html: highlightText(item.label, this.state.strippedValue) }}></a>
			  		{rightItem}
				</li>
			)	
	}
	getListByStatus() {
		if(this.state.status == globalSearchStatus.RESET) 
			return [];
		else if(this.state.status == globalSearchStatus.ERROR) 
			return [];
		else  if(this.state.status == globalSearchStatus.AUTOCOMPLETE) 
			return this.state.autocompleteList;
		else return [];
	}
	shouldComponentUpdate(nextProps, nextState) {
		var popupElement = document.getElementById('global-search-popup');
		if(popupElement.style.display == 'none') {
			if(this.state.status !== nextState.status)
				return true;
			return false;
		}
		else return true;
		return true;
	}
	renderMenuItem(item, index) {
  		if(this.state.status === globalSearchStatus.AUTOCOMPLETE) {
  			return this.renderAutocompleteItems(item,index);
  		}
  		else if(this.state.status === globalSearchStatus.RECENTSEARCH) {
  			return this.renderRecentSearchItems(item);
  		}
	}
	renderMenu(items, value) {
	  	if(this.state.status === globalSearchStatus.RESET) {
	  		return <ul/>;
	  	}
	  	else if(this.state.status === globalSearchStatus.ERROR) {
	  		return (<ul className="autocomplete-menu">
		  				<li className="autocomplete-menu--item">
		                    <strong>Oops! No suggestions found</strong>
		                    <br />
		                    <span className="text-light-grey font12">Search by bike name e.g: Honda Activa</span>
		                </li>
		            </ul>)
	  	}
	  	else {
	  		return (<ul className="autocomplete-menu">
	  				{
	  					items.map(function(item,index){
	  						return this.renderMenuItem(item,index)
	  					}.bind(this))
	  				}
	  				</ul>);
	  	}
	  	
	}
	render() {
		var globalSearchVisibility = 'none';
		if(!isServer()) {
			var popupElement = document.getElementById('global-search-popup');
			globalSearchVisibility = (popupElement && popupElement.style) ? popupElement.style.display : 'none' ;
		}
		return (
			<div id="global-search-popup" className="global-search-popup" style={{'display':globalSearchVisibility}}>
		        <div className="form-control-box">
		        	<span className="back-arrow-box" id="gs-close" onClick={closeGlobalSearchPopUp}>
		                <span className="bwmsprite back-long-arrow-left"></span>
		            </span>
		            <span className="cross-box hide" id="gs-text-clear" onClick={this.clickClearButton}>
		                <span className="bwmsprite cross-md-dark-grey"></span>
		            </span>
			        <Autocomplete
					  value={this.state.value}
					  items={this.getListByStatus()}
					  inputProps = {{
			        			placeholder:'Search' ,
			        			id:'globalSearch' ,
			        			className:'form-control padding-right30'
			        		}}
					  onClick = {this.onSearchIconClick}
			          onChange = {this.onChange}
					  renderMenu = {this.renderMenu}
						wrapperStyle = {{
							'width': '100%'
						}}
					/>
					<span id="loaderGlobalSearch" className="fa fa-spinner fa-spin position-abt pos-right10 pos-top15 text-black" style={{'display':'none','right':'35px','top':'13px'}}></span>
		            <GlobalSearchList searchProps = {{ className:'global-search-section bg-white' }} styleProps={{display:this.state.status === globalSearchStatus.RECENTSEARCH?'block':'none' }} searchItems = { this.state.globalSearchList }/>
		        	
		        </div>
		    </div>
		        
		)
	}
}

module.exports = GlobalSearchPopup;
