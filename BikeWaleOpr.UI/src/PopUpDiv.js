/*****************************************************************
this function takes as input one Blank div and other Main div
*****************************************************************/
//Function to Popup and close div
function popup(windowname, backpage) 
{
    blanket_size(windowname, backpage);
    window_pos(windowname);
    toggle(backpage);
    toggle(windowname);
}

//Height of BackPage and Popup
function blanket_size(popUpDivVar, backpage) 
{
    if (typeof window.innerWidth != 'undefined')
        viewportheight = window.innerHeight;
    else
        viewportheight = document.documentElement.clientHeight;
    if ((viewportheight > document.body.parentNode.scrollHeight) && (viewportheight > document.body.parentNode.clientHeight))
        blanket_height = viewportheight;
    else 
    {
        if (document.body.parentNode.clientHeight > document.body.parentNode.scrollHeight)
            blanket_height = document.body.parentNode.clientHeight;
        else
            blanket_height = document.body.parentNode.scrollHeight;
    }
    var blanket = document.getElementById(backpage);
    blanket.style.height = blanket_height + 'px';

    var popUpDiv = document.getElementById(popUpDivVar);
    popUpDiv_height = blanket_height / 2 - 650; //150 is half popup's height
    popUpDiv.style.top = popUpDiv_height + 'px';
}

//Width of Popup
function window_pos(popUpDivVar) 
{
    if (typeof window.innerWidth != 'undefined')
        viewportwidth = window.innerHeight;
    else
        viewportwidth = document.documentElement.clientHeight;
    if ((viewportwidth > document.body.parentNode.scrollWidth) && (viewportwidth > document.body.parentNode.clientWidth))
        window_width = viewportwidth
    else 
    {
        if (document.body.parentNode.clientWidth > document.body.parentNode.scrollWidth)
            window_width = document.body.parentNode.clientWidth;
        else
            window_width = document.body.parentNode.scrollWidth;
    }
    var popUpDiv = document.getElementById(popUpDivVar);
    window_width = window_width / 2 - 150; //150 is half popup's width
    popUpDiv.style.left = window_width + 'px';
}

//Toggle (Open and close) window
function toggle(div_id) 
{
    var el = document.getElementById(div_id);
    if (el.style.display == 'none')
        el.style.display = 'block';
    else
        el.style.display = 'none';
}