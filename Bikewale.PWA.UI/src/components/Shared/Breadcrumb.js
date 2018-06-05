import React from 'react'
import {Link} from 'react-router-dom'

class Breadcrumb extends React.Component {
    renderLastItem(item, index) { 
        if(!item) return null;
        return(<li key={index}>
                <span className="breadcrumb-link__label">{item.Title}</span>
            </li>)

    }
    renderLinkableItem(item, index) {
    	if(!item) {
    		return null;
    	}

        if(item.isReactLink) {
        	return (<li key={index}>
				<Link to={item.Href} title={item.Title} className="breadcrumb-link">
					<span className="breadcrumb-link__label">{item.Title}</span>
				</Link>
			</li>)
		}
		return (<li key={index}>
				<a className="breadcrumb-link" href={item.Href} title={item.Title}>
					<span className="breadcrumb-link__label">{item.Title}</span>
				</a>
			</li>)
		}
    render() {
        if(!this.props.breadcrumb || this.props.breadcrumb.length == 0)
            return null;
        var length = this.props.breadcrumb.length;
        return (
            <section>
                <div className="breadcrumb">
                <span className="breadcrumb-title">You are here:</span>
                    <ul> 
                        {
                            this.props.breadcrumb.map(function(item,index){
                                if(index == length-1) {
                                    return this.renderLastItem(item, index);
                                }
                                else {
                                	return this.renderLinkableItem(item, index);
                                }
                            }.bind(this))
                        }
                    </ul>
                <div className="clear"></div></div><div className="clear"></div>
            </section>
        )
        
        
    }
}

export default Breadcrumb

