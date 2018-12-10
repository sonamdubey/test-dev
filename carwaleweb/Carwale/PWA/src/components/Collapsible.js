import React from 'react'
import PropTypes from 'prop-types'

import {
	createRipple
} from '../utils/Ripple'

const propsTypes = {
	transitionTime: PropTypes.number,
	easing: PropTypes.string,
	open: PropTypes.bool,
	onOpen: PropTypes.func,
	onClose: PropTypes.func,
	classname: PropTypes.string,
	handleClick: PropTypes.func
}

const defaultProps = {
	transitionTime: 300,
	easing: 'linear',
	classname: '',
	open: false,
	onOpen: null,
	onClose: null,
	handleClick: null
}

/**
 * Reference: https://github.com/glennflanagan/react-collapsible/blob/develop/src/Collapsible.js
 */
class Collapsible extends React.Component {
	constructor(props) {
		super(props)

		if(props.open) {
			this.state = {
				isActive: true,
				height: 'auto',
				transition: `height ${props.transitionTime}ms ${props.easing}`
			}
		}
		else {
			this.state = {
				isActive: false,
				height: 0,
				transition: `height ${props.transitionTime}ms ${props.easing}`
			}
		}
	}

	componentWillReceiveProps(nextProps) {
		if (nextProps.open) {
			this.setState({
				isActive: true,
				height: this.refs.accordionBodyInner.offsetHeight || 'auto'
			})
		}
		else {
			this.setState({
				isActive: false,
				height: 0
			})
		}
	}

	handleClick = (event) => {
		if(this.state.isActive) {
			this.setState({
				isActive: false,
				height: 0
			})

			if (this.props.onClose) {
				this.props.onClose();
			}
		}
		else {
			this.setState({
				isActive: true,
				height: this.refs.accordionBodyInner.offsetHeight
			})

			if (this.props.onOpen) {
				this.props.onOpen();
			}
		}
		if (this.props.onToggle) {
			this.props.onToggle(event);
		}
	}

	render() {
		const itemState = this.state.isActive ? 'accordion-item--active ' : '';
		const bodyStyle = {
			height: this.state.height,
			WebkitTransition: this.state.transition,
			transition: this.state.transition
		}
		return (
			<div>
				<div
					onClick={this.handleClick}
					className={"accordion__head " + itemState + this.props.classname}
				>
					<div className="accordion-head__content">
						<p className="accordion-head__title">
							{this.props.title}
						</p>
						{
							this.props.selectionPreview
								? <p className="accordion-head__preview">{this.props.selectionPreview}</p>
								: ''
						}
					</div>
				</div>
				<div
					className="accordion__body"
					style={bodyStyle}
				>
					<div ref="accordionBodyInner">
						{this.props.children}
					</div>
				</div>
			</div>
		)
	}
}

Collapsible.propsTypes = propsTypes
Collapsible.defaultProps = defaultProps

export default Collapsible
