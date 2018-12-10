IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPQCategoryItemCityRules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPQCategoryItemCityRules]
GO

	

-- =============================================
-- Author:		Shalini Nair
-- Create date: 22/06/2016
-- Description:	Insert model rules for pq additional charges
-- =============================================
CREATE PROCEDURE [dbo].[InsertPQCategoryItemCityRules] @ItemCategoryId INT
	,@StateId INT
	,@CityId INT
	,@ZoneId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO PQ_CategoryItemsCityRules (
		ItemCategoryId
		,StateId
		,CityId
		,ZoneId
		)
	VALUES (
		@ItemCategoryId
		,CASE 
			WHEN @StateId = 0
				THEN (
						SELECT ct.StateId
						FROM Cities ct WITH (NOLOCK)
						WHERE ct.ID = @CityId
						)
			ELSE @StateId
			END
		,@CityId
		,@ZoneId
		)
END

