function AddRow(element, label, type) {
    var table = document.getElementById("tblBasicInfo");
    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);

    var cell1 = row.insertCell(0);
    cell1.innerHTML = label;

    var cell2 = row.insertCell(1);
    cell2.innerHTML = ":";

    var cell3 = row.insertCell(2);
    var element1 = document.createElement("input");
    element1.type = type;
    element1.setAttribute("style", "text-align:left");
    cell3.appendChild(element1);
}

function CleanHtml(rteId) {
    var h = tinyMCE.get(rteId).getContent();
    h = h.replace(/<[/]?(font|st1|shape|path|lock|imagedata|stroke|formulas|span|xml|del|ins|[ovwxpm]:\w+)[^>]*?>/gi, '');
    h = h.replace(/<[/]?(font|link|m|st1|meta|object|style|span|xml|del|ins|[ovwxp]:\\w+)[^>]*?>/gi, '');
    h = h.replace(/<([^>]*)(?:lang|style|size|face|[ovwxp]:\\w+)=(?:'[^']*'|\"[^\"]*\"|[^\\s>]+)([^>]*)>/gi, '<$1 $2>');
    h = h.replace(/<!--\[if\s.*(?:[^<]+|<(?!!\[endif\]-->))*<!\[endif\]-->/gi, '');
    h = h.replace(/<([^>]*)class="MsoNormal"([^>]*)>/gi, '<$1 $2>');
    if( h.indexOf("<p><!--[") >= 0 ) {
        h = RemoveHtml(h, "<p><!--[", "--></p>");
    }
    if (h.indexOf("</p>") == 0)
        h = RemoveHtml(h, "", "</p>"); 

    if( h.indexOf("<!--[") >= 0){   
        h = RemoveHtml(h, "<!--[", "<!--[endif] -->");
    }
    tinyMCE.activeEditor.setContent(h);
}

function RemoveHtml (html, beginStr, endStr){
    var retVal = html;
    if (beginStr == "" && html.indexOf(endStr) > -1){            
        var j = html.indexOf(endStr);
        retVal = html.substring(j + endStr.length);
    }
    if (html.indexOf(beginStr) > -1 && html.indexOf(endStr) > -1) {
        var i = html.indexOf(beginStr);
        var j = html.indexOf(endStr);

        retVal = html.replace(html.substring(i, j + endStr.length),"");
    }
    return retVal;
}