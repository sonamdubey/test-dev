import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {toJS} from '../../immutableWrapperContainer'
import BikeImageCarouselComponent from './BikeImageCarouselComponent'
import { fetchPopularBikesDataForImageCarousel } from '../../actionCreators/imageCarouselActionCreator.js'

var mapStateToProps = function(store) {
    return {
        FetchArgs : [9,7],
        BikeImagesListData : store.getIn(['Widgets','BikeImagesCarouselReducer','PopularBikeImagesListData'])
    }
}

var mapDispatchToProps = function(dispatch) {
    return {
        fetchBikeImagesList : bindActionCreators(fetchPopularBikesDataForImageCarousel, dispatch)
    }
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(BikeImageCarouselComponent));