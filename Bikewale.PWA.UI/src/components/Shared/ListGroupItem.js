import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
  // element id
  id: PropTypes.number,
  // element name
  name: PropTypes.string,
  // element active-inactive status
  active: PropTypes.bool,
  // custom click handler
  onClick: PropTypes.func
}

const defaultProps = {
  id: null,
  name: null,
  active: false
}

class ListGroupItem extends React.PureComponent {
  constructor(props) {
    super(props);

    this.handleClick = this.handleClick.bind(this);
  }

  handleClick() {
    if (this.props.onClick) {
      this.props.onClick();
    }
  }

  render() {
    let activeClass = this.props.active ? "list-group-item--active" : "";

    return (
      <li
        key={this.props.id}
        className={"list-group__item " + activeClass}
        onClick={this.handleClick}
      >
        <p className="list-group-item__title">{this.props.name}</p>
      </li>
    )
  }
}


ListGroupItem.propTypes = propTypes
ListGroupItem.defaultProps = defaultProps

export default ListGroupItem
