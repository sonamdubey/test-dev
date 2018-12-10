import React from 'react'
import PropTypes from 'prop-types'
import { throttle } from 'throttle-debounce'

import Autocomplete from './Autocomplete'

import autocomplete from '../utils/Autocomplete'

import {
  makeCancelable
} from '../utils/CancelablePromise'

import {
  AREA_AUTOCOMPLETE_URL
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
  // Callback function gets fired on area selection
  setArea: PropTypes.func,
  // Confirm button
  showConfirmBtn: PropTypes.bool,
}

const defaultProps = {
  id: 'getArea',
  className: 'area-field',
  placeholder: 'Type your area',
  setClearButton: true,
}

/**
 * This Class is a container component for the {@link Autocomplete} component
 * to be used in global location screen.
 * @class AreaAutocomplete
 * @extends {React.Component}
 */

 //The component is exported as unconnected component for custom handling
 export class AreaAutocomplete extends React.Component {
  constructor(props) {
    super(props)
    this.apiRequest = null
    this.state = {
      input: props.cityLocation.areaName.trim() === 'Select Area' ? '' : props.cityLocation.areaName,
      areaList: [],
      isFetching: false,
    }
    this.fetchAreas = throttle(500, this.fetchAreas);
  }

  componentWillReceiveProps = (nextProps) =>{
    if (this.props.cityLocation !== nextProps.cityLocation) {
      this.setState({
        input: nextProps.cityLocation.areaName.trim() === 'Select Area' ? '' : nextProps.cityLocation.areaName,
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

    this.fetchAreas(value)

    if(onChange) {
      onChange(event);
    }
  }

  /**
   * Method to make request for areas
   * and wrap the promise as cancelable
   * and set it in state.
   * @memberof AreaAutocomplete
   * @param {string} input User input value
   */
  fetchAreas = input => {
    if (input) {
      let apiRequest = makeCancelable(autocomplete({
        url: AREA_AUTOCOMPLETE_URL,
        source: 43,
        resultCount: 5,
        cityId: this.props.cityLocation.cityId
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
        areaList: list,
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
      areaList: [],
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
      setArea,
    } = this.props

    const {
      areaId,
      areaName,
      areaMaskingName,
    } = item.payload

    let areaSelection = {
      areaId,
      areaName,
      areaMaskingName,
    }

    this.setState({
      input: areaName,
    })

    setArea(areaSelection)
  }

  render() {
    const {
      areaList,
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
        list={areaList}
        isFetching={isFetching}
        onSuggestionClick={this.handleSuggestionClick}
        setFocus={!showConfirmBtn}
        setClearButton={setClearButton}
        noResultMessage="No area found!"
      />
    )
  }
}

AreaAutocomplete.propTypes = propTypes
AreaAutocomplete.defaultProps = defaultProps

export default AreaAutocomplete
