IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ENTRYDEALERSTEMP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ENTRYDEALERSTEMP]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--THIS PROCEDURE INSERTS THE VALUES FOR THE FEALER REGISTRATION ITO A TEMP TABLE TEMPDEALERS
--Modified By : Vaibhav K (28-May-2012)
--				Adding two new field values UpdatedBy and Source in TEMPDEALERS
--Modifier	: 2. Vaibhav K (16-10-2012)
--			: Modified for DCRM data pooling as per parameters IsVerified & type
--			: If Verified insert data into DCRM_VerifiedDealerPool & delete from TempDealers if any
--			: Else Insert / Update Tempdealers as per type passed (1-insert & 2-update)
--			: 3.Sachin Bharti(22nd March 2013)To add entry regarding DealerType ISTcDealer
--Modified by: Kritika Choudhary on 31st Dec 2015, added ApplicationId as parameter and in the insert query for TempDealers and DCRM_VerifiedDealerPool

CREATE PROCEDURE [dbo].[ENTRYDEALERSTEMP]
	@LOGINID		VARCHAR(30) = NULL, 
	@FIRSTNAME		VARCHAR(50) = NULL, 
	@LASTNAME		VARCHAR(50) = NULL, 
	@EMAILID		VARCHAR(60) = NULL, 
	@ORGANIZATION	VARCHAR(60), 
	@ADDRESS1		VARCHAR(100) = NULL, 
	@ADDRESS2		VARCHAR(100) = NULL, 
	@AREAID		NUMERIC = NULL, 
	@CITYID			NUMERIC, 
	@STATEID		NUMERIC, 
	@PINCODE		VARCHAR(6) = NULL, 
	@PHONENO		VARCHAR(15) = NULL, 
	@FAXNO		VARCHAR(15) = NULL, 
	@MOBILENO		VARCHAR(15) = NULL, 
	@ENTRYDATE		DATETIME, 
	@WEBSITEURL	VARCHAR(50) = NULL, 
	@CONTACTPERSON	VARCHAR(60) = NULL, 
	@CONTACTHOURS	VARCHAR(20) = NULL, 
	@CONTACTEMAIL	VARCHAR(60)= NULL, 
	@LOGOURL		VARCHAR(100) = NULL,
	@DealerTypeId	INT = NULL ,-- usde to set what is the type of Dealer UCD,NCD or Both
	
	@StockCarCount	INT = NULL,-- how many cars do you have at your dealership at any point of time
	@SellingInMonth	INT = NULL,-- how many do you sell in a month
	@UsingPC		BIT, -- do u use computer at your dealership
	@UsingSoftware	BIT,-- do you use software to manage ur dealership
	@UsingTradingCars	BIT,-- want trading cars software
	@VerifyReason   INT = NULL ,-- used to tag Reason for Verify the Dealer
	@NotVerifyReason   INT = NULL ,-- used to tag Reason for  not to verify Dealer
	@TempDealerId 	NUMERIC OUTPUT,
	@EmailStatus   NUMERIC = NULL OUTPUT,-- used to check whether EmailId already exist for new entry 
	@MobileStatus   NUMERIC = NULL OUTPUT,-- used to check whether MobileNO already exist for new entry
	@AddDealerId   NUMERIC = NULL OUTPUT,-- used to get DealerId which is newly added by Direct register
	@UpdatedBy		NUMERIC(18,0) = -1,
	@Source			NUMERIC(18,0) = 1,
	
	--Parameters added by Vaibhav K (16-10-2012) for DCRM Sales data pooling (OPTIONAL)
	@IsVerified		BIT = 0,
	@Type			SMALLINT = 1,
	@TempId			INT = -1,
	@LeadSource		SMALLINT = -1,
	@ReferredBy		INT = -1,
	@ApplicationId INT=NULL
 AS

