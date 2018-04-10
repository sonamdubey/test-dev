import React from 'react';

import Accordion from '../Shared/Accordion';

class SelectBikePopup extends React.Component {
  constructor(props) {
    super(props);
    
    this.getList = this.getList.bind(this);
  }

  getList() {
    let data = [
      {
        makeId: 1,
        makeName: "Honda"
      },
      {
        makeId: 2,
        makeName: "Royal Enfield"
      },
      {
        makeId: 3,
        makeName: "TVS"
      },
      {
        makeId: 4,
        makeName: "Bajaj"
      },
      {
        makeId: 5,
        makeName: "Hero"
      },
      {
        makeId: 6,
        makeName: "Yamaha"
      },
      {
        makeId: 7,
        makeName: "Suzuki"
      },
      {
        makeId: 8,
        makeName: "KTM"
      }
    ];

    let list = data.map(function(item) {
      return (
        <div data-trigger={item.makeName}>
          <ul className="panel-body__list">
            <li className="panel-bike-list__item bike-list-item--active">
              <p className="bike-list-item__label">Discover 135</p>
            </li>
            <li className="panel-bike-list__item">
              <p className="bike-list-item__label">Pulsar</p>
            </li>
            <li className="panel-bike-list__item">
              <p className="bike-list-item__label">Platina</p>
            </li>
            <li className="panel-bike-list__item">
              <p className="bike-list-item__label">Discover 125</p>
            </li>
          </ul>
        </div>
      )
    });

    return (
      list
    )
  }

  render() {
    let {
      isActive
    } = this.props

    const popupActiveClassName = isActive ? 'select-bike-popup--active' : ''

    return (
      <div className={"select-bike-popup " + popupActiveClassName}>
        <div className="select-bike-popup__content">
          <div className="select-bike__head">
            <div className="select-bike-head__content">
              <span className="select-bike__close"></span>
            </div>
          </div>
          <div className="select-bike__body select-bike__accordion">
            <Accordion closeable={true}>
              {this.getList()}
            </Accordion>
          </div>
          <div className="select-bike__footer">
            <span className="select-bike__next">Next</span>
          </div>
        </div>
      </div>
    );
  }
}

export default SelectBikePopup;
