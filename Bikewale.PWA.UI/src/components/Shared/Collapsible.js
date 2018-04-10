import React, { Component } from 'react';
import PropTypes from 'prop-types';

const propTypes = {
  transitionTime: PropTypes.number,
  easing: PropTypes.string,
  open: PropTypes.bool,
  classParentString: PropTypes.string,
  openedClassName: PropTypes.string,
  triggerStyle: PropTypes.object,
  triggerClassName: PropTypes.string,
  triggerOpenedClassName: PropTypes.string,
  contentOuterClassName: PropTypes.string,
  contentInnerClassName: PropTypes.string,
  accordionPosition: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  handleTriggerClick: PropTypes.func,
  onOpen: PropTypes.func,
  onClose: PropTypes.func,
  onOpening: PropTypes.func,
  onClosing: PropTypes.func,
  trigger: PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.element
  ]),
  triggerWhenOpen: PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.element
  ]),
  triggerDisabled: PropTypes.bool,
  lazyRender: PropTypes.bool,
  overflowWhenOpen: PropTypes.oneOf([
    'hidden',
    'visible',
    'auto',
    'scroll',
    'inherit',
    'initial',
    'unset'
  ]),
  triggerSibling: PropTypes.oneOfType([
    PropTypes.element,
    PropTypes.func,
  ])
}

const defaultProps = {
  transitionTime: 200,
  easing: 'linear',
  open: false,
  classParentString: 'collapsible',
  triggerDisabled: false,
  lazyRender: false,
  overflowWhenOpen: 'hidden',
  openedClassName: '',
  triggerStyle: null,
  triggerClassName: '',
  triggerOpenedClassName: '',
  contentOuterClassName: '',
  contentInnerClassName: '',
  className: '',
  triggerSibling: null,
  onOpen: null,
  onClose: null,
  onOpening: null,
  onClosing: null
}

class Collapsible extends Component {
  constructor(props) {
    super(props)

    // Bind class methods
    this.handleTriggerClick = this.handleTriggerClick.bind(this);
    this.handleTransitionEnd = this.handleTransitionEnd.bind(this);
    this.continueOpenCollapsible = this.continueOpenCollapsible.bind(this);
    this.setInnerRef = this.setInnerRef.bind(this);

    // Defaults the dropdown to be closed
    if (props.open) {
      this.state = {
        isClosed: false,
        shouldSwitchAutoOnNextCycle: false,
        height: 'auto',
        transition: 'none',
        hasBeenOpened: true,
        overflow: props.overflowWhenOpen,
        inTransition: false
      }
    } else {
      this.state = {
        isClosed: true,
        shouldSwitchAutoOnNextCycle: false,
        height: 0,
        transition: `height ${props.transitionTime}ms ${props.easing}`,
        hasBeenOpened: false,
        overflow: 'hidden',
        inTransition: false
      }
    }
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.state.shouldOpenOnNextCycle) {
      this.continueOpenCollapsible();
    }

    if (prevState.height === 'auto' && this.state.shouldSwitchAutoOnNextCycle === true) {
      window.setTimeout(() => { // Set small timeout to ensure a true re-render
        this.setState({
          height: 0,
          overflow: 'hidden',
          isClosed: true,
          shouldSwitchAutoOnNextCycle: false
        });
      }, 50);
    }

