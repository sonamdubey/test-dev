IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetRSLeadCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetRSLeadCount]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 14.08.2013
-- Description:	Get the lead count for a particular day for Royal Sundaram
-- =============================================

CREATE PROCEDURE [dbo].[GetRSLeadCount]
	@Mobile varchar(15),
	@ClientId tinyint Output,
	@Count int Output
AS
BEGIN
	--SET @IsSent = 0;
	SET @Count = 0
	SET @ClientId = 0;

	Select distinct @ClientId=ClientId 
	from INS_PremiumLeads WITH (NOLOCK) 
	where Mobile=@Mobile AND RequestDateTime >= GETDATE()-7
	
	IF @ClientId = 0
	BEGIN
		SELECT @Count=Count(ID) 
		FROM INS_PremiumLeads WITH (NOLOCK) 
		WHERE ClientId=2 -- Royal Sundram client ID
		and Convert(date,RequestDateTime) = CONVERT(date,GetDate())
	END
	Select ClientId = @ClientId,Count1 = @Count
	
END


