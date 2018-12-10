import React from 'react';
import PropTypes from 'prop-types';

const propTypes = {
	// Dropdown options
	options: PropTypes.array,
	// Property name to access value from `options` array
	optionValue: PropTypes.string,
	// Property name to access label from `options` array
	optionLabel: PropTypes.string,
	// Select placeholder
	placeholder: PropTypes.string,
	// Select value
	value: PropTypes.oneOfType([
		PropTypes.number,
		PropTypes.string,
	]),
	// Callback function get fired on value change
	onChange: PropTypes.func,
}

const defaultProps = {
	optionValue: 'value',
	optionLabel: 'label',
	placeholder: 'Select option',
}

class Select extends React.Component {
	constructor(props) {
		super(props);

		this.state = {
			value: props.value || -1
		}
	}

	handleChange = (event) => {
		const {
			options,
			placeholder,
			onChange,
		} = this.props;

		let selectedIndex = event.target.selectedIndex;
		let selectedOption = {};
		let isPlaceholderSelected;

		// Decrement selected index by 1 if placeholder is rendered
		if (placeholder) {
			selectedIndex = selectedIndex - 1;
		}

		isPlaceholderSelected = selectedIndex === -1 ? true : false;

		if (!isPlaceholderSelected) {
			selectedOption = options[selectedIndex];
		}

		this.setState({
			value: !isPlaceholderSelected ? selectedOption.value : -1
		})

		if(onChange) {
			onChange(selectedOption);
		}
	}

	getOptions = () => {
		const {
			options,
			optionValue,
			optionLabel,
		} = this.props;

		let newOptions = options.map((option) => {
			return {
				value: option[optionValue],
				label: option[optionLabel]
			}
		})

		return newOptions;
	}

	renderPlaceholder = () => {
		const {
			placeholder,
		} = this.props;

		return placeholder ? <option value="-1" disabled>{placeholder}</option> : null;
	}

	renderOptions = () => {
		const {
			selected
		} = this.state;

		let children = this.getOptions().map((option) => {
			return (
				<option
					key={option.value.toString()}
					value={option.value}
				>
					{option.label}
				</option>
			)
		})

		return children;
	}

	renderSelect = () => {
		const {
			value,
		} = this.state;

		const {
			options,
		} = this.props;

		if(options && options.length) {
			return (
				<select
					className="select-box__field"
					onChange={this.handleChange}
					value={value}
				>
					{this.renderPlaceholder()}
					{this.renderOptions()}
				</select>
			)
		}
	}

	render() {
		return (
			<div className="select-box-wrapper">
				{this.renderSelect()}
			</div>
		)
	}
}

Select.propTypes = propTypes;
Select.defaultProps = defaultProps;

export default Select;
