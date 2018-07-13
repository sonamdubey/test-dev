import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import omit from 'omit.js';

function fixControlledValue(value) {
  if (typeof value === 'undefined' || value === null) {
    return '';
  }

  return value;
}

class Input extends React.Component {
  static propTypes = {
    /** A custom class for input container. */
    containerClassName: PropTypes.string,
    /** The default value of the `Input` element. */
    defaultValue: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    /** If `true`, the input will be disabled. */
    disabled: PropTypes.bool,
    /** The `id` of the input element. */
    id: PropTypes.string,
    /** A custom class for input. */
    inputClassName: PropTypes.string,
    /** A custom text for label. */
    label: PropTypes.string,
    /** A custom class for label. */
    labelClassName: PropTypes.string,
    /** Name attribute of the `input` element. */
    name: PropTypes.string,
    /** The short hint displayed in the input before the user enters a value. */
    placeholder: PropTypes.string,
    /** The prefix text/icon for the `Input`. */
    prefix: PropTypes.oneOfType([PropTypes.string, PropTypes.node]),
    /** The prefix for component classes. */
    prefixClass: PropTypes.string,
    /** Specify an input field as read-only. */
    readOnly: PropTypes.bool,
    /** Set field as required. */
    required: PropTypes.bool,
    /** The suffix text/icon for the `Input`. */
    suffix: PropTypes.oneOfType([PropTypes.string, PropTypes.node]),
    /** Define input type. */
    type: PropTypes.oneOf(['text', 'number', 'tel', 'email', 'password']),
    /** The value of the `input` element. */
    value: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
    /** Callback fired when input is blurred. */
    onBlur: PropTypes.func,
    /** Callback fired when the value is changed. */
    onChange: PropTypes.func,
    /** Callback fired when input is focused. */
    onFocus: PropTypes.func,
  };

  static defaultProps = {
    prefixClass: 'oxygen-input',
    type: 'text',
  };

  renderAffixInput(children) {
    const { props } = this;

    if (!('prefix' in props || 'suffix' in props)) {
      return children;
    }

    const prefix = props.prefix && (
      <div className={`${props.prefixClass}-prefix`}>{props.prefix}</div>
    );

    const suffix = props.suffix && (
      <div className={`${props.prefixClass}-suffix`}>{props.suffix}</div>
    );

    return (
      <div className={`${props.prefixClass}-affix-content`}>
        {prefix}
        {children}
        {suffix}
      </div>
    );
  }

  render() {
    const { id, label, value, prefixClass } = this.props;

    let { containerClassName, labelClassName, inputClassName } = this.props;

    containerClassName = classNames([`${prefixClass}`], containerClassName);
    labelClassName = classNames([`${prefixClass}-label`], labelClassName);
    inputClassName = classNames([`${prefixClass}-field`], inputClassName);

    // Fix https://fb.me/react-unknown-prop
    const inputProps = omit(this.props, [
      'containerClassName',
      'labelClassName',
      'inputClassName',
      'label',
      'prefix',
      'prefixClass',
      'suffix',
    ]);

    if ('value' in this.props) {
      inputProps.value = fixControlledValue(value);

      // Input elements must be either controlled or uncontrolled,
      // specify either the value prop, or the defaultValue prop, but not both.
      delete inputProps.defaultValue;
    }

    return (
      <div className={containerClassName}>
        {label && (
          <label htmlFor={id} className={labelClassName}>
            {label}
          </label>
        )}
        {this.renderAffixInput(
          <input {...inputProps} className={inputClassName} />,
        )}
      </div>
    );
  }
}

export default Input;
