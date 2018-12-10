IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MSMQCheck]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MSMQCheck]
GO

	-- =============================================
-- Author:		Raghu
-- Create date: 29/4/2013
-- Description:	<Returns a bit which signifies whether the lead has to be pushed into the MSMQ or not> 
-- Modified by : Raghu on <30/12/2013> Added WITH(NOLOCK) Condition
--DECLARE @Bit VARCHAR(20) EXEC [MSMQCheck] "501;27;1;",@Bit OUTPUT SELECT @Bit
-- =============================================

CREATE procedure [dbo].[MSMQCheck] 
	-- Add the parameters for the stored procedure here
		@VersionId INT,
		@Bit BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ModelId INT,@MakeId INT
	 
	SELECT @MakeId = MV.MakeId,@ModelId = MV.ModelId FROM vwMMV MV WHERE VersionId = @VersionId

    -- Insert statements for procedure here
	IF EXISTS(
	SELECT Top 1
	ThirdPartyLeadSettingId
	FROM [ThirdPartyLeadSettings] WITH(NOLOCK)
	WHERE MakeId = @MakeId AND ModelId = @ModelId
	 AND IsActive = 1
		AND DATEDIFF(DD,GETDATE(),CampaignEndDate) > 0
		AND (LeadVolume - LeadsSent) > 0	 	
	) 
	
	BEGIN
		SET @Bit=1
	END
	ELSE
	BEGIN
		SET @Bit=0
	END
		
END



/****** Object:  StoredProcedure [dbo].[CW_InsertNewCarPurchaseInquiry]    Script Date: 12/30/2013 2:07:36 PM ******/
SET ANSI_NULLS OFF

