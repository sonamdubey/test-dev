IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_EditDealerDisclaimer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_EditDealerDisclaimer]
GO

	-- =============================================
-- Author:		Suresh Prajapati
-- Create date: 08th Dec, 2014
-- Description:	Procedure to Edit Added Dealer Disclaimer Specifies by ID. 
-- =============================================
CREATE PROCEDURE [dbo].[BW_EditDealerDisclaimer]
	-- Add the parameters for the stored procedure here
	@DisclaimerId INT
	,@NewDisclaimer VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	UPDATE BW_DealerDisclaimer
	SET Disclaimer=@NewDisclaimer WHERE ID=@DisclaimerId
END

