IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[VizuryApiDump_V2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[VizuryApiDump_V2]
GO

	-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 06-11-2014
-- Description:	Data Dump for the Vizury API
-- [dbo].[VizuryApiDump_V2]
-- =============================================
CREATE PROCEDURE [dbo].[VizuryApiDump_V2] 
	-- Add the parameters for the stored procedure here
@CityId int=10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
with CTE as
(
select c.ID,c.Color,c.CarVersionID from VersionColors c with(nolock)
where c.IsActive=1
)
select 
CBS.Name 'carbodytype'
,CM.Name+' '+CMO.Name 'modelname'
,CM.Name 'brandname'
,CV.Name 'variantname'
,CFT.Descr 'fueltype'
,isnull(CV.HostURL+CV.DirPath+CV.largePic ,'') 'imageurl'
,CM.Name+' '+CMO.Name+' '+CV.Name 'productid'
,isnull(IV.ItemValue,-1) 'arai_mileadge'
,isnull(NCS.Displacement,-1) 'cc'
,isnull(NCSP.Price,0) 'exshowroomprice'
,isnull(STUFF ((select ', '+ Color from CTE where CTE.CarVersionID=CV.ID FOR XML PATH('')),1,1,''),'') 'colors'
,Cv.id 'versionid'
,cmo.MaskingName 'modelmasking'
,cm.Name 'makemasking'
,isnull(cmo.XLargePic,' ') 'largeimgurl'
from
CarVersions CV with(nolock)
INNER JOIN CarModels CMO with(nolock) on CMO.ID=CV.CarModelId
INNER JOIN CarMakes CM with(nolock) on CM.ID=CMO.CarMakeId
LEFT JOIN CarFuelTypes CFT with(nolock) on CV.CarFuelType=CFT.CarFuelTypeId
LEFT JOIN CD.ItemValues IV with(nolock) on IV.CarVersionId=CV.ID and IV.ItemMasterId=12
LEFT JOIN NewCarSpecifications NCS with(nolock) on NCS.CarVersionId=CV.ID
LEFT JOIN NewCarShowroomPrices NCSP with(nolock) on NCSP.CarVersionId=CV.ID
LEFT JOIN CarBodyStyles CBS with(nolock) on CBS.ID=CV.BodyStyleId
where
CV.CarModelId not in --Excluded Skoda Fabia and Tata Nano from result (author:Rohan sapkal 27-01-2015)
(47
,382
,756
,229
,463
,534
,758
,759) 
AND CV.New=1 
AND CV.IsDeleted=0 
AND NCSP.CityId=@CityId
END