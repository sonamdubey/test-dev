IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BerkshireInsuranceUpdateJsonString_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BerkshireInsuranceUpdateJsonString_SP]
GO

	
-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 6/4/2012
-- Description:	Procedure will insert berkshire insurance quotation parameters into BerkshireInsuranceLeads table.
-- =============================================
CREATE PROCEDURE [dbo].[BerkshireInsuranceUpdateJsonString_SP]
	-- Add the parameters for the stored procedure here
	@LeadId						BIGINT,
	@JsonRequestString			VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
		
	UPDATE dbo.BerkshireInsuranceLeads
	SET JsonRequestString = @JsonRequestString
	WHERE BerkshireLeadId = @LeadId;	
END

