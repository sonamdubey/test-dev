if (window.globalPageObject != undefined) {
    sessionStorage.setItem("PAGE", JSON.stringify(globalPageObject));
}
else {
    console.error("Logging global page object - " + JSON.stringify(window.globalPageObject) + ". Logging session storage page object - " + sessionStorage.getItem("PAGE"));
    if (sessionStorage.getItem("PAGE") != null) {
        sessionStorage.removeItem("PAGE")
    }
}

if (window.globalPlatformObject != undefined) {
    sessionStorage.setItem("PLATFORM", JSON.stringify(globalPlatformObject));
}