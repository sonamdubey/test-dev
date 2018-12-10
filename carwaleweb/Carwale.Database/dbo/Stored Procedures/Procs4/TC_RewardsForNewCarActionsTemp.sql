IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RewardsForNewCarActionsTemp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RewardsForNewCarActionsTemp]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 24-04-2015
-- Description:	Reward Calculations
-- Modified By Vivek on 3-7-2015, changed conditions for first call to lead
-- =============================================
CREATE PROCEDURE [dbo].[TC_RewardsForNewCarActionsTemp]
@BranchId INT,
@TC_DealerTypeId INT,
@Date DATETIME,

@PQP NUMERIC = 100 --@PQIn24Hours
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE 
		    @PQPId SMALLINT = 13 --@PQIn24HoursId
			
	-- Declaring Different Variables to get different points of the dealer

	DECLARE 
			@PriceQuotePW NUMERIC = 0

    DECLARE 
			@PriceQuotePA NUMERIC = 0

	DECLARE @EntryDate DATETIME
	SET @EntryDate = CONVERT(DATE,@Date)


		--Calls taken on leads came after 6 pm yesterday and before 6pm today
			--This should just be a per lead called on the same day for leads received till 6pm, 
			--and on next calendar day for all leads received after 6pm. Max once per lead.
			   --Actions from web
			DECLARE @CallLeadCount NUMERIC = 0
			SET @CallLeadCount = 0
			SELECT @CallLeadCount = COUNT(DISTINCT C.TC_LeadId) FROM TC_Lead L WITH(NOLOCK)
			JOIN   TC_Calls C WITH(NOLOCK) ON L.TC_LeadId = C.TC_LeadId AND C.IsActionTaken = 1 AND C.CallType = 1 AND L.LeadType = 3 AND ISNULL(C.TC_ActionApplicationId,1) = 1
			WHERE 
				 L.BranchId = @BranchId
				AND C.ScheduledOn >  DATEADD(day, DATEDIFF(day, 0, @Date - 1), '18:00:00.000')
				--AND L.LeadCreationDate <  DATEADD(day, DATEDIFF(day, 0, GETDATE()), '18:00:00.000')
				AND L.LeadCreationDate BETWEEN  DATEADD(day, DATEDIFF(day, 1, @Date), '18:00:00.000') AND DATEADD(day, DATEDIFF(day, 0, @Date), '18:00:00.000')
				AND C.ActionTakenOn <= DATEADD(day, DATEDIFF(day, 0, @Date), '18:00:00.000')
				
			SET @PriceQuotePW = @PQP * @CallLeadCount
			
			-- Actions from App
			SELECT @CallLeadCount = COUNT(DISTINCT C.TC_LeadId) FROM TC_Lead L WITH(NOLOCK)
			JOIN   TC_Calls C WITH(NOLOCK) ON L.TC_LeadId = C.TC_LeadId AND C.IsActionTaken = 1 AND C.CallType = 1 AND L.LeadType = 3 AND ISNULL(C.TC_ActionApplicationId,1) = 2
			WHERE 
				 L.BranchId = @BranchId
				AND C.ScheduledOn >  DATEADD(day, DATEDIFF(day, 0, @Date - 1), '18:00:00.000')
				--AND L.LeadCreationDate <  DATEADD(day, DATEDIFF(day, 0, GETDATE()), '18:00:00.000')
				AND L.LeadCreationDate BETWEEN  DATEADD(day, DATEDIFF(day, 1, @Date), '18:00:00.000') AND DATEADD(day, DATEDIFF(day, 0, @Date), '18:00:00.000')
				AND C.ActionTakenOn <= DATEADD(day, DATEDIFF(day, 0, @Date), '18:00:00.000')
				
			SET @PriceQuotePA = 2 * @PQP * @CallLeadCount

			-- inserting reward points to reward table
			EXEC TC_InsertRewardPoints  @DealerId = @BranchId,
										@EntryDate = @EntryDate,
										@TC_DealerTypeId = @TC_DealerTypeId,
										@TC_RewardPointsId = @PQPId,
										@RewardPoints = @PQP,
										@TotalRewardsFromWeb = @PriceQuotePW,
										@TotalRewardsFromApp = @PriceQuotePA


END
/****** Object:  StoredProcedure [dbo].[TC_RewardsForUsedCarActionsTemp]    Script Date: 07/07/2015 17:20:17 ******/
SET ANSI_NULLS ON
