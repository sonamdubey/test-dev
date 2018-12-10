// Code for tracking Newtwork connection of Client
if ("connection" in navigator && Common.utils.storageAvailable("sessionStorage") && !window.sessionStorage.getItem("network_tracked")) {
    var con = navigator.connection;
    var label = "effectiveType=" + con.effectiveType + ",rtt=" + con.rtt + "ms,downlink=" + con.downlink + "Mb/s";
    cwTracking.trackAction("CWNonInteractive", "NetworkSpeed", "TrackSpeed", label);
    window.sessionStorage.setItem("network_tracked", 1);
}