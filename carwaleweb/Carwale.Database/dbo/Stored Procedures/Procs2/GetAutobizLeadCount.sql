IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAutobizLeadCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAutobizLeadCount]
GO

	

CREATE PROCEDURE [dbo].[GetAutobizLeadCount] 
@DailyCount INT OUTPUT,
@YesterdayCount INT OUTPUT,
@MonthlyCount INT OUTPUT
AS
BEGIN
	
	SELECT @DailyCount = COUNT(*) FROM (
		SELECT DISTINCT TC.Mobile
			,TI.BranchId
		FROM TC_NewCarInquiries TN WITH (NOLOCK)
		INNER JOIN TC_InquiriesLead TI WITH (NOLOCK) ON TN.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
		INNER JOIN TC_CustomerDetails TC WITH (NOLOCK) ON TC.Id = TI.TC_CustomerId
		INNER JOIN TC_InquirySource T WITH (NOLOCK) ON T.Id = TN.TC_InquirySourceId
		INNER JOIN tc_lead L WITH (NOLOCK) ON L.TC_CustomerId = TC.id
		WHERE day(CreatedOn) = DAY(GETDATE())
			AND MONTH(CreatedOn) = Month(GETDATE())
			AND YEAR(CreatedOn) = YEAR(GETDATE())
			AND TI.BranchId NOT IN (
				9735
				,15756
				,16345
				,9350
				,15418
				,16403
				,16183
				,10216
				,535
				,4815
				)
			AND T.Product_NewCar = 1
			) AS A
	

	SELECT @YesterdayCount = COUNT(*) FROM (
	
		SELECT DISTINCT TC.Mobile
			,TI.BranchId
		FROM TC_NewCarInquiries TN WITH (NOLOCK)
		INNER JOIN TC_InquiriesLead TI WITH (NOLOCK) ON TN.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
		INNER JOIN TC_CustomerDetails TC WITH (NOLOCK) ON TC.Id = TI.TC_CustomerId
		INNER JOIN TC_InquirySource T WITH (NOLOCK) ON T.Id = TN.TC_InquirySourceId
		INNER JOIN tc_lead L WITH (NOLOCK) ON L.TC_CustomerId = TC.id
		WHERE day(CreatedOn) = DAY(GETDATE()-1)
			AND MONTH(CreatedOn) = Month(GETDATE())
			AND YEAR(CreatedOn) = YEAR(GETDATE())
			AND TI.BranchId NOT IN (
				9735
				,15756
				,16345
				,9350
				,15418
				,16403
				,16183
				,10216
				,535
				,4815
				)
			AND T.Product_NewCar = 1
	) AS B

	SELECT @MonthlyCount = COUNT(*) FROM (
		SELECT DISTINCT TC.Mobile
			,TI.BranchId
		FROM TC_NewCarInquiries TN WITH (NOLOCK)
		INNER JOIN TC_InquiriesLead TI WITH (NOLOCK) ON TN.TC_InquiriesLeadId = TI.TC_InquiriesLeadId
		INNER JOIN TC_CustomerDetails TC WITH (NOLOCK) ON TC.Id = TI.TC_CustomerId
		INNER JOIN TC_InquirySource T WITH (NOLOCK) ON T.Id = TN.TC_InquirySourceId
		INNER JOIN tc_lead L WITH (NOLOCK) ON L.TC_CustomerId = TC.id
		WHERE MONTH(CreatedOn) = Month(GETDATE())
			AND YEAR(CreatedOn) = YEAR(GETDATE())
			AND TI.BranchId NOT IN (
				9735
				,15756
				,16345
				,9350
				,15418
				,16403
				,16183
				,10216
				,535
				,4815
				)
			AND T.Product_NewCar = 1
	) as C
END
