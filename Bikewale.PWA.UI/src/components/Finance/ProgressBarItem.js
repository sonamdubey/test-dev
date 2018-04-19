import React from 'react';
import PropTypes from 'prop-types'

const propTypes = {
	// class name
	childClassName: PropTypes.string,
	// count
  id: PropTypes.string,
  // status
  status: PropTypes.number
}

const defaultProps = {
	childClassName: '',
  id: '',
  status: '2'  // here, status 1: active; 2: disabled; 3: done 
}

class ProgressBar extends React.Component {
  constructor(props) {
    super(props);
  }

  getClassName() {
    switch(this.props.status) {
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
      childClassName,
      id,
      status
    } = this.props
    const classStatus = ' '+this.getClassName();
    return(
      <div className="progress-bar__item">
        <div className={"progress-bar__item-content " + childClassName + classStatus}>
            <span className="selection-field__index">{id}</span>
              {this.props.children}
          </div>
      </div>

    );
  }
}

ProgressBar.propTypes = propTypes
ProgressBar.defaultProps = defaultProps

export default ProgressBar
