IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCustomerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCustomerId]
GO

	-- =============================================
-- Author:		Raghupathy
-- Create date: 19-10-2012
-- Description:	Set CRM_LeadId in NewCarPurchaseInquiries table
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCustomerId]
	@PQId numeric(18,0),
	@CustomerId numeric(18,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE NewCarPurchaseInquiries	
    SET CustomerId = @CustomerId
    WHERE Id=@PQId
END


