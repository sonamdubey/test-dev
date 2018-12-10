IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SaveEnteredDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SaveEnteredDetails]
GO

	
-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,16th April,2013>
-- Description:	<Description, Used to insert details inserted by carnation user>
-- =============================================
CREATE PROCEDURE [dbo].[TC_SaveEnteredDetails]
	-- Add the parameters for the stored procedure here
	 @UserEmail VarChar(200),
     @Password VarChar(200),
     @BranchId BigInt,
     @UserId BigInt,
     @Json VarChar(2000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO TC_CarNationEntryDetails(UserId, BranchId, Email, Pwd, Json, EntryDate) VALUES (@UserId, @BranchId, @UserEmail, @Password, @Json, GETDATE())
END
