IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Db_CustomerVerification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Db_CustomerVerification]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 25-Sep-08
-- Description:	To Verify Name and Email
-- =============================================
CREATE PROCEDURE [dbo].[Db_CustomerVerification] 
	-- Add the parameters for the stored procedure here
	@Name				VARCHAR(50),
	@Email				VARCHAR(50),
	@Mobile				NUMERIC,
	@UpdateDb			Bit,
	@IsNameVerified		Bit Output,
	@IsEmailVerified	Bit Output,
	@IsMobileVerified	Bit Output	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Set @IsNameVerified = 0
	Set @IsEmailVerified = 0
	Set @IsMobileVerified = 0

    Select Name FROM Db_Names WHERE Lower(Name) = Lower(@Name)
	If @@RowCount > 0 Set @IsNameVerified = 1

	Select DomainName FROM Db_Emails WHERE Lower(DomainName) = Lower(@Email)
	If @@RowCount > 0 Set @IsEmailVerified = 1
	
	Select Number FROM MobilePatterns Where Number = @Mobile
	If @@RowCount > 0 Set @IsMobileVerified = 1
	
	/* UPDATE USER DATABASE */
	If @UpdateDb = 1
		BEGIN
			If @IsNameVerified = 0
			BEGIN
				Insert Into Db_Names Values(@Name)
			END
			
			If @IsEmailVerified = 0
			BEGIN 
				Insert Into Db_Emails Values(@Email)
			END
		END
END
