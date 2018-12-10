IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewCarQuoteChoosen_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewCarQuoteChoosen_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: Oct-16-2008
-- Description:	SP to Map QuoteId LoanId and Bank Option choosen for loan 
-- =============================================
CREATE PROCEDURE [dbo].[NewCarQuoteChoosen_SP] 
	-- Add the parameters for the stored procedure here
	@QuoteId	NUMERIC,
	@LoanId		NUMERIC,
	@FaId		INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO NewCarQuoteChoosen VALUES(@QuoteId, @LoanId, @FaId)
END
