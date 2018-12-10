CREATE TABLE [dbo].[SellInquiries] (
    [ID]                    NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]              NUMERIC (18)  NOT NULL,
    [CarVersionId]          NUMERIC (18)  NOT NULL,
    [CarRegNo]              VARCHAR (15)  NULL,
    [EntryDate]             DATETIME      NOT NULL,
    [StatusId]              NUMERIC (18)  NOT NULL,
    [Price]                 DECIMAL (18)  NOT NULL,
    [MakeYear]              DATETIME      NOT NULL,
    [Kilometers]            NUMERIC (18)  NOT NULL,
    [Color]                 VARCHAR (100) NULL,
    [ColorCode]             VARCHAR (6)   NULL,
    [Comments]              VARCHAR (500) NULL,
    [IsArchived]            BIT           CONSTRAINT [DF_SellInquiries_IsArchived] DEFAULT ((0)) NULL,
    [ImportChecksum]        NUMERIC (18)  CONSTRAINT [DF_SellInquiries_Checksum] DEFAULT ((-1)) NULL,
    [LastUpdated]           DATETIME      NULL,
    [ViewCount]             NUMERIC (18)  NULL,
    [PaidInqLeft]           INT           CONSTRAINT [DF_SellInquiries_PaidInqLeft] DEFAULT ((0)) NULL,
    [FreeInqLeft]           INT           CONSTRAINT [DF_SellInquiries_FreeInqLeft] DEFAULT ((0)) NULL,
    [PackageType]           SMALLINT      CONSTRAINT [DF_SellInquiries_PackageType] DEFAULT ((1)) NULL,
    [PackageExpiryDate]     DATETIME      NULL,
    [TC_StockId]            NUMERIC (18)  NULL,
    [CertificationId]       SMALLINT      NULL,
    [ModifiedBy]            INT           NULL,
    [ModifiedDate]          DATETIME      NULL,
    [DescRating]            SMALLINT      NULL,
    [AlbumRating]           SMALLINT      NULL,
    [CalculatedEMI]         INT           DEFAULT (NULL) NULL,
    [CertifiedLogoUrl]      VARCHAR (200) NULL,
    [IsPremium]             BIT           DEFAULT ((0)) NULL,
    [UsedCarMasterColorsId] SMALLINT      NULL,
    [EMI]                   INT           NULL,
    [SourceId]              TINYINT       CONSTRAINT [DF_SellInquiries_SourceId] DEFAULT ((2)) NOT NULL,
    CONSTRAINT [PK_SellInquiries] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SellInq_CarVersions] FOREIGN KEY ([CarVersionId]) REFERENCES [dbo].[CarVersions] ([ID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_SellInq_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_SellInq_SellInqStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[SellInquiriesStatus] ([ID]) ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [Idx_SellInquiries_Dealerid]
    ON [dbo].[SellInquiries]([DealerId] ASC)
    INCLUDE([ViewCount]);


GO
CREATE NONCLUSTERED INDEX [ix_SellInquiries__TC_StockId]
    ON [dbo].[SellInquiries]([TC_StockId] ASC)
    INCLUDE([ID]);


GO
CREATE NONCLUSTERED INDEX [IX_SellInquiries_PackageType]
    ON [dbo].[SellInquiries]([PackageType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SellInquiries_StatusId]
    ON [dbo].[SellInquiries]([StatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SellInquiries_PackageExpiryDate]
    ON [dbo].[SellInquiries]([PackageExpiryDate] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_SellInquiries__DealerId__StatusId__PackageExpiryDate]
    ON [dbo].[SellInquiries]([DealerId] ASC, [StatusId] ASC, [PackageExpiryDate] ASC)
    INCLUDE([CarVersionId]);


GO
----Manish 16-11-2012 Modified for selecting used car score prametres
--Modified by Reshma Shetty(03-05-2013)Added parameter owners to update the newly introduced Owners field in LiveListing
-- Modified by Manish on 10-07-2013 when owner value is zero than show "Unregistered Car"
-- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table
-- Modified by Avishkar (15-11-2013) Added @IsPremium to set IsPremium column in LiveListing
-- Added by Avishkar 22-11-2013 to set to set N/A if Owners is NULL CASE isnull(SI.Owners,'-1')
--Modified By Reshma Shetty 26/12/2013 'Unregistered Car' has been changed to 'UnRegistered Car' where R is caps to identify if the Owners is set only through this trigger,likewise for 'More Than 4 Owners'
-- Modified By: Avishkar on 10-4-2014  To set lead score for all live listings
-- Modified by: Manish on 24-07-2014 added with (nolock) keyword.
-- Modified by: Manish on 12-08-2014 commented the sp ComputeLLSortScore execution since it will execute through scheduled job.
-- Modified By Vivek Gupta on 10-11-2014, added @UsedCarEMI to copy stock EMI to livelisting
-- Added column SellerName,SellerContact By Sadhana Upadhyay on 20 Apr 2015
--Modified by Manish on 30 June 2015 added parameter Response on ll_updatelisting SP and commented print statement
-- Modified by Supriya Bhide on 22 Jan 2016 added parameter PackageId on ll_updatelisting SP
-- Modified by Supriya Bhide (8-2-2016), added condition for dealer in fetching PackageId from ConsumerCreditPoints table
-- Modified by Navead on 30-08-2016 fetch PackgeId for Ct cars
-- Modified by Kinjal and Sahil on 19-10-2016,20-10-2016 to handle auto removal of cars on update whose package is expiry for ct cars.
CREATE TRIGGER [dbo].[TrigUpdateDealerCarData] ON [dbo].[SellInquiries]
FOR INSERT
	,UPDATE
AS
DECLARE @ProfileId AS VARCHAR(50)
	,@SellerType AS SMALLINT
	,@Seller AS VARCHAR(50)
	,@Inquiryid AS NUMERIC
	,@MakeId AS NUMERIC
	,@MakeName AS VARCHAR(100)
	,@ModelId AS NUMERIC
	,@ModelName AS VARCHAR(100)
	,@VersionId AS NUMERIC
	,@VersionName AS VARCHAR(100)
	,@StateId AS NUMERIC
	,@StateName AS VARCHAR(100)
	,@CityId AS NUMERIC
	,@CityName AS VARCHAR(100)
	,@AreaId AS NUMERIC
	,@AreaName AS VARCHAR(100)
	,@Lattitude AS DECIMAL(18, 4)
	,@Longitude AS DECIMAL(18, 4)
	,@MakeYear AS DATETIME
	,@Price AS NUMERIC
	,@Kilometers AS NUMERIC
	,@Color AS VARCHAR(100)
	,@Comments AS VARCHAR(500)
	,@EntryDate AS DATETIME
	,@LastUpdated AS DATETIME
	,@PackageId AS SMALLINT
	,@PackageType AS SMALLINT
	,@ShowDetails AS BIT
	,@Priority AS SMALLINT
	,@StatusId AS SMALLINT
	,@DealerStatus AS BIT
	,@PackageExpiryDate AS DATETIME
	,@ViewCountP AS NUMERIC
	,@DataCanBeUpdated AS BIT
	,@NoOfRows AS NUMERIC
	,@NextRowId AS NUMERIC
	,@CurRowId AS NUMERIC
	,@LoopIndex AS NUMERIC
	,@EMI AS BIGINT
	,@Owners AS VARCHAR(20)
	,@CertificationId AS SMALLINT = NULL
	,@AdditionalFuel AS VARCHAR(50) = NULL
	,@DealerId INT
	,-- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table,
	@IsPremium BIT = 0
	,--Added by Avishkar 14-11-2013 for @IsPremium 
	@UsedCarEMI NUMERIC
	,-- Modified By Vivek Gupta on 10-11-2014
	-- Added By : Sadhana Upadhyay on 20 Apr 2015
	@SellerName AS VARCHAR(100)
	,@SellerContact AS VARCHAR(20)
	,@Response AS INT
	,@SourceId	AS INT
--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
--@FUELTYPE       as DECIMAL(1),-------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
--@owners        as  VARCHAR(10)   -------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
BEGIN
	SET @NoOfRows = @@ROWCOUNT
	SET @NoOfRows = (
			SELECT count(*)
			FROM Inserted
			)

	--PRINT @NoOfRows
	IF @NoOfRows = 1
	BEGIN
		--PRINT 'Single Row'

		--if there is only one record which has been updated then perform the simple operation
		SET @DataCanBeUpdated = 1

		SELECT @ViewCountP = IsNull(I.ViewCount, 0)
		FROM Inserted AS I

		--PRINT '1st'

		IF (
				UPDATE (ViewCount)
				)
		BEGIN
			SET @DataCanBeUpdated = 0
		END

		--PRINT '2nd'

		IF @ViewCountP = 0
		BEGIN
			SET @DataCanBeUpdated = 1
		END

		--PRINT '3rd'

		/*

			This has been updated to the new one. Just kept for information purpose

			@ShowDetails =  (CASE I.PackageType WHEN 8 THEN 0 ELSE 1 END), 

			@Priority =  (CASE I.PackageType WHEN 7 THEN 2 WHEN 5 THEN 3 WHEN 6 THEN 3 WHEN 9 THEN 3 WHEN 8 THEN 4 END),

		*/
		IF @DataCanBeUpdated = 1
		BEGIN
			-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
		--	PRINT '4th'

			-- Modified by Navead on 30-08-2016 fetch PackgeId for Ct cars
			SELECT @PackageId = (Case WHEN CDM.PackageId IS NULL THEN CCP.CustomerPackageId ELSE CDM.PackageId END)
			FROM Inserted AS I 
			LEFT JOIN CWCTDealerMapping CDM on CDM.CWDealerID = I.DealerId AND isnull(CDM.IsMigrated,0) = 1 AND CDM.PackageEndDate>=GETDATE()
			LEFT JOIN ConsumerCreditPoints CCP on I.DealerId = CCP.ConsumerId AND CCP.ConsumerType = 1 AND CCP.ExpiryDate>=GETDATE()
		
			SELECT @ProfileId = 'D' + CAST(I.ID AS VARCHAR(50))
				,@SellerType = 1
				,@Seller = 'Dealer'
				,@InquiryId = I.ID
				,@MakeId = Ma.ID
				,@MakeName = Ma.NAME
				,@ModelId = Mo.ID
				,@ModelName = Mo.NAME
				,@VersionId = Cv.ID
				,@VersionName = Cv.NAME
				,@StateId = S.Id
				,@StateName = S.NAME
				,@CityId = C.Id
				,@CityName = C.NAME
				,@AreaId = A.ID
				,@AreaName = A.NAME
				,@Lattitude = C.Lattitude
				,@Longitude = C.Longitude
				,@MakeYear = I.MakeYear
				,@Price = I.Price
				,@Kilometers = I.Kilometers
				,@Color = I.Color
				,@Comments = I.Comments
				,@EntryDate = I.EntryDate
				,@LastUpdated = I.LastUpdated
				--,@PackageId = CCP.CustomerPackageId -- Modified by Navead on 30-08-2016 fetch PackgeId for Ct cars
				,@PackageType = I.PackageType				
				,@ShowDetails = 1
				,@Priority = (
					CASE I.PackageType
						WHEN 19
							THEN 2
						WHEN 18
							THEN 3
						END
					)
				,@StatusId = I.StatusId
				,@DealerStatus = D.STATUS
				,@PackageExpiryDate = I.PackageExpiryDate
				,@CertificationId = I.CertificationId
				,@AdditionalFuel = SI.AdditionalFuel
				,@EMI = I.CalculatedEMI
				,--AM Modified 28-09-2012 to use I as Inserted instead of Sellinquiries
				@DealerId = I.DealerId
				,-- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table
				@Owners = (
					CASE ISNULL(SI.Owners, '-1') --Modified by Reshma Shetty(03-05-2013)Added parameter owners to update the newly introduced Owners field in LiveListing
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
								--WHEN '0'
								--     THEN 'Unregistered Car'  --Modified by Manish on 10-07-2013 when owner value is zero than show "Unregistered Car"
								--ELSE
								--	 'More than 4 owners'
								----------------------'Unregistered Car' has been changed to 'UnRegistered Car' where R is caps to identify if the Owners is set only through this trigger,likewise for 'More Than 4 Owners'
						WHEN '0'
							THEN 'UnRegistered Car' --Modified by Manish on 10-07-2013 when owner value is zero than show "Unregistered Car"
						ELSE 'More Than 4 Owners'
						END
					)
				,@IsPremium = I.IsPremium
				,--Modified by Avishkar (15-11-2013) Added @IsPremium to set IsPremium column in LiveListing
				@UsedCarEMI = I.EMI -- Modified By Vivek Gupta on 10-11-2014
				-- Added By Sadhana Upadhyay on 20 Apr 2015
				,@SellerContact = D.ActiveMaskingNumber
				,@SellerName = d.Organization
				,@SourceId = I.SourceId
			--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
			--@FUELTYPE=CV.carfueltype, ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
			--@owners=SI.OWNERS  ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
			FROM CarMakes AS Ma WITH (NOLOCK)
				,CarModels AS Mo WITH (NOLOCK)
				,CarVersions AS Cv WITH (NOLOCK)
				,(
					(
						(
							(
								Inserted AS I LEFT JOIN Dealers AS D WITH (NOLOCK) ON D.ID = I.DealerID
								) LEFT JOIN Areas AS A WITH (NOLOCK) ON A.ID = D.AreaId
							) LEFT JOIN Cities AS C WITH (NOLOCK) ON C.ID = D.CityId
						) LEFT JOIN States AS S WITH (NOLOCK) ON S.ID = D.StateId
					)
			INNER JOIN SellInquiriesDetails AS SI WITH (NOLOCK) ON SI.SellInquiryId = I.ID 
			--LEFT JOIN ConsumerCreditPoints AS CCP WITH (NOLOCK) ON CCP.ConsumerId = D.ID AND CCP.ConsumerType = 1 -- Modified by Navead on 30-08-2016 fetch PackgeId for Ct cars
																													-- Modified by Supriya Bhide (8-2-2016)
			--INNER JOIN SellInquiries AS SE ON SE.ID = SI.SellInquiryId
			WHERE Cv.ID = I.CarVersionId
				AND Mo.Id = Cv.CarModelId
				AND Ma.ID = Mo.CarMakeId

			--PRINT '5th'
			--PRINT 'ProfileId:'
			--check for the fields isapproved, isfake, statusid and the classifiedexpirydate
			--PRINT '6th'

			IF ((@DealerStatus = 0
				AND @StatusId = 1
				AND (@PackageExpiryDate >= CONVERT(VARCHAR, GETDATE(), 101)))
				OR (@StatusId = 1 AND @SourceId = 1 ))
				-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
			BEGIN
				--PRINT '7th'


				IF (
						UPDATE (StatusID)
						)
				BEGIN
							SELECT @Response =Count(UCPI.Id)
							FROM usedcarpurchaseinquiries AS UCPI WITH (NOLOCK)
							WHERE UCPI.sellinquiryid = @INQUIRYID
								AND UCPI.isapproved = 1
								AND UCPI.IsFake = 0
							

				END

				

				EXEC LL_UpdateListing @ProfileId
					,@SellerType
					,@Seller
					,@Inquiryid
					,@MakeId
					,@MakeName
					,@ModelId
					,@ModelName
					,@VersionId
					,@VersionName
					,@StateId
					,@StateName
					,@CityId
					,@CityName
					,@AreaId
					,@AreaName
					,@Lattitude
					,@Longitude
					,@MakeYear
					,@Price
					,@Kilometers
					,@Color
					,@Comments
					,@EntryDate
					,@LastUpdated
					,@PackageId
					,@PackageType
					,@ShowDetails
					,@Priority
					,@Owners
					,@CertificationId
					,@AdditionalFuel
					,@EMI
					,@DealerId
					,@IsPremium
					,@UsedCarEMI -- Modified By Vivek Gupta on 10-11-2014
					,@SellerName
					,@SellerContact --Added By Sadhana Upadhyay on 20 Apr 2015
					,@Response  --Added by Manish on 30 June 2015
				--for @IsPremium  Added By Avishkar(13-11-2013) 
				--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
				--@FUELTYPE,@owners  ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
			
						--Modified By: Avishkar on 10-4-2014  To set lead score for all live listings
						-- EXEC ComputeLLSortScore  @ProfileId	  -- Commented by Manish on 12-08-2014	
			END
			ELSE
			BEGIN
				--IF NOT EXISTS (SELECT ID FROM SellInquiries WHERE @StatusId = 1 AND @SourceId = 1 AND ID = @Inquiryid )
				--BEGIN
					DELETE
					FROM LiveListings
					WHERE ProfileId = @ProfileId
				--END
			END
		END
	END
	ELSE
		IF @NoOfRows > 1
		BEGIN
			--PRINT 'Multiple Row'

			--else iterate through the rows and update the data accordingly
			--initilialize the values
			SET @LoopIndex = 1

			WHILE @NoOfRows >= @LoopIndex
			BEGIN
				IF (@LoopIndex > 1)
				BEGIN
					SELECT @NextRowId = MIN(ID)
					FROM Inserted
					WHERE ID > @CurRowId
				END
				ELSE
				BEGIN
					SELECT @NextRowId = MIN(ID)
					FROM Inserted
				END

				/*

				This has been updated. Just kept here for information purpose

				@ShowDetails =  (CASE I.PackageType WHEN 8 THEN 0 ELSE 1 END), 

				@Priority =  (CASE I.PackageType WHEN 7 THEN 2 WHEN 5 THEN 3 WHEN 6 THEN 3 WHEN 9 THEN 3 WHEN 8 THEN 4 END),

			*/
				--fetch the data for the row
				-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
				SELECT @CurRowId = I.ID
					,@ProfileId = 'D' + CAST(I.ID AS VARCHAR(50))
					,@SellerType = 1
					,@Seller = 'Dealer'
					,@InquiryId = I.ID
					,@MakeId = Ma.ID
					,@MakeName = Ma.NAME
					,@ModelId = Mo.ID
					,@ModelName = Mo.NAME
					,@VersionId = Cv.ID
					,@VersionName = Cv.NAME
					,@StateId = S.Id
					,@StateName = S.NAME
					,@CityId = C.Id
					,@CityName = C.NAME
					,@AreaId = A.ID
					,@AreaName = A.NAME
					,@Lattitude = C.Lattitude
					,@Longitude = C.Longitude
					,@MakeYear = I.MakeYear
					,@Price = I.Price
					,@Kilometers = I.Kilometers
					,@Color = I.Color
					,@Comments = I.Comments
					,@EntryDate = I.EntryDate
					,@LastUpdated = I.LastUpdated
					,@PackageId = CCP.CustomerPackageId
					,@PackageType = I.PackageType
					,@ShowDetails = 1
					,@Priority = (
						CASE I.PackageType
							WHEN 19
								THEN 2
							WHEN 18
								THEN 3
							END
						)
					,@StatusId = I.StatusId
					,@DealerStatus = D.STATUS
					,@PackageExpiryDate = I.PackageExpiryDate
					,@CertificationId = I.CertificationId
					,@AdditionalFuel = SI.AdditionalFuel
					,@EMI = I.CalculatedEMI
					,-- AM Modified 28-09-2012 to use I as Inserted instead of Sellinquiries
					@DealerId = I.DealerId
					,-- Added by Avishkar 14-10-2013 to populate DealerId in Livelistings table
					@Owners = (
						CASE ISNULL(SI.Owners, '-1')
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
									--WHEN '0'
									--     THEN 'Unregistered Car'  --Modified by Manish on 10-07-2013 when owner value is zero than show "Unregistered Car"
									--ELSE
									--	 'More than 4 owners'
									----------------------'Unregistered Car' has been changed to 'UnRegistered Car' where R is caps to identify if the Owners is set only through this trigger,likewise for 'More Than 4 Owners'
							WHEN '0'
								THEN 'UnRegistered Car' --Modified by Manish on 10-07-2013 when owner value is zero than show "Unregistered Car"
							ELSE 'More Than 4 Owners'
							END
						)
					,@IsPremium = I.IsPremium --Modified by Avishkar (15-11-2013) Added @IsPremium to set IsPremium column in LiveListing
					--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
					--@FUELTYPE=CV.carfueltype, ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
					--@owners=SI.OWNERS  ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
					,@SellerName = D.Organization
					,@SellerContact = D.ActiveMaskingNumber
					,@SourceId = I.SourceId
				FROM CarMakes AS Ma WITH (NOLOCK)
					,CarModels AS Mo WITH (NOLOCK)
					,CarVersions AS Cv WITH (NOLOCK)
					,(
						(
							(
								(
									Inserted AS I LEFT JOIN Dealers AS D WITH (NOLOCK) ON D.ID = I.DealerID
									) LEFT JOIN Areas AS A WITH (NOLOCK) ON A.ID = D.AreaId
								) LEFT JOIN Cities AS C WITH (NOLOCK) ON C.ID = D.CityId
							) LEFT JOIN States AS S WITH (NOLOCK) ON S.ID = D.StateId
						)
				JOIN SellInquiriesDetails AS SI WITH (NOLOCK) ON SI.SellInquiryId = I.ID
				LEFT JOIN ConsumerCreditPoints AS CCP WITH (NOLOCK) ON CCP.ConsumerId = D.ID AND CCP.ConsumerType = 1	-- Modified by Supriya Bhide (8-2-2016)
				--INNER JOIN SellInquiries AS SE ON SE.ID = SI.SellInquiryId
				WHERE Cv.ID = I.CarVersionId
					AND Mo.Id = Cv.CarModelId
					AND Ma.ID = Mo.CarMakeId
					AND I.ID = @NextRowId

				--PRINT @ProfileId

				--check for the fields isapproved, isfake, statusid and the classifiedexpirydate
			
				IF ((@DealerStatus = 0
					AND @StatusId = 1
					AND (@PackageExpiryDate >= CONVERT(VARCHAR, GETDATE(), 101))) 
					OR ( @StatusId = 1 AND @SourceId = 1 ))
				
				BEGIN
					
					
						IF (
							UPDATE (StatusID)
							)
					BEGIN
							SELECT @Response=Count(UCPI.Id)
								FROM usedcarpurchaseinquiries AS UCPI WITH (NOLOCK)
								WHERE UCPI.sellinquiryid = @InquiryId
									AND UCPI.isapproved = 1
									AND UCPI.IsFake = 0
						
					END

					
					-- Modified By: Reshma Shetty Date: 30/08/2012 -- Added parameter @EMI to calculate EMI for HDFC empaneled dealers
					EXEC LL_UpdateListing @ProfileId
						,@SellerType
						,@Seller
						,@Inquiryid
						,@MakeId
						,@MakeName
						,@ModelId
						,@ModelName
						,@VersionId
						,@VersionName
						,@StateId
						,@StateName
						,@CityId
						,@CityName
						,@AreaId
						,@AreaName
						,@Lattitude
						,@Longitude
						,@MakeYear
						,@Price
						,@Kilometers
						,@Color
						,@Comments
						,@EntryDate
						,@LastUpdated
						,@PackageId
						,@PackageType
						,@ShowDetails
						,@Priority
						,@Owners
						,@CertificationId
						,@AdditionalFuel
						,@EMI
						,@DealerId
						,@IsPremium
						,@UsedCarEMI -- Modified By Vivek Gupta on 10-11-2014
						,@SellerName
						,@SellerContact --Added By Sadhana Upadhyay on 20 Apr 2015
						,@Response  --Added by Manish on 30 June 2015

					-- @IsPremium  Added By Avishkar(13-11-2013) 
					--Commented by Reshma Shetty(16-1-2013) since no longer used in calculation
					--@FUELTYPE,@owners  ---line add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring
							--Modified By: Avishkar on 10-4-2014  To set lead score for all live listings
							--	EXEC ComputeLLSortScore  @ProfileId  -- Commented by Manish on 12-08-2014
				END
				ELSE
				BEGIN
					--IF NOT EXISTS (SELECT ID FROM SellInquiries WHERE @StatusId = 1 AND @SourceId = 1 AND ID = @Inquiryid )
						--BEGIN
							DELETE
							FROM LiveListings
							WHERE ProfileId = @ProfileId
						--END
				END
				--increase the counter
				SET @LoopIndex = @LoopIndex + 1
			END
		END
				--PRINT 'EMI:'+cast(@EMI as varchar(10))
END
GO
CREATE TRIGGER [dbo].[TrigDealerDealerCarData] 

   ON  [dbo].[SellInquiries]
   FOR DELETE
AS 
DECLARE
	@ProfileId		AS VARCHAR(50),@Inquiryid BIGINT
BEGIN
	
	SELECT	
		@ProfileId = 'D' + CAST(D.ID AS VarChar(50)),@Inquiryid=D.ID
	FROM 
		Deleted AS D
	
	Delete From LiveListings Where ProfileId = @ProfileId

	
	

END

