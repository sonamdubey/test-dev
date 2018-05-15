import React from 'react'
import {Link} from 'react-router-dom'

class Breadcrumb extends React.Component {
    renderLastItem(item) { 
        if(!item) return null;
        return(<li>
                <span className="breadcrumb-link__label">{item.Title}</span>
            </li>)

    }
    renderLinkableItem(item) {
    	if(!item) {
    		return null;
    	}

        if(item.isReactLink) {
        	return (<li>
				<Link to={item.Href} title={item.Title} className="breadcrumb-link">
					<span className="breadcrumb-link__label">{item.Title}</span>
				</Link>
			</li>)
		}
		return (<li>
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
                                    return this.renderLastItem(item);
                                }
                                else {
                                	return this.renderLinkableItem(item);
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

