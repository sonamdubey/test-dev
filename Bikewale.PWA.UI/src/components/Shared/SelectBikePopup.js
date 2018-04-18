import React from 'react';

import Autocomplete from '../Autocomplete';
import Accordion from '../Shared/Accordion';

class SelectBikePopup extends React.Component {
  constructor(props) {
    super(props);

    this.getList = this.getList.bind(this);
    this.setReference = this.setReference.bind(this);
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
      },
      {
        makeId: 9,
        makeName: "Yamaha"
      },
      {
        makeId: 10,
        makeName: "Suzuki"
      },
      {
        makeId: 11,
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

  setReference(ref) {
    this.popupContent = ref;
  }

  render() {
    const {
      isActive
    } = this.props

    const popupActiveClassName = isActive ? 'select-bike-popup--active' : ''
    const popupClasses = `select-bike-popup ${popupActiveClassName}`

    return (
      <div className={popupClasses}>
        <div ref={this.setReference} className="select-bike-popup__content">
          <div className="select-bike__head">
            <div className="select-bike-head__content">
              <span className="select-bike__close"></span>
              <div className="select-bike__search-box">
                <p className="select-bike-search__title">Select Make and Model</p>
                <div className="autocomplete-box">
                  <div className="autocomplete-field">
                    <Autocomplete
                      inputProps={{
                        className: "form-control",
                        placeholder: "Type to select Make and Model"
                      }}
                    />
                  </div>
                </div>
              </div>
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
