import React from 'react';

import Autocomplete from '../Autocomplete';
import PopularCityList from './PopularCityList';
import ListGroup from './ListGroup';
import ListGroupItem from './ListGroupItem';

import { unlockScroll } from '../../utils/scrollLock';
import { addPopupEvents, removePopupEvents } from '../../utils/popupScroll';

class SelectCityPopup extends React.Component {
  constructor(props) {
    super(props);
  }

  componentDidMount() {
    this.props.fetchCity();
    addPopupEvents(this.popupContent)
  }
  
  componentWillUnmount() {
    removePopupEvents(this.popupContent)
  }

  handleCityClick = (item) => {
    if (this.props.onCityClick) {
      this.props.onCityClick(item);
    }
  }

  handleCloseClick = () => {
    if (this.props.onCloseClick) {
      this.props.onCloseClick();
    }
    
    unlockScroll();
  }

  getOtherCityList = () => {
    const {
      data,
      onClick
    } = this.props

    let listItems = data.Other.map((item, index) => {
      let active = data.Selection.cityId === item.cityId ? true : false;

      return (
        <ListGroupItem
          id={item.cityId}
          name={item.cityName}
          active={active}
          onClick={this.handleCityClick.bind(this, item)}
        />
      )
    })

    return (
      <ListGroup type="other-city__list">
        {listItems}
      </ListGroup>
    )
  }
  
  setContentRef = (ref) => {
    this.popupContent = ref
  }

  render() {
    const {
      isActive,
      data
    } = this.props

    const popupActiveClassName = isActive ? 'popup--active' : ''
    const popupClasses = `select-city-popup popup-content ${popupActiveClassName}`

    return (
      <div className={popupClasses}>
        <div ref={this.setContentRef} className="select-city-popup__content">
          <div className="popup__head">
            <div className="popup-head__content">
              <span onClick={this.handleCloseClick} className="popup__close"></span>
              <div className="popup__search-box">
                <p className="popup-search__title">Select City</p>
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
                      data.Selection && data.Selection.cityId > 0
                        ? <span className="autocomplete-box__clear">Clear</span>
                        : <span className="select-city__locate-me"></span> 
                    }
                  </div>
                </div>
              </div>
            </div>
          </div>
          {
            data.Popular && data.Other && (
              <div className="select-city__body">
                <div className="city-list-content">
                  <p className="city-list__heading">Popular cities</p>
                  <PopularCityList
                    data={data.Popular}
                    selection={data.Selection}
                    onClick={this.handleCityClick}
                  />
                </div>
                <div className="city-list-content">
                  <p className="city-list__heading">Other cities</p>
                  {this.getOtherCityList()}
                </div>
              </div>
            )
          }
          <div className="popup__footer">
            <span className="popup-footer__next">Next</span>
          </div>
        </div>
      </div>
    );
  }
}

export default SelectCityPopup;
