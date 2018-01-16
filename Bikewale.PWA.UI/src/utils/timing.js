import {isServer} from '../utils/commonUtils'

var timer = null;
function resetTimer() {
	timer = null;
}
function startTimer(componentRenderCount,adCount) {	// to be called only once by the api called first
	var startTime = (new Date()).getTime();
	timer = {
		adCount : adCount,
		componentRenderCount : componentRenderCount,
		startTime : startTime,
		endTime : 0
	}
	if(!isServer()) {
		window.endTimer = endTimer;
	}
}

function endTimer(event) {
	if(timer) {
		switch (event) {
			case "component-render" : timer.componentRenderCount = timer.componentRenderCount <= 0 ? 0 : (timer.componentRenderCount-1);
										break;
			case "ad" : timer.adCount = timer.adCount <= 0 ? 0 : (timer.adCount - 1 );
										break;
		}
		if(timer.componentRenderCount == 0 && timer.adCount == 0) {
			timer.endTime = (event === "component-render") ? (new Date()).getTime() + 50 : (new Date()).getTime();
			logTiming(timer);
			timer = null;
		}
	}

}

import {cwTracking} from './analyticsUtils'
function logTiming() {
	if(timer) {
		var loadTime = timer.endTime - timer.startTime;
		if(window.FirstRenderWithAppshell) {
			window.FirstRenderWithAppshell = false;
		}
		else {
			cwTracking.trackCustomData("BWPerformance","MsitePWA","navigationStart="+timer.startTime+"|loadEventEnd="+timer.endTime);
			if(window.FirstRenderWithAppshell == undefined || window.FirstRenderWithAppshell == null)	
				window.FirstRenderWithAppshell = false;
		}
	}
}

module.exports = {
	resetTimer,
	startTimer,
	endTimer,
	logTiming
}