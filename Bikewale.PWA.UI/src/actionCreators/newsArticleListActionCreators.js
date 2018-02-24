import {newsListAction,newBikesListAction} from '../actionTypes/actionTypes'

import { CMSUserReviewSlugData , CMSUserReviewSlugPosition , isCMSUserReviewSlugClosed } from '../utils/commonUtils.js'
import {extractPageNoFromURL, extractPageCategoryFromURL} from '../components/News/NewsCommon'
import {refreshGPTAds} from '../utils/googleAdUtils'
import {NewsArticlesPerPage} from '../utils/constants'


var fetchNewsArticleList = function(pageNo) {
	return function(dispatch) {
		var page = extractPageCategoryFromURL();
		if(pageNo == -1) {
			pageNo = extractPageNoFromURL(window.location.href);
		}
		var method = 'GET';
		var url;
		if(page == "news")
			url = 'api/pwa/cms/news/posts/'+NewsArticlesPerPage+'/pn/'+pageNo+'/'; // TODO remove hardcoded api
		else
			url = '/api/pwa/cms/expertreview/posts/'+NewsArticlesPerPage+'/pn/'+pageNo+'/'; // TODO remove hardcoded api
		
		var xhr = new XMLHttpRequest();
		xhr.onreadystatechange = function() {
			if(xhr.readyState == 4) {
				if(xhr.status == 200) {
					
					dispatch({type:newsListAction.FETCH_NEWSLIST_SUCCESS,payload:JSON.parse(xhr.responseText)});
				}
				else 
					dispatch({type:newsListAction.FETCH_NEWSLIST_FAILURE}) 
			}
			
		}

		
		xhr.open(method,url);
		xhr.send();
		dispatch({type:newsListAction.FETCH_NEWSLIST});

	}


}

var fetchNewBikesListDataForNewsList = function(basicId) {
	
	return function(dispatch) {
		var url ; 
		if(parseInt(basicId) > 0) {
			url = '/api/pwa/cms/bikelists/id/'+basicId+'/page/';
		}
		else {
			url = '/api/pwa/cms/bikelists/id/0/page/';
		}
		
		var xhr = new XMLHttpRequest();
		xhr.onreadystatechange = function() {
			if(xhr.readyState == 4 && xhr.status == 200) {
				if(xhr.status == 200)
					dispatch({type:newBikesListAction.FETCH_NEW_BIKES_LIST_SUCCESS_FOR_NEWS_LIST,payload:JSON.parse(xhr.responseText)})
				else
					dispatch({type:newBikesListAction.FETCH_NEW_BIKES_LIST_FAILURE_FOR_NEWS_LIST})
			}

		}
		xhr.open('GET',url)
		xhr.send();
		dispatch({type:newBikesListAction.FETCH_NEW_BIKES_LIST_FOR_NEWS_LIST});

	}
}

var resetArticleListData = function() {
	return function(dispatch) {
		dispatch({type:newsListAction.NEWSLIST_RESET});
		dispatch({type:newBikesListAction.NEW_BIKES_LIST_RESET_FOR_NEWS_LIST});
	}
}

module.exports = {
	fetchNewsArticleList : fetchNewsArticleList,
	fetchNewBikesListDataForNewsList : fetchNewBikesListDataForNewsList,
	resetArticleListData : resetArticleListData
}
