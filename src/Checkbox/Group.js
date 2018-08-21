import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import shallowEqual from 'shallowequal';

import Checkbox from './Checkbox';

class CheckboxGroup extends React.Component {
  static propTypes = {
    /** Align `checkbox` icon. */
    alignIcon: PropTypes.oneOf(['left', 'right']),
    /** A custom class for `CheckboxGroup` component. */
    className: PropTypes.string,
    /** Default selected value. */
    defaultValue: PropTypes.array,
    /** The `name` propery of all `input`. */
    name: PropTypes.string,
    /** Set children. */
    options: PropTypes.array,
    /** The prefix for `CheckboxGroup` component. */
    prefixClass: PropTypes.string,
    /** Type of checkbox. */
    type: PropTypes.oneOf(['default', 'pill']),
    /** Set currently selected value. */
    value: PropTypes.array,
    /** Callback fired when checkbox value is changed. */
    onChange: PropTypes.func,
  };

  static defaultProps = {
    prefixClass: 'oxygen-checkbox-group',
    type: 'default',
  };

  constructor(props) {
    super(props);

    this.state = {
      value: props.value || props.defaultValue || [],
    };
  }

  componentWillReceiveProps(nextProps) {
    if ('value' in nextProps) {
      this.setState({
        value: nextProps.value || [],
      });
    }
  }

  shouldComponentUpdate(nextProps, nextState) {
    // TODO: Shallow equal entire `nextProps` and `nextState`
    return (
      !shallowEqual(this.props.value, nextProps.value) ||
      !shallowEqual(this.state.value, nextState.value)
    );
  }

  getOptions = () => {
    const { options } = this.props;

    // TODO: add feature to set custom
    return options.map(option => {
      if (typeof option === 'string') {
        return {
          label: option,
          value: option,
        };
      }

      return option;
    });
  };

  toggleOption = option => {
    const { props, state } = this;

    const { onChange } = props;

    const optionIndex = state.value.indexOf(option.value);
    const value = [...state.value];

    if (optionIndex === -1) {
      value.push(option.value);
    } else {
      value.splice(optionIndex, 1);
    }
    if (!('value' in props)) {
      this.setState({
        value,
      });
    }

    if (onChange) {
      onChange(value);
    }
  };

  render() {
    const {
      className,
      name,
      type,
      options,
      prefixClass,
      alignIcon,
    } = this.props;

    const { value } = this.state;

    const groupPrefixClass = classNames({
      [`${prefixClass}`]: type === 'default',
    });
    const groupClassName = classNames(groupPrefixClass, className);

    let children;
    if (options && options.length) {
      children = this.getOptions().map(option => (
        <Checkbox
          key={option.value.toString()}
          checked={value.indexOf(option.value) !== -1}
          className={`${groupPrefixClass}-item`}
          name={name}
          value={option.value}
          onChange={this.toggleOption.bind(this, option)}
          alignIcon={alignIcon}
        >
          {option.label}
        </Checkbox>
      ));
    }

    return <div className={groupClassName}>{children}</div>;
  }
}

export default CheckboxGroup;
