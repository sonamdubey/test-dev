IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelSummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelSummary]
GO

	-- =============================================  
-- Author:  Reshma Shetty  
-- Create date: 26/04/2013  
-- Description: Returns distinct specs summary for a model  
--Modified By :Shalini
--				added itemRank
--Modified By : Shalini on 04/09/14	corrected the wrong itemmasterIds  
--               EXEC GetModelSummary 31  
-- =============================================  
CREATE PROCEDURE [dbo].[GetModelSummary]   
 -- Add the parameters for the stored procedure here  
 @ModelId INT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
  with cte as (
  SELECT DISTINCT IM.ItemMasterId AS ItemID,CASE IM.ItemMasterId WHEN 72 THEN 'Steering' ELSE IM.NAME END  AS Item,  
  COALESCE(CASE   
   WHEN IV.ItemMasterId = 72  
    THEN  CASE WHEN IV.ItemValue =0  
       THEN 'Manual steering'  
       WHEN IV.ItemValue =1 THEN 'Power steering'  
       END  
   ELSE IV.CustomText  
   END,  
  UDF.NAME +' '+'seater',  
  CONVERT(VARCHAR(20),IV.ItemValue)) AS Value  
  FROM CD.ItemValues IV WITH(NOLOCK)  
  INNER JOIN CD.ItemMaster IM WITH(NOLOCK) ON IM.ItemMasterId = IV.ItemMasterId  
  INNER JOIN CarVersions CV WITH(NOLOCK) ON CV.ID = IV.CarVersionId  
  LEFT JOIN CD.UserDefinedMaster UDF WITH(NOLOCK) ON UDF.UserDefinedId = IV.UserDefinedId  
  WHERE IV.ItemMasterId IN (  
   257, --Engines  
   258, --GearBoxes  
   9, -- Seating Capacity  
   72 -- Steering Type  
   )  
  AND CV.IsDeleted=0 AND CV.New=1
  AND CarModelId = @ModelId 
  )

  select *,ROW_NUMBER() over(partition by ItemID order by Item) ItemRank from cte 
  ORDER BY Item 
END  
