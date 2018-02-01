import { createStore, applyMiddleware} from 'redux'

import thunk from 'redux-thunk'

import rootReducer from '../reducers/rootReducer'

import {fromJS} from 'immutable'

var middleware;
if(process.env.NODE_ENV == 'production') {
	middleware = applyMiddleware(thunk);
}
else {
	var createLogger = require('redux-logger').createLogger;
	middleware = applyMiddleware(thunk,createLogger());  
}

export default function configureStore(initialData) {

	if(initialData == undefined || initialData == null) {
		return createStore(rootReducer,middleware);
	}
	else {
		return createStore(rootReducer,fromJS(initialData),middleware);
		
	}
}
