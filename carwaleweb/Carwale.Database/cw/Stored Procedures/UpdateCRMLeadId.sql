IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[UpdateCRMLeadId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[UpdateCRMLeadId]
GO

	
-- =============================================
-- Author:		Avishkar
-- Create date: 19-10-2012
-- Description:	Set CRM_LeadId in NewCarPurchaseInquiries table
-- Modified by Rohan.s on 27-05-2015 , UPDATE PushStatus Column in PQDealerAdLeads Table
-- =============================================
CREATE PROCEDURE [cw].[UpdateCRMLeadId] @PQId NUMERIC(18, 0)
	,@CRMLeadId NUMERIC(18, 0)
	,@PQDealerAdLeads_ID NUMERIC(18, 0) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @PQDealerAdLeads_ID IS NOT NULL
	BEGIN
		UPDATE PQDealerAdLeads
		SET PushStatus = @CRMLeadId
		WHERE Id = @PQDealerAdLeads_ID
	END

	UPDATE NewCarPurchaseInquiries
	SET CRM_LeadId = @CRMLeadId
	WHERE Id = @PQId
END
