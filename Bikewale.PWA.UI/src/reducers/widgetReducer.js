import {combineReducers} from 'redux-immutable'
import {BikeImagesCarouselReducer} from './imageCarouselReducer'
var Widgets = combineReducers({
    BikeImagesCarouselReducer : BikeImagesCarouselReducer
});
module.exports = Widgets;