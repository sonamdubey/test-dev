import {videosDetailAction} from '../actionTypes/actionTypes'
import {isInt} from '../utils/commonUtils'
import {getGlobalCity} from '../utils/popUpUtils'

module.exports = {
	fetchVideoDetail : function(videoInitialData) {
		return function(dispatch) {
			
			var url = null;
			var basicId = null;
			if(videoInitialData) {
				if(isInt(videoInitialData)) { // passed basicid
					dispatch({type:videosDetailAction.FETCH_VIDEODETAIL});
					basicId = videoInitialData; // add videoinitialdetail directly to url
				}
				else {
					dispatch({type:videosDetailAction.FETCH_VIDEODETAIL_WITH_INITIAL_DATA,payload : videoInitialData});
					basicId = videoInitialData.BasicId; // add videoInitialData.BasicId to url
				}
			}

			var xhr = new XMLHttpRequest();
			xhr.onreadystatechange = function() {
				if(xhr.readyState == 4) {
					if(xhr.status == 200) {
						dispatch({type : videosDetailAction.FETCH_VIDEODETAIL_SUCCESS,payload:JSON.parse(xhr.responseText)});
					}
					else 
						dispatch({type:videosDetailAction.FETCH_VIDEODETAIL_ERROR})
				}
			}
			xhr.open('GET','/api/pwa/videodet/'+basicId+'/');
		 
			xhr.send();
			
		}
	},
	resetVideoDetail : function() {
		return function(dispatch) {
			dispatch({type : videosDetailAction.FETCH_VIDEODETAIL_RESET});
		}
	},
	fetchModelSlug : function(basicId) {
		return function(dispatch) {
			var globalCity = getGlobalCity();
			var globalCityName = ( globalCity && globalCity.name ) ? globalCity.name : '';
			var xhr = new XMLHttpRequest();
			xhr.onreadystatechange = function() {
				if(xhr.readyState == 4) {
					if(xhr.status == 200) {
						dispatch({type : videosDetailAction.FETCH_MODELINFO_SUCCESS,payload:JSON.parse(xhr.responseText)});
					}
					else 
						dispatch({type:videosDetailAction.FETCH_MODELINFO_ERROR})
				}
			}
			xhr.open('GET','/api/pwa/cms/bikeinfo/id/'+basicId+'/page/?city='+globalCityName);
			xhr.send();
			dispatch({type:videosDetailAction.FETCH_MODELINFO});
		}
	},
	fetchRelatedInfo : function(apiList) {
		return function(dispatch) {
 		
	   		if(!apiList || apiList.length==0) return;
	   		var globalCity = getGlobalCity();
	   		var globalCityName = ( globalCity && globalCity.name ) ? globalCity.name : '';
	   		var apiResult = {};
	   		var returned=0;
	   		apiList.map(function(api,index) {
	   			if(!api.Url) return;
	   			var xhr = new XMLHttpRequest();
	   			xhr.onreadystatechange = function() {
	   				if(xhr.readyState == 4) {
	   					if(xhr.status == 200) {
	   						returned++;
	   						apiResult[index] = JSON.parse(xhr.responseText);
	   						if(returned == apiList.length) {
	   							dispatch({type : videosDetailAction.FETCH_RELATEDINFO_SUCCESS,payload:apiResult})
	   						}
	   					}
	   				}
	   			}
	   			xhr.open('GET',window.location.origin+'/'+api.Url+'/?city='+globalCityName); 
				xhr.send();
				
	   		})
			dispatch({type:videosDetailAction.FETCH_RELATEDINFO});
		}
	}
	
}