import React from 'react'
class Popup extends React.Component {
  render() {
    return (
      <div className='toast-popup'>
        <div className='toast-popup_inner'>
            {/*<span onClick={this.props.closePopup} className="toast-close">close</span>*/}
            <p>{this.props.text}</p>
        </div>
      </div>
    );
  }
}

export default class CreateToastmsg extends React.Component {
  constructor() {
    super();
    this.state = {
        showPopup: false,
        text: ""
    };
    this.handleClick = this.handleClick.bind(this);
    this.handleOutsideClick = this.handleOutsideClick.bind(this);
  }

  handleClick() {
    if (!this.state.showPopup) {
      document.addEventListener('click', this.handleOutsideClick, false);
    } else {
      document.removeEventListener('click', this.handleOutsideClick, false);
    }

    this.setState(prevState => ({
       showPopup: !prevState.showPopup,
    }));
  }
  handleOutsideClick(e) {
    if (this.node.contains(e.target)) {
      return;
    }
    this.handleClick();
  }
  render() {
    return (
      <span className='app' ref={node => { this.node = node; }}>
        <span onClick={this.handleClick} className="info-icon"></span>
        {this.state.showPopup ?
          <Popup
            text={this.props.name}
            /*closePopup={this.handleClick}*/
          />
          : null
        }
      </span>
    );
  }
}
