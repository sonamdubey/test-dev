import React from "react";
import PropTypes from "prop-types";
import classNames from "classnames";

class CityDropdown extends React.Component {
  static propTypes = {
    /** A custom `class` for select container. */
    containerClassName: PropTypes.string,
    /** If `true`, the select dropdown will be disabled. */
    disabled: PropTypes.bool,
    /** A custom id for select element. */
    id: PropTypes.string,
    /** A custom text for label. */
    label: PropTypes.string,
    /** A custom `class` for label. */
    labelClassName: PropTypes.string,
    /** Options to select from. */
    options: PropTypes.array,
    /** A custom property name to access label from `options` prop. */
    optionLabel: PropTypes.string,
    /** A custom property name to access value from `options` prop. */
    optionValue: PropTypes.string,
    /** The short hint displayed in the select before the user selects a value. */
    placeholder: PropTypes.string,
    /** A prefix for `Select` component classes. */
    prefixClass: PropTypes.string,
    /** If `true`, the select dropdown will be marked as required. */
    required: PropTypes.bool,
    /** A custom `class` for select element. */
    selectClassName: PropTypes.string,
    /** The value of the `select` element. */
    value: PropTypes.oneOf([PropTypes.number, PropTypes.string]),
    /** Callback function fired on value change. */
    onChange: PropTypes.func
  };

  static defaultProps = {
    placeholder: "Select option",
    prefixClass: "oxygen-select",
    optionLabel: "label",
    optionValue: "value"
  };

  constructor(props) {
    super(props);

    this.state = {
      value: props.value || -1
    };
  }

  getOptions = () => {
    const { options, optionLabel, optionValue } = this.props;

    const newOptions = options.map(option => ({
      label: option[optionLabel],
      value: option[optionValue]
    }));

    return newOptions;
  };

  renderPopularCities = () => {
    const { optionLabel } = this.props;
    let mumbaiOptions = ["40", "13", "395", "6", "8"];
    let delhiOptions = ["246", "224", "225", "273"];
    let popularCityOrder = [
      "10",
      "1",
      "2",
      "12",
      "105",
      "176",
      "128",
      "198",
      "244",
      "160",
      "220"
    ];
    let popularCities = [];
    let otherCityGroup = [];
    popularCityOrder.forEach(
      function(city) {
        let cityObject = this.getCity(city);
        if (cityObject != null) {
          if (cityObject[optionLabel] == "New Delhi") {
            popularCities.push(
              <optgroup key="-2" label="Delhi NCR">
                <option key="10" value="10">
                  Delhi
                </option>
                {this.getAvailableCities(delhiOptions)}
              </optgroup>
            );
          } else if (cityObject[optionLabel] == "Mumbai") {
            popularCities.push(
              <optgroup key="-3" label="Mumbai">
                <option key="1" value="1">
                  Mumbai
                </option>
                {this.getAvailableCities(mumbaiOptions)}
              </optgroup>
            );
          } else {
            otherCityGroup.push(
              <option key={city} value={city}>
                {cityObject[optionLabel]}
              </option>
            );
          }
        }
      }.bind(this)
    );
    if (otherCityGroup.length > 0) {
      popularCities.push(
        <optgroup key="-4" label="Other Popular Cities">
          {otherCityGroup}
        </optgroup>
      );
    }
    if (popularCities.length > 0) {
      popularCities.push(
        <option key="-5" value="-5" disabled>
          -----------------------
        </option>
      );
    }

    return popularCities;
  };

  getAvailableCities(allCities) {
    const { optionLabel } = this.props;
    let availableCities = [];

    allCities.forEach(cityValue => {
      let cityObject = this.getCity(cityValue);
      if (cityObject != null) {
        availableCities.push(
          <option key={cityValue} value={cityValue}>
            {cityObject[optionLabel]}
          </option>
        );
      }
    });
    return availableCities;
  }

  getCity(cityValue) {
    const { options, optionValue } = this.props;
    let option;
    for (option of options) {
      if (cityValue == option[optionValue]) {
        return option;
      }
    }
    return null;
  }

  handleChange = event => {
    const { onChange, optionLabel, optionValue } = this.props;

    const select = event.target;

    const selectedOption = {};

    selectedOption[optionValue] = select.value;
    selectedOption[optionLabel] = select.selectedOptions[0].innerHTML;

    this.setState({
      value: select.value
    });

    if (onChange) {
      onChange(selectedOption);
    }
  };

  renderPlaceholder = () => {
    const { placeholder } = this.props;

    if (placeholder) {
      return (
        <option key="-1" value="-1" disabled>
          {placeholder}
        </option>
      );
    }

    return null;
  };

  renderLabel = () => {
    const { label } = this.props;

    if (label) {
      const { id, prefixClass } = this.props;

      let { labelClassName } = this.props;

      labelClassName = classNames(`${prefixClass}-label`, labelClassName);

      return (
        <label htmlFor={id} className={labelClassName}>
          {label}
        </label>
      );
    }

    return null;
  };

  renderOptions = () => {
    const options = this.getOptions().map(option => (
      <option key={option.value.toString()} value={option.value}>
        {option.label}
      </option>
    ));

    return options;
  };

  renderSelect = () => {
    const { value } = this.state;

    const { id, disabled, options, prefixClass, required } = this.props;

    let { selectClassName } = this.props;

    if (options && options.length) {
      selectClassName = classNames(`${prefixClass}-field`, selectClassName);

      return (
        <div className={`${prefixClass}-content`}>
          <select
            id={id}
            className={selectClassName}
            onChange={this.handleChange}
            value={value}
            disabled={disabled}
            required={required}
          >
            {this.renderPlaceholder()}
            {this.renderPopularCities()}
            {this.renderOptions()}
          </select>
          <span className={`${prefixClass}-arrow-separator`} />
        </div>
      );
    }

    return null;
  };

  render() {
    const { prefixClass } = this.props;
    let { containerClassName } = this.props;

    containerClassName = classNames(`${prefixClass}`, containerClassName);

    return (
      <div className={containerClassName}>
        {this.renderLabel()}
        {this.renderSelect()}
      </div>
    );
  }
}

export default CityDropdown;
