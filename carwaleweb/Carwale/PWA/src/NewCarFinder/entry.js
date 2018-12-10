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
import { throttle } from 'throttle-debounce'
import { storage } from '../utils/Storage'
import { setAbtestCookieValue } from './utils/ProductExperiment'
import { ClientErrorLogger } from '../utils/ErrorLogger';

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

let preloadedState = storage.getSessionValue('store')
if(preloadedState && preloadedState.newCarFinder && preloadedState.newCarFinder.filter)
{
	preloadedState = undefined
	storage.clearSessionValue('store')
}

setAbtestCookieValue()

let getMergedLocalAndSessionStore = () => {
	let preloadedState = storage.getSessionValue('store')
	let preloadedStateFromLocalStorage = storage.getValue('store')
	// Merge store from loal storage into preload state.
	if (preloadedStateFromLocalStorage != undefined)
	{
		if(preloadedState != undefined)
		{
			preloadedState.newCarFinder['shortlistCars'] = preloadedStateFromLocalStorage.newCarFinder.shortlistCars
			preloadedState.filtersScreen = preloadedStateFromLocalStorage.filtersScreen
		}
		else
		{
			preloadedState = preloadedStateFromLocalStorage;
		}
	}
	return preloadedState;
}

preloadedState = getMergedLocalAndSessionStore()

const store = createStore(
	rootReducer,
	preloadedState ? preloadedState : undefined,
	storeEnhancer
)

store.subscribe(throttle(1000, () => {
	const state = store.getState()
	const { newCarFinder, filtersScreen } = state
	storage.setSessionValue('store', {
		newCarFinder: {
			budget: newCarFinder.budget,
			bodyType: newCarFinder.bodyType,
			fuelType: newCarFinder.fuelType,
			make: newCarFinder.make,
			transmissionType: newCarFinder.transmissionType,
			seats: newCarFinder.seats
		},
		compareCars: state.compareCars
	})

	storage.setSessionValue('cwc_ncfPopUp',1)


	storage.setValue('store', {
		newCarFinder: {
			shortlistCars: newCarFinder.shortlistCars
		},
		filtersScreen:{
			filters: filtersScreen.filters
		}
	})
}))

render(
	<Provider store={store}>
		<BrowserRouter>
			<div className="m-container">
				<App />
			</div>
		</BrowserRouter>
	</Provider>,
	document.getElementById('root')
)

if ("serviceWorker" in navigator) {
	window.addEventListener('load', () => {
		navigator.serviceWorker.register('/sw.js', { scope: "/find-car" }).then(registration => {
			console.log('SW registered: ', registration);
		}).catch(registrationError => {
			console.log('SW registration failed: ', registrationError);
		});
	});
}

ClientErrorLogger(["aeplcdn", "carwale"], "/api/exceptions/")

