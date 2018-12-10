import React from 'react'
import PropTypes from 'prop-types'
import { throttle } from 'throttle-debounce'

import Autocomplete from '../components/Autocomplete'

import autocomplete from '../utils/Autocomplete'

import {
  makeCancelable
} from '../utils/CancelablePromise'

import {
  CITY_AUTOCOMPLETE_URL
} from '../constants'

const propTypes = {
  // Input field id
  id: PropTypes.string,
  // Input field class
  className: PropTypes.string,
  // Input field placeholder
  placeholder: PropTypes.string,
  // Object with city details
  cityLocation: PropTypes.object,
  // Callback function gets fired on input change
  onChange: PropTypes.func,
  // Set clear button for input field
  setClearButton: PropTypes.bool,
  // Callback function gets fired on input clear
  onClear: PropTypes.func,
  // Callback function gets fired on clear button click
  clearSelection: PropTypes.func,
  // Callback function gets fired on city selection
  setCity: PropTypes.func,
  // Confirm button
  showConfirmBtn: PropTypes.bool,
  // Is Location Popup active
  isActive:PropTypes.bool
}

const defaultProps = {
  id: 'getCity',
  className: 'city-field',
  placeholder: 'Type your city',
  setClearButton: true,
  isActive: true
}

/**
 * This Class is a container component for the {@link Autocomplete} component
 * to be used in location screen.
 * @class CityAutocomplete
 * @extends {React.Component}
 */

 //The component is exported as unconnected component for custom handling
 export class CityAutocomplete extends React.Component {
  constructor(props) {
    super(props)
    this.apiRequest = null
    this.state = {
      input: props.cityLocation.cityName.trim() === 'Select City' ? '' : props.cityLocation.cityName,
      cityList: [],
      isFetching: false,
    }
    this.fetchCities = throttle(500, this.fetchCities);
  }

  componentWillReceiveProps = (nextProps) =>{
    if (this.props.cityLocation !== nextProps.cityLocation || !nextProps.isActive) {
      this.setState({
        input: nextProps.cityLocation.cityName.trim() === 'Select City' ? '' : nextProps.cityLocation.cityName,
      })
    }
  }

  componentWillUnmount() {
    this.apiRequest && this.apiRequest.cancel()
  }

  onChange = event => {
    const {
      onChange,
    } = this.props;

    const value = event.target.value

    this.setState({
      input: value,
      isFetching: true,
    })

    this.fetchCities(value)

    if(onChange) {
      onChange(event);
    }
  }

  /**
   * Method to make request for cities
   * and wrap the promise as cancelable
   * and set it in state.
   * @memberof CityAutocomplete
   * @param {string} input User input value
   */
  fetchCities = input => {
    if (input) {
      let apiRequest = makeCancelable(autocomplete({
        url: CITY_AUTOCOMPLETE_URL,
        source: 43,
        resultCount: 5
      }, input))
      apiRequest
        .promise
        .then((json) => this.afterFetch(json))
        .catch(({isCanceled, ...error}) => console.log('isCanceled', isCanceled));
      this.apiRequest = apiRequest
    }
  }

  afterFetch = (list) => {
    if(this.state.input && list){
      this.setState({
        cityList: list,
        isFetching: false,
      })
    }
  }

  onClear = (inputField) => {
    const {
      clearSelection,
      onClear,
    } = this.props;

    this.setState({
      input: '',
      cityList: [],
    }, inputField.focus())

    if (clearSelection) {
      clearSelection()
    }

    if (onClear) {
      onClear()
    }
  }

  handleSuggestionClick = (item) => {
    const {
      setCity,
    } = this.props

    const {
      cityId,
      cityName,
      cityMaskingName,
    } = item.payload

    let citySelection = {
      cityId,
      cityName,
      cityMaskingName,
      isConfirmBtnClicked: true
    }

    this.setState({
      input: cityName,
    })
    setCity(citySelection)
  }

  render() {
    const {
      cityList,
      input,
      isFetching,
    } = this.state;

    const {
      inputRef,
      setClearButton,
      onKeyDown,
      showConfirmBtn,
      onFocus,
      onBlur,
    } = this.props

    const inputProps = {
      ...defaultProps,
      value: input,
    }

    const inputEvents = {
      onChange: this.onChange,
      onClear: this.onClear,
      onKeyDown: onKeyDown,
      onClick: this.props.onClick,
      onFocus: onFocus,
      onBlur: onBlur,
    }

    return (
      <Autocomplete
        inputRef={inputRef}
        inputProps={inputProps}
        inputEvents={inputEvents}
        list={cityList}
        isFetching={isFetching}
        onSuggestionClick={this.handleSuggestionClick}
        setFocus={!showConfirmBtn}
        setClearButton={setClearButton}
        noResultMessage="No city found!"
      />
    )
  }
}

CityAutocomplete.propTypes = propTypes
CityAutocomplete.defaultProps = defaultProps

export default CityAutocomplete
