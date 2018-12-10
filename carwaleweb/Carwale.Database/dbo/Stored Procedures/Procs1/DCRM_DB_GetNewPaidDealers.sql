IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DB_GetNewPaidDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DB_GetNewPaidDealers]
GO

	 
--==============================================
-- Author:		Mihir A Chheda
-- Create date: 25-02-2016
-- Description:	fetch users target and its achievement based on date range specified for dash board 
--              consider only 34,30,31,32,33,81,47,77,39 pakages
--				[SELECT * FROM DCRM_DB_NewPaidDealers]

--- Modified By : Mihir A Chheda ON 9/3/2016  Removed join from ConsumerPackageRequests and used
--                ConsumerCreditPointslogs table for join condition

--- Modified By : Mihir A Chheda ON 11/3/2016  considered the RVN_DealerPackageFeatures table to get package detail of Seller Leads 

--NOTE  If any packages[34,30,31,32,33,81,47,77] from this list and package[39] is pithced to dealer and delivery start on same date
--      then it will not be considered as new paid dealer although last package having start date befor 6 month
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DB_GetNewPaidDealers]
AS
BEGIN
	
	DECLARE @UCDPackageIds VARCHAR(150) = '34,30,31,32,33,81,47,77,39' -- this id taken from pakages table 
	DECLARE @FirstDayOfCurrentMonth DATE=CONVERT(DATE,DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0))
	DECLARE @SixMonthOldDate  DATE=DATEADD(M,-6,CONVERT(DATE,GETDATE()))

    --get list of dealers by considering package which they have taken,last package expirydate > current month ,also last entry in DCRM_DB_NewPaidDealers > 6
    SELECT	 PackageDetail.UserId,PackageDetail.ConsumerId,PackageDetail.PackageId,PackageDetail.StartDate,PackageDetail.EndDate
	INTO	 #TEMPPackageDetail
    FROM 
		(SELECT      UD.UserId,CCPL.ConsumerId,CCPL.PackageId ,CONVERT(DATE,CCPL.EntryDate) StartDate,
					CCP.ExpiryDate EndDate
		FROM		ConsumerCreditPoints(NOLOCK) CCP
		JOIN		ConsumerCreditPointsLogs(NOLOCK) CCPL ON CCPL.ConsumerId=CCP.ConsumerId
		JOIN		InquiryPointCategory(NOLOCK) IPC ON IPC.Id=CCP.PackageType	
		JOIN        Packages(NOLOCK) P ON P.Id=CCP.CustomerPackageId
		JOIN	    DCRM_ADM_UserDealers UD WITH(NOLOCK) ON UD.DealerId = CCP.ConsumerId AND UD.RoleId = 3
		WHERE		CCP.ConsumerType=1 AND CCPL.ConsumerType=1 
					AND CCP.CustomerPackageId IN (SELECT ListMember FROM fnSplitCSV(@UCDPackageIds)) --(34,30,31,32,33,81,47,77,39) 
					AND CCPL.PackageId IN (SELECT ListMember FROM fnSplitCSV(@UCDPackageIds)) --(34,30,31,32,33,81,47,77,39) 
					AND IPC.isActive=1 AND P.isActive=1 AND CCP.ExpiryDate > @FirstDayOfCurrentMonth 	
					AND CCP.ConsumerId NOT IN (SELECT DealerId FROM DCRM_DB_NewPaidDealers WITH(NOLOCK) WHERE EntryDate > @SixMonthOldDate )           
		UNION ALL

		SELECT		AUD.UserId,RVN.DealerId,RVN.PackageId,RVN.PackageStartDate,RVN.PackageEndDate
		FROM		RVN_DealerPackageFeatures RVN WITH(NOLOCK)
		JOIN 	DCRM_ADM_UserDealers AUD WITH(NOLOCK) ON AUD.DealerID = RVN.DealerID AND AUD.RoleId = 3	
		WHERE	    RVN.PackageEndDate  > @FirstDayOfCurrentMonth AND RVN.PackageId = 39 AND RVN.PackageStatus = 2 AND RVN.CampaignType = 3 --for paid only campaignType = 3
					AND  RVN.DealerId NOT IN (SELECT DealerId FROM DCRM_DB_NewPaidDealers WITH(NOLOCK) WHERE EntryDate > @SixMonthOldDate )           
		)AS PackageDetail
	ORDER BY PackageDetail.ConsumerId


	--get list of dealers by considering package which they have taken,last package expirydate > current month ,also last entry in DCRM_DB_NewPaidDealers > 6 month
	DECLARE		@TemptTable Table(UserId INT,ConsumerId INT,PackageId INT,StartDate DATE,EndDate DATE,RowNumber INT)
	INSERT INTO @TemptTable(UserId,ConsumerId,PackageId ,StartDate,EndDate,RowNumber)
	SELECT      temp.UserId,temp.ConsumerId,temp.PackageId,temp.StartDate,temp.EndDate,
				ROW_NUMBER() OVER(PARTITION BY temp.ConsumerId ORDER BY temp.StartDate DESC) AS RowNum
	FROM        #TEMPPackageDetail temp
	

	--get list of dealers who's taken package only once till now and package is approved in current month consider that dealer as achievement
	DECLARE @TemptTable1 Table(UserId INT,ConsumerId INT,PackageId INT,StartDate DATE,EndDate DATE)
	INSERT INTO @TemptTable1(UserId,ConsumerId,PackageId ,StartDate,EndDate )
	SELECT		 UserId,ConsumerId,PackageId,StartDate,EndDate
	FROM		@TemptTable
	WHERE		ConsumerId IN (
				SELECT ConsumerId
				FROM   @TemptTable
				GROUP BY ConsumerId
				HAVING COUNT(ConsumerId)  = 1 
				)AND StartDate >= @FirstDayOfCurrentMonth

   
    --get list of dealers who's taken many packages  till now but the difference between last two package is more then 6 month  and package is approved in current month consider that dealer as achievement
	DECLARE @TemptTable2 Table(UserId INT,ConsumerId INT,PackageId INT,StartDate DATE,EndDate DATE)
	INSERT INTO	@TemptTable2(UserId,ConsumerId,PackageId ,StartDate,EndDate )
	SELECT		t1.UserId,t1.ConsumerId,t1.PackageId,t1.StartDate,t1.EndDate
	FROM		@TemptTable t1
	JOIN		@TemptTable t2  ON t1.ConsumerId=t2.ConsumerId
	WHERE		t1.RowNumber=1 AND t2.RowNumber=2 
				AND DATEDIFF(MONTH,t2.StartDate,t1.StartDate)  > 6 
				AND t1.StartDate  >= @FirstDayOfCurrentMonth


	INSERT INTO DCRM_DB_NewPaidDealers(UserId,DealerId,PackageId,StartDate,EndDate)
	SELECT UserId,ConsumerId,PackageId,StartDate,EndDate FROM @TemptTable1

	INSERT INTO DCRM_DB_NewPaidDealers(UserId,DealerId,PackageId,StartDate,EndDate)
	SELECT UserId,ConsumerId,PackageId,StartDate,EndDate FROM @TemptTable2

	DROP TABLE #TEMPPackageDetail
END
