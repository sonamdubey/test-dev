IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ES_GetMakeModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ES_GetMakeModel]
GO

	


-- =============================================
-- Author:		Ashwini
-- Create date: 21-Sept-2015
-- Description:	Get make model and root to add document to ES index
-- =============================================
CREATE PROCEDURE [dbo].[ES_GetMakeModel]
@ModelId int 
AS
BEGIN
	 SELECT MK.ID AS MakeId
	,MK.NAME AS MakeName
	,M.id AS ModelId
	,M.NAME AS ModelName
	,R.RootId AS RootId
	,R.RootName AS RootName
	,M.MaskingName AS MaskingName
	,M.New
	,M.Used
	,M.Futuristic
FROM CarMakes MK WITH (NOLOCK)
JOIN CarModels M WITH (NOLOCK) ON M.CarMakeId = MK.ID
JOIN CarModelRoots R WITH (NOLOCK) ON R.RootId = M.RootId
WHERE M.IsDeleted = 0
	AND MK.IsDeleted = 0
	--AND M.new = 1
	AND M.ID = @ModelId
END

