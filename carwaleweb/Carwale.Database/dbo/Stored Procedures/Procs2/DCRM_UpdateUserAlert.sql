IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateUserAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateUserAlert]
GO

	
CREATE PROCEDURE [dbo].[DCRM_UpdateUserAlert]
	@Id				AS 	NUMERIC,
	@DueDate		AS  DATETIME,
	@Comment		AS 	NCHAR(300),
	@Status			AS 	NUMERIC,
	@Result 		INT OUTPUT
	
AS
	
	
BEGIN
	
	UPDATE DCRM_UserAlerts SET DueDate= @DueDate, ActionDate = GETDATE(), Comment = CONVERT(VARCHAR,ISNULL(Comment,'')) + '<BR>' + CONVERT(VARCHAR,@Comment),
	Status = @Status WHERE Id = @Id
	
	SET @Result = 1  
	
END