    // If there has been a change in the open prop (controlled by accordion)
    if (prevProps.open !== this.props.open) {
      if (this.props.open === true) {
        this.openCollapsible();
        if (this.props.onOpening) {
          this.props.onOpening();
        }
      } else {
        this.closeCollapsible();
        if (this.props.onClosing) {
          this.props.onClosing();
        }
      }
    }
  }

  closeCollapsible() {
    this.setState({
      shouldSwitchAutoOnNextCycle: true,
      height: this.collapsibleInner.offsetHeight,
      transition: `height ${this.props.transitionTime}ms ${this.props.easing}`,
      inTransition: true
    });
  }

  openCollapsible() {
    this.setState({
      inTransition: true,
      shouldOpenOnNextCycle: true
    });
  }

  continueOpenCollapsible() {
    this.setState({
      height: this.collapsibleInner.offsetHeight,
      transition: `height ${this.props.transitionTime}ms ${this.props.easing}`,
      isClosed: false,
      hasBeenOpened: true,
      inTransition: true,
      shouldOpenOnNextCycle: false
    });
  }

  handleTriggerClick(event) {
    event.preventDefault();

    if (this.props.triggerDisabled) {
      return;
    }

    if (this.props.handleTriggerClick) {
      this.props.handleTriggerClick(this.props.accordionPosition);
    }
    else {
      if (this.state.isClosed === true) {
        this.openCollapsible();

        if (this.props.onOpening) {
          this.props.onOpening();
        }
      }
      else {
        this.closeCollapsible();

        if (this.props.onClosing) {
          this.props.onClosing();
        }
      }
    }
  }

  renderNonClickableTriggerElement() {
    const {
      triggerSibling: TriggerSibling
    } = this.props

    if (TriggerSibling && typeof TriggerSibling === 'string') {
      return (
        <span className={`${this.props.classParentString}__trigger-sibling`}>
          {TriggerSibling}
        </span>
      )
    }
    else if (TriggerSibling) {
      return <TriggerSibling />
    }

    return null;
  }

  handleTransitionEnd() {
    // Switch to height auto to make the container responsive
    if (!this.state.isClosed) {
      this.setState({
        height: 'auto',
        overflow: this.props.overflowWhenOpen,
        inTransition: false
      });

      if (this.props.onOpen) {
        this.props.onOpen();
      }
    }
    else {
      this.setState({
        inTransition: false
      });

      if (this.props.onClose) {
        this.props.onClose();
      }
    }
  }

  setInnerRef(ref) {
    this.collapsibleInner = ref;
  }

  render() {
    var dropdownStyle = {
      height: this.state.height,
      WebkitTransition: this.state.transition,
      msTransition: this.state.transition,
      transition: this.state.transition,
      overflow: this.state.overflow
    }

    const {
      isClosed,
      hasBeenOpened,
      inTransition
    } = this.state

    var openClass = isClosed ? 'is-closed' : 'is-open';
    var disabledClass = this.props.triggerDisabled ? 'is-disabled' : '';

    //If user wants different text when tray is open
    var trigger = (isClosed === false) && (this.props.triggerWhenOpen !== undefined)
      ? this.props.triggerWhenOpen
      : this.props.trigger;

    // Don't render children until the first opening of the Collapsible if lazy rendering is enabled
    var children = this.props.lazyRender
      && !hasBeenOpened
      && isClosed
      && !inTransition ? null : this.props.children;

    // Construct CSS classes strings
    const triggerClassString = `${this.props.classParentString}__trigger ${openClass} ${disabledClass} ${
      isClosed ? this.props.triggerClassName : this.props.triggerOpenedClassName
      }`;
    const parentClassString = `${this.props.classParentString} ${
      isClosed ? this.props.className : this.props.openedClassName
      }`;
    const outerClassString = `${this.props.classParentString}__contentOuter ${this.props.contentOuterClassName}`;
    const innerClassString = `${this.props.classParentString}__contentInner ${this.props.contentInnerClassName}`;

    return (
      <div className={parentClassString.trim()}>
        <div
          className={triggerClassString.trim()}
          onClick={this.handleTriggerClick}
          style={this.props.triggerStyle && this.props.triggerStyle}
        >
          {trigger}
        </div>

        {
          this.props.triggerSibling
          ? this.renderNonClickableTriggerElement()
          : ''
        }

        <div
          className={outerClassString.trim()}
          style={dropdownStyle}
          onTransitionEnd={this.handleTransitionEnd}
        >
          <div
            className={innerClassString.trim()}
            ref={this.setInnerRef}
          >
            {children}
          </div>
        </div>
      </div>
    );
  }
}

Collapsible.propTypes = propTypes;
Collapsible.defaultProps = defaultProps;

export default Collapsible;
