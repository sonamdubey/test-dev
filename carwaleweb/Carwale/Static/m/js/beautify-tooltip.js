/* Beauty Tooltip Code Starts Here */
var btObj = {
    invokeToolTipData: {
        clickElement: undefined,
        contentElement: undefined,
        width: undefined,
        position: undefined
    },
    btSelector: '', btIdFinder: '',
    bindCommonBT: function () {
        var self = this;
        self.btTipDiv = btObj.btIdFinder;
        self.btTipContent = null;
        self.btTipFill = null;
        self.btTipWidth = null;
        self.btTipPosition = null;
        self.btTipStrokeStyle = null;
        self.btTipStrokeWidth = null;
        self.btTipspikeLength = null;
        self.btTipShadow = null;
        self.btTipShadowColor = null;
        self.btTipPadding = null;
        self.btTipShadowOffsetX = null;
        self.btTipShadowOffsetY = null;
        self.btTipShadowBlur = null;
        self.btTipShadowOverlap = null;

        // function to bind Beauty tooltip
        btObj.btSelector = self.btTipDiv;
        self.btCall = function () {
            $(self.btTipDiv).bt({
                contentSelector: self.btTipContent,
                fill: (self.btTipFill == null || self.btTipFill == undefined) ? '#fff' : self.btTipFill,
                width: (self.btTipWidth == null || self.btTipWidth == undefined) ? 220 : self.btTipWidth,
                positions: (self.btTipPosition == null || self.btTipPosition == undefined) ? ['top', 'bottom'] : self.btTipPosition,
                strokeStyle: (self.btTipStrokeStyle == null || self.btTipStrokeStyle == undefined) ? '#6b6b6b' : self.btTipStrokeStyle,
                strokeWidth: (self.btTipStrokeWidth == null || self.btTipStrokeWidth == undefined) ? 1 : self.btTipStrokeWidth,
                spikeLength: (self.btTipspikeLength == null || self.btTipspikeLength == undefined) ? 8 : self.btTipspikeLength,
                shadow: (self.btTipShadow == null || self.btTipShadow == undefined) ? true : self.btShadow,
                shadowColor: (self.btTipShadowColor == null || self.btTipShadowColor == undefined) ? '#929292' : self.btTipShadowColor,
                padding: (self.btTipPadding == null || self.btTipPadding == undefined) ? 10 : self.btPadding,
                shadowOffsetX: (self.btTipShadowOffsetX == null || self.btTipShadowOffsetX == undefined) ? 2 : self.btTipShadowOffsetX,
                shadowOffsetY: (self.btTipShadowOffsetY == null || self.btTipShadowOffsetY == undefined) ? 2 : self.btTipShadowOffsetY,
                shadowBlur: (self.btTipShadowBlur == null || self.btTipShadowBlur == undefined) ? 4 : self.btTipShadowBlur,
                shadowOverlap: (self.btTipShadowOverlap == null || self.btTipShadowOverlap == undefined) ? false : self.btTipShadowOverlap,
                trigger: (self.btTipTrigger == null || self.btTipTrigger == undefined) ? ['none'] : self.btTipTrigger,
            });
        }
    },
    btToolTipAd: function (pageId, label) {
        var modelPageId = 68;
        var versionPageId = 69;
        var picPageId = 76;
        var camp = new btObj.bindCommonBT();

        if (btObj.btIdFinder == '#camp-model-info-tooltip') {
            camp.btTipContent = "$('.camp-model-info-content').html()";
            camp.btTipWidth = 230
            try {
                if (pageId == picPageId) {
                    cwTracking.trackAction('CWNonInteractive', 'SponsoredLink_Shown', 'PIC_ToolTip_m', label);
                    cwTracking.trackAction('CWNonInteractive', 'Model_Page_Tooltip_Experiment_4Cities_MSite', 'Dealer_Link_shown', zoneNameForModelVersion + "," + ModelName);
                }
                else if (pageId == versionPageId) {
                    cwTracking.trackAction('CWNonInteractive', 'SponsoredLink_Shown', 'VersionPage_ToolTip_m', label);
                    cwTracking.trackAction('CWNonInteractive', 'Version_Page_Tooltip_Experiment_4Cities_MSite', 'Dealer_Link_shown', zoneNameForModelVersion + "," + ModelName);
                } else {
                    cwTracking.trackAction('CWNonInteractive', 'SponsoredLink_Shown', 'ModelPage_ToolTip_m', label);
                    cwTracking.trackAction('CWNonInteractive', 'Model_Page_Tooltip_Experiment_4Cities_MSite', 'Dealer_Link_shown', zoneNameForModelVersion + "," + ModelName);
                }
            } catch (e) {
            }

        }
        else if (btObj.btIdFinder == '#nocamp-model-info-tooltip') {
            camp.btTipContent = "$('.nocamp-model-info-content').html()";
            try {
                if (pageId == picPageId) {
                    cwTracking.trackAction('CWNonInteractive', 'SponsoredLink_Shown', 'PIC_ToolTip_m', label);
                }
                else if (pageId == versionPageId)
                    cwTracking.trackAction('CWNonInteractive', 'SponsoredLink_Shown', 'VersionPage_ToolTip_m', label);
                else
                    cwTracking.trackAction('CWNonInteractive', 'SponsoredLink_Shown', 'ModelPage_ToolTip_m', label);
            } catch (e) {
            }

        }
        else if ($(btObj.btIdFinder).hasClass('emiTooltip__js')) {
            camp.btTipContent = "$(this).closest('.customizeEmiContainer__js').siblings().find('.emiTooltipContent__js').html()";
            camp.btTipWidth = 210;
        }
        else if (btObj.btIdFinder.attr("id") == 'gst-est-tooltip') {
            camp.btTipContent = "$('.gst-est-tooltip-content').html()";
            camp.btTipWidth = 230;
            camp.btTipPosition = ['top'];
        }
        else if (btObj.btIdFinder.attr("id") == 'gst-tooltip') {
            camp.btTipContent = "$('.gst-tooltip-content').html()";
            camp.btTipWidth = 230;
            camp.btTipPosition = ['top'];
        }

        else if ($(btObj.btIdFinder).hasClass('average-info-tooltip')) {
            camp.btTipContent = "$(this).siblings('.average-info-content').html()";
            camp.btTipWidth = 210;
        }

        camp.btCall();
    },

    registerEvents: function () {
        $(document).on('click', '.bt-wrapper a', function (e) {
            e.preventDefault();
            $(btObj.btSelector).btOff();
        });
        $(window).on("resize", function () {
            var temp = $(btObj.btSelector);
            if (temp.btOff) temp.btOff();
        });
        $('.ad-tooltip').on('click', function () {
            btObj.btIdFinder = $(this).attr('id');
            btObj.btIdFinder = '#' + btObj.btIdFinder;
            var pageId = $(this).data("assigned-id");
            var label = $(this).data("label");
            btObj.btToolTipAd(pageId, label);
            $(btObj.btSelector).btOn();
            if (btObj.btIdFinder == '#camp-model-info-tooltip') {
                var btContentElement = $('.bt-wrapper').find('.bt-content');
                if (btContentElement.has('[campaigncta]')) {
                    btContentElement.find('[campaigncta]').removeAttr("data-campaign-event");
                    window.registerCampaignEvent(btContentElement[0]);
                }
            }
        });
    },
    registerEventsClass: function () {
        $('.class-ad-tooltip').on('click', function () {
            btObj.btIdFinder = $(this);
            var pageId = $(this).data("assigned-id");
            var label = $(this).data("label");
            btObj.btToolTipAd(pageId, label);
            $('.class-ad-tooltip').btOff();
            btObj.btSelector.btOn();
            
        });
    },

    invokeToolTip: function () {
        var self = this;
        btObj.btIdFinder = $(self.invokeToolTipData.clickElement);
        var camp = new btObj.bindCommonBT();
        camp.btTipContent = "$(btObj.invokeToolTipData.contentElement).html()";
        if (self.invokeToolTipData.width != undefined)
            camp.btTipWidth = self.invokeToolTipData.width;
        if (self.invokeToolTipData.position != undefined)
            camp.btTipPosition = self.invokeToolTipData.position;
        camp.btCall();
        btObj.btSelector.btOn();
    }
}
$(window).load(function () {
    btObj.registerEvents();
    btObj.registerEventsClass();
});
/* Beauty Tooltip Code Ends Here */