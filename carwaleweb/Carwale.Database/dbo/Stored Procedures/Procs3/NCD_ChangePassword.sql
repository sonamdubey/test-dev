IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_ChangePassword]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_ChangePassword]
GO

	
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 9/11/2011
-- Description:	Using this SP for updating the Password for existing Users in NCD.
-- =============================================
CREATE PROCEDURE [dbo].[NCD_ChangePassword] 
	-- Add the parameters for the stored procedure here
	@UserId int,
	@Oldpassword varchar(30),
	@Password varchar(30),
	@Result varchar(3) output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.	
	SET NOCOUNT ON;
	
	if exists (select ID from NCD_Users where Id=@UserId and Password=@Oldpassword)
		begin
			Update NCD_Users set Password=@Password where Id=@UserId
			set @Result='Y'
		end
	else
		begin
		 set @Result='N'
		end		
END

