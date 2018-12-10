IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CL_ChangeExtension]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CL_ChangeExtension]
GO

	-- =============================================
-- Author	:	Sachin Bharti(21st June 2014)
-- Description	:	Change dialer type for user extension
-- =============================================
CREATE PROCEDURE [dbo].[CL_ChangeExtension]
	@ExtensionIds	VARCHAR(100),
	@DialerType		SMALLINT,
	@Result			SMALLINT = NULL OUTPUT 
AS
BEGIN
	
	SET NOCOUNT ON;
	SET @Result = -1
	IF (@DialerType <>3)
		BEGIN
			UPDATE CL_ExtensionMap SET DialerType = (CASE @DialerType WHEN 4 THEN 2 ELSE @DialerType END) ,
				DrishtiLoginId = NULL, OfficeId = (CASE @DialerType WHEN 4 THEN 2 ELSE 1 END) 
			WHERE UserId IN (select listmember from fnSplitCSV(@ExtensionIds)) 
			
			IF @@ROWCOUNT <> 0
				SET @Result = 1
		END
	ELSE IF(@DialerType = 3)
		BEGIN
			UPDATE CL_ExtensionMap SET DialerType = @DialerType 
			WHERE UserId IN (select listmember from fnSplitCSV(@ExtensionIds)) 
			
			IF @@ROWCOUNT <> 0
			SET @Result = 1
		END
END
