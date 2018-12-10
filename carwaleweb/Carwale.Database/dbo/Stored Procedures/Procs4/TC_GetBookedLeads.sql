IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetBookedLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetBookedLeads]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: November 25,2015
-- Description:	Fetch Booked Leads
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetBookedLeads] 
	@TC_UserId BIGINT ,
	@TC_InquiriesLeadId INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @BranchId BIGINT,@TC_DealerTypeId INT

	SELECT @BranchId = Branchid FROM TC_Users WITH(NOLOCK) WHERE Id = @TC_UserId
	SELECT @TC_DealerTypeId = TC_DealerTypeId FROM Dealers WITH(NOLOCK) WHERE Id = @BranchId
	
	IF(@TC_DealerTypeId = 1)
	BEGIN
		SELECT  C.id AS [CustomerId]
				,TI.CarDetails AS CarDetails
				,C.Salutation AS CustomerSalutation
				,C.CustomerName  AS CustomerName
				,C.LastName AS CustomerLastName
				,C.Mobile AS CustomerMobile
				,C.Email AS CustomerEmail
				,C.Buytime AS Buytime
				,C.Comments AS Comments
				,C.Address AS CutomerAddress
				,C.Salutation AS BookingSalutation
				,C.CustomerName  AS BookingName
				,C.LastName AS BookingLastName
				,C.Mobile AS BookingMobile
				,CB.BookingDate AS BookingDate
				,ST.Price AS Price
				,CB.NetPayment AS Payment
				,NULL AS PendingPayment
				,NULL AS VinNo
				,NULL AS IsLoanRequired
				,NULL AS PrefDeliveryDate
				,NULL AS ModelYear
				,NULL AS CarColorId
				,NULL AS PaymentMode
				,NULL AS PickupDateTime
				
				FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
				INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId
				INNER JOIN TC_InquiriesLead AS TI WITH (NOLOCK) ON  TCAC.TC_LeadId = TI.TC_LeadId 
				INNER JOIN TC_BuyerInquiries AS BI WITH (NOLOCK) ON  TI.TC_InquiriesLeadId = BI.TC_InquiriesLeadId AND BI.BookingStatus = 34
				INNER JOIN TC_CarBooking AS CB WITH(NOLOCK) ON BI.StockId = CB.StockId
				INNER JOIN TC_Stock AS ST WITH(NOLOCK) ON BI.StockId = ST.Id
				WHERE  TI.TC_UserId = @TC_UserId 
				AND (
				( TI.TC_InquiriesLeadId = @TC_InquiriesLeadId 
				OR @TC_InquiriesLeadId IS NULL)
				)
	END

	IF(@TC_DealerTypeId = 2)
	BEGIN
		SELECT	C.id AS [CustomerId]
				,TI.CarDetails AS CarDetails
				,C.Salutation AS CustomerSalutation
				,C.CustomerName  AS CustomerName
				,C.LastName AS CustomerLastName
				,C.Mobile AS CustomerMobile
				,C.Email AS CustomerEmail
				,C.Buytime AS Buytime
				,C.Comments AS Comments
				,C.Address AS CutomerAddress
				,NCB.Salutation AS BookingSalutation
	            ,NCB.BookingName AS BookingName
				,NCB.LastName AS BookingLastName
				,NCB.BookingMobile AS BookingMobile
				,NCB.BookingDate AS BookingDate
				,NCB.Price  AS Price
				,NCB.Payment AS Payment
				,NCB.PendingPayment AS PendingPayment 
				,NCB.VinNo AS VinNo
				,NCB.IsLoanRequired AS IsLoanRequired
				,NCB.PrefDeliveryDate AS PrefDeliveryDate
				,NCB.ModelYear AS ModelYear
				,NCB.CarColorId AS CarColorId
				,NCB.PaymentMode AS PaymentMode
				,NCB.PickupDateTime AS PickupDateTime
	    
				FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
				INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId
				INNER JOIN TC_InquiriesLead AS TI WITH (NOLOCK) ON  tcac.TC_LeadId = ti.TC_LeadId and TI.TC_LeadDispositionID = 4
				INNER JOIN TC_NewCarInquiries AS NCI WITH (NOLOCK) ON  TI.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
				INNER JOIN TC_NewCarBooking AS NCB WITH(NOLOCK) ON NCI.TC_NewCarInquiriesId =  NCB.TC_NewCarInquiriesId 
				WHERE  TI.TC_UserId = @TC_UserId 
				AND (
				( TI.TC_InquiriesLeadId = @TC_InquiriesLeadId 
				OR @TC_InquiriesLeadId IS NULL)
				)
	END

	IF(@TC_DealerTypeId = 3)
	BEGIN

	SELECT  C.id AS [CustomerId]
				,TI.CarDetails AS CarDetails
				,C.Salutation AS CustomerSalutation
				,C.CustomerName  AS CustomerName
				,C.LastName AS CustomerLastName
				,C.Mobile AS CustomerMobile
				,C.Email AS CustomerEmail
				,C.Buytime AS Buytime
				,C.Comments AS Comments
				,C.Address AS CutomerAddress
				,C.Salutation AS BookingSalutation
				,C.CustomerName  AS BookingName
				,C.LastName AS BookingLastName
				,C.Mobile AS BookingMobile
				,CB.BookingDate AS BookingDate
				,ST.Price AS Price
				,CB.NetPayment AS Payment
				,NULL AS PendingPayment
				,NULL AS VinNo
				,NULL AS IsLoanRequired
				,NULL AS PrefDeliveryDate
				,NULL AS ModelYear
				,NULL AS CarColorId
				,NULL AS PaymentMode
				,NULL AS PickupDateTime
				
				FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
				INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId
				INNER JOIN TC_InquiriesLead AS TI WITH (NOLOCK) ON  TCAC.TC_LeadId = TI.TC_LeadId 
				INNER JOIN TC_BuyerInquiries AS BI WITH (NOLOCK) ON  TI.TC_InquiriesLeadId = BI.TC_InquiriesLeadId AND BI.BookingStatus = 34
				INNER JOIN TC_CarBooking AS CB WITH(NOLOCK) ON BI.StockId = CB.StockId
				INNER JOIN TC_Stock AS ST WITH(NOLOCK) ON BI.StockId = ST.Id
				WHERE  TI.TC_UserId = @TC_UserId 
				AND (
				( TI.TC_InquiriesLeadId = @TC_InquiriesLeadId 
				OR @TC_InquiriesLeadId IS NULL)
				)

		UNION

		SELECT	C.id AS [CustomerId]
				,TI.CarDetails AS CarDetails
				,C.Salutation AS CustomerSalutation
				,C.CustomerName  AS CustomerName
				,C.LastName AS CustomerLastName
				,C.Mobile AS CustomerMobile
				,C.Email AS CustomerEmail
				,C.Buytime AS Buytime
				,C.Comments AS Comments
				,C.Address AS CutomerAddress
				,NCB.Salutation AS BookingSalutation
	            ,NCB.BookingName AS BookingName
				,NCB.LastName AS BookingLastName
				,NCB.BookingMobile AS BookingMobile
				,NCB.BookingDate AS BookingDate
				,NCB.Price  AS Price
				,NCB.Payment AS Payment
				,NCB.PendingPayment AS PendingPayment 
				,NCB.VinNo AS VinNo
				,NCB.IsLoanRequired AS IsLoanRequired
				,NCB.PrefDeliveryDate AS PrefDeliveryDate
				,NCB.ModelYear AS ModelYear
				,NCB.CarColorId AS CarColorId
				,NCB.PaymentMode AS PaymentMode
				,NCB.PickupDateTime AS PickupDateTime
	    
				FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
				INNER JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId
				INNER JOIN TC_InquiriesLead AS TI WITH (NOLOCK) ON  tcac.TC_LeadId = ti.TC_LeadId and TI.TC_LeadDispositionID = 4
				INNER JOIN TC_NewCarInquiries AS NCI WITH (NOLOCK) ON  TI.TC_InquiriesLeadId = NCI.TC_InquiriesLeadId
				INNER JOIN TC_NewCarBooking AS NCB WITH(NOLOCK) ON NCI.TC_NewCarInquiriesId =  NCB.TC_NewCarInquiriesId 
				WHERE  TI.TC_UserId = @TC_UserId 
				AND (
				( TI.TC_InquiriesLeadId = @TC_InquiriesLeadId 
				OR @TC_InquiriesLeadId IS NULL)
				)
	END
	
    
END

-------------------------------------------


SELECT TOP 1 * FROM CarMakes ORDER BY ID DESC
SELECT TOP 1 * FROM CarModels ORDER BY ID DESC
SELECT TOP 1 * FROM CarVersions ORDER BY ID DESC

--INSERT INTO CarMakes(Name,IsDeleted,MaCreatedOn)
--VALUES('Make',1,GETDATE())

SELECT TOP 1* FROM CarMakes ORDER BY ID DESC--76

--INSERT INTO CarModels (Name,CarMakeId,IsDeleted,comment,MoCreatedOn)
--VALUES('Model',76,1,'Dummy CarModel',GETDATE())

SELECT TOP 1* FROM CarModels ORDER BY ID DESC--939

--INSERT INTO CarVersions (Name,CarModelId,IsDeleted,comment, SegmentId, BodyStyleId,VCreatedOn)
--VALUES('Version',939,1,'Dummy CarVersion',1,1, GETDATE())

SELECT TOP 1 * FROM CarVersions ORDER BY ID DESC--4205 /

-------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[TC_INQNewCarBuyerSave]    Script Date: 12/9/2015 7:07:52 PM ******/
SET ANSI_NULLS ON
