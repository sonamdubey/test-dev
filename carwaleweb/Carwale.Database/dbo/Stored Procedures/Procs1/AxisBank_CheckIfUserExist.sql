IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_CheckIfUserExist]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_CheckIfUserExist]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 27 June 2013
-- Description:	Proc returns customer id for the given email address
-- =============================================
CREATE PROCEDURE AxisBank_CheckIfUserExist 
	-- Add the parameters for the stored procedure here	
	@UserId		numeric(18,0),
	@EmailId	varchar(100) output,
	@IsExist	bit output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @EmailId=Email FROM AxisBank_Users WHERE UserId=@UserId

	if @@ROWCOUNT>0
	SET @IsExist=1;
	else
	SET @IsExist=0;
END


