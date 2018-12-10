IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SellCarUpdateCurrentStep]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SellCarUpdateCurrentStep]
GO

	-- =============================================
-- Author:		amit verma
-- Create date: 11/14/2013
-- Description:	Update current step in CustomerSellInquiries table for a specific ID
-- =============================================
CREATE PROCEDURE SellCarUpdateCurrentStep
	-- Add the parameters for the stored procedure here
	@CarId NUMERIC(18,0),
	@CurrentStep TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CustomerSellInquiries
	SET CurrentStep = @CurrentStep WHERE ID = @CarId
END

