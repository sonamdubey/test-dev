IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_GetFilesByUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_GetFilesByUser]
GO

	
-- =============================================
-- Author:		Amit Verma
-- Create date: 24/12/2013
-- Description:	Get files by userid
/*
	DECLARE @Count INT
	EXEC AxisBank_GetFilesByUser 4,1,5,'1321654654564',@Count out
	SELECT @Count
*/
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_GetFilesByUser]
	-- Add the parameters for the stored procedure here
	@UserID NUMERIC(18,0),
	@Index INT = 1,
	@PageSize INT = 10,
	@FileReferenceNumber VARCHAR(50) = NULL,
	@Count INT = 0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * FROM(
		select * ,ROW_NUMBER() OVER (order by FileReferenceNumber) as RowNo from 
		(SELECT distinct FileReferenceNumber
			FROM AxisBank_CarValuations WHERE CustomerId = @UserID
			AND (@FileReferenceNumber IS NULL OR FileReferenceNumber IN (SELECT * FROM dbo.AxisBank_SplitText(@FileReferenceNumber,',')))
	) T) T2 WHERE RowNo BETWEEN ( ((@Index - 1) * @PageSize )+ 1) AND (@Index*@PageSize)

	SELECT @Count = COUNT(distinct FileReferenceNumber) FROM AxisBank_CarValuations WHERE CustomerId = @UserID
	AND (@FileReferenceNumber IS NULL OR FileReferenceNumber IN (SELECT * FROM dbo.AxisBank_SplitText(@FileReferenceNumber,',')))
END


