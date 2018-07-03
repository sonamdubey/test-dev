var qnapage = $("#tblQnA");
var koUtils = ko.utils;

var moderateQuestion = {
    questionId :"",moderatedBy:"", userEmail : "", userName : "", questionText:"", tag : "" , RejectionReasonId : ""
}
var QnA = function () {
    var self = this;
    self.selectedQuestionId;
    self.questionText = ko.observable();
    self.questionTags = ko.observableArray([]);    
    self.oldQuestionTags = [];
    self.isTagEdited = false;
    self.userName = ko.observable();
    self.userEmail = ko.observable();
    self.moderatorId = ko.observable(0);
    self.selectedMakeMaskingName;
    self.selectedModelMaskingName;
    self.selectedMakeId;
    self.selectedModelId;
    self.bikeModels = ko.observableArray();
    self.questionStatus = ko.observable();
    self.answersList = ko.observableArray();
    self.answerText = ko.observable('');
    self.internalUserId = ko.observable(0);
    self.internalUserName;
    self.internalUserEmail;
    self.makeTag;
    
    self.editMakeTag = function (d, e) {
        var makeId = $(e.target).find('option:selected').data("makeid");
        self.selectedMakeMaskingName = $(e.target).val();
        if (makeId) {
            self.selectedMakeId = parseInt(makeId);
        }

        if (self.selectedMakeId && self.selectedMakeId > 0) {
            self.makeTag = {
                Name: self.selectedMakeMaskingName
            };
            self.isTagEdited = true;
            self.questionTags(self.makeTag);
            $.ajax({
                type: "GET",
                url: "/api/models/makeid/" + self.selectedMakeId + "/requesttype/8/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if (response && response.length > 0) {
                        self.bikeModels(response);
                        $('select[name="ModelMaskingName"]').val(qnapage.data("modelid")).material_select();
                    }
                    else {
                        self.bikeModels([]);
                    }

                },
                complete: function (xhr) {
                    if (xhr.status != 200) {
                        self.bikeModels([]);
                    }
                    $('select[name="ModelMaskingName"]').material_select();
                }
            });
        }

    };

    self.setModel = function () {
        self.selectedModelMaskingName = $('select[name="ModelMaskingName"]').find('option:selected').val();        
        if (self.selectedModelMaskingName != undefined && self.selectedModelMaskingName != "") {
            var modelTag = {
                Name: self.selectedModelMaskingName
            };
            
            
            var arr = [];
            arr.push(self.makeTag);
            arr.push(modelTag);
            self.questionTags(arr);
            var objModel = self.bikeModels().find(item => item.maskingName === modelTag.Name);
            self.selectedModelId = objModel.modelId;
        }              
    };

    self.changeUser = function(d, e) {
        self.internalUserId($(e.target).val());
        self.internalUserName = $(e.target).find('option:selected').text();
        self.internalUserEmail = $(e.target).find('option:selected').data('internaluseremail');
    }
    self.openQuestionDetailsModal = function (questionNo, modalId) {
        var question = jsData[questionNo];
        
        self.selectedQuestionId = question.Id;
        self.questionText(question.Text);
        self.questionTags(question.Tags);     
        self.userEmail(question.EndUser.Email);
        self.userName(question.EndUser.Name);               
        $('select#ddlMakes').val("none").material_select();
        self.bikeModels([]);
        $('select#ddlModels').material_select();
        self.isTagEdited = false;
        self.questionStatus(question.Status);
        self.answerText('');
        $('select#ddlInternalUsers').val("0").material_select();       
        self.fetchAnswers();
        self.oldQuestionTags = self.questionTags();
        self.internalUserId(0);
        $('textarea').trigger('autoresize');
        $(modalId).modal('open');
    };

    self.fetchAnswers = function() {
        if(self.questionStatus() === 3) {
            $.ajax({
                type: "GET",
                url: "/api/questions/" + self.selectedQuestionId + "/answers/",
                contentType: "application/json",
                dataType: 'json',
                success: function (response) {
                    if(response && response.answers) {
                        self.answersList(response.answers);
                    }
                },
                complete: function(response) {

                }
            });
        }
        else {
            self.answerText('');
        }
    };

    self.updateTags = function (actionType) {
        var isSuccess = false;
        var updatedTags = {
            moderatorId: self.moderatorId(),
            oldTags: self.oldQuestionTags.map(item => item.Id),
            newTags: koUtils.arrayMap(self.questionTags(), item => item.Name),
            modelId: self.selectedModelId
        };

        return $.ajax({
            type: "POST",
            url: "/api/questions/" + self.selectedQuestionId + "/updatetags/",
            contentType: "application/json",
            dataType: 'json',
            data: ko.toJSON(updatedTags),
            success: function (response) {
                if (response && response.questionId) {
                    isSuccess = response.isSuccessful;
                    if (isSuccess) {
                        Materialize.toast("Tags updated", 2000);
                        
                    }
                    else {
                        Materialize.toast("Failed to update tags.", 2000);
                    }
                }
                else {
                    Materialize.toast("Some error occured. Please try again later.", 2000);
                }
            },
            complete: function (xhr) {
                if (xhr.status != 200) {
                    Materialize.toast("Failed to update tags.", 2000);
                }

            }
        });
    }

    self.changeStatus = function(statusText) {
        var currentQ = qnapage.find(".tr_" + self.selectedQuestionId);        
        currentQ.find(".question-status").text(statusText);
        currentQ.find(".question-details").hide();
    };
    
    self.moderateQuestion = function (actionType, sendEmail) {

        var isUpdateTag = self.isTagEdited;

        //Tags are updated when the action is for approving a question.
        if (actionType == 0 && isUpdateTag) {
            return self.updateTags(actionType).done(self.performModeration(actionType, sendEmail));
        }
        if (self.selectedQuestionId != undefined && self.questionText() && self.moderatorId() > 0 && !isUpdateTag) {
            if(actionType == 1) {
                var rejectionReasonId = $("input[name='disapprovalReason']:checked").val();
                if(rejectionReasonId === undefined || rejectionReasonId === null || rejectionReasonId === "") {
                    Materialize.toast("Please select a valid Rejection Reason.", 2000);
                }
                else {
                    return self.performModeration(actionType, sendEmail);
                }
            }
            else {
                return self.performModeration(actionType, sendEmail);
            }
        }
        return new Promise();
    };

    self.performModeration = function(actionType, sendEmail) {
        
        var tags = koUtils.arrayMap(self.questionTags(), item => item.Name).join();
        
        var moderateQuestion = {
            questionId: self.selectedQuestionId,
            moderatedBy: self.moderatorId(),
            userEmail: self.userEmail(),
            userName: self.userName(),
            questionText: self.questionText(),                
            RejectionReasonId: actionType == 1 ? $("input[name='disapprovalReason']:checked").val() : ""
        };
        return $.ajax({
            type: "POST",
            url: "/api/questions/" + moderateQuestion.questionId + "/moderate/" + actionType + "/?sendEmail=" + sendEmail,
            contentType: "application/json",
            dataType: 'json',
            data: ko.toJSON(moderateQuestion),
            success: function (response) {
                if (response && response.message) {
                    if (response.isModeratedSuccessfully) {
                        self.changeStatus(actionType > 0 ? "Rejected" : "Approved");
                    }
                    if (actionType == 1) {
                        $('#rejectionReason').modal('close');
                    }
                    if(self.questionStatus() === 1) {
                        Materialize.toast(response.message, 2000);
                    }
                    
                }
                else {
                    Materialize.toast("Some error occured. Please try again later.", 2000);
                }                
            },
            complete: function (xhr) {
                if (xhr.status != 200) {
                    Materialize.toast("Question not approved", 2000);
                }

            }
        });
    };

    self.saveAnswer = function() {
        var objAnsweredBy = {
            id: self.internalUserId(),
            name: self.internalUserName,
            email: self.internalUserEmail
        };
        var answeredQuestion = {

            questionId: self.selectedQuestionId,
            questionText: self.questionText(),            
            askedByName: self.userName(), 
            askedByEmail: self.userEmail(),
            text: self.answerText(),
            answeredBy: objAnsweredBy
        };

        return $.ajax({
            type: "POST",
            url: "/api/questions/" + answeredQuestion.questionId + "/answer/?platformId=1&sourceId=1",
            contentType: "application/json",
            dataType: 'json',
            data: ko.toJSON(answeredQuestion),
            success: function(response) {
                if(response) {
                    self.changeStatus("Answered");
                    Materialize.toast(response, 2000);
                }
            },                
            complete: function (xhr) {
                if (xhr.status != 200) {
                    Materialize.toast("Answer not saved", 2000);
                }
            }
        });
    };

    self.answerQuestion = function() {
        
        if(self.questionStatus() == 2) {
            return self.saveAnswer();
        }
        else {
            return self.moderateQuestion(0, false).done(self.saveAnswer);
        }
    };
};

var vmQnA = new QnA;
if (qnapage) {
    vmQnA.moderatorId(qnapage.data("moderatorid"));
    ko.applyBindings(vmQnA, qnapage[0]);
}
