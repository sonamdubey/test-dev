CREATE TABLE [dbo].[CustomerSellInquiries] (
    [ID]                    NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]            NUMERIC (18)   NOT NULL,
    [CityId]                INT            NULL,
    [CarVersionId]          NUMERIC (18)   NOT NULL,
    [CarRegNo]              VARCHAR (50)   NULL,
    [EntryDate]             DATETIME       NOT NULL,
    [Price]                 DECIMAL (18)   NOT NULL,
    [MakeYear]              DATETIME       NOT NULL,
    [Kilometers]            NUMERIC (18)   NULL,
    [Color]                 VARCHAR (100)  NULL,
    [ColorCode]             VARCHAR (6)    NULL,
    [Comments]              VARCHAR (500)  NULL,
    [IsArchived]            BIT            CONSTRAINT [DF_CustomerSellInquiries_IsArchived] DEFAULT ((0)) NULL,
    [IsApproved]            BIT            CONSTRAINT [DF_CustomerSellInquiries_IsApproved] DEFAULT ((0)) NOT NULL,
    [ForwardDealers]        BIT            CONSTRAINT [DF_CustomerSellInquiries_ForwardDealers] DEFAULT ((1)) NOT NULL,
    [ListInClassifieds]     BIT            CONSTRAINT [DF_CustomerSellInquiries_ListInClassifieds] DEFAULT ((1)) NOT NULL,
    [IsFake]                BIT            CONSTRAINT [DF_CustomerSellInquiries_IsFake] DEFAULT ((0)) NULL,
    [StatusId]              SMALLINT       CONSTRAINT [DF_CustomerSellInquiries_StatusId] DEFAULT ((1)) NULL,
    [LastBidDate]           DATETIME       NULL,
    [ClassifiedExpiryDate]  DATETIME       NULL,
    [ViewCount]             NUMERIC (18)   NULL,
    [PaidInqLeft]           INT            CONSTRAINT [DF_CustomerSellInquiries_PaidInqLeft] DEFAULT ((0)) NOT NULL,
    [FreeInqLeft]           INT            CONSTRAINT [DF_CustomerSellInquiries_FreeInqLeft] DEFAULT ((0)) NOT NULL,
    [PackageType]           SMALLINT       CONSTRAINT [DF_CustomerSellInquiries_PackageType] DEFAULT ((1)) NOT NULL,
    [PackageExpiryDate]     DATETIME       NULL,
    [SourceId]              SMALLINT       CONSTRAINT [DF_CustomerSellInquiries_SourceId] DEFAULT ((1)) NOT NULL,
    [IsVerified]            BIT            CONSTRAINT [DF_CustomerSellInquiries_IsVerified] DEFAULT ((0)) NULL,
    [CarRegState]           CHAR (4)       NULL,
    [PinCode]               INT            NULL,
    [Progress]              INT            CONSTRAINT [DF_CustomerSellInquiries_Progress] DEFAULT ((50)) NULL,
    [ReasonForSelling]      VARCHAR (500)  NULL,
    [PackageId]             INT            NULL,
    [Referrer]              NVARCHAR (100) NULL,
    [IPAddress]             VARCHAR (150)  NULL,
    [IsPremium]             BIT            DEFAULT ((0)) NULL,
    [CustomerName]          VARCHAR (100)  NULL,
    [CustomerEmail]         VARCHAR (100)  NULL,
    [CustomerMobile]        VARCHAR (20)   NULL,
    [IsListingCompleted]    BIT            DEFAULT ((0)) NULL,
    [CurrentStep]           TINYINT        DEFAULT ((0)) NULL,
    [ListingCompletedDate]  DATETIME       NULL,
    [PaymentMode]           TINYINT        DEFAULT ((2)) NULL,
    [UsedCarMasterColorsId] SMALLINT       NULL,
    [AreaId]                NUMERIC (18)   NULL,
    [IsShowContact]         BIT            NULL,
    [ShareToCT]             BIT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CustomerSellInquiries] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_CustomerSellInquiries_EntDt]
    ON [dbo].[CustomerSellInquiries]([EntryDate] ASC)
    INCLUDE([ID], [IsApproved], [IsFake], [PackageType], [IsVerified]);


