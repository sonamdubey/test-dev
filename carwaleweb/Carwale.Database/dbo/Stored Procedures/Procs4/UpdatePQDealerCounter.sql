IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdatePQDealerCounter]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdatePQDealerCounter]
GO

	

-- =============================================
-- Author:		Vinayak Mishra
-- Create date: 06/11/2014
-- Description:	To Update counter for pqdealersponsored and stop the updation of counter for the same Ad click by same user.
-- exec [dbo].[UpdatePQDealerCounter] 35,1,""
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePQDealerCounter] 
	-- Add the parameters for the stored procedure here
	@Id NUMERIC
	,@PushStatus INT
	,@CampaignId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @PushStatusCount INT
	DECLARE @dealerType INT
    -- Insert statements for procedure here
	SET @PushStatusCount=(select count(1) from PQDealerAdLeads WITH (NOLOCK) WHERE PushStatus=@PushStatus)
	IF @PushStatusCount<2  --If previously exists then the count is 2.So for any @pushstatuscount<2 the condition will execute 
	BEGIN
		select @dealerType=Type from PQ_DealerSponsored WITH (NOLOCK) where Id=@CampaignId
		IF @dealerType=2
		BEGIN
			UPDATE PQ_DealerSponsored SET DailyCount+=1,TotalCount+=1 where Id=@CampaignId
		END
	END
END
