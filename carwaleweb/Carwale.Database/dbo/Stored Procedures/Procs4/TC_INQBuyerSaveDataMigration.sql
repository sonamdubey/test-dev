IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQBuyerSaveDataMigration]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQBuyerSaveDataMigration]
GO

	-- Created By:	Surendra
-- Create date: 4th Jan 2013
-- Description:	Adding Buyer's Inquiry with Stock
-- =============================================
 CREATE PROCEDURE [dbo].[TC_INQBuyerSaveDataMigration] 
(        
@AutoVerified BIT, --1 if inquiry ias added from trading cars
@BranchId BIGINT,
@StockId BIGINT,

-- TC_CustomerDetails's related param
@CustomerName VARCHAR(100),
@Email VARCHAR(100),
@Mobile VARCHAR(15),
@Location VARCHAR(50),
@Buytime VARCHAR(20),
--@CustomerComments VARCHAR(400), Removed by surendra

--- Followup related Param
@Comments VARCHAR(500),
@Eagerness SMALLINT,-- Renamed
@NextFollowup DATETIME,
@InquirySource SMALLINT,
----- Other Param
@LeadOwnerId BIGINT,
@CreatedBy BIGINT,
-- Loose Inquiery related
@MinPrice BIGINT,
@MaxPrice BIGINT,
@FromMakeYear SMALLINT,
@ToMakeYear SMALLINT,
@ModelIds VARCHAR(400),
@ModelNames VARCHAR(400),
@BodyTypeIds VARCHAR(200),
@FuelTypeIds VARCHAR(200),
@BodyTypeNames VARCHAR(200),
@FuelTypeNames VARCHAR(200),
@UsedCarPurchaseInquiryId BIGINT =NULL,
@CW_CustomerId BIGINT = NULL,
@SellInquiryId BIGINT = NULL --id FROM SELLERINQUIRIES
)
AS           

