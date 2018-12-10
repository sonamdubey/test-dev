IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CV_InsertPendingList_14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CV_InsertPendingList_14]
GO

	--Modified by Aditi Dhaybar 0n 13/10/14. 
CREATE PROCEDURE [dbo].[CV_InsertPendingList_14.10.1]
	@CWICode 		AS VARCHAR(50), 
	@CUICode 		AS VARCHAR(50), 
	@Email			AS VARCHAR(100),  
	@Mobile			AS VARCHAR(50),  
	@EntryDateTime	AS DATETIME,
	@SourceId		AS TINYINT,				--Modified by Aditi Dhaybar 0n 13/10/14. Added SourceId 
	@CVID			AS NUMERIC OUTPUT
	
AS
	
BEGIN

SET @CVID = -1 -- default value

IF NOT EXISTS(SELECT Email FROM CV_PendingList WITH (NOLOCK) WHERE Email = @Email AND Mobile = @Mobile AND EntryDateTime Between DATEADD(MINUTE, -30, GETDATE()) AND GETDATE())
BEGIN
	INSERT INTO CV_PendingList
		(
			CWICode, 	CUICode,	Email, 		Mobile, 		EntryDateTime,   SourceId	--Modified by Aditi Dhaybar 0n 13/10/14. Added SourceId 
		)	
	VALUES
		(
			@CWICode, 	@CUICode, 	@Email, 	@Mobile, 	@EntryDateTime,    @SourceId
		)	

	SET @CVID = SCOPE_IDENTITY()
END

END
