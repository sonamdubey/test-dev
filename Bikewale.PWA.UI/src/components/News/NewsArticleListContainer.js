import {bindActionCreators} from 'redux'
import {connect} from 'react-redux'
import ArticleListComponent from './ArticleListComponent'
import { resetArticleListData,fetchNewsArticleList , fetchNewBikesListDataForNewsList } from '../../actionCreators/newsArticleListActionCreators'
import {fetchNewsArticleDetail} from '../../actionCreators/newsArticleDetailActionCreators'

import {toJS} from '../../immutableWrapperContainer'


var mapStateToProps = function(store){
	
	return {
		ArticleListData : store.getIn(['News','NewsArticleListReducer','ArticleListData']),
		NewBikesListData : store.getIn(['News','NewsArticleListReducer','NewBikesListData'])
	}

}

var mapDispatchToProps = function(dispatch){
	return {
		fetchArticleList : bindActionCreators(fetchNewsArticleList,dispatch),
		fetchArticleDetail : bindActionCreators(fetchNewsArticleDetail,dispatch),
		fetchNewBikesListData : bindActionCreators(fetchNewBikesListDataForNewsList,dispatch),
		resetArticleListData : bindActionCreators(resetArticleListData,dispatch)
	}
}

var NewsArticleListContainer = connect(mapStateToProps,mapDispatchToProps)(toJS(ArticleListComponent));

export default NewsArticleListContainer;
