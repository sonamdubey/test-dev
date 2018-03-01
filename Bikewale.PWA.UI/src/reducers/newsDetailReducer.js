import { fromJS , toJS } from 'immutable'
import {newsDetailAction,newBikesListAction,modelObjectAction} from '../actionTypes/actionTypes.js'
import {Status} from '../utils/constants'
import { mapNewsArticleDataToInitialData } from '../components/News/NewsCommon'
import {triggerPageView} from '../utils/analyticsUtils'
import {startTimer} from '../utils/timing'

export function NewsDetailReducer(state,action) {
	try {


		const initialState = fromJS({
			ArticleDetailData : {
					Status : Status.Reset,
					InitialDataDict : {},
					ArticleDetail : null
				},
			NewBikesListData : {
					Status : Status.Reset,
					NewBikesList : null,
			        BikeMakeList : null
				},
			RelatedModelObject : {
					Status : Status.Reset,
					ModelObject : null
			}	
		})

		if(state && window._SERVER_RENDERED_DATA == true) {

		    var articleDetail = state.getIn(['ArticleDetailData','ArticleDetail']);
			var newBikesList = state.getIn(['NewBikesListData','NewBikesList']);
			var newbikeMakeList = state.getIn(['NewBikesListData','BikeMakeList']);
			var modelObject = state.getIn(['RelatedModelObject','ModelObject']);
			

			if(articleDetail && newBikesList) { // modelObject may be null
				var articleInitialData = mapNewsArticleDataToInitialData(articleDetail.toJS());
				var initialDataDict = {};
				initialDataDict[articleInitialData.ArticleUrl] = articleInitialData;
				
				return fromJS({
					ArticleDetailData : {
							Status : Status.Fetched,
							InitialDataDict : initialDataDict,
							ArticleDetail : articleDetail
						},
					NewBikesListData : {
							Status : Status.Fetched,
							NewBikesList : newBikesList,
							BikeMakeList : newbikeMakeList
						},
					RelatedModelObject : {
							Status : Status.Fetched,
							ModelObject : modelObject
					}
				})


			}
			else
				state = null;

		}
		
		if(state == undefined || state == null) {
			return initialState;
		}
		
		switch(action.type) {
			case newsDetailAction.FETCH_NEWSDETAIL : 
				
				startTimer(1,2); // 1 api (set of 3) + 2 ads
				var initialDataDict = state.getIn(['ArticleDetailData','InitialDataDict'])
				return state.setIn(['ArticleDetailData'] , fromJS({
						Status : Status.IsFetching,
						InitialDataDict : initialDataDict ,
						ArticleDetail : null
				}))
				
			case newsDetailAction.FETCH_NEWSDETAIL_WITH_INITIAL_DATA : 
				startTimer(1,2); // 1 api (set of 3) + 2 ads
				var initialDataDict = state.getIn(['ArticleDetailData','InitialDataDict']);
				if(action.payload) {
					document.title = action.payload.Title ? action.payload.Title + " - BikeWale": "BikeWale";
					var initialDataDictModified = initialDataDict.setIn([action.payload.ArticleUrl],action.payload);
					initialDataDict = initialDataDictModified;
				} 
				return state.setIn(['ArticleDetailData'] , fromJS({
						Status : Status.IsFetching,
						InitialDataDict : initialDataDict,
						ArticleDetail : null
				}))
				
			case newsDetailAction.FETCH_NEWSDETAIL_SUCCESS :
				
				var initialDataDict = state.getIn(['ArticleDetailData','InitialDataDict']);
				var docTitle = "";
				if(action.payload) {
					docTitle = action.payload.Title ? action.payload.Title + " - BikeWale": "BikeWale";
					var articleInitialData =  initialDataDict.getIn([action.payload.ArticleUrl]);	
					if(!articleInitialData) {
						articleInitialData = mapNewsArticleDataToInitialData(action.payload);
						initialDataDict = initialDataDict.setIn([action.payload.ArticleUrl],articleInitialData); 
					}	
				}
				if(document.title !== docTitle) 
					document.title = docTitle;
				triggerPageView(window.location.pathname,document.title);
				return state.setIn(['ArticleDetailData'] , fromJS({
						Status : Status.Fetched,
						InitialDataDict : initialDataDict,
						ArticleDetail : action.payload
				}))
				

			case newsDetailAction.FETCH_NEWSDETAIL_FAILURE : 
				var initialDataDict = state.getIn(['ArticleDetailData','InitialDataDict'])
				return state.setIn(['ArticleDetailData'] , fromJS({
						Status : Status.Error,
						InitialDataDict : initialDataDict,
						ArticleDetail : null
				}))
				
			case newsDetailAction.NEWSDETAIL_RESET : 
				var initialDataDict = state.getIn(['ArticleDetailData','InitialDataDict'])
				return state.setIn(['ArticleDetailData'] , fromJS({
						Status : Status.Reset,
						InitialDataDict : initialDataDict,
						ArticleDetail : null
				}))
				

			case newBikesListAction.FETCH_NEW_BIKES_LIST_FOR_NEWS_DETAIL:
				return state.setIn(['NewBikesListData'] , fromJS({
						Status : Status.IsFetching,
						NewBikesList : null,
				        BikeMakeList : null
				}))

			case newBikesListAction.FETCH_NEW_BIKES_LIST_SUCCESS_FOR_NEWS_DETAIL:
				return state.setIn(['NewBikesListData'] , fromJS({
						Status : Status.Fetched,
						NewBikesList : action.payload.NewBikesList,
						BikeMakeList : action.payload.BikeMakeList
				}))
				
			case newBikesListAction.FETCH_NEW_BIKES_LIST_FAILURE_FOR_NEWS_DETAIL:
				return state.setIn(['NewBikesListData'] , fromJS({
						Status : Status.Error,
						NewBikesList : null
				}))

			case newBikesListAction.NEW_BIKES_LIST_RESET_FOR_NEWS_DETAIL: 
				return state.setIn(['NewBikesListData'] , fromJS({
						Status : Status.Reset,
						NewBikesList : null,
						BikeMakeList : null
				}))
				

			case modelObjectAction.FETCH_MODEL_OBJECT_FOR_NEWS_DETAIL :
				return state.setIn(['RelatedModelObject'] , fromJS({
						Status : Status.IsFetching,
						ModelObject : null
				}))
					
			case modelObjectAction.FETCH_MODEL_OBJECT_SUCCESS_FOR_NEWS_DETAIL :
				return state.setIn(['RelatedModelObject'] , fromJS({
						Status : Status.Fetched,
						ModelObject : action.payload
				}))
					
			case modelObjectAction.FETCH_MODEL_OBJECT_FAILURE_FOR_NEWS_DETAIL :
				return state.setIn(['RelatedModelObject'] , fromJS({
						Status : Status.Error,
						ModelObject : null
				}))
				
			case modelObjectAction.MODEL_OBJECT_RESET_FOR_NEWS_DETAIL :
				return state.setIn(['RelatedModelObject'] , fromJS({
						Status : Status.Reset,
						ModelObject : null
				}))
				
		}

		return state;
	}
	catch(e) {
		return state;
	}
}
