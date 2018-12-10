IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCampaignMaskingNumber]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCampaignMaskingNumber]
GO

	-- =============================================
-- Author:		Vicky Lund
-- Create date: 29th Jan 2016
-- Description:	To update PQ_dealersponsored phone column
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCampaignMaskingNumber] @CampaignId INT
	,@Mobile VARCHAR(50)
	,@LastUpdatedBy INT
AS
BEGIN
	UPDATE PQ_DealerSponsored
	SET Phone = @Mobile
		,UpdatedBy = @LastUpdatedBy
		,UpdatedOn = getdate()
	WHERE Id = @CampaignId
END


