IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_Telecallers_Sp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_Telecallers_Sp]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 04-Aug-2008
-- Description:	Configure new agent to taking calls
-- =============================================
Create PROCEDURE [dbo].[CH_Telecallers_Sp] 
	-- Add the parameters for the stored procedure here
	@TcId		NUMERIC
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Select TcId From CH_Telecallers Where TcId = @TcId
	
	IF @@RowCount = 0
		BEGIN
			INSERT INTO CH_Telecallers(TcId) Values(@TcId)
		END
END