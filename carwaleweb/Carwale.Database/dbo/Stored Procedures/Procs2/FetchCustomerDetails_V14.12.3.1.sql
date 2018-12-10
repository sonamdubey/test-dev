IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FetchCustomerDetails_V14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FetchCustomerDetails_V14]
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
-- Modified by Aditi Dhaybar on 26/09/14 for new contact seller process
-- Modified By: Rakesh Yadav On 02 Feb 2016, stop using password column
-- Modified by: Rakesh Yadav on 28 june 2016, removed print @StateId and print @DOB
CREATE PROCEDURE [dbo].[FetchCustomerDetails_V14.12.3.1]
	@CustomerId		NUMERIC,
	@Name			VARCHAR(100)	OUTPUT, 
	@Email			VARCHAR(100)	OUTPUT,  
	@Address		VARCHAR(250)	OUTPUT, 
	@StateId		NUMERIC	OUTPUT, 
	@State			VARCHAR(50)	OUTPUT,
	@CityId			NUMERIC	OUTPUT, 
	@City			VARCHAR(50)	OUTPUT,
	@AreaId		NUMERIC	OUTPUT, 
	@Area			VARCHAR(50)	OUTPUT,
	@PinCodeId		NUMERIC	OUTPUT, 
	@PinCode		VARCHAR(10)	OUTPUT,
	@Phone1		VARCHAR(50)	OUTPUT, 
	@Phone2		VARCHAR(50)	OUTPUT, 
	@Mobile		VARCHAR(50)	OUTPUT, 
	--@Password		VARCHAR(50)	OUTPUT, 
	@PrimaryPhone		TINYINT	OUTPUT, 
	@Industry		VARCHAR(100)	OUTPUT, 
	@Designation		VARCHAR(100)	OUTPUT, 
	@Organization		VARCHAR(100)	OUTPUT, 
	@DOB			DATETIME 	OUTPUT, 
	@ContactHours		VARCHAR(50)	OUTPUT, 
	@ContactMode		VARCHAR(50)	OUTPUT, 
	@CurrentVehicle	VARCHAR(100)	OUTPUT, 
	@InternetUsePlace	VARCHAR(50)	OUTPUT, 
	@CarwaleContact	VARCHAR(50)	OUTPUT, 
	@InternetUseTime	VARCHAR(50)	OUTPUT, 
	@ReceiveNewsletters	BIT 		OUTPUT, 
	@RegistrationDateTime	DATETIME 	OUTPUT,  
	@IsEmailVerified		BIT 		OUTPUT,  
	@IsVerified		BIT 		OUTPUT,
	@IsFake		BIT 		OUTPUT,
	@IsExist		BIT 		OUTPUT,
	@Comment		VARCHAR(1500)	OUTPUT
AS
	DECLARE 
		@CityVerified AS VARCHAR(100),
		@CityTemp AS VARCHAR(100)
BEGIN
		
	SET @IsExist = 0	
	SET @CityVerified = ''
	SET @CityTemp = ''
	--get the details from the customers table, assuming that the customer is verified	

	SELECT 
		@Name	 		= C.Name, 
		@Email			= C.Email, 
		@Address 		= C.Address, 
		@StateId		= IsNull(C.StateId, 0), 
		@State			= S.Name,
		@CityId			= IsNull(C.CityId, 0),  
		@CityVerified		= IsNull(CI.Name, ''),
		@AreaId		= IsNull(C.AreaId, 0),  
		@Area			= A.Name,
		@PinCodeId		= IsNull(C.AreaId, 0),  
		@PinCode		= A.PinCode,
		@Phone1		= C.Phone1, 
		--@Phone2		= C.Phone2, 
		@Mobile		= C.Mobile, 
		--@Password		= C.Password, 
		--@PrimaryPhone		= C.PrimaryPhone, 
		--@Industry		= C.Industry, 
		--@Designation		= C.Designation, 
		--@Organization		= C.Organization, 
		--@DOB			= C.DOB, 
		--@ContactHours		= C.ContactHours, 
		--@ContactMode		= C.ContactMode, 
		--@CurrentVehicle	= C.CurrentVehicle, 
		--@InternetUsePlace	= C.InternetUsePlace, 
		--@CarwaleContact	= C.CarwaleContact, 
		--@InternetUseTime	= C.InternetUseTime, 
		@ReceiveNewsletters	= C.ReceiveNewsletters,
		@RegistrationDateTime	= C.RegistrationDateTime,
		@IsEmailVerified		= C.IsEmailVerified,
		@IsVerified		= C.IsVerified,
		@IsFake		= C.IsFake
		--@Comment		= C.Comment				
	FROM 
		(((Customers  AS C  WITH (NOLOCK) LEFT JOIN Cities AS Ci  WITH (NOLOCK) ON Ci.ID = C.CityId)
		LEFT JOIN Areas AS A WITH (NOLOCK) ON A.ID = C.AreaId)
		LEFT JOIN States AS S WITH (NOLOCK) ON S.ID = C.StateId)	
		
	WHERE 
		C.ID = @CustomerId	

	IF @@ROWCOUNT > 0 
		SET @IsExist = 1		
	
	--IF @IsVerified = 0	--for new customer fetch the data from the temp customers table and overwrite the existing values
	--BEGIN
	--	--fetch the details of the customer from the TempCustomers table

	--	SELECT 
	--		@Name	 		= TC.Name, 
	--		@Email			= TC.Email, 
	--		@Address 		= TC.Address, 
	--		@StateId		= IsNull(TC.State, 0),  
	--		@State			= S.Name,
	--		@CityTemp		= IsNull(TC.City, ''),
	--		@Area			= TC.Area,
	--		@PinCode		= TC.PinCode,
	--		@Phone1		= CASE(@Phone1) WHEN '' THEN TC.Phone1 ELSE @Phone1 END, 
	--		@Phone2		= CASE(@Phone2) WHEN '' THEN TC.Phone2 ELSE @Phone2 END, 
	--		@Mobile		= CASE(@Mobile) WHEN '' THEN TC.Mobile ELSE @Mobile END, 
	--		@PrimaryPhone		= TC.PrimaryPhone, 
	--		@Industry		= TC.Industry, 
	--		@Designation		= TC.Designation, 
	--		@Organization		= TC.Organization, 
	--		@DOB			= TC.DOB, 
	--		@ContactHours		= TC.ContactHours, 
	--		@ContactMode		= TC.ContactMode, 
	--		@CurrentVehicle	= TC.CurrentVehicle, 
	--		@InternetUsePlace	= TC.InternetUsePlace, 
	--		@CarwaleContact	= TC.CarwaleContact, 
	--		@InternetUseTime	= TC.InternetUseTime, 
	--		@ReceiveNewsletters	= TC.ReceiveNewsletters,
	--		@RegistrationDateTime	= TC.RegistrationDateTime
	--	FROM 
	--		Customers AS C,
	--		(TempCustomers  AS TC LEFT JOIN States AS S ON S.ID = TC.State)	
	--	WHERE 
	--		C.ID 	= @CustomerId	AND 
	--		TC.ID	= C.TempId
		
	--	PRINT @StateId
	--	PRINT @DOB	
	--END

	IF @CityVerified = ''
		SET @City = @CityTemp
	ELSE
		SET @City = @CityVerified
END