import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import ArticleDetailComponent from './ArticleDetailComponent'
import {fetchNewsArticleDetail , fetchNewBikesListDataForNewsDetail , fetchRelatedModelObjectForNewsDetail} from '../../actionCreators/newsArticleDetailActionCreators'

import {toJS} from '../../immutableWrapperContainer'

var mapStateToProps = function(store) {
	return {
		ArticleDetailData : store.getIn(['News','NewsDetailReducer','ArticleDetailData']),
		RelatedModelObject : store.getIn(['News','NewsDetailReducer','RelatedModelObject']),
		NewBikesListData :store.getIn(['News','NewsDetailReducer','NewBikesListData'])
	}
}

var mapDispatchToProps = function(dispatch) {
	return {
		fetchArticleDetail : bindActionCreators(fetchNewsArticleDetail,dispatch),
		fetchNewBikesListData : bindActionCreators(fetchNewBikesListDataForNewsDetail,dispatch),
		fetchRelatedModelObject : bindActionCreators(fetchRelatedModelObjectForNewsDetail,dispatch)
	}
}

var newsDetailContainer = connect(mapStateToProps,mapDispatchToProps)(toJS(ArticleDetailComponent));

module.exports = newsDetailContainer;

