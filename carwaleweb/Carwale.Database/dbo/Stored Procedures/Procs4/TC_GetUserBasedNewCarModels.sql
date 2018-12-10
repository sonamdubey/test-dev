IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetUserBasedNewCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetUserBasedNewCarModels]
GO

	
-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,06th June, 2013>
-- Description:	<Description,Gives new car Models based on user divisions>
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application.
-- Modified By : Tejashree Patil on 11 Dec 2015, Added Futuristic models.
-- Modifried by : kritika Choudhary on 18th Feb 2016, modified select queries for multiple makeIds
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetUserBasedNewCarModels]
	-- Add the parameters for the stored procedure here
	@MakeIds VARCHAR(MAX), 
	@UserId INT,
	@ApplicationId INT = 1-- Modified By : Tejashree Patil on 30-10-2014
AS
BEGIN
	SET NOCOUNT ON;
   /* IF ((SELECT COUNT(DISTINCT(U.Id))-- user is having Admin, Sales Manager role or model pemission is not set for that user then show all models 
					FROM		TC_Users U 
					INNER JOIN  TC_UsersRole R ON U.Id = @UserId AND R.UserId = @UserId
					WHERE	 R.RoleId IN (1,7,12)) = 1  OR (SELECT COUNT(TC_UsersId) FROM TC_UserModelsPermission WHERE TC_UsersId = @UserId) = 0)
	BEGIN
		SELECT	DISTINCT V.ModelId AS Value, V.Model AS Text, V.Make, V.HostURL,CV.OriginalImgPath
		        ,ROW_NUMBER() OVER (pARTITION BY cv.CarModelId Order by V.VersionId) AS RowNumber 
	    INTO	#table1
		FROM	vwAllMMV V 
		JOIN    CarVersions CV ON V.ModelId = CV.CarModelId 
		WHERE	V.MakeId IN ( SELECT ListMember FROM fnSplitCSVValues(@MakeIds)) 
				AND ((V.New = 1 AND V.IsModelNew = 1) OR (V.ModelFuturistic = 1))
				AND V.ApplicationId=ISNULL(@ApplicationId,1) and CV.IsDeleted = 0-- Modified By : Tejashree Patil on 30-10-2014
		ORDER BY Text 
		SELECT Value,Text,Make,HostUrl,OriginalImgPath FROM #table1 WHERE RowNumber=1
		
	END
	
	ELSE -- Model pemission is set for that user
	BEGIN
			SELECT	DISTINCT V.ModelId AS Value, V.Model AS Text, V.Make, V.HostURL,CV.OriginalImgPath
					,ROW_NUMBER() OVER (pARTITION BY cv.CarModelId Order by V.VersionId) AS RowNumber 
            INTO	#table2
			FROM	vwAllMMV V 
			JOIN    CarVersions CV ON V.ModelId = CV.CarModelId 
			WHERE	V.ModelId IN (SELECT ModelId FROM TC_UserModelsPermission WITH (NOLOCK) WHERE TC_UsersId =@UserId)
					AND V.MakeId IN ( SELECT ListMember FROM fnSplitCSVValues(@MakeIds))
					AND ((V.New = 1 AND V.IsModelNew = 1) OR (ModelFuturistic = 1))
					AND V.ApplicationId=ISNULL(@ApplicationId,1) and CV.IsDeleted = 0-- Modified By : Tejashree Patil on 30-10-2014
			ORDER BY Text  
			SELECT Value,Text,Make,HostUrl,OriginalImgPath FROM #table2 WHERE RowNumber=1
			
			IF ((SELECT COUNT(DISTINCT(U.Id))-- user is having Admin, Sales Manager role or model pemission is not set for that user then show all models 
					FROM		TC_Users U WITH (NOLOCK)
					INNER JOIN  TC_UsersRole R WITH (NOLOCK)ON U.Id = @UserId AND R.UserId = @UserId
					WHERE	 R.RoleId IN (1,7,12)) = 1  OR (SELECT COUNT(TC_UsersId) FROM TC_UserModelsPermission WHERE TC_UsersId = @UserId) = 0)
			BEGIN
				SELECT	DISTINCT V.ModelId AS Value, V.Model AS Text, V.Make, V.HostURL,CV.OriginalImgPath
				,ROW_NUMBER() OVER (pARTITION BY cv.CarModelId Order by V.VersionId) AS RowNumber 
				INTO	#table3
				FROM	vwAllMMV V 
				JOIN    CarVersions CV ON V.ModelId = CV.CarModelId 
				WHERE	V.MakeId IN ( SELECT ListMember FROM fnSplitCSVValues(@MakeIds)) 
						AND ((V.New = 1) OR (V.ModelFuturistic = 1))
						AND V.ApplicationId=ISNULL(@ApplicationId,1) and CV.IsDeleted = 0-- Modified By : Tejashree Patil on 30-10-2014
				ORDER BY Text  
				SELECT Value,Text,Make,HostUrl,OriginalImgPath FROM #table3 WHERE RowNumber=1
			
			END
			ELSE -- Model pemission is set for that user
			BEGIN
				SELECT	DISTINCT V.ModelId AS Value, V.Model AS Text, V.Make,  V.HostURL,CV.OriginalImgPath
				,ROW_NUMBER() OVER (pARTITION BY cv.CarModelId Order by V.VersionId) AS RowNumber 
				INTO	#table4
				FROM	vwAllMMV V 
				JOIN    CarVersions CV ON V.ModelId = CV.CarModelId 
				WHERE	V.ModelId IN (SELECT ModelId FROM TC_UserModelsPermission WHERE TC_UsersId =@UserId)
						AND V.MakeId IN ( SELECT ListMember FROM fnSplitCSVValues(@MakeIds))
						AND ((V.New = 1 AND V.IsModelNew = 1) OR (V.ModelFuturistic = 1))
						AND V.ApplicationId=ISNULL(@ApplicationId,1) and CV.IsDeleted = 0-- Modified By : Tejashree Patil on 30-10-2014
				ORDER BY Text  
				SELECT Value,Text,Make,HostUrl,OriginalImgPath FROM #table4 WHERE RowNumber=1
			
			END
	
	END*/

	
	SELECT	DISTINCT V.ModelId AS Value, V.Model AS Text, V.Make, V.ModelHostUrl HostUrl , V.ModelOriginalImgPath OriginalImgPath--,CV.OriginalImgPath
	FROM	vwAllMMV V 
	WHERE	V.MakeId IN ( SELECT ListMember FROM fnSplitCSVValues(@MakeIds)) 
			AND ((V.New = 1 AND V.IsModelNew = 1) OR (V.ModelFuturistic = 1))
			AND V.ApplicationId=ISNULL(@ApplicationId,1)-- and CV.IsDeleted = 0-- Modified By : Tejashree Patil on 30-10-2014
	ORDER BY Text
	
END