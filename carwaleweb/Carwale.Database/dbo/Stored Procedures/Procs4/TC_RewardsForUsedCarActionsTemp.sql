IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsForUsedCarActionsTemp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsForUsedCarActionsTemp]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 24-04-2015
-- Description:	Reward Calculations
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsForUsedCarActionsTemp]
	@BranchId INT,
	@TC_DealerTypeId INT,
	@Date DATETIME,	
	@WAP NUMERIC = 100 --@CallsOnLead	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements. tc_rewardpoints
	SET NOCOUNT ON;

    DECLARE 
			@WAPId SMALLINT = 5

	DECLARE @EntryDate DATETIME
	SET @EntryDate = CONVERT(DATE,@Date)
			-- Declaring Different Variables to get different points of the dealer

   -- Pints Variable decalred for storing points earned from web action
	DECLARE 
			@WarrantiesActivatedPW NUMERIC = 0

      -- Pints Variable decalred for storing points earned from aPP action
	DECLARE
			@WarrantiesActivatedPA NUMERIC = 0

    
	DECLARE @WarrantyCount NUMERIC = NULL
			SELECT @WarrantyCount = COUNT(Absure_ActivatedWarrantyId)
			FROM AbSure_ActivatedWarranty WITH(NOLOCK)
			WHERE DealerId = @BranchId
			AND ISNULL(TC_ActionApplicationId,1) = 1
			AND CONVERT(DATE,EntryDate) = CONVERT(DATE,@Date)

			SET @WarrantiesActivatedPW = @WarrantyCount * @WAP

		--warranties activated through App
			SELECT @WarrantyCount = COUNT(Absure_ActivatedWarrantyId)
			FROM AbSure_ActivatedWarranty WITH(NOLOCK)
			WHERE DealerId = @BranchId
			AND ISNULL(TC_ActionApplicationId,1) = 2
			AND CONVERT(DATE,EntryDate) = CONVERT(DATE,@Date)

			SET @WarrantiesActivatedPA = 2* @WarrantyCount * @WAP
					
			--inserting points to the table
			EXEC TC_InsertRewardPoints		@DealerId = @BranchId,
											@EntryDate = @EntryDate,
											@TC_DealerTypeId = @TC_DealerTypeId,
											@TC_RewardPointsId = @WAPId,
											@RewardPoints = @WAP,
											@TotalRewardsFromWeb = @WarrantiesActivatedPW,
											@TotalRewardsFromApp = @WarrantiesActivatedPA

END
