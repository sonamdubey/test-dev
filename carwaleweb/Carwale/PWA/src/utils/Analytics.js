export const fireNonInteractiveTracking = (cat, act, lab) => {
    setTimeout(() => {
        dataLayer.push({
            event: "CWNonInteractive",
            cat,
            act,
            lab,
            transport: 'beacon'
        })
    }, 0)
}

export const fireInteractiveTracking = (cat, act, lab) => {
    setTimeout(() => {
        dataLayer.push({
            event: "CWInteractive",
            cat,
            act,
            lab,
            transport: 'beacon'
        })
    }, 0)
}
const LocationQueue = []
const sendPageviewToGA = ({pathname, search}) => {
    ga('gtm1.set', {
        page: pathname + search,
        title: document.title
    })
    ga('gtm1.send', 'pageview')
}
export const firePageView = (location) => {
    if (typeof ga !== "undefined") {
        while (LocationQueue.length) {
            sendPageviewToGA(LocationQueue.shift())
        }
        sendPageviewToGA(location)
    }
    else {
        LocationQueue.push(location)
    }
}
