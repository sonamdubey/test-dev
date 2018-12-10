import React from 'react'

class SwitchElement extends React.Component {
    constructor(props) {
        super(props);
    }
    onValueChanged = (event) => {
        if(this.props.onSwitchChange) {
            this.props.onSwitchChange(event);
        }
    }
    render() {
        const {
            id,
            value,
            defaultChecked,
            labelName,
            radioButtonName,
            status
        } = this.props
      return (
        <div className="input-radio">
            <input
                type="radio"
                name={radioButtonName}
                id={id}
                value={value}
                onChange={this.onValueChanged}
                defaultChecked={status}
            />
            <label htmlFor={id}>{labelName}</label>
        </div>
      );
    }
}
export default SwitchElement

