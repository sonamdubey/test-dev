import React from 'react'
class GlobalSearchList extends React.Component {
    constructor(props) {
		super(props);
    }
    render() {
		
        var inputProps = {};
        if(this.props.inputProps) {
            inputProps = this.props.inputProps;
        }

        return(
				<div id={inputProps.id ? inputProps.id:''} style={{ 'display':this.props.items.length>0?'block':'none'}}>
                    <div className={inputProps.titleClass ? inputProps.titleClass:''}>{inputProps.title}</div>
                    <ul id={inputProps.ulId ? inputProps.ulId:''} className="recent-searches-dropdown bw-ui-menu">
                        {typeof this.props.renderRecentSearchList == 'function' ? this.props.renderRecentSearchList(this.props.items,this.props.value) : null}
                    </ul>
                </div>
			)

    }
}

module.exports = GlobalSearchList