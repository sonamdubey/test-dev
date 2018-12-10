IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQLeadFollowupLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQLeadFollowupLoad]
GO

	
-- =============================================  
-- Author:  <Author,Nilesh Utture>  
-- Create date: <Create Date,07/01/2013>  
-- Description: <Description, This SP will give list of all inquiries for a particular customer>  
-- Modified By: Nilesh Utture on 27th Feb, 2013 Added UserId in Select Query
-- Modified By: Nilesh Utture on 8th April, 2013 Added CASE statement in SELECT statement which retrieves comments
-- Modified By: Nilesh Utture on 24th April, 2013 commented UserId check wherever required and called sp TC_GetUserAuthorization
-- Modified by: Nilesh Utture on 26th July, 2013 Added Condition to NewCarInquiry Type "AND ISNULL(TCNI.CarDeliveryStatus, 0) <> 77"
--				This condition denotes that the particular inquiry is closed
-- Modified By Vivek Gupta on 31st July,2013, Added @leadOwnerId to get User Name of Particular lead.
-- EXEC TC_INQLeadFollowupLoad 5,3224,1,3264
-- Modified By: Vivek gupta on 12th Aug,2013, Added Address Column in select statement to fetch customer details.
-- Modified BY : Tejashree Patil on 27 Aug 2013, Fetched Salutations,LastName 
-- Modified By : Vivek Gupta on 2nd sep , Added  a left outr join in retrieving new car inquiries data for InvoiceDate
-- Modified By : Tejashree Patil on 5 Sept 2013, Fetched BookingCancelDate.
-- Modified By : Tejashree Patil on 2 Sept 2013 , Fetched UniqueCustomerId.
-- Modified by : Manish on 26-09-2013 since TD status change date is capturing in "TDStatusEntryDate" field
-- Modified By : Vivek Gupta on 23-07-2014, commented TC_LeadDispositionId check in where clauses to get closed inquiries also.
-- Modified By Vivek Gupta on 6-10-2014, fetched CarDeliveryStatus
-- Modified By : Tejashree Patil on 30-10-2014, Added @ApplicationId to identify application, Changed vwMMV to vwAllMMV.
-- Modified By: Ashwini Dhamankar on 25/11/2014 , Fetched CwOfferId
-- Modified By: Tejashree Patil on 4 Dec 2014 , Fetched IsPrebook field.
-- [dbo].[TC_INQLeadFollowupLoad] 5,205,1,176,1
-- Modified By Vivek Gupta on 22-12-2014, fetched buyerqueriescount after adding new join 
-- Modified By Vivek Gupta on 21-01-2015, added CustomerVerified field to know if the customer is verified or not
-- Modified By Vivek Gupta on 23-04-2015, for newcar inquiries made put left join with vwallmmv to get inq having versionid null
-- Modified By Vivek Gupta on 30-09-2015, Added MostInterested for inquiries and sorted by MostInterested
-- Modified By : Nilima More on 18 Jan 2016, Fetched Organization Name.
-- Modified By : Nilima More On 18th 2016,if PQ is not sent PQcompletedDate should be null.
-- exec TC_INQLeadFollowupLoad 5,23726,243,23195,243,1
-- Modified By : Ashwini Dhamankar on May 13,2016 (Fetched IsPaymentSuccess) 
-- Modified By : Suresh Prajapati on 28th June, 2016
-- Description : Removed SP call TC_INQActivityFeedLoad
-- Modified By : Sunil M. Yadav On 01st Sept 2016 , Get only those advantage leads which are not forworded to actual dealers.
-- Modified By: Tejashree Patil on 5 Oct 2016, Optimized SP changed table variable to temparory table
-- [dbo].[TC_INQLeadFollowupLoad]5,29527,243,29011,243,1
-- Modified By : Ashwini Dhamankar on Oct 5,2016 (Added leadinquiryTypeid = 5 (advantage))
-- =============================================  
CREATE PROCEDURE [dbo].[TC_INQLeadFollowupLoad]
	-- Add the parameters for the stored procedure here  
	@BranchId INT
	,@CustomerId INT
	,@UserId INT
	,@LeadId INT
	,@leadOwnerId INT = NULL
	,@ApplicationId TINYINT = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT OFF;

	DECLARE @IsBuyerAuthorized BIT = 0
	DECLARE @IsSellerAuthorized BIT = 0
	DECLARE @IsNewAuthorized BIT = 0
	DECLARE @IsUserAuthorized BIT = 0

	-- Modified By: Nilesh Utture on 24th April, 2013
	EXEC TC_GetUserAuthorization @UserId
		,@BranchId
		,@LeadId
		,@IsBuyerAuthorized OUTPUT
		,@IsSellerAuthorized OUTPUT
		,@IsNewAuthorized OUTPUT -- true if user is having Admin/reporting Sales manager rights

	IF (
			@IsBuyerAuthorized = 1
			OR @IsSellerAuthorized = 1
			OR @IsNewAuthorized = 1
			)
	BEGIN
		SET @IsUserAuthorized = 1
	END
	-- Modified By: Tejashree Patil on 5 Oct 2016
	CREATE TABLE #TC_InquiriesLead(
		TC_InquiriesLeadId	BIGINT
		,BranchId	NUMERIC(8)
		,TC_CustomerId	BIGINT
		,TC_UserId	BIGINT
		,InquiryCount	SMALLINT
		,NextFollowUpDate	DATETIME
		,LastFollowUpDate	DATETIME
		,LastFollowUpComment	VARCHAR(200)
		,TC_InquiryTypeId	SMALLINT
		,TC_InquiryStatusId	SMALLINT
		,TC_InquiriesFollowupActionId	SMALLINT
		,CreatedBy	BIGINT
		,CreatedDate	DATETIME
		,ModifiedBy	BIGINT
		,ModifiedDate	DATETIME
		,IsActionTaken	BIT
		,TC_LeadTypeId	TINYINT
		,InqTypeDesc	VARCHAR(10)
		,IsActive	BIT
		,TC_LeadId	INT
		,TC_LeadInquiryTypeId	TINYINT
		,TC_LeadStageId	TINYINT
		,TC_LeadDispositionID	TINYINT
		,CarDetails	VARCHAR(200)
		,LatestInquiryDate	DATETIME
		,TC_BWLeadStatusId	TINYINT
	)
	-- Modified By: Tejashree Patil on 5 Oct 2016
	INSERT  #TC_InquiriesLead
	SELECT TC_InquiriesLeadId
		,BranchId
		,TC_CustomerId
		,TC_UserId
		,InquiryCount
		,NextFollowUpDate
		,LastFollowUpDate
		,LastFollowUpComment
		,TC_InquiryTypeId
		,TC_InquiryStatusId
		,TC_InquiriesFollowupActionId
		,CreatedBy
		,CreatedDate
		,ModifiedBy
		,ModifiedDate
		,IsActionTaken
		,TC_LeadTypeId
		,InqTypeDesc
		,IsActive
		,
		--Old_TC_CustomerId,
		TC_LeadId
		,TC_LeadInquiryTypeId
		,TC_LeadStageId
		,TC_LeadDispositionID
		,CarDetails
		,LatestInquiryDate
		,TC_BWLeadStatusId
	FROM TC_InquiriesLead AS L1 WITH (NOLOCK)
	WHERE L1.BranchId = @BranchId
		AND L1.TC_LeadId = @LeadId;--AND L1.TC_UserId = @UserId;


	-- Insert statements for procedure here  
	WITH CteLeadDetails
	AS (
		SELECT TCBICount.BuyerQueriesCount
			,B.TC_BuyerInquiriesId AS Id
			,L.TC_LeadInquiryTypeId
			,CONVERT(VARCHAR(10), S.Id) AS StockId
			,CONVERT(VARCHAR(10), S.StatusId) AS StockStatus
			,B.BookingDate
			,'' AS PQRequestedDate
			,'' AS PQCompletedDate
			,'' AS TDRequestedDate
			,'' AS TDDate
			,'' AS TDCalendarId
			,'' AS SourceId
			,'' AS TDStatus
			,'' AS PQStatus
			,B.BookingStatus
			,'' AS InvoiceDate
			,-- Modified By : Vivek Gupta on 2nd sep
			ISNULL(B.TC_LeadDispositionID, 0) AS DispoId
			,(
				CASE L.TC_LeadInquiryTypeId
					WHEN 1
						THEN 'Buyer'
					END
				) AS Type
			,V.Car
			,B.CreatedOn
			,@IsBuyerAuthorized AS IsUserAuthorized
			,'' AS BookingCancelDate
			,V.MakeId AS MakeId
			,-- Modified By : Tejashree Patil on 5 Sept 2013
			0 AS CarDeliveryStatus
			,- 1 CwOfferId
			,0 IsOfferClaimed
			,0 IsPrebook
			,CAST(ISNULL(MostInterested, 0) AS VARCHAR) AS MostInterested
			,'' AS TC_DealsStockVINId
			,L.TC_BWLeadStatusId AS TC_BWLeadStatusId
			,0 AS IsPaymentSuccess --Modified By : Ashwini Dhamankar on May 13,2016 (Fetched IsPaymentSuccess) 
			,NULL AS ActualDealerId
			,NULL AS ActualDealerName
			,0 AS CityId
			,0 AS VersionId
			,NULL AS CwDealerInqId
		FROM #TC_InquiriesLead L WITH (NOLOCK)
		INNER JOIN TC_BuyerInquiries B WITH (NOLOCK) ON L.TC_InquiriesLeadId = B.TC_InquiriesLeadId
		INNER JOIN TC_Stock S WITH (NOLOCK) ON B.StockId = S.Id
		INNER JOIN vwAllMMV V WITH (NOLOCK) ON V.VersionId = S.VersionId
		LEFT JOIN (
			SELECT BIC.TC_BuyerInquiryId
				,Count(Id) BuyerQueriesCount
			FROM TC_BuyerInquiryComments BIC WITH (NOLOCK)
			JOIN TC_BuyerInquiries BIN WITH (NOLOCK) ON BIC.TC_BuyerInquiryId = BIN.TC_BuyerInquiriesId
			JOIN #TC_InquiriesLead ILN WITH (NOLOCK) ON BIN.TC_InquiriesLeadId = ILN.TC_InquiriesLeadId
			GROUP BY BIC.TC_BuyerInquiryId
			) AS TCBICount ON TCBICount.TC_BuyerInquiryId = B.TC_BuyerInquiriesId
		WHERE L.BranchId = @BranchId
			AND L.TC_LeadId = @LeadId
			AND V.ApplicationId = ISNULL(@ApplicationId, 1) -- Modified By : Tejashree Patil on 30-10-2014
			--AND L.TC_UserId = @UserId -- Modified By: Nilesh Utture on 24th April, 2013
			-- AND (B.TC_LeadDispositionID IS NULL OR B.TC_LeadDispositionId = 4)
		
		UNION ALL
		
		SELECT '' BuyerQueriesCount
			,B.TC_BuyerInquiriesId AS Id
			,L.TC_LeadInquiryTypeId
			,''
			,''
			,''
			,''
			,''
			,''
			,''
			,''
			,''
			,''
			,''
			,''
			,'' AS InvoiceDate
			,-- Modified By : Vivek Gupta on 2nd sep
			ISNULL(B.TC_LeadDispositionID, 0)
			,(
				CASE L.TC_LeadInquiryTypeId
					WHEN 1
						THEN 'Buyer'
					END
				)
			,''
			,B.CreatedOn
			,@IsBuyerAuthorized AS IsUserAuthorized
			,''
			,- 1 AS MakeId
			,-- Modified By : Tejashree Patil on 5 Sept 2013
			0
			,- 1 CwOfferId
			,0 IsOfferClaimed
			,0 IsPrebook
			,CAST(ISNULL(MostInterested, 0) AS VARCHAR) AS MostInterested
			,'' AS TC_DealsStockVINId
			,L.TC_BWLeadStatusId AS TC_BWLeadStatusId
			,0 AS IsPaymentSuccess
			,NULL AS ActualDealerId
			,NULL AS ActualDealerName
			,0 AS CityId
			,0 AS VersionId
			,NULL AS CwDealerInqId
		FROM #TC_InquiriesLead L WITH (NOLOCK)
		INNER JOIN TC_BuyerInquiries B WITH (NOLOCK) ON L.TC_InquiriesLeadId = B.TC_InquiriesLeadId
		WHERE L.BranchId = @BranchId
			AND B.StockId IS NULL
			--AND L.TC_UserId = @UserId -- Modified By: Nilesh Utture on 24th April, 2013
			AND L.TC_LeadId = @LeadId
		--AND (B.TC_LeadDispositionId IS NULL OR B.TC_LeadDispositionId = 4)
		
		UNION ALL
		
		SELECT ''
			,SL.TC_SellerInquiriesId AS Id
			,L.TC_LeadInquiryTypeId
			,ISNULL(CONVERT(VARCHAR, ST.Id), '')
			,''
			,SL.PurchasedDate
			,''
			,''
			,''
			,''
			,''
			,''
			,''
			,''
			,SL.PurchasedStatus
			,'' AS InvoiceDate
			,-- Modified By : Vivek Gupta on 2nd sep
			ISNULL(SL.TC_LeadDispositionID, '') AS DispoId
			,(
				CASE L.TC_LeadInquiryTypeId
					WHEN 2
						THEN 'Seller'
					END
				)
			,V.Car
			,SL.CreatedOn
			,@IsSellerAuthorized AS IsUserAuthorized
			,''
			,V.MakeId AS MakeId
			,-- Modified By : Tejashree Patil on 5 Sept 2013
			0
			,- 1 CwOfferId
			,0 IsOfferClaimed
			,0 IsPrebook
			,'' AS MostInterested
			,'' AS TC_DealsStockVINId
			,L.TC_BWLeadStatusId AS TC_BWLeadStatusId
			,0 AS IsPaymentSuccess
			,NULL AS ActualDealerId
			,NULL AS ActualDealerName
			,0 AS CityId
			,0 AS VersionId
			,NULL AS CwDealerInqId
		FROM #TC_InquiriesLead L WITH (NOLOCK)
		INNER JOIN TC_SellerInquiries SL WITH (NOLOCK) ON L.TC_InquiriesLeadId = SL.TC_InquiriesLeadId
		INNER JOIN vwAllMMV V WITH (NOLOCK) ON SL.CarVersionId = V.VersionId
		LEFT OUTER JOIN TC_Stock ST WITH (NOLOCK) ON ST.TC_SellerInquiriesId = SL.TC_SellerInquiriesId
		WHERE L.BranchId = @BranchId
			AND L.TC_LeadId = @LeadId
			AND V.ApplicationId = ISNULL(@ApplicationId, 1) -- Modified By : Tejashree Patil on 30-10-2014
			--AND L.TC_UserId = @UserId  -- Modified By: Nilesh Utture on 24th April, 2013
			--AND (SL.TC_LeadDispositionID IS NULL OR SL.TC_LeadDispositionID = 4)
		
		UNION ALL
		
		SELECT ''
			,N.TC_NewCarInquiriesId AS Id
			,L.TC_LeadInquiryTypeId
			,N.TC_Deals_StockId
			,''
			,N.BookingDate
			,CONVERT(VARCHAR, PQ.RequestedDate)
			,CONVERT(VARCHAR, ISNULL(PQ.PqCompletedDate, ISNULL(PQR.PQDate, NULL))) PqCompletedDate
			,CONVERT(VARCHAR, N.TDRequestedDate)
			,CONVERT(VARCHAR, N.TDStatusEntryDate)
			,/*CONVERT(VARCHAR,N.TDDate)*/ -- Modified by manish on 26-09-2013 since status change data is capturing in separate field
			CONVERT(VARCHAR, TDC.TC_TDCalendarId)
			,CONVERT(VARCHAR, N.TC_InquirySourceId)
			,CONVERT(VARCHAR, N.TDStatus)
			,N.PQStatus
			,N.BookingStatus
			,NCB.InvoiceDate
			,-- Modified By : Vivek Gupta on 2nd sep
			ISNULL(N.TC_LeadDispositionId, 0) AS DispoId
			,CONVERT(VARCHAR, (
					CASE N.IsCorporate
						WHEN 0
							THEN '<b>Individual </b>'
						WHEN 1
							THEN '<b>Corporate </b>'
						END
					)) + CONVERT(VARCHAR, (
					CASE L.TC_LeadInquiryTypeId
						WHEN 3
							THEN 'New'
					   WHEN 5
					    THEN 'Advantage' -- Modified By : Ashwini Dhamankar on Oct 5,2016 (Added inquiryType = 5 (advantage))
					END
					))
			,ISNULL(V.Car, 'Car Not Specified')
			,N.CreatedOn
			,@IsNewAuthorized AS IsUserAuthorized
			,N.BookingCancelDate
			,ISNULL(V.MakeId, 0) AS MakeId
			,-- Modified By : Tejashree Patil on 5 Sept 2013
			ISNULL(N.CarDeliveryStatus, 0)
			,ISNULL(N.CwOfferId, - 1) CwOfferId
			,ISNULL(NCB.IsOfferClaimed, 0) IsOfferClaimed
			,ISNULL(IsPrebook, 0) IsPrebook
			,CAST(ISNULL(MostInterested, 0) AS VARCHAR) AS MostInterested --Modified By: Ashwini Dhamankar on 25/11/2014 , Fetched CwOfferId
			,N.TC_DealsStockVINId
			,L.TC_BWLeadStatusId AS TC_BWLeadStatusId
			,ISNULL(N.IsPaymentSuccess, 0) AS IsPaymentSuccess
			,D.ID AS ActualDealerId
			,D.Organization AS ActualDealerName
			,ISNULL(N.CityId,0) AS CityId
			,N.VersionId
			,DIM.CwDealerInqId AS CwDealerInqId
		FROM TC_NewCarInquiries N  WITH (NOLOCK)
		INNER JOIN #TC_InquiriesLead L WITH (NOLOCK) ON L.TC_InquiriesLeadId = N.TC_InquiriesLeadId
		LEFT JOIN vwAllMMV V WITH(NOLOCK) ON N.VersionId = V.VersionId
		LEFT OUTER JOIN TC_TDCalendar TDC WITH (NOLOCK) ON TDC.TC_TDCalendarId = N.TC_TDCalendarId
		LEFT OUTER JOIN TC_PqRequest PQ WITH (NOLOCK) ON PQ.TC_NewCarInquiriesId = N.TC_NewCarInquiriesId
		LEFT OUTER JOIN TC_NewCarBooking NCB WITH (NOLOCK) ON N.TC_NewCarInquiriesId = NCB.TC_NewCarInquiriesId
		LEFT OUTER JOIN TC_PriceQuoteRequests PQR WITH (NOLOCK) ON PQR.TC_InquiriesId = N.TC_NewCarInquiriesId
		LEFT OUTER JOIN TC_Deals_InquiriesMapping DIM WITH(NOLOCK) ON DIM.CwDealerInqId = N.TC_NewCarInquiriesId -- Sunil M. Yadav On 01st Sept 2016
		LEFT OUTER JOIN TC_Deals_Stock TDS WITH(NOLOCK) ON TDS.Id = N.TC_Deals_StockId
		LEFT OUTER JOIN Dealers D WITH(NOLOCK) ON TDS.BranchId = D.ID 
		WHERE L.BranchId = @BranchId --- Added join by vivek on 02-09-2013 in TC_NewCarBooking table
			AND L.TC_LeadId = @LeadId
			AND ISNULL(V.ApplicationId, 1) = ISNULL(@ApplicationId, 1) -- Modified By : Tejashree Patil on 30-10-2014
			--AND L.TC_UserId = @UserId  -- Modified By: Nilesh Utture on 24th April, 2013
			-- AND (N.TC_LeadDispositionId IS NULL OR (N.TC_LeadDispositionId = 4 AND ISNULL(N.CarDeliveryStatus, 0) <> 77))-- Modified by: Nilesh Utture on 26th July, 2013 

		UNION ALL

		SELECT ''
			,SI.TC_Service_InquiriesId AS Id
			,L.TC_LeadInquiryTypeId
			,'' TC_Deals_StockId
			,''
			,SI.ServiceCompletedDate BookingDate
			,'' RequestedDate
			,'' PqCompletedDate
			,'' TDRequestedDate
			,'' TDStatusEntryDate
			,/*CONVERT(VARCHAR,N.TDDate)*/ -- Modified by manish on 26-09-2013 since status change data is capturing in separate field
			'' TC_TDCalendarId
			,1 as TC_InquirySourceId
			,'' TDStatus
			,'' PQStatus
			,SI.BookingStatus
			,'' InvoiceDate
			,-- Modified By : Vivek Gupta on 2nd sep
			ISNULL(SI.TC_LeadDispositionId, 0) AS DispoId
			,(
				CASE L.TC_LeadInquiryTypeId
					WHEN 4
						THEN 'Service'
					END
				) AS Type
			,ISNULL(V.Car, 'Car Not Specified')
			,SI.EntryDate AS CreatedOn
			,@IsNewAuthorized AS IsUserAuthorized
			,'' BookingCancelDate
			,ISNULL(V.MakeId, 0) AS MakeId
			,-- Modified By : Tejashree Patil on 5 Sept 2013
			ISNULL(SI.BookingStatus, 0) CarDeliveryStatus
			,-1 CwOfferId
			,0 IsOfferClaimed
			,0 IsPrebook
			,CAST(ISNULL(SI.VersionId , 0) AS VARCHAR) AS MostInterested --Modified By: Ashwini Dhamankar on 25/11/2014 , Fetched CwOfferId
			,'' AS TC_DealsStockVINId
			,'' AS TC_BWLeadStatusId
			,'' AS IsPaymentSuccess
			,NULL AS ActualDealerId
			,NULL AS ActualDealerName
			,0 AS CityId
			,0 AS VersionId
			,NULL AS CwDealerInqId
			FROM TC_Service_Inquiries SI WITH (NOLOCK)
			INNER JOIN #TC_InquiriesLead L WITH (NOLOCK) ON L.TC_InquiriesLeadId =SI.TC_InquiriesLeadId
			LEFT JOIN vwAllMMV V ON SI.VersionId = V.VersionId
			WHERE L.BranchId = @BranchId --- Added join by vivek on 02-09-2013 in TC_NewCarBooking table
			AND L.TC_LeadId = @LeadId
			AND ISNULL(V.ApplicationId, 1) = ISNULL(@ApplicationId, 1)
		)
	SELECT *
	FROM CteLeadDetails WITH (NOLOCK)
	ORDER BY MostInterested DESC
		,CreatedOn DESC;

	DECLARE @IsLeadVerified BIT = 0

	IF EXISTS (
			SELECT TOP 1 TC_LeadId
			FROM TC_Lead L WITH (NOLOCK)
			WHERE L.TC_CustomerId = @CustomerId
				AND BranchId = @BranchId
				AND L.TC_LeadStageId = 2
			)
	BEGIN
		SET @IsLeadVerified = 1
	END

	--Modified BY : Tejashree Patil on 27 Aug 2013, Fetched Salutations,LastName 
	SELECT C.UniqueCustomerId
		,C.Salutation
		,C.CustomerName
		,C.LastName
		,C.Email
		,C.Mobile
		,@IsLeadVerified 'IsVerified'
		,C.Address
		,C.IsVerified AS CustomerVerified -- Modified By : Tejashree Patil on 2 Sept 2013 
	FROM TC_CustomerDetails C WITH (NOLOCK)
	WHERE Id = @CustomerId;

	SELECT TC_InquiryStatusId
		,TC_LeadStageId
	FROM #TC_InquiriesLead 
	WHERE TC_LeadId = @LeadId;--AND TC_UserId = @UserId  ;

	-- EXEC TC_INQActivityFeedLoad @LeadId;
	-- Modified By : Nilesh Utture on 27th Feb, 2013
	SELECT UserName
		,@IsUserAuthorized AS IsUserAuthorized
	FROM TC_Users U WITH (NOLOCK)
	WHERE U.Id = @UserId --WHERE TC_LeadId = @LeadId -- Modified By: Nilesh Utture on 24th April, 2013

	SELECT A.TC_LeadDispositionId
		,A.VisitDate
		,A.Purpose
		,U.UserName
	FROM TC_Appointments A WITH (NOLOCK)
	INNER JOIN TC_Users U WITH (NOLOCK) ON U.Id = A.TC_UsersId
	WHERE TC_LeadId = @LeadId;

	DROP TABLE #TC_InquiriesLead

	IF (@leadOwnerId IS NOT NULL)
	BEGIN
		SELECT U.UserName
			,D.Organization Organization
		FROM TC_Users U WITH (NOLOCK)
		INNER JOIN Dealers D WITH (NOLOCK) ON U.BranchId = D.ID
		WHERE U.ID = @leadOwnerId
	END
END




	