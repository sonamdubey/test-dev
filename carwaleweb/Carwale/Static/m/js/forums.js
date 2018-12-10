function PageChanged() {
    if (typeof threadId != "undefined" && typeof postUrl != "undefined")
    {
        $("#formPosts").attr("action", "/m/forums/" + threadId + "-" + postUrl + "-p" + $("#ddlPages").val() + ".html");
        $("#formPosts").submit();
    }
    else
    {
        ClientErrorLogger(["threadId = " +threadId,"postUrl = "+ postUrl, "PageChanged()"], "/api/exceptions/");
    }
}

function PrevClicked() {
    if (typeof threadId != "undefined" && typeof postUrl != "undefined" && typeof prevPage != "undefined")
    {
        $("#formPosts").attr("action", "/m/forums/" + threadId + "-" + postUrl + "-p" + prevPage + ".html");
        $("#formPosts").submit();
    }
    else
    {
        ClientErrorLogger(["threadId = " + threadId, "postUrl = " + postUrl, "prevPage = " + prevPage, "PrevClicked()"], "/api/exceptions/");
    } 
}

function NextClicked() {
    if (typeof threadId != "undefined" && typeof postUrl != "undefined" && typeof nextPage != "undefined")
    {
        $("#formPosts").attr("action", "/m/forums/" + threadId + "-" + postUrl + "-p" + nextPage + ".html");
        $("#formPosts").submit();
    }
    else
    {
        ClientErrorLogger(["threadId = " + threadId, "postUrl = " + postUrl, "nextPage = " + nextPage, "NextClicked()"], "/api/exceptions/");
    }
}