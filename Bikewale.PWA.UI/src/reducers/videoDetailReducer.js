import {Status} from '../utils/constants'
import {fromJS, toJS} from 'immutable'
import {videosDetailAction} from '../actionTypes/actionTypes'
import {formVideoUrl , mapVideoDataToInitialData} from '../components/Videos/VideosCommonFunc'
import {updateData} from '../utils/commonUtils'
import {triggerPageView} from '../utils/analyticsUtils'
import {startTimer} from '../utils/timing'
var initialState = fromJS({
	VideoInfoStatus : Status.Reset,
	VideoInfo : null,
	InitialDataDict : {},
	ModelInfoStatus : Status.Reset ,
	ModelInfo : null,
	RelatedInfoStatus : Status.Reset,
	RelatedInfoApi : null,
	RelatedInfo : null
})






export function VideoDetail(state,action) {
	try{
		if(!state)
			return initialState;
		if(state && window._SERVER_RENDERED_DATA == true) {
			var VideoInfo = state.getIn(['VideoInfo','VideoInfo']);
			var RelatedInfoApi = state.getIn(['VideoInfo','RelatedInfoApi']);
			var ModelInfo = state.getIn(['ModelInfo']);
			var RelatedInfo = state.getIn(['RelatedInfo']);

			if(VideoInfo && ModelInfo && RelatedInfo) {
				var videoInitialData = mapVideoDataToInitialData(VideoInfo.toJS());
				var initialDataDict = {};
				initialDataDict[VideoInfo.BasicId] = videoInitialData;
				return fromJS({
					VideoInfoStatus : Status.Fetched,
					VideoInfo : VideoInfo,
					InitialDataDict : initialDataDict,
					ModelInfoStatus : Status.Fetched ,
					ModelInfo : ModelInfo,
					RelatedInfoStatus : Status.Fetched,
					RelatedInfoApi : RelatedInfoApi,
					RelatedInfo : RelatedInfo
				})
			}
			else {
				return initialState;
			}
		}
		
		switch(action.type) {
			case videosDetailAction.FETCH_VIDEODETAIL :
				startTimer(1,0);
				return updateData(state,{'VideoInfoStatus':Status.IsFetching , 'VideoInfo' : null , 'RelatedInfoApi': null ,
											'ModelInfoStatus' : Status.Reset  , 'ModelInfo' : null , 
										     'RelatedInfoStatus' : Status.Reset , 'RelatedInfo' : null});
			case videosDetailAction.FETCH_VIDEODETAIL_WITH_INITIAL_DATA :
				startTimer(1,0);
				var initialDataDict = state.getIn(['InitialDataDict']);
				if(action.payload) {
					 initialDataDict = initialDataDict.setIn([action.payload.BasicId],action.payload);
				}
				return updateData(state,{'VideoInfoStatus':Status.IsFetching , 'InitialDataDict':initialDataDict , 'VideoInfo': null , 'RelatedInfoApi': null ,
											'ModelInfoStatus' : Status.Reset  , 'ModelInfo' : null , 
										     'RelatedInfoStatus' : Status.Reset , 'RelatedInfo' : null});
				
			case videosDetailAction.FETCH_VIDEODETAIL_SUCCESS :
				var initialDataDict = state.getIn(['InitialDataDict']);
				var relatedInfoApi = null;
				var videoInfo = null;
				if(action.payload) {
					videoInfo = (!action.payload.VideoInfo)?null:action.payload.VideoInfo;
					var videoInitialData = initialDataDict.getIn([videoInfo.BasicId]);
					relatedInfoApi = (!action.payload.RelatedInfoApi)?null:action.payload.RelatedInfoApi;
					videoInfo = (!action.payload.VideoInfo)?null:action.payload.VideoInfo;
					 
					if(!videoInitialData) {
						videoInitialData = mapVideoDataToInitialData(action.payload.VideoInfo);
						initialDataDict = initialDataDict.setIn([action.payload.VideoInfo.BasicId]);
					}
				}

				document.title = !videoInfo ? "Bike Videos, Expert Video Reviews with Road Test & Bike Comparison - BikeWale" : videoInfo.VideoTitle + " - BikeWale";
				triggerPageView(window.location.pathname,document.title);
				
				return updateData(state,{'VideoInfoStatus':Status.Fetched , 'InitialDataDict':initialDataDict , 'VideoInfo':videoInfo , 'RelatedInfoApi': relatedInfoApi});
			
			case videosDetailAction.FETCH_VIDEODETAIL_ERROR :
				return updateData(state,{'VideoInfoStatus':Status.Error , 'VideoInfo': null , 'RelatedInfoApi': null});
			
			case videosDetailAction.FETCH_VIDEODETAIL_RESET :
				return updateData(state,{'VideoInfoStatus':Status.Reset , 'VideoInfo': null , 'RelatedInfoApi': null,
										'ModelInfoStatus':Status.Reset , 'ModelInfo': null ,
										'RelatedInfoStatus':Status.Reset , 'RelatedInfo': null });
			case videosDetailAction.FETCH_MODELINFO :
				return updateData(state,{'ModelInfoStatus':Status.IsFetching , 'ModelInfo': null});
			case videosDetailAction.FETCH_MODELINFO_SUCCESS :
				return updateData(state,{'ModelInfoStatus':Status.Fetched , 'ModelInfo': action.payload});
			case videosDetailAction.FETCH_MODELINFO_ERROR :
				return updateData(state,{'ModelInfoStatus':Status.Error , 'ModelInfo': null});
			case videosDetailAction.FETCH_MODELINFO_RESET :
				return updateData(state,{'ModelInfoStatus':Status.Reset , 'ModelInfo': null});
			case videosDetailAction.FETCH_RELATEDINFO :
				return updateData(state,{'RelatedInfoStatus':Status.IsFetching , 'RelatedInfo': null});
			case videosDetailAction.FETCH_RELATEDINFO_SUCCESS :
				return updateData(state,{'RelatedInfoStatus':Status.Fetched , 'RelatedInfo': action.payload});
			case videosDetailAction.FETCH_RELATEDINFO_ERROR :
				return updateData(state,{'RelatedInfoStatus':Status.Error , 'RelatedInfo': null});
			case videosDetailAction.FETCH_RELATEDINFO_RESET :
				return updateData(state,{'RelatedInfoStatus':Status.Reset , 'RelatedInfo': null});
			default : return state;
		}

	}
	catch(err){
		console.log(err);
		return state;
	}
}