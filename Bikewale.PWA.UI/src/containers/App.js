import React from 'react'
import {render} from 'react-dom'
import { BrowserRouter } from 'react-router-dom'
import Navigation from '../components/Shared/Navigation'
import NavigationDrawer from '../components/Shared/NavigationDrawer'
import AdUnit320x50 from '../components/AdUnit320x50'

import {isServer} from '../utils/commonUtils'
import {addAdSlot , removeAdSlot} from '../utils/googleAdUtils'

import LoadPeripheralComponents from '../components/peripheralComponents'

import Routes from './Routes'


class App extends React.Component {
 
    constructor(props) {
        super(props);
        this.state = {
            PeripheralComponents : null
        }
       
           
    }
    componentWillMount() {

        LoadPeripheralComponents().then(function(PeripheralComponents){

            var PeripheralComponents = (<div>
                                            <PeripheralComponents.GlobalCityPopup/>
                                            <PeripheralComponents.GlobalSearchPopup/>
                                            <BrowserRouter>
                                                <PeripheralComponents.OnRoadPricePopup onRoadPriceDataObject={PeripheralComponents.onRoadPricePopupDataObject}/>
                                            </BrowserRouter>
                                        </div>)
            this.setState({
                PeripheralComponents : PeripheralComponents
            });
          

        }.bind(this))
    }
    render() {
        
        return (
            <div>
                <Navigation/>
                <div className="body-content">
                   {Routes()}
                </div>
                <NavigationDrawer/>
                <div id="peripheralComponents">
                    {this.state.PeripheralComponents}
                </div>
                <div className="blackOut-window"></div>
               
            </div>
        )
                    
	}
}

module.exports = App
