IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_AddDealerForDealerLocator]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_AddDealerForDealerLocator]
GO

	

-- ==============================================================================================
-- Author	:	Sachin Bharti(20th Aug 2014)
-- Description	:	Add Dealer details for DealerLocator
--					@DealerType -	1 : Add New entry 
--									2 : Update existing entry present in Dealer_NewCar table
--					@IsUpdated	-	1 : New Car Dealer added successfully.
--									2 :	Both TC_Dealer and NewCarDealer mapped successfully
--								   -1 : Problem in addtion or updation
-- Modifier	:	Sachin Bharti(2nd Feb 2014)
-- Modification	:	Update dealer mobile no in 	Dealer_NewCar.PrimaryMobileNo column also
-- EXEC [dbo].[NCD_AddDealerForDealerLocator] 2,94,9,'CAR POINT S.N.',1,1,1,'152 SAROJINI NAGAR',110001,'forward.carwale@gmail.com','9029291922',7,null,null
-- ===============================================================================================
CREATE PROCEDURE [dbo].[NCD_AddDealerForDealerLocator]
	@DealerType		INT,--1 - New Car Dealer is not mapped with TC_dealerId, 2 - New Car Dealer is already mapped with TC_dealerId
	@TC_DealerID	INT = NULL,
	@NCD_DealerID	INT = NULL,
	@DealerName		VARCHAR(200),
	@DealerStateId	INT,
	@DealerCityId	INT,
	@DealerAreaId	INT,
	@DealerAddress	VARCHAR(500),
	@DealerPincode	VARCHAR(6),
	@DealerEmail	VARCHAR(500),
	@DealerMobile	VARCHAR(200),
	@DealerMakeId	INT,
	@IsUpdated		INT OUTPUT,
	@NewNCD_DealerID	INT OUTPUT -- Dealer_NewCar Id after updation  or new entry
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE	@CustomerID		INT = NULL
	SET @IsUpdated = -1

	--If New Car Dealer is added for DealerLocator Page
	IF @DealerType = 1 AND @NCD_DealerID IS NULL 
	BEGIN
	
		--Check availaibility in Dealers table
		SELECT ID FROM Dealers D(NOLOCK) WHERE D.ID = @TC_DealerID 
		IF @@ROWCOUNT <> 0
		BEGIN


			--Add New entry in Dealer_NewCar
			INSERT INTO Dealer_NewCar	(	Name,Address,CityId,Pincode,EMailId,DealerMobileNo,PrimaryMobileNo,IsNewDealer,TcDealerId,MakeId,IsCampaignActive)
					VALUES				(	@DealerName , @DealerAddress,@DealerCityId,@DealerPincode,@DealerEmail,@DealerMobile,@DealerMobile,1,@TC_DealerID,@DealerMakeId,0)

			SET @NewNCD_DealerID = SCOPE_IDENTITY()

			--Now add entry for Dealer - Make mapping
			SELECT *FROM TC_DealerMakes WHERE DealerId = @TC_DealerID AND MakeId = @DealerMakeId
			IF @@ROWCOUNT = 0
			BEGIN
				INSERT INTO TC_DealerMakes(DealerId,makeId)VALUES(@TC_DealerID,@DealerMakeId)  	
			END

			-- Check if customer is already registered with the email id dealer registered  
			SELECT @CUSTOMERID = Id  FROM Customers WHERE Email =  @DealerEmail  
			IF @@RowCount =  0  
			BEGIN  
				--ALSO REGISTER DEALER AS A CUSTOMER  
				INSERT INTO CUSTOMERS(Name, Email, RegistrationDateTime, IsVerified) 
							Values	 (@DealerName , @DealerEmail, GETDATE(), 1)   
     
				--GET THE CUSTOMER ID  
				SET @CustomerID = SCOPE_IDENTITY()    
			END  
    
			--CHECK IF CUSTOMER IS ALREADY MAPPED  
			SELECT * FROM MAPDEALERS WHERE DealerId = @TC_DealerID
			IF @@RowCount = 0  
			BEGIN  
				--MAP DEALER TO CUSTOMER  
				INSERT INTO MAPDEALERS(DealerId, CustomerId) VALUES(@TC_DealerID, @CUSTOMERID)  
			END  
			SET @IsUpdated = 1
		END
	
	END

	--If Dealer is existing and need to update
	IF @DealerType = 2 AND @NCD_DealerID IS NOT NULL
	BEGIN
		
		--Check availaibility in Dealer_NewCar table
		SELECT ID FROM Dealer_NewCar DN(NOLOCK) WHERE DN.Id = @NCD_DealerID
		IF @@ROWCOUNT <> 0
			BEGIN
				UPDATE Dealers SET	FirstName = @DealerName , Organization = @DealerName , Address1 = @DealerAddress,
									StateId = @DealerStateId , CityId = @DealerCityId , AreaId = @DealerAreaId ,
									Pincode = @DealerPincode , MobileNo = @DealerMobile , EmailId = @DealerEmail
				WHERE ID = @TC_DealerID

				UPDATE Dealer_NewCar SET 
										Name = @DealerName ,
										Address = @DealerAddress,
										CityId = @DealerCityId ,
										Pincode = @DealerPincode ,
										EMailId = @DealerEmail ,
										DealerMobileNo = @DealerMobile,
										PrimaryMobileNo = @DealerMobile,
										IsNewDealer = 1 ,
										TcDealerId = @TC_DealerID ,
										MakeId = @DealerMakeId
						WHERE Id = @NCD_DealerID
				SET @NewNCD_DealerID = @NCD_DealerID
				--Now add entry for Dealer - Make mapping
				SELECT *FROM TC_DealerMakes WHERE DealerId = @TC_DealerID AND MakeId = @DealerMakeId
				IF @@ROWCOUNT = 0
				BEGIN
					INSERT INTO TC_DealerMakes(DealerId,makeId)VALUES(@TC_DealerID,@DealerMakeId)  	
				END

				-- Check if customer is already registered with the email id dealer registered  
				SELECT @CUSTOMERID = Id  FROM Customers WHERE Email =  @DealerEmail  
				IF @@RowCount =  0  
					BEGIN  
						--ALSO REGISTER DEALER AS A CUSTOMER  
						INSERT INTO CUSTOMERS(Name, Email, RegistrationDateTime, IsVerified) 
									Values	 (@DealerName , @DealerEmail, GETDATE(), 1)   
     
						 --GET THE CUSTOMER ID  
						SET @CustomerID = SCOPE_IDENTITY()    
					END  
    
				--CHECK IF CUSTOMER IS ALREADY MAPPED  
				SELECT * FROM MAPDEALERS WHERE DealerId = @TC_DealerID
				IF @@RowCount = 0  
					BEGIN  
						--MAP DEALER TO CUSTOMER  
						INSERT INTO MAPDEALERS(DealerId, CustomerId) VALUES(@TC_DealerID, @CUSTOMERID)  
					END  
				SET @IsUpdated = 2
			END
	END

END






