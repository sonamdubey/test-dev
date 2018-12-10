IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BindCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BindCarVersions]
GO

	-- =============================================    

-- Author:  <Prashant Vishe>    

-- Create date: <30 aug 2013>    

-- Description: <for retrieving information of versions of particular model.>    

  

-- =============================================    

CREATE PROCEDURE [dbo].[BindCarVersions]    

 -- Add the parameters for the stored procedure here    

 @Id numeric    

AS    

BEGIN    

 -- SET NOCOUNT ON added to prevent extra result sets from    

 -- interfering with SELECT statements.    

 SET NOCOUNT ON;    

    

    -- Insert statements for procedure here    

 SELECT CV.ID, CV.Name, Se.Id AS SegmentId, Se.Name AS Segment,     

	  Bo.ID AS BodyStyleId, Bo.Name AS BodyStyle, CV.CarFuelType, CV.CarTransmission, CarModelId,     

	  CV.Used, CV.New, CV.Indian, CV.Imported, CV.Classic, CV.Modified, CV.Futuristic,    

	  SSe.Id AS SubSegmentId, SSe.Name AS SubSegmentName,CONVERT(VARCHAR(24), CV.VCreatedOn, 113) AS CreatedOn,CONVERT(VARCHAR(24), CV.VUpdatedOn, 113) AS UpdatedOn,OU.UserName AS UpdatedBy,CV.MaskingName   

 FROM CarSegments Se, CarBodyStyles Bo, CarModels Mo,     

	  ((CarVersions CV LEFT JOIN CarSubSegments SSe ON CV.SubSegmentId = SSe.Id) LEFT JOIN OprUsers OU ON CV.VUpdatedBy = OU.id )     

 WHERE CV.SegmentId=Se.ID AND CV.BodyStyleId=Bo.id AND CV.IsDeleted=0     

		AND Mo.ID=CV.CarModelId AND Mo.Id= @Id
ORDER BY CV.New  DESC

    

    

  SELECT cm.Name + ' ' + Cmo.Name as carName from CarMakes cm,CarModels Cmo WHERE Cmo.CarMakeId=cm.ID and Cmo.ID=@Id   

END 
