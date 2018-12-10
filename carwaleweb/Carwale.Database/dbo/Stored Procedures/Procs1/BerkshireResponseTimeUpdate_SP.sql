IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireResponseTimeUpdate_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireResponseTimeUpdate_SP]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 8 Jan 2013
-- Description:	Proc will update the ResponseEntryDate for the provided berkshire lead id
-- Modified By : Ashish G. Kamble on 15 Jan 2013
-- Description : Made update statement conditional.
-- =============================================
CREATE PROCEDURE [dbo].[BerkshireResponseTimeUpdate_SP]
	-- Add the parameters for the stored procedure here
	@LeadId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Update response time only once. If responseEntryDate is Null.
	IF EXISTS(SELECT BerkshireLeadId FROM BerkshireInsuranceLeads WHERE BerkshireLeadId = @LeadId AND ResponseEntryDate IS NULL)
	BEGIN
		UPDATE BerkshireInsuranceLeads
		SET ResponseEntryDate = GETDATE()
		WHERE BerkshireLeadId = @LeadId
	END
END
