import React from 'react'
import {withRouter} from 'react-router-dom'
import PropTypes from 'prop-types'
import ArticleList from './ArticleList'
import NewsPagination from './NewsPagination'
import SpinnerRelative from '../Shared/SpinnerRelative'
import Breadcrumb from '../Shared/Breadcrumb'
import NewBikes from '../NewBikes'
import Footer from '../Shared/Footer'
import AdUnit from '../AdUnit'
import { NewsArticlesPerPage, Status, AD_PATH_NEWS_MOBILE_TOP_320_50, AD_DIV_REVIEWS_TOP_320_50, AD_DIV_REVIEWS_MIDDLE_300_250, AD_PATH_NEWS_MOBILE_MIDDLE_300_250, AD_PATH_NEWS_MOBILE_BOTTOM_320_50, AD_DIV_REVIEWS_BOTTOM_320_50, AD_DIMENSION_320_50, AD_DIMENSION_300_250} from '../../utils/constants'
import { isServer, CMSUserReviewSlugPosition, CMSUserReviewSlugData } from '../../utils/commonUtils'
import { getGlobalCity } from '../../utils/popUpUtils'

import { scrollPosition , resetScrollPosition , isBrowserWithoutScrollSupport } from '../../utils/scrollUtils'
import {addAdSlot , removeAdSlot} from '../../utils/googleAdUtils'
import {endTimer} from '../../utils/timing'

if(!process.env.SERVER) {
    require('../../../stylesheet/news.sass');
}

