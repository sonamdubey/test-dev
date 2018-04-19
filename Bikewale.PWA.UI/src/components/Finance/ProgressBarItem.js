import React from 'react';
import PropTypes from 'prop-types'

const propTypes = {
	// count
  id: PropTypes.string,
  // status
  status: PropTypes.number
}

const defaultProps = {
  id: '',
  status: 2  // here, status 1: active; 2: disabled; 3: done 
}

class ProgressBarItem extends React.Component {
  constructor(props) {
    super(props);
    this.getClassName = this.getClassName.bind(this);
  }

  getClassName (status) {
    switch(status) {
      case 1:
      return 'active';
    case 2:
      return 'disabled';
    case 3:
      return 'done';
    }
  }
  render() {
    const {
      id,
      status
    } = this.props
    const classStatus = ' ' + this.getClassName(status);
    return(
      <div className="progress-bar__item">
        <div className={"progress-bar__item-content " + classStatus}>
            <span className="selection-field__index">{id}</span>
              {this.props.children}
          </div>
      </div>

    );
  }
}

ProgressBarItem.propTypes = propTypes
ProgressBarItem.defaultProps = defaultProps

export default ProgressBarItem
