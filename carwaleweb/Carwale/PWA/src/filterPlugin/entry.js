//react 16 requirements https://reactjs.org/docs/javascript-environment-requirements.html
import 'raf/polyfill'
import 'core-js/es6/map'
import 'core-js/es6/set'

import React from 'react'
import {render} from 'react-dom'
import {Provider} from 'react-redux'
import {createStore,applyMiddleware} from 'redux'
import thunk from 'redux-thunk'
import { BrowserRouter } from 'react-router-dom'
import { throttle } from 'throttle-debounce'
import { storage } from '../utils/Storage'
import rootReducer from './reducers'
import App from './containers/App'
import 'core-js/fn/object/assign'
import 'core-js/fn/array/find-index'
require('es6-promise').polyfill()
import 'isomorphic-fetch'
import {openFilterScreen} from './actionCreators/FiltersScreen'
import Cookies  from 'js-cookie'
const middleware = [thunk]
let storeEnhancer

if (__DEV__) {
	// imported here to allow tree shaking in production
	const {logger} = require('redux-logger')
	middleware.push(logger)
	const {composeWithDevTools} = require('redux-devtools-extension')
	storeEnhancer = composeWithDevTools(applyMiddleware(...middleware))
} else {
	storeEnhancer = applyMiddleware(...middleware)
}

let preloadedState = storage.getValue('store')

const store = createStore(
	rootReducer,
	preloadedState ? preloadedState : undefined,
	storeEnhancer
)

store.subscribe(throttle(1000, () => {
	const state = store.getState()
	const { filtersScreen } = state

	let localStorage = storage.getValue('store')

	storage.setValue('store', {
		... localStorage,
		filtersScreen : {
			filters: filtersScreen.filters
		}
	})
}))

const filterBtn = document.getElementsByClassName("filterPluginBtn")
function renderApp(reqFilters, cityId, filterPreSelected, trackingCategory, callbackFunc){
	render(
		<Provider store = {store} >
			<BrowserRouter>
				<div className="m-container">
					<App reqFilters={reqFilters} cityId={cityId} filterPreSelected={filterPreSelected} trackingCategory={trackingCategory} callbackFunction = {callbackFunc}/>
				</div>
			</BrowserRouter>
		</Provider>,
		document.getElementById("root")
	);
}

function initialize() {
	const masterCityId = Cookies.get('_CustCityIdMaster')
	const cityId = masterCityId ? parseInt(masterCityId) : ''
	renderApp(this.getAttribute('data-reqfilters'),cityId,this.getAttribute('data-filterpreselected'),this.getAttribute('data-trackingCategory'), this.getAttribute('data-callbackFunc')  )
	store.dispatch(openFilterScreen(null))
}

registerEvents();

function registerEvents() {
	for (let i = 0; i < filterBtn.length; i++) {
		filterBtn[i].addEventListener("click", initialize, false);
	}
}
