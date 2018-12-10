IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Insurance_GetContactsDetails_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Insurance_GetContactsDetails_V16]
GO

	
-- =============================================
-- Author:		Sunil M. Yadav
-- Create date: 09th September 2016
-- Description:	Get details of insurance leads.
-- EXEC TC_Insurance_GetContactsDetails 20553,NULL,NULL,NULL,NULL,NULL,NULL
-- Modified By: Ashwini Dhamankar on Sep 13,2016 (Fetched LastDIV,LastNCB,LastDiscount,LastPremium) 
-- Modified By: Nilima More On 13th sept,2016 added @InquiryId null condition AND IsZeroDep
-- Modified By : Nilima More On 23rd Sept added IsAllContacts flag to fetch all data on contact list page
-- exec [TC_Insurance_GetContactsDetails_V16.9.1] 20553,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,1
-- exec [TC_Insurance_GetContactsDetails] 20553,NULL,NULL,NULL,NULL,NULL,NULL,NULL
-- Modified By : Vaibhav K 24 Oct 2016 added new parameter @AssignedTo and condition in TC_Insurance_Reminder
-- Also fetched AssignedTo and MappingInqId from TC_Insurance_Reminder
-- =============================================
create PROCEDURE [dbo].[TC_Insurance_GetContactsDetails_V16.10.1] @DealerId INT
	,@CustomerName VARCHAR(50) = NULL
	,@MobileNumber VARCHAR(50) = NULL
	,@CarRegistrationNumber VARCHAR(50) = NULL
	,@InsuranceServiceProvider VARCHAR(50) = NULL
	,@ExpiryToDate DATETIME = NULL
	,@InquiryId INT = NULL
	,@OrderBy VARCHAR(20) = 'ExpiryDate'
	,@OrderDirection VARCHAR(5) = NULL
	,@IsAllContacts BIT=0
	,@AssignedTo INT = 0
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
		,TIH.Name AS HypothecationName
		,TIH.Id AS HypothecationId
		,TIR.IsClaimsExist
		,TIR.RegisteredAddress
		,TIR.CustomerId
		,TII.TC_InquiriesLeadId AS TCInquiriesLeadId
		,TII.IsZeroDep AS IsZeroDep
		,TIR.RegistrationNumberSearch
		,TIR.AssignedTo
		,ISNULL(TIR.MappingInqId, -1) MappingInqId
	    ,RowNumber=ROW_NUMBER() OVER 
		( ORDER BY case
			when @OrderDirection <> 'ASC' then ''
			when @OrderBy = 'CustomerName ' then TIR.CustomerName 
			end ASC
           ,case
			when @OrderDirection <> 'ASC' then ''
			when @OrderBy = 'CarOwned' then CVW.Car
			end ASC
	       ,case
			when @OrderDirection <> 'ASC' then cast(null as date)
			when @OrderBy = 'ExpiryDate' then TIR.ExpiryDate
			end ASC
	       ,case
			when @OrderDirection <> 'DESC' then ''
			when @OrderBy = 'CustomerName' then TIR.CustomerName 
			end DESC
	       ,case
			when @OrderDirection <> 'DESC' then ''
			when @OrderBy = 'CarOwned' then CVW.Car
			end DESC
	       ,case
			when @OrderDirection <> 'DESC' then cast(null as date)
			when @OrderBy = 'ExpiryDate' then TIR.ExpiryDate
			end DESC
			,case when @OrderDirection <> 'ASC' then ''
			when @OrderBy = 'InsuranceProvider ' then TIR.InsuranceProvider 
			end ASC
			,case when @OrderDirection <> 'DESC' then ''
			when @OrderBy = 'InsuranceProvider' then TIR.InsuranceProvider 
			end DESC
		)
	FROM TC_Insurance_Reminder TIR WITH (NOLOCK)
	JOIN vwMMV CVW WITH (NOLOCK) ON CVW.VersionId = TIR.VersionId
	LEFT JOIN TC_Insurance_Inquiries TII WITH (NOLOCK) ON TII.TC_Insurance_InquiriesId = TIR.MappingInqId
	LEFT JOIN TC_Insurance_Hypothecation TIH WITH (NOLOCK) ON TIH.Id = TIR.HypothecationId
	WHERE 
		((
		(@InquiryId IS NULL AND (TIR.MappingInqId = -1 OR @IsAllContacts = 1)) --INCOMPLETE INQUIRIES ,CONTACT LIST
		
		OR TIR.MappingInqId = @InquiryId))AND TIR.BranchId = @DealerId	
		--((@InquiryId IS NULL ) OR (TIR.MappingInqId = @InquiryId) AND TIR.BranchId = @DealerId	
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
			 OR @ExpiryToDate IS NOT NULL AND TIR.ExpiryDate <=  @ExpiryToDate 
			)
		AND (	--Vaibhav K 24 Oct 2016
			ISNULL(@AssignedTo, 0) = 0
			OR TIR.AssignedTo = @AssignedTo
			)
		ORDER BY case
			when @OrderDirection <> 'ASC' then ''
			when @OrderBy = 'CustomerName ' then TIR.CustomerName 
			end ASC
           ,case
			when @OrderDirection <> 'ASC' then ''
			when @OrderBy = 'CarOwned' then CVW.Car
			end ASC
	       ,case
			when @OrderDirection <> 'ASC' then cast(null as date)
			when @OrderBy = 'ExpiryDate' then TIR.ExpiryDate
			end ASC
	       ,case
			when @OrderDirection <> 'DESC' then ''
			when @OrderBy = 'CustomerName' then TIR.CustomerName 
			end DESC
	       ,case
			when @OrderDirection <> 'DESC' then ''
			when @OrderBy = 'CarOwned' then CVW.Car
			end DESC
	       ,case
			when @OrderDirection <> 'DESC' then cast(null as date)
			when @OrderBy = 'ExpiryDate' then TIR.ExpiryDate
			end DESC
			,case when @OrderDirection <> 'ASC' then ''
			when @OrderBy = 'InsuranceProvider ' then TIR.InsuranceProvider 
			end ASC
			,case when @OrderDirection <> 'DESC' then ''
			when @OrderBy = 'InsuranceProvider' then TIR.InsuranceProvider 
			end DESC
END
----------------------------------------------------------------------
