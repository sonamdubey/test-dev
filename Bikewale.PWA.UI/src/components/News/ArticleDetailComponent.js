import React from 'react'
import {withRouter} from 'react-router-dom'
import PropTypes from 'prop-types'
import SocialMediaSlug from '../Shared/SocialMediaSlug'
import ArticleDetailContent  from './ArticleDetailContent'
import ArticleDetailPagination from './ArticleDetailPagination'
import ArticleDetailTitle from './ArticleDetailTitle'
import SpinnerRelative from '../Shared/SpinnerRelative'
import Breadcrumb from '../Shared/Breadcrumb'
import NewBikes from '../NewBikes'
import ModelSlug from './ModelSlug'
import Footer from '../Shared/Footer'
import { isServer } from '../../utils/commonUtils'
import {mapNewsArticleDataToInitialData} from './NewsCommon'
import {addAdSlot , removeAdSlot} from '../../utils/googleAdUtils'
import { scrollPosition , resetScrollPosition , isBrowserWithoutScrollSupport } from '../../utils/scrollUtils'
import { getGlobalCity } from '../../utils/popUpUtils'

import {endTimer} from '../../utils/timing'
import AdUnit from '../AdUnit'
import { Status, GA_PAGE_MAPPING,AD_PATH_NEWS_MOBILE_BOTTOM_320_50, AD_DIV_REVIEWS_BOTTOM_320_50, AD_PATH_NEWS_MOBILE_TOP_320_50, AD_DIV_REVIEWS_TOP_320_50, AD_DIMENSION_320_50} from '../../utils/constants'


class ArticleDetail extends React.Component {
    propTypes : {
        history: PropTypes.object.isRequired,
        ArticleDetailData :  React.PropTypes.object,
        RelatedModelObject : React.PropTypes.object,
        NewBikesListData : React.PropTypes.object
    };
    constructor(props) {
        super(props)
        
        if(isServer()) {
            if(this.props.ArticleDetailData && this.props.ArticleDetailData.ArticleDetail) {
                this.props.ArticleDetailData.Status = Status.Fetched;
            }
            if(this.props.NewBikesListData && this.props.NewBikesListData.NewBikesList) {
                this.props.NewBikesListData.Status = Status.Fetched;
            }
            if(this.props.RelatedModelObject && this.props.RelatedModelObject.ModelObject) {
                this.props.RelatedModelObject.Status = Status.Fetched;
            }
        }

        var globalCity = getGlobalCity();
        this.globalCityName = (globalCity && globalCity.name.length > 0) ? globalCity.name : '';

        this.extractBasicIdFromArticleUrl = this.extractBasicIdFromArticleUrl.bind(this);
        if(typeof(gaObj)!="undefined")
        {
            gaObj = GA_PAGE_MAPPING["DetailsPage"];
        }  
    }
    componentDidUpdate() {
        var basicIdFromData = this.props.ArticleDetailData && this.props.ArticleDetailData.ArticleDetail ? this.props.ArticleDetailData.ArticleDetail.BasicId : null;
        var basicIdFromUrl = this.props.match.params["basicId"] ? this.props.match.params["basicId"] : -1  ;
        if(basicIdFromData == basicIdFromUrl) {
            this.logger();
            this.scrollToPosition();
        }
        
    }
    componentDidMount() {    

        this.logger();

        var basicId = this.extractBasicIdFromArticleUrl();
        if(!basicId) {
            return;
        }

        
        if(this.props.ArticleDetailData && this.props.ArticleDetailData.Status == Status.Fetched) { // data to be further rendered after server render
            if(this.props.RelatedModelObject && this.props.RelatedModelObject.Status !== Status.Fetched) {
                this.props.fetchRelatedModelObject(basicId);
            }
            if(this.props.NewBikesListData && this.props.NewBikesListData.Status !== Status.Fetched) {
                this.props.fetchNewBikesListData(basicId);
            }
        }
        else {  // data that is purely client rendered
            var articleInitialData = (this.props.ArticleDetailData.InitialDataDict == null) ? null : this.props.ArticleDetailData.InitialDataDict[window.location.pathname];
            if(!articleInitialData) {
                this.props.fetchArticleDetail(basicId);  //initialdata is never null as initialData is always set from previous page or entire store is server rendered
            }
            //if initialdata != null then fetchArticleDetail already performed by previous page
            this.props.fetchRelatedModelObject(basicId);
            this.props.fetchNewBikesListData(basicId);
        }


        if(isBrowserWithoutScrollSupport()) {
            window.scrollTo(0, 0); 
        }
        

    }
    componentWillReceiveProps (nextProps) {
        try {
            var prevUrlParam = this.props.match.params;
            var nextUrlParam = nextProps.match.params;
            //componentWillRecieveProps is called on first load in UC Browser and iOS Chrome, not in other browsers
            if(prevUrlParam["basicId"] === nextUrlParam["basicId"]) { // condition 1 : new url has been pushed
                return;
            }
            var newHashValue = this.props.location.hash;
            var oldHashValue = nextProps.location.hash;
            if((newHashValue && newHashValue.indexOf('#') >= 0) || (oldHashValue && oldHashValue.indexOf('#') >= 0) ) {
                // condition 2 :  global city popup  clicked -- should not call apis
                return;
            }
            
            if(nextProps && 
                (nextProps.ArticleDetailData.Status != Status.IsFetching) ) {
                    // condition 3 : back / forward button has been clicked, so all the dataobjects have fetched state from previous url. hence now new api have to be hit
                    var articleInitialData = nextProps.ArticleDetailData.InitialDataDict[window.location.pathname];
                    var basicId = (articleInitialData == null) ? this.extractBasicIdFromArticleUrl() : articleInitialData.BasicId;
                    if(!articleInitialData) { // not previously visited
                        nextProps.fetchArticleDetail(basicId);
                    }
                    else {
                        nextProps.fetchArticleDetail(articleInitialData);
                    }
                    nextProps.fetchRelatedModelObject(basicId);
                    nextProps.fetchNewBikesListData(basicId);
                    if(isBrowserWithoutScrollSupport()) {
                        window.scrollTo(0,0);
                    }
                    
            } 
            
        }
        catch(e){

        }
    
    }
    
