IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_UpdateSlotMachineWinDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_UpdateSlotMachineWinDetails]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 20-Feb-2013
-- Description:	Update winning details for the slot machine app
-- =============================================
CREATE PROCEDURE [dbo].[OLM_UpdateSlotMachineWinDetails]
	-- Add the parameters for the stored procedure here
	@Id			NUMERIC(18,0),
	@IsWinner	BIT,
	@WinCount	SMALLINT,
	@WinNumbers	VARCHAR(15)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update OLM_SlotMachineRecords
		SET IsWinner = @IsWinner, NineCount = @WinCount, WinNumbers = @WinNumbers
	WHERE Id = @Id
END
