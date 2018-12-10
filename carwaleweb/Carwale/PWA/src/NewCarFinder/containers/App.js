import React from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'

import styles from '../../../style/main.scss';

import Main from './Main'

import Toast from '../../components/Toast'

class App extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		return (
			<div>
				<Main />
			</div>
		)
	}
}

export default App
