
import {combineReducers} from 'redux-immutable'
import {NewsArticleListReducer} from './newsListReducer'
import {NewsDetailReducer} from './newsDetailReducer'


var News = combineReducers({
	NewsArticleListReducer : NewsArticleListReducer ,
	NewsDetailReducer : NewsDetailReducer
})

module.exports = News;