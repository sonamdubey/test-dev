import React from 'react'
import PropTypes from 'prop-types'
import classNames from 'classnames'

const propTypes = {
  // Custom class
  className: PropTypes.string,
  // Conditionally set cross icon
  closable: PropTypes.bool,
  // Text to show inside tag
  text: PropTypes.string.isRequired,
  // Callback function that gets fired on tag click
  handleClick: PropTypes.func,
}

function Tag(props) {
  const {
    className,
    closable,
    text,
    handleClick,
  } = props;

  const tagClassName = classNames('tag', className)

  return (
    <span
      className={tagClassName}
      onClick={handleClick}
    >
      {text}
      {closable && (
        <span className="tag-cross"></span>
      )}
    </span>
  )
}

Tag.propTypes = propTypes;

export default Tag
