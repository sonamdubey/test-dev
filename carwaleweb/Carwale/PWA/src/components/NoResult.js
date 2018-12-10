import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
	// custom component specific class
	type: PropTypes.string,
	// custom image class to provide component specific style
	imageClass: PropTypes.string,
	// no result title
	title: PropTypes.string,
	// no result subtitle
	subtitle: PropTypes.string
}

const defaultProps = {
	type: '',
	imageClass: null,
	title: 'No result found',
	subtitle: null
}

class NoResult extends React.Component {
	constructor(props) {
		super(props)
	}

	render() {
		return (
			<div className={"no-result__content " + this.props.type}>
				{
					this.props.imageClass
						? <div className={this.props.imageClass}></div>
						: ''
				}

				<p className="no-result__title">{this.props.title}</p>

				{
					this.props.subtitle
						?	<p className="no-result__subtitle">{this.props.subtitle}</p>
						: ''
				}

				{this.props.children}
			</div>
		)
	}
}

NoResult.propTypes = propTypes
NoResult.defaultProps = defaultProps

export default NoResult
