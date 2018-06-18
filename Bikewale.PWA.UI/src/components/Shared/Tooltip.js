import React from 'react'
import PropTypes from 'prop-types'

const propTypes = {
  // a custom class
  className: PropTypes.string,
  // visibility flag
  isActive: PropTypes.bool,
  // tooltip message to display
  message: PropTypes.string,
  // tooltip placement
  placement: PropTypes.string,
  // a custom object to pass style
  style: PropTypes.object,
  // arrow
  arrow: PropTypes.bool,
  // a custom function to call on tooltip open
  onOpen: PropTypes.func,
  // a custom function to call on tooltip close
  onClose: PropTypes.func
}

const defaultProps = {
  className: '',
  isActive: false,
  message: '',
  placement: 'top',
  style: {
    width: 260
  },
  arrow: true,
  onOpen: null,
  onClose: null
}

class Tooltip extends React.Component {
  constructor(props) {
    super(props)
    
    this.state = {
      isActive: this.props.isActive
    }

    this.handleClick = this.handleClick.bind(this);
    this.handleClose = this.handleClose.bind(this);
  }

  handleClick() {
    let closeButton = document.querySelectorAll('.tooltip-box__close')

    for(let i = 0; i < closeButton.length; i++) {
      closeButton[i].click();
    }

    this.setState({
      isActive: true
    })

    if (this.props.onOpen) {
      this.props.onOpen();
    }
  }

  handleClose() {
    this.setState({
      isActive: false
    })

    if (this.props.onClose) {
      this.props.onClose();
    }
  }

  render() {
    const {
      isActive
    } = this.state

    const {
      className,
      placement,
      style,
      arrow,
      message,
      children
    } = this.props

    const tooltipPlacement = "tooltip--" + placement

    return (
      <span className="tooltip__content">
        <span onClick={this.handleClick}>
          {
            children
          }
        </span>
        {
          isActive && (
            <div className={`tooltip-box ${className} ${tooltipPlacement}`} style={style}>
              <div className="tooltip-box__content">
                <span className="tooltip-box__close" onClick={this.handleClose}></span>
                <div className="tooltip-box__message">{message}</div>
              </div>
              {
                arrow && (
                  <span className="tooltip-box__arrow"></span>
                )
              }
            </div>
          )
        }
      </span>
    )
  }
}

Tooltip.propTypes = propTypes
Tooltip.defaultProps = defaultProps

export default Tooltip
