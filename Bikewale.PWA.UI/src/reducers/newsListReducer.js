import {fromJS} from 'immutable'
import {newsListAction,newBikesListAction} from '../actionTypes/actionTypes'
import {refreshGPTAds} from '../utils/googleAdUtils'
import {extractPageNoFromURL} from '../components/News/NewsCommon'
import { NewsArticlesPerPage , Status } from '../utils/constants'
import {triggerPageView} from '../utils/analyticsUtils'
import {startTimer} from '../utils/timing'

export function NewsArticleListReducer(state,action) {
	try {
		const initialState = fromJS({
		ArticleListData : {
							Status : Status.Reset,
					        PageNo : -1,
					        ArticleList : null

						} ,
		NewBikesListData : {
							Status : Status.Reset,
							NewBikesList : null
						}
		})


		if(state && window._SERVER_RENDERED_DATA == true) {
			
			var articleList = state.getIn(['ArticleListData','ArticleList']);
			var newBikesList = state.getIn(['NewBikesListData','NewBikesList'])
			
			if(articleList && newBikesList) {
				var pageNo = Math.floor(articleList.getIn(['StartIndex']) / NewsArticlesPerPage) +  1; //  corner case - last page with less than NewsArticlesPerPage articles
				return fromJS({
					ArticleListData : {
									Status : Status.Fetched,
							        PageNo : pageNo,
							        ArticleList : articleList
								} ,
					NewBikesListData : {
									Status : Status.Fetched, 
									NewBikesList : newBikesList
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
			case newsListAction.FETCH_NEWSLIST:
				// refreshGPTAds(); // trigger refresh when 1st api is called as at this point the required ads will be mounted
				document.title = 'Bike News - Latest Indian Bike News &amp; Views | BikeWale';
				startTimer(1,2); // 1 api (set of 2) + 2 ads
				return state.setIn(['ArticleListData'] ,  fromJS({
							Status : Status.IsFetching,
							ArticleList : null, 
							PageNo : -1
				}))
				
			case newsListAction.FETCH_NEWSLIST_SUCCESS:
				triggerPageView(window.location.pathname,document.title);
				var pageNo = extractPageNoFromURL(window.location.href);
				return state.setIn(['ArticleListData'] , fromJS({
							Status : Status.Fetched,
							ArticleList : action.payload , 
							PageNo : pageNo
				}))
				

				
			case newsListAction.FETCH_NEWSLIST_FAILURE:
				return state.setIn(['ArticleListData'] , fromJS({
							Status : Status.Error,
							ArticleList : null,
							PageNo :-1
				}))
				
			case newsListAction.NEWSLIST_RESET:
				return state.setIn(['ArticleListData'] , fromJS({
							Status : Status.Reset,
							ArticleList : null,
							PageNo :-1
				}))

				

			case newBikesListAction.FETCH_NEW_BIKES_LIST_FOR_NEWS_LIST:

				return state.setIn(['NewBikesListData'] , fromJS({
							Status : Status.IsFetching,
							NewBikesList : null
				}))
				
			
			case newBikesListAction.FETCH_NEW_BIKES_LIST_SUCCESS_FOR_NEWS_LIST:
				return state.setIn(['NewBikesListData'] , fromJS({
							Status : Status.Fetched,
							NewBikesList : action.payload
				}))
			

			case newBikesListAction.FETCH_NEW_BIKES_LIST_FAILURE_FOR_NEWS_LIST:
				return state.setIn(['NewBikesListData'] , fromJS({
							Status : Status.Error,
							NewBikesList : null
				}))
				
			case newBikesListAction.NEW_BIKES_LIST_RESET_FOR_NEWS_LIST: 
				return state.setIn(['NewBikesListData'] , fromJS({
							Status : Status.Reset,
							NewBikesList : null
				}))
				
			
		}
		return state;	
	}
	catch(e) {
		return state;
	}
	
}
