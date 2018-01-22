import {Status} from '../utils/constants'
import {fromJS,toJS} from 'immutable'
import {videosByCategoryAction} from '../actionTypes/actionTypes'
import {updateData} from '../utils/commonUtils'
import {triggerPageView} from '../utils/analyticsUtils'
var initialState = fromJS({
	Status : Status.Reset,
	SectionTitle : null,
	Videos : null
})

export function VideosByCategory(state,action) {
	try {
		if(!state) {
			return initialState;
		}
		if(state && window._SERVER_RENDERED_DATA == true) {
			var SectionTitle = state.getIn(['SectionTitle']);
			var Videos = state.getIn(['Videos']);
			if(Videos) {
				return fromJS({
					Status : Status.Fetched,
					SectionTitle : SectionTitle,
					Videos : Videos
				})
			}
			else
				return initialState;
		}
		switch(action.type) {
			case videosByCategoryAction.FETCH : 
				return updateData(state,{'Status':Status.IsFetching,'SectionTitle':null,'Videos':null});
			case videosByCategoryAction.FETCH_WITH_INITIAL_DATA :
				var sectionTitle = action.payload && action.payload.SectionTitle ? action.payload.SectionTitle : "";
				return updateData(state,{'Status':Status.IsFetching , 'SectionTitle':sectionTitle , 'Videos' : null});
			case videosByCategoryAction.FETCH_SUCCESS : 
				var sectionTitle = action.payload && action.payload.SectionTitle ? action.payload.SectionTitle : "";
				var videos = action.payload && action.payload.Videos ? action.payload.Videos : [];
				
				document.title = sectionTitle + " - BikeWale";
				triggerPageView(window.location.pathname,document.title);
				
				return updateData(state,{'Status':Status.Fetched,'SectionTitle':sectionTitle , 'Videos':videos});
			case videosByCategoryAction.FETCH_ERROR : 
				return updateData(state,{'Status':Status.Error,'Videos':null});
			case videosByCategoryAction.FETCH_RESET :
				return updateData(state,{'Status':Status.Reset,'SectionTitle':null,'Videos':null});
			default :
				return state;
			

		}
	}
	catch(err) {
		return state;
	}
}