IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetLPADetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetLPADetails]
GO

	-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 27-04-2016
-- Description:	get product,dealer,L3 deatils to generate LPA automatically when product closing Stage =70%
-- EXEC DCRM_GetLPADetails 14093 12943  14045 12732 12943  1995  13917 13065 13065 14005
-- Modified By : Komal Manjare on(24-August-2016)
-- Desc : get L2name and L2Email for the respective L3
-- Modified By : Sunil M. Yadav On 03 Nov 2016, get owners mobile no. , email, name instead of contact person.
-- Modified By : Vaibhav K 03 Nov 2016, fetch more details - DealerAddress2,DealerStateName,DealerAreaName,DealerPincode
-- =============================================
CREATE  PROCEDURE [dbo].[DCRM_GetLPADetails] 
   @SalesId INT 
AS
BEGIN
    DECLARE @ContractType INT=0

	SET @ContractType =(SELECT	ISNULL(ContractType ,0)
	                    FROM	DCRM_SalesDealer(NOLOCK) 
	                    WHERE	Id=@SalesId)

    SELECT	D.ID DealerId ,D.LegalName DealerName,P.Id PackageId,P.Name PackageName,OU.UserName L3Name,
			OU.LoginId + '@carwale.com' L3Email,OU.PhoneNo L3ContactNumber,
			D.Address1 DealerAddress, ISNULL(D.Address2, '') DealerAddress2, ST.Name DealerStateName, ISNULL(AR.Name,'') DealerAreaName, D.Pincode DealerPincode,
			D.FirstName AS ContactPerson,D.EmailId AS ContactEmail,
			D.OwnerMobile ContactNumber,DSD.PitchDuration Duration,DSD.NoOfLeads NumberOfLeads,
			CASE 
				--WHEN DSD.ContractType = 2	THEN ISNULL(CAST(ROUND(DSD.ClosingAmount/NULLIF(DSD.PitchDuration,0),2)AS decimal(10,2)),0) 
				WHEN DSD.ContractType = 1	THEN ISNULL(CAST(ROUND(DSD.ClosingAmount/NULLIF(DSD.NoOfLeads,0),2) AS decimal(10,2)),0)
				ELSE 0 END AS CostPerLead ,
			ISNULL(DSD.ClosingAmount,0) ProductAmount,CONVERT(VARCHAR(50),DSD.UpdatedOn,107) LpaGenratedDate,
			DSD.ContractType,DSD.Id SalesId,DSD.Model CarName,C.Name DealerCityName,
			CONVERT(VARCHAR(50),DSD.StartDate,107) StartDate,CONVERT(VARCHAR(50),DATEADD(DAY,(DSD.PitchDuration-1),DSD.StartDate),107) EndDate,
			(SELECT OU.UserName FROM DCRM_ADM_MappedUsers DAM1 (NOLOCK) INNER JOIN OprUsers OU(NOLOCK)  ON OU.Id=DAM1.OprUserId WHERE DAM1.NodeRec=DAM.NodeRec.GetAncestor(1) AND OU.IsActive=1)AS L2Name, --Komal Manjare on 24-August-2016
			(SELECT OU.LoginId FROM DCRM_ADM_MappedUsers DAM1 (NOLOCK) INNER JOIN OprUsers OU(NOLOCK) ON OU.Id=DAM1.OprUserId WHERE DAM1.NodeRec=DAM.NodeRec.GetAncestor(1) AND OU.IsActive=1) +'@carwale.com' AS L2Email
	
	FROM	DCRM_SalesDealer(NOLOCK) DSD
	JOIN    Dealers(NOLOCK) D ON D.ID=DSD.DealerId
	JOIN    Packages(NOLOCK) P ON P.Id=DSD.PitchingProduct
	JOIN    DCRM_ADM_UserDealers(NOLOCK) DAUM ON D.ID=DAUM.DealerId
	JOIN    OprUsers(NOLOCK) OU ON OU.Id=DAUM.UserId
	JOIN    Cities(NOLOCK) C ON C.ID=D.CityId
	JOIN	States(NOLOCK) ST ON C.StateId = ST.ID
	LEFT JOIN Areas(NOLOCK) AR on D.AreaId = AR.ID
	JOIN	DCRM_ADM_MappedUsers DAM (NOLOCK) ON DAM.OprUserId=OU.Id -- Komal Manjare on 24-August-2016
	WHERE   DSD.Id=@SalesId AND DAUM.RoleId = 3
END
