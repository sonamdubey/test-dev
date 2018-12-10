IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DealerList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DealerList]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	20th Nov 2013
-- Description	:	Show all active Dealer list
-- Modifier		:	Sachin Bharti(22nd April 2014)
-- Purpose		:	Get callIds for BackOffice and SalesField executives
-- Modified by : Manish on 30-07-2014 added with Recompile option in query
-- Modified by : Vinay Kumar Prajapati on 18 nov 2014 select new colomn ApplicationType and CityId.
-- Modifier	:	Sachin Bharti(4th June 2015)
-- Purpose	:	Added column DealerType
-- Modified By : Sunil M. Yadav On 07 Jan 2016
-- Purpose :	@ApplicationId added for bikewale and carwale dealer.
-- Modifier : Ajay Singh(2 feb 2016) 
-- Description: Added parameter IsGroup(2 for group and 1 for multioutlet and 0 for outlets) 
-- Modifier :	Amit Yadav(8 Feb 2016)
-- Purpose	:	Field for check if the dealer is legal or not.
-- Modifier : Vaibhav K (25-Mar-2016) add filter to search dealer list by dealerId
-- Modifier : Vaibhav K (7-Sept-2016) added left join CWCTDealerMapping to get IsMigrated and CTDealerId
-- Modifier : Kartik Rathod on 28 sept 2016,  removes if else codintion for @IsGroup and fetch package details for dealer
-- exec [dbo].[DCRM_DealerList] 1,null,null,null,null,null,null,null,null,null,null,null,0
-- ============================================= 
CREATE PROCEDURE [dbo].[DCRM_DealerList] 
	@ApplicationId	INT				=	NULL,
	@DealerType		TINYINT			=	NULL,
	@StateId		INT				=	NULL,
	@CityId			INT				=	NULL,
	@AreaId			INT				=	NULL,
	@Organization	VARCHAR(100)	=	NULL,
	@EmailId		VARCHAR(100)	=	NULL,
	@IsDealerActive	TINYINT			=	NULL,
	@VerificatnStatus	TINYINT		=	NULL,
	@SortCriteria	INT				=	NULL,
	@SortDirection	INT				=	NULL,
	@UserId			INT				=   NULL,
	@IsGroup        INT             = 0,
	@DealerId		INT				=	NULL
