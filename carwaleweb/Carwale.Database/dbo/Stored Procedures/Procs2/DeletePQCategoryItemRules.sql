IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DeletePQCategoryItemRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DeletePQCategoryItemRules]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 23/06/2016
-- Description:	Delete PQ category item rules (PQ additional charges)
-- Modified By : Sanjay Soni, @IsAutoPopulateSpecificationEdited added and write delete rule for that flag
-- =============================================
CREATE PROCEDURE [dbo].[DeletePQCategoryItemRules] @ItemCategoryId INT
	,@IsModelRulesEdited BIT
	,@IsCityRulesEdited BIT
	,@IsFuelRulesEdited BIT
	,@IsAdditionalRulesEdited BIT
	,@IsAutoPopulateSpecificationEdited BIT 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (@IsModelRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_CategoryItemsModelRules
		WHERE ItemCategoryId = @ItemCategoryId
	END

	IF (@IsCityRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_CategoryItemsCityRules
		WHERE ItemCategoryId = @ItemCategoryId
	END

	IF (@IsFuelRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_CategoryItemsFuelRules
		WHERE ItemCategoryId = @ItemCategoryId
	END

	IF (@IsAdditionalRulesEdited = 1)
	BEGIN
		DELETE
		FROM PQ_CategoryItemsAdditionalRules
		WHERE ItemCategoryId = @ItemCategoryId
	END

	IF (@IsAutoPopulateSpecificationEdited = 1)
	BEGIN
		DELETE
		FROM PQ_CategoryItemsAutoPopulateSpecification
		WHERE ItemCategoryId = @ItemCategoryId
	END
END

