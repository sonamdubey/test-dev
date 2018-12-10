IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetUserDefinedValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetUserDefinedValues]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================  [CD].[GetUserDefinedValues] 61
CREATE PROCEDURE [CD].[GetUserDefinedValues]
	-- Add the parameters for the stored procedure here
	@ItemMasterID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT UserDefinedId,Name FROM CD.UserDefinedMaster WHERE ItemMasterId = @ItemMasterID
END