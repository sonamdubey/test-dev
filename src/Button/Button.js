import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';

import getSize from '../utils/Size';

class Button extends React.PureComponent {
  static propTypes = {
    /** Make button occupy width of its container. */
    block: PropTypes.bool,
    /** The content of the button. */
    children: PropTypes.node,
    /** A custom class for `button` component. */
    className: PropTypes.string,
    /** Disabled state of button. */
    disabled: PropTypes.bool,
    /** Invert button. */
    ghost: PropTypes.bool,
    /** Redirect url of link button. */
    href: PropTypes.string,
    /** Defines HTML button `type` attribute. */
    htmlType: PropTypes.oneOf(['button', 'reset', 'submit']),
    /** The prefix for component classes. */
    prefixClass: PropTypes.string,
    /** The size of the button. */
    size: PropTypes.oneOf(['small', 'default', 'large']),
    /** Target attribute for anchor. */
    target: PropTypes.string,
    /** Button style variant. */
    type: PropTypes.oneOf(['default', 'primary', 'secondary']),
    /** Callback fired when the button is clicked. */
    onClick: PropTypes.func,
  };

  static defaultProps = {
    htmlType: 'button',
    prefixClass: 'oxygen-btn',
    size: 'default',
    type: 'default',
  };

  render() {
    const {
      className,
      children,
      htmlType,
      type,
      size,
      prefixClass,
      ghost,
      block,
      ...otherProps
    } = this.props;

    const isDefault = type === 'default';
    const buttonSize = getSize(size);

    const buttonClass = classNames(prefixClass, className, {
      [`${prefixClass}-${type}`]: !isDefault && !ghost,
      [`${prefixClass}-outline-${type}`]: !isDefault && ghost,
      [`${prefixClass}-${buttonSize}`]: !isDefault,
      [`${prefixClass}-block`]: block,
    });

    if ('href' in otherProps) {
      return (
        <a {...otherProps} className={buttonClass}>
          {children}
        </a>
      );
    }

    return (
      <button {...otherProps} type={htmlType} className={buttonClass}>
        {children}
      </button>
    );
  }
}

export default Button;
