import React from 'react'
import PropTypes from 'prop-types'

import ListItem from './ListItem';
import { HOST_URL } from '../../utils/constants'

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
  }
  
  handleCityClick = (item) => {
    if(this.props.onClick) {
      this.props.onClick(item);
    }
  }

  getList = () => {
    const {
      data,
      selection
    } = this.props

    let listItems = data.map((item, index) => {
      let element = {
        name: item.cityName,
        icon: HOST_URL + '0x0/bw/static/icons/city/' + item.cityId + '.svg'
      }

      let activeClass = ''
      if (selection.cityId === item.cityId) {
        activeClass = `city--active ${selection.userChange ? "city--select" : "city--global"}`
      }

      return (
        <li
          key={item.id}
          className={"city-popular-list__item " + activeClass}
          onClick={this.handleCityClick.bind(this, item)}
        >
          <ListItem item={element} />
        </li>
      )
    });

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
