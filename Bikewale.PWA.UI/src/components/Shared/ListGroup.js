import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
  // list type
  type: PropTypes.string
}

const defaultProps = {
  type: ''
}

class ListGroup extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <ul className={"list__group " + this.props.type}>
        {this.props.children}
      </ul>
    )
  }
}

ListGroup.propTypes = propTypes
ListGroup.defaultProps = defaultProps

export default ListGroup
