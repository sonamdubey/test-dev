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
import Cookies  from 'js-cookie'
import 'core-js/fn/array/find-index'
require('es6-promise').polyfill()
import 'isomorphic-fetch'
import { throttle } from 'throttle-debounce'

import rootReducer from './reducers';
import Location from './containers/Location';
import Events from './utils/events';

import 'core-js/fn/object/assign'
import 'core-js/fn/array/find-index'

const middleware = [thunk]
let storeEnhancer

if(__DEV__) {
  // imported here to allow tree shaking in production
  const { logger } = require('redux-logger')
  middleware.push(logger)
  const { composeWithDevTools } = require('redux-devtools-extension')
  storeEnhancer = composeWithDevTools(applyMiddleware(...middleware))
}
else{
  storeEnhancer = applyMiddleware(...middleware)
}

const store = createStore(
  rootReducer,
  storeEnhancer
)

// Store location store and events in global window scope
window.LOCATION_STORE = store;
window.LOCATION_EVENTS = new Events(store);

render(
  <Provider store={store}>
    <BrowserRouter>
      <Location />
    </BrowserRouter>
  </Provider>,
  document.getElementById('root')
)
