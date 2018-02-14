import { bikeImageCarouselAction } from  '../actionTypes/actionTypes'
import {isInt } from '../utils/commonUtils'

var fetchPopularBikesDataForImageCarousel = function(topCount,requiredImageCount) {
    return function(dispatch) {
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function() {
            if(xhr.readyState == 4) {
                if(xhr.status == 200) 
                    dispatch({ type : bikeImageCarouselAction.FETCH_POPULAR_BIKELIST_SUCCESS, payload : JSON.parse(xhr.responseText) });
                else 
                    dispatch({ type : bikeImageCarouselAction.FETCH_POPULAR_BIKELIST_FAILURE });
            }
        }
			
        xhr.open('GET', '/api/pwa/images/models/?modelCount=' + topCount + '&imageCount=' + requiredImageCount)
        xhr.send();
        dispatch({ type : bikeImageCarouselAction.FETCH_POPULAR_BIKELIST });
    }
}

module.exports = {
    fetchPopularBikesDataForImageCarousel : fetchPopularBikesDataForImageCarousel
};