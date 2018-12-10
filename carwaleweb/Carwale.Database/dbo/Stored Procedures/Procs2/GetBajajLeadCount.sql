IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetBajajLeadCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetBajajLeadCount]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 16.10.2013
-- Description:	Get the daily and monthly lead count for a for Bajaj Alliance
-- =============================================

CREATE PROCEDURE [dbo].[GetBajajLeadCount]
	@ClientId tinyint,
	@MonthlyCount int Output,
	@DailyCount int Output
AS
BEGIN

Select @MonthlyCount = Count(ID)
FROM INS_PremiumLeads WITH (NOLOCK) 
WHERE ClientId = 1 AND DATEPART(month,RequestDateTime) = DATEPART(month,GETDATE()) 

SELECT @DailyCount = Count(ID) 
FROM INS_PremiumLeads WITH (NOLOCK) 
WHERE ClientId=1 
and Convert(date,RequestDateTime) = CONVERT(date,GetDate())

END



