import React from 'react';

class SelectionStepCard extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return(
      <div className="selection-steps__card">
        <div className="selection-step__card-content">
          {this.props.children}
        </div>
      </div>
    );
  }
}

export default SelectionStepCard