var pageMetas = $('#managePageMetas');
var deleteButton = $('#deleteAccordian');
var pageMetaRecord = { pageMetaId: "", status: "",updatedBy: "" ,makeIdList: null,modelIdList: null};
var currentUserId = $('#userId').val();
function Show() {
    var status = $("input[name='group1']:checked").val();
    if (status)
        window.location.href = "/pageMetas/?pageMetaStatus=" + status;
}

$('.update-status').on('click', function () {
    if (confirm("Do you really want to delete?")) {
        var ele = $(this);
        pageMetaRecord.status = ele.data('setstatus');
        pageMetaRecord.pageMetaId = ele.data('pagemetaid');
        if (ele.data('makeid') > 0)
        {
            if(ele.data('modelid') > 0)
            {
                pageMetaRecord.modelIdList = ele.data('modelid');
            }
            else
            {
                pageMetaRecord.makeIdList = ele.data('makeid');
            }

        }
        pageMetaRecord.updatedBy = currentUserId;

        $.ajax({
            type: "POST",
            url: "/api/pagemetas/update/",
            contentType: "application/json",
            data: ko.toJSON(pageMetaRecord),
            success: function () {
                ele.closest('tr').hide();
                Materialize.toast("Page Meta Id " + pageMetaRecord.pageMetaId + " Status Updated Successfully", 2000);
            },
            error: function (e) {
                Materialize.toast('Error occured', 4000);
            }

        });
    }
    

});

$('.deleteCheckBoxClicked').change(function () {

    if (!deleteButton.hasClass('active')) {
        deleteButton.addClass('active');
        deleteButton.find('.collapsible-header').addClass('active');
        deleteButton.find('.collapsible-body').css({ display: "block" });
    }
});

$('#chkAll').change(function () {
    if (this.checked) {
        $('input:checkbox').each(function () {
            $(this).prop('checked', true);
        });
    }
    else {
        $('input:checkbox').each(function () {
            $(this).prop('checked', false);
        });
    }
});

function checkSelected() {
    if ($("input:checked").length == 0) {
        $('#checkboxEmpty').modal('open');
        return false;
    }
    else
        return true;
};

var pageMetas = function () {
    var self = this;
    self.pageMetaIdList = '';
    self.modelIdList = '';
    self.makeIdList = '';

    self.deleteRecords = function () {
        if (checkSelected()) {
            self.findSelectedReviews();
            self.changeStatus(0);
        }
    };

    self.findSelectedReviews = function () {
        $("input:checked").each(function () {
            if ($(this).attr('data-metaid'))
                self.pageMetaIdList = self.pageMetaIdList + $(this).attr('data-metaid') + ',';

            if ($(this).attr('data-checkedmakeid') > 0)
            {
                if ($(this).attr('data-checkedmodelid') == 0)
                   self.makeIdList = self.makeIdList + $(this).attr('data-checkedmakeid') + ',';
               else
                    self.modelIdList = self.modelIdList + $(this).attr('data-checkedmodelid') + ',';
            }
          


        });
    };

    self.slideUp = function () {

        if (deleteButton.hasClass('active')) {
            deleteButton.removeClass('active');
            deleteButton.find('.collapsible-header').removeClass('active');
            deleteButton.find('.collapsible-body').css({ display: "none" });
        }
        $('input:checkbox').each(function () {
            $(this).prop('checked', false);
        });

    };
    self.changeStatus = function (status) {
        if (confirm("Do you really want to delete?"))
        {
            pageMetaRecord.pageMetaId = self.pageMetaIdList;
            pageMetaRecord.status = 0;
            pageMetaRecord.updatedBy = currentUserId;
            pageMetaRecord.modelIdList = self.modelIdList;
            pageMetaRecord.makeIdList = self.makeIdList;
            $.ajax({
                type: "POST",
                url: "/api/pagemetas/update/",
                contentType: "application/json",
                data: ko.toJSON(pageMetaRecord),
                success: function (response) {
                    if (response) {
                        var array = self.pageMetaIdList.toString().split(',');
                        for (var i = 0; i < array.length; i++) {

                            if (array[i]) {
                                $("#metaDelete_" + array[i]).closest("tr").remove();
                                Materialize.toast("Page Meta Id " + array[i] + " Status Updated Successfully", 2000);
                            }
                        }

                    }
                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        Materialize.toast("Failed to update", 2000);
                    }
                    self.pageMetaIdList = '';
                    self.modelIdList = null;
                    self.makeIdList = null;
                    self.slideUp();

                }
            });
        }
        else
        {
            self.slideUp();
        }

        }
       
};

var vmPageMetas = new pageMetas();
ko.applyBindings(vmPageMetas, pageMetas[0]);
