IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[UpdatePreVerLeadScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[UpdatePreVerLeadScore]
GO

	-- =============================================
-- Author:		Naresh Palaiya
-- Create date: 21/05/2014
-- Description:	Update the Previous version Lead score into tables CRM_Leads table
-- =============================================
CREATE PROCEDURE [CRM].[UpdatePreVerLeadScore] 
	-- Add the parameters for the stored procedure here
	@LeadID bigint,
	@LeadScore float  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    	
	IF(@LeadId <> -1)
	BEGIN
--Update the Previous version score into the CRM_Leads table
	UPDATE CL
	SET PreVerLeadScore=@LeadScore
	FROM CRM_Leads CL where CL.ID = @LeadID	
	END 	   
END
