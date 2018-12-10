import React from 'react'
import Main from './Main'
import styles from '../../../style/filterplugin.scss';

class App extends React.Component {
	constructor(props) {
		super(props)
	}
	render() {
		return (
			<div>
				<Main filterPreSelected={this.props.filterPreSelected} cityId={this.props.cityId} trackingCategory={this.props.trackingCategory} callbackFunction = {this.props.callbackFunction}/>
			</div>
		)
	}
}

export default App
