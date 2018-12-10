import React from "react";
import PropTypes from "prop-types";

import AutocompleteNoResult from "./AutocompleteNoResult";

const PropTypeReactComponent = PropTypes.oneOfType([
  PropTypes.func,
  PropTypes.string
]);

const propTypes = {
  // Input properties
  inputProps: PropTypes.object,
  // Input reference
  inputRef: PropTypes.func,
  // Input events
  inputEvents: PropTypes.object,
  // Suggestion list
  list: PropTypes.array,
  // Callback function fired on suggestion list item click
  onSuggestionClick: PropTypes.func,
  // Set focus to input field
  setFocus: PropTypes.bool,
  // Set clear button for input field
  setClearButton: PropTypes.bool,
  // Custom 'No result found' component
  noResultComponent: PropTypeReactComponent,
  // Display no result message
  noResultMessage: PropTypes.string
};

const defaultProps = {
  setClearButton: true,
  noResultComponent: AutocompleteNoResult
};

class Autocomplete extends React.Component {
  constructor(props) {
    super(props);

    this.blurTimer = null;

    this.state = {
      isListActive: false
    };
  }

  componentDidMount() {
    if (this.props.setFocus) {
      this.inputField.focus();
    }
  }

  componentWillUnmount() {
    clearTimeout(this.blurTimer);
  }

  componentWillReceiveProps(nextProps) {
    if (
      this.inputField === document.activeElement &&
      nextProps.inputProps.value &&
      nextProps.list
    ) {
      this.setState({
        isListActive: true
      });
    }
    if (nextProps.setFocus) {
      this.inputField.focus();
    }
  }

  handleInputFocus = () => {
    const { onFocus } = this.props.inputEvents;

    const { value } = this.props.inputProps;

    const { list } = this.props;

    if (value.length && list.length) {
      this.setState({
        isListActive: true
      });
    }

    if (onFocus) {
      onFocus();
    }
  };

  handleInputChange = event => {
    const { onChange } = this.props.inputEvents;

    const value = event.target.value;

    this.setState({
      isListActive: !!value.length && this.props.list.length
    });

    if (onChange) {
      onChange(event);
    }
  };

  handleInputKeyDown = event => {
    const { onKeyDown } = this.props.inputEvents;

    if (onKeyDown) {
      onKeyDown(event);
    }
  };

  handleInputBlur = () => {
    const { onBlur } = this.props.inputEvents;

    this.blurTimer = setTimeout(() => {
      this.setState({
        isListActive: false
      });
    }, 100);

    if (onBlur) {
      onBlur();
    }
  };

  handleInputClear = () => {
    const { onClear } = this.props.inputEvents;

    this.setState({
      isListActive: false
    });

    if (onClear) {
      onClear(this.inputField);
    }
  };

  handleSuggestionMouseDown = (item) => {
    const { onSuggestionClick } = this.props;

    /*
     * According to event order `onBlur` event gets fired before `onClick`,
     * `onClick` callback function was not getting fired on input blur event.
     * Trigger click event with onMouseDown event.
     * https: //stackoverflow.com/questions/17769005/onclick-and-onblur-ordering-issue
     */
    if (onSuggestionClick) {
      onSuggestionClick(item)
    }
  }

	onClick = () => {
		const {
			onClick,
		} = this.props.inputEvents

		if (onClick) {
			onClick()
		}
	}

setReference = ref => {
    const { inputRef } = this.props;
    this.inputField = inputRef ? inputRef(ref) : ref;
  };

  renderSuggestionList = () => {
    const { list } = this.props;

    const styleList = this.state.isListActive ? "show" : "hide";

    if (list) {
      return (
        <ul className={"autocomplete-list " + styleList}>
          {this.suggestionListItem()}
        </ul>
      );
    }
  };

  suggestionListItem = () => {
    const { list, isFetching } = this.props;

    if (list.length) {
      let listElement = list.map((item, index) => {
        return (
          <li key={index} className="autocomplete-list-item">
            <div
              className="autocomplete-list-item__label"
              onMouseDown={this.handleSuggestionMouseDown.bind(this, item)}
            >
              {this.highlighter(item.result)}
            </div>
          </li>
        );
      });

      return listElement;
    } else if (!isFetching) {
      const { noResultComponent: NoResult, noResultMessage } = this.props;

      return <NoResult message={noResultMessage} />;
    }
  };

  highlighter = result => {
    const { value } = this.props.inputProps;

    let escapedPhrase = value.replace(
      /[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g,
      "\\$&"
    );
    let tokens = result.split(new RegExp("(" + escapedPhrase + ")", "ig"));
    const regx = new RegExp(escapedPhrase, "i");
    let final = tokens.reduce((acc, token, index) => {
      return acc.concat(
        regx.test(token) ? (
          <strong key={index}>{token}</strong>
        ) : (
          <span key={index}>{token}</span>
        )
      );
    }, []);

    return final;
  };

  render() {
    const { inputProps, setClearButton } = this.props;

    const styleClearTarget = inputProps.value.length ? "show" : "hide";

    return (
      <div className="autocomplete-box">
        <div className="autocomplete-field">
          <input
            type="text"
            ref={this.setReference}
            id={inputProps.id ? inputProps.id : ""}
            placeholder={inputProps.placeholder ? inputProps.placeholder : ""}
            className={inputProps.className ? inputProps.className : ""}
            value={inputProps.value}
            onChange={this.handleInputChange}
            onFocus={this.handleInputFocus}
            onBlur={this.handleInputBlur}
            onKeyDown={this.handleInputKeyDown}
            onClick={this.onClick}
            autoComplete="off"
          />
          {setClearButton && (
            <span
              className={"clear-field-target " + styleClearTarget}
              onClick={this.handleInputClear}
            >
              Clear
            </span>
          )}
        </div>
        {this.renderSuggestionList()}
      </div>
    );
  }
}

Autocomplete.defaultProps = defaultProps;
Autocomplete.propTypes = propTypes;

export default Autocomplete;
