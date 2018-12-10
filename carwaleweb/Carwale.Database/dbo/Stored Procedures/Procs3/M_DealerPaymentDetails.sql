IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[M_DealerPaymentDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[M_DealerPaymentDetails]
GO

	-- =============================================
-- Author	:	Sachin Bharti(10th June 2015)
-- Description	:	Get payment details Dealer wise
-- Modifier	:	Sachin Bharti(29th June 2015)
-- Purpose	:	Added query for Total,Active,Paid and Live Listing Dealers
-- @MetricType : 1 - Dealer Details having warranty Approved 
--				 2 - Dealer's have warranty sold but not approved yet
--				 3 - Dealer details with whome warranty is under discussion
--				 4 - Total Dealer details assigned to user
--				 5 - Active Dealer details assigned to user
--				 6 - Paid Dealer details assigned to user
--				 7 - Live Listing Dealer details assigned to user
-- Execute [dbo].[M_DealerPaymentDetails] null,4,88,0
-- =============================================
CREATE PROCEDURE [dbo].[M_DealerPaymentDetails]
	
	@IsMtd TINYINT = NULL,
	@MetricType INT,
	@OprUserId	INT,
	@ForSelf	SMALLINT = NULL

AS
BEGIN
	
	--declare table type to store reporting users
	DECLARE @TempTable AS TABLE (UserId INT)

	IF @ForSelf = 0 
		BEGIN
			--now insert reporting person also in the temp table
			INSERT INTO @TempTable (UserId) SELECT UsersId from [dbo].[Fn_DCRM_GetChildUsersIncludingParent](@OprUserId)
		END
	ELSE IF @ForSelf = 1
		BEGIN
			--now insert reporting person also in the temp table
			INSERT INTO @TempTable (UserId) VALUES(@OprUserId)	
		END

	--Warranty Approved Dealer Details
	IF @MetricType = 1
		BEGIN
			SELECT 
				D.Organization,D.ID AS DealerId,
				SUM(DPD.PaymentReceived) AS Amount,
				SUM(DSD.Quantity) AS PackageQuantity,
				CONVERT(VARCHAR(20),DPD.ReceivedDate,106) AS UpdatedOn
			FROM 
				DCRM_SalesDealer DSD(NOLOCK)
				INNER JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId AND DSD.LeadStatus = 2  AND DSD.CampaignType = 3--closed products and paid campaigns only
				INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DPD.TransactionId = DSD.TransactionId AND IsApproved = 1 --approved payments
				INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct AND PK.InqPtCategoryId = 37 -- for warranties only
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DSD.UpdatedBy AND MU.IsActive =1
			WHERE
				DSD.UpdatedBy IN (SELECT UserId FROM @TempTable)
				AND(@IsMtd IS NULL OR MONTH(DPD.ReceivedDate) = MONTH(GETDATE()))
				AND YEAR(DPD.ReceivedDate) = YEAR(GETDATE())
			GROUP BY
				D.Organization,D.ID,
				DSD.Quantity,
				CONVERT(VARCHAR(20),DPD.ReceivedDate,106)
			ORDER BY 
				CAST(CONVERT(VARCHAR(20),DPD.ReceivedDate,106) AS DATE) DESC
		END

	--Dealer's have warranty sold but not approved yet
	IF @MetricType = 2
		BEGIN
			SELECT 
				D.Organization,D.ID AS DealerId,
				SUM(DPD.PaymentReceived) AS Amount,
				DSD.Quantity AS PackageQuantity,
				CONVERT(VARCHAR(20),DPD.AddedOn,106) AS UpdatedOn
			FROM 
				DCRM_SalesDealer DSD(NOLOCK)
				INNER JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId AND DSD.LeadStatus = 2 AND DSD.CampaignType = 3--closed products and paid campaigns only
				INNER JOIN DCRM_PaymentDetails DPD(NOLOCK) ON DSD.TransactionId = DPD.TransactionId AND IsApproved IS NULL --converted payments
				INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct AND PK.InqPtCategoryId = 37 -- for warranties only
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DSD.UpdatedBy AND MU.IsActive =1
			WHERE
				DSD.UpdatedBy IN (SELECT UserId FROM @TempTable)
				AND(@IsMtd IS NULL OR MONTH(DPD.AddedOn) = MONTH(GETDATE()))
				AND YEAR(DPD.AddedOn) = YEAR(GETDATE())
			GROUP BY
				D.Organization,D.ID,
				DSD.Quantity,
				CONVERT(VARCHAR(20),DPD.AddedOn,106)
			ORDER BY
				CAST(CONVERT(VARCHAR(20),DPD.AddedOn,106) AS DateTime) DESC
		ENd

	--Dealer details with whome warranty is under discussion
	IF @MetricType = 3
		BEGIN
			SELECT 
				D.Organization,D.ID AS DealerId,
				DSD.Quantity AS PackageQuantity,
				DSD.ClosingAmount AS Amount,
				CONVERT(VARCHAR(20),CAST(DSD.ClosingDate AS datetime),106) AS UpdatedOn
			FROM 
				DCRM_SalesDealer DSD(NOLOCK) 
				INNER JOIN Packages PK(NOLOCK) ON PK.Id = DSD.PitchingProduct AND PK.InqPtCategoryId = 37 -- for warranties only
				INNER JOIN Dealers D(NOLOCK) ON D.ID = DSD.DealerId AND DSD.LeadStatus = 1 -- in open stage
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DSD.UpdatedBy AND MU.IsActive =1
			WHERE
				DSD.UpdatedBy IN (SELECT UserId FROM @TempTable)
				AND YEAR(DSD.CreatedOn) = YEAR(GETDATE())
			ORDER BY
				CAST(DSD.ClosingDate AS datetime) DESC
		END

	--Total Dealer details assigned to user
	IF @MetricType = 4
		BEGIN
			SELECT 
				D.Organization,
				D.ID AS DealerId,
				D.MobileNo ,
				D.ContactPerson,
				C.Name AS City,
				A.Name AS Area
			FROM 
				Dealers D(NOLOCK) 
				INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.DealerId = D.Id
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DAU.UserId AND MU.IsActive =1
				INNER JOIN Areas A(NOLOCK) ON A.ID = D.AreaId 
				INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			WHERE
				DAU.UserId IN (SELECT UserId FROM @TempTable)
			ORDER BY
				D.Organization
		END

	--Active Dealer details assigned to user
	IF @MetricType = 5
		BEGIN
			SELECT 
				DISTINCT D.ID AS DealerId,
				D.Organization,
				D.MobileNo ,
				D.ContactPerson,
				C.Name AS City,
				A.Name AS Area
			FROM 
				AbSure_Trans_Credits TC(NOLOCK) 
				INNER JOIN Dealers D(NOLOCK) ON D.ID = TC.DealerId
				INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON TC.DealerId = DAU.DealerId
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DAU.UserId AND MU.IsActive =1
				INNER JOIN Areas A(NOLOCK) ON A.ID = D.AreaId 
				INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			WHERE
				TC.CreditAmount > 0 AND
				DAU.UserId IN (SELECT UserId FROM @TempTable)

			UNION -- removes duplicate rows

			SELECT 
				DISTINCT D.ID AS DealerId,
				D.Organization,
				D.MobileNo ,
				D.ContactPerson,
				C.Name AS City,
				A.Name AS Area
			FROM 
				ConsumerCreditPoints CCP(NOLOCK)
				INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.DealerId = CCP.ConsumerId AND DATEDIFF(DAY,GETDATE(),CCP.ExpiryDate) > 0 --not expired yet
				INNER JOIN SellInquiries SI(NOLOCK) ON SI.DealerId = DAU.DealerId AND DATEDIFF(DAY,SI.LastUpdated,GETDATE()) BETWEEN 0 AND 30 --stock updated in last 30 days
				INNER JOIN Dealers D(NOLOCK) ON D.ID = DAU.DealerId
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DAU.UserId AND MU.IsActive =1
				INNER JOIN Areas A(NOLOCK) ON A.ID = D.AreaId 
				INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			WHERE
				CCP.ConsumerType = 1 AND--For Dealer
				DATEDIFF(DAY,CONVERT(VARCHAR(20),GETDATE(),106),CONVERT(VARCHAR(20),CCP.ExpiryDate,106)) > 0 AND
				DAU.UserId IN (SELECT UserId FROM @TempTable)
		END

	--Paid Dealer details assigned to user
	IF @MetricType = 6
		BEGIN
			SELECT 
				DISTINCT D.ID AS DealerId,
				D.Organization,
				D.MobileNo ,
				D.ContactPerson,
				C.Name AS City,
				A.Name AS Area
			FROM 
				AbSure_Trans_Credits TC(NOLOCK) 
				INNER JOIN Dealers D(NOLOCK) ON D.ID = TC.DealerId
				INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON TC.DealerId = DAU.DealerId
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DAU.UserId AND MU.IsActive = 1
				LEFT JOIN Cities C(NOLOCK) ON C.ID = D.CityId
				LEFT JOIN Areas A(NOLOCK) ON A.ID = D.AreaId 
			WHERE
				TC.CreditAmount > 0 AND
				DAU.UserId IN (SELECT UserId FROM @TempTable)

			UNION -- removes duplicate rows

			SELECT 
				DISTINCT D.ID AS DealerId,
				D.Organization,
				D.MobileNo ,
				D.ContactPerson,
				C.Name AS City,
				A.Name AS Area
			FROM 
				ConsumerCreditPoints CCP(NOLOCK)
				INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.DealerId = CCP.ConsumerId 
															AND DATEDIFF(DAY,GETDATE(),CCP.ExpiryDate) > 0 --not expired yet
															AND CCP.PackageType <> 28 --excluding free listings
				INNER JOIN Dealers D(NOLOCK) ON D.ID = DAU.DealerId
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DAU.UserId AND MU.IsActive = 1
				LEFT JOIN Areas A(NOLOCK) ON A.ID = D.AreaId 
				LEFT JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			WHERE
				CCP.ConsumerType = 1 AND--For Dealer
				DATEDIFF(DAY,CONVERT(VARCHAR(20),GETDATE(),106),CONVERT(VARCHAR(20),CCP.ExpiryDate,106)) > 0 AND
				DAU.UserId IN (SELECT UserId FROM @TempTable)

		END

	--Get all Live Listing dealers
	IF @MetricType = 7
		BEGIN
			SELECT 
				DISTINCT D.ID AS DealerId,
				D.Organization,
				D.MobileNo,
				D.ContactPerson,
				C.Name AS City,
				A.Name AS Area
			FROM
				Livelistings LI(NOLOCK)
				INNER JOIN SellInquiries SI(NOLOCK) ON LI.Inquiryid = SI.ID AND LI.SellerType = 1
				INNER JOIN DCRM_ADM_UserDealers DAU(NOLOCK) ON DAU.DealerId = SI.DealerId
				INNER JOIN Dealers D(NOLOCK) ON D.ID = DAU.DealerId
				INNER JOIN DCRM_ADM_MappedUsers MU(NOLOCK) ON MU.OprUserId = DAU.UserId AND MU.IsActive = 1
				INNER JOIN Areas A(NOLOCK) ON A.ID = D.AreaId 
				INNER JOIN Cities C(NOLOCK) ON C.ID = D.CityId
			WHERE
				DAU.UserId IN (SELECT UserId FROM @TempTable)
		END
END
