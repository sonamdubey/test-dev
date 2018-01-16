import {videosByCategoryAction} from '../actionTypes/actionTypes'

module.exports = {
	fetchVideosByCategory : function(categoryId,SectionTitle) {
		return function(dispatch) {
			//TODO category id can be -1 if not known
			var xhr = new XMLHttpRequest();
			xhr.onreadystatechange = function() {
				if(xhr.readyState == 4) {
					if(xhr.status == 200) {
						dispatch({type : videosByCategoryAction.FETCH_SUCCESS,payload:JSON.parse(xhr.responseText)});
					}
					else 
						dispatch({type:videosByCategoryAction.FETCH_ERROR})
				}
			}
			if(!SectionTitle) {
				dispatch({type:videosByCategoryAction.FETCH});	
			}
			else {
				dispatch({type:videosByCategoryAction.FETCH_WITH_INITIAL_DATA,payload:{SectionTitle:SectionTitle}});
			}
			
			xhr.open('GET','/api/pwa/catvideos/catId/'+categoryId+'/count/9/')
			xhr.send();

			
		}
	}
}