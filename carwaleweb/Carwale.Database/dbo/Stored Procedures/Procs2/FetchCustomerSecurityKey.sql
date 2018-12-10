IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FetchCustomerSecurityKey]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FetchCustomerSecurityKey]
GO

	
--THIS PROCEDURE WILL RETURN THE CUSTOMER DETAILS IN THE FORM OF THE QUERY. CUSTOMER ID WILL BE PASSED AS INPUT
--IF IT IS THE NEW CUSTOMWER THEN DETAILS FROM THE TEMPCUSTOMERS TABLE WILL BE RETURN ELSE DETAILS FROM THE
--CUSTOMERS TABLE WILL BE RETURNED

--Customers	Id, Name, email, Address, StateId, CityId, AreaId, PinCodeId, phone1, phone2, Mobile, password, PrimaryPhone, Industry, 
--		Designation, Organization, DOB, ContactHours, ContactMode, CurrentVehicle, InternetUsePlace, CarwaleContact, InternetUseTime, 
--		ReceiveNewsletters, RegistrationDateTime, IsEmailVerified, IsVerified, TempId
--TempCustomers  Id, Name, email, Address, Area, City, State, PinCode, phone1, phone2, Mobile, password, PrimaryPhone, Industry, Designation, 
--		Organization, DOB, ContactHours, ContactMode, CurrentVehicle, InternetUsePlace, CarwaleContact, InternetUseTime, 
--		ReceiveNewsletters, RegistrationDateTime

CREATE PROCEDURE [dbo].[FetchCustomerSecurityKey]
	@Key			VARCHAR(50), 
	@CustomerId		NUMERIC	OUTPUT
AS
BEGIN
	
	SET @CustomerId = -1

	SELECT 
		@CustomerId 	= CSK.CustomerId
	FROM 
		CustomerSecurityKey AS CSK, Customers AS CU
	WHERE 
		CSK.CustomerKey = @Key AND CU.ID = CSK.CustomerId AND CU.IsFake = 0

	

END
