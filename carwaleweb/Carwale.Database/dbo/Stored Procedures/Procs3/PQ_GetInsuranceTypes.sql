IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetInsuranceTypes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetInsuranceTypes]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 12 OCT 2013
-- Description:	To get types of insurance need to be bind with DropDown
-- Modified: Vicky Lund, 05/07/2016, Used alias name of table PriceQuote_Insurances
-- =============================================
CREATE PROCEDURE [dbo].[PQ_GetInsuranceTypes]
	-- Add the parameters for the stored procedure here
	@MakeID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT I.CategoryItemId Value
		,CI.CategoryName AS TEXT
	FROM PriceQuote_Insurances I WITH (NOLOCK)
	INNER JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = I.CategoryItemId
	WHERE (
			I.MakeId = @MakeId
			OR I.IsGeneric = 1
			)
		AND I.IsActive = 1
END
