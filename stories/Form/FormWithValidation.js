import React from 'react';

import Form from '../../src/Form';
import '../../src/Form/style/form-item.scss';
import Input from '../../src/Input';
import '../../src/Input/style/input.scss';
import Button from '../../src/Button';
import '../../src/Button/style/button.scss';

class FormWithValidation extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      username: '',
      usernameValidateStatus: '',
      usernamehelperText: '',
      mobile: '',
      mobileValidateStatus: '',
      mobilehelperText: ''
    }
  }

  handleUserNameChange = (event) => {
    this.setState({
      username: event.target.value
    })
  }

  handleMobileChange = (event) => {
    this.setState({
      mobile: event.target.value
    })
  }

  handleSubmit = () => {
    if (this.state.username.length < 3) {
      this.setState({
        usernameValidateStatus: 'error',
        usernamehelperText: 'Enter name'
      })
    }
    else {
      this.setState({
        usernameValidateStatus: '',
        usernamehelperText: ''
      })
    }

    if (!this.state.mobile.length) {
      this.setState({
        mobileValidateStatus: 'error',
        mobilehelperText: 'Enter mobile number'
      })
    }
    else if (this.state.mobile.length < 10) {
      this.setState({
        mobileValidateStatus: 'error',
        mobilehelperText: 'Mobile number should be 10 digits'
      })
    }
    else {
      this.setState({
        mobileValidateStatus: '',
        mobilehelperText: ''
      })
    }
  }

  render() {
    return (
      <div className="example-container">
        <Form>
          <Form.Item
            validateStatus={this.state.usernameValidateStatus}
            helperText={this.state.usernamehelperText}
          >
            <Input
              id="name"
              label="Name"
              placeholder="Enter your name"
              value={this.state.username}
              onChange={this.handleUserNameChange}
            />
          </Form.Item>
          <Form.Item
            validateStatus={this.state.mobileValidateStatus}
            helperText={this.state.mobilehelperText}
          >
            <Input
              id="mobile"
              label="Mobile"
              type="tel"
              placeholder="Enter your mobile"
              value={this.state.mobile}
              onChange={this.handleMobileChange}
              prefix="+91"
              maxLength={10}
            />
          </Form.Item>
          <Form.Item>
            <Button type="primary" onClick={this.handleSubmit}>Submit</Button>
          </Form.Item>
        </Form>
      </div>
    )
  }
}

export default FormWithValidation;
