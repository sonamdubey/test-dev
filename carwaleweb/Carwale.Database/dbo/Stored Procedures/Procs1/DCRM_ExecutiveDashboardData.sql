IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ExecutiveDashboardData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ExecutiveDashboardData]
GO

	
----------------------------------------------------
-- Author : Sunil Yadav On 01 Desc 2015
-- Description : get all dealers details mapped against @ExecutiveId
-- Modified By : Komal Manjare(21-April-2016)
-- Description : get legal flag and legaldetails for NCD dealers
-- Modified By : Komal Manjare on 10-11-2016
-- Desc : get applicationId for dealer
-------------------------------------------------

CREATE PROCEDURE [dbo].[DCRM_ExecutiveDashboardData]	
 @ExecutiveId    INT  
AS  
  
BEGIN  
 IF @ExecutiveId > 0 
   BEGIN  		
		--Get All the Dealers
		DECLARE @TempDealers Table(DealerId INT, DealerName VARCHAR(250),DealerType INT,DealerJoiningDate DATE,DealerExpiryDate DATE,IsLegal BIT,LegalName VARCHAR(250),TanNumber VARCHAR(50),PanNumber VARCHAR(50),ApplicationId INT) -- DealerType,DealerJoiningDate, DealerExpiryDate added
		
		INSERT INTO @TempDealers(DealerId, DealerName,DealerType,DealerJoiningDate,DealerExpiryDate,Islegal,LegalName,TanNumber,PanNumber,ApplicationId ) -- Modified By : Komal Manjare(21-April-2016)
		SELECT DISTINCT D.ID AS DealerId, D.Organization AS DealerName,D.TC_DealerTypeId AS DealerType,D.JoiningDate AS DealerJoiningDate,D.ExpiryDate AS DealerExpiryDate,
		CASE WHEN  
					D.TC_DealerTypeId IN(2,3) AND (D.TanNumber IS NULL OR D.PanNumber IS NULL OR D.LegalName IS NULL)  THEN 0 -- only for NCD and NCD -UCD
			 ELSE 1 END AS IsLegal,D.LegalName,D.TanNumber,D.PanNumber,D.ApplicationId --  Komal Manjare on 10-11-2016
		FROM DCRM_ADM_UserDealers DAU WITH (NOLOCK) 
			INNER JOIN Dealers AS D  WITH (NOLOCK) ON DAU.DealerId = D.ID AND DAU.RoleId = 3 AND D.TC_DealerTypeId IN ( 2,3) AND 
			D.IsDealerDeleted=0 -- Modified By:Komal Manjare on 25 July 2016 ,Isdeleted condition for dealers insteadt of status check
			--Status = 0
		WHERE DAU.UserId =@ExecutiveId 
		
		--Get Contract Data
		DECLARE @TempContractData Table(DealerId INT, ContractStartDate DateTime, ContractEndDate DateTime, LeadSigned INT, LeadDelivered INT,ContractBehaviour INT,ContractType INT,ContractStatus INT) -- ContractBehaviour , ContractType added)
		
		INSERT INTO @TempContractData(DealerId, ContractStartDate, ContractEndDate, LeadSigned, LeadDelivered,ContractBehaviour,ContractType,ContractStatus )
		SELECT DealerId, StartDate, EndDate, TotalGoal, TotalDelivered,ContractBehaviour,ContractType,ContractStatus FROM 
		(
		SELECT TM.DealerId, TM.StartDate, TM.EndDate, TM.TotalGoal, TM.TotalDelivered, TM.ContractBehaviour,TM.ContractType,TM.ContractStatus ,(ROW_NUMBER()OVER (PARTITION BY TM.DealerId ORDER BY TM.ContractId DESC)) AS ROWNUM 
		FROM TC_ContractCampaignMapping TM WITH (NOLOCK) INNER JOIN @TempDealers TD ON TM.DealerId = TD.DealerId 
		--WHERE TM.ContractStatus = 1
		) T WHERE ROWNUM = 1
		
		DECLARE @FROMDATE DATE
		DECLARE @TODATE DATE
		SET @TODATE= CONVERT(DATE,GETDATE())
		SET @FROMDATE =DATEADD(day,-6,CONVERT(DATE,GETDATE())) 

		--get Test Drive for last week  
		DECLARE @TempTDLastWeek Table(DealerId INT,  TC_TDCalendarId INT ,TDStatus INT )
		INSERT INTO @TempTDLastWeek(DealerId,TC_TDCalendarId,TDStatus)
		SELECT TDC.BranchId,TDC.TC_TDCalendarId,TDC.TDStatus FROM TC_TDCalendar TDC  WITH(NOLOCK)
		JOIN @TempDealers D ON D.DealerId = TDC.BranchId
		WHERE TDC.TDDate BETWEEN @FROMDATE AND @TODATE AND TDC.TDStatus = 28 
		
		--get sum of all cars booked through carwale ex-showrrom amount  
		DECLARE @TempTotalCarBookedPrice Table(DealerId INT,Price INT,TotalBookedCar INT )
		
		INSERT INTO @TempTotalCarBookedPrice(DealerId,Price ,TotalBookedCar)
		EXEC DCRM_GetTotalBookedCarPrice @ExecutiveId
		
		--get last meering date 
		DECLARE @TempLastMeetingDate Table (DealerId INT , LastMeetingDate DATETIME)
		
		INSERT INTO @TempLastMeetingDate(DealerId,LastMeetingDate)
		SELECT DealerId,MeetingDate FROM(
		SELECT  DSM.DealerId, DSM.MeetingDate,(ROW_NUMBER() OVER(PARTITION BY DSM.DealerId ORDER BY DSM.MeetingDate DESC)) AS ROWNUM
		 FROM DCRM_SalesMeeting DSM WITH(NOLOCK)
		JOIN @TempDealers TD ON TD.DealerId = DSM.DealerId
		--ORDER BY DSM.MeetingDate
		) T WHERE ROWNUM = 1 
		
		--get all required fields from temp tables
		SELECT TD.DealerId,TD.DealerName,TD.DealerType,TD.DealerJoiningDate,TD.DealerExpiryDate,
		TCD.ContractStartDate,TCD.ContractEndDate,TCD.LeadSigned,TCD.LeadDelivered AS LeadsDelivered ,TCD.ContractBehaviour,TCD.ContractType, TCD.ContractStatus,

		CASE 
								WHEN TCD.ContractStatus = 3 THEN 1
								ELSE 0
								END IsExpired, 

		CASE 
									WHEN TCD.ContractStatus = 1 AND  TCD.ContractBehaviour = 1  AND ((TCD.LeadDelivered *100 / TCD.LeadSigned )>= 85 ) THEN 1
									WHEN TCD.ContractStatus = 1 AND TCD.ContractBehaviour = 2 AND (DATEDIFF(day,CONVERT(DATE ,GETDATE()),ISNULL(TCD.ContractEndDate,CONVERT(DATE, '2099-01-01'))) BETWEEN  0 AND 7)	THEN 1
									ELSE 0
									END IsAboutToExpire,

		CASE
									WHEN (DATEDIFF(day,TD.DealerJoiningDate,CONVERT(DATE ,GETDATE())) BETWEEN  0 AND 30) AND  (DATEDIFF(day,ISNULL(TD.DealerExpiryDate,CONVERT(DATE, '2099-01-01')),CONVERT(DATE ,GETDATE())) < 0)   THEN 1
									ELSE 0
									END IsNewDealer,
								
		TTCBP.TotalBookedCar AS carsBooked ,
		TTCBP.Price,
		COUNT(DISTINCT TDLW.TC_TDCalendarId ) TDCount,
		TLMD.LastMeetingDate,TD.IsLegal,TD.LegalName,TD.TanNumber,TD.PanNumber,TD.ApplicationId   --LegalDetails for Dealer -- Komal Manjare on 10-11-2016
		FROM @TempDealers TD
		LEFT JOIN @TempContractData TCD ON TD.DealerId = TCD.DealerId
		LEFT JOIN @TempTDLastWeek TDLW ON TDLW.DealerId = TD.DealerId
		LEFT JOIN @TempTotalCarBookedPrice TTCBP ON TTCBP.DealerId = TD.DealerId
		LEFT JOIN @TempLastMeetingDate TLMD ON TLMD.DealerId = TD.DealerId
		GROUP BY TD.DealerId,TD.DealerName,TD.DealerType,TD.DealerJoiningDate,TD.DealerExpiryDate,
		TCD.ContractStartDate,TCD.ContractEndDate,TCD.LeadSigned,TCD.LeadDelivered,TCD.ContractBehaviour,TCD.ContractType,TCD.ContractStatus,
		TTCBP.TotalBookedCar,
		TTCBP.Price,
		TLMD.LastMeetingDate,TD.IsLegal,TD.LegalName,TD.TanNumber,TD.PanNumber,TD.ApplicationId
		ORDER BY DealerName ASC
		
	END
 
END