BEGIN	
DECLARE @Status SMALLINT 
DECLARE @LeadDivertedTo VARCHAR(100)
SET NOCOUNT ON;	
	-- inserting record in main table (TC_inquiries) of inquiries
	BEGIN TRY
		BEGIN TRANSACTION ProcessBuyerInquiries
					
			--Registering/Updating Customer
			DECLARE @CustomerId BIGINT
			DECLARE @CustStatus SMALLINT
			DECLARE @ActiveLeadId BIGINT
			SET @Status=0
			
			IF(@SellInquiryId IS NOT NULL) -- this means inquiry come from carwale
			BEGIN
				SELECT @BranchId=DealerId,@StockId=TC_StockId FROM SellInquiries WHERE ID=@SellInquiryId
			END


			-- following sp will add or update customer and will return customer staus and active lead
			EXEC TC_CustomerDetailSave @BranchId=@BranchId,@CustomerEmail=@Email,@CustomerName=@CustomerName,@CustomerMobile=@mobile,
				@Location=@Location,@Buytime=@Buytime,@Comments=NULL,@CreatedBy=@CreatedBy,@Address=NULL,@SourceId=@InquirySource,
				@CustomerId=@CustomerId OUTPUT,@Status=@CustStatus OUTPUT,@ActiveLeadId=@ActiveLeadId OUTPUT, @CW_CustomerId=@CW_CustomerId
				
			IF(@CustStatus=1) --Customer Is Fake
			BEGIN
				SET @Status=99		
			END
			ELSE--- Customer is not fake hence proceed
			BEGIN
				
				DECLARE @CarDetails VARCHAR(MAX)
				DECLARE @LeadIdOutput BIGINT,@INQLeadIdOutput BIGINT
				DECLARE @INQDate DATETIME=GETDATE()
				
				IF(@StockId IS NOT NULL)
				BEGIN
					SELECT @CarDetails=V.Make + ' ' + V.Model + ' '  + V.Version + ' '  
					FROM TC_Stock S INNER JOIN vwMMV V ON S.VersionId=V.VersionId
					WHERE S.Id=@StockId
				END
				ELSE
				BEGIN -- loose inquiry
					--SELECT @CarDetails=ISNULL(@ModelIds,'') + ' ' + ISNULL(CONVERT(VARCHAR(10),@MinPrice),'')
					SELECT @CarDetails=([dbo].TC_CarDetailBuyInq(@MinPrice,@MaxPrice,@FromMakeYear,@ToMakeYear,@ModelNames,@BodyTypeNames,@FuelTypeNames))				 
			
				END						
						
					
				EXECUTE TC_INQLeadSave @AutoVerified=@AutoVerified,@BranchId =@BranchId,
						@CustomerId =@CustomerId,
						@LeadOwnerId=@LeadOwnerId,
						@Eagerness =@Eagerness,
						@CreatedBy =@CreatedBy,
						@InquirySource=@InquirySource,
						@LeadId =@ActiveLeadId,
						@INQDate=@INQDate,
						@LeadInqTypeId =1,
						@CarDetails =@CarDetails,
						@LeadStage = NULL,
						@LeadIdOutput= @LeadIdOutput OUTPUT,
						@INQLeadIdOutput= @INQLeadIdOutput OUTPUT,
						@NextFollowupDate=@NextFollowup,
						@FollowupComments =@Comments,
						@ReturnStatus=@Status OUTPUT,
						@LeadDivertedTo=@LeadDivertedTo OUTPUT	
				
				-- Inserting record in Buyer Inquiries
				-- Restriction for same buyer inquiry for same stock with not lead disposition	
				IF(@StockId IS NOT NULL)
				BEGIN
					IF NOT EXISTS(	SELECT TOP 1 TC_BuyerInquiriesId 
									FROM	TC_BuyerInquiries WITH(NOLOCK) 
									WHERE	TC_InquiriesLeadId = @INQLeadIdOutput AND TC_LeadDispositionId IS NULL AND StockId=@StockId)
					BEGIN
						INSERT INTO TC_BuyerInquiries(	StockId,UsedCarPurchaseInquiryId,TC_InquiriesLeadId,CreatedBy,CreatedOn,MakeYearFrom,MakeYearTo,
														PriceMin,PriceMax,TC_InquirySourceId, BuyDate)
						VALUES(@StockId,@UsedCarPurchaseInquiryId,@INQLeadIdOutput,@CreatedBy,@INQDate,@FromMakeYear,@ToMakeYear,@MinPrice,
						@MaxPrice,@InquirySource, CAST(DATEADD(DAY,CONVERT(INT,@Buytime),GETDATE())AS DATETIME))
					END
				END
				ELSE
				BEGIN
					INSERT INTO TC_BuyerInquiries(	StockId,UsedCarPurchaseInquiryId,TC_InquiriesLeadId,CreatedBy,CreatedOn,MakeYearFrom,MakeYearTo,
													PriceMin,PriceMax,TC_InquirySourceId, BuyDate)
					VALUES(@StockId,@UsedCarPurchaseInquiryId,@INQLeadIdOutput,@CreatedBy,@INQDate,@FromMakeYear,@ToMakeYear,@MinPrice,
					@MaxPrice,@InquirySource, CAST(DATEADD(DAY,CONVERT(INT,@Buytime),GETDATE())AS DATETIME))
				END
				
				
				DECLARE @TC_BuyerInquiries BIGINT
				SET @TC_BuyerInquiries=SCOPE_IDENTITY();
				
				-- Adding Model preference for loose inquiry			
				IF(@ModelIds IS NOT NULL)
				BEGIN
					INSERT INTO Tc_PrefModelMake(TC_BuyerInquiriesId,ModelId,CreatedOn)
					SELECT @TC_BuyerInquiries, listmember,@INQDate FROM [dbo].[fnSplitCSV](@ModelIds)				
				END
				
				-- Adding Body Types preferences for loose inquiry				
				IF(@BodyTypeIds IS NOT NULL)
				BEGIN
					INSERT INTO Tc_PrefBodyStyle(TC_BuyerInquiriesId,BodyType,CreatedOn)
					SELECT @TC_BuyerInquiries, listmember,@INQDate FROM [dbo].[fnSplitCSV](@BodyTypeIds)				
				END
				
				-- Adding fueltype preferences for loose inquiry
				IF(@FuelTypeIds IS NOT NULL)
				BEGIN
					INSERT INTO TC_PrefFuelType(TC_BuyerInquiriesId,FuelType,CreatedOn)
					SELECT @TC_BuyerInquiries, listmember,@INQDate FROM [dbo].[fnSplitCSV](@FuelTypeIds)				
				END
											
				IF(@StockId IS NOT NULL)
				BEGIN				
					IF NOT EXISTS(SELECT StockId FROM TC_StockAnalysis WHERE StockId = @StockId )
					BEGIN
						INSERT INTO TC_StockAnalysis(StockId, CWResponseCount, TCResponseCount) VALUES(@StockId, 0, 0)
					END

					IF (@InquirySource = 1 ) -- CarWale as source
						BEGIN
							-- Update CWResponseCount to Table.
							UPDATE TC_StockAnalysis Set CWResponseCount = CWResponseCount + 1 WHERE StockId = @StockId
						END
					ELSE -- Any other source
						BEGIN
							-- Update TCResponseCount to Table.
							UPDATE TC_StockAnalysis Set TCResponseCount = TCResponseCount + 1 WHERE StockId = @StockId
						END	
				END
								
				--SET @Status=1
			END	
	
		COMMIT TRANSACTION ProcessBuyerInquiries
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRANSACTION ProcessBuyerInquiries
		DECLARE @Inputparameters VARCHAR(MAX)
		SET @Inputparameters='@AutoVerified:' + CAST(@AutoVerified AS VARCHAR(5))+',@BranchId:' + CAST( ISNULL(@BranchId,-1)  AS VARCHAR(50))
							+ ',@StockId:' + CAST(ISNULL(@StockId,-1)  AS VARCHAR(50))+',@CustomerName:' + ISNULL(@CustomerName,'') +',@Email: '+ ISNULL(@Email,'')
							+ ',@Mobile:'+ ISNULL(@Mobile,'') +',@Location:' + ISNULL(@Location,'') +',@Buytime:'+ ISNULL(@Buytime,'') +',@Comments:'+ ISNULL(@Comments,'')
							+ ',@Eagerness:' + CAST(ISNULL(@Eagerness,'') AS VARCHAR(50))+ ',@InquirySource:' + CAST(ISNULL(@InquirySource,0) AS VARCHAR(50))
							+ ',@LeadOwnerId:' + CAST(ISNULL(@LeadOwnerId,0) AS VARCHAR(50))+',@CreatedBy:' + CAST(ISNULL(@CreatedBy,0) AS VARCHAR(50))
							+ ',@MinPrice:' + CAST(ISNULL(@MinPrice,-1)  AS VARCHAR(50))+',@MaxPrice:' + CAST(ISNULL(@MaxPrice,-1)  AS VARCHAR(50))
							+ ',@FromMakeYear:' + CAST(ISNULL(@FromMakeYear,-1) AS VARCHAR(50))+ ',@ToMakeYear:' + CAST(@ToMakeYear  AS VARCHAR(50))
							+ ',@ModelIds:'+ ISNULL(@ModelIds,'') +',@ModelNames:'+ ISNULL(@ModelNames,'') +',@BodyTypeIds:' + ISNULL(@BodyTypeIds,'')
							+ ',@FuelTypeIds:' +ISNULL(@FuelTypeIds,'') +',@BodyTypeNames:'+ISNULL(@BodyTypeNames,'') +',@FuelTypeNames:' +ISNULL(@FuelTypeNames,'') 
							+ ',@UsedCarPurchaseInquiryId:' + CAST(ISNULL(@UsedCarPurchaseInquiryId,-1)  AS VARCHAR(50)) 
							+ ',@CW_CustomerId:' + CAST(ISNULL(@CW_CustomerId,-1) AS VARCHAR(50)) +',@SellInquiryId:' + CAST(ISNULL(@SellInquiryId,-1) AS VARCHAR(50))

		 INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date,Inputparameters)
         VALUES('TC_INQBuyerSave',(ERROR_MESSAGE()+', ERROR_NUMBER(): '+ERROR_NUMBER()),GETDATE(),@Inputparameters)
		--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END




