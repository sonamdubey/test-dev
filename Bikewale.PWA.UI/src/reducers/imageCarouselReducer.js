import { fromJS , toJS } from 'immutable'
import { bikeImageCarouselAction } from '../actionTypes/actionTypes.js'
import { mapPopularBikesToInitialData } from '../components/Widgets/WidgetsCommon.js'
import { Status } from '../utils/constants'
import { startTimer } from '../utils/timing'

export function BikeImagesCarouselReducer(state,action) {

    try {
        const initialState = fromJS({
            PopularBikeImagesListData : {
                Status : Status.Reset,
                BikeImagesList : null
            }
        });

        if(state && window._SERVER_RENDERED_DATA == true) {
            var bikesList = state.getIn(['PopularBikeImagesListData','BikeImagesList']);
            if(bikesList) {
                var bikesInitialData = bikesList.toJS();
                return fromJS({
                    PopularBikeImagesListData : {
                        Status : Status.Fetched,
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
            case bikeImageCarouselAction.FETCH_POPULAR_BIKELIST:
                return state.setIn(['PopularBikeImagesListData'] ,  fromJS({
                    Status : Status.IsFetching,
                    BikeImagesList : null
                }))
				
            case bikeImageCarouselAction.FETCH_POPULAR_BIKELIST_SUCCESS:
                if(action.payload) {
                    return state.setIn(['PopularBikeImagesListData'] , fromJS({
                        Status : Status.Fetched,
                        BikeImagesList : action.payload
                    }));
                }
				

				
            case bikeImageCarouselAction.FETCH_POPULAR_BIKELIST_FAILURE:
                return state.setIn(['PopularBikeImagesListData'] , fromJS({
                    Status : Status.Error,
                    BikeImagesList : null
                }))
				
            case bikeImageCarouselAction.POPULAR_BIKELIST_RESET:
                return state.setIn(['PopularBikeImagesListData'] , fromJS({
                    Status : Status.Reset,
                    BikeImagesList : null
                }))
        }
        return state;	
    }
    catch(e) {
        return state;
    }
	
}
