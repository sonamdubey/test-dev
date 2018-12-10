IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_UpdateFunnelStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_UpdateFunnelStatus]
GO

	-- =============================================================================
--	Author	:	Sachin Bharti(5th Dec 2014)
--	Description	:	Update funnel status and add new one also
--	Modifier	:	Sachin Bharti(5th Jan 2015)
--	Purpose	:	Make payment type is 1 for free trial default entry
--	Modifier	:	Sachin Bharti(19th Jan 2015)
--	Purpose	:	Added new parameter @WarrantyPerSlab for warranty based products
-- execute [dbo].[M_UpdateFunnelStatus]  0,12803,4271,-1,1,1,3,50,10,10000,10,'12/5/2015',1,-1,-1,null,'ffgfgg','12/19/2015' ,2,3,0,-1,0,null,'172.168.1.16','C# Essentials 2nd Edition.pdf',1,1234
-- Modified By : Kartik Rathod on 25 Nov 2015 add @newComments,@oldComments
--Modified By  : Ajay singh on 29 Nov 2015 added Model,ExceptionModel,LeadPerDay,StartDate,NewContractType,OldContractType 
-- Modified By : Kartik Rathod on 2 dec 2015, added @IsMultiOutlet,@OutletId to saving outlet id for Multi outlet CampaignType and insert outletId's against DCRM_salesDealerId in DCRM_MultiOutletMapping table
-- Modified By : vinay Kumar prajapati 21st Desc 2015 added @Source =2 for App and 1 for web Application
-- Modified by : Amit Yadav 1 Jan 2016 multioutlet to get the outlet mapping.
-- Modified By : Sunil M. Yadav On 20th May 2016 
-- Description : Save Exception model details on every percentage for sales funnel.
-- Modified By : Mihir A Chheda [10-08-2016]
-- Description : store lpa number in table	
-- ==============================================================================
CREATE PROCEDURE [dbo].[M_UpdateFunnelStatus] 
	
	@Type			SMALLINT,	--Used for new Insert and Update existing entry
	@SalesId		NUMERIC(18,0),
	@DealerId		NUMERIC(18,0),
	@SalesMeetingId NUMERIC(18,0) = -1,	
	@MeetingMode	SMALLINT,
	@DealerType		SMALLINT = NULL,

	--New Product Details
	@ProductRelationshipType	SMALLINT = NULL,
	@NewPitchProdId	VARCHAR(10)	=	NULL,
	@NewPitchDuratn	VARCHAR(10)	=	NULL,
	@NewClosingAmnt		VARCHAR(10)	=	NULL,
	@NewClosingProbability	VARCHAR(10)	=	NULL,
	@NewExpClosingDate		DATETIME	=	NULL,
	@NewRSAPackageQunatity	INT	=	NULL,
	@NewLeadStatus		VARCHAR(10)	=	NULL,
	@NewLostReasons		VARCHAR(10)	=	NULL,
	@OtherLostReason	VARCHAR(100)	=	NULL,
	--EXEC M_UpdateFunnelStatus 0,12803,4271,-1,1,1,3,50,'10','10000','10','12/5/2015',null,'1','-1',-1,50,10,10000,10,'12/2/2015',0,1,null,1,0,null,null,-1,null,null,null,null,'ffgfgg','ffgfgg',null,null,null,'12/19/2015',null,2,2,null

	--Old Product Details
	@OldPitchProdId	VARCHAR(10)	=	NULL,
	@OldPitchDuration	VARCHAR(10)	=	NULL,
	@OldClosingAmnt	VARCHAR(10)	=	NULL,
	@OldClosingProbability	VARCHAR(10)	=	NULL,
	@OldExpClosingDate	DATETIME	=	NULL,
	

	@IsCpLog		BIT =	NULL,	--If 1 then update record in DCRM_CpLog
	@IsProductLog	BIT	=	NULL,	--If 1 then update record in DCRM_ProductLog
	@CampaignType	SMALLINT	=	NULL,
	@UpdatedBy		INT=NULL	,
	@IsFromMobile	BIT	=	NULL,
	@NewSalesId		INT	OUTPUT,
	@IsProductExist	BIT OUTPUT,
	@WarrantyPerSlab	FLOAT = NULL,
	@FileHostURL	VARCHAR(250) = NULL,
	@AttachedLPAName	VARCHAR(MAX) = NULL,
	@IsLpaUploaded	BIT = NULL,
	@NoOfLeads		INT = NULL,
	@NewComments	VARCHAR(1000)=NULL,
	@OldComments	VARCHAR(1000)=NULL,
	@Model			VARCHAR(100) = NULL,
	@ExceptionModel VARCHAR(100) = NULL,
	@LeadPerDay		INT = NULL,
	@StartDate      DATETIME = NULL,
	--@EndDate        DATETIME = NULL
	@OutletId VARCHAR(100) = NULL,--OutletsIds for Group/Multioutlets.
	@NewContractType  INT = NULL,
	@OldContractType  INT = NULL,
	@IsMultiOutlet BIT =NULL,--To check the entry in DCRM_SalesDealer is Group/Multioutlet.
	@Source SmallInt = 1 ,
	@LPANumber INT = NULL  -- Mihir A Chheda [10-08-2016] 
	
