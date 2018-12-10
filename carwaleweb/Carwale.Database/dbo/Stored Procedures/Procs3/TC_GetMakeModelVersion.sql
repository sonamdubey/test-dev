IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetMakeModelVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetMakeModelVersion]
GO
	-- =============================================
-- Author:		Tejashree Patil on 1 July 2013
-- Description:	To get Make , Model, Version
-- [TC_GetMakeModelVersion]NULL,1,NULL
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetMakeModelVersion]
	@IsUsed BIT,
	@IsNew BIT,
	@MakeId INT = NULL
AS
BEGIN
	SELECT	CMA.ID AS CarMakeId, CMA.Name AS MakeName,
			CMO.ID AS ModelId, CMO.Name ModelName,
			CV.ID AS CarVersionId, CV.Name AS VersionName                                    
	FROM	CarModels CMO WITH (NOLOCK)
			INNER JOIN	CarMakes CMA WITH (NOLOCK) 
						ON  CMO.CarMakeId = CMA.ID
			INNER JOIN	CarVersions CV WITH (NOLOCK)  
						ON CV.CarModelId=CMO.ID                                  
	WHERE	CV.IsDeleted = 0 
			AND CMO.IsDeleted = 0  
			AND CV.Futuristic = 0                                   
			AND ( @IsUsed IS NULL OR CMO.Used=@IsUsed)
			AND ( @IsNew IS NULL OR CMO.New=@IsNew)
			AND ( @IsUsed IS NULL OR CV.Used=@IsUsed)
			AND ( @IsNew IS NULL OR CV.New=@IsNew)	
			AND CMO.Futuristic=0 
			AND CV.Futuristic=0 
			AND ( @MakeId IS NULL OR CMO.CarMakeId=@MakeId)
END
