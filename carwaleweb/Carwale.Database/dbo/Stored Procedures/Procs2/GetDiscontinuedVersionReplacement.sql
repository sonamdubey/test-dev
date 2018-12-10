IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDiscontinuedVersionReplacement]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDiscontinuedVersionReplacement]
GO

	-- =============================================    
-- Author:  Vikas    
-- Create date: 04-12-2012    
-- Description: To get the Version which has replaced a discontinued Version (if any)    
-- =============================================    
CREATE PROCEDURE GetDiscontinuedVersionReplacement     
 -- Add the parameters for the stored procedure here    
 @VersionId INT     
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
  
 DECLARE @ReplacementID INT    
 SET @ReplacementID = (SELECT ISNULL(ReplacedByVersionId, 0) From CarVersions Where ID = @VersionId)    
 -- Insert statements for procedure here    
 IF @ReplacementID != 0     
 BEGIN     
  SELECT     
   @ReplacementID As ReplacedByVersionId, Cma.Name As ReplacementMakeName, Cmo.Name As ReplacementModelName, Cv.Name As ReplacementVersionName     
  FROM     
   CarVersions Cv  
   INNER JOIN CarModels Cmo  On Cv.CarModelId = Cmo.ID  
   INNER JOIN CarMakes Cma On Cma.ID = Cmo.CarMakeId    
  WHERE     
   Cv.ID = @ReplacementID    
 END    
END 