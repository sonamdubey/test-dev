IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetLeadAutoVerification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetLeadAutoVerification]
GO

	
-- =============================================
-- Author:		Raghu
-- Create date: 10/4/2013
-- Description:	Checks for the working and non-working hrs and holidays
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetLeadAutoVerification] 
    @Id INT,
	@Starttime TINYINT OUTPUT,
	@EndTime TINYINT OUTPUT
AS
BEGIN
	SELECT @Starttime = StartTime ,@EndTime = EndTime 
	FROM [CRM_LeadAutoVerificationConfig] 
	WHERE Id = @Id
END

