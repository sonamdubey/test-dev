import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';

import FormItem from './FormItem';

class Form extends React.Component {
  static Item = FormItem;

  static propTypes = {
    /** A custom class for `form` element. */
    className: PropTypes.string,
    /** Callback fired when form is submitted. */
    onSubmit: PropTypes.func,
    /** A prefix for `Form` component classes. */
    prefixClass: PropTypes.string,
  };

  static defaultProps = {
    prefixClass: 'oxygen-form',
  };

  render() {
    const { className, prefixClass, ...otherProps } = this.props;

    const formClassName = classNames(prefixClass, className);

    return <form {...otherProps} className={formClassName} />;
  }
}

export default Form;
