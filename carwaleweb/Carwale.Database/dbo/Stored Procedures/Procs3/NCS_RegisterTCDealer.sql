IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_RegisterTCDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_RegisterTCDealer]
GO

	-- =============================================  
-- Author:  Vaibhav K  
-- Create date: 17-Aug-2012  
-- Description: To register a ncd dealer as TC dealer and keep log in table NCS_TCDealerLog  
-- Modified by:  Nilesh Utture  
-- Modified date: 3-October-2012  
-- Description:  Added parameters @Type, @IsDuplicate and @NCSId in order to check from which page the dealer is registered,   
--     to avoide duplicate entry in Dealers Table and If dealer is NCS dealer add TCDealerId to NCS_dealers table respectively  
--Modified By : Vinay Kumar Prajapati , 7th Nov 2014 Add ApplicationType (1-Carwale, 2- BikeWale) and AreaId 
-- Modified By : Suresh Prajapati on 24th Nov, 2015
-- Description : To Save OprUser and Branch mapping
-- Modified By : Suresh Prajapati on 11th Dec
-- Description : To Make CW Field Executive Dealer(i.e. DealerTypeId = 6) Multiotlet
-- =============================================  
CREATE PROCEDURE [dbo].[NCS_RegisterTCDealer]
	-- Add the parameters for the stored procedure here  
	@ApplicationType TINYINT
	,@Name VARCHAR(200)
	,@Password VARCHAR(50)
	,@CityId NUMERIC
	,@AreaId NUMERIC
	,@LandlineNo VARCHAR(50)
	,@Mobile VARCHAR(50)
	,@ContactPerson VARCHAR(50)
	,@Address VARCHAR(500)
	,@EMail VARCHAR(250)
	,@RegEMail VARCHAR(100)
	,@EntryDateTime DATETIME
	,@Status BIT
	,@NCDId NUMERIC
	,@TCDealerId NUMERIC OUTPUT
	,@UpdatedBy NUMERIC = - 1
	,@UpdatedOn DATETIME = GETDATE
	,@IsDuplicate INT OUTPUT
	,@MakeId VARCHAR(500)
	,@DealerType SMALLINT
	,@Type SMALLINT = NULL
	,@PaidDealer BIT = 0
	,@OprUserId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT ON;

	DECLARE @CUSTOMERID NUMERIC
		,@STATEID INT = - 1
		,@Pincode VARCHAR(6) = ''
		,@NCSId NUMERIC(18, 0)
		,@ISNCSDealer BIT

	IF @Type = 1
		SET @ISNCSDealer = 1
	ELSE
		SET @ISNCSDealer = 0

	SET @TCDealerId = - 1

	--Get stateId fromm cityId  
	SELECT @STATEID = StateId
		,@Pincode = DefaultPinCode
	FROM Cities WITH (NOLOCK)
	WHERE ID = @CityId

	SELECT @TCDealerId = ID
	FROM Dealers WITH (NOLOCK)
	WHERE EmailId = @RegEMail -- Modified by: Nilesh Utture on 3-October-2012  

	IF @@RowCount = 0 -- If No dealer registered with this emailId
	BEGIN
		--Insert the entry for the dealer & mark as TCDealer  
		INSERT INTO Dealers (
			LoginId
			,Passwd
			,FirstName
			,LastName
			,EmailId
			,Organization
			,Address1
			,Address2
			,AreaId
			,CityId
			,StateId
			,Pincode
			,PhoneNo
			,FaxNo
			,MobileNo
			,JoiningDate
			,ExpiryDate
			,WebsiteUrl
			,ContactPerson
			,ContactHours
			,ContactEmail
			,LogoUrl
			,ROLE
			,STATUS
			,Preference
			,IsTCDealer
			,DealerSource
			,TC_DealerTypeId
			,IsNcs
			,RegDealerId
			,PaidDealer
			,ApplicationId
			,IsMultiOutlet
			)
		VALUES (
			'deepak.t'
			,@Password
			,@Name
			,''
			,@RegEMail
			,@Name
			,@Address
			,''
			,@AreaId
			,@CITYID
			,@STATEID
			,@Pincode
			,@LandlineNo
			,NULL
			,@Mobile
			,@EntryDateTime
			,NULL
			,NULL
			,@CONTACTPERSON
			,'10-7'
			,NULL
			,''
			,'DEALERS'
			,@STATUS
			,0
			,1
			,31
			,@DealerType
			,@ISNCSDealer
			,@NCDId
			,@PaidDealer
			,@ApplicationType
			,CASE ISNULL(@DealerType, 0)
				WHEN 6
					THEN 1
				ELSE 0
				END
			)

		SET @TCDealerId = SCOPE_IDENTITY()

		UPDATE Dealers
		SET EmailId = @EMail
		WHERE ID = @TCDealerId

		IF (ISNULL(@DealerType, 0) = 6)
		BEGIN
			INSERT INTO TC_CWExecutiveMapping (
				OprUserId
				,BranchId
				)
			VALUES (
				@OprUserId
				,@TCDealerId
				)
		END

		-- Added By Vinay Kumar Prajapati 7th Nov 2014
		-- DO City Mapping Against Dealership 
		IF @CityId IS NOT NULL
		BEGIN
			INSERT INTO TC_DealerCities (
				DealerId
				,CityId
				,IsActive
				)
			VALUES (
				@TCDealerId
				,@CityId
				,1
				)
		END

		-- DO Make Mapping Against Dealership 
		IF @MakeId <> ''
			AND @MakeId IS NOT NULL
		BEGIN
			DECLARE @idString VARCHAR(MAX)

			SET @idString = @MakeId + ','

			WHILE CHARINDEX(',', @idString) > 0
			BEGIN
				DECLARE @tmpstr VARCHAR(50)

				SET @tmpstr = SUBSTRING(@idString, 1, (CHARINDEX(',', @idString) - 1))

				INSERT INTO TC_DealerMakes (
					DealerId
					,makeId
					)
				VALUES (
					@TCDealerId
					,@tmpstr
					)

				SET @idString = SUBSTRING(@idString, CHARINDEX(',', @idString) + 1, LEN(@idString))
			END
		END

		SET @IsDuplicate = 0
	END
	ELSE
	BEGIN
		SET @IsDuplicate = 1 -- Modified by: Nilesh Utture on 3-October-2012  
	END

	SELECT @CUSTOMERID = Id
	FROM Customers WITH (NOLOCK)
	WHERE Email = @RegEMail

	-- Check if customer is already registered with the email id dealer registered  
	IF @@RowCount = 0
	BEGIN
		--ALSO REGISTER DEALER AS A CUSTOMER  
		INSERT INTO CUSTOMERS (
			NAME
			,Email
			,RegistrationDateTime
			,IsVerified
			)
		VALUES (
			@Name + ' ' + ''
			,@RegEMail
			,@EntryDateTime
			,1
			)

		--GET THE CUSTOMER ID  
		SET @CUSTOMERID = SCOPE_IDENTITY()
	END

	--CHECK IF CUSTOMER IS ALREADY MAPPED  
	SELECT *
	FROM MAPDEALERS WITH (NOLOCK)
	WHERE DealerId = @TCDealerId

	IF @@RowCount = 0
	BEGIN
		--MAP DEALER TO CUSTOMER  
		INSERT INTO MAPDEALERS (
			DealerId
			,CustomerId
			)
		VALUES (
			@TCDealerId
			,@CUSTOMERID
			)
	END

	--Update NCD dealer with its TCDealerId  
	IF @TCDealerId <> - 1
	BEGIN
		--IF (@Type = 2) -- Modified by: Nilesh Utture on 3-October-2012  
		--BEGIN
		--	UPDATE Dealer_NewCar
		--	SET TcDealerId = @TCDealerId
		--	WHERE Id = @NCDId
		--END
		--ELSE
		IF (@Type = 1)
		BEGIN
			UPDATE NCS_Dealers
			SET TcDealerId = @TCDealerId
			WHERE Id = @NCDId

			UPDATE Dealers
			SET IsNcs = 1
				,RegDealerId = @NCDId
			WHERE Id = @TCDealerId
		END
	END
END


