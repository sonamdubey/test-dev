//react 16 requirements https://reactjs.org/docs/javascript-environment-requirements.html
import 'raf/polyfill'
import 'core-js/es6/map'
import 'core-js/es6/set'

import React from 'react'
import { render } from 'react-dom'

import { Provider } from 'react-redux'
import { createStore, applyMiddleware } from 'redux'
import thunk from 'redux-thunk'

import { BrowserRouter } from 'react-router-dom'


import rootReducer from './reducers'
import App from './containers/App'

import 'core-js/fn/array/find-index'
require('es6-promise').polyfill()
import 'isomorphic-fetch'

const middleware = [thunk]
let storeEnhancer

if (__DEV__) {
	// imported here to allow tree shaking in production
	const { logger } = require('redux-logger')
	middleware.push(logger)
	const { composeWithDevTools } = require('redux-devtools-extension')
	storeEnhancer = composeWithDevTools(applyMiddleware(...middleware))
}
else {
	storeEnhancer = applyMiddleware(...middleware)
}

const store = createStore(
	rootReducer,
	undefined,
	storeEnhancer
)

render(
	<Provider store={store}>
		<BrowserRouter>
			<App />
		</BrowserRouter>
	</Provider>,
	document.getElementById('root')
)

// TODO: Handle service worker of used cars
// if (__PROD__ && "serviceWorker" in navigator) {
// 	window.addEventListener('load', () => {
// 		navigator.serviceWorker.register('/sw.js', { scope: "/find-car" }).then(registration => {
// 			console.log('SW registered: ', registration);
// 		}).catch(registrationError => {
// 			console.log('SW registration failed: ', registrationError);
// 		});
// 	});
// }
