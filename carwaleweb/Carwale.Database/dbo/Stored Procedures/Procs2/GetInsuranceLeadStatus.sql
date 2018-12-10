IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetInsuranceLeadStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetInsuranceLeadStatus]
GO

	
-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 24 April 2015
-- Description:	Proc to get the chola mandalam insurance status for given cw lead id
-- =============================================
CREATE PROCEDURE [dbo].[GetInsuranceLeadStatus]	
	@Id BIGINT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT PushStatus AS Status, StatusId, Quotation
	FROM INS_PremiumLeads
	WHERE ID = @Id
END
