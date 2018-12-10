IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPQCategoryItemFuelRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPQCategoryItemFuelRules]
GO
	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 22/06/2016
-- Description:	Add fuel rules for pq additionalCharge (PQ category item)
-- =============================================
CREATE PROCEDURE [dbo].[InsertPQCategoryItemFuelRules]
	-- Add the parameters for the stored procedure here
	@ItemCategoryId INT
	,@FuelType INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO PQ_CategoryItemsFuelRules (
		ItemCategoryId
		,FuelTypeId
		)
	VALUES (
		@ItemCategoryId
		,@FuelType
		)
END

