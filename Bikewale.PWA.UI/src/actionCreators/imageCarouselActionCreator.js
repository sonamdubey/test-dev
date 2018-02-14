import { imageCarouselAction } from  '../actionTypes/actionTypes'
import {isInt } from '../utils/commonUtils'

var fetchPopularBikesDataForImageCarousel = function(topCount,requiredImageCount) {
    return function(dispatch) {
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function() {
            if(xhr.readyState == 4) {
                if(xhr.status == 200) 
                    dispatch({ type : imageCarouselAction.FETCH_BIKELIST_SUCCESS, payload : JSON.parse(xhr.responseText) });
                else 
                    dispatch({ type : imageCarouselAction.FETCH_BIKELIST_FAILURE });
            }
        }
			
        xhr.open('GET', '/api/pwa/images/models/?modelCount=' + topCount + '&imageCount=' + requiredImageCount)
        xhr.send();
        dispatch({ type : imageCarouselAction.FETCH_BIKELIST });
    }
}

module.exports = {
    fetchPopularBikesDataForImageCarousel : fetchPopularBikesDataForImageCarousel
};