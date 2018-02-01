import {videosLandingAction} from '../actionTypes/actionTypes'

module.exports ={
	fetchTopVideosData : function() {
		return function(dispatch) {
			var xhr = new XMLHttpRequest() ;
			xhr.onreadystatechange = function() {
				if(xhr.readyState == 4) {
					if(xhr.status == 200)
						dispatch({type:videosLandingAction.FETCH_TOPVIDEOSLANDING_SUCCESS,payload:JSON.parse(xhr.responseText)});
					else 
						dispatch({type:videosLandingAction.FETCH_TOPVIDEOSLANDING_FAILURE});
				}

			}
			
			xhr.open('GET','/api/pwa/topvideos/');
			xhr.send();
			dispatch({type:videosLandingAction.FETCH_TOPVIDEOSLANDING});
		}
	},
	fetchOtherVideosData : function() {
		return function(dispatch) {
			var xhr = new XMLHttpRequest();
			xhr.onreadystatechange = function() {
				if(xhr.readyState == 4) {
					if(xhr.status == 200) 
						dispatch({type:videosLandingAction.FETCH_OTHERVIDEOSLANDING_SUCCESS, payload:JSON.parse(xhr.responseText)});
					else 
						dispatch({type:videosLandingAction.FETCH_OTHERVIDEOSLANDING_FAILURE});
				}
			}
			
			xhr.open('GET','/api/pwa/othervideos/count/6/')
			xhr.send();
			dispatch({type:videosLandingAction.FETCH_OTHERVIDEOSLANDING});
		}
	}
}
