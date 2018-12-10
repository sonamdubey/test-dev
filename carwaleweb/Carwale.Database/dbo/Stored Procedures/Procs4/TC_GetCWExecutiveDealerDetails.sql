IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCWExecutiveDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCWExecutiveDealerDetails]
GO

	-- =======================================================================================
-- Author		: Suresh Prajapati
-- Created Date : 16th Nov, 2015
-- Description	: To get details of the dealer under specified CW Feild Executive's BranchId
-- TC_GetCWExecutiveDealerDetails 18934
-- =======================================================================================
CREATE PROCEDURE [dbo].[TC_GetCWExecutiveDealerDetails] @BranchId INT
AS
BEGIN
	CREATE TABLE #TempAllDealers (
		DealerId INT
		,DealerTypeId INT
		,DealerFirstName VARCHAR(100)
		,DealerLastName VARCHAR(100)
		,OrganizationName VARCHAR(100)
		)

	INSERT INTO #TempAllDealers
	SELECT UD.DealerId
		,D.TC_DealerTypeId
		,D.FirstName
		,D.LastName
		,D.Organization
	FROM DCRM_ADM_UserDealers AS UD WITH (NOLOCK)
	INNER JOIN TC_CWExecutiveMapping AS EM WITH (NOLOCK) ON EM.OprUserId = UD.UserId
	INNER JOIN Dealers AS D WITH (NOLOCK) ON D.ID = UD.DealerId
	WHERE EM.BranchId = @BranchId
		AND D.IsDealerActive = 1
		AND D.IsDealerDeleted = 0
		AND D.TC_DealerTypeId IN (
			1
			,2
			)

	------------------- Below insertion added for Testing, need to be removed before taking it to production -------------------
	--INSERT INTO #TempAllDealers
	--VALUES (
	--	5
	--	,2
	--	,'TEST'
	--	,'TEST'
	--	)
	--INSERT INTO #TempAllDealers
	--VALUES (
	--	5
	--	,1
	--	,'TEST'
	--	,'TEST'
	--	)
	--INSERT INTO #TempAllDealers
	--VALUES (
	--	9
	--	,1
	--	,'TEST'
	--	,'TEST'
	--	)
	--------------------------------XXXX--------------------------------------------
	------------------------ NCD Details Stats Here ------------------------
	IF EXISTS (
			SELECT DealerTypeId
			FROM #TempAllDealers WITH (NOLOCK)
			WHERE DealerTypeId IN(1,2) -- I.E NCD DEALER
			)
	BEGIN
		WITH CtePackage
		AS (
			SELECT AD.DealerId AS DealerId
				,P.NAME AS PackageName
				,CPR.ApprovalDate AS StartDate
				,DATEADD(DAY, CPR.ActualValidity, CPR.ApprovalDate) AS ExpiryDate
				,ROW_NUMBER() OVER (
					PARTITION BY AD.DealerId ORDER BY CPR.ApprovalDate DESC
					) AS RowNum
			FROM ConsumerPackageRequests AS CPR WITH (NOLOCK)
			INNER JOIN Packages AS P WITH (NOLOCK) ON P.Id = CPR.PackageId
			JOIN #TempAllDealers AS AD WITH (NOLOCK) ON AD.DealerId = CPR.ConsumerId
			WHERE CPR.ConsumerType = 1
				AND AD.DealerTypeId = 2
				--ORDER BY CPR.Id DESC
			)
			,CteContract
		AS (
			SELECT AD.DealerId AS DealerId
				,SUM(ISNULL(CCM.TotalGoal, 0)) AS NumberOfLeadSigned
				,LatestCampaign.TotalDelivered AS NumberOfLeadDelivered -- Against Cuurent Contract
			--,LatestCampaign.ContractId AS ContractId -- Against Cuurent Contract
			FROM TC_ContractCampaignMapping AS CCM WITH (NOLOCK)
			INNER JOIN #TempAllDealers AS AD WITH (NOLOCK) ON AD.DealerId = CCM.DealerId
			INNER JOIN (
				SELECT TOP 1 ContractId
					,TotalDelivered
				FROM TC_ContractCampaignMapping WITH (NOLOCK)
				WHERE ContractStatus = 1
					AND ContractType = 1
					AND ContractBehaviour = 1
				ORDER BY StartDate DESC
				) AS LatestCampaign ON 1 = 1
			WHERE CCM.ContractStatus = 1
				AND CCM.ContractType = 1
				AND CCM.ContractBehaviour = 1
				AND AD.DealerTypeId = 2
			GROUP BY AD.DealerId
				,LatestCampaign.ContractId
				,LatestCampaign.TotalDelivered
			)
		SELECT CD.DealerId
			,AD.DealerFirstName
			,AD.DealerLastName
			,AD.DealerTypeId
			,AD.OrganizationName
			,PD.PackageName
			,PD.StartDate
			,PD.ExpiryDate
			,CD.NumberOfLeadDelivered
			,CD.NumberOfLeadSigned
		FROM CteContract AS CD WITH (NOLOCK)
		INNER JOIN CtePackage AS PD WITH (NOLOCK) ON PD.DealerId = CD.DealerId
		INNER JOIN #TempAllDealers AS AD WITH (NOLOCK) ON AD.DealerId = PD.DealerId
		WHERE PD.RowNum = 1
			AND AD.DealerTypeId = 2
	END

	------------------------ UCD Details Stats Here ------------------------
	IF EXISTS (
			SELECT DealerTypeId
			FROM #TempAllDealers WITH (NOLOCK)
			WHERE DealerTypeId = 1 -- I.E UCD DEALER
			)
	BEGIN
		DECLARE @FromDate DATETIME
			,@ToDate DATETIME

		SET @FromDate = DateAdd(MONTH, - 1, Convert(DATE, GetDate()))
		SET @ToDate = GETDATE();

		WITH CteStock
		AS (
			SELECT AD.DealerId AS DealerId
				,COUNT(DISTINCT S.Id) AS TotalStock
				,(
					SELECT COUNT(Id)
					FROM (
						SELECT S.Id AS Id
						FROM TC_Stock AS S WITH (NOLOCK)
						INNER JOIN CarVersions CV WITH (NOLOCK) ON S.VersionId = CV.ID -- to match exact count from stock list page
						INNER JOIN CarModels CMo WITH (NOLOCK) ON CV.CarModelId = CMo.ID
						INNER JOIN CarMakes CMa WITH (NOLOCK) ON CMo.CarMakeId = CMa.ID
						LEFT JOIN TC_CarPhotos AS CP WITH (NOLOCK) ON CP.StockId = S.Id
						WHERE S.StatusId = 1
							AND S.IsActive = 1
							AND S.IsApproved = 1
							AND CP.IsActive = 1
							AND S.BranchId = AD.DealerId
						GROUP BY S.Id
						HAVING COUNT(CP.Id) BETWEEN 1
								AND 4
						) AS T
					) AS StockWithLessImage
			FROM TC_Stock AS S WITH (NOLOCK)
			INNER JOIN #TempAllDealers AS AD WITH (NOLOCK) ON AD.DealerId = S.BranchId
			INNER JOIN CarVersions CV WITH (NOLOCK) ON S.VersionId = CV.ID -- to match exact count from stock list page
			WHERE S.StatusId = 1
				AND S.IsActive = 1
				AND S.IsApproved = 1
				--AND DATEDIFF(D, S.EntryDate, GETDATE()) > 30
				AND AD.DealerTypeId = 1
			GROUP BY AD.DealerId
			)
			,CteLead
		AS (
			SELECT S.BranchId AS DealerId
				,COUNT(DISTINCT TCBI.TC_BuyerInquiriesId) AS TotalLeads
			FROM TC_Lead AS TCL WITH (NOLOCK)
			JOIN TC_Stock AS S WITH (NOLOCK) ON S.BranchId = TCL.BranchId
			JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TCL.TC_LeadId = TCIL.TC_LeadId
			JOIN TC_BuyerInquiries AS TCBI WITH (NOLOCK) ON TCIL.TC_InquiriesLeadId = TCBI.TC_InquiriesLeadId
				AND TCIL.TC_LeadInquiryTypeId = 1
				AND TCBI.CreatedOn BETWEEN @FromDate
					AND @ToDate
			GROUP BY S.BranchId
			)
			,CtePackage
		AS (
			SELECT AD.DealerId AS DealerId
				,P.NAME AS PackageName
				,CPR.ApprovalDate AS StartDate
				,DATEADD(day, CPR.ActualValidity, CPR.ApprovalDate) AS ExpiryDate
				,ROW_NUMBER() OVER (
					PARTITION BY AD.DealerId ORDER BY CPR.ApprovalDate DESC
					) AS RowNum
			FROM ConsumerPackageRequests AS CPR WITH (NOLOCK)
			INNER JOIN Packages AS P WITH (NOLOCK) ON P.Id = CPR.PackageId
			JOIN #TempAllDealers AS AD WITH (NOLOCK) ON AD.DealerId = CPR.ConsumerId
			WHERE CPR.ConsumerType = 1
				AND AD.DealerTypeId = 1
				--ORDER BY CPR.Id DESC
			)
		SELECT SD.DealerId
			,AD.DealerFirstName
			,AD.DealerLastName
			,AD.DealerTypeId
			,AD.OrganizationName
			,SD.TotalStock
			,SD.StockWithLessImage
			,PD.PackageName
			,PD.StartDate
			,PD.ExpiryDate
			,LD.TotalLeads
		FROM CteStock AS SD WITH (NOLOCK)
		LEFT JOIN CteLead AS LD WITH (NOLOCK) ON SD.DealerId = LD.DealerId
		LEFT JOIN CtePackage AS PD WITH (NOLOCK) ON PD.DealerId = LD.DealerId
		INNER JOIN #TempAllDealers AS AD WITH (NOLOCK) ON AD.DealerId = PD.DealerId
		WHERE PD.RowNum = 1
			AND AD.DealerTypeId = 1

		DROP TABLE #TempAllDealers
	END
END

