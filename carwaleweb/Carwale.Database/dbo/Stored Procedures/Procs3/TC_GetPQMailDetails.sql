IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetPQMailDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetPQMailDetails]
GO
	
-- =================================================================================================================
-- Author		: Kritika Choudhary
-- Created On	: 11th Feb, 2016
-- Description	: get Price quote mail details
-- =================================================================================================================
CREATE PROCEDURE [dbo].[TC_GetPQMailDetails]
@CustId Int=NULL,
@VersionId Int=NULL
AS
BEGIN
   		SELECT	CV.Name VersionName, CM.Name ModelName, CMK.Name MakeName
		FROM	CarVersions CV WITH(NOLOCK) 
		JOIN    CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	    JOIN    CarMakes CMK WITH (NOLOCK) ON CM.CarMakeId = CMK.ID
		WHERE	CV.ID = @VersionId and CMK.IsDeleted=0 and CM.IsDeleted=0 and CV.IsDeleted=0

		IF @CustId IS NOT NULL
		BEGIN
			SELECT CustomerName, Email, Mobile
			FROM TC_CustomerDetails WITH(NOLOCK) 
			WHERE Id = @CustId and IsActive=1
			
		END
	
END
