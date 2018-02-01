
import Promise from 'promise-polyfill'; 
import {isServer}  from '../utils/commonUtils'
if(!isServer()) {
    if(!window.Promise) {
        window.Promise = Promise;
    }
}
export default () => {  
    return new Promise(resolve => {
        require.ensure([], function(require){
            var GlobalCityPopup = require('../components/GlobalCityPopup');
            var GlobalSearchPopup = require('../components/GlobalSearchPopup');
            var OnRoadPricePopup = require('../components/OnRoadPricePopup');
            var onRoadPricePopupDataObject = require('../utils/popUpUtils').onRoadPricePopupDataObject;
                       
            resolve({
                GlobalCityPopup: GlobalCityPopup,
                GlobalSearchPopup : GlobalSearchPopup,
                OnRoadPricePopup : OnRoadPricePopup,
                onRoadPricePopupDataObject : onRoadPricePopupDataObject
            });
        }, "peripheralComponents");
    });
};
