IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NewCarTestDriveReq_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NewCarTestDriveReq_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: Dec 16, 2008
-- Description:	SP to save or update test drive request for new car
-- =============================================
CREATE PROCEDURE [dbo].[NewCarTestDriveReq_SP]
	-- Add the parameters for the stored procedure here
	@QuoteId		NUMERIC,
	@Address		VARCHAR(100),
	@TestDriveDate	DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	If Exists( Select QuoteId FROM NewCarTestDriveReq WHERE QuoteId = @QuoteId )
		BEGIN
			UPDATE NewCarTestDriveReq Set TestDriveDate = @TestDriveDate, Address = @Address  
			WHERE QuoteId = @QuoteId
		END
	ELSE
		BEGIN
			INSERT INTO NewCarTestDriveReq(QuoteId, Address, TestDriveDate) 
			VALUES(@QuoteId, @Address, @TestDriveDate)
		END	
END