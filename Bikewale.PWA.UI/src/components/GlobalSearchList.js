import React from 'react'
import {setDataForPriceQuotePopup , closeGlobalSearchPopUp, recentSearches, setPriceQuoteFlag, getStrippedTerm, MakeModelRedirection} from '../utils/popUpUtils'
import SearchList from '../components/SearchList'
import {GetCatForNav} from '../utils/analyticsUtils'
class GlobalSearchList extends React.Component {
    constructor(props) {
		super(props);
		this.renderListItem = this.renderListItem.bind(this);
		this.renderRecentSearchList = this.renderRecentSearchList.bind(this);
		this.checkOnRoadLinkClick = this.checkOnRoadLinkClick.bind(this);
    }
    checkOnRoadLinkClick (item,event) {
        event.preventDefault();
        setDataForPriceQuotePopup(event,item);
        setPriceQuoteFlag();

    }
    onSelect(state) {
        var keywrd = state.label + '_' + this.state.value;
        var category = GetCatForNav();
        dataLayer.push({ 'event': 'Bikewale_all', 'cat': category, 'act': 'Search_Keyword_Present_in_Autosuggest', 'lab': keywrd });
        this.setState({
            value : state.label,
            strippedValue : getStrippedTerm(state.label)
        })
        MakeModelRedirection(this.state.value,state); 
    }
    renderListItem(item) {
        var rightItem = null;
        var bikename = item.payload.name || '';
        if (item.payload.modelId > 0) {
            if (item.payload.futuristic == 'True') {
                rightItem  = <span className="menu--right-align-label">coming soon</span>;
            } else {
                if (item.payload.isNew == 'True') {
                    rightItem = <a data-modelId={item.modelId} className="getquotation menu--right-align-label text-blue" onClick={this.checkOnRoadLinkClick.bind(this,item)}>Check On-Road Price</a>;

                } else {
                    rightItem = <span className="menu--right-align-label">discontinued</span>;
                }
            }
                
        }
        return (
                <li data-makeid={item.payload.makeId} data-modelid={item.payload.modelId} className="autocomplete-menu--item" onClick={function(){this.onSelect(item)}.bind(this)}>
                    <span className="menu--item-icon bwmsprite history-icon"></span>
                    <a className="menu--item-label" href="javascript:void(0)" data-href={'/m/'+item.payload.makeMaskingName+'-bikes/'+item.payload.modelMaskingName} rel="nofollow"> 
                        {bikename}
                    </a>
                {rightItem}
                </li>
            )
    }
    renderRecentSearchList(items, value) {
        if(items !=null && items.length>0)
            return (
	  				    items.map(function(item,index){
	  				        return this.renderListItem(item,index)
	  				    }.bind(this))
            );
    }
    render() {
		
        var searchProps = {};
        if(this.props.searchProps) {
            searchProps = this.props.searchProps;
        }

        return(
				<div className={searchProps.className ? searchProps.className:''} style={{'display': this.props.styleProps.display}}>
                    <SearchList inputProps = {{ id:'history-search', titleClass:'search-title', title:'Recently Viewed', ulId:'global-recent-searches'}} items={this.props.searchItems.recentSearchList} renderRecentSearchList={this.renderRecentSearchList}></SearchList>
                    <SearchList inputProps = {{ id:'trending-search', titleClass:'search-title', title:'Trending Searches', ulId:'trending-bikes'}} items={this.props.searchItems.trendingSearchList} renderRecentSearchList={this.renderRecentSearchList}></SearchList>
                </div>
			)
    }
}

module.exports = GlobalSearchList