IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RemoveCWstock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RemoveCWstock]
GO

	-- Modified By: Tejashree Patil On 19 Nov 2012:Added parameter @BranchId and added 'AND CarId=@StockId' condition in DELETE query, 
-- commented @DealerId parameter
-- DECLARE @RetVal TINYINT  execute [TC_RemoveCWstock] 3535,5,@RetVal OUTPUT SELECT @REtVal
-- Modified By: Nilesh Utture on 11th March, 2013 Updated LastUpdatedDate in TC_Stock
-- Modified By:  Manish on 26-07-2013 for maintaining log of the uploaded car
-- Modified By Vivek Gupta on 6th jan 2014 , Added queries to remove youtube link from carvideos and sellinquiriesdetails
-- =============================================
CREATE  PROCEDURE [dbo].[TC_RemoveCWstock]
	@StockId BIGINT,
	@BranchId BIGINT,
	@RetVal TINYINT OUTPUT
AS
BEGIN
	-- Modified By: Tejashree Patil On 19 Nov 2012
	BEGIN TRY
		BEGIN TRANSACTION ProcessTC_RemoveCWstock
	IF EXISTS(SELECT ID FROM TC_Stock WHERE IsSychronizedCW=1 AND BranchId=@BranchId AND Id=@StockId AND IsActive=1 AND StatusId=1)
	BEGIN
		--DECLARE @DealerId INT
		--SELECT @DealerId=BranchId FROM TC_Stock Where Id=@StockId

		UPDATE SellInquiries SET StatusId=2 where TC_StockId = @StockId AND SourceId = 2
		UPDATE dbo.TC_Stock SET IsSychronizedCW =0,IsFeatured=0, LastUpdatedDate = GETDATE()  WHERE Id= @StockId

		--DELETE FROM DealerFeaturedCars WHERE DealerId=@DealerId
		DELETE FROM DealerFeaturedCars WHERE DealerId=@BranchId AND CarId=@StockId 
		
		-------------------------Below insert statement add by Manish on 26-07-2013 for maintaining log of the uploaded car-----------
				INSERT INTO TC_StockUploadedLog(SellInquiriesId, 
												DealerId,
												IsCarUploaded,
												CreatedOn)
								        SELECT  ID,
								                DealerId,
								                0,
								                GETDATE()
								         FROM  SellInquiries WITH (NOLOCK)
								         WHERE TC_StockId = @StockId AND SourceId = 2
        ---------------------------------------------------------------------------------------------------------------------------------------------
		-- Modified By Vivek Gupta on 6th jan 2014
		DECLARE @SellInquiryId INT = (SELECT ID FROM SellInquiries WITH(NOLOCK) WHERE TC_StockId = @StockId AND SourceId = 2)

		UPDATE TC_CarVideos SET StatusId = 2 , IsSellerInq = 0,  ModifiedDate = GETDATE() WHERE StockId = @StockId AND IsActive = 1

		UPDATE CarVideos SET IsActive = 0, IsApproved = 0--, VideoUrl = NULL
		WHERE InquiryId = @SellInquiryId AND IsActive = 1

		UPDATE SellInquiriesDetails SET YoutubeVideo = NULL, IsYouTubeVideoApproved = 0
		WHERE SellInquiryId = @SellInquiryId
		---------------------------------------------------------------------------------------------------------------------------------------------
		SET @RetVal= 1		
	END
	ELSE 
	BEGIN
		SET @RetVal= 2
	END
	COMMIT TRANSACTION ProcessTC_RemoveCWstock
	END TRY

	BEGIN CATCH
		--print 'rollback'
		ROLLBACK TRANSACTION ProcessTC_RemoveCWstock

		INSERT INTO TC_Exceptions (
			Programme_Name
			,TC_Exception
			,TC_Exception_Date
			,InputParameters
			)
		VALUES (
			'TC_RemoveCWstock'
			,(ERROR_MESSAGE() + 'ERROR_NUMBER(): ' + CONVERT(VARCHAR, ERROR_NUMBER()))
			,GETDATE()
			,' @StockId:' + ISNULL(@StockId, 'NULL') + ' @BranchId :' + ISNULL(@BranchId, 'NULL') + ' @RetVal : ' + ISNULL(@RetVal, 'NULL')
			)
			--SELECT ERROR_NUMBER() AS ErrorNumber;
	END CATCH;
END









