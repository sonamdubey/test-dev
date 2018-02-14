import {combineReducers} from 'redux-immutable'
import {BikeImagesCarousel} from './imageCarouselReducer'
var Widgets = combineReducers({
    BikeImagesCarousel : BikeImagesCarousel
});
module.exports = Widgets;