IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPQCategoryItemAutoPopulateSpecs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPQCategoryItemAutoPopulateSpecs]
GO
	
-- =============================================
-- Author:	Sanjay Soni
-- Create date: 06/07/2016
-- Description:	To add Auto populate specification for PQ category item
-- =============================================
CREATE PROCEDURE [dbo].[InsertPQCategoryItemAutoPopulateSpecs] @ItemCategoryId INT
	,@IsAutoPopulate BIT
	,@ValueType INT
	,@Value INT
	,@RefChargeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO PQ_CategoryItemsAutoPopulateSpecification (
		ItemCategoryId
		,IsAutoPopulate
		,ValueType
		,Value
		,RefChargeId
		)
	VALUES (
		@ItemCategoryId
		,@IsAutoPopulate
		,@ValueType
		,@Value
		,@RefChargeId
		)
END

