import {newsDetailAction,newBikesListAction,modelObjectAction} from  '../actionTypes/actionTypes'

import {isInt } from '../utils/commonUtils'
import { refreshGPTAds } from '../utils/googleAdUtils'
import { extractPageCategoryFromURL } from '../components/News/NewsCommon'
import {getGlobalCity} from '../utils/popUpUtils'

var fetchNewsArticleDetail = function(articleInitialData) {
	
	return function(dispatch) {	
        var page = extractPageCategoryFromURL();
		var url = null;
		if(articleInitialData) {
			if(isInt(articleInitialData)) {
                dispatch({ type: newsDetailAction.FETCH_NEWSDETAIL });
                if (page == "news") {
                    url = '/api/pwa/cms/id/' + articleInitialData + '/page/';
                }
                else {
                    url = '/api/pwa/cms/id/' + articleInitialData + '/pages/';
                }
			}
			else {
				dispatch({type:newsDetailAction.FETCH_NEWSDETAIL_WITH_INITIAL_DATA,payload:articleInitialData});
				url = '/'+articleInitialData.ArticleApi;
			}
			

		}
		var xhr = new XMLHttpRequest();
		xhr.onreadystatechange = function() {
			if(xhr.readyState == 4) {
				if(xhr.status == 200)
					dispatch({type:newsDetailAction.FETCH_NEWSDETAIL_SUCCESS,payload:JSON.parse(xhr.responseText)});
				else 
					dispatch({type:newsDetailAction.FETCH_NEWSDETAIL_FAILURE})
			}
		}
		
		xhr.open('GET',url);
		xhr.send();
	}


}

var fetchNewBikesListDataForNewsDetail = function(basicId) {
	
	return function(dispatch) {
	    var url;
	    var globalCity = getGlobalCity();
	    var globalCityName = ( globalCity && globalCity.name.length>0 ) ? globalCity.name : '';
		if(parseInt(basicId) > 0) {
		    url = '/api/pwa/cms/bikelists/id/'+basicId+'/page/?city='+globalCityName;
		}
		else {
		    url = '/api/pwa/cms/bikelists/id/0/page/?city='+globalCityName;
		}
		var xhr = new XMLHttpRequest();
		xhr.onreadystatechange = function() {
			if(xhr.readyState == 4) {
				if(xhr.status == 200)	
					dispatch({type:newBikesListAction.FETCH_NEW_BIKES_LIST_SUCCESS_FOR_NEWS_DETAIL,payload:JSON.parse(xhr.responseText)})
				else 
					dispatch({type:newBikesListAction.FETCH_NEW_BIKES_LIST_FAILURE_FOR_NEWS_DETAIL})
			}
	
		}
		xhr.open('GET',url)
		xhr.send();
		dispatch({type:newBikesListAction.FETCH_NEW_BIKES_LIST_FOR_NEWS_DETAIL});

	}
}

var fetchRelatedModelObjectForNewsDetail = function(basicId) {
	return function(dispatch) {
		
		var url = '/api/v1/pwa/cms/bikeinfo/id/'+basicId+'/page/';
		var xhr = new XMLHttpRequest();
		xhr.onreadystatechange = function() {
			if(xhr.readyState == 4) {
				if(xhr.status == 200)
 					dispatch({type:modelObjectAction.FETCH_MODEL_OBJECT_SUCCESS_FOR_NEWS_DETAIL,payload:JSON.parse(xhr.responseText)})
 				else 
 					dispatch({type:modelObjectAction.FETCH_MODEL_OBJECT_FAILURE_FOR_NEWS_DETAIL})
			}
			
				
		}
		xhr.open('GET',url)
		xhr.send();
		dispatch({type:modelObjectAction.FETCH_MODEL_OBJECT_FOR_NEWS_DETAIL})
	}
}

module.exports = {
	fetchNewsArticleDetail : fetchNewsArticleDetail,
	fetchNewBikesListDataForNewsDetail : fetchNewBikesListDataForNewsDetail,
	fetchRelatedModelObjectForNewsDetail : fetchRelatedModelObjectForNewsDetail
};