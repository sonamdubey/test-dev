import { fromJS , toJS } from 'immutable'
import { imageCarouselAction } from '../actionTypes/actionTypes.js'
import { mapPopularBikesToInitialData } from '../components/Widgets/WidgetsCommon.js'
import { Status } from '../utils/constants'
import { startTimer } from '../utils/timing'

export function BikeImagesCarousel(state,action) {
    try {
        const initialState = fromJS({
            PopularBikeImagesListData : {
                Status : Status.Reset,
                InitialDataDict : {},
                BikeImagesList : null
            }
        })


        if(state && window._SERVER_RENDERED_DATA == true) {
            var bikesList = state.getIn(['PopularBikeImagesListData','BikeImagesList']);
            if(bikesList) {
                var bikesInitialData = mapPopularBikesToInitialData(bikesList.toJS());
                var initialDataDict = {};
                initialDataDict['PopularBikeImagesListData'] = { Status : Status.Fetched, BikeImagesList: bikesInitialData };
                return fromJS({
                    PopularBikeImagesListData : {
                        Status : Status.Fetched,
                        InitialDataDict : initialDataDict,
                        BikeImagesList : bikesList
                    }
                });
            }
            else
                state = null;

        }

		
        if(state == undefined || state == null) {
            return initialState;
        }
		
		
        switch(action.type) {
            case imageCarouselAction.FETCH_BIKELIST:
                var initialData = state.getIn(['PopularBikeImagesListData','InitialDataDict']);
                return state.setIn(['PopularBikeImagesListData'] ,  fromJS({
                    Status : Status.IsFetching,
                    InitialDataDict : initialData,
                    BikeImagesList : null
                }))
				
            case imageCarouselAction.FETCH_BIKELIST_SUCCESS:
                var bikesInitialData = null;
                var initialData = state.getIn(['PopularBikeImagesListData', 'InitialDataDict']);
                if(action.payload) {
                    var bikesInitialData = mapPopularBikesToInitialData(action.payload);
                    initialData = initialData.setIn(['PopularBikeImagesListData'], { Status : Status.Fetched, BikeImagesList : bikesInitialData });
                }
                return state.setIn(['PopularBikeImagesListData'] , fromJS({
                    Status : Status.Fetched,
                    InitialDataDict : initialData,
                    BikeImagesList : bikesInitialData
                }));
				

				
            case imageCarouselAction.FETCH_BIKELIST_FAILURE:
                var initialData = state.getIn(['PopularBikeImagesListData','InitialDataDict']);
                return state.setIn(['PopularBikeImagesListData'] , fromJS({
                    Status : Status.Error,
                    InitialDataDict : initialData,
                    BikeImagesList : null
                }))
				
            case imageCarouselAction.BIKELIST_RESET:
                var initialData = state.getIn(['PopularBikeImagesListData', 'InitialDataDict']);
                return state.setIn(['PopularBikeImagesListData'] , fromJS({
                    Status : Status.Reset,
                    InitialDataDict : initialData,
                    BikeImagesList : null
                }))
        }
        return state;	
    }
    catch(e) {
        return state;
    }
	
}
