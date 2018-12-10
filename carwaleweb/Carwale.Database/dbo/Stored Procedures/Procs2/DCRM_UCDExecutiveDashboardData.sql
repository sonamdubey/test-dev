IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UCDExecutiveDashboardData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UCDExecutiveDashboardData]
GO

	
----------------------------------------------------
-- Author : Komal Manjare
-- Date   : 10-March-2016
-- Description : get all dealers details mapped against @ExecutiveId for UCD Dealers
-- Modified By : Komal Manjare(21-April-2016)
-- Description : get legal flag and legaldetails for dealer
-- Modified By : Komal Manjare(09-May-2016)
-- Desc : change about to expire logic  
-- Modified By : Komal Manjare on 25 July 2016 ,Isdeleted condition for dealers insteadt of status check
-- Modified By : Sunil M. Yadav On 09th Nov 2016, select UploadedStocks AS 0 ,comment select from SellInquiries, because table will be deleted after mysql release
-------------------------------------------------
CREATE PROCEDURE [dbo].[DCRM_UCDExecutiveDashboardData]	
 @ExecutiveId    INT  
AS  
BEGIN
	IF @ExecutiveId > 0 
   BEGIN 
   --Dealer Data

   DECLARE @CurrentDate DATE=GETDATE()

		DECLARE @TempDealers Table(DealerId INT, DealerName VARCHAR(250),DealerType INT,DealerJoiningDate DATE,DealerExpiryDate DATE,IsLegal BIT,LegalName VARCHAR(250),TanNumber VARCHAR(50),PanNumber VARCHAR(50),ApplicationId INT) -- DealerType,DealerJoiningDate, DealerExpiryDate added
		
		INSERT INTO @TempDealers(DealerId, DealerName,DealerType,DealerJoiningDate,DealerExpiryDate,IsLegal,LegalName,TanNumber,PanNumber,ApplicationId)
		SELECT DISTINCT D.ID AS DealerId, D.Organization AS DealerName,D.TC_DealerTypeId AS DealerType,
						D.JoiningDate AS DealerJoiningDate,D.ExpiryDate AS DealerExpiryDate,
						CASE WHEN  D.TC_DealerTypeId IN(1,3) AND (D.TanNumber IS NULL OR D.PanNumber IS NULL OR D.LegalName IS NULL)  THEN 0 -- only for NCD and NCD -UCD
						ELSE 1 END AS IsLegal,D.LegalName,D.TanNumber,D.PanNumber,ApplicationId	
		FROM Dealers D WITH (NOLOCK) 
		INNER JOIN DCRM_ADM_UserDealers AS DAU  WITH (NOLOCK) ON DAU.DealerId= D.ID AND DAU.RoleId = 3 AND D.TC_DealerTypeId IN ( 1,3) AND 
		D.IsDealerDeleted=0 -- Modified By:Komal Manjare on 25 July 2016 ,Isdeleted condition for dealers insteadt of status check
		-- Status = 0
		WHERE DAU.UserId =@ExecutiveId 
		
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	--pacakge data

	DECLARE @TempPackageData TABLE(DealerId INT,PackageId INT,PackageName VARCHAR(250),PackageStartDate DATE,PackageEndDate DATE)

	INSERT INTO @TempPackageData(DealerId,PackageId,PackageName,PackageStartDate,PackageEndDate)
	SELECT DealerId,PackageId,PackageName,PackageStartDate,PackageEndDate
	FROM 
		(SELECT TD.DealerId,  Pkg.Id AS PackageId,Pkg.Name AS PackageName,CPR.ApprovalDate AS PackageStartDate ,CCP.ExpiryDate AS PackageEndDate
		 ,(ROW_NUMBER()OVER (PARTITION BY CPR.ConsumerId ORDER BY CPR.ApprovalDate DESC)) AS ROWNUM 

		 FROM ConsumerPackageRequests CPR(NOLOCK)
		 INNER JOIN @TempDealers AS TD  ON TD.DealerId=CPR.ConsumerId
		 INNER JOIN ConsumerCreditPoints CCP(NOLOCK) ON CCP.ConsumerId = CPR.ConsumerId 
		 INNER JOIN Packages PKG(NOLOCK)  ON PKG.Id = Cpr.PackageId  
		 WHERE CCP.ConsumerType = 1 AND  CPR.ConsumerType = 1 AND CPR.IsActive = 1 	AND CPR.ApprovalDate IS NOT NULL
		) T where ROWNUM = 1

