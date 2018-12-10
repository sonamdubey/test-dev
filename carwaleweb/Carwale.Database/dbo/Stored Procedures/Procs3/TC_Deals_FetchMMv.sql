IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_FetchMMv]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_FetchMMv]
GO

	
-- =============================================
-- Author:	   : <Khushaboo Patil>
-- Create date : <28th Dec 15>
-- Description : <Get Aged cars make model version and version colors>
-- Modified By : Khushaboo Patil on 22/4/2016 do not fetch discontinued makes
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_FetchMMv]
	@MakeId		INT = NULL,
	@ModelId	INT = NULL,
	@VersionId	INT = NULL
AS
BEGIN
	IF @MakeId IS NULL AND @ModelId IS NULL AND @VersionId IS NULL
		BEGIN
			SELECT Id AS MakeId, Name AS MakeName  
			FROM CarMakes WITH(NOLOCK)
			WHERE IsDeleted = 0 AND Futuristic = 0 AND New = 1 
			ORDER BY MakeName
		END
	ELSE IF @MakeId IS NOT NULL
		BEGIN
			SELECT Id AS ModelId, Name AS ModelName  
			FROM CarModels WITH(NOLOCK)
			WHERE IsDeleted = 0 AND Futuristic = 0 AND CarMakeId = @MakeId
			ORDER BY ModelName
		END
	ELSE IF @ModelId IS NOT NULL
		BEGIN
			SELECT Id AS VersionId, Name AS VersionName  
			FROM CarVersions WITH(NOLOCK)
			WHERE IsDeleted = 0 AND Futuristic = 0 AND CarModelId = @ModelId
			ORDER BY VersionName
		END
	ELSE IF @VersionId IS NOT NULL
		BEGIN
			SELECT ID AS VersionColorId, Color AS ColorName 
			FROM VersionColors WITH(NOLOCK)
			WHERE CarVersionID = @VersionId AND IsActive = 1
			ORDER BY ColorName

			SELECT VW.MakeId,VW.ModelId,VW.VersionId  
			FROM vwMMV VW WITH(NOLOCK) 
			WHERE VW.VersionId = @VersionId
		END
END
-------------------------------------
