/**
 * This class represents a wrapper
 * arround header that is used in NCF
 * and conatin all presentation logic related to NCF header
 * @class HeaderWrapper
 * @extends {React.Component}
 */

import React from 'react'
import { connect } from 'react-redux'

import Header from '../../containers/Header'
import ShortlistIcon from './ShortListIcon'

class HeaderWrapper extends React.Component {
	constructor(props) {
		super(props)
		this.state = {
			isReverseScroll: false
		}
		this.lastScrollPosition = 0
	}
    componentDidMount() {
		window.addEventListener('scroll', this.floatHeader)
	}

	componentWillUnmount() {
		window.removeEventListener('scroll', this.floatHeader)
	}
	floatHeader = () => {
		if(this.props.isSticky && !this.props.isShortlistPopupActive && !this.props.isNavDrawerActive) {
			let windowScrollPosition = window.pageYOffset || document.documentElement.scrollTop
			if (this.lastScrollPosition > windowScrollPosition) {
				if(!this.state.isReverseScroll){
					this.setState({
						isReverseScroll: true
					})
				}
			}
			else if(this.lastScrollPosition < windowScrollPosition){
				if(this.state.isReverseScroll){
					this.setState({
						isReverseScroll: false
					})
				}
			}
			this.lastScrollPosition = windowScrollPosition
		}
	}
	getShortlistIcon = () => {
		if(this.props.isShortlistIconVisible){
			return <ShortlistIcon />
		}
		return null
	}
	render() {
		return (
			<Header
				rightChildern={ this.getShortlistIcon() }
				isSticky={ this.props.isSticky && this.state.isReverseScroll }
			/>
		)
	}
}

const mapStateToProps = (state) => {
	const {
		isSticky,
		isShortlistIconVisible
	} = state.newCarFinder.headerWrapper
	const {
		isNavDrawerActive
	} = state.header
	const {
		active
	} = state.newCarFinder.shortlistCars

	return {
		isSticky,
		isShortlistIconVisible,
		isNavDrawerActive,
		isShortlistPopupActive: active
	}
}

export default connect(mapStateToProps) (HeaderWrapper)
