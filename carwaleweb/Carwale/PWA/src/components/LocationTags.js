import React from "react";
import PropTypes from "prop-types";
import classNames from "classnames";

import Form from "oxygen/lib/Form";
import Tag from "./Tag";
import TagsInput from "./TagsInput";
import CityAutocomplete from "./CityAutocomplete";
import AreaAutocomplete from "./AreaAutocomplete";
import { fireInteractiveTracking } from '../utils/Analytics';

const FormItem = Form.Item;

const propTypes = {
  // Pass object with city details
  cityLocation: PropTypes.object,
  // Callback function fired on city selection
  setCity: PropTypes.func,
  // Callback function fired on area selection
  setArea: PropTypes.func,
  // Callback function fired on city removal
  onCityRemove: PropTypes.func,
  // Callback function fired on area removal
  onAreaRemove: PropTypes.func,
  // Set clear button for input field
  setClearButton: PropTypes.bool
};

class LocationTags extends React.Component {
  constructor(props) {
    super(props);

    const { cityLocation } = this.props;
    let selectionTags = this.getTags(cityLocation);

    this.state = {
      tags: selectionTags,
      inputValue: "",
      focus: false
    };
  }

  getTags = cityLocation => {
    const { citiesWithArea } = this.props;
    let selectionTags = [];
    if (cityLocation.cityId > -1) {
      selectionTags.push(cityLocation.cityName);

      if (citiesWithArea.indexOf(cityLocation.cityId) > -1 && cityLocation.areaId > -1) {
        selectionTags.push(cityLocation.areaName);
      }
    }
    return selectionTags;
  };

  pushTags = cityLocation => {
    let selectionTags = this.getTags(cityLocation);
    if (selectionTags.length) {
      this.setState({
        tags: selectionTags,
        inputValue: ""
      });
    }
  };

  componentWillReceiveProps(nextProps) {
    this.pushTags(nextProps.cityLocation);
    if (
      nextProps.citiesWithArea.indexOf(nextProps.cityLocation.cityId) < 0 &&
      nextProps.cityLocation.cityId > -1
    ) {
      this.handleInputBlur();
    }
  }

  handleChange = (tags, changed, index) => {
    const { onCityRemove, onAreaRemove } = this.props;

    const removedIndex = index[0];
    let newTags = this.state.tags.slice(0, removedIndex);

    if (removedIndex === 0 && onCityRemove) {
      onCityRemove();
    }
    if (removedIndex === 1 && onAreaRemove) {
      onAreaRemove();
    }

    this.setState({
      tags: newTags,
      inputValue: ""
    });
  };

  handleOnChangeInput = tag => {
    this.setState({
      inputValue: tag
    });
  };

  handleCitySelection = city => {
    const { setCity } = this.props;
    this.setState({
      tags: [city.cityName],
      inputValue: ""
    });
    setCity(city);
  };

  handleAreaSelection = area => {
    const { setArea } = this.props;
    this.setState({
      tags: this.state.tags.concat(area.areaName),
      inputValue: "",
      focus: false
    });
    setArea(area);
  };
  handleCityInputClick = () => {
    fireInteractiveTracking("GlobalCityPopUp", "Tap_City_Box", "");
  }

  handleAreaInputClick = () => {
    let cityName = this.state.tags[0];
    fireInteractiveTracking("GlobalCityPopUp", "Tap_Area_Box", cityName);
  }

  handleInputFocus = () => {
    this.setState({
      focus: true
    });
  };

  handleInputBlur = () => {
    this.setState({
      focus: false
    });
  };

  renderLayout = (tagComponents, inputComponent) => {
    return (
      <div className="tags-input-control__layout">
        {tagComponents}
        {inputComponent}
      </div>
    );
  };

  renderTag = props => {
    let { tag, key, onRemove, getTagDisplayValue } = props;
    return (
      <Tag
        key={key}
        text={getTagDisplayValue(tag)}
        handleClick={onRemove.bind(this, key)}
        closable
      />
    );
  };

  renderInput = props => {
    let { onChange, value, addTag, ref, ...other } = props;
    const { cityLocation, citiesWithArea, setClearButton } = this.props;
    if (cityLocation.areaId > -1) {
      return null;
    }

    if (
      citiesWithArea.indexOf(cityLocation.cityId) < 0 &&
      cityLocation.cityId > -1
    ) {
      return null;
    }

    if (cityLocation.cityId > -1) {
      if (citiesWithArea.indexOf(cityLocation.cityId) > -1) {
        return (
          <AreaAutocomplete
            {...other}
            inputRef={ref}
            onChange={onChange}
            cityLocation={cityLocation}
            setArea={this.handleAreaSelection}
            setClearButton={setClearButton}
            onClick={this.handleAreaInputClick}
            onFocus={this.handleInputFocus}
            onBlur={this.handleInputBlur}
            showConfirmBtn={false} // TODO: Create a new property to handle Autocomplete focus
          />
        );
      }

      return null;
    } else {
      return (
        <CityAutocomplete
          {...other}
          inputRef={ref}
          onChange={onChange}
          cityLocation={cityLocation}
          setCity={this.handleCitySelection}
          setClearButton={setClearButton}
          onClick={this.handleCityInputClick}
          onFocus={this.handleInputFocus}
          onBlur={this.handleInputBlur}
        />
      );
    }
  };
  render() {
    const { tags, inputValue, focus } = this.state;
    const { validateStatus, helperText } = this.props;
    const tagsInputClassName = classNames("tags-input-control", {
      "has-focus": focus
    });
    return (
      <Form className="location-tags-form">
        <FormItem validateStatus={validateStatus} helperText={helperText}>
          <TagsInput
            className={tagsInputClassName}
            value={tags}
            inputValue={inputValue}
            onChange={this.handleChange}
            onChangeInput={this.handleOnChangeInput}
            renderLayout={this.renderLayout}
            renderInput={this.renderInput}
            citiesWithArea={this.citiesWithArea}
            renderTag={this.renderTag}
            maxTags={2}
            addKeys={[]}
          />
        </FormItem>
      </Form>
    );
  }
}

LocationTags.propTypes = propTypes;

export default LocationTags;
