IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ReactivateStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ReactivateStock]
GO

	-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 14 Dec,2011
-- Description:	This Procedure is used to make suspended stocks to available
-- Modified By : Teajshree Patil Desc:Only Date considered in condition instead of DateTime
-- Modified By : Surendra Desc: Now Package type is also Updating in Sellinquiries table PackageType=@PackageType (Issue: If new Package is assigned to dealer 
-- carpackge were not reflecting hence care were displaying not available
-- Modified By:  Manish on 26-07-2013 for maintaining log of the uploaded car
-- =============================================
CREATE PROCEDURE [dbo].[TC_ReactivateStock]
(
@BranchId NUMERIC,-- DelerId
@StockIds VARCHAR(MAX)=NULL,--this param contain comma seperated stock ids
@UserId INT -- Primary key of TC_users's Table is used for modifiedby
)	
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Getting PAckage Expiry Date from ConsumerCreditPoints
	DECLARE @ExpDate DATETIME
	DECLARE @Points INT
	DECLARE @ActiveStock INT
	DECLARE @UpdatingCount INT
	DECLARE @PackageType SMALLINT
	
	SELECT @ActiveStock=COUNT(*) FROM  TC_Stock WHERE BranchId=@BranchId AND StatusId=1 AND IsActive=1 AND IsSychronizedCW=1
	SELECT Top 1 @ExpDate=ExpiryDate,@Points=Points,@PackageType=PackageType FROM ConsumerCreditPoints WHERE ConsumerType=1 AND ConsumerId=@BranchId ORDER BY ExpiryDate DESC
	
	--Modified By : Teajshree Patil
	--Only Date considered in condition instead of DateTime
	IF(CAST(CONVERT(VARCHAR(8), @ExpDate, 10) AS DATE)<CAST(CONVERT(VARCHAR(8), GETDATE(), 10) AS DATE))
	BEGIN
		RETURN -1
	END

	
	IF @StockIds IS NULL -- It means All Suspended Stock need to Reactivate
	BEGIN
		SELECT @UpdatingCount=COUNT(*) FROM TC_Stock WHERE BranchId=@BranchId AND StatusId=4 AND IsActive=1 -- Suspended stock count
		IF((@UpdatingCount+@ActiveStock)>@Points )
		BEGIN
			RETURN -1
		END
		
		--Updating SellInquiries Table to make stock Active by Updating Expiry date
		--UPDATE SellInquiries SET PackageExpiryDate=@ExpDate,LastUpdated=GETDATE(),ModifiedBy=@UserId WHERE DealerId=@BranchId AND StatusId=1	
		
		UPDATE SellInquiries 
			SET PackageExpiryDate=@ExpDate,LastUpdated=GETDATE(),ModifiedBy=@UserId ,PackageType=@PackageType
		FROM SellInquiries as S
		  JOIN TC_Stock as T on S.TC_StockId=T.Id
		WHERE S.DealerId=@BranchId 
		AND S.StatusId=1
		AND T.StatusId=4

          -------------------------Below insert statement add by Manish on 26-07-2013 for maintaining log of the uploaded car-----------
				INSERT INTO TC_StockUploadedLog(SellInquiriesId, 
												DealerId,
												IsCarUploaded,
												CreatedOn)
								        SELECT  S.ID,
								                DealerId,
								                1,
								                GETDATE()
								         FROM SellInquiries as S WITH (NOLOCK)
										 JOIN TC_Stock as T WITH (NOLOCK) on S.TC_StockId=T.Id
										 WHERE S.DealerId=@BranchId 
										 AND S.StatusId=1
										 AND T.StatusId=4
     ---------------------------------------------------------------------------------------------------------------------------------------------




		UPDATE TC_Stock SET StatusId=1,IsSychronizedCW=1,LastUpdatedDate=GETDATE(),ModifiedBy=@UserId WHERE BranchId=@BranchId AND StatusId=4	
		RETURN 0
	END
	ELSE -- It means only comma seperated Stock ids need to Reactivate
		BEGIN
			
			SELECT @UpdatingCount =dbo.GetOccurrence(@StockIds,',')
			IF((@UpdatingCount+1+@ActiveStock)>@Points )
			BEGIN
				RETURN -1
			END
			
				
			DECLARE @Separator_position INT -- This is used to locate each separator character  
			DECLARE @StockId VARCHAR(1000) -- this holds each array value as it is returned  
		  -- For my loop to work I need an extra separator at the end. I always look to the  
		  -- left of the separator character for each array value  
		  
			SET @StockIds = @StockIds + ','  
		  
		  -- Loop through the string searching for separtor characters    
			WHILE PATINDEX('%' + ',' + '%', @StockIds) <> 0   
			BEGIN  			
				-- patindex matches the a pattern against a string  
				SELECT  @Separator_position = PATINDEX('%' + ',' + '%',@StockIds)  
				SELECT  @StockId = LEFT(@StockIds, @Separator_position - 1)  
				
				--Updating SellInquiries Table to make stock Active by Updating Expiry date
				UPDATE SellInquiries SET PackageExpiryDate=@ExpDate,LastUpdated=GETDATE(),ModifiedBy=@UserId,PackageType=@PackageType
				WHERE DealerId=@BranchId AND TC_StockId=@StockId AND StatusId=1
				
				-- Updating TC_Stock for making stock availabe
				UPDATE TC_Stock SET StatusId=1,IsSychronizedCW=1,LastUpdatedDate=GETDATE(),ModifiedBy=@UserId
				WHERE Id=@StockId AND BranchId=@BranchId AND StatusId=4
				
				
	   -------------------------Below insert statement add by Manish on 26-07-2013 for maintaining log of the uploaded car-----------
				INSERT INTO TC_StockUploadedLog(SellInquiriesId, 
												DealerId,
												IsCarUploaded,
												CreatedOn)
								        SELECT  ID,
								                DealerId,
								                1,
								                GETDATE()
								         FROM  SellInquiries
								         WHERE TC_StockId = @StockId
								           AND DealerId=@BranchId 
								           AND StatusId=1
     ---------------------------------------------------------------------------------------------------------------------------------------------
				
	            
				-- This replaces what we just processed with and empty string  
				SELECT  @StockIds = STUFF(@StockIds, 1, @Separator_position, '')  
			END 
			RETURN 0
		END	-- while end	
	
END
