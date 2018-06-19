import React from 'react';
import PropTypes from 'prop-types'

import { progressBarStatus } from '../../utils/progressBarConstants'

const propTypes = {
  // count
  stepNumber: PropTypes.number,
  // status
  status: PropTypes.number,
  //to check it is active or not
  isActive: PropTypes.bool
}

const defaultProps = {
  stepNumber: -1,
  status: progressBarStatus.DISABLE,
  isActive: false  //here, true is on focus
}

class ProgressBarItem extends React.Component {
  constructor(props) {
    super(props);
    this.getClassName = this.getClassName.bind(this);
  }

  getClassName (status) {
    switch(status) {
      case progressBarStatus.DISABLE:
      return 'disabled';
      case progressBarStatus.DONE:
        return 'done';
      default:
        return 'disabled';
    }
  }
  render() {
    const {
      stepNumber,
      status,
      isActive
    } = this.props
    const classStatus = this.getClassName(status);
    const classActive = classActive ? ' focused' : ' ';
    return(
      <div className="progress-bar__item">
        <div className={`progress-bar-item__content ${classStatus} ${classActive}`}>
            { stepNumber != -1 
            ?
              <span className="progress-bar-item__step-count">
                <span className="progress-step-count__label">{stepNumber}</span>
              </span>
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