-----------------------------------------------------------------------------------------------------------------------------------------------------


   --Available stock for the dealer
	DECLARE @TempAvailableStocks TABLE( DealerId INT,AvailableStocks INT)
	
	INSERT INTO @TempAvailableStocks(DealerId,AvailableStocks)
	SELECT TD.DealerId,COUNT(TS.ID)
	FROM @TempDealers TD 
	INNER JOIN TC_Stock TS(NOLOCK) ON TS.BranchId=TD.DealerId and TS.IsActive=1
	GROUP BY TD.DealerId,TS.IsActive
	--select * from @TempAvailableStocks	  
   -----------------------------------------------------------------------------------------------------------------------------------------------------
   --Uploaded Stock for dealer													-- Sunil M. Yadav On 09th Nov 2016
	--DECLARE @TempUploadedStocks TABLE( DealerId INT,UploadedStocks INT) 
	--INSERT INTO @TempUploadedStocks(DealerId,UploadedStocks)
	--SELECT TD.DealerId,COUNT(DISTINCT SI.ID)
		
	--FROM @TempDealers TD
	--INNER JOIN TC_Stock TS(NOLOCK) ON TS.BranchId=TD.DealerId 
	--INNER JOIN SellInquiries SI(NOLOCK) ON SI.DealerId = TD.DealerId AND SI.StatusId=1
	--GROUP BY TD.DealerId,SI.StatusId
	--select * from @TempUploadedStocks	
  -----------------------------------------------------------------------------------------------------------------------------------------------------
   --Stock without photos for dealer																-- Sunil M. Yadav On 09th Nov 2016
	--DECLARE @TempStockWithoutPhotos TABLE( DealerId INT,StockWithoutPhotos INT)
	--INSERT INTO @TempStockWithoutPhotos(DealerId,StockWithoutPhotos)
	--SELECT TD.DealerId,COUNT(DISTINCT SI.ID) AS StockWithoutPhotos
		
	--FROM @TempDealers TD
	--INNER JOIN SellInquiries SI(NOLOCK) ON SI.DealerId = TD.DealerId
	--LEFT JOIN CarPhotos CP WITH(NOLOCK) ON CP.InquiryId=SI.ID  AND CP.IsActive=1 AND CP.IsDealer=1
	--WHERE CP.InquiryId IS NULL
	--GROUP BY TD.DealerId
	
	--select * from @TempStockWithoutPhotos
	 -----------------------------------------------------------------------------------------------------------------------------------------------------
   --cars booked for dealers

	DECLARE @TempCarsBooked TABLE( DealerId INT,CarsBooked INT)

	INSERT INTO @TempCarsBooked(DealerId,CarsBooked)
	SELECT  TD.DealerId,COUNT(TBI.StockId) AS BookedCars	
	FROM @TempDealers TD
	JOIN TC_Stock TS(NOLOCK) ON TS.BranchId=TD.DealerId
	JOIN TC_BuyerInquiries TBI (NOLOCK) ON TBI.StockId=TS.Id  
	JOIN TC_InquirySource TIS WITH(NOLOCK) ON TIS.Id = TBI.TC_InquirySourceId 
	WHERE TBI.BookingStatus = 34 AND (DATEDIFF(day,TBI.BookingDate,@CurrentDate) BETWEEN  0 AND 30) AND TIS.TC_InquiryGroupSourceId = 11
	GROUP BY TD.DealerId
	--select * from @TempCarsBooked
	------------------------------------------------------------------------------------------------------------------------------------------------
	--Leads Delivered for dealer

	DECLARE @TempLeadsDelivered TABLE( DealerId INT,LeadsDelivered INT)

	INSERT INTO @TempLeadsDelivered(DealerId,LeadsDelivered)
	SELECT DISTINCT  TD.DealerId,COUNT(TBI.StockId) AS LeadsDelivered
	FROM @TempDealers TD
	JOIN TC_Stock TS(NOLOCK) ON TS.BranchId=TD.DealerId
	JOIN TC_BuyerInquiries TBI (NOLOCK) ON TBI.StockId=TS.Id  
	JOIN TC_InquirySource TIS WITH(NOLOCK) ON TIS.Id = TBI.TC_InquirySourceId  
	WHERE DATEDIFF(day,TBI.CreatedOn,GETDATE()) BETWEEN  0 AND 30
	GROUP BY TD.DealerId
	----------------------------------------------------------------------------------------------------------------------------------------------------
	 --Last Meeting for dealer
		DECLARE @TempLastMeetingDate Table (DealerId INT , LastMeetingDate DATETIME)

		INSERT INTO @TempLastMeetingDate(DealerId,LastMeetingDate)
		SELECT DealerId,MeetingDate 
		FROM(
				SELECT  DSM.DealerId, DSM.MeetingDate,(ROW_NUMBER() OVER(PARTITION BY DSM.DealerId ORDER BY DSM.MeetingDate DESC)) AS ROWNUM
				FROM DCRM_SalesMeeting DSM WITH(NOLOCK)
				JOIN @TempDealers TD ON TD.DealerId = DSM.DealerId
		) T WHERE ROWNUM = 1 

	--------------------------------------------------------------------------------------------------------------------------------------------	
	 	
	SELECT TD.DealerId,TD.DealerName,TD.DealerType,TD.DealerExpiryDate,TD.DealerJoiningDate,							 --DealerDetails
		TPD.PackageId,TPD.PackageName,TPD.PackageStartDate,TPD.PackageEndDate,TMD.LastMeetingDate,						 --PackageDetails
		TAS.AvailableStocks,0 UploadedStocks --TUS.UploadedStocks													Sunil M. Yadav On 09th Nov 2016
		,0 StockWithoutPhotos,TCB.CarsBooked,TLD.LeadsDelivered,				 --StockDetails			
		CASE 		 
			WHEN (DATEDIFF(day,TD.DealerJoiningDate,@CurrentDate) BETWEEN  0 AND 30) AND TD.DealerExpiryDate IS NOT NULL
			AND  (DATEDIFF(day,TD.DealerExpiryDate,@CurrentDate) < 0)  THEN 1
			ELSE 0
		END IsNewDealer,
		CASE  
			WHEN (DATEDIFF(day,@CurrentDate,TPD.PackageEndDate) BETWEEN  0 AND 7) THEN 1 ELSE 0   --Modified:Komal Manjare(09-may-2016)  datediff condition modified
		END IsAboutToExpire,
		CASE 
			WHEN (DATEDIFF(day,TPD.PackageEndDate,@CurrentDate)>0) THEN 1 ELSE 0 
		END IsExpired,
		TD.IsLegal,Td.LegalName,TD.TanNumber,TD.PanNumber,TD.ApplicationId																--LegalDetails

		FROM @TempDealers TD
		LEFT JOIN @TempPackageData TPD ON TPD.DealerId=TD.DealerId
		LEFT JOIN @TempAvailableStocks TAS ON TAS.DealerId=TD.DealerId
		-- LEFT JOIN @TempUploadedStocks TUS ON TUS.DealerId=TD.DealerId
		-- LEFT JOIN @TempStockWithoutPhotos TSWP ON TSWP.DealerId=TD.DealerId
		LEFT JOIN @TempCarsBooked TCB ON TCB.DealerId=TD.DealerId
		LEFT JOIN @TempLeadsDelivered TLD ON TLD.DealerId=TD.DealerId
		LEFT JOIN @TempLastMeetingDate TMD ON TMD.DealerId=TD.DealerId

	    order by TD.DealerName desc
  
   END

END

