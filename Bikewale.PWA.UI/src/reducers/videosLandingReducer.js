import {Status} from '../utils/constants'
import {fromJS, toJS} from 'immutable'
import {videosLandingAction} from '../actionTypes/actionTypes'
import {updateData} from '../utils/commonUtils'
import {triggerPageView} from '../utils/analyticsUtils'
var initialState = fromJS({
	TopVideosStatus : Status.Reset,
	TopVideos : null,
	OtherVideosStatus : Status.Reset,
	OtherVideos : null
});

export function VideosLanding(state,action) {
    try {
		if(!state)
			return initialState;
		if(state && window._SERVER_RENDERED_DATA == true) {
			var TopVideos = state.getIn(['TopVideos']);
			var OtherVideos = state.getIn(['OtherVideos']);
			if(TopVideos && OtherVideos) {
				return fromJS({
					TopVideosStatus : Status.Fetched,
					TopVideos : TopVideos,
					OtherVideosStatus : Status.Fetched,
					OtherVideos : OtherVideos
				});	
			}
			else {
				return initialState;
			}
			
		}
		switch(action.type) {
			case videosLandingAction.FETCH_TOPVIDEOSLANDING:
				document.title = "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale";
				triggerPageView(window.location.pathname,document.title);
				return updateData(state,{'TopVideosStatus':Status.IsFetching,'TopVideos':null});
			case videosLandingAction.FETCH_TOPVIDEOSLANDING_SUCCESS: 
				return updateData(state,{'TopVideosStatus':Status.Fetched,'TopVideos':action.payload});
			
			case videosLandingAction.FETCH_TOPVIDEOSLANDING_FAILURE:
				return updateData(state,{'TopVideosStatus':Status.Error,'TopVideos':null});
			
			case videosLandingAction.FETCH_TOPVIDEOSLANDING_RESET:
				return updateData(state,{'TopVideosStatus':Status.Reset,'TopVideos':null});
			
			case videosLandingAction.FETCH_OTHERVIDEOSLANDING:
				return updateData(state,{'OtherVideosStatus':Status.IsFetching,'OtherVideos':null});
			
			case videosLandingAction.FETCH_OTHERVIDEOSLANDING_SUCCESS:
				return updateData(state,{'OtherVideosStatus':Status.Fetched,'OtherVideos':action.payload});
			case videosLandingAction.FETCH_OTHERVIDEOSLANDING_FAILURE:
				return updateData(state,{'OtherVideosStatus':Status.IsFetching,'OtherVideos':null});
			case videosLandingAction.FETCH_OTHERVIDEOSLANDING_RESET:
				return updateData(state,{'OtherVideosStatus' : Status.Reset , 'OtherVideos':null});
			default : return state;	
		}

	}
	catch(err){
		console.log(err);
		return state;
	}
}
