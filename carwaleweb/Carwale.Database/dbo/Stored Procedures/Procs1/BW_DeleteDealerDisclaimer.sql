IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_DeleteDealerDisclaimer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_DeleteDealerDisclaimer]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 08th Dec, 2014
-- Description:	Procedure to Delete Added Dealer Disclaimer Specifies by ID. 
-- =============================================
CREATE PROCEDURE [dbo].[BW_DeleteDealerDisclaimer]
	-- Add the parameters for the stored procedure here
	@DisclaimerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	UPDATE BW_DealerDisclaimer
	SET IsActive=0 WHERE ID=@DisclaimerId
END

