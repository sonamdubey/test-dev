import React from 'react'

const bhriguHostPath = BHRIGU_HOST_URL

const hostPath= location.href.host

const isMobileSite = () => {
        let url = document.location.href;
        if (url.indexOf('/m/') > 0)
            return true;
        else
            return false;
    }
const type = {
        impression: 'imp',
        click: 'click'
    }

const getSource = () => {
        let url = document.referrer;
        if (url.length == 0)
            return 'src=direct';
        else if (url.indexOf(hostPath) < 0)
            return 'src=' + url;

        return '';
    }

const getPageUrl = () => {
        let url = document.location.href;
        if (url.indexOf('?') > 0)
            return 'pi=' + url.split('?')[0];
        else if (url.indexOf('#') > 0)
            return 'pi=' + url.split('#')[0];
        else
            return 'pi=' + url;
    }

const getQSFromUrl = () => {
        let url = document.location.href;
        let qs = '';
        if (url.indexOf('?') > 0) {
            qs = url.split('?')[1];
        }
        else if (url.indexOf('#') > 0) {
            qs = url.split('#')[1];
        }
        if (qs.length > 0) {
            qs = qs.replace(/&+/g, '|');
            return 'qs=' + qs;
        }
        else
            return '';
    }

const getReferrer = () => {
        let url = document.referrer;
        let host = location.host;
        if (url.length > 0 && url.indexOf(host) >= 0) {
            let relativePath = url.split(hostPath)[1];
            return 'ref=' + (relativePath == undefined || relativePath == null ? '' : relativePath.replace(/&+/g, '|'));
        }
        return '';
    }

const addToQS = (qs, value) => {
        if (qs.length > 0)
            qs += '&' + value;
        else
            qs = value;
        return qs;
    }

const getAttributeValue = (type) => {
        switch (type) {
            case 'imp': return 'cwti';
            case 'click': return 'cwtc';
            default: return 'cwcti';
        }
    }

const getCompleteQS = (node, type) => {
        let cat, lbl, act, qs = '', attrValue = getAttributeValue(type), sendQS = false;
        if (node.is('[qs]'))
            sendQS = true;
        cat = node.data(attrValue + 'cat'), lbl = node.data(attrValue + 'lbl'), act = node.data(attrValue + 'act');
        qs = getFinalQS(cat, act, lbl, sendQS);
        return qs;
    }

const getFinalQS = (cat, act, lbl, sendQS) => {
        let src, pageUrl, urlQs, ref, qs = '';
        src = getSource(), pageUrl = getPageUrl(), ref = getReferrer();
        if (cat != undefined && cat.length > 0)
            qs = addToQS(qs, 'cat=' + cat);
        if (act != undefined && act.length > 0)
            qs = addToQS(qs, 'act=' + act);
        if (lbl != undefined && lbl.length > 0)
            qs = addToQS(qs, 'lbl=' + lbl);
        if (src != undefined && src.length > 0)
            qs = addToQS(qs, src);
        if (pageUrl != undefined && pageUrl.length > 0)
            qs = addToQS(qs, pageUrl);

        if (ref != undefined && ref.length > 0)
            qs = addToQS(qs, ref);

        if (sendQS) {
            urlQs = getQSFromUrl();
            if (urlQs != undefined && urlQs.length > 0)
                qs = addToQS(qs, urlQs);
        }

        return qs;
    }

export const trackCustomData = (cat, act, lbl, sendQS) => {
        let qs = getFinalQS(cat, act, lbl, sendQS);
        sendRequest(qs);
    }

const trackDataFromNode = (node, type) => {
        let qs = getCompleteQS(node, type);
        sendRequest(qs);
    }

const sendRequest = (qs) => {
        if (qs.length > 0)
            qs = '&' + qs;
            let img = new Image();
        img.src = bhriguHostPath + Date.now() + qs;
    }

