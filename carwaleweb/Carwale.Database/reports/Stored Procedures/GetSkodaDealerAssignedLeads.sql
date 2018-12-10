IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetSkodaDealerAssignedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetSkodaDealerAssignedLeads]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Ocy 09 2011
-- Description:	Get Skoda Dealer AssignedLeads Report
-- =============================================
CREATE PROCEDURE [reports].[GetSkodaDealerAssignedLeads]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	
SELECT 
       ND.Id AS DealerId,
       ND.Name,
       C.Name AS City,
       --ND.Address,
       Month(CSD.StartDate) as Month,
       count(CSD.LeadId) as Leads
FROM CRM_Customers AS CC with(nolock)
     join CRM_Leads as CL with(nolock) on CL.CNS_CustId=CC.ID
     join CRM_CarBasicData as CBD with(nolock) on CBD.LeadId=CL.ID
     join CRM_CarDealerAssignment as CDA with(nolock) on CDA.CBDId=CBD.ID
     join CRM_SkodaDealerAssignment CSD with(nolock)on  CSD.LeadId=CL.ID and CSD.PushStatus='SUCCESS'
     join NCS_Dealers ND with(nolock) on ND.ID=CDA.DealerId
     join Cities AS C with(nolock) on C.ID=ND.CityId
     JOIN CRM_InterestedIn AS CII with(nolock) ON CL.Id = CII.LeadId
     JOIN CRM_PostData AS CP with(nolock) ON CL.Id = CP.LeadId
     JOIN CRM_LeadSource CLS with(nolock) ON CL.ID = CLS.LeadId
WHERE Year(CSD.StartDate)=2011
 group by ND.Id,ND.Name,C.Name,Month(CSD.StartDate)
END