AS
 BEGIN
 SELECT 
		DISTINCT D.ID AS ID, D.Organization AS Dealer,D.OutletCnt ,D.ID AS DealerId,D.ApplicationId,D.CityId,
		D.LogoUrl AS DealerLogo,D.PhoneNo, D.MobileNo, D.EmailId,
		--(SELECT Count(Inquiryid) from LiveListings li with (NOLOCK) where D.Id = Li.DealerId AND Li.SellerType = 1 ) 
		-- 1  As LiveStock,
		JoiningDate,  A.Name AS Area, 
		C.Name AS Location ,DAR.Name AS Region,S.Name AS State,
		CASE D.IsTCDealer WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END TCStatus , 
		CASE ISNULL(D.Status,0) WHEN 'False' THEN 1 ELSE 0 END AS IsDealerActive , 
		CASE ISNULL(D.IsDealerDeleted,0) WHEN 'True' THEN 1 ELSE 0 END AS IsDealerDeleted ,DNR.Reason,D.DeletedComment, 
		ISNULL((SELECT TOP 1 DC.Id FROM DCRM_Calls DC WITH(NOLOCK) 
			INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DC.UserId = DAU.UserId AND DC.DealerId = DAU.DealerId AND DAU.UserId = @UserId
			WHERE DC.DealerId = D.ID AND DAU.RoleId IN (3,5) AND DC.ActionTakenId =2 ),0)AS FldCallId,	--Get calls only for field users
		ISNULL((SELECT TOP 1 DC.Id FROM DCRM_Calls DC WITH(NOLOCK) 
			INNER JOIN DCRM_ADM_UserDealers DAU WITH(NOLOCK) ON DC.UserId = DAU.UserId AND DC.DealerId = DAU.DealerId AND DAU.UserId = @UserId
			WHERE DC.DealerId = D.ID AND DAU.RoleId IN (4) AND DC.ActionTakenId =2 ),0)AS BOCallId,		--Get calls only for Back office users
		(SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU WITH(NOLOCK) 
			INNER JOIN OprUsers OU(NOLOCK) ON DAU.UserId = OU.Id WHERE DAU.DealerId = D.ID AND DAU.RoleId = 4 )AS ServiceBO,  
		(SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU WITH(NOLOCK) 
			INNER JOIN OprUsers OU(NOLOCK) ON DAU.UserId = OU.Id WHERE DAU.DealerId = D.ID AND DAU.RoleId = 3 )AS SalesField,  
		(SELECT TOP 1 OU.UserName FROM DCRM_ADM_UserDealers DAU WITH(NOLOCK) 
			INNER JOIN OprUsers OU(NOLOCK) ON DAU.UserId = OU.Id WHERE DAU.DealerId = D.ID AND DAU.RoleId = 5 )AS ServiceField,
		TC.DealerType
		,CASE WHEN D.TanNumber IS NULL OR D.PanNumber IS NULL OR D.LegalName IS NULL THEN 'No'
		ELSE 'Yes' END AS Legal,
		CMP.IsMigrated, CMP.CTDealerID CarTradeDealerId,	--Vaibhav K (7-Sept-2016)
		PK.Name AS PackageName,CCP.ExpiryDate AS PackageExpiryDate
	FROM 
		Dealers AS D (NOLOCK)
		-- LEFT JOIN LiveListings Li(NOLOCK) ON D.Id = Li.DealerId AND Li.SellerType = 1
		-- LEFT JOIN SellInquiries Si ON D.Id = Si.DealerId AND StatusId = 1 AND si.PackageExpiryDate >= CONVERT(VARCHAR, GETDATE(), 101)  
		LEFT JOIN DCRM_NotVerifiedReason DNR(NOLOCK) ON DNR.Id = D.DeletedReason 
		LEFT JOIN Areas AS A(NOLOCK) ON  A.ID = D.AreaId  
		LEFT JOIN DCRM_Calls DC(NOLOCK) ON DC.DealerId = D.Id AND DC.ActionTakenId = 2
		LEFT JOIN TC_DealerType(NOLOCK) TC ON TC.TC_DealerTypeId = D.TC_DealerTypeId 
		LEFT JOIN Cities AS C(NOLOCK) ON C.ID = D.CityId  
		LEFT JOIN DCRM_ADM_RegionCities DARC(NOLOCK) ON C.Id = DARC.CityId 
		LEFT JOIN DCRM_ADM_Regions DAR(NOLOCK) ON DARC.RegionId = DAR.Id 
		LEFT JOIN States S(NOLOCK) ON S.ID = C.StateId
		LEFT JOIN CWCTDealerMapping CMP WITH (NOLOCK) ON D.ID = CMP.CWDealerID	--Vaibhav K (7-Sept-2016)
		LEFT JOIN ConsumerCreditPoints CCP WITH (NOLOCK) on D.ID = CCP.consumerid and CCP.consumertype=1 and CONVERT(DATE,CCP.ExpiryDate) >= CONVERT(DATE,GETDATE())
		LEFT JOIN packages PK WITH (NOLOCK) on CCP.CustomerPackageId = PK.ID		-- added by kartik 29 sept
	WHERE 
		(@ApplicationId IS NULL OR D.ApplicationId = @ApplicationId)
		AND (@VerificatnStatus IS NULL OR D.DealerVerificationStatus = @VerificatnStatus)
		AND (@IsDealerActive IS NULL OR D.IsDealerActive = @IsDealerActive)  
		AND (@DealerType IS NULL OR TC.TC_DealerTypeId = @DealerType)
		AND (@Organization IS NULL OR D.Organization LIKE @Organization)
		AND (@EmailId IS NULL OR D.EmailId LIKE @EmailId)
		AND (@StateId IS NULL OR D.StateId = @StateId)
		AND (@CityId IS NULL OR D.CityId = @CityId)
		AND (@AreaId IS NULL OR A.ID = @AreaId)
		AND (		--added by kartik removes if else codintion for @IsGroup
				( ISNULL(@IsGroup,0) = 1 AND D.IsGroup = 0 AND  D.IsMultiOutlet = 1)  OR (  ISNULL(@IsGroup,0) = 2 AND D.IsGroup = 1 AND D.IsMultiOutlet = 0) OR ( ISNULL(@IsGroup,0) = 0 AND  D.IsMultiOutlet = 0	AND D.IsGroup = 0)
			)
		AND (@DealerId IS NULL OR D.ID = @DealerId)--Vaibhav K 25-Mar-2016
	/*GROUP BY 
		D.ID, D.Organization, D.LogoUrl, JoiningDate, DAR.Name, A.Name, C.Name, D.PhoneNo,S.Name,D.ApplicationId,D.CityId,
		D.MobileNo, D.EmailId,D.IsTCDealer,DNR.Reason,D.DeletedComment,  CASE ISNULL(D.IsDealerDeleted,0)
		WHEN 'True' THEN 1 ELSE 0 END ,CASE ISNULL(D.Status,0) WHEN 'False' THEN 1 ELSE 0 END ,DC.Id ,D.OutletCnt ,
		TC.DealerType, D.TanNumber,D.PanNumber, D.LegalName, CMP.IsMigrated, CMP.CTDealerID,PK.Name,CCP.ExpiryDate*/
	 -- ORDER BY 
		 -- LiveStock DESC
		OPTION (RECOMPILE);
	IF @UserId IS NOT NULL
	BEGIN
		SELECT DAU.RoleId FROM DCRM_ADM_UserRoles DAU(NOLOCK) WHERE DAU.UserId = @UserId
	END
 END

