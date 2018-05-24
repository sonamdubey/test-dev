/*
 * https://github.com/glennflanagan/react-responsive-accordion
 * - Modify class name
 */

import React from 'react';
import PropTypes from 'prop-types';
import Collapsible from './Collapsible';

const propTypes = {
  transitionTime: PropTypes.number,
  easing: PropTypes.string,
  startPosition: PropTypes.number,
  classParentString: PropTypes.string,
  onTriggerClick: PropTypes.func,
  closeable: PropTypes.bool,
  allOpen: PropTypes.bool,
  items: PropTypes.arrayOf(PropTypes.shape({
    props: PropTypes.shape({
      'data-trigger': PropTypes.oneOfType([
        PropTypes.string,
        PropTypes.element
      ]).isRequired,
      'data-triggerWhenOpen': PropTypes.oneOfType([
        PropTypes.string,
        PropTypes.element
      ]),
      'data-triggerDisabled': PropTypes.bool
    })
  }))
}

const defaultProps = {
  transitionTime: 200,
  easing: 'linear',
  startPosition: -1,
  classParentString: 'collapsible',
  onTriggerClick: null,
  closeable: false,
  allOpen: false
}

class Accordion extends React.Component {
  constructor(props) {
    super(props)

    this.state = {
      openPosition: this.props.startPosition
    }

    this.handleTriggerClick = this.handleTriggerClick.bind(this);
  }

  handleTriggerClick(position, event) {
    let closeAll = false;

    if (this.props.closeable) {
      closeAll = (!this.state.closeAll && position === this.state.openPosition);
    }

    this.setState({
      openPosition: position,
      closeAll: closeAll
    });

    if (this.props.onTriggerClick) {
      this.props.onTriggerClick(position, event);
    }
  }

  render() {
    if (!this.props.items) {
      return null;
    }
    var nodes = this.props.items.map((node, index) => {

      var triggerWhenOpen = (node.props['data-trigger-when-open']) ? node.props['data-trigger-when-open'] : node.props['data-trigger'];
      var triggerDisabled = (node.props['data-trigger-disabled']) || false;
      var triggerOnOpen = (node.props['data-onOpen']) ? node.props['data-onOpen'] : null;

      return (
        <Collapsible
          key={"collapsible" + index}
          handleTriggerClick={this.handleTriggerClick}
          open={(!this.state.closeAll && this.state.openPosition === index) || this.props.allOpen}
          onOpen={triggerOnOpen}
          trigger={node.props['data-trigger']}
          triggerWhenOpen={triggerWhenOpen}
          triggerDisabled={triggerDisabled}
          transitionTime={this.props.transitionTime}
          transitionCloseTime={this.props.transitionCloseTime}
          easing={this.props.easing}
          classParentString={this.props.classParentString}
          accordionPosition={index}
        >
          {node}
        </Collapsible>
      );
    });

    return (
      <div>
        {nodes}
      </div>
    );
  }
}

Accordion.propTypes = propTypes;
Accordion.defaultProps = defaultProps;

export default Accordion;
