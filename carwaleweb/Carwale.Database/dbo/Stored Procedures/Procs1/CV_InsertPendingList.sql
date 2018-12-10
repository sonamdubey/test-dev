IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_InsertPendingList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_InsertPendingList]
GO

	CREATE PROCEDURE [dbo].[CV_InsertPendingList]
	@CWICode 		AS VARCHAR(50), 
	@CUICode 		AS VARCHAR(50), 
	@Email			AS VARCHAR(100),  
	@Mobile		AS VARCHAR(50),  
	@EntryDateTime	AS DATETIME,
	@CVID			AS NUMERIC OUTPUT
	
AS
	
BEGIN

SET @CVID = -1 -- default value

IF NOT EXISTS(SELECT Email FROM CV_PendingList WITH (NOLOCK) WHERE Email = @Email AND Mobile = @Mobile AND EntryDateTime Between DATEADD(MINUTE, -30, GETDATE()) AND GETDATE())
BEGIN
	INSERT INTO CV_PendingList
		(
			CWICode, 	CUICode,	Email, 		Mobile, 		EntryDateTime
		)	
	VALUES
		(
			@CWICode, 	@CUICode, 	@Email, 	@Mobile, 	@EntryDateTime
		)	

	SET @CVID = SCOPE_IDENTITY()
END

END