BEGIN
	SET @EmailStatus = -1 
	SET @MobileStatus = -1
	If @IsVerified = 0 AND @Type = 1 AND @EmailStatus = -1 AND @MobileStatus = -1	--If Dealer is not verified & New Enrty to be made
		BEGIN
			If @EMAILID <> ''
				BEGIN
					--Check status of EmailID is it already exist 
					SELECT @EmailStatus = ID FROM TempDealers WHERE EmailId = @EMAILID 
				END
			If @MOBILENO <> ''
				BEGIN
					--Check status of MobileNO is it already exist
					SELECT @MobileStatus = ID FROM TempDealers WHERE MobileNo = @MOBILENO
				END
			--If we have do not have any match for EmailId and MobileNO then new entry is done
			If @EmailStatus = -1 AND @MobileStatus = -1
				BEGIN
					--NOW INSERT THE DATA INTO THE TABLE
					INSERT INTO TEMPDEALERS( LOGINID, FIRSTNAME, 
						LASTNAME, EMAILID, ORGANIZATION, 
						ADDRESS1, ADDRESS2, AREAID, CITYID, 
						STATEID, PINCODE, PHONENO, 
						FAXNO, MOBILENO, ENTRYDATE, 
						WEBSITEURL, CONTACTPERSON, CONTACTHOURS, 
						CONTACTEMAIL, LOGOURL, ROLE,
						UpdatedBy,Source,LeadSource,ReferredBy,TC_DealerTypeId,ApplicationId)
					VALUES(@LOGINID, @FIRSTNAME, 
					@LASTNAME, @EMAILID, @ORGANIZATION, 
					@ADDRESS1, @ADDRESS2, @AREAID, @CITYID, 
					@STATEID, @PINCODE, @PHONENO, 
					@FAXNO, @MOBILENO, @ENTRYDATE, 
					@WEBSITEURL, @CONTACTPERSON, @CONTACTHOURS, 
					@CONTACTEMAIL, @LOGOURL, 'DEALERS',
					@UpdatedBy,@Source,@LeadSource,@ReferredBy,@DealerTypeId,@ApplicationId)

					SET @TempDealerId = SCOPE_IDENTITY()

					INSERT INTO TEMPDEALERDETAILS(DealerId, StockCarsCount, SellingInMonth, UsingPC, UsingSoftware, UsingTradingCars)

					VALUES(@TempDealerId,@StockCarCount, @SellingInMonth,@UsingPC,@UsingSoftware,	@UsingTradingCars)
				END
		END
	ELSE IF @IsVerified = 0 AND @Type = 2 	--IF Dealer is not verified & only data needs to be updated
		BEGIN
			If @EMAILID <> ''
				BEGIN
					--Check status of EmailID is it already exist 
					SELECT @EmailStatus = ID FROM TempDealers WHERE EmailId = @EMAILID AND Organization <> @ORGANIZATION
				END
			If @MOBILENO <> ''
				BEGIN
					--Check status of MobileNO is it already exist
					SELECT @MobileStatus = ID FROM TempDealers WHERE MobileNo = @MOBILENO AND Organization <> @ORGANIZATION
				END
			--If we have do not have any match for EmailId and MobileNO then new entry is done
			If @EmailStatus = -1 AND @MobileStatus = -1
				BEGIN
					UPDATE TEMPDEALERS 
						SET 
						ORGANIZATION = @ORGANIZATION, FIRSTNAME =	@FIRSTNAME, LASTNAME =@LASTNAME, EMAILID = @EMAILID, 
						ADDRESS1 = @ADDRESS1, AREAID = @AREAID, CITYID = @CITYID, STATEID	= @STATEID,
						PINCODE	= @PINCODE, PHONENO	= @PHONENO, FAXNO = @FAXNO, MOBILENO = @MOBILENO, CONTACTPERSON = @CONTACTPERSON, 
						CONTACTHOURS =	@CONTACTHOURS, CONTACTEMAIL = @CONTACTEMAIL,DeletedReason = @NotVerifyReason,
						LeadSource = @LeadSource, ReferredBy = @ReferredBy ,TC_DealerTypeId = @DealerTypeId
					WHERE
						ID = @TempId
						
					SET @TempDealerId = @TempId
				END
		END	
	ELSE IF @IsVerified = 1	 --If the dealer is Verified directly send data to DCRM_VerifiedDealerPool
		BEGIN
			If @EMAILID <> ''
				BEGIN
					--Check status of EmailID is it already exist 
					SELECT @EmailStatus = ID FROM Dealers WHERE EmailId = @EMAILID AND Organization <> @ORGANIZATION
				END
			If @MOBILENO <> ''
				BEGIN
					--Check status of MobileNO is it already exist
					SELECT @MobileStatus = ID FROM Dealers WHERE MobileNo = @MOBILENO AND Organization <> @ORGANIZATION
				END
			--If we have do not have any match for EmailId and MobileNO then new entry is done
			If @EmailStatus = -1 AND @MobileStatus = -1
				BEGIN
					INSERT INTO DCRM_VerifiedDealerPool
						(
							Organization,Name,EmailId,StateId,CityId,AreaId,
							Address1,Pincode,MobileNo,PhoneNo,FaxNo,EntryDate,
							ContactPerson,ContactHours,ContactEmail,VerifiedBy,VerifiedON,LeadSource,ReferredBy,DealerStatus,TC_DealerTypeId,ApplicationId
						)
					VALUES
						(
							@ORGANIZATION,@FIRSTNAME,@EMAILID,@STATEID,@CITYID,@AREAID,
							@ADDRESS1,@PINCODE,@MOBILENO,@PHONENO,@FAXNO,@ENTRYDATE,
							@CONTACTPERSON,@CONTACTHOURS,@CONTACTEMAIL,@UpdatedBy,GETDATE(),@LeadSource,@ReferredBy,@VerifyReason,@DealerTypeId,@ApplicationId
						)
					SET @TempDealerId = SCOPE_IDENTITY()
					UPDATE TempDealers SET IsDeleted = 1, VerificationPoolId = @TempDealerId WHERE ID = @TempId
					
					IF @Type = 3--If Dealer is added directly by the field Executive
						BEGIN
							DECLARE @PrevDealerId NUMERIC (18,0)
							
							SELECT Top 1 @PrevDealerId =  D.ID FROM Dealers D (NOLOCK) ORDER BY ID DESC
							
							EXECUTE DCRM_AssignVerifiedDealers @TempDealerId,-1,@UpdatedBy,@UpdatedBy,@ENTRYDATE
							
							SELECT Top 1 @AddDealerId =  D.ID FROM Dealers D (NOLOCK) ORDER BY ID DESC
							
							IF @PrevDealerId = @AddDealerId 
								BEGIN
									SET @AddDealerId = -1
								END
						END
					 
				END
		END
END


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
