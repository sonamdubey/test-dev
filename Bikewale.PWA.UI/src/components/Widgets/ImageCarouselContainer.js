import {connect} from 'react-redux'
import {bindActionCreators} from 'redux'
import {toJS} from '../../immutableWrapperContainer'
import ImageCarouselComponent from './ImageCarouselComponent'
import { fetchPopularBikesDataForImageCarousel } from '../../actionCreators/imageCarouselActionCreator.js'

var mapStateToProps = function(store) {
    return {
        BikeImagesListData : store.getIn(['Widgets','BikeImagesCarousel','PopularBikeImagesListData'])
    }
}

var mapDispatchToProps = function(dispatch) {
    return {
        fetchPopularBikesDataForImageCarousel : bindActionCreators(fetchPopularBikesDataForImageCarousel, dispatch)
    }
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(toJS(ImageCarouselComponent));