import React from 'react';

class SelectBikePopup extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    let {
      isActive
    } = this.props

    const popupActiveClassName = isActive ? 'select-bike-popup--active' : ''

    return (
      <div className={"select-bike-popup " + popupActiveClassName}>
        <div className="select-bike__head">
          <div className="select-bike-head__content">
            <span className="select-bike__close"></span>
          </div>
        </div>
        <div className="select-bike__body">
        </div>
        <div className="select-bike__footer">
          <span className="select-bike__next">Next</span>
        </div>
      </div>
    );
  }
}

export default SelectBikePopup;
