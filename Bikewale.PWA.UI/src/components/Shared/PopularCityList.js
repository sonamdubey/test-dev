import React from 'react'
import PropTypes from 'prop-types'

import ListItem from './ListItem';

const propTypes = {
  // list type
  type: PropTypes.string
}

const defaultProps = {
  type: ''
}

class PopularCityList extends React.Component {
  constructor(props) {
    super(props);

    this.getList = this.getList.bind(this);
  }

  getList() {
    const {
      data,
      selection,
      onClick
    } = this.props

    let listItems = data.map(function (item, index) {
      let element = {
        name: item.cityName,
        icon: item.icon
      }

      let activeClass
      if (selection.cityId === item.cityId) {
        activeClass = `city--active ${selection.userChange ? "city--select" : "city--global"}`
      }

      return (
        <li
          key={item.id}
          className={"city-popular-list__item " + activeClass}
          onClick={onClick.bind(this, item)}
        >
          <ListItem item={element} />
        </li>
      )
    }, this);

    return listItems;
  }

  render() {
    return (
      <ul className={"city-popular__list " + this.props.type}>
        {this.getList()}
      </ul>
    )
  }
}

PopularCityList.propTypes = propTypes
PopularCityList.defaultProps = defaultProps

export default PopularCityList
