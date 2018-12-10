IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetDealerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetDealerId]
GO

	-- =============================================
-- Author:   Vinay Kumar Prajapati 
-- Create date: Jun 25,2015
-- Description:	To get dealerId On basis of CarId Absure
-- Updated By 1: Vinay Kumar Prajapati , To get dealerId(BranchId)
-- Updated By 2: Ruchira Patil , 27th July 2015 (to fetch RC not Mandatory)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetDealerId]
(
	@AbSure_CarDetailsId	BIGINT	
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @StockId		NUMERIC,
			@DealerId		BIGINT ,
			@IsRCPending	BIT,
			@Status			INT

	--SELECT  @DealerId = CD.DealerId FROM AbSure_CarDetails AS CD WITH(NOLOCK) WHERE CD.Id = @AbSure_CarDetailsId
	SELECT @StockId = StockId FROM AbSure_CarDetails AS CD WITH(NOLOCK)  WHERE CD.Id=@AbSure_CarDetailsId
	IF @StockId IS NOT NULL AND @StockId > 0
		BEGIN
			SELECT @DealerId=ISNULL(TS.BranchId,-1) , @IsRCPending = ISNULL(CD.RCImagePending,0), @Status = CD.Status
			FROM AbSure_CarDetails  AS CD WITH(NOLOCK) 
			INNER JOIN TC_Stock AS TS WITH(NOLOCK) ON TS.Id=CD.StockId
			INNER JOIN Dealers AS D WITH(NOLOCK) ON D.ID = TS.BranchId
			WHERE CD.Id=@AbSure_CarDetailsId
		END
	ELSE
		BEGIN		     
			 SELECT @DealerId=ISNULL(CD.DealerId,-1) , @IsRCPending = ISNULL(CD.RCImagePending,0), @Status = CD.Status
			 FROM AbSure_CarDetails AS CD WITH(NOLOCK)
			 INNER JOIN Dealers AS D WITH(NOLOCK) ON D.ID = CD.DealerId 
			 WHERE CD.Id=@AbSure_CarDetailsId			 
		END
	
	--Added By: Yuga Hatolkar
	SELECT @DealerId DealerId,@IsRCPending IsRCPending, @Status	Status, 
	(SELECT COUNT(*) FROM AbSure_DoubtfulCarReasons WITH(NOLOCK) WHERE AbSure_CarDetailsId = @AbSure_CarDetailsId AND DoubtfulReason IN(1, 2) AND IsActive = 1) DoubtfulCount	
END

