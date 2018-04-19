import React from 'react';

import ProgressBarItem from './ProgressBarItem'

class ProgressBar extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return(
      <div className="progress-bar">
        {this.props.children}
      </div>

    );
  }
}

export default ProgressBar