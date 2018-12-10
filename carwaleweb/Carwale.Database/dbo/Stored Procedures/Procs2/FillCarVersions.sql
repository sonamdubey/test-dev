IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FillCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FillCarVersions]
GO

	
-- =============================================       
-- Author: Prashant Vishe        
-- Create date: <30 Aug 2013>       
-- Description:For filling car versions related data...   
-- Modified On 16 Sept 2013 added queries for fetching CreatedBy  column    
-- Modified By:prashant vishe on 25 sept 2013 
-- Modification:added query for selecting MaskingName 
-- =============================================       
CREATE PROCEDURE [dbo].[FillCarVersions] 
  -- Add the parameters for the stored procedure here       
  @Id NUMERIC 
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from       
      -- interfering with SELECT statements.       
      SET nocount ON; 

      -- Insert statements for procedure here     
      SELECT CV.id, 
             CV.name, 
             Se.id                                    AS SegmentId, 
             Se.name                                  AS Segment, 
             Bo.id                                    AS BodyStyleId, 
             Bo.name                                  AS BodyStyle, 
             CV.carfueltype, 
             CV.cartransmission, 
             carmodelid, 
             CV.used, 
             CV.new, 
             CV.indian, 
             CV.imported, 
             CV.classic, 
             CV.modified, 
             CV.futuristic, 
             SSe.id                                   AS SubSegmentId, 
             SSe.name                                 AS SubSegmentName, 
             CONVERT(VARCHAR(24), CV.vcreatedon, 113) AS CreatedOn, 
             CONVERT(VARCHAR(24), CV.vupdatedon, 113) AS UpdatedOn, 
             OU.username                              AS UpdatedBy, 
             OUS.username                             AS CreatedBy, 
             CV.maskingname, --added By prashant on 16 sept 2013 
			 CONVERT(DATE,CV.Discontinuation_date) AS Discontinuation_date,
			 CONVERT(DATE,CV.LaunchDate) AS LaunchDate
      FROM   carsegments AS Se WITH(NOLOCK), 
             carbodystyles AS Bo WITH(NOLOCK), 
             carmodels AS Mo WITH(NOLOCK), 
             (((carversions AS CV WITH(NOLOCK) 
                LEFT JOIN carsubsegments AS SSe WITH(NOLOCK)
                       ON CV.subsegmentid = SSe.id) 
               LEFT JOIN oprusers AS OU WITH(NOLOCK)
                      ON CV.vupdatedby = OU.id) 
              LEFT JOIN oprusers AS OUS WITH(NOLOCK) 
                     ON CV.vcreatedby = OUS.id )  --added By prashant on 16 sept 2013 
      WHERE  CV.segmentid = Se.id 
             AND CV.bodystyleid = Bo.id 
             AND CV.isdeleted = 0 
             AND Mo.id = CV.carmodelid 
             AND CV.id = @Id 
  END 
