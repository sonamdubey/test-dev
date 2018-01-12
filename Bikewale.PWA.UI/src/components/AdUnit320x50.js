import React from 'react'
import {isServer} from '../utils/commonUtils'
import {addAdSlot , removeAdSlot} from '../utils/googleAdUtils'


class AdUnit320x50 extends React.Component {
	constructor(props) {
		super(props);
		this.displayAd = this.displayAd.bind(this);
		this.refreshAd = this.refreshAd.bind(this);
		this.addAdSlot = this.addAdSlot.bind(this);
		this.clearAdDiv = this.clearAdDiv.bind(this);
	}
	clearAdDiv(){
		var element = document.getElementById(this.props.adContainerId);
		if(element) {
			element.setAttribute('data-google-query-id', '');
			element.innerHTML = '';

		}
	}
	addAdSlot(){
		addAdSlot(this.props.adSlot,[320,50],this.props.adContainerId,this.props.tags);
	}
	refreshAd() {
		if(!this.props)
			return;
		removeAdSlot(this.props.adSlot);
		this.clearAdDiv()
		this.addAdSlot();
		this.displayAd();
		
	}
	displayAd() {
		if(googletag) {
			var adContainerId = this.props.adContainerId;
			googletag.cmd.push(function() {
				googletag.display(adContainerId);
			})
		}
	}
	componentDidMount() {
		this.refreshAd();
	}
	componentWillUpdate(nextProps, nextState) {
		this.refreshAd();
	
	}

	componentWillUnmount(){
		
		removeAdSlot(this.props.adSlot);
		this.clearAdDiv();
	}
	shouldComponentUpdate(nextProps, nextState) {
		if(this.props.uniqueKey === nextProps.uniqueKey) {
			return false;
		}
		return true;
	}

	render() {
		return (
			<div id={this.props.adContainerId} className = "margin-top15 margin-bottom10 text-center" >
			
			</div>
		)
	}
}

module.exports = AdUnit320x50