GO
CREATE NONCLUSTERED INDEX [IX_CarVersionId_CustomerSellInquiries]
    ON [dbo].[CustomerSellInquiries]([CarVersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CustomerSellInquiries__CustomerId__StatusId__PackageType]
    ON [dbo].[CustomerSellInquiries]([CustomerId] ASC, [StatusId] ASC, [PackageType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CustomerSellInquiries_CustomerMobile]
    ON [dbo].[CustomerSellInquiries]([CustomerMobile] ASC);


GO
-- =============================================
-- Author:		<Rajeev>
-- Create date: <3/10/08>
-- Description:	<For updating data of Live stock>
-- =============================================
CREATE TRIGGER [dbo].[TrigInsertVerifiedCarData] 
   ON  [dbo].[CustomerSellInquiries]
   FOR INSERT
AS 
DECLARE
	@ProfileId			AS VARCHAR(50), 
	@VerifiedData		AS BIT,
	@NoOfRows			AS SMALLINT,
	@NoOfRows1			AS SMALLINT,
	@PackageType		AS INT,
	@IsVerified			AS BIT
	
BEGIN
	
	SET @VerifiedData = 1

	SELECT	@ProfileId = I.Id,
			@PackageType = I.PackageType,
			@IsVerified = IsNull(I.IsVerified, 0)
	FROM Inserted AS I
	

	
	--SET @NoOfRows = @@ROWCOUNT
	--PRINT 'IsApproved'
	--PRINT @NoOfRows
	--PRINT 'IsApproved Finishes'
	
	SET @NoOfRows = 1
			
	IF @NoOfRows = 1
		BEGIN
			IF(UPDATE(PackageType))	
				BEGIN
					IF (@PackageType = 2)
					BEGIN
						SET @VerifiedData = 1
					END
				END
														
			IF @VerifiedData = 1
				BEGIN
					SELECT Status FROM AP_VerifiedSellInq WHERE SellInqId = @ProfileId
					SET @NoOfRows1 = @@ROWCOUNT
					IF @NoOfRows1 <> 0
						BEGIN
							UPDATE AP_VerifiedSellInq 
							SET Status = 1, VerificationDate = GETDATE()
							WHERE SellInqId = @ProfileId
						END
					ELSE
						BEGIN
							INSERT INTO AP_VerifiedSellInq(SellInqId, VerificationDate, Status)
							VALUES(@ProfileId, GETDATE(), 1)
						END
				END
				
		END
		
		PRINT 'IsApproved final'
END

GO
-- =============================================
-- Author:		<Rajeev>
-- Create date: <3/10/08>
-- Description:	<For updating data of Live stock>
-- Avishkar 14-08-2012 Modified to set Area name in Livelistings
-- Manish 16-11-2012 Modified for selecting used car score prametres
--Modified by Reshma Shetty(16-1-2013)removed parameters fuel type and owners since no longer used in calculation
--Modified by Reshma Shetty(03-05-2013)Added parameter owners to update the newly introduced Owners field in LiveListing
--Modified by amit verma (10/31/2013) added check for packagetype != 1
-- Added by Avishkar 14-11-2013 to set IsPremium colum 	in CustomerSellInquiries
-- Added by Avishkar 22-11-2013 to set to set N/A if Owners is NULL CASE isnull(CSI.Owners,'-1')
--Modified by Avishkar (30-11-2013) Used package type 30 and 31 to set showdetails		
--Modified by Aditi dhaybar on 3/2/15 to get the AreaId and AreaName
-- Added column SellerName,SellerContact By Sadhana Upadhyay on 20 Apr 2015	
--Modified by Manish on 30 June 2015 added parameter Response on ll_updatelisting SP and commented print statement
-- Modified by Supriya Bhide on (22-1-2016) added parameter PackageId on ll_updatelisting
-- =============================================
CREATE TRIGGER [dbo].[TrigUpdateCustCarData] 

   ON  [dbo].[CustomerSellInquiries]
   FOR INSERT,UPDATE
AS 
DECLARE
	@ProfileId		AS VARCHAR(50), 
	@SellerType		AS SMALLINT, 
	@Seller			AS VARCHAR(50), 
	@Inquiryid		AS NUMERIC, 
	@MakeId			AS NUMERIC, 
	@MakeName		AS VARCHAR(100), 
	@ModelId		AS NUMERIC, 
	@ModelName		AS VARCHAR(100),
	@VersionId		AS NUMERIC, 
	@VersionName	AS VARCHAR(100),
	@StateId		AS NUMERIC, 
	@StateName		AS VARCHAR(100),
	@CityId			AS NUMERIC, 
	@CityName		AS VARCHAR(100),
	@AreaId			AS NUMERIC, 
	@AreaName		AS VARCHAR(100),
	@Lattitude		AS DECIMAL(18,4), 
	@Longitude		AS DECIMAL(18,4), 
	@MakeYear		AS DATETIME, 
	@Price			AS NUMERIC, 
	@Kilometers		AS NUMERIC, 
	@Color			AS VARCHAR(100),
	@Comments		AS VARCHAR(500),
	@EntryDate		AS DATETIME, 
	@LastUpdated	AS DATETIME,
	@PackageId		AS SMALLINT,-- Added by Supriya Bhide(22-1-2016)
	@PackageType	AS SMALLINT, 
	@ShowDetails	AS BIT, 
	@Priority		AS SMALLINT,
	@IsApproved		AS BIT,
	@IsFake			AS BIT,
	@StatusId		AS SMALLINT,
	@ClassifiedExpiryDate AS DATETIME,
	@ViewCountP		AS NUMERIC,	
	@DataCanBeUpdated	AS BIT,
	@NoOfRows		AS NUMERIC,
	@NextRowId		AS NUMERIC,
	@CurRowId		AS NUMERIC,
	@LoopIndex		AS NUMERIC,
	@AdditionalFuel AS VARCHAR(50),
	@Owners         AS VARCHAR(20),
	@IsListingCompleted AS BIT, -- Added By Avishkar(13-11-2013)  so that if Listing Completed is true then only car will be uploaded to Livelisting.
	@DealerId INT =NULL,
	@IsPremium BIT =0 ,-- Added by Avishkar 14-11-2013 for @IsPremium
	@VideoCount TINYINT =0 , --Modified By Reshma Shetty 27/12/2013 To update the VideoCount if already present in the offline stocks
	-- Added By : Sadhana Upadhyay on 20 Apr 2015
	@SellerName AS VARCHAR(100),
	@SellerContact AS VARCHAR(20)
	--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
	--@FUELTYPE       as DECIMAL(1),-------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
	--@owners        as  VARCHAR(10)   -------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
BEGIN
	SET @NoOfRows = @@ROWCOUNT
	
	set @NoOfRows = (select count(*) from Inserted)

	--PRINT @NoOfRows

	IF @NoOfRows = 1
	BEGIN
		SET @DataCanBeUpdated = 1

		SELECT @ViewCountP = IsNull(I.ViewCount, 0) FROM Inserted AS I
		
		IF(UPDATE(ViewCount))	
		BEGIN
			SET @DataCanBeUpdated = 0
		END

		IF @ViewCountP = 0
		BEGIN
			SET @DataCanBeUpdated = 1
		END

		IF @DataCanBeUpdated = 1
		BEGIN
			SELECT	
				@ProfileId = 'S' + CAST(I.ID AS VarChar(50)),	@SellerType = 2,		@Seller = 'Individual',
				@InquiryId = I.ID,		@MakeId = Ma.ID,			@MakeName = Ma.Name,	@ModelId = Mo.ID, 
				@ModelName = Mo.Name, 	@VersionId = Cv.ID,			@VersionName = Cv.Name,	@StateId = S.Id,
				@StateName = S.Name,	@CityId = C.Id,				@CityName = C.Name,		@AreaId = A.ID,
				@AreaName = A.Name,			@Lattitude = C.Lattitude,	@Longitude = C.Longitude,
				@MakeYear = I.MakeYear,	@Price = I.Price,			@Kilometers = I.Kilometers, 
				@Color = I.Color,		@Comments = I.Comments,		@EntryDate = I.EntryDate,
				@LastUpdated = GETDATE(),
				@PackageId = I.PackageId,-- Added by Supriya Bhide(22-1-2016)
				@PackageType = I.PackageType,	
				--@ShowDetails = (CASE I.PackageType WHEN 2 THEN 1 ELSE 0 END), --Modified by Avishkar (30-11-2013) Used package type 30 and 31 to set showdetails		
				@ShowDetails = (CASE I.PackageType WHEN 30 THEN 1  WHEN 31 THEN 1 ELSE 0 END), 
				@Priority = (CASE I.PackageType WHEN 2 THEN 1 WHEN 1 THEN 5 END),
				@IsApproved = I.IsApproved, @IsFake = I.IsFake, @StatusId = I.StatusId, 
				@ClassifiedExpiryDate = I.ClassifiedExpiryDate,@AdditionalFuel=CSI.AdditionalFuel,
				@IsListingCompleted=I.IsListingCompleted, -- Added By Avishkar(13-11-2013)  so that if Listing Completed is true then only car will be uploaded to Livelisting.
				@Owners=(CASE ISNULL(CSI.Owners,'-1')
							WHEN '1'
								THEN 'First Owner '
							WHEN '2'
								THEN 'Second Owner '
							WHEN '3'
								THEN 'Third Owner '
							WHEN '4'
								THEN 'Fourth Owner'
							WHEN '-1'
								THEN 'N/A'							
							ELSE
								 'More than 4 owners'
							END
						),
			    @IsPremium= I.IsPremium, --Modified by Avishkar (15-11-2013) Added @IsPremium to set IsPremium column in LiveListing
				--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
				--@FUELTYPE=CV.carfueltype, ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
				--@owners=CSI.OWNERS  ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
				@VideoCount=CSI.IsYouTubeVideoApproved,--Modified By Reshma Shetty 27/12/2013 To update the VideoCount if already present in the offline stocks
				-- Added By Sadhana Upadhyay on 20 Apr 2015 To add Seller Name and contact no. in livelisting Table
				@SellerName = I.CustomerName,
				@SellerContact =  (
								CASE 
									WHEN I.IsShowContact = 1
										THEN I.CustomerMobile
									ELSE ''
									END
								)
			FROM 
				CarMakes AS Ma, CarModels AS Mo, CarVersions AS Cv, 
				((Inserted AS I LEFT JOIN Cities AS C ON C.ID = I.CityId)
				LEFT JOIN States AS S ON S.ID = C.StateId
				LEFT JOIN Areas AS A ON A.ID = I.AreaId)--Modified by Aditi dhaybar on 3/2/15 to get the AreaId and AreaName
				JOIN CustomerSellInquiryDetails as CSI on CSI.InquiryId=I.ID
			WHERE
				Cv.ID = I.CarVersionId AND Mo.Id = Cv.CarModelId AND Ma.ID = Mo.CarMakeId

			--check for the fields isapproved, isfake, statusid and the classifiedexpirydate
			IF @IsApproved = 1 AND @IsFake = 0 AND @StatusId = 1 AND 
						(
							@ClassifiedExpiryDate >= CONVERT(varchar, GETDATE(), 101) 
							AND @PackageType != 1 ----Modified by amit verma (10/31/2013) added check for packagetype != 1
							AND @IsListingCompleted=1 -- Added By Avishkar(13-11-2013)  so that if Listing Completed is true then only car will be uploaded to Livelisting.
						)
			BEGIN
				EXEC LL_UpdateListing 
						@ProfileId, @SellerType, @Seller, @Inquiryid, @MakeId, @MakeName, 
						@ModelId, @ModelName, @VersionId, @VersionName, @StateId, @StateName,
						@CityId, @CityName, @AreaId, @AreaName, @Lattitude, @Longitude,
						@MakeYear, @Price, @Kilometers, @Color, @Comments, @EntryDate, 
						@LastUpdated, 
						@PackageId,-- Added by Supriya Bhide(22-1-2016)
						@PackageType, @ShowDetails, @Priority,@Owners,NULL/*CertificationId*/,@AdditionalFuel,
						NULL,/*EMI*/
						@DealerId,@IsPremium, -- Added by Avishkar 14-11-2013 for @IsPremium
						/*EMI*/
						--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
						--,@FUELTYPE,@owners  ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
						--,@VideoCount--Modified By Reshma Shetty 27/12/2013 To update the VideoCount if already present in the offline stocks
						NULL, @SellerName,@SellerContact	--Added By Sadhana Upadhyay on 20 Apr 2015
						,NULL -- Response -- added by Manish on 30 June 2015
			END
			ELSE
			BEGIN
				Delete From LiveListings Where ProfileId = @ProfileId				
				
			END

		END
		
	END--first if for rowcount
	ELSE IF @NoOfRows > 1
	BEGIN
	--	PRINT 'Bulk Update'
		--else iterate through the rows and update the data accordingly
		--initialize the values
		SET @LoopIndex = 1
		--get the next row id
		SELECT @NextRowId = Min(Inserted.Id) From Inserted
		
		WHILE @NoOfRows >= @LoopIndex
		BEGIN
			--PRINT 'Printing NextRowId'
			--PRINT @NextRowId
			SELECT	
				@CurRowId = I.ID,	
				@ProfileId = 'S' + CAST(I.ID AS VarChar(50)),	@SellerType = 2,		@Seller = 'Individual',
				@InquiryId = I.ID,		@MakeId = Ma.ID,			@MakeName = Ma.Name,	@ModelId = Mo.ID, 
				@ModelName = Mo.Name, 	@VersionId = Cv.ID,			@VersionName = Cv.Name,	@StateId = S.Id,
				@StateName = S.Name,	@CityId = C.Id,				@CityName = C.Name,		@AreaId = A.ID,
				@AreaName = A.Name,			@Lattitude = C.Lattitude,	@Longitude = C.Longitude,
				@MakeYear = I.MakeYear,	@Price = I.Price,			@Kilometers = I.Kilometers, 
				@Color = I.Color,		@Comments = I.Comments,		@EntryDate = I.EntryDate,
				@LastUpdated = GETDATE(),
				@PackageId = I.PackageId,-- Added by Supriya Bhide(22-1-2016)
				@PackageType = I.PackageType,	
				@ShowDetails = (CASE I.PackageType WHEN 2 THEN 1 ELSE 0 END), 
				@Priority = (CASE I.PackageType WHEN 2 THEN 1 WHEN 1 THEN 5 END),
				@IsApproved = I.IsApproved, @IsFake = I.IsFake, @StatusId = I.StatusId, 
				@ClassifiedExpiryDate = I.ClassifiedExpiryDate,
	            @AdditionalFuel=CSI.AdditionalFuel,
				@IsListingCompleted=I.IsListingCompleted, -- Added By Avishkar(13-11-2013)  so that if Listing Completed is true then only car will be uploaded to Livelisting.
	            @Owners=(
						CASE ISNULL(CSI.Owners,'-1')
							WHEN '1'
								THEN 'First Owner '
							WHEN '2'
								THEN 'Second Owner '
							WHEN '3'
								THEN 'Third Owner '
							WHEN '4'
								THEN 'Fourth Owner'
							WHEN '-1'
								THEN 'N/A'							
							ELSE
								 'More than 4 owners'
							END
						),
				 @IsPremium= I.IsPremium --Modified by Avishkar (15-11-2013) Added @IsPremium to set IsPremium column in LiveListing
	            --Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
	            --@FUELTYPE=CV.carfueltype, ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
				--@owners=CSI.OWNERS  ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
				,@VideoCount=CSI.IsYouTubeVideoApproved--Modified By Reshma Shetty 27/12/2013 To update the VideoCount if already present in the offline stocks
				,@SellerName = I.CustomerName
				,@SellerContact = (
								CASE 
									WHEN I.IsShowContact = 1
										THEN I.CustomerMobile
									ELSE ''
									END
								)
			FROM 
				CarMakes AS Ma, CarModels AS Mo, CarVersions AS Cv, 
				((Inserted AS I LEFT JOIN Cities AS C ON C.ID = I.CityId)
				LEFT JOIN States AS S ON S.ID = C.StateId
				LEFT JOIN Areas AS A ON A.ID= I.AreaId) --Modified by Aditi dhaybar on 3/2/15 to get the AreaId and AreaName
				JOIN CustomerSellInquiryDetails as CSI on CSI.InquiryId=I.ID
			WHERE
				Cv.ID = I.CarVersionId AND Mo.Id = Cv.CarModelId AND Ma.ID = Mo.CarMakeId AND I.ID = @NextRowId	
			--PRINT @@RowCount
			--check for the fields isapproved, isfake, statusid and the classifiedexpirydate
			IF @IsApproved = 1 AND @IsFake = 0 AND @StatusId = 1 AND 
						(
							@ClassifiedExpiryDate >= CONVERT(varchar, GETDATE(), 101) 
							AND @PackageType != 1  ----Modified by amit verma (10/31/2013) added check for packagetype != 1
							AND @IsListingCompleted=1 -- Added By Avishkar(13-11-2013)  so that if Listing Completed is true then only car will be uploaded to Livelisting.
						)
			BEGIN
			--	PRINT 'executing LL_UpdateListing'
			--	PRINT @LastUpdated
				EXEC LL_UpdateListing 
						@ProfileId, @SellerType, @Seller, @Inquiryid, @MakeId, @MakeName, 
						@ModelId, @ModelName, @VersionId, @VersionName, @StateId, @StateName,
						@CityId, @CityName, @AreaId, @AreaName, @Lattitude, @Longitude,
						@MakeYear, @Price, @Kilometers, @Color, @Comments, @EntryDate, 
						@LastUpdated, 
						@PackageId,-- Added by Supriya Bhide(22-1-2016)
						@PackageType, @ShowDetails, @Priority,@Owners,NULL/*CertificationId*/,@AdditionalFuel,
						NULL,/*EMI*/
						@DealerId,@IsPremium,-- Added by Avishkar 14-11-2013 for @IsPremium 
						--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
						--,@FUELTYPE,@owners  ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
						--,@VideoCount--Modified By Reshma Shetty 27/12/2013 To update the VideoCount if already present in the offline stocks
						NULL, @SellerName,@SellerContact	--Added By Sadhana Upadhyay on 20 Apr 2015
						,NULL -- Response -- added by Manish on 30 June 2015
			END
			ELSE
			BEGIN
				Delete From LiveListings Where ProfileId = @ProfileId				
			END
			--increase the counter
			SET @LoopIndex = @LoopIndex + 1
			--Also reset the next row id
			SELECT @NextRowId = MIN(ID) FROM Inserted WHERE ID > @CurRowId
		END	--end of while

	END--end of else for no of rows

END

/****** Object:  Trigger [dbo].[TrigUpdateDealerCarData]    Script Date: 07/10/2015 11:03:55 ******/
SET ANSI_NULLS ON

GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[TrigUpdateClassifiedExpirtyDate] 
   ON  dbo.CustomerSellInquiries
   FOR INSERT,UPDATE
AS 
DECLARE
	@PackageType	AS SMALLINT,
	@NoOfRows		AS NUMERIC,
	@RecordId		AS NUMERIC
BEGIN
	SET @NoOfRows = @@ROWCOUNT
	
	IF @NoOfRows = 1
	BEGIN
		IF(UPDATE(PackageType))
		BEGIN
			--If the package type is 2 then update the classified expirty date by 45 days
			Select @PackageType = PackageType, @RecordId = Id From Inserted
		
			IF @PackageType = 2
			BEGIN
				--Update the classified expiry date and set the status back to 1
				Update CustomerSellInquiries Set 
					ClassifiedExpiryDate = DATEADD(dd, 30, GETDATE()),
					StatusId = 1, IsApproved = 1, IsFake = 0
				Where 
					Id = @RecordId
			END
		END
	
	END
	
	
END

GO
DISABLE TRIGGER [dbo].[TrigUpdateClassifiedExpirtyDate]
    ON [dbo].[CustomerSellInquiries];


GO
-- =============================================
-- Author:		<Rajeev>
-- Create date: <3/10/08>
-- Description:	<For deleting data of Live stock>
-- Added by Avishkar 14-11-2013 to set IsPremium column 
-- =============================================
CREATE TRIGGER [dbo].[TrigDeleteCustCarData] 

   ON  [dbo].[CustomerSellInquiries]
   FOR DELETE
AS 
DECLARE
	@ProfileId		AS VARCHAR(50),@Inquiryid BIGINT
BEGIN
	
	SELECT	
		@ProfileId = 'S' + CAST(D.ID AS VarChar(50)),@Inquiryid=D.ID
	FROM 
		Deleted AS D
	
	Delete From LiveListings Where ProfileId = @ProfileId
	

END
