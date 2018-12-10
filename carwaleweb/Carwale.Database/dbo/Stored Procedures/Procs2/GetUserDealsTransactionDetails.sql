IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUserDealsTransactionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUserDealsTransactionDetails]
GO

	
-- =============================================
-- Author:		Shalini Nair
-- Create date: 08/01/2015
-- Description:	Get user transaction details based on username,usermobile,useremail and transactionID
-- EXEC GetUserDealsTransactionDetails 'p','','',''
-- =============================================
CREATE PROCEDURE [dbo].[GetUserDealsTransactionDetails]
	-- Add the parameters for the stored procedure here
	@UserName VARCHAR(100)
	,@UserEmail VARCHAR(100)
	,@UserContactNo VARCHAR(50)
	,@TransactionID VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT DI.CustomerName
		,DI.CustomerMobile
		,DI.CustomerEmail
		,CMA.NAME +' '+ CM.NAME + ' '+ CV.NAME AS Car
		,STK.MakeYear AS ManufacturedMonthYear
		,DI.EntryDateTime AS EntryDateTime
		,Status = CASE WHEN PG.TransactionCompleted = 1 AND DI.PushStatus IS NOT NULL
							THEN 'Payment Success' 
						WHEN PG.TransactionCompleted = 0 AND DI.PushStatus IS NULL
							THEN 'Payment Failed'
						WHEN PG.TransactionCompleted IS NULL AND DI.PushStatus IS NOT NULL
							THEN 'Unpaid Inquiry'
						WHEN PG.TransactionCompleted IS NULL AND DI.PushStatus IS NULL
							THEN ''
						END
		,AutobizLinkOfInquiry = CASE WHEN DI.PushStatus IS NOT NULL 
									THEN 'Available'
								ELSE ''
									END
		,D.ID AS DealerId
		,D.CityId 
		,DI.ID AS ReferenceId
		,TCL.TC_InquiriesLeadId AS [LeadId]
		,TCL.TC_CustomerId AS [CustomerId]
		,TCL.TC_UserId AS [UserId]
	FROM DealInquiries DI WITH (NOLOCK)
	JOIN TC_Deals_Stock STK WITH (NOLOCK) ON DI.StockId = STK.Id
	JOIN Dealers D WITH(NOLOCK) ON D.ID = STK.BranchId
	JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = STK.CarVersionId
	JOIN CarModels CM WITH (NOLOCK) ON CM.ID = CV.CarModelId
	JOIN CarMakes CMA WITH (NOLOCK) ON CMA.ID = CM.CarMakeId
	LEFT JOIN TC_NewCarInquiries TC WITH(NOLOCK) ON TC.TC_NewCarInquiriesId = DI.PushStatus
	LEFT JOIN TC_InquiriesLead TCL WITH(NOLOCK) ON TCL.TC_InquiriesLeadId = TC.TC_InquiriesLeadId
	LEFT JOIN TC_CustomerDetails CUST WITH (NOLOCK) ON CUST.Id = TCL.TC_CustomerId
	LEFT JOIN PGTransactions PG WITH (NOLOCK) ON DI.ID = PG.CarId AND PG.PackageId = 80 -- for Deals 

	WHERE 
	--(PG.PackageId = 80 OR PG.PackageId IS NULL )-- for Deals 
		--AND 
		(
			@UserName = ''
			OR DI.CustomerName LIKE '%' + @UserName + '%'
			)
		AND (
			@UserEmail = ''
			OR DI.CustomerEmail LIKE '%' + @UserEmail + '%'
			)
		AND (
			@UserContactNo = ''
			OR DI.CustomerMobile LIKE '%' + @UserContactNo + '%'
			)
		AND (
			@TransactionID = ''
			OR DI.ID LIKE @TransactionID
			)

		ORDER BY DI.EntryDateTime DESC
END
