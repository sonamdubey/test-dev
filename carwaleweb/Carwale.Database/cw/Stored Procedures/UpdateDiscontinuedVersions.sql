IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[UpdateDiscontinuedVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[UpdateDiscontinuedVersions]
GO

	         
-- =============================================          
CREATE PROCEDURE [cw].[UpdateDiscontinuedVersions]        
 @DiscontinuitionId numeric(18,0),         
 @Discontinuition_date datetime,         
 @ReplacedByVersionId smallint,         
 @comment varchar(5000),         
 @OldVersionId numeric(18,0)        
AS          
BEGIN          
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
          
  update CarVersions set New=0,DiscontinuationId=@DiscontinuitionId,ReplacedByVersionId=@ReplacedByVersionId,comment=@comment,Discontinuation_date=@Discontinuition_date where Id=@OldVersionId;        
  -- Added By Ravi Koshal to update min and max price of a model
   Execute UpdateModelPrices  @OldVersionId,NULL             
END 

