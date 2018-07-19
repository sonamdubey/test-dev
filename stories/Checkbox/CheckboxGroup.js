import React from 'react';

import CheckboxGroup from '../../src/Checkbox/Group';
import '../../src/Checkbox/style/checkbox.scss';

class CheckboxGroupExample extends React.Component {
  constructor(props) {
    super(props);

    this.checkboxOptions = [
      {
        value: 1,
        label: "Apple"
      },
      {
        value: 2,
        label: "Banana"
      },
      {
        value: 3,
        label: "Cherry"
      },
      {
        value: 4,
        label: "Dragon Fruit"
      }
    ]
  }

  handleChange(values) {
    console.log('value: ', values)
  }

  render() {
    return (
      <div className="example-container">
        <CheckboxGroup
          options={this.checkboxOptions}
          onChange={this.handleChange}
          defaultValue={[1]}
          name="fruits"
          type={this.props.type}
        />
      </div>
    )
  }
}

export default CheckboxGroupExample;
