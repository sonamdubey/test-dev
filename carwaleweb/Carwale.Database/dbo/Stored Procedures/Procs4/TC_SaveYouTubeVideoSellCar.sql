IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveYouTubeVideoSellCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveYouTubeVideoSellCar]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 25/11/2013
-- Description:	Save youtube video key for stocks for stock lists.
-- Modified By: Vivek Gupta on 20-12-2013, changed IsYouTubeVideoApproved = 0 to IsYouTubeVideoApproved = 1
-- Modified By: Vivek Gupta on 23-12-2013, added a variable @SellInquiryId to get sellinquiryid from stockid(@CarId)
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveYouTubeVideoSellCar]
	-- Add the parameters for the stored procedure here
	@VideoKey VARCHAR(200)
	,@CarId INT
	,--@CarId is stockid that is coming from autobiz
	@BranchId INT
	,@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @TC_CarVideoId INT
	DECLARE @IsSychronizedCW BIT

	SELECT @IsSychronizedCW = IsSychronizedCW
	FROM TC_Stock WITH (NOLOCK)
	WHERE Id = @CarId

	IF (@IsSychronizedCW = 0) --if car is not uploaded in carwale
	BEGIN
		IF NOT EXISTS (
				SELECT Id
				FROM TC_CarVideos WITH (NOLOCK)
				WHERE StockId = @CarId
					AND IsActive = 1
					AND IsMain = 1
				)
		BEGIN
			INSERT INTO TC_CarVideos (
				StockId
				,BranchId
				,IsMain
				,VideoUrl
				,StatusId
				)
			VALUES (
				@CarId
				,@BranchId
				,1
				,@VideoKey
				,2
				)
		END
		ELSE
		BEGIN
			IF (@VideoKey <> '')
			BEGIN
				UPDATE TC_CarVideos
				SET VideoUrl = @VideoKey
					,ModifiedDate = GETDATE()
					,ModifiedBy = @UserId
				WHERE StockId = @CarId
					AND BranchId = @BranchId
					AND IsActive = 1
			END
			ELSE
			BEGIN
				UPDATE TC_CarVideos
				SET ModifiedDate = GETDATE()
					,IsActive = 0
					,IsMain = 0
					,ModifiedBy = @UserId
				--SET VideoUrl = NULL, ModifiedDate = GETDATE(), IsActive = 0, IsMain = 0 , ModifiedBy = @UserId
				WHERE StockId = @CarId
					AND IsActive = 1
			END
		END
	END
END
