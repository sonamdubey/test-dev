import React from 'react';
import PropTypes from 'prop-types'

const propTypes = {
	// count
  stepNumber: PropTypes.number,
  // status
  status: PropTypes.number,
  //current focused
  currentFocused: PropTypes.String
}

const defaultProps = {
  stepNumber: -1,
  status: 1,  // here, status 1: disabled; 2: done 
  currentFocused: ''
}

class ProgressBarItem extends React.Component {
  constructor(props) {
    super(props);
    this.getClassName = this.getClassName.bind(this);
  }

  getClassName (status) {
    switch(status) {
      case 1:
      return 'disabled';
      case 2:
        return 'done';
      default:
        return 'disabled';
    }
  }
  render() {
    const {
      stepNumber,
      status
    } = this.props
    const classStatus = this.getClassName(status);
    console.log(this.props.currentFocused)
    return(
      <div className="progress-bar__item">
        <div className={"progress-bar-item__content " + classStatus + this.props.currentFocused}>
            { stepNumber != -1 
            ?
              <span className="progress-bar-item__step-count">{stepNumber}</span>
            :
              ''
            }
            <span className="progress-bar-item__step-title">{this.props.children}</span>
          </div>
      </div>

    );
  }
}

ProgressBarItem.propTypes = propTypes
ProgressBarItem.defaultProps = defaultProps

export default ProgressBarItem
