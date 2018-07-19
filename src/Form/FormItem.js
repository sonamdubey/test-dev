import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';

class FormItem extends React.Component {
  static propTypes = {
    /** The content of the form item. */
    children: PropTypes.node,
    /** The helper message. */
    helperText: PropTypes.string,
    /** A prefix for `FormItem` component classes. */
    prefixClass: PropTypes.string,
    /** The validation status. */
    validateStatus: PropTypes.oneOf(['error', '']),
  };

  static defaultProps = {
    prefixClass: 'oxygen-form-item',
  };

  renderWrapper(child1, child2) {
    const { prefixClass, validateStatus } = this.props;

    let wrapperClassName = `${prefixClass}-control`;

    if (validateStatus) {
      wrapperClassName = classNames(wrapperClassName, {
        'has-error': validateStatus === 'error',
      });
    }

    return (
      <div className={wrapperClassName}>
        {child1}
        {child2}
      </div>
    );
  }

  renderHelper() {
    const { helperText, prefixClass } = this.props;

    if (helperText) {
      const helperTexterClassName = `${prefixClass}-help`;

      return <div className={helperTexterClassName}>{helperText}</div>;
    }

    return null;
  }

  render() {
    const { children, helperText, prefixClass } = this.props;

    let itemClassName = `${prefixClass}`;

    if (helperText) {
      itemClassName = classNames(itemClassName, [`${prefixClass}-with-help`]);
    }

    return (
      <div className={itemClassName}>
        {this.renderWrapper(children, this.renderHelper())}
      </div>
    );
  }
}

export default FormItem;
