$.widget("ui.autocomplete", $.ui.autocomplete, {
    options: $.extend({}, this.options, {
        multiselect: false,
        multiselectlength: undefined
    }),
    _create: function () {
        this._super();
        var self = this,
            o = self.options;
        if (o.multiselect) {
            self.selectedItems = {};
            self.multiselect = $("<div></div>")
                .addClass("ui-autocomplete-multiselect ui-state-default ui-widget")
                .css("width", self.element.width())
                .insertBefore(self.element)
                .append(self.element)
                .bind("click.autocomplete", function () {
                    self.element.focus();
                });

            var autocompleteWidth = $('.ui-autocomplete-multiselect').width();
            var itemWidth = 0;
            var fontSize = parseInt(self.element.css("fontSize"), 10);
            function autoSize(e) {
                // Hackish autosizing
                var $this = $(this);
                itemWidth = $('.ui-autocomplete-multiselect .ui-autocomplete-multiselect-item').innerWidth() + 14;
                $this.width(autocompleteWidth - itemWidth);
            };

            var kc = $.ui.keyCode;
            self.element.bind({
                "keydown.autocomplete": function (e) {
                    if ((this.value === "") && (e.keyCode == kc.BACKSPACE)) {
                        var prev = self.element.prev();
                        delete self.selectedItems[prev.text()];
                        prev.remove();
                    }
                    var TABKEY = 9;
                    if (e.keyCode == TABKEY) {
                        e.preventDefault();
                        self.element.focus();
                    }
                },
                // TODO: Implement outline of container
                "focus.autocomplete blur.autocomplete": function () {
                    self.multiselect.toggleClass("ui-state-active");
                },
                "keypress.autocomplete change.autocomplete focus.autocomplete blur.autocomplete": autoSize
            }).trigger("change");

            // TODO: There's a better way?
            o.select = o.select || function (e, ui) {
                
                var noOfselected = $('.ui-autocomplete-multiselect .ui-autocomplete-multiselect-item').length;
                var elemName = ui.item.label;
                var elemid = ui.item.id;
                elemName = (ui.item.label).split(',')[0];
                var searchtype = "city";
                if (noOfselected == 1) {
                    searchtype = "area";
                }
               
                $("<div></div>")
                        .attr("searchid", elemid)
                        .addClass("ui-autocomplete-multiselect-item")                        
                        .text(elemName)
                        .append(
                            $("<span></span>")
                                .addClass("cwsprite cross-sm-dark-grey cur-pointer")
                                .click(function () {
                                    var item = $(this).parent();
                                    Common.utils.trackAction("TopMenu", "Deselect", searchtype + "deleted", Location.utils.getInputlocation());
                                    delete self.selectedItems[item.text()];
                                    if (item.index() == 0)
                                        $('.ui-autocomplete-multiselect-item').remove();
                                    else item.remove();
                                    self.element.show();
                                })                                
                        )
                        .insertBefore(self.element);

                    self.selectedItems[ui.item.label] = ui.item;
                    self._value("");
                    autoSize();
                    self.element.focus();
                    var multiselectObj = $('.ui-autocomplete-multiselect .ui-autocomplete-multiselect-item');
                    
                    if (multiselectObj.length == o.multiselectlength) {
                        self.element.hide();
                    }
                    else if (noOfselected < 1 && elemid != "1" && elemid != "10") {
                        self.element.hide();
                    }
                    return false;
            }
        }
        return this;
    }
});