import React from 'react';

import Autocomplete from '../Autocomplete';
import Accordion from '../Shared/Accordion';
import NoResult from './NoResult';

class SelectBikePopup extends React.Component {
  constructor(props) {
    super(props);
  }

  handleCloseClick = () => {
    if (this.props.onCloseClick) {
      this.props.onCloseClick();
    }
  }

  getList = () => {
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

  render() {
    const {
      isActive
    } = this.props

    const popupActiveClassName = isActive ? 'popup--active' : ''
    const popupClasses = `select-bike-popup popup-content ${popupActiveClassName}`

    return (
      <div className={popupClasses}>
        <div className="select-bike-popup__content">
          <div className="popup__head">
            <div className="popup-head__content">
              <span onClick={this.handleCloseClick} className="popup__close"></span>
              <div className="popup__search-box">
                <p className="popup-search__title">Select Make and Model</p>
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
            {/*<NoResult
              type="select-bike__no-bike-content"
              imageClass="select-bike__no-bike"
              title="No Matching Bikes Found"
            />*/}
          </div>
          <div className="popup__footer">
            <span className="popup-footer__next">Next</span>
          </div>
        </div>
      </div>
    );
  }
}

export default SelectBikePopup;
