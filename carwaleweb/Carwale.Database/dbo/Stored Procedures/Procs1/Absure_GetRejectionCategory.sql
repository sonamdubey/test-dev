IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetRejectionCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetRejectionCategory]
GO

	
-- =============================================
-- Author:        Vinay Kumar Prajapati
-- Create date:  19th  May 2015
-- Description:    To Get All Category and  Rejection Reason
-- Modified By : Ruchira Patil on 29th May 2015 (to fetch only those categories and subcategories which are of ctq type)
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetRejectionCategory] 
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

   -- For Basic Category

      SELECT RR.Id, RR.Reason FROM  AbSure_RejectionReasons AS RR WITH(NOLOCK)  WHERE RR.IsActive=1
  

   --- For CTQ Category And subCategory

       --Commented by ruchira Patil on 29th May 2015
       --   SELECT  QCG.AbSure_QCategoryId AS CategoryId, QCG.Category  FROM AbSure_QCategory AS QCG WITH(NOLOCK)
      --WHERE QCG.IsActive=1 
      --ORDER BY QCG.Category

      
      SELECT DISTINCT CQ.AbSure_QCategoryId AS CategoryId,CQ.Category 
      FROM AbSure_Questions Q
      INNER JOIN AbSure_QCategory CQ ON CQ.AbSure_QCategoryId = Q.AbSure_QCategoryId
      WHERE AbSure_CTQTypeId=1 and Q.isactive=1

       --Commented by ruchira Patil on 29th May 2015
   --   SELECT  AQC.AbSure_QCategoryId AS CategoryId,SC.AbSure_QSubCategoryId AS SubCategoryId,SC.SubCategory 
      --FROM AbSure_QCategory AS AQC WITH(NOLOCK)
   --   INNER JOIN AbSure_QSubCategory AS SC WITH(NOLOCK) ON  SC.AbSure_QCategoryId =AQC.AbSure_QCategoryId
      --WHERE AQC.IsActive=1 AND Sc.IsActive=1
      --ORDER BY AQC.Category,SC.SubCategory

      SELECT DISTINCT AQC.AbSure_QCategoryId AS CategoryId,SC.AbSure_QSubCategoryId AS SubCategoryId,SC.SubCategory 
      FROM AbSure_Questions Q WITH(NOLOCK)
      INNER JOIN AbSure_QCategory AS AQC WITH(NOLOCK) ON Q.AbSure_QCategoryId = AQC.AbSure_QCategoryId
      INNER JOIN AbSure_QSubCategory AS SC WITH(NOLOCK) ON  SC.AbSure_QCategoryId =AQC.AbSure_QCategoryId
      WHERE AQC.IsActive=1 AND Sc.IsActive=1 AND Q.AbSure_CTQTypeId=1 AND Q.IsActive=1

END