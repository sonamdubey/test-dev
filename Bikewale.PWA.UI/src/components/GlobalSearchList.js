import React from 'react'
import {setDataForPriceQuotePopup , closeGlobalSearchPopUp, recentSearches, setPriceQuoteFlag, getStrippedTerm, MakeModelRedirection} from '../utils/popUpUtils'
import SearchList from '../components/SearchList'
import {triggerGA, GetCatForNav} from '../utils/analyticsUtils'
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
    onSelect(item, value) {
        var category = GetCatForNav();
        if(value === 1)
            triggerGA(category,  'Recently_View_Search_Bar_Clicked', item.payload.name);
        else if(value === 2)
            triggerGA(category, 'Trending_Searches_Search_Bar_Clicked', item.payload.name);
        else if(value === 3)
            triggerGA(category, 'TrackDay_2018_Link Clicked', 'Track Day 2018');
        MakeModelRedirection(item); 
    }
    renderListItem(item, index, value) {
        var rightItem = null;
        var bikename = item.payload.name || '';
        var icon;
        if(value === 1){
            icon = <span className="menu--item-icon bwmsprite history-icon"></span>;
        }
        else if(value ===2 ) {
            icon = <span className="menu--item-icon trending-icon"></span>;
        }

        if (item.payload.modelId > 0) {
            if (item.payload.futuristic == 'True') {
                rightItem  = <span className="menu--right-align-label">coming soon</span>;
            } else {
                if (item.payload.isNew == 'True') {
                    rightItem = <a data-modelid={item.modelId} className="getquotation menu--right-align-label text-blue upcoming-link" onClick={this.checkOnRoadLinkClick.bind(this,item)}>Check On-Road Price</a>;

                } else {
                    rightItem = <span className="menu--right-align-label">discontinued</span>;
                }
            }
                
        }
        if(item.payload.type === "4"){
            return(
                <li key={index} data-makeid={item.payload.makeId} data-modelid={item.payload.modelId} className="autocomplete-menu--item" onClick={function(){this.onSelect(item,value)}.bind(this)}>
                    {icon}
                    <a className="menu--item-label" href="javascript:void(0)" data-href={'/m/electric-bikes/'} rel="nofollow"> 
                        {bikename}
                    </a>
                </li>
            )
        }
        else if(item.payload.modelId === "0" && item.payload.makeId === "0") {
            return (
                <li key={index} data-makeid={item.payload.makeId} data-modelid={item.payload.modelId} className="autocomplete-menu--item" onClick={function(){this.onSelect(item, 3)}.bind(this)}>
                    {icon}
                    <a className="menu--item-label" href={'https://www.bikewale.com/'+item.payload.href} data-href={'/'+item.payload.href} rel="nofollow"> 
                        {bikename}
                    </a>
                </li>
            )
        }
        else {
            var bodyType = item.payload.type === "3" ? '-scooters/' : '-bikes/';
            return(
                <li key={index} data-makeid={item.payload.makeId} data-modelid={item.payload.modelId} className="autocomplete-menu--item" onClick={function(){this.onSelect(item,value)}.bind(this)}>
                    {icon}
                    <a className="menu--item-label" href="javascript:void(0)" data-href={'/m/'+item.payload.makeMaskingName+bodyType+item.payload.modelMaskingName} rel="nofollow"> 
                        {bikename}
                    </a>
                    {rightItem}
                </li>
            )
        }
        
    }
    renderRecentSearchList(items, value) {
        if(items !=null && items.length>0)
            return (
	  			items.map(function(item,index){
	  				return this.renderListItem(item, index, value)
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
                    <SearchList inputProps = {{ id:'history-search', titleClass:'search-title', title:'Recently Viewed', ulId:'global-recent-searches'}} items={this.props.searchItems.recentSearchList} renderRecentSearchList={this.renderRecentSearchList} value={1}></SearchList>
                    <SearchList inputProps = {{ id:'trending-search', titleClass:'search-title', title:'Trending Searches', ulId:'trending-bikes'}} items={this.props.searchItems.trendingSearchList} renderRecentSearchList={this.renderRecentSearchList} value={2}></SearchList>
                </div>
			)
    }
}

module.exports = GlobalSearchList