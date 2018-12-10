IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_TransferFiles]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_TransferFiles]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 24/12/2013
-- Description:	Transfer files from a user to another user
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_TransferFiles]
	-- Add the parameters for the stored procedure here
	@SourceUserID NUMERIC(18,0),
	@TargetUserID NUMERIC(18,0),
	@Files VARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE AxisBank_CarValuations
	SET CustomerId = @TargetUserID
	WHERE FileReferenceNumber IN (SELECT items FROM DBO.AxisBank_SplitText(@Files,',')) AND CustomerId = @SourceUserID
END

