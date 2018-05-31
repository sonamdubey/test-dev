import {RemoveSpecialCharacters} from './commonUtils'

function addAdToGTcmd(adUnitPath, adDimension, adDivId, adTarget) {
	googletag.cmd.push(function () {
		for (var tagKey in adTarget) {
			if (adTarget[tagKey]) {
				googletag.pubads().setTargeting(tagKey, RemoveSpecialCharacters(adTarget[tagKey]));
			}
			else {
				googletag.pubads().clearTargeting(tagKey);
			}
		}

		var slot = googletag.defineSlot(adUnitPath, adDimension, adDivId);
		if (!slot) return;
		slot.addService(googletag.pubads());
		googletag.pubads().enableAsyncRendering();
		googletag.pubads().updateCorrelator();
		googletag.pubads().enableSingleRequest(false);

		googletag.pubads().collapseEmptyDivs();
		googletag.enableServices();

	});
}

function addAdSlot(adUnitPath , adDimension , adDivId , tags) {
  try {
    if(!googletag && !googletag.cmd)
      return;

    var adExists = false; // remains false if the ad slot has already been defined irrespective of whether googletag has loaded

    // event listener slotrenderended is added in appshell itself as it is not specific to any particuler ad
    if(googletag.apiReady) 
    {
      var slots = googletag.pubads().getSlots();
      for(var i=0;i<slots.length;i++) {
        var ad = slots[i];
        if(ad.getAdUnitPath() === adUnitPath) {
            adExists = true;
            break;
        }
      }
		}
		
    if(!adExists) {
    	let adTarget = {
    		Model: '',
    		Make: '',
    		City: '',
    		Series: '',
    		Tags: ''
    	}

    	adTarget = Object.assign(adTarget, tags)

    	addAdToGTcmd(adUnitPath, adDimension, adDivId, adTarget);
    }    
    
  }
  catch(e) {
    
  }
	  
    
}


function removeAdSlot(adUnitPath) {
  try {
    if(typeof googletag == "undefined" || !googletag.apiReady) {
      return;
    }
    var slots = googletag.pubads().getSlots();
    for(var i = 0;i<slots.length;i++) {
      var ad = slots[i];
      if(ad.getAdUnitPath() === adUnitPath) {
        googletag.destroySlots([ad]);
        break;
      }
    }  
  }
  catch(e) {
   
  }
  
  
}


function refreshGPTAds() {
 try {
  setTimeout(function(){
    if(typeof googletag != "undefined") {
      googletag.cmd.push(function () {
        googletag.pubads().refresh();
      });
     
    }
  },100)
    
 }
 catch(e) {
 }
  
}

module.exports = {
	addAdSlot,
	removeAdSlot,
	refreshGPTAds
}