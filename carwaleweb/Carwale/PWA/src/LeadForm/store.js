import { createStore, applyMiddleware } from "redux";
import thunk from "redux-thunk";

import rootReducer from "./Reducers/index";

class Store {
  constructor() {
    this.store = null;
  }

  createInstance() {
    const middleware = [thunk];
    let storeEnhancer;
    if (__DEV__) {
      const { logger } = require("redux-logger");
      middleware.push(logger);
      const { composeWithDevTools } = require("redux-devtools-extension");
      storeEnhancer = composeWithDevTools(applyMiddleware(...middleware));
    } else {
      storeEnhancer = applyMiddleware(...middleware);
    }

    this.store = createStore(rootReducer, storeEnhancer);
  }

  getInstance() {
    if (this.store == null) {
      this.createInstance();
    }
    return this.store;
  }
}

const store = new Store().getInstance();

export default store;
