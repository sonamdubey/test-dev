<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Bikewale.test" Trace ="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="http://st.carwale.com/jquery-1.7.2.min.js?v=1.0" ></script>
    <script src="src/framework/knockout-3.1.0.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Makes List : <ul data-bind="foreach: objData">
            <li data-bind="text: MakeName"></li>
        </ul>
        <%--The name is <span data-bind="text: personFName"></span>
        The name is <span data-bind="text: personLName"></span>
        The full name is <span data-bind="text: FullName"></span>--%>
    </div>
    </form>
    <script type="text/javascript">
        $.getJSON("http://localhost:84/api/BikeMakes/Get", function (data) {
            //objData = ko.mapping.fromJSON(data);
            //ko.applyBindings(objData);
        });
        function foreachEx() {
            var objData;
            //$.ajax({
            //    type: 'POST',
            //    url: 'http://localhost:84/api/BikeMakes/Get',
            //    //data: myJSONData,
            //    //dataType: 'application/xml',
            //    success: function (data) {
            //        objData = eval(data);
            //    } // Success Function 
            //});   // Ajax Call

            $.getJSON("http://localhost:84/api/BikeMakes/Get", function (data) {
                objData = ko.mapping.fromJSON(data);
                ko.applyBindings(objData);
            });

            //var self = this;

            //self.makes = ko.observableArray(objData);            
        }

        //ko.applyBindings(new foreachEx());

        //var myViewModel = {
        //    personFName: ko.observable('Bob'),
        //    personLName: ko.observable('Marley')
        //};

        //ko.applyBindings(myViewModel);
        


        //var newModel = {            
        //    FirstName: ko.observable('ashish'),
        //    LastName: ko.observable('kamble')
        //}

        //newModel.FullName = ko.computed(function () {            
        //    return this.FirstName() + "  " + this.LastName();
        //}, newModel);

        //ko.applyBindings(newModel);

    </script>
</body>
</html>
