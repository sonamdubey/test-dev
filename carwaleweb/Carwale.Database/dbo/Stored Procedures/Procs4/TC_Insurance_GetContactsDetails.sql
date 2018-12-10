IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetContactsDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetContactsDetails]
GO

	-- =============================================
-- Author:		Sunil M. Yadav
-- Create date: 09th September 2016
-- Description:	Get details of insurance leads.
-- EXEC TC_Insurance_GetContactsDetails 20553,NULL,NULL,NULL,NULL,NULL,NULL
-- Modified By: Ashwini Dhamankar on Sep 13,2016 (Fetched LastDIV,LastNCB,LastDiscount,LastPremium) 
-- Modified By: Nilima More On 13th sept,2016 added @InquiryId null condition AND IsZeroDep
-- exec [TC_Insurance_GetContactsDetails] 20553,NULL,NULL,NULL,NULL,NULL,163
-- =============================================
CREATE PROCEDURE [dbo].[TC_Insurance_GetContactsDetails] @DealerId INT
	,@CustomerName VARCHAR(50) = NULL
	,@MobileNumber VARCHAR(50) = NULL
	,@CarRegistrationNumber VARCHAR(50) = NULL
	,@InsuranceServiceProvider VARCHAR(50) = NULL
	,@ExpiryToDate DATETIME = NULL
	,@InquiryId INT = NULL
	,@OrderBy VARCHAR(20) = 'ExpiryDate'
	,@OrderDirection VARCHAR(5) = NULL
AS
BEGIN
	DECLARE @CurrentDate DATETIME = GETDATE();

	SET @CarRegistrationNumber = REPLACE(LOWER(@CarRegistrationNumber), ' ', '');

	SELECT TIR.TC_Insurance_ReminderId AS ReminderId
		,TIR.InsuranceProvider
		,TIR.CustomerName
		,TIR.MobileNumber
		,CVW.Car AS CarOwned
		,TIR.RegistrationNumber AS RegNum
		,TIR.PolicyNumber
		,TIR.ExpiryDate
		,TIR.VersionId
		,TIR.PolicyPeriodFrom
		,TIR.ChassisNumber
		,TII.IDV
		,TII.NCB
		,TII.Discount
		,TII.Premium
		,TIR.LastIDV
		,TIR.LastNCB
		,TIR.LastDiscount
		,TIR.LastPremium
		,TIH.NAME AS HypothecationName
		,TIH.Id AS HypothecationId
		,TIR.IsClaimsExist
		,TIR.RegisteredAddress
		,TIR.CustomerId
		,TII.TC_InquiriesLeadId AS TCInquiriesLeadId
		,TII.IsZeroDep AS IsZeroDep
		,TIR.RegistrationNumberSearch
		,RowNumber = ROW_NUMBER() OVER (
			ORDER BY CASE 
					WHEN @OrderDirection <> 'ASC'
						THEN ''
					WHEN @OrderBy = 'CustomerName '
						THEN TIR.CustomerName
					END ASC
				,CASE 
					WHEN @OrderDirection <> 'ASC'
						THEN ''
					WHEN @OrderBy = 'CarOwned'
						THEN CVW.Car
					END ASC
				,CASE 
					WHEN @OrderDirection <> 'ASC'
						THEN cast(NULL AS DATE)
					WHEN @OrderBy = 'ExpiryDate'
						THEN TIR.ExpiryDate
					END ASC
				,CASE 
					WHEN @OrderDirection <> 'DESC'
						THEN ''
					WHEN @OrderBy = 'CustomerName'
						THEN TIR.CustomerName
					END DESC
				,CASE 
					WHEN @OrderDirection <> 'DESC'
						THEN ''
					WHEN @OrderBy = 'CarOwned'
						THEN CVW.Car
					END DESC
				,CASE 
					WHEN @OrderDirection <> 'DESC'
						THEN cast(NULL AS DATE)
					WHEN @OrderBy = 'ExpiryDate'
						THEN TIR.ExpiryDate
					END DESC
				,CASE 
					WHEN @OrderDirection <> 'ASC'
						THEN ''
					WHEN @OrderBy = 'InsuranceProvider '
						THEN TIR.InsuranceProvider
					END ASC
				,CASE 
					WHEN @OrderDirection <> 'DESC'
						THEN ''
					WHEN @OrderBy = 'InsuranceProvider'
						THEN TIR.InsuranceProvider
					END DESC
			)
	FROM TC_Insurance_Reminder TIR WITH (NOLOCK)
	JOIN vwMMV CVW WITH (NOLOCK) ON CVW.VersionId = TIR.VersionId
	LEFT JOIN TC_Insurance_Inquiries TII WITH (NOLOCK) ON TII.TC_Insurance_InquiriesId = TIR.MappingInqId
	LEFT JOIN TC_Insurance_Hypothecation TIH WITH (NOLOCK) ON TIH.Id = TIR.HypothecationId
	WHERE (
			(
				@InquiryId IS NULL
				AND TIR.MappingInqId = - 1
				)
			OR TIR.MappingInqId = @InquiryId
			)
		AND TIR.BranchId = @DealerId
		AND (
			@CustomerName IS NULL
			OR TIR.CustomerName LIKE '%' + @CustomerName + '%'
			)
		AND (
			@MobileNumber IS NULL
			OR TIR.MobileNumber LIKE '%' + @MobileNumber + '%'
			)
		AND (
			@CarRegistrationNumber IS NULL
			OR TIR.RegistrationNumberSearch LIKE '%' + @CarRegistrationNumber + '%'
			)
		AND (
			@InsuranceServiceProvider IS NULL
			OR TIR.InsuranceProvider LIKE '%' + @InsuranceServiceProvider + '%'
			)
		AND (
			@ExpiryToDate IS NULL
			OR @ExpiryToDate IS NOT NULL
			AND TIR.ExpiryDate <= @ExpiryToDate
			)
	ORDER BY CASE 
			WHEN @OrderDirection <> 'ASC'
				THEN ''
			WHEN @OrderBy = 'CustomerName '
				THEN TIR.CustomerName
			END ASC
		,CASE 
			WHEN @OrderDirection <> 'ASC'
				THEN ''
			WHEN @OrderBy = 'CarOwned'
				THEN CVW.Car
			END ASC
		,CASE 
			WHEN @OrderDirection <> 'ASC'
				THEN cast(NULL AS DATE)
			WHEN @OrderBy = 'ExpiryDate'
				THEN TIR.ExpiryDate
			END ASC
		,CASE 
			WHEN @OrderDirection <> 'DESC'
				THEN ''
			WHEN @OrderBy = 'CustomerName'
				THEN TIR.CustomerName
			END DESC
		,CASE 
			WHEN @OrderDirection <> 'DESC'
				THEN ''
			WHEN @OrderBy = 'CarOwned'
				THEN CVW.Car
			END DESC
		,CASE 
			WHEN @OrderDirection <> 'DESC'
				THEN cast(NULL AS DATE)
			WHEN @OrderBy = 'ExpiryDate'
				THEN TIR.ExpiryDate
			END DESC
		,CASE 
			WHEN @OrderDirection <> 'ASC'
				THEN ''
			WHEN @OrderBy = 'InsuranceProvider '
				THEN TIR.InsuranceProvider
			END ASC
		,CASE 
			WHEN @OrderDirection <> 'DESC'
				THEN ''
			WHEN @OrderBy = 'InsuranceProvider'
				THEN TIR.InsuranceProvider
			END DESC
END
	----------------------------------------------------------------------
