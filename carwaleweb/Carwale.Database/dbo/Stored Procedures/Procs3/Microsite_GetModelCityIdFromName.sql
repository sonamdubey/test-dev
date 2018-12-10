IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetModelCityIdFromName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetModelCityIdFromName]
GO

	-- =============================================
-- Author:		Komal Manjare
-- Create date: 29 July 2015
-- Description:	Fetch ModelId from dealertable 
-- Modified by : Kritika Choudhary on 24th Sep 2015, added select quey for cityId
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetModelCityIdFromName]
@DealerId INT,
@ModelName VARCHAR(20),
@IsDealer BIT=0,
@CityName VARCHAR(20)=NULL
AS

BEGIN 
   SELECT
      CASE @IsDealer
			WHEN 1 
			   THEN (SELECT top 1 DM.ID   
			         FROM TC_DealerModels AS DM WITH(NOLOCK) 
					 INNER JOIN CarModels CM ON Cm.Id=DM.CWModelId
					 WHERE Dm.DealerId=@DealerId AND  DM.IsDeleted=0 AND (REPLACE(DWModelName,' ','')=REPLACE(@ModelName,' ','')))
			   ELSE ( SELECT ID
			          FROM CarModels WITH(NOLOCK) 
					  WHERE IsDeleted=0 AND (REPLACE(Name,' ','')=REPLACE(@ModelName,' ','')))
	  END AS ModelId
	IF(@CityName IS NOT NULL)
	BEGIN

		 SELECT ID AS CityId
		 FROM Cities
		 WHERE IsDeleted=0 AND (REPLACE(Name,' ','')=REPLACE(@CityName,' ',''))
	END
	  
END