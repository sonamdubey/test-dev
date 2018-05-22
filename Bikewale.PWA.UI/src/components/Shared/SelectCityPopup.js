import React from 'react';

import Autocomplete from '../Autocomplete';
import PopularCityList from './PopularCityList';
import ListGroup from './ListGroup';
import ListGroupItem from './ListGroupItem';
import NoResult from './NoResult';

import { unlockScroll } from '../../utils/scrollLock';
import { addPopupEvents, removePopupEvents } from '../../utils/popupScroll';

class SelectCityPopup extends React.Component {
  constructor(props) {
    super(props);
    this.handleCloseClick = this.handleCloseClick.bind(this);
    this.handleCityClick = this.handleCityClick.bind(this);
    let cityName = this.getCityName(this.props);
    this.state = {
      Popular: this.props.data.Popular,
      Other: this.props.data.Other,
      cityValue: cityName
    }
  }

  getCityName = (props) => {
    return props != null && props.data != null && props.data.Selection && props.data.isGlobalCityInList ? props.data.Selection.cityName : '';
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.data.Popular != this.state.Popular || nextProps.data.Other != this.state.Other) {
      let cityName = this.getCityName(nextProps);
      this.setState({
        Popular: nextProps.data.Popular,
        Other: nextProps.data.Other,
        cityValue: cityName
      });
    }
  }

  componentDidMount() {
    addPopupEvents(this.popupContent);
  }

  componentWillUnmount() {
    removePopupEvents(this.popupContent);
  }

  filterCityList = (event) => {
    var updatedPopular = this.props.data.Popular;
    var updatedOther = this.props.data.Other;
    updatedPopular = updatedPopular.filter(function (item) {
      return item.cityName.toLowerCase().search(
        event.target.value.toLowerCase()) !== -1;
    });
    updatedOther = updatedOther.filter(function (item) {
      return item.cityName.toLowerCase().search(
        event.target.value.toLowerCase()) !== -1;
    });
    this.setState({
      cityValue: event.currentTarget.value,
      Popular: updatedPopular,
      Other: updatedOther
    });
  }

  handleCityClick = (item) => {
    if (this.props.onCityClick) {
      this.props.onCityClick(item);
      this.setState(...this.state, {
        cityValue: item.cityName
      });
    }
    if (this.props.onCloseClick) {
      this.props.onCloseClick();
    }
  }

  handleCloseClick = () => {
    if (this.props.onCloseClick) {
      this.props.onCloseClick();
    }
    unlockScroll();
  }

  handleClearClick = () => {
    this.setState({
      cityValue: '',
      Popular: this.props.data.Popular,
      Other: this.props.data.Other
    });
  }


  getOtherCityList = () => {
    const {
      data,
      onClick
    } = this.props

    let listItems = this.state.Other.map((item, index) => {
      let active = data.Selection.cityId === item.cityId;

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
      <div className={popupClasses} id="selectcity-popup">
        <div ref={this.setContentRef} className="select-city-popup__content">
          <div className="popup__head">
            <div className="popup-head__content">
              <span onClick={this.handleCloseClick} className="popup__close"></span>
              <div className="popup__search-box">
                <p className="popup-search__title">Select City</p>
                <div className="autocomplete-box">
                  <div className="autocomplete-field">
                    <input type="text" value={this.state.cityValue} className="form-control" placeholder="Type to select city" id="selectcity-popup" onChange={this.filterCityList} />
                    {
                      data.Selection && data.Selection.cityId > 0
                        ? <span onClick={this.handleClearClick} className="autocomplete-box__clear">Clear</span>
                        : null // <span className="select-city__locate-me"></span> Future implementation
                    }
                  </div>
                </div>
              </div>
            </div>
          </div>
          {
            this.state.Popular.length > 0 || this.state.Other.length > 0 ? (
              <div className="select-city__body">
                {this.state.Popular.length > 0 &&
                  <div className="city-list-content">
                    <p className="city-list__heading">Popular cities</p>
                    <PopularCityList
                      data={this.state.Popular}
                      selection={data.Selection}
                      onClick={this.handleCityClick}
                    />
                  </div>}
                {this.state.Other.length > 0 &&
                  <div className="city-list-content">
                    <p className="city-list__heading">Other cities</p>
                    {this.getOtherCityList()}
                  </div>}
              </div>)
              : <NoResult
                type="select-bike__no-bike-content"
                imageClass="select-city__no-city"
                title="No Matching Cities Found"
              />
          }
        </div>
      </div>
    );
  }
}

export default SelectCityPopup;
