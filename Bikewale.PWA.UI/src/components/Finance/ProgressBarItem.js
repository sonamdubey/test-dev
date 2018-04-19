import React from 'react';
import PropTypes from 'prop-types'

const propTypes = {
	// count
  id: PropTypes.number,
  // status
  status: PropTypes.number
}

const defaultProps = {
  id: {},
  status: 1  // here, status 1: disabled; 2: active; 3: done 
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
      return 'active';
    case 3:
      return 'done';
    default:
      return 'disabled';
    }
  }
  render() {
    const {
      id,
      status
    } = this.props
    const classStatus = this.getClassName(status);
    return(
      <div className="progress-bar__item">
        <div className={"progress-bar__item-content " + classStatus}>
            <span className="selection-field__index">{id}</span>
            <span className="selection-field__title">{this.props.children}</span>
          </div>
      </div>

    );
  }
}

ProgressBarItem.propTypes = propTypes
ProgressBarItem.defaultProps = defaultProps

export default ProgressBarItem
