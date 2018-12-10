IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MapAutofriendDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MapAutofriendDealer]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE FEALER REGISTRATION ITO A TEMP TABLE TEMPDEALERS

CREATE PROCEDURE [MapAutofriendDealer]
	@LOGINID		VARCHAR(30), 
	@PASSWD		VARCHAR(20),
	@FIRSTNAME		VARCHAR(50), 
	@LASTNAME		VARCHAR(50), 
	@EMAILID		VARCHAR(60), 
	@ORGANIZATION	VARCHAR(60), 
	@ADDRESS1		VARCHAR(100), 
	@ADDRESS2		VARCHAR(100), 
	@AREAID		NUMERIC, 
	@CITYID		NUMERIC, 
	@STATEID		NUMERIC, 
	@PHONENO		VARCHAR(15), 
	@PINCODE		VARCHAR(6), 
	@FAXNO		VARCHAR(15), 
	@MOBILENO		VARCHAR(15), 
	@JOININGDATE	DATETIME, 
	@EXPIRYDATE		DATETIME, 
	@WEBSITEURL	VARCHAR(50), 
	@CONTACTPERSON	VARCHAR(60), 
	@CONTACTHOURS	VARCHAR(20), 
	@CONTACTEMAIL	VARCHAR(60),
	@STATUS		BIT,
	@DealerId		NUMERIC OUTPUT
	
 AS
	
BEGIN
	INSERT INTO 
		DEALERS
			( 	LoginId, 		Passwd, 	FirstName, 	LastName, 		EmailId, 	
				Organization, 		Address1, 	Address2, 	AreaId,			CityId, 		
				StateId, 		Pincode, 	PhoneNo, 	FaxNo, 			MobileNo, 	
				JoiningDate, 		ExpiryDate, 	WebsiteUrl, 	ContactPerson, 		ContactHours, 
				ContactEmail, 		LogoUrl, 	ROLE, 		Status, 			Preference
			)
			VALUES
			(	@LOGINID, 		@PASSWD,		@FIRSTNAME, 		@LASTNAME, 		@EMAILID, 
				@ORGANIZATION, 	@ADDRESS1, 		@ADDRESS2, 		@AREAID, 		@CITYID, 
				@STATEID, 		@PINCODE, 		@PHONENO, 		@FAXNO, 		@MOBILENO, 
				@JOININGDATE,	@EXPIRYDATE, 	@WEBSITEURL, 	@CONTACTPERSON, 	@CONTACTHOURS, 	
				@CONTACTEMAIL,	'',			'DEALERS',		@STATUS,		0
			)
	
	SET @DealerId = SCOPE_IDENTITY()
	
	--now update the preferences
	INSERT INTO DealerSettings (DealerId, MaxSellInquiries, MaxPurchaseInquiries)  VALUES (@DealerId, 1000,1000)
	
END