    scrollToPosition() {
        if(scrollPosition.x >= 0 && scrollPosition.y >= 0) { // needs to be scrolled
            if(this.props.ArticleDetailData && this.props.ArticleDetailData.Status == Status.Fetched) { // checks whether ready to scroll
                window.scrollTo(scrollPosition.x,scrollPosition.y);
                resetScrollPosition();
                
            }
        }
    }
    logger() {
        try {
            if(this.props && 
                this.props.ArticleDetailData.Status == Status.Fetched && 
                this.props.RelatedModelObject.Status == Status.Fetched &&
                this.props.NewBikesListData.Status ==  Status.Fetched) {
                    endTimer("component-render");
            }   
        }
        catch(err) {

        }
    }
    extractBasicIdFromArticleUrl() {
        var basicId ; 
        var url = window.location.pathname;
        var regexp = /\/m\/news\/(\d+)-.*\.html/;
        var matches = url.match(regexp);
        if(matches) {
            return matches[1];
            
        }
    }
    
    onArticlePaginationClickEvent(articleInitialData) {
        this.props.history.push(articleInitialData.ArticleUrl) 
        this.props.fetchArticleDetail(articleInitialData);
        this.props.fetchNewBikesListData(articleInitialData.BasicId); 
        this.props.fetchRelatedModelObject(articleInitialData.BasicId);
        if(isBrowserWithoutScrollSupport()) {
            window.scrollTo(0,0);  
        } 
    }
    renderImage(title,src) {
        return (<img alt={title} title={title} src={src}/>)
    }
    renderArticleContent(articleDetail,initialData) { 
        if(articleDetail) 
        {   
            var imageUrl = (!articleDetail.HostUrl || !articleDetail.LargePicUrl) ? 'https://imgd.aeplcdn.com/640x348/bikewaleimg/images/noimage.png?q=70' : articleDetail.HostUrl + articleDetail.LargePicUrl;
            var imageTag = this.renderImage(articleDetail.Title , imageUrl);
            return (
                <div>
                    <div className="article-content">
                        <div className="article-content-image-wrapper">
                            {imageTag}
                        </div>
                        <ArticleDetailContent htmlContent={articleDetail.Content}/>

                    </div>
                    {this.renderModelSlug()}
                    <SocialMediaSlug/>
                    <ArticleDetailPagination prevArticle={articleDetail.PrevArticle} nextArticle={articleDetail.NextArticle} onArticlePaginationClickEvent={this.onArticlePaginationClickEvent.bind(this)}/>
                </div>                        
            )
        }
        else if(initialData) {
            var imageUrl = (!initialData.HostUrl || !initialData.LargePicUrl) ? 'https://imgd.aeplcdn.com/640x348/bikewaleimg/images/noimage.png?q=70' : initialData.HostUrl + initialData.LargePicUrl;
            
            var imageTag = this.renderImage(initialData.Title , imageUrl);
            return(
                <div>
                    <div className="article-content">
                        {imageTag}
                        <SpinnerRelative/>
                    </div>
                </div>
            )
        }
        else {
            return <SpinnerRelative/>;
            
        }
    }
    renderNewBikesList() {
        if(!this.props.NewBikesListData || this.props.NewBikesListData.Status !== Status.Fetched ) {
            return false;
        }
        return (
            this.props.NewBikesListData.NewBikesList.map(function(newBikes) {
                return (<NewBikes key={newBikes.Heading} newBikesData={newBikes}/>)
            })
        )
    }
    renderModelSlug() {
        if(!this.props.RelatedModelObject || this.props.RelatedModelObject.Status !== Status.Fetched || !this.props.RelatedModelObject.ModelObject)
            return false;
        return (
            <ModelSlug modelInfo={this.props.RelatedModelObject}/>
        )
    }
    renderBreadcrumb(title) {
        if(this.props.ArticleDetailData && this.props.ArticleDetailData.Status == Status.Fetched) 
        {
            return (<Breadcrumb breadcrumb={[{Href : '/m/',Title : 'Home'},{Href : '/m/news/',Title : 'News'},{Href : '',Title : title}]}/>);
        }
        else {
            return false;
        }
    }
    renderFooter() {
        if(this.props.ArticleDetailData && this.props.ArticleDetailData.Status == Status.Fetched) 
        {
            return (<Footer/>);
        }
        else {
            return false;
        }
    }
    render() {
         
        var componentData = this.props.ArticleDetailData;
        if(!componentData) {
            return null;
        }
        var articleInitialData;
        
        var loadingState = (<div><SpinnerRelative/></div>);


        if(!componentData.InitialDataDict) {
            articleInitialData = null;
        }
        else {
            var url = window.location.pathname;
            articleInitialData = componentData.InitialDataDict[url];

        }
        
        if(!articleInitialData) {
            
            if(componentData.Status == Status.Reset || componentData.Status == Status.IsFetching || componentData.Status == Status.Error) {
                return loadingState;
            }

            //no initial data -> hard refresh
            articleInitialData = mapNewsArticleDataToInitialData(componentData.ArticleDetail);
            if(!articleInitialData) {
                // articleDetail does not have data
                return loadingState; 
            }
                
            
        } 

        var articleDetail = componentData.ArticleDetail;
        var adSlotTop = null;
        var adSlotBottom = null;
        
        if(articleDetail) {
        	let targetTags = {
        		City: this.globalCityName,
        		Tags: articleDetail.Tags
        	}

        	adSlotTop = <AdUnit uniqueKey={articleDetail.Title} tags={targetTags} adSlot={AD_PATH_NEWS_MOBILE_TOP_320_50} adDimension={AD_DIMENSION_320_50} adContainerId={AD_DIV_REVIEWS_TOP_320_50} />;

        	adSlotBottom = <AdUnit uniqueKey={articleDetail.Title} tags={targetTags} adSlot={AD_PATH_NEWS_MOBILE_BOTTOM_320_50} adDimension={AD_DIMENSION_320_50} adContainerId={AD_DIV_REVIEWS_BOTTOM_320_50} />;
        }

        var documentTitle = (articleInitialData.Title == "") ?"BikeWale News" : (articleInitialData.Title + " - BikeWale News");
       
        return (
            <div>

                {adSlotTop}
                <div className="container bg-white box-shadow section-bottom-margin article-details-container">
                    <ArticleDetailTitle title={articleInitialData.Title} authorName={articleInitialData.AuthorName} authorMaskingName={articleInitialData.AuthorMaskingName} displayDate={articleInitialData.DisplayDateTime} />
                    
                    <div className="article-content-padding">
                        {this.renderArticleContent(articleDetail,articleInitialData)}
                    </div>
                </div>
                {this.renderNewBikesList()}
                <div className="margin-bottom15">
                    {adSlotBottom}
                </div>
                {this.renderBreadcrumb(articleInitialData.Title)}
                {this.renderFooter()}
            </div>
               
            
            
        )
    }
}

export default withRouter(ArticleDetail);
