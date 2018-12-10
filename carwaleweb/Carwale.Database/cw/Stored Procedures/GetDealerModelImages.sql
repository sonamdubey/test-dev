IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetDealerModelImages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetDealerModelImages]
GO

	




-- ============================================= 
-- Author: Prashant Vishe    
-- Create date: <28 Oct 2013> 
-- Description:for retrieving dealer model images related information..
-- Modified By Sourav Roy on 4/13/2015 , Retrieve Data from CarModels instead of Microsite_images
-- ============================================= 
CREATE PROCEDURE [cw].[GetDealerModelImages] 
  -- Add the parameters for the stored procedure here 
  @DealerId INT 
AS 
  BEGIN 
      -- SET NOCOUNT ON added to prevent extra result sets from 
      -- interfering with SELECT statements. 
      SET nocount ON; 

      -- Insert statements for procedure here 
      SELECT Cast(ID AS VARCHAR) + '_http://' + XLargePic  AS ImageUrl, 
             ID AS modelId, 
             'http://' + XLargePic AS orgImageUrl 
      FROM CarModels CM WITH(NOLOCK) --Modified By Sourav Roy on 4/13/2015 
	  INNER JOIN TC_DealerMakes DM WITH(NOLOCK) ON DM.MakeId=Cm.CarMakeId 
	  Where Dm.DealerId=@DealerId	
		AND IsDeleted = 0 
		AND Futuristic = 0 AND New = 1 

      SELECT CM.name AS MakeName 
      FROM   carmakes CM WITH(NOLOCK)
			  INNER JOIN tc_dealermakes TDM WITH(NOLOCK)
			  ON TDM.makeid = CM.id 
      WHERE  TDM.dealerid = @DealerId 
  END 




