import React from 'react'
import {isServer} from '../utils/commonUtils'
import {addAdSlot , removeAdSlot} from '../utils/googleAdUtils'
class AdUnit320x50 extends React.Component {
	componentDidMount() {
		if(!this.props)
			return;
		addAdSlot(this.props.adSlot,[320,50],this.props.adContainerId,this.props.tags);
	}
	
	render() {
		return (
			<div id={this.props.adContainerId} className = "margin-top15 margin-bottom10 text-center" >
				
			</div>
		)
	}
}

module.exports = AdUnit320x50



