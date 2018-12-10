IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveSlotMachineInfo]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveSlotMachineInfo]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 20-Feb-2013
-- Description:	Save User record on playing slot machine app
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveSlotMachineInfo]
	-- Add the parameters for the stored procedure here
	@Name		VARCHAR(50),
	@Contact	VARCHAR(15),
	@Email		VARCHAR(50),
	@ClientIp	VARCHAR(20),
	@NewId		NUMERIC(18,0) OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    --Initialize the new id with -1
    SET @NewId = -1
    
	INSERT INTO OLM_SlotMachineRecords (Name, Contact, Email, ClientIp, IsWinner)
	VALUES (@Name, @Contact, @Email, @ClientIp, 0)
	
	--set new id as the new generated row id of record
	SET @NewId = SCOPE_IDENTITY()
END