class ArticleListComponent extends React.Component{
    propTypes : {
        history: PropTypes.object.isRequired,
        ArticleListData : React.PropTypes.object,
        NewBikesListData : React.PropTypes.object
    };
    constructor(props){
        super(props);
        if(isServer()) {

            if(this.props.ArticleListData && this.props.ArticleListData.ArticleList) {
                this.props.ArticleListData.Status = Status.Fetched;
                this.props.ArticleListData.PageNo = Math.floor(parseInt(this.props.ArticleListData.ArticleList.StartIndex) / parseInt(NewsArticlesPerPage)) +  1; // corner case - last page with less than NewsArticlesPerPage articles
                
            }
            if(this.props.NewBikesListData && this.props.NewBikesListData.NewBikesList) {
                this.props.NewBikesListData.Status = Status.Fetched;
            }
            
        }

        var globalCity = getGlobalCity();
        this.globalCityName = (globalCity && globalCity.name.length > 0) ? globalCity.name : '';

        this.updateArticleList = this.updateArticleList.bind(this);
        this.onArticleClickEvent = this.onArticleClickEvent.bind(this);
       
    }
    componentDidUpdate() {
        var pageNoFromData = this.props.ArticleListData.PageNo; // if no pageno then -1
        var pageNoFromUrl = this.props.match.params["pageNo"] ? this.props.match.params["pageNo"] : 1;
        if(pageNoFromData == pageNoFromUrl) {
            this.logger();
            this.scrollToPosition();    
        }
                
        
    }
    componentDidMount() {
        this.logger();
  

        if(this.props.ArticleListData && this.props.ArticleListData.Status == Status.Fetched) {
            //bikesList has been fetched
            if(this.props.NewBikesListData && this.props.NewBikesListData.Status == Status.Fetched) {
                return;
            }
            else {
                // bikesList to be fetched
                this.props.fetchNewBikesListData();
            }
        }
        else {
            //both data needs to be fetched
            this.props.fetchArticleList(this.props.ArticleListData.PageNo); // pageNo is initially -1
            this.props.fetchNewBikesListData();

        }
        
        if(isBrowserWithoutScrollSupport()) {
            window.scrollTo(0,0);
        }
    }
    componentWillReceiveProps (nextProps) {
        try {
            var prevUrlParam = this.props.match.params;
            var nextUrlParam = nextProps.match.params;
            //componentWillRecieveProps is called on first load in UC Browser and iOS Chrome, not in other browsers
            if(prevUrlParam["pageNo"] === nextUrlParam["pageNo"]) { // condition 1 : new url has been pushed
                return;
            }
            var newHashValue = this.props.location.hash;
            var oldHashValue = nextProps.location.hash;
            if((newHashValue && newHashValue.indexOf('#') >= 0) || (oldHashValue && oldHashValue.indexOf('#') >= 0) ) {
                // condition 2 :  global city popup  clicked -- should not call apis
                return;
            }
            
            if(nextProps &&
                (nextProps.ArticleListData.Status != Status.IsFetching)) {
                    // condition 3 : back / forward button has been clicked, so all the dataobjects have fetched state from previous url. hence now new api have to be hit
                    nextProps.fetchArticleList(-1);
                    nextProps.fetchNewBikesListData();
                    if(isBrowserWithoutScrollSupport()) {
                        window.scrollTo(0,0);
                    }
            }  
           
        }
        catch(e) {}
        
    }
    componentWillUnmount() {
        this.props.resetArticleListData();
        
    }
    updateArticleList(PageNo) {
        this.props.fetchArticleList(PageNo);
        this.props.fetchNewBikesListData();
        if(isBrowserWithoutScrollSupport()) {
            window.scrollTo(0,0);
        }
        
    }
    onArticleClickEvent(ArticleInitialData) {
        if(!ArticleInitialData || !ArticleInitialData.ArticleUrl)
            return;
        this.props.history.push(ArticleInitialData.ArticleUrl) 
        this.props.fetchArticleDetail(ArticleInitialData);
        
    }
    scrollToPosition() {
        if(scrollPosition.x >= 0 && scrollPosition.y >= 0) { // needs to be scrolled
            if(this.props.ArticleListData && this.props.ArticleListData.Status == Status.Fetched) { // checks whether ready to scroll
                window.scrollTo(scrollPosition.x,scrollPosition.y);
                resetScrollPosition();
                
            }
        }




    }
    logger() {
        try {
            if(this.props &&
                this.props.ArticleListData.Status == Status.Fetched &&
                this.props.NewBikesListData.Status == Status.Fetched) {
                    endTimer("component-render")
            }
        }
        catch(err) {
            
        }
    }
    renderNewBikesList() {
		 	if( !this.props.NewBikesListData || this.props.NewBikesListData.Status !== Status.Fetched ) {
		 		return false;
		 	}
    		return (
    				this.props.NewBikesListData.NewBikesList.map(function(newBikes) {
    						return (<NewBikes key={newBikes.Heading} newBikesData={newBikes}/>)
    				})
    		)
    }
    render() {
        var componentData = this.props.ArticleListData;
        
        var loadingState = (<div>
                                <SpinnerRelative/>
                            </div>)
        
        if(!componentData || componentData.Status == Status.Reset || componentData.Status == Status.IsFetching || componentData.Status == Status.Error) {
            return loadingState;				
        }

        let targetTags = {
        	City: this.globalCityName
        }
				
        var adSlotTop = <AdUnit uniqueKey={componentData.PageNo} tags={targetTags} adSlot={AD_PATH_NEWS_MOBILE_TOP_320_50} adDimension={AD_DIMENSION_320_50} adContainerId={AD_DIV_REVIEWS_TOP_320_50}/> ;
        var adSlotMiddle = <AdUnit uniqueKey={componentData.PageNo} tags={targetTags} adSlot={AD_PATH_NEWS_MOBILE_MIDDLE_300_250} adDimension={AD_DIMENSION_300_250} adContainerId={AD_DIV_REVIEWS_MIDDLE_300_250} />; 
        var adSlotBottom = <AdUnit uniqueKey={componentData.PageNo} tags={targetTags} adSlot={AD_PATH_NEWS_MOBILE_BOTTOM_320_50} adDimension={AD_DIMENSION_320_50} adContainerId={AD_DIV_REVIEWS_BOTTOM_320_50} />;
        
        return (<div>
                    {adSlotTop}
                    <div className="container bg-white box-shadow section-bottom-margin">
                        <h1 className="box-shadow card-heading">Bike News</h1>
                        <ArticleList articleList={componentData.ArticleList.Articles} 
                                     pageNo = {componentData.PageNo}
                                     onArticleClickEvent={this.onArticleClickEvent}/>
                        <NewsPagination startIndex={componentData.ArticleList.StartIndex} 
                                        endIndex={componentData.ArticleList.EndIndex}
                                        pageNo={componentData.PageNo}
                                        articleCount={componentData.ArticleList.RecordCount}
                                        articlePerPage={NewsArticlesPerPage}
                                        updateArticleList={this.updateArticleList}/>
                    </div>
                    <div className="margin-bottom15">
                        {adSlotMiddle}
                    </div>
                    {this.renderNewBikesList()}
                    <div className="margin-bottom15">
                        {adSlotBottom}
                    </div>
                    <Breadcrumb breadcrumb={[{Href : '/m/',Title : 'Home'},{Href : '',Title : 'News'}]}/>
                    <Footer/>
                </div>
        )
    }
}


export default withRouter(ArticleListComponent);
