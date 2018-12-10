IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_SetServiceReminder]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_SetServiceReminder]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <18/07/2016>
-- Description:	<Update service next service date in reminder>
-- =============================================
CREATE PROCEDURE [dbo].[TC_Service_SetServiceReminder]
	@ServiceInquiryId	INT,
	@RegistrationNumber VARCHAR(50),
	@NextServiceDate	DATETIME
AS
BEGIN		

	DECLARE @RegistrationNumberSearch VARCHAR(50)
	SET @RegistrationNumberSearch = LOWER(REPLACE(@RegistrationNumber,' ' ,''))

	--update next service date in reminder table
	UPDATE TC_Service_Reminder
	SET ServiceDueDate = @NextServiceDate,LastServiceDate = GETDATE()
	WHERE RegistrationNumber  = @RegistrationNumberSearch

END
