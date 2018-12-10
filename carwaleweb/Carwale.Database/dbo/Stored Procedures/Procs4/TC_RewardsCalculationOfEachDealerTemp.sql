IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsCalculationOfEachDealerTemp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsCalculationOfEachDealerTemp]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 28-04-2015
-- Description:	Extracting Dealers And Calculating their reward points daily
-- Modified By Vivek Gupta on 17-06-2015, removed condition of paid dealers, coz need to calculate points for every dealer
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsCalculationOfEachDealerTemp]
@Date DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--DECLARE @Date DATETIME = NULL

	IF @Date IS NULL
	SET @Date = GETDATE()

	DECLARE @FPBId SMALLINT = 21,--Follow Up Points Id
		    @BBPBId SMALLINT = 22, --Bike Booking Points Id Bikewale
			@PCPBId SMALLINT = 23 --Package Continuation Points Id Bikewale


	DECLARE @FPB NUMERIC = 5,--Follow Up Points Id
		    @BBPB NUMERIC = 25, --Bike Booking Points Id Bikewale
			@PCPB NUMERIC = 100 --Package Continuation Points Id Bikewale



	--SELECT  @FPB  = Points FROM TC_RewardPoints WHERE Id=@FPBId  
	--SELECT  @BBPB  = Points FROM TC_RewardPoints WHERE Id=@BBPBId  
	--SELECT  @PCPB  = Points FROM TC_RewardPoints WHERE Id=@PCPBId  

	
	
	---BikeWale Reward Program Points Calculation---
	DECLARE @TempBikewaleDealers TABLE (DealerId INT, TC_DealerTypeId INT)
	DECLARE @DealerId INT, @TC_DealerTypeId INT  

	INSERT INTO @TempBikewaleDealers
	SELECT Id, TC_DealerTypeId FROM Dealers WITH(NOLOCK) WHERE ApplicationId = 2 AND IsDealerActive = 1

	WHILE EXISTS (SELECT TOP 1 DealerId FROM @TempBikewaleDealers)
	BEGIN
		SET @DealerId = (SELECT TOP 1 DealerId FROM @TempBikewaleDealers)
		SET @TC_DealerTypeId = 2
															   
		EXEC TC_RewardsForNewCarActionsBikeWale @DealerId, @TC_DealerTypeId,@Date,@FPB,@BBPB ,@PCPB
		
		DELETE FROM @TempBikewaleDealers WHERE DealerId = @DealerId

	END

END
