import React from 'react';

import Autocomplete from '../Autocomplete';
import PopularCityList from './PopularCityList';
import ListGroup from './ListGroup';
import ListGroupItem from './ListGroupItem';

class SelectCityPopup extends React.Component {
  constructor(props) {
    super(props);

    this.handleCityClick = this.handleCityClick.bind(this);
    this.getOtherCityList = this.getOtherCityList.bind(this);
  }

  componentDidMount() {
    this.props.fetchCity();
  }

  handleCityClick(item) {
    if (this.props.onCityClick) {
      this.props.onCityClick(item);
    }
  }

  getOtherCityList() {
    const {
      data,
      onClick
    } = this.props

    let listItems = data.Other.map(function(item, index) {
      let active = data.Selection.cityId === item.cityId ? true : false;

      return (
        <ListGroupItem
          id={item.cityId}
          name={item.cityName}
          active={active}
          onClick={this.handleCityClick.bind(this, item)}
        />
      )
    }, this)

    return (
      <ListGroup type="other-city__list">
        {listItems}
      </ListGroup>
    )
  }

  render() {
    const {
      isActive,
      data
    } = this.props

    const popupActiveClassName = isActive ? 'select-bike-popup--active' : ''
    const popupClasses = `select-bike-popup ${popupActiveClassName}`

    return (
      <div className={popupClasses}>
        <div className="select-bike-popup__content">
          <div className="select-bike__head">
            <div className="select-bike-head__content">
              <span className="select-bike__close"></span>
              <div className="select-bike__search-box">
                <p className="select-bike-search__title">Select City</p>
                <div className="autocomplete-box">
                  <div className="autocomplete-field">
                    <Autocomplete
                      value={data.Selection ? data.Selection.cityName : ''}
                      inputProps={{
                        className: "form-control",
                        placeholder: "Type to select city"
                      }}
                    />
                    {
                      data.Selection && data.Selection.cityId > 0 && 
                      (
                        <span className="autocomplete-box__clear">Clear</span>
                      )
                    }
                  </div>
                </div>
              </div>
            </div>
          </div>
          {
            data.Popular && data.Other && (
              <div className="select-bike__body select-bike__accordion">
                <div className="select-city__popular">
                  <p className="city-list__heading">Popular cities</p>
                  <PopularCityList
                    data={data.Popular}
                    selection={data.Selection}
                    onClick={this.handleCityClick}
                  />
                </div>
                <div className="select-city__popular">
                  <p className="city-list__heading">Other cities</p>
                  {this.getOtherCityList()}
                </div>
              </div>
            )
          }
          <div className="select-bike__footer">
            <span className="select-bike__next">Next</span>
          </div>
        </div>
      </div>
    );
  }
}

export default SelectCityPopup;
