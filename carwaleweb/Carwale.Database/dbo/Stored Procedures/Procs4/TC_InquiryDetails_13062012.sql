IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryDetails_13062012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryDetails_13062012]
GO

	-- Author:		Tejashree Patil
-- Modified date: 11 April 2012
-- Description:	Getting details of buyer inquiries.
-- =============================================
--Author: Binumon George
--Date:30-03-2012
--Descripton: geting all type of inquiry Details
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiryDetails_13062012]
@TC_InquiryId BIGINT,
@BranchId INT,
@InqType TINYINT
AS
BEGIN
	IF(@InqType=1)--buyer
		BEGIN
			SELECT DISTINCT INQT.InquiryType 'Inquiry Type ',
			--INQ.CreatedDate 'Inquiry Date',
			COALESCE(INQ.CarName +' '+'MakeYear '+ISNULL(SUBSTRING(CONVERT(VARCHAR(12),ST.MakeYear,106),3,9),'-') +', '+ 
			ISNULL(ST.Colour,'-') +', '+ CONVERT(VARCHAR(10),ST.Price)+', ' + CONVERT(VARCHAR(10),ST.Kms)+ 
			' kms, ' + CC.RegistrationPlace,'Min Price ' + ISNULL(CAST(MinPrice AS VARCHAR(10)),'-') + 
			' Max Price ' + ISNULL(CAST(MaxPrice AS VARCHAR(10)),'-') + ', '+' From Year ' + ISNULL(CAST(FromMakeYear AS VARCHAR(10)),'-') +
			' To Year '+ ISNULL(CAST(ToMakeYear AS VARCHAR(10)),'-') +' ' +
			(CASE ISNULL(BodyType,' ') WHEN ' ' THEN ' ' ELSE ','+BodyType END)+' ' +' ' +ISNULL(ModelNames,' ')+' ')
			AS 'Intrested In ',	SRC.Source AS 'Inquiry Source', ST.Id 'Stock Id '
								
			FROM TC_Inquiries INQ WITH(NOLOCK)
			LEFT JOIN TC_BuyerInquiries BI WITH(NOLOCK)ON BI.TC_InquiriesId=INQ.TC_InquiriesId 
			LEFT JOIN TC_BuyerInqWithoutStock BIW WITH(NOLOCK)ON BIW.TC_InquiriesId=INQ.TC_InquiriesId
			LEFT JOIN TC_Stock ST WITH(NOLOCK) ON ST.Id=BI.StockId 
			LEFT JOIN TC_CarCondition CC WITH(NOLOCK) ON ST.Id=CC.StockId 
			INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id
			INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId
			WHERE INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=1
		END
	ELSE IF(@InqType=2)-- seller
		BEGIN
			SELECT DISTINCT INQT.InquiryType 'Inquiry Type ',
			--INQ.CreatedDate 'Inquiry Date',
			INQ.CarName + ',' +SUBSTRING(CONVERT(VARCHAR,SI.MakeYear, 106),3,9)  +', '+ (CASE ISNULL(SI.Price,'') 
			WHEN '' THEN '' ELSE ','+CONVERT(VARCHAR(10),SI.Price) END) + (CASE ISNULL(SI.Kms,'') WHEN '' THEN '' ELSE ','+
			CONVERT(VARCHAR(10),SI.Kms)+' Kms ' END)+ ISNULL(CONVERT(VARCHAR(10),SI.RegistrationPlace),'') AS 'Intrested In ',
			SRC.Source 'Inquiry Source', SI.CWInquiryId 'Profile Id', dbo.TC_GetOwnerValue(SI.Owners) AS 'No.of Owners', 
			SI.Insurance,SI.InsuranceExpiry, SI.Tax AS 'One Time Tax', SI.CarDriven AS 'Driven In', dbo.TC_GetBoolValue(SI.Accidental) AS 'Is Car Accident Free', dbo.TC_GetBoolValue(SI.FloodAffected) 'Is Flood Affected'
			FROM TC_Inquiries INQ WITH(NOLOCK) 
			LEFT JOIN TC_SellerInquiries SI WITH(NOLOCK)ON SI.TC_InquiriesId=INQ.TC_InquiriesId 
			INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id
			INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId
			WHERE INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=2
		END
	ELSE IF(@InqType=3)--testDrive
		BEGIN
			SELECT DISTINCT INQT.InquiryType 'Inquiry Type',
			INQ.CarName  AS 'Intrested In ',
			dbo.GetFuelType(TD.FuelType) AS 'FuelType' ,dbo.GetTransmissionType(TD.Transmission) AS 'Transmission Type', SRC.Source 'Inquiry Source'
			FROM TC_Inquiries INQ WITH(NOLOCK)
			LEFT JOIN TC_TestdriveRequests TD WITH(NOLOCK)ON TD.TC_InquiriesId=INQ.TC_InquiriesId 
			INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id
			INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId
			WHERE INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=3
		END
	ELSE IF(@InqType=4)--Price Quotes
		BEGIN
			SELECT DISTINCT INQT.InquiryType 'Inquiry Type',
			INQ.CarName  AS 'Intrested In ',
			PQ.BuyTime, SRC.Source 'Inquiry Source'
			FROM TC_Inquiries INQ WITH(NOLOCK)
			LEFT JOIN TC_PriceQuoteRequests PQ WITH(NOLOCK)ON PQ.TC_InquiriesId=INQ.TC_InquiriesId 
			INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id
			INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId
			WHERE INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=4
		END
	ELSE IF(@InqType=5)--Service Rquest
		BEGIN
			SELECT DISTINCT INQT.InquiryType 'Inquiry Type',
			INQ.CarName  AS 'Intrested In ',
			dbo.GetServiceType(SR.TypeOfService)AS TypeOfService, SR.RegNo,  CONVERT(varchar, SR.PreferredDate, 106) AS PreferredDate, SRC.Source 'Inquiry Source'
			FROM TC_Inquiries INQ 
			LEFT JOIN TC_ServiceRequests SR WITH(NOLOCK)ON SR.TC_InquiriesId=INQ.TC_InquiriesId 
			INNER JOIN TC_InquirySource SRC WITH(NOLOCK)ON INQ.SourceId=SRC.Id
			INNER JOIN TC_InquiryType INQT WITH(NOLOCK)ON INQ.InquiryType=INQT.TC_InquiryTypeId
			WHERE INQ.BranchId=@BranchId AND INQ.TC_InquiriesId=@TC_InquiryId AND INQ.InquiryType=5
		END
	ELSE IF(@InqType=6)--CallInquiry
		BEGIN
			SELECT 'CallInquiry'
		END
	ELSE IF(@InqType=7)--EmailInquiry
		BEGIN
			SELECT 'EmailInquiry'
		END
	ELSE IF(@InqType=8)--LoanInquiry
		BEGIN
			SELECT 'LoanInquiry'
		END
END

 