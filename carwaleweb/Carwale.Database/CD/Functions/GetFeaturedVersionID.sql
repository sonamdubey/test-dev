IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetFeaturedVersionID]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CD].[GetFeaturedVersionID]
GO

	-- modified by amit on 11/13/2013 to not include any version for the same model of the input versions
-- modified by amit on 3 march 2014 : changed logic to return featured car
CREATE FUNCTION [CD].[GetFeaturedVersionID]
	(@Versions VARCHAR(255))
	returns INT
AS
BEGIN
	DECLARE @tbVersions TABLE (Id INT IDENTITY, VersionId INT)
	
	INSERT INTO @tbVersions (VersionId) (select items from [dbo].[SplitTextRS](@Versions,','))

	DECLARE @FeaturedVersionId INT
	set @FeaturedVersionId = (select FeaturedVersionId from (
							Select Top 1 FeaturedVersionId, SpotlightUrl
							from CompareFeaturedCar CFC WITH(NOLOCK)
							INNER JOIN @tbVersions VE ON CFC.VersionId = VE.VersionId							
							WHERE FeaturedVersionId NOT IN(
								SELECT CV.Id FROM CarVersions CV WITH(NOLOCK)
								WHERE CV.CarModelId IN (
								SELECT DISTINCT CarModelId FROM CarVersions CV
								INNER JOIN @tbVersions VE ON CV.Id = VE.VersionId))
							AND IsCompare = 1 AND IsActive = 1
							ORDER BY VE.Id ASC, CFC.CFC_Id DESC) T)
	return @FeaturedVersionId
END
