IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetCustomersList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetCustomersList]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: 18-12-2013
-- Description:	To get list of active users
-- =============================================
CREATE PROCEDURE AxisBank_GetCustomersList
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT AU.UserId AS UserId
		,Au.FirstName + ' ' + LastName AS UserName
	FROM AxisBank_Users AU
	WHERE AU.IsActive = 1
END

