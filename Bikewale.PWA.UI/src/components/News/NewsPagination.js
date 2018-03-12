import React from 'react'
import {Link} from 'react-router-dom'

var getDisplayPageNos = function(articleCount,articlePerPage,pageNo,maxNoOfPages) {
   
    
    if(maxNoOfPages <= 5) {
        var displayPageNos = [];
        for(var i=1 ; i <= maxNoOfPages ; i++)
            displayPageNos.push(i);
        return displayPageNos;
    }

    pageNo = parseInt(pageNo);
    if(pageNo <= 3) {
        return [1,2,3,4,5];
    }

    var displayPageNos = [(pageNo-2),(pageNo-1),pageNo];
    for(var i = pageNo+1 ; (i <= pageNo+2) && (i <= maxNoOfPages) ; i++)
        displayPageNos.push(i);
    return displayPageNos;

}

class NewsPagination extends React.Component {
    propTypes: {
        'articleCount' : React.PropTypes.number
    };
    onClickLink(pageNo) {
      
        this.props.updateArticleList(pageNo);
    }
    getClassNameForPaginationArrow(pageNo,comparePageNo) {
        if(pageNo == comparePageNo)
            return 
    }
    renderPageList(pageNo,displayPageNos) {
      
        return displayPageNos.map(function(displayPageNo){
                                        if(displayPageNo == pageNo) {
                                            return (<li key={displayPageNo} className="active">{pageNo}</li>)
                                        }
                                        else {
                                            return (<li key={displayPageNo}><Link to={'/m/'+ this.props.pageUrl +'/page/'+displayPageNo+'/'} onClick={this.onClickLink.bind(this,displayPageNo)}>{displayPageNo}</Link></li>)
                                        }
                                    }.bind(this))
    }
    renderPreviousPageArrow(pageNo) {
        if(pageNo == 1) {
            return (
                <span className="pagination-control-prev inactive">
                    <Link to="" className="bwmsprite bwsprite prev-page-icon"/>
                </span>
            )
        }
        else {
            var goToPageNo = parseInt(pageNo) - 1 ;
            return (
                    <span className="pagination-control-prev" >
                         <Link to={'/m/'+ this.props.pageUrl +'/page/'+goToPageNo+'/'} className="bwmsprite bwsprite prev-page-icon" onClick={this.onClickLink.bind(this,goToPageNo)}/>
                    </span>
                
            )
        }
        
    }
    renderNextPageArrow(pageNo,maxNoOfPages) {
        if(pageNo == maxNoOfPages) {
            return (
                <span className="pagination-control-next inactive">
                    <Link to="" className="bwmsprite bwsprite next-page-icon"/>
                </span>
            )
        }
        else {
            var goToPageNo = parseInt(pageNo) + 1;
            return (
                <span className="pagination-control-next" >
                    <Link to={'/m/'+ this.props.pageUrl +'/page/'+goToPageNo+'/'} className="bwmsprite bwsprite next-page-icon" onClick={this.onClickLink.bind(this,goToPageNo)}/>
                </span>
            )
        }
        
    }
	render() {
      
        var {articleCount,endIndex,pageNo,startIndex,articlePerPage} = this.props;
        var maxNoOfPages = Math.ceil(articleCount / articlePerPage);
        var displayPageNos = getDisplayPageNos(articleCount,articlePerPage,pageNo,maxNoOfPages);
        return  (
                    <div className="pagination-container">
                        <div className="grid-5 omega">
                            <span className="text-bold text-default">{startIndex}-{endIndex}</span> of <span className="text-bold text-default">{articleCount}</span> articles
                        </div>
                        <div className="pagination-list-content grid-7 alpha omega position-rel rightfloat">
                            <ul className="pagination-list">
                                {this.renderPageList(pageNo,displayPageNos)}
                            </ul>
                            {this.renderPreviousPageArrow(pageNo)}
                            {this.renderNextPageArrow(pageNo,maxNoOfPages)}
                            
                        </div>
                        <div className="clear"></div>
                    </div>
			)
	}
}

export default NewsPagination
