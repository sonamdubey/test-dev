import React from 'react'
import SearchList from '../components/SearchList'
class GlobalSearchList extends React.Component {
    constructor(props) {
		super(props);
    }
    render() {
		
        var searchProps = {};
        if(this.props.searchProps) {
            searchProps = this.props.searchProps;
        }

        return(
				<div className={searchProps.className ? searchProps.className:''}>
                    <SearchList inputProps = {{ id:'history-search', titleClass:'search-title', title:'Recently Viewed', ulId:'global-recent-searches'}} items={this.props.searchItems.recentSearchList}></SearchList>
                    <SearchList inputProps = {{ id:'trending-search', titleClass:'search-title', title:'Trending Searches', ulId:'trending-bikes'}} items={this.props.searchItems.trendingSearchList}></SearchList>
                </div>
			)

    }
}

module.exports = GlobalSearchList