import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
  // data object
  item: PropTypes.object
}

const defaultProps = {
  showDescription: false
}

class ListItem extends React.PureComponent {
  constructor(props) {
    super(props)
  }

  render() {
    const { item, showDescription } = this.props
    return (
      <div>
        {
          item.icon
            ? <div className="item__icon-content">
                <div className="item-icon__image-content">
                  <img
                    className="item-icon-image"
                    src={item.icon}
                    alt={item.name}
                  />
                </div>
              </div>
            : ''
        }
        {
          item.name
            ? <p className="item__heading">{item.name}</p>
            : ''
        }
        {
          item.description && showDescription
            ? <div className="item__description">{item.description}</div>
            : ''
        }
      </div>
    )
  }
}


ListItem.propTypes = propTypes
ListItem.defaultProps = defaultProps

export default ListItem
