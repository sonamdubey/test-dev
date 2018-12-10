IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ALLOWDEALERS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ALLOWDEALERS]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 23rd Dec,2011
-- Description:	Inserting IsTCDealer=1
-- =============================================
--THIS PROCEDURE FETCHES THE DATA FROM THE TEMPDEALERS TABLE AND INSERT THE RECORD INTO THE DEALERS TABLE, 
--ALONG WITH THE PASSWORD AND THE EXPIRY DATE

CREATE PROCEDURE [dbo].[ALLOWDEALERS]
	@TEMPID		NUMERIC,
	@PASSWD		VARCHAR(20), 
	@JOININGDATE	DATETIME,
	@EXPIRYDATE		DATETIME,
	@IsAutoFriendDealer	BIT,
	@EMAIL		VARCHAR(60)		OUTPUT,
	@NAME		VARCHAR(100)		OUTPUT,
	@ORG			VARCHAR(60)		OUTPUT,
	@DEALERID		NUMERIC 		OUTPUT
AS
	DECLARE

	
	@LOGINID		VARCHAR(30), 
	@FIRSTNAME		VARCHAR(50), 
	@LASTNAME		VARCHAR(50), 
	@EMAILID		VARCHAR(60), 
	@ORGANIZATION	VARCHAR(60), 
	@ADDRESS1		VARCHAR(100), 
	@ADDRESS2		VARCHAR(100), 
	@AREAID		NUMERIC, 
	@CITYID		NUMERIC, 
	@STATEID		NUMERIC, 
	@PINCODE		VARCHAR(6), 
	@PHONENO		VARCHAR(15), 
	@FAXNO		VARCHAR(15), 
	@MOBILENO		VARCHAR(15), 
	@WEBSITEURL	VARCHAR(50), 
	@CONTACTPERSON	VARCHAR(60), 
	@CONTACTHOURS	VARCHAR(20), 
	@CONTACTEMAIL	VARCHAR(60), 
	@LOGOURL		VARCHAR(100),
	@ROLE		VARCHAR(30),
	@CUSTOMERID	NUMERIC,
	
	@RowCount		NUMERIC,
	@StockCarsCount           INT,
	@SellingInMonth	INT,
	@UsingPC		BIT,
	@UsingSoftware	BIT, 
	@UsingTradingCars	BIT
		
BEGIN

	--GET VALLUES FRO THE TEMP TABLE

	SELECT @LOGINID = LoginId, @FIRSTNAME = FirstName, @LASTNAME = LastName, @EMAILID = EmailId, @ORGANIZATION =Organization,
		 @ADDRESS1 = Address1, @ADDRESS2 = Address2, @AREAID = AreaId, @CITYID = CityId, @STATEID = StateId, @PINCODE = Pincode, 
		 @PHONENO = PhoneNo, @FAXNO = FaxNo, @MOBILENO = MobileNo, @WEBSITEURL = WebsiteUrl, 
		 @CONTACTPERSON = ContactPerson, @CONTACTHOURS = ContactHours, @CONTACTEMAIL = ContactEmail, 
		@LOGOURL = LogoUrl, @ROLE = ROLE 
	FROM 	TempDealers 
	WHERE ID = @TEMPID

	--NOW INSERT THE DATA INTO THE TABLE
	INSERT INTO DEALERS( LoginId, Passwd, FirstName, LastName, 
				EmailId, Organization, Address1, Address2, AreaId,
				CityId, StateId, Pincode, PhoneNo, FaxNo, 
				MobileNo, JoiningDate, ExpiryDate, WebsiteUrl, 
				ContactPerson, ContactHours, ContactEmail, LogoUrl, ROLE, Status, Preference,IsTCDealer)
			VALUES(@LOGINID, @PASSWD, @FIRSTNAME, 
				@LASTNAME, @EMAILID, @ORGANIZATION, 
				@ADDRESS1, @ADDRESS2,@AREAID, @CITYID, 
				@STATEID, @PINCODE, @PHONENO, 
				@FAXNO, @MOBILENO, @JOININGDATE, @EXPIRYDATE, 
				@WEBSITEURL, @CONTACTPERSON, @CONTACTHOURS, 
				@CONTACTEMAIL, @LOGOURL, 'DEALERS', 0, 0,1)
	
	SET @DEALERID = SCOPE_IDENTITY()  
	
	--SET THE NAME OF THE ORGANIZATION AND NAME OF THE USER
	SET @EMAIL = @EMAILID
	SET @NAME = @FIRSTNAME + ' ' + @LASTNAME
	SET @ORG = @ORGANIZATION                                                                                                                                                                                                                                      
                                                                                                                                                  
	
	SELECT @CUSTOMERID = Id  FROM Customers WHERE Email =  @EMAIL
	SET @RowCount = @@RowCount 
	
	-- Check if customer is already registered with the email id dealer registered
	IF @RowCount =  0
		BEGIN
			--ALSO REGISTER DEALER AS A CUSTOMER
			INSERT INTO CUSTOMERS(Name, Email, RegistrationDateTime, IsVerified) Values(@NAME, @EMAIL, @JOININGDATE, 1) 
			
			--GET THE CUSTOMER ID
			SET @CUSTOMERID = SCOPE_IDENTITY()  
		END
	

	
	--CHECK IF CUSTOMER IS ALREADY MAPPED
	SELECT * FROM MAPDEALERS WHERE DealerId = @DEALERID 
	
	IF @@RowCount = 0
		BEGIN
			--MAP DEALER TO CUSTOMER
			INSERT INTO MAPDEALERS(DealerId, CustomerId) VALUES(@DEALERID, @CUSTOMERID)
		END
	
	
	-- FETCH DATA FROM  TempDealerDetails & Insert that data into DealerDetails with Orginal DealerId

	--Fetching Data
	SELECT @StockCarsCount = StockCarsCount, @SellingInMonth = SellingInMonth, @UsingPC = UsingPC, 
		@UsingSoftware = UsingSoftware, @UsingTradingCars = UsingTradingCars
	 FROM TempDealerDetails WHERE DealerId = @TEMPID
	
	--Inserting Data
	INSERT INTO DEALERDETAILS(DealerId, StockCarsCount, SellingInMonth, UsingPC, UsingSoftware, UsingTradingCars)
	VALUES(@DEALERID,@StockCarsCount, @SellingInMonth,@UsingPC,@UsingSoftware,@UsingTradingCars)


	IF @IsAutoFriendDealer = 1
	BEGIN
		SELECT CarwaleDealerId FROM AutoFriendDealerMap WHERE  CarwaleDealerId = @DEALERID OR Lower(AutoFriendDealer) = Lower(@ORGANIZATION)
		
		IF @@RowCount = 0
		BEGIN
			INSERT INTO AutoFriendDealerMap(AutoFriendDealer, CarwaleDealerId, IsActive ) VALUES(@ORGANIZATION, @DEALERID, 1)
		END
	END

	--after inserting the record successfully delete the entry from the tempdealers table
	DELETE FROM TEMPDEALERS WHERE ID = @TEMPID
END

