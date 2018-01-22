import React from 'react'
import {  StaticRouter , Route } from 'react-router-dom'

import ServerAppComponent from './ServerAppComponent'



class ServerRouterWrapper extends React.Component {
	
	render() {

		var url = this.props.Url;
    return (
        <StaticRouter location={url}>
          <ServerAppComponent childComponentProps={this.props}/>
        </StaticRouter>
     
    )
	}
}

module.exports = ServerRouterWrapper
