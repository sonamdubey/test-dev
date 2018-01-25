import React from 'react'

class Autocomplete extends React.Component {
	constructor(props) {
		super(props);
	}
	render() {
		
		var inputProps = {};
		if(this.props.inputProps) {
			inputProps = this.props.inputProps;
		}

		return(
				<div style={this.props.wrapperStyle}>
					<input type='text'
							id={inputProps.id ? inputProps.id:''} 
							placeholder={inputProps.placeholder ? inputProps.placeholder:''} 
							className = {inputProps.className ? inputProps.className:''} 
							value = {this.props.value ? this.props.value : ''}
							onchange = {()=>{typeof this.props.onChange == 'function' ? this.props.onChange() : null}} />
					{typeof this.props.renderMenu == 'function' ? this.props.renderMenu(this.props.items,this.props.value) : null}
				</div>
			)

	}

}


module.exports = Autocomplete
