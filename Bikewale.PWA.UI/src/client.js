import React from 'react'
import {render} from 'react-dom'
import {BrowserRouter} from 'react-router-dom'

import {Provider} from 'react-redux'



import configureStore from './store/configureStore'

import App from './containers/App'

import {onPopState , wrapHistoryAPIFunction , isBrowserWithoutScrollSupport} from './utils/scrollUtils'
if(!process.env.SERVER) {
	require('../stylesheet/app.sass');
}

if(process.env.NODE_ENV != 'production') {
	var preactDevtools = require('preact/devtools');
}


var ua = window.navigator.userAgent;

if(!isBrowserWithoutScrollSupport()){
	
	wrapHistoryAPIFunction();
	window.addEventListener('popstate',onPopState,true);
}

function AppEntryPoint() {
	var reduxState = window.state;
	if(reduxState) {
		window._SERVER_RENDERED_DATA = true;
		
	}

	var store = configureStore(reduxState);

	window._SERVER_RENDERED_DATA = false;
	
	
	render(
		<Provider store={store}>
			<BrowserRouter>
				<App/>
			</BrowserRouter>
		</Provider>
		,document.getElementById('root'));

	
}

window.AppEntryPoint = AppEntryPoint;


