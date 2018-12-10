IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetCurrentScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetCurrentScore]
GO

	

-- =============================================
-- Author:		Vinay Kumar
-- Create date: 20 AUG 2013
-- Description:	This proc is used to get current score of data in  pool & achieved Data
-- =============================================
CREATE PROCEDURE  [dbo].[CRM_GetCurrentScore]
AS
BEGIN 
  --current score
    SELECT COUNT(CDA.CBDId) AS TotalAchieved
        FROM CRM_CarDealerAssignment AS  CDA  WITH(NOLOCK)
        INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID
        WHERE CONVERT(DATE,CDA.CreatedOn,101) = CONVERT(DATE,GETDATE(),101)
        AND CBD.VersionId IN(SELECT Id FROM CarVersions AS CV WITH(NOLOCK)
        WHERE CV.CarModelId IN(SELECT GMM.ModelId
        FROM CRM_ADM_GroupModelMapping AS GMM WITH (NOLOCK) WHERE GMM.GroupType=1 ))
   --pool data
    SELECT COUNT(DISTINCT CL.ID) AS Pool
        FROM CRM_Leads AS CL  WITH(NOLOCK) 
        WHERE CL.LeadStageId = 1 AND CL.LeadStatusId = -1 AND CL.Owner = -1
        AND CL.GroupId IN(SELECT CAF.Id FROM CRM_ADM_FLCGroups AS CAF WITH(NOLOCK) WHERE CAF.GroupType = 1 )
       -- AND CONVERT(DATE,Cl.CreatedOn,101) = CONVERT(DATE,GETDATE(),101)

	 
 END
 
 
 
 
 
 
