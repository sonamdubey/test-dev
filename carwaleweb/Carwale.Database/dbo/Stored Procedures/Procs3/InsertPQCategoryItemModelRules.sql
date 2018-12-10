IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPQCategoryItemModelRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPQCategoryItemModelRules]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 22/06/2016
-- Description:	Insert model rules for pq additional charges
-- =============================================
CREATE PROCEDURE [dbo].[InsertPQCategoryItemModelRules] @ItemCategoryId INT
	,@MakeId INT
	,@ModelId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO PQ_CategoryItemsModelRules (
		ItemCategoryId
		,MakeId
		,ModelId
		)
	VALUES (
		@ItemCategoryId
		,CASE 
			WHEN @MakeId = 0
				THEN (
						SELECT CarMakeId
						FROM CarModels CM WITH (NOLOCK)
						WHERE CM.ID = @ModelId
						)
			ELSE @MakeId
			END
		,@ModelId
		)
END

