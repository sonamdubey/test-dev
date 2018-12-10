IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetModelsForMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetModelsForMake]
GO

	--Date Created:  09 Jan 2015
--Author: Rakesh Yadav
--Desc: Fetch models for makes whos models are available in dealers stock(used on search page of used car dealerWebsites)
CREATE PROCEDURE [dbo].[Microsite_GetModelsForMake] 
@DealerId Int,
@MakeId int
AS
BEGIN
	SELECT DISTINCT VM.ModelId,VM.Make AS MakeName,VM.Model AS ModelName
	FROM 
	vwMMV VM INNER JOIN TC_Stock TC WITH (nolock) on TC.VersionId=VM.VersionId
	WHERE TC.BranchId=@DealerId AND VM.MakeId=@MakeId AND TC.IsActive=1 AND StatusId=1
	ORDER BY VM.Model
END
