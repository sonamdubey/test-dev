IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ThirdPartyPushCheck]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ThirdPartyPushCheck]
GO

	
-- =============================================
-- Author:		Sanjay Soni
-- Create date: 1/2/2016
-- Description:	<Returns a bit which signifies whether the lead has to be pushed into the Third Party or not> 
-- =============================================
CREATE PROCEDURE [dbo].[ThirdPartyPushCheck]
	-- Add the parameters for the stored procedure here
	@ModelId INT
	,@Bit BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (
			SELECT TOP 1 ThirdPartyLeadSettingId
			FROM ThirdPartyLeadSettings T WITH (NOLOCK)
			WHERE (
					ModelId = @ModelId
					OR (
						T.ModelId = - 1
						AND MakeId = (
							SELECT CarMakeId
							FROM CarModels WITH (NOLOCK)
							WHERE ID = @ModelId
							)
						)
					)
				AND IsActive = 1
				AND LeadVolume > LeadsSent
				AND GETDATE() >= CampaignStartDate
				AND GETDATE() <= CampaignEndDate
			)
		SET @Bit = 1
	ELSE
		SET @Bit = 0
END
