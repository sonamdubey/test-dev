IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateThirdPartyInquiryDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateThirdPartyInquiryDetails]
GO

	


-- =============================================
-- Author:		Vinayak
-- Create date: 30/11/2015
-- Description:	To Update response after calling third party api
-- =============================================
CREATE PROCEDURE [dbo].[UpdateThirdPartyInquiryDetails] 
	-- Add the parameters for the stored procedure here
	@Id INT
	,@PqDealerAdLeadId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE CarTradeNewCarLeads
	SET PqDealerLeadId =@PqDealerAdLeadId
	WHERE  CarTradeNewCarLeadsId = @Id
	
END

