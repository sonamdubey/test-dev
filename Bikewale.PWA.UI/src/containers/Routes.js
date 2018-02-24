import React from 'react'
import { Switch ,  Route } from 'react-router-dom'
import {isServer} from '../utils/commonUtils'


import universal from 'react-universal-component'
import universalImport from 'babel-plugin-universal-import/universalImport.js'
import importCss from 'babel-plugin-universal-import/importCss.js'
import path from 'path'
import Spinner from '../components/Shared/Spinner'
var origin = isServer() ? '' : window.location.origin;

var universalComponentOptions = {
	loading : null,
	loadingTransition : false
}

//VIDEOS
var VideoLandingUC = universal(props => universalImport({
	chunkName : 'videosBundle',
	resolve : () => require.resolveWeak('../components/Videos/VideoLandingComponent'),
	load : (props) => Promise.all([
			System.import( /* webpackChunkName : 'videosBundle' */ `../components/Videos/VideoLandingComponent`),
			importCss('videosBundle')
		]).then(function(props,proms) {
			return proms[0];
		}.bind(null,props) ).catch(function(){
		})
}),universalComponentOptions);
var VideoCategoryUC = universal(props => universalImport({
	chunkName : 'videosBundle',
	resolve : () => require.resolveWeak('../components/Videos/VideosByCategoryContainer'),
	load : (props) => Promise.all([
			System.import( /* webpackChunkName : 'videosBundle' */ `../components/Videos/VideosByCategoryContainer`),
			importCss('videosBundle')
		]).then(function(props,proms) {
			return proms[0];
		}.bind(null,props) ).catch(function(){
		})
}),universalComponentOptions);
var VideoDetailUC = universal(props => universalImport({
	chunkName : 'videosBundle',
	resolve : () => require.resolveWeak('../components/Videos/VideoDetailContainer'),
	load : (props) => Promise.all([
			System.import( /* webpackChunkName : 'videosBundle' */ `../components/Videos/VideoDetailContainer`),
			importCss('videosBundle')
		]).then(function(props,proms) {
			return proms[0];
		}.bind(null,props) ).catch(function(){
		})
}),universalComponentOptions);

//NEWS
var NewsListUC = universal(props => universalImport({
	chunkName : 'newsBundle',
	resolve : () => require.resolveWeak('../components/News/NewsArticleListContainer'),
	load : (props) => Promise.all([
			System.import( /* webpackChunkName : 'newsBundle' */ `../components/News/NewsArticleListContainer`),
			importCss('newsBundle')
		]).then(function(props,proms) {
			return proms[0];
		}.bind(null,props) ).catch(function(){
		})
}),universalComponentOptions);
var NewsDetailUC = universal(props => universalImport({
	chunkName : 'newsBundle',
	resolve : () => require.resolveWeak('../components/News/NewsDetailContainer'),
	load : (props) => Promise.all([
			System.import( /* webpackChunkName : 'newsBundle' */ `../components/News/NewsDetailContainer`),
			importCss('newsBundle')
		]).then(function(props,proms) {
			return proms[0];
		}.bind(null,props) ).catch(function(){
		})
}),universalComponentOptions);


module.exports = () => {
	var path = window.location.pathname;
	return <Switch>
			<Route exact path='/m/(news|expert-reviews)/' render={(props) => (<NewsListUC path ={path} />)}/>
		    <Route exact path='/m/(news|expert-reviews)/page/:pageNo/' render={(props) => (<NewsListUC path ={path}/>)}/>
		    <Route exact path='/m/(news|expert-reviews)/:basicId(\d+)-:title.html' render={(props) => (<NewsDetailUC path ={path}/>)}/>
		    <Route exact path='/m/bike-videos/' render={(props) => (<VideoLandingUC path ={path}/>)}/>
		    <Route exact path='/m/bike-videos/category/*-:categoryId(\d+)/' render={(props) => (<VideoCategoryUC path ={path}/>)}/>
			<Route exact path='/m/bike-videos/:title-:basicId(\d+)/' render={(props) => (<VideoDetailUC path ={path}/>)}/>

		</Switch>
}

