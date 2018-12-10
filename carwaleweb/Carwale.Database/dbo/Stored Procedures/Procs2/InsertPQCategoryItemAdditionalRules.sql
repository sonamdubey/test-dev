IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPQCategoryItemAdditionalRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPQCategoryItemAdditionalRules]
GO
	

-- =============================================
-- Author:	Shalini Nair
-- Create date: 22/06/2016
-- Description:	To add additional rules for PQ category item
-- =============================================
CREATE PROCEDURE [dbo].[InsertPQCategoryItemAdditionalRules] @ItemCategoryId INT
	,@DisplacementMax INT 
	,@DisplacementMin INT
	,@ExShowroomMax INT
	,@ExShowroomMin INT
	,@GroundClearanceMax INT
	,@GroundClearanceMin INT
	,@LengthMax INT
	,@LengthMin INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO PQ_CategoryItemsAdditionalRules (
		ItemCategoryId
		,DisplacementMax
		,DisplacementMin
		,ExShowroomMax
		,ExShowroomMin
		,GroundClearanceMax
		,GroundClearanceMin
		,LengthMax
		,LengthMin
		)
	VALUES (
		@ItemCategoryId
		,@DisplacementMax
		,@DisplacementMin
		,@ExShowroomMax
		,@ExShowroomMin
		,@GroundClearanceMax
		,@GroundClearanceMin
		,@LengthMax
		,@LengthMin
		)
END

