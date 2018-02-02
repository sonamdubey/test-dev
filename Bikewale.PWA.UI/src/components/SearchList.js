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
				<div id={inputProps.id ? inputProps.id:''}>
                    <div class={inputProps.titleClass ? inputProps.titleClass:''}>{inputProps.title}</div>
                    <ul id={inputProps.ulId ? inputProps.ulId:''} class="recent-searches-dropdown bw-ui-menu">
                        // <ul id="global-recent-searches" style={{'position': 'relative','margin':'0','textAlign': 'left','height':'auto !important','background':'#fff'}} className="hide"></ul>
                    </ul>
                </div>
			)

    }
}

module.exports = GlobalSearchList