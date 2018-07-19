import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import RcCheckbox from 'rc-checkbox';

class Checkbox extends React.Component {
  static propTypes = {
    /** Align `checkbox` icon. */
    alignIcon: PropTypes.oneOf(['left', 'right']),
    /** If `true`, the checkbox will be checked. */
    checked: PropTypes.bool,
    /** The content of the checkbox. */
    children: PropTypes.node,
    /** A custom class for `checkbox` component. */
    className: PropTypes.string,
    /** The default value of `checkbox` element. */
    defaultChecked: PropTypes.bool,
    /** If `true`, the checkbox will be disabled. */
    disabled: PropTypes.bool,
    /** The `name` attribute of `checkbox` element. */
    name: PropTypes.string,
    /** The prefix for `checkbox` component. */
    prefixClass: PropTypes.string,
    /** Callback fired when checkbox value is changed. */
    onChange: PropTypes.func,
  };

  static defaultProps = {
    alignIcon: 'left',
    prefixClass: 'oxygen-checkbox',
  };

  handleChange = event => {
    const { onChange } = this.props;

    const value = event.target.checked;

    if (onChange) {
      onChange(value);
    }
  };

  render() {
    const {
      alignIcon,
      className,
      children,
      prefixClass,
      ...otherProps
    } = this.props;

    const labelClassName = classNames(`${prefixClass}-wrapper`, className);
    const contentClassName = `${prefixClass}-content`;

    const rcCheckbox = (
      <RcCheckbox
        {...otherProps}
        prefixCls={this.props.prefixClass}
        onChange={this.handleChange}
      />
    );

    return (
      <label className={labelClassName}>
        {alignIcon === 'left' && rcCheckbox}
        {children && <span className={contentClassName}>{children}</span>}
        {alignIcon === 'right' && rcCheckbox}
      </label>
    );
  }
}

export default Checkbox;