AS
BEGIN
	
	DECLARE @TodayDate					DATETIME 
	DECLARE @TransactionId				INT = NULL
	DECLARE @DealerFieldExecutive		INT
	DECLARE	@DealerBackOfficeExecutive	INT


	SET	@TodayDate = GETDATE()
	SET @NewSalesId = 0
	SET @IsProductExist = 0

	--Get Sales field executive for the Dealer
	SELECT 
		@DealerFieldExecutive = DAU.UserId From DCRM_ADM_UserDealers DAU(NOLOCK) 
	WHERE 
		DealerId = @DealerId and RoleId = 3 --Sales field exceutive

	--Get Back office executive for the Dealer
	SELECT 
		@DealerBackOfficeExecutive = DAU.UserId From DCRM_ADM_UserDealers DAU(NOLOCK) 
	WHERE 
		DealerId = @DealerId and RoleId = 4 --Back office exceutive

	--declare temp table to store attached lpa names
	DECLARE @TempLPANameTable TABLE(RowId INT IDENTITY(1,1), LPAName VARCHAR(500))
	DECLARE @TotalFiles INT , @RowCount INT , @FileName VARCHAR(500)
	DECLARE @MultiOutletIds VARCHAR(100) = @OutletId             --to get the comma seperated value of dealer id

	IF @AttachedLPAName IS NOT NULL AND @AttachedLPAName != ''
		BEGIN
			--insert value in the LPAName table
			INSERT INTO @TempLPANameTable SELECT *FROM fnSplitCSVToChar(@AttachedLPAName)
			SET @TotalFiles = @@ROWCOUNT
			SELECT *FROM @TempLPANameTable
		END

	IF @SalesId = -1
		BEGIN
			--check if adding product is already exist in open stage 
			SELECT DSD.Id FROM DCRM_SalesDealer DSD(NOLOCK) 
			WHERE DSD.DealerId = @DealerId AND DSD.PitchingProduct = @NewPitchProdId AND DSD.LeadStatus = 1
			IF @@ROWCOUNT > 0
				BEGIN
					SET @IsProductExist = 1
				END
			ELSE
				BEGIN
					SET @IsProductExist = 0
				END
		END
	
	--if added product is not exist in open stage then go thorugh further process
	IF @IsProductExist = 0
	BEGIN
		--If entry is exist then update the existing record
		IF @Type = 0 
			BEGIN		
				IF @SalesMeetingId = -1 OR @SalesMeetingId IS NULL
					BEGIN
						--Make new entry in Sales Meeting if no meeting is recorded
						EXECUTE [dbo].[DCRM_RecordSalesMeeting] @DealerId,@DealerType,@SalesMeetingId,@SalesId,NULL,NULL,NULL,NULL,
															@UpdatedBy,NULL,NULL,@TodayDate,@IsFromMobile,@MeetingMode,NULL,NULL,NULL,NULL,NULL
						SET @SalesMeetingId = @@IDENTITY
					END

				--Updated entry in DCRM_SalesDealer				
				UPDATE 
					DCRM_SalesDealer
				SET		
					UpdatedOn = GETDATE(), UpdatedBy = @UpdatedBy,PitchDuration = @NewPitchDuratn,
					ClosingProbability = @NewClosingProbability,PitchingProduct = @NewPitchProdId, 
					ClosingAmount = @NewClosingAmnt,ClosingDate = @NewExpClosingDate,LeadStatus = @NewLeadStatus , 
					LostReason = @NewLostReasons,InitiatedBy = @UpdatedBy,OtherResons = @OtherLostReason,
					Quantity = @NewRSAPackageQunatity,IsFromMobile = @IsFromMobile,
					FieldExecutive = @DealerFieldExecutive , BOExecutive = @DealerBackOfficeExecutive,
					PercentageSlab = @WarrantyPerSlab,ProductStatus = @ProductRelationshipType,
					FileHostURL = NULL , IsLPAUploaded = @IsLpaUploaded, NoOfLeads = @NoOfLeads,Comments = @NewComments,
					StartDate=@StartDate,ContractType=@NewContractType,
					Model=@Model,ExceptionModel=@ExceptionModel,LeadPerDay=@LeadPerDay, -- Modified By : Sunil M. Yadav On 20th May 2016 	
					LPANumber=@LPANumber -- Modified By :Mihir A Chheda [10-08-2016] 		    
				WHERE 
					Id = @SalesId

				SET @NewSalesId = @SalesId
				
				--If user Loss or Converted Dealer then upadte SalesDealer with closed SalesMeetingID
				IF @NewLeadStatus = 2 OR @NewLeadStatus = 3
					BEGIN
					--If product is free,trial and adjustmetn/replacement then create a transaction with 0 amount
					--and also update payment details for that transaction with 0 amount 
					IF (@CampaignType = 1 OR @CampaignType = 2 OR @CampaignType = 4)
						BEGIN
							INSERT INTO DCRM_PaymentTransaction (	TotalClosingAmount,DiscountAmount,ProductAmount,ServiceTax,IsTDSGiven,
																	TDSAmount,FinalAmount,PANNumber,TANNumber,CreatedBy,CreatedOn	)
														VALUES	(	0,0,0,null,0,null,0,null,null,@UpdatedBy,GETDATE()	)
						
							SET @TransactionId = SCOPE_IDENTITY()
							--now insert payment details
							IF @TransactionId IS NOT NULL
								BEGIN
									INSERT INTO DCRM_PaymentDetails	( Mode,Amount,PaymentDate,AddedBy,AddedOn,TransactionId,PaymentType,PaymentReceived,AmountReceived,IsApproved)
										VALUES	( 1,0,GETDATE(),@UpdatedBy,GETDATE(),@TransactionId,1,0,0,NULL)
								END
						END

					--now update data in funnel table
					UPDATE DCRM_SalesDealer 
						SET 
							ClosedMeetingID = @SalesMeetingId ,UpdatedOn = GETDATE() ,CampaignType = @CampaignType , TransactionId = @TransactionId,Model=@Model,ExceptionModel=@ExceptionModel,LeadPerDay=@LeadPerDay
							,IsMultiOutlet = ISNULL(@IsMultiOutlet,0)						
						WHERE Id = @SalesId

					IF(@IsMultiOutlet = 1)
						BEGIN
							INSERT INTO DCRM_SalesDealerMapping (SalesDealerId,DealerId) 
							SELECT @SalesId DCRM_SalesDealerId,ListMember AS DealerId FROM fnSplitCSV(@MultiOutletIds)
						END
				END

				--if change in closing probability then log the details
				IF @IsCpLog = 1 AND @SalesId IS NOT NULL
					BEGIN
						INSERT INTO DCRM_CPLog	( DealerId,SalesDealerId,UpdatedBy,UpdatedOn,OldValue,NewValue,SalesMeetingId )
										VALUES	( @DealerId,@SalesId,@UpdatedBy,GETDATE(),@OldClosingProbability,@NewClosingProbability,@SalesMeetingId )
					
					END

				--Now log the record in DCRM_ProductLog for other changes against Dealer in that SalesMeeting
				IF @IsProductLog = 1 AND @SalesId IS NOT NULL
					BEGIN
						INSERT INTO DCRM_ProductLog	( DealerId,SalesDealerId,UpdatedBy,UpdatedOn,OldPackage,NewPackage,
													  OldClosingAmount,NewClosingAmount,OldClosingDate,NewClosingDate,
													  OldDuration,NewDuration,SalesMeetingId,NewComments,OldComments,NewContractId,OldContractId )
											VALUES	( @DealerId,@SalesId,@UpdatedBy,GETDATE(),@OldPitchProdId,@NewPitchProdId,
													  @OldClosingAmnt,@NewClosingAmnt,@OldExpClosingDate,@NewExpClosingDate,
													  @OldPitchDuration,@NewPitchDuratn,@SalesMeetingId,@NewComments,@OldComments,@NewContractType,@OldContractType )
					END
				
			END
		--If no entry exist then add new one
		ELSE IF @Type = 1 AND @SalesMeetingId <> -1
			BEGIN
				INSERT INTO DCRM_SalesDealer
								(	DealerId,EntryDate,LeadSource,ProductStatus,ClosingProbability,ClosingDate,
									ClosingAmount,PitchingProduct,PitchDuration,LeadStatus,UpdatedBy,UpdatedOn,
									CreatedOn,LostReason,InitiatedBy,OtherResons,StartMeetingId,Quantity,CampaignType,
									IsFromMobile,FieldExecutive , BOExecutive,PercentageSlab,FileHostURL,AttachedLPA,
									IsLPAUploaded,NoOfLeads ,Comments ,Model,ExceptionModel,LeadPerDay,StartDate--,IsMultiOutlet
									,ContractType,Source,LPANumber)
						VALUES	(	@DealerId,@TodayDate,31,@ProductRelationshipType,@NewClosingProbability,@NewExpClosingDate,
									@NewClosingAmnt,@NewPitchProdId,@NewPitchDuratn,@NewLeadStatus,@UpdatedBy,@TodayDate,
									@TodayDate,@NewLostReasons,@UpdatedBy,@OtherLostReason,@SalesMeetingId,@NewRSAPackageQunatity,@CampaignType,
									@IsFromMobile, @DealerFieldExecutive , @DealerBackOfficeExecutive , @WarrantyPerSlab,NULL,NULL,
									@IsLpaUploaded,@NoOfLeads,@NewComments,@Model,@ExceptionModel,@LeadPerDay,@StartDate--,ISNULL(@IsMultiOutlet,0)
									,@NewContractType,@Source,@LPANumber) -- -- Mihir A Chheda [10-08-2016] 
		
				SET @NewSalesId = SCOPE_IDENTITY()

				IF(@IsMultiOutlet = 1 AND  (SELECT DISTINCT SalesDealerId FROM DCRM_SalesDealerMapping WHERE SalesDealerId = @NewSalesId)IS NULL)
						BEGIN
							INSERT INTO DCRM_SalesDealerMapping (SalesDealerId,DealerId) 
							SELECT @NewSalesId DCRM_SalesDealerId,ListMember AS DealerId FROM fnSplitCSV(@MultiOutletIds)
						END

				--If user Loss or Converted package against the Dealer then upadte SalesDealer with closed SalesMeetingID
				IF @NewLeadStatus = 2 OR @NewLeadStatus = 3
					BEGIN
						--If product is free trail then create a transaction with 0 amount
						--and also update payment details for that transaction with 0 amount 
						IF ((@CampaignType = 1 OR @CampaignType = 2 OR @CampaignType = 4) AND @NewLeadStatus = 2)
							BEGIN
								INSERT INTO DCRM_PaymentTransaction 
												(TotalClosingAmount,DiscountAmount,ProductAmount,ServiceTax,IsTDSGiven,TDSAmount,FinalAmount,PANNumber,TANNumber,CreatedBy,CreatedOn)
											VALUES
												(0,0,0,null,0,null,0,null,null,@UpdatedBy,GETDATE())
								SET @TransactionId = SCOPE_IDENTITY()
					
								IF @TransactionId IS NOT NULL
									BEGIN
										INSERT INTO DCRM_PaymentDetails(Mode,Amount,PaymentDate,AddedBy,AddedOn,TransactionId,PaymentType,PaymentReceived,AmountReceived)
										VALUES(1,0,GETDATE(),@UpdatedBy,GETDATE(),@TransactionId,1,0,0)

										UPDATE DCRM_SalesDealer SET TransactionId = @TransactionId WHERE Id = @NewSalesId
									END
							END

						UPDATE DCRM_SalesDealer 
							SET ClosedMeetingID = @SalesMeetingId ,UpdatedOn = GETDATE(),CampaignType = @CampaignType, TransactionId = @TransactionId,Model=@Model,ExceptionModel=@ExceptionModel,LeadPerDay=@LeadPerDay
								,IsMultiOutlet = ISNULL(@IsMultiOutlet,0)								
						WHERE 
							Id = @NewSalesId

						IF(@IsMultiOutlet = 1 AND  (
						                            SELECT DISTINCT SalesDealerId 
													FROM DCRM_SalesDealerMapping WITH(NOLOCK)
						                            WHERE SalesDealerId = @NewSalesId)IS NULL
													)
						BEGIN
							INSERT INTO DCRM_SalesDealerMapping (SalesDealerId,DealerId) 
							SELECT @NewSalesId DCRM_SalesDealerId,ListMember AS DealerId FROM fnSplitCSV(@MultiOutletIds)
						END
					END
		
			END

		--now insert attached lpa file name
		IF @NewSalesId > 0 AND (@Type = 1 OR @Type = 0 )
			BEGIN
				SET @RowCount = 1
				WHILE(@RowCount <= @TotalFiles)
					BEGIN 
						SELECT @FileName = LPAName FROM @TempLPANameTable WHERE RowId = @RowCount
						SET @FileName = CAST(@NewSalesId as varchar)+'_'+@FileName 
						INSERT INTO M_AttachedLpaDetails
									(
										SalesDealerId,
										AttachedFileName,
										FileHostUrl,
										UploadedOn,
										UploadedBy
									)
									VALUES
									(
										@NewSalesId,
										@FileName,
										@FileHostURL,
										GETDATE(),
										@UpdatedBy
									)
						SET @RowCount += 1
					END
			END
	END

END
