IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DB_SetRenewalsData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DB_SetRenewalsData]
GO

	
-- =============================================
-- Author:		Kartik Rathod
-- Create date: 28 Feb 2016
-- Description:	Renewals Sp for NCD AND UCD 
-- Modifier :   KARTIK RATHOD on 10 Mar 2016 to add condition for seller leads package (39)
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DB_SetRenewalsData]
	
AS
BEGIN
		DECLARE @NCProducts VARCHAR(150)= '59,70',@UCProducts VARCHAR(150)= '34,30,31,32,33,81,47,77,39'
		DECLARE @mydate DATETIME,@lastDay DATETIME,@firstDay DATETIME
		
		SET @mydate		= CONVERT (DATE, GETDATE()) 
		SET @firstDay	= DATEADD(dd,-(DAY(@mydate)-1),@mydate+'00:00:00.000')									--first day of month
		SET @lastDay	= DATEADD(dd,-(DAY(DATEADD(mm,1,@mydate))),DATEADD(mm,1,Convert(datetime, CONVERT (date,@mydate) )+0.999999))		--last day of month
		

		/* #TempRenewals table will store all the data for current month from DCRM_DB_Renewals Table  */
		
		SELECT	DealerId , EntryDate , ContractId , BusinessUnitId , PkgExpiryDate
		INTO	#TempRenewals 
		FROM	DCRM_DB_Renewals WITH(NOLOCK)
		WHERE	
			EntryDate BETWEEN @firstDay AND @lastDay

		--SELECT * FROM #TempRenewals

		/* #TEMPUCD table stores the all ucd active packages from RVN and ConsumerCreditPoints tables*/
		
		SELECT UCDTbl.*
		INTO  #TEMPUCD
		FROM (
				(
                SELECT        CCP.ConsumerId AS DealerId,                                --here consumerid consider as DealerID
                                CCP.Id AS ContractId,                                    --here ConsumerCreditPointsID consider as ContractId
                                CCP.ExpiryDate AS EndDate,
                                AUD.UserId
                    FROM        ConsumerCreditPoints CCP  WITH(NOLOCK)
                    INNER JOIN    ConsumerPackageRequests CPR WITH(NOLOCK) ON CCP.ConsumerId = CPR.ConsumerId AND CCP.ConsumerType = 1    --ConsumerType = 1 for Dealers
                    LEFT JOIN    DCRM_ADM_UserDealers AUD WITH(NOLOCK) ON AUD.DealerID = CCP.ConsumerId AND AUD.RoleId = 3                --RoleId=3 Sales Field
                        WHERE    --RVN.DealerId IS NULL AND 
                                CCP.ExpiryDate > @firstDay	--AND @lastDay 
                                AND CCP.CustomerPackageId IN (SELECT ListMember FROM fnSplitCSV(@UCProducts))            --UCD Packages to be considered
                )
                UNION
                (
                    SELECT    RVN.DealerId as DealerId,
                            RVN.DealerPackageFeatureID AS ContractId,
                            RVN.PackageEndDate AS EndDate,
                            AUD.UserId
                    FROM RVN_DealerPackageFeatures RVN WITH(NOLOCK)
                    LEFT JOIN     DCRM_ADM_UserDealers AUD WITH(NOLOCK) ON AUD.DealerID = RVN.DealerID AND AUD.RoleId = 3    
                        WHERE    --CCP.ConsumerId IS NULL AND 
                                RVN.PackageEndDate  > @firstDay --AND @lastDay
                                 AND RVN.PackageId = 39 AND RVN.PackageStatus = 2 AND ISNULL(RVN.CampaignType,3) = 3 --for paid only campaignType = 3
                                                                                                                    --packageid =39 for seller leads            --packageStatus = 3 Running
                        
                )
			)AS UCDTbl
		
		--SELECT * FROM #TEMPUCD

		/* 
			#TempTable table will store all the contracts with enddate fall between current month for UCD (ie.GroupType = 1) and NCD (ie.GroupType = 1) dealer 
		*/
		
		SELECT	AllEndingContracts.* 
		INTO	#TempEndingContract
		FROM 
		(
			(													--in case of NCD Dealer
			SELECT DISTINCT		CCM.DealerId,
								CCM.ContractId,
								CCM.CampaignId,
								CCM.EndDate,
								ROW_NUMBER() OVER(PARTITION BY CCM.DealerId ORDER BY CCM.EndDate DESC) AS RowNum,
								2 AS GroupType,																--GroupType = 2 for NCD
								AUD.UserId
			FROM		TC_ContractCampaignMapping CCM WITH(NOLOCK)
			INNER JOIN	RVN_DealerPackageFeatures RVN WITH(NOLOCK) ON RVN.DealerPackageFeatureID = CCM.ContractId
			--INNER JOIN	DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.TransactionId = RVN.TransactionId
			INNER JOIN	Packages(NOLOCK) P ON P.Id=RVN.PackageId 
			LEFT JOIN	DCRM_ADM_UserDealers AUD WITH(NOLOCK) ON AUD.DealerID =  CCM.DealerId AND AUD.RoleId = 3	--RoleId=3 Sales Field
				WHERE 
						CCM.EndDate BETWEEN @firstDay AND @lastDay AND RVN.PackageId IN (SELECT ListMember FROM fnSplitCSV(@NCProducts)) AND CCM.ContractStatus = 1 AND P.isActive=1
			)
		UNION
			(																	--in case of UCD Dealer
			SELECT DISTINCT TMP.DealerId,
							TMP.ContractId,
							NULL AS CampaignId,		
							TMP.EndDate AS EndDate,
							ROW_NUMBER() OVER(PARTITION BY TMP.DealerId ORDER BY TMP.EndDate DESC) AS RowNum,
							1 AS GroupType, 
							TMP.UserId
			FROM #TEMPUCD TMP WITH(NOLOCK)
			)
		) AS AllEndingContracts 
		--ORDER BY DealerId

		--SELECT * FROM #TempEndingContract 
		
		/* 
			#TempDump Table will store the only that dealer who does not have any Active contract  
			Grouptype will differntiate between UCD and NCD dealer 1 for UCD and 2 for NCD
		*/

		SELECT	EndingContracts.* 
		INTO	#TempDump
		FROM 
		(
			(													--in case of NCD Dealer
			SELECT DISTINCT TEMP.DealerId,
							TEMP.ContractId,
							TEMP.CampaignId,
							TEMP.EndDate,
							TEMP.GroupType,
							TEMP.UserId,
							0 AS IsRenewed,
							GETDATE() AS UpdatedOn	--,ROW_NUMBER() OVER(PARTITION BY TEMP.DealerId ORDER BY TEMP.EndDate DESC) AS Row
			FROM		#TempEndingContract TEMP WITH(NOLOCK)
			LEFT JOIN	(SELECT CCM.DealerId 
							FRom TC_ContractCampaignMapping CCM WITH(NOLOCK) 
							JOIN RVN_DealerPackageFeatures RVN WITH(NOLOCK) ON RVN.DealerPackageFeatureID = CCM.ContractId	AND RVN.PackageId IN (SELECT ListMember FROM fnSplitCSV(@NCProducts)) 
														AND CCM.ContractStatus = 1  AND (CCM.EndDate IS NULL OR CCM.EndDate > @lastDay ) --here for lead base contract enddate is null,so we will make our condition true for 
																																		-- lead base contract 
							JOIN	Packages(NOLOCK) P ON P.Id=RVN.PackageId  AND P.isActive=1) AS CAP ON CAP.DealerId =TEMP.DealerId		--CAP -Current active packages
				WHERE																												
					CAP.DealerId IS NULL  AND TEMP.GroupType = 2 AND  TEMP.RowNum = 1		--NCD PACKAGE considered
			)
		UNION 
			(													--in case of UCD Dealer
			SELECT DISTINCT TEMP.DealerId,
							TEMP.ContractId,
							TEMP.CampaignId,
							TEMP.EndDate,
							TEMP.GroupType,
							TEMP.UserId,
							0 AS IsRenewed,
							GETDATE() AS UpdatedOn
			FROM	#TempEndingContract TEMP WITH(NOLOCK)
			WHERE	TEMP.RowNum = 1 AND TEMP.GroupType = 1 AND TEMP.EndDate BETWEEN @firstDay AND @lastDay 			--GroupType = 1 for UCD
			)
		) AS EndingContracts 
		--ORDER BY DealerId

		--SELECT * FROM #TempDump 

		/*
			this IF section will dumb all the dealers who does not have active contract in DCRM_DB_Renewals Table
			AND INSERT the new dealer into the table if that data not present in DCRM_DB_Renewals table
		*/

		IF EXISTS (SELECT DealerId FROM #TempDump WITH(NOLOCK))
		BEGIN
			IF EXISTS (SELECT DealerId FROM #TempRenewals WITH(NOLOCK))
				BEGIN 
						/* Update DCRM_DB_Renewals TAble only if any contract is expired */
				
						UPDATE	DDR																		
						SET		DDR.DealerID = TD.DealerId,
								DDR.ContractId = TD.ContractId ,
								DDR.CampaignId = TD.CampaignId ,
								DDR.PkgExpiryDate = TD.EndDate,
								DDR.BusinessUnitId = TD.GroupType,
								DDR.UserId = TD.UserId,
								DDR.IsRenewed = 0,
								DDR.UpdatedOn=GETDATE()
						
						FROM	DCRM_DB_Renewals DDR WITH(NOLOCK)
						JOIN	#TempDump TD WITH(NOLOCK)	ON DDR.DealerId = TD.DealerId
						JOIN	#TempRenewals TR WITH(NOLOCK) ON TD.DealerId = TR.DealerId
							WHERE 
								DDR.DealerId = TD.DealerId AND DDR.BusinessUnitId = TD.GroupType AND DDR.EntryDate BETWEEN @firstDay AND @lastDay

					/* insert new entry of dealer if there is new contract is expired and not having entry in the DCRM_DB_Renewals table */

						INSERT INTO DCRM_DB_Renewals 
										(DealerId,ContractId,CampaignId,PkgExpiryDate,BusinessUnitId,UserId,IsRenewed,UpdatedOn)
						SELECT TD.DealerId,TD.ContractId,TD.CampaignId,Td.EndDate,TD.GroupType,TD.UserId,TD.IsRenewed,TD.UpdatedOn  
						FROM #TempDump TD WITH(NOLOCK)
						LEFT JOIN #TempRenewals TR WITH(NOLOCK) ON TD.DealerId = TR.DealerId  AND TR.BusinessUnitId = TD.GroupType
						WHERE 
							TR.DealerId IS NULL 
				END
			ELSE 
				BEGIN
					INSERT INTO DCRM_DB_Renewals 
								(DealerId,ContractId,CampaignId,PkgExpiryDate,BusinessUnitId,UserId,IsRenewed,UpdatedOn)
					SELECT TD.DealerId,TD.ContractId,TD.CampaignId,Td.EndDate,TD.GroupType,TD.UserId,TD.IsRenewed,TD.UpdatedOn 
					FROM	#TempDump TD WITH(NOLOCK)
				END
		END

		/*
			following IF loop find out if there is any active contract is started for only those dealers that already present in the DCRM_DB_Renewals table
			And if YES then update the table DCRM_DB_Renewals with new details of Contract
		*/
		
		IF EXISTS (SELECT TOP 1 DealerId FROM #TempRenewals WITH(NOLOCK))
		BEGIN	
				/* 
					only for NCD dealer
					this section will check if any existing dealer in the table DCRM_DB_Renewals is having contractstatus = 1 
					with Endate is null in case of	leadbase contract and in case of duration not having enddate in current month	
				*/

				UPDATE DDR
				SET		DDR.ContractId=CCM.ContractId ,
						DDR.CampaignId = CCM.CampaignId ,
						DDR.PkgExpiryDate = CCM.EndDate,
						DDR.BusinessUnitId = 2,									--BusinessUnitId = 2 for NCD
						DDR.UserId=AUD.UserId,
						DDR.IsRenewed = 1,
						DDR.UpdatedOn=GETDATE()
				 
				FROM		DCRM_DB_Renewals DDR WITH(NOLOCK)
				INNER JOIN	#TempRenewals TR WITH(NOLOCK) ON DDR.DealerId = TR.DealerId
				INNER JOIN	TC_ContractCampaignMapping CCM WITH(NOLOCK) ON TR.DealerId = CCM.DealerId
				INNER JOIN	RVN_DealerPackageFeatures RVN WITH(NOLOCK) ON RVN.DealerPackageFeatureID = CCM.ContractId
				--INNER JOIN	DCRM_SalesDealer DSD WITH(NOLOCK) ON DSD.TransactionId = RVN.TransactionId
				INNER JOIN	Packages(NOLOCK) P ON P.Id=RVN.PackageId
				LEFT JOIN	DCRM_ADM_UserDealers AUD WITH(NOLOCK) ON AUD.DealerID =  CCM.DealerId AND AUD.RoleId = 3
						WHERE	
						DDR.DealerId = CCM.DealerId AND DDR.EntryDate=TR.EntryDate AND DDR.BusinessUnitId =2 AND CCM.ContractStatus = 1 AND (CCM.EndDate IS NULL OR CCM.EndDate > @lastDay )
						AND RVN.PackageId IN (SELECT ListMember FROM fnSplitCSV(@NCProducts)) AND TR.BusinessUnitId = 2 AND P.isActive=1				--here for lead base contract enddate is null,so we will make our condition true for 
																											-- lead base contract 
		
				/* 
					Only for uCD dealer
					#TempUCDTable table will containt all the data for the UCD dealer to find out if there is any existing dealer in table DCRM_DB_Renewals
					which have current active contract 
					BusinessUnit = 1 for UCD and Busineesunit = 2 is for NCD dealer
				
				*/
				SELECT  UCDRenewTbl.*
				INTO	#TempUCDRenew
				FROM
				(
					(
					SELECT	CCP.ConsumerId AS DealerId, 
							CCP.Id AS ContractId,
							CCP.ExpiryDate AS EndDate,
							--ROW_NUMBER() OVER(PARTITION BY CPR.ConsumerId ORDER BY CPR.EntryDate DESC) AS RowNum, 
							1 AS GroupType,
							AUD.UserId,
							TR.EntryDate
				
				
					FROM		ConsumerCreditPoints CCP WITH(NOLOCK)
					INNER JOIN	ConsumerPackageRequests CPR WITH(NOLOCK) ON CCP.ConsumerId = CPR.ConsumerId AND CCP.ConsumerType = 1
					INNER JOIN	#TempRenewals TR WITH(NOLOCK)  ON CCP.ConsumerId = TR.DealerId
					LEFT JOIN	DCRM_ADM_UserDealers AUD WITH(NOLOCK) ON AUD.DealerID = CCP.ConsumerId AND AUD.RoleId = 3
						WHERE	
						CCP.ExpiryDate > TR.PkgExpiryDate AND CCP.ExpiryDate > @lastDay AND CCP.CustomerPackageId IN (SELECT ListMember FROM fnSplitCSV(@UCProducts))
						AND TR.BusinessUnitId = 1																							--BusinessUnitId = 1 for UCD
					)
					UNION
					(
						SELECT	RVN.DealerId as DealerId,
								RVN.DealerPackageFeatureID AS ContractId,
								RVN.PackageEndDate AS EndDate,
								1 AS GroupType,
								AUD.UserId,
								TR.EntryDate
						FROM RVN_DealerPackageFeatures RVN WITH(NOLOCK)
						INNER JOIN	#TempRenewals TR WITH(NOLOCK)  ON RVN.DealerId = TR.DealerId
						LEFT JOIN 	DCRM_ADM_UserDealers AUD WITH(NOLOCK) ON AUD.DealerID = RVN.DealerID AND AUD.RoleId = 3	
							WHERE  RVN.PackageId = 39 AND RVN.PackageStatus = 2 AND ISNULL(RVN.CampaignType,3) = 3 --for paid only campaignType = 3
									AND RVN.PackageEndDate > TR.PkgExpiryDate  AND		RVN.PackageEndDate > 	@lastDay	AND TR.BusinessUnitId = 1																										--packageStatus = 3 Running
					)
				) AS UCDRenewTbl


				SELECT	TMP.DealerId,
						TMP.ContractId,
						TMP.EndDate,
						ROW_NUMBER() OVER(PARTITION BY TMP.DealerId ORDER BY TMP.EndDate DESC) AS RowNum,
						TMP.GroupType,
						TMP.UserId,
						TMP.EntryDate
				INTO #TempUCDTable
				FROM #TempUCDRenew TMP WITH(NOLOCK)


				--SELECT * FROM #TempUCDTable WHERE RowNum = 1
				
				UPDATE	DDR
				SET		DDR.ContractId=TEMP.ContractId ,
						DDR.PkgExpiryDate = TEMP.EndDate,
						DDR.BusinessUnitId = 1,
						DDR.UserId=TEMP.UserId,
						DDR.IsRenewed = 1,
						DDR.UpdatedOn=GETDATE()
				
				FROM DCRM_DB_Renewals DDR WITH(NOLOCK)
				INNER JOIN #TempUCDTable TEMP WITH(NOLOCK) ON TEMP.DealerId = DDR.DealerId
					WHERE 
						DDR.DealerId = TEMP.DealerId AND DDR.BusinessUnitId = 1 AND  DDR.EntryDate=TEMP.EntryDate AND TEMP.RowNum = 1
		
			DROP TABLE #TempUCDRenew
			DROP TABLE #TempUCDTable
		END

		DROP TABLE #TempEndingContract
		DROP TABLE #TempDump
		DROP TABLE #TempRenewals
		DROP TABLE #TEMPUCD
END